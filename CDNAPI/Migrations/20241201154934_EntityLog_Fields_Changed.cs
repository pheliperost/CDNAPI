using Microsoft.EntityFrameworkCore.Migrations;

namespace CDNAPI.Migrations
{
    public partial class EntityLog_Fields_Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutputFormat",
                table: "EntityLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputFormat",
                table: "EntityLogs");
        }
    }
}
