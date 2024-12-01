using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CDNAPI.Migrations
{
    public partial class AddingAgoraLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgoraLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MinhaCDNLogId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_AgoraLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgoraLogs_MinhaCDNLogs_MinhaCDNLogId",
                        column: x => x.MinhaCDNLogId,
                        principalTable: "MinhaCDNLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgoraLogs_MinhaCDNLogId",
                table: "AgoraLogs",
                column: "MinhaCDNLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgoraLogs");
        }
    }
}
