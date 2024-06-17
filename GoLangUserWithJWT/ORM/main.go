package main

import (
    "encoding/json"
    "fmt"
    "log"
    "net/http"
    "strconv"
    "strings"
    "time"

    "github.com/dgrijalva/jwt-go"
    "github.com/gorilla/mux"
    "golang.org/x/crypto/bcrypt"
)

var jwtKey = []byte("minha_chave_secreta")

func main() {
    router := mux.NewRouter()

    router.HandleFunc("/register", register).Methods("POST")
    router.HandleFunc("/login", login).Methods("POST")

    protected := router.PathPrefix("/").Subrouter()
    protected.Use(jwtMiddleware)
    protected.HandleFunc("/users", getUsers).Methods("GET")
    protected.HandleFunc("/users/{id}", updateUser).Methods("PUT")
    protected.HandleFunc("/users/{id}", patchUser).Methods("PATCH") // Usando PATCH para atualizações parciais
    protected.HandleFunc("/users/{id}", deleteUser).Methods("DELETE")

    fmt.Println("Servidor iniciado em http://localhost:8000")
    log.Fatal(http.ListenAndServe(":8000", router))
}

func register(w http.ResponseWriter, r *http.Request) {
    var newUser User
    json.NewDecoder(r.Body).Decode(&newUser)

    hashedPassword, err := bcrypt.GenerateFromPassword([]byte(newUser.Password), bcrypt.DefaultCost)
    if err != nil {
        http.Error(w, "Erro ao criptografar a senha", http.StatusInternalServerError)
        return
    }
    newUser.Password = string(hashedPassword)

    result := db.Create(&newUser)
    if result.Error != nil {
        http.Error(w, "Erro ao registrar usuário", http.StatusInternalServerError)
        return
    }
    json.NewEncoder(w).Encode(newUser)
}

func login(w http.ResponseWriter, r *http.Request) {
    var credentials struct {
        Username string `json:"username"`
        Password string `json:"password"`
    }
    json.NewDecoder(r.Body).Decode(&credentials)

    var user User
    result := db.Where("username = ?", credentials.Username).First(&user)
    if result.Error != nil {
        http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        return
    }

    err := bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(credentials.Password))
    if err != nil {
        http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        return
    }

    token := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
        "username": user.Username,
        "exp":      time.Now().Add(time.Hour * 24).Unix(),
    })
    tokenString, err := token.SignedString(jwtKey)
    if err != nil {
        http.Error(w, "Erro ao gerar o token JWT", http.StatusInternalServerError)
        return
    }
    json.NewEncoder(w).Encode(map[string]string{"token": tokenString})
}

func getUsers(w http.ResponseWriter, r *http.Request) {
    var users []User
    db.Find(&users)
    json.NewEncoder(w).Encode(users)
}

func updateUser(w http.ResponseWriter, r *http.Request) {
    id, _ := strconv.Atoi(mux.Vars(r)["id"])
    var user User
    if err := db.First(&user, id).Error; err != nil {
        http.Error(w, "Usuário não encontrado", http.StatusNotFound)
        return
    }

    json.NewDecoder(r.Body).Decode(&user)
    db.Save(&user)
    json.NewEncoder(w).Encode(user)
}

func deleteUser(w http.ResponseWriter, r *http.Request) {
    id, _ := strconv.Atoi(mux.Vars(r)["id"])
    if err := db.Delete(&User{}, id).Error; err != nil {
        http.Error(w, "Usuário não encontrado", http.StatusNotFound)
        return
    }
    w.WriteHeader(http.StatusNoContent)
}

// Função para atualizar parcialmente um usuário (rota protegida)
func patchUser(w http.ResponseWriter, r *http.Request) {
    params := mux.Vars(r)
    id, err := strconv.Atoi(params["id"])
    if err != nil {
        http.Error(w, "ID inválido", http.StatusBadRequest)
        return
    }

    var user User
    result := db.First(&user, id)
    if result.Error != nil {
        http.Error(w, "Usuário não encontrado", http.StatusNotFound)
        return
    }

    var updatedUser map[string]interface{}
    if err := json.NewDecoder(r.Body).Decode(&updatedUser); err != nil {
        http.Error(w, "Dados inválidos", http.StatusBadRequest)
        return
    }

    if username, ok := updatedUser["username"].(string); ok && username != "" {
        user.Username = username
    }
    if password, ok := updatedUser["password"].(string); ok && password != "" {
        hashedPassword, _ := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
        user.Password = string(hashedPassword)
    }
    if age, ok := updatedUser["age"].(float64); ok && age != 0 {
        user.Age = int(age)
    }

    db.Save(&user)
    json.NewEncoder(w).Encode(user)
}


func jwtMiddleware(next http.Handler) http.Handler {
    return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
        authHeader := r.Header.Get("Authorization")
        if authHeader == "" {
            http.Error(w, "Token JWT não fornecido", http.StatusUnauthorized)
            return
        }

        tokenString := strings.Replace(authHeader, "Bearer ", "", 1)
        claims := &jwt.MapClaims{}
        token, err := jwt.ParseWithClaims(tokenString, claims, func(token *jwt.Token) (interface{}, error) {
            return jwtKey, nil
        })
        if err != nil || !token.Valid {
            http.Error(w, "Token JWT inválido", http.StatusUnauthorized)
            return
        }

        next.ServeHTTP(w, r)
    })
}


