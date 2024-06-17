package main

import (
    "log"
    "os"

    "gorm.io/driver/sqlite"
    "gorm.io/driver/sqlserver"
    "gorm.io/gorm"
)

var db *gorm.DB

func init() {
    var err error
    if os.Getenv("ENV") == "production" {
        // Conexão com SQL Server em produção
        dsn := "sqlserver://username:password@localhost:1433?database=your_db"
        db, err = gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
    } else {
        // Conexão com SQLite em desenvolvimento
        db, err = gorm.Open(sqlite.Open("sqlite.db"), &gorm.Config{})
    }
    if err != nil {
        log.Fatalf("Erro ao conectar ao banco de dados: %v", err)
    }
    
    // Migrar a estrutura da tabela
    db.AutoMigrate(&User{})
}
