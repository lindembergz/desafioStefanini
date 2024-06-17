/*

curl -X POST http://localhost:8000/register -H "Content-Type: application/json" -d "{\"username\":\"user1\", \"password\":\"pass123\", \"age\":25}"

curl -X POST http://localhost:8000/login -H "Content-Type: application/json" -d "{\"username\":\"user1\", \"password\":\"pass123\"}"

curl -X GET http://localhost:8000/users -H "Authorization: Bearer YOUR_JWT_TOKEN"

curl -X PUT http://localhost:8000/users/1 -H "Authorization: Bearer curl -X PUT http://localhost:8000/users/1 -H "Authorization: Bearer " -H "Content-Type: application/json" -d "{\"username\":\"updatedUser\", \"password\":\"newPass123\", \"age\":30}"

curl -X DELETE http://localhost:8000/users/1 -H "Authorization: Bearer YOUR_JWT_TOKEN"
*/


package main

import (
    "context"
    "database/sql"
    "encoding/json"
    "fmt"
    "log"
    "net/http"
    "os"
    "strconv"
    "strings"
    "time"
    "github.com/mattn/go-sqlite3" // Driver SQLite
    "github.com/denisenkom/go-mssqldb" // Driver SQL Server
    "github.com/dgrijalva/jwt-go"
    "github.com/gorilla/mux"
    "golang.org/x/crypto/bcrypt"
)


// User representa um usuário
type User struct {
    ID       int    `json:"id"`
    Username string `json:"username"`
    Password string `json:"password"`
    Age      int    `json:"age"`
}

// Variáveis globais
var db *Database
var jwtKey = []byte("minha_chave_secreta")

// Função para inicializar a tabela de usuários
func (db *Database) InitUserTable() error {
    query := `
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        username TEXT NOT NULL UNIQUE,
        password TEXT NOT NULL,
        age INTEGER NOT NULL
    );`
    _, err := db.Exec(query)
    return err
}

func main() {
    var err error

    // Verifica se estamos em ambiente de produção ou desenvolvimento
    env := os.Getenv("ENVIRONMENT")
    if env == "production" {
        db, err = InitSQLServer()
    } else {
        db, err = InitSQLite()
    }
    if err != nil {
        log.Fatalf("Erro ao iniciar o banco de dados: %v", err)
    }
    defer db.Close()

    // Inicializa a tabela de usuários
    err = db.InitUserTable()
    if err != nil {
        log.Fatalf("Erro ao inicializar a tabela de usuários: %v", err)
    }

    // Inicializa o roteador
    router := mux.NewRouter()

    // Rotas públicas
    router.HandleFunc("/register", register).Methods("POST")
    router.HandleFunc("/login", login).Methods("POST")

    // Subroteador para rotas protegidas
    protected := router.PathPrefix("/").Subrouter()
    protected.HandleFunc("/users", getUsers).Methods("GET")
    protected.HandleFunc("/users/{id}", updateUser).Methods("PUT")
    protected.HandleFunc("/users/{id}", deleteUser).Methods("DELETE")
    protected.Use(jwtMiddleware)

    // Inicia o servidor
    fmt.Println("Servidor iniciado em http://localhost:8000")
    log.Fatal(http.ListenAndServe(":8000", router))
}

// Função para registrar um novo usuário
func register(w http.ResponseWriter, r *http.Request) {
    var newUser User
    err := json.NewDecoder(r.Body).Decode(&newUser)
    if err != nil {
        http.Error(w, "Dados inválidos", http.StatusBadRequest)
        return
    }

    // Valida se todos os campos foram fornecidos
    if newUser.Username == "" || newUser.Password == "" || newUser.Age <= 0 {
        http.Error(w, "Todos os campos são obrigatórios", http.StatusBadRequest)
        return
    }

    // Criptografa a senha antes de armazenar
    hashedPassword, err := bcrypt.GenerateFromPassword([]byte(newUser.Password), bcrypt.DefaultCost)
    if err != nil {
        http.Error(w, "Erro ao criptografar a senha", http.StatusInternalServerError)
        return
    }
    newUser.Password = string(hashedPassword)

    query := "INSERT INTO users (username, password, age) VALUES (?, ?, ?)"
    _, err = db.Exec(query, newUser.Username, newUser.Password, newUser.Age)
    if err != nil {
        http.Error(w, "Erro ao registrar usuário", http.StatusInternalServerError)
        return
    }

    w.WriteHeader(http.StatusCreated)
    json.NewEncoder(w).Encode(newUser)
}

