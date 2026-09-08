using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySys.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Departments");
        }
    }
}
