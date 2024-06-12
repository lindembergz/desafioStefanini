using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questao5.Migrations
{
    /// <inheritdoc />
    public partial class Primeiramigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contacorrente",
                columns: table => new
                {
                    IdContaCorrente = table.Column<Guid>(type: "TEXT", maxLength: 37, nullable: false),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacorrente", x => x.IdContaCorrente);
                });

            migrationBuilder.CreateTable(
                name: "Movimento",
                columns: table => new
                {
                    IdMovimento = table.Column<Guid>(type: "TEXT", maxLength: 37, nullable: false),
                    IdContaCorrente = table.Column<Guid>(type: "TEXT", maxLength: 37, nullable: false),
                    DataMovimento = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    TipoMovimento = table.Column<int>(type: "INTEGER", maxLength: 1, nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimento", x => x.IdMovimento);
                    table.ForeignKey(
                        name: "FK_Movimento_contacorrente_IdContaCorrente",
                        column: x => x.IdContaCorrente,
                        principalTable: "contacorrente",
                        principalColumn: "IdContaCorrente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimento_IdContaCorrente",
                table: "Movimento",
                column: "IdContaCorrente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimento");

            migrationBuilder.DropTable(
                name: "contacorrente");
        }
    }
}
