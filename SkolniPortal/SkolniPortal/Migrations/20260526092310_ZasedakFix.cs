using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolniPortal.Migrations
{
    /// <inheritdoc />
    public partial class ZasedakFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zasedaky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pocetMist = table.Column<int>(type: "int", nullable: false),
                    forTrida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mista = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zasedaky", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zasedaky");
        }
    }
}