// Função para fazer login e obter um token JWT
func login(w http.ResponseWriter, r *http.Request) {
    var credentials struct {
        Username string `json:"username"`
        Password string `json:"password"`
    }
    json.NewDecoder(r.Body).Decode(&credentials)

    query := "SELECT id, username, password, age FROM users WHERE username = ?"
    row := db.QueryRow(query, credentials.Username)

    var user User
    err := row.Scan(&user.ID, &user.Username, &user.Password, &user.Age)
    if err != nil {
        if err == sql.ErrNoRows {
            http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        } else {
            http.Error(w, "Erro ao procurar usuário", http.StatusInternalServerError)
        }
        return
    }

    err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(credentials.Password))
    if err != nil {
        http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        return
    }

    // Gera o token JWT
    token := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
        "username": user.Username,
        "exp":      time.Now().Add(time.Hour * 24).Unix(), // Token expira em 24 horas
    })
    tokenString, err := token.SignedString(jwtKey)
    if err != nil {
        http.Error(w, "Erro ao gerar o token JWT", http.StatusInternalServerError)
        return
    }

    json.NewEncoder(w).Encode(map[string]string{"token": tokenString})
}

// Função para obter todos os usuários (rota protegida)
func getUsers(w http.ResponseWriter, r *http.Request) {
    query := "SELECT id, username, age FROM users"
    rows, err := db.Query(query)
    if err != nil {
        http.Error(w, "Erro ao obter usuários", http.StatusInternalServerError)
        return
    }
    defer rows.Close()

    var users []User
    for rows.Next() {
        var user User
        err := rows.Scan(&user.ID, &user.Username, &user.Age)
        if err != nil {
            http.Error(w, "Erro ao escanear usuário", http.StatusInternalServerError)
            return
        }
        users = append(users, user)
    }

    json.NewEncoder(w).Encode(users)
}

// Função para atualizar um usuário (rota protegida)
func updateUser(w http.ResponseWriter, r *http.Request) {
    vars := mux.Vars(r)
    userID, err := strconv.Atoi(vars["id"])
    if err != nil {
        http.Error(w, "ID de usuário inválido", http.StatusBadRequest)
        return
    }

    var updatedUser User
    err = json.NewDecoder(r.Body).Decode(&updatedUser)
    if err != nil {
        http.Error(w, "Dados inválidos", http.StatusBadRequest)
        return
    }

    // Atualiza o usuário no banco de dados
    query := "UPDATE users SET username = ?, password = ?, age = ? WHERE id = ?"
    _, err = db.Exec(query, updatedUser.Username, updatedUser.Password, updatedUser.Age, userID)
    if err != nil {
        http.Error(w, "Erro ao atualizar usuário", http.StatusInternalServerError)
        return
    }

    json.NewEncoder(w).Encode(updatedUser)
}

// Função para excluir um usuário (rota protegida)
func deleteUser(w http.ResponseWriter, r *http.Request) {
    vars := mux.Vars(r)
    userID, err := strconv.Atoi(vars["id"])
    if err != nil {
        http.Error(w, "ID de usuário inválido", http.StatusBadRequest)
        return
    }

    query := "DELETE FROM users WHERE id = ?"
    _, err = db.Exec(query, userID)
    if err != nil {
        http.Error(w, "Erro ao excluir usuário", http.StatusInternalServerError)
        return
    }

    w.WriteHeader(http.StatusNoContent)
}

// Middleware para verificar o token JWT
func jwtMiddleware(next http.Handler) http.Handler {
    return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
        authHeader := r.Header.Get("Authorization")
        if authHeader == "" {
            http.Error(w, "Token JWT não fornecido", http.StatusUnauthorized)
            return
        }

        tokenString := strings.TrimPrefix(authHeader, "Bearer ")
        if tokenString == authHeader {
            http.Error(w, "Formato de token inválido", http.StatusUnauthorized)
            return
        }

        token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
            if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
                return nil, fmt.Errorf("Método de assinatura inválido")
            }
            return jwtKey, nil
        })

        if err != nil {
            http.Error(w, "Token JWT inválido", http.StatusUnauthorized)
            return
        }

        if claims, ok := token.Claims.(jwt.MapClaims); ok && token.Valid {
            // Adiciona o nome de usuário ao contexto da solicitação
            ctx := context.WithValue(r.Context(), "username", claims["username"])
            r = r.WithContext(ctx)
            next.ServeHTTP(w, r)
        } else {
            http.Error(w, "Token JWT inválido", http.StatusUnauthorized)
        }
    })
}
