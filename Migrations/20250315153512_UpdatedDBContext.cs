using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirstEFAPI.Migrations
{
    public partial class UpdatedDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Students");
        }
    }
}
