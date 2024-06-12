package main

import (
    "database/sql"
    "fmt"
    "log"
    "os"

    _ "github.com/mattn/go-sqlite3" // Driver SQLite
    _ "github.com/denisenkom/go-mssqldb" // Driver SQL Server
)

// Database representa a conexão com o banco de dados
type Database struct {
    *sql.DB
}

// InitSQLite inicia uma conexão com o banco de dados SQLite
func InitSQLite() (*Database, error) {
    dbPath := "sqlite.db" // Caminho do arquivo SQLite
    db, err := sql.Open("sqlite3", dbPath)
    if err != nil {
        return nil, err
    }
    return &Database{db}, nil
}

// InitSQLServer inicia uma conexão com o banco de dados SQL Server
func InitSQLServer() (*Database, error) {
    dsn := "sqlserver://user:password@localhost/database" // DSN do SQL Server
    db, err := sql.Open("sqlserver", dsn)
    if err != nil {
        return nil, err
    }
    return &Database{db}, nil
}

func mainsqldb() {
    var db *Database
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

    // Teste de conexão
    err = db.Ping()
    if err != nil {
        log.Fatalf("Erro ao conectar ao banco de dados: %v", err)
    }
    fmt.Println("Conexão com o banco de dados estabelecida com sucesso!")

    // Restante do seu código...
}
