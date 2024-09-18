using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consumer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Protocolos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroProtocolo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroVia = table.Column<int>(type: "int", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeMae = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomePai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocolos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Protocolos_Cpf_Rg_NumeroVia",
                table: "Protocolos",
                columns: new[] { "Cpf", "Rg", "NumeroVia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Protocolos_NumeroProtocolo",
                table: "Protocolos",
                column: "NumeroProtocolo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Protocolos");
        }
    }
}
