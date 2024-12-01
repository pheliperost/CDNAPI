using Microsoft.EntityFrameworkCore.Migrations;

namespace CDNAPI.Migrations
{
    public partial class EntityLog_OutputFormat_field_removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputFormat",
                table: "EntityLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutputFormat",
                table: "EntityLogs",
                nullable: true);
        }
    }
}
