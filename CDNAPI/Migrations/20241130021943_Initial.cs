using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CDNAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinhaCDNLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    clientId = table.Column<string>(nullable: true),
                    provider = table.Column<string>(nullable: true),
                    httpmethod = table.Column<string>(nullable: true),
                    statuscode = table.Column<string>(nullable: true),
                    uripath = table.Column<string>(nullable: true),
                    cachestatus = table.Column<string>(nullable: true),
                    timetaken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinhaCDNLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinhaCDNLogs");
        }
    }
}
