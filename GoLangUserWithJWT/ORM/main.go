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
    "github.com/google/uuid"
    "golang.org/x/crypto/bcrypt"
)


var jwtKey = []byte("minha_chave_secreta")

func main() {
    router := mux.NewRouter()

    router.HandleFunc("/register", register).Methods("POST")
    router.HandleFunc("/login", login).Methods("POST")
    router.HandleFunc("/refresh", refreshToken).Methods("POST") // rota para refresh token

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
    if err := json.NewDecoder(r.Body).Decode(&newUser); err != nil {
        log.Printf("Erro ao decodificar o novo usuário: %v", err)
        http.Error(w, "Dados de entrada inválidos", http.StatusBadRequest)
        return
    }

    hashedPassword, err := bcrypt.GenerateFromPassword([]byte(newUser.Password), bcrypt.DefaultCost)
    if err != nil {
        log.Printf("Erro ao criptografar a senha: %v", err)
        http.Error(w, "Erro ao criptografar a senha", http.StatusInternalServerError)
        return
    }
    newUser.Password = string(hashedPassword)

    result := db.Create(&newUser)
    if result.Error != nil {
        log.Printf("Erro ao registrar usuário: %v", result.Error)
        http.Error(w, "Erro ao registrar usuário", http.StatusInternalServerError)
        return
    }
    log.Printf("Usuário registrado com sucesso: %s", newUser.Username)
    json.NewEncoder(w).Encode(newUser)
}

func generateJWT(username string) (string, error) {
    expirationTime := time.Now().Add(24 * time.Hour)
    claims := &Claims{
        Username: username,
        StandardClaims: jwt.StandardClaims{
            ExpiresAt: expirationTime.Unix(),
        },
    }

    token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
    tokenString, err := token.SignedString(jwtKey)
    if err != nil {
        return "", err
    }

    return tokenString, nil
}

func login(w http.ResponseWriter, r *http.Request) {
    var credentials struct {
        Username string `json:"username"`
        Password string `json:"password"`
    }
    if err := json.NewDecoder(r.Body).Decode(&credentials); err != nil {
        log.Printf("Erro ao decodificar as credenciais: %v", err)
        http.Error(w, "Dados de entrada inválidos", http.StatusBadRequest)
        return
    }

    var user User
    result := db.Where("username = ?", credentials.Username).First(&user)
    if result.Error != nil {
        log.Printf("Usuário não encontrado: %v", result.Error)
        http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        return
    }

    err := bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(credentials.Password))
    if err != nil {
        log.Printf("Senha inválida: %v", err)
        http.Error(w, "Nome de usuário ou senha inválidos", http.StatusUnauthorized)
        return
    }

    accessToken, err := generateJWT(user.Username)
    if err != nil {
        log.Printf("Erro ao gerar o token JWT: %v", err)
        http.Error(w, "Erro ao gerar o token JWT", http.StatusInternalServerError)
        return
    }

    refreshToken, err := generateRefreshToken(user.ID)
    if err != nil {
        log.Printf("Erro ao gerar o refresh token: %v", err)
        http.Error(w, "Erro ao gerar o refresh token", http.StatusInternalServerError)
        return
    }

    log.Printf("Login bem-sucedido para usuário: %s", user.Username)
    json.NewEncoder(w).Encode(map[string]string{
        "access_token":  accessToken,
        "refresh_token": refreshToken,
    })
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


func generateRefreshToken(userID uint) (string, error) {
    refreshToken := uuid.NewString()
    expiresAt := time.Now().Add(7 * 24 * time.Hour) // 1 week

    refresh := RefreshToken{
        Token:     refreshToken,
        UserID:    userID,
        ExpiresAt: expiresAt,
    }
    if err := db.Create(&refresh).Error; err != nil {
        return "", err
    }
    return refreshToken, nil
}

func validateRefreshToken(token string) (*User, error) {
    var refreshToken RefreshToken
    if err := db.Where("token = ?", token).First(&refreshToken).Error; err != nil {
        return nil, err
    }

    if refreshToken.ExpiresAt.Before(time.Now()) {
        return nil, fmt.Errorf("refresh token expired")
    }

    var user User
    if err := db.First(&user, refreshToken.UserID).Error; err != nil {
        return nil, err
    }
    return &user, nil
}



func refreshToken(w http.ResponseWriter, r *http.Request) {
    var request struct {
        Token string `json:"token"`
    }
    json.NewDecoder(r.Body).Decode(&request)

    user, err := validateRefreshToken(request.Token)
    if err != nil {
        http.Error(w, "Invalid refresh token", http.StatusUnauthorized)
        return
    }

    accessToken, err := generateJWT(user.Username)
    if err != nil {
        http.Error(w, "Could not generate access token", http.StatusInternalServerError)
        return
    }

    newRefreshToken, err := generateRefreshToken(user.ID)
    if err != nil {
        http.Error(w, "Could not generate refresh token", http.StatusInternalServerError)
        return
    }

    json.NewEncoder(w).Encode(map[string]string{
        "access_token":  accessToken,
        "refresh_token": newRefreshToken,
    })
}

