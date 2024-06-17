package main

import (
    "gorm.io/gorm"
    "time"
    "github.com/dgrijalva/jwt-go"
)

type User struct {
    gorm.Model
    Username string `gorm:"unique"`
    Password string
    Age      int
}

type RefreshToken struct {
    ID        uint `gorm:"primaryKey"`
    Token     string
    UserID    uint
    ExpiresAt time.Time
}


type Claims struct {
    Username string `json:"username"`
    jwt.StandardClaims
}

type Credentials struct {
    Username string `json:"username"`
    Password string `json:"password"`
}