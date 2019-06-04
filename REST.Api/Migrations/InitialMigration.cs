using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace REST.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeregnungsDatens",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    StartDatum = table.Column<DateTimeOffset>(nullable: false),
                    StartUhrzeit = table.Column<DateTime>(nullable: false),
                    EndDatum = table.Column<DateTimeOffset>(nullable: false),
                    Betrieb = table.Column<string>(maxLength: 50, nullable: false),
                    SchlagId = table.Column<Guid>(nullable: false),
                    Duese = table.Column<string>(maxLength: 50, nullable: false),
                    WasseruhrAnfang = table.Column<int>(maxLength: 50, nullable: false),
                    WasseruhrEnde = table.Column<int>(maxLength: 50, nullable: false),
                    Vorkomnisse = table.Column<string>(maxLength: 50, nullable: false),
                    IstAbgeschlossen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeregnungsDaten", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "Schlaege",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_Schlag", x => x.ID);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Schlag_BeregnungsDatenId",
                table: "BeregnungsDaten",
                column: "SchlagId");
        }
        protected override void Down( MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BeregnungsDaten");
            migrationBuilder.DropTable(name: "Schlag");
        }
    }
}
