using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanupUnnecessarySectionPartIdInExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionPartId",
                table: "Exam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SectionPartId",
                table: "Exam",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
