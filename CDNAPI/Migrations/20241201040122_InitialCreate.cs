using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CDNAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MinhaCDNLog = table.Column<string>(nullable: true),
                    AgoraLog = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    URL = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLogs");
        }
    }
}
