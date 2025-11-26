using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMovimentacaoEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_OrdensDeCompra_OrdemDeCompraId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesEstoque_OrdemDeCompraId",
                table: "MovimentacoesEstoque");

            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cargo",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_OrdemDeCompraId",
                table: "MovimentacoesEstoque",
                column: "OrdemDeCompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_OrdensDeCompra_OrdemDeCompraId",
                table: "MovimentacoesEstoque",
                column: "OrdemDeCompraId",
                principalTable: "OrdensDeCompra",
                principalColumn: "Id");
        }
    }
}
