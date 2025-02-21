using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Course");
        }
    }
}
