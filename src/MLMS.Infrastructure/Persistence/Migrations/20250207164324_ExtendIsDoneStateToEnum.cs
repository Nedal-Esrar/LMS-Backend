using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendIsDoneStateToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSectionPartDone");

            migrationBuilder.CreateTable(
                name: "UserSectionPart",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SectionPartId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSectionPart", x => new { x.UserId, x.SectionPartId });
                    table.ForeignKey(
                        name: "FK_UserSectionPart_SectionPart_SectionPartId",
                        column: x => x.SectionPartId,
                        principalTable: "SectionPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSectionPart_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSectionPart_SectionPartId",
                table: "UserSectionPart",
                column: "SectionPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSectionPart");

            migrationBuilder.CreateTable(
                name: "UserSectionPartDone",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SectionPartId = table.Column<long>(type: "bigint", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSectionPartDone", x => new { x.UserId, x.SectionPartId });
                    table.ForeignKey(
                        name: "FK_UserSectionPartDone_SectionPart_SectionPartId",
                        column: x => x.SectionPartId,
                        principalTable: "SectionPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSectionPartDone_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSectionPartDone_SectionPartId",
                table: "UserSectionPartDone",
                column: "SectionPartId");
        }
    }
}
