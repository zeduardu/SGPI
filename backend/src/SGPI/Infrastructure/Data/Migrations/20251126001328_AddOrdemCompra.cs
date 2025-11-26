using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdemCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensDeCompra_AspNetUsers_UsuarioAprovadorId",
                table: "OrdensDeCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdensDeCompra_AspNetUsers_UsuarioSolicitanteId",
                table: "OrdensDeCompra");

            migrationBuilder.DropIndex(
                name: "IX_OrdensDeCompra_UsuarioAprovadorId",
                table: "OrdensDeCompra");

            migrationBuilder.DropIndex(
                name: "IX_OrdensDeCompra_UsuarioSolicitanteId",
                table: "OrdensDeCompra");

            migrationBuilder.DropColumn(
                name: "DataAprovacao",
                table: "OrdensDeCompra");

            migrationBuilder.DropColumn(
                name: "Justificativa",
                table: "OrdensDeCompra");

            migrationBuilder.DropColumn(
                name: "UsuarioSolicitanteId",
                table: "OrdensDeCompra");

            migrationBuilder.RenameColumn(
                name: "UsuarioAprovadorId",
                table: "OrdensDeCompra",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "ValorUnitarioCotado",
                table: "ItensOrdensDeCompra",
                newName: "PrecoUnitario");

            migrationBuilder.AddColumn<int>(
                name: "FornecedorId",
                table: "OrdensDeCompra",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdensDeCompra_FornecedorId",
                table: "OrdensDeCompra",
                column: "FornecedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensDeCompra_Fornecedores_FornecedorId",
                table: "OrdensDeCompra",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensDeCompra_Fornecedores_FornecedorId",
                table: "OrdensDeCompra");

            migrationBuilder.DropIndex(
                name: "IX_OrdensDeCompra_FornecedorId",
                table: "OrdensDeCompra");

            migrationBuilder.DropColumn(
                name: "FornecedorId",
                table: "OrdensDeCompra");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "OrdensDeCompra",
                newName: "UsuarioAprovadorId");

            migrationBuilder.RenameColumn(
                name: "PrecoUnitario",
                table: "ItensOrdensDeCompra",
                newName: "ValorUnitarioCotado");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAprovacao",
                table: "OrdensDeCompra",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Justificativa",
                table: "OrdensDeCompra",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioSolicitanteId",
                table: "OrdensDeCompra",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensDeCompra_UsuarioAprovadorId",
                table: "OrdensDeCompra",
                column: "UsuarioAprovadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensDeCompra_UsuarioSolicitanteId",
                table: "OrdensDeCompra",
                column: "UsuarioSolicitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensDeCompra_AspNetUsers_UsuarioAprovadorId",
                table: "OrdensDeCompra",
                column: "UsuarioAprovadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensDeCompra_AspNetUsers_UsuarioSolicitanteId",
                table: "OrdensDeCompra",
                column: "UsuarioSolicitanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
