using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLMS.Infrastructure.Common.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceExamEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMajor");

            migrationBuilder.DropTable(
                name: "UserSectionPartExamState");

            migrationBuilder.DropColumn(
                name: "PassThresholdPoints",
                table: "SectionPart");

            migrationBuilder.AddColumn<long>(
                name: "ExamId",
                table: "SectionPart",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExamId",
                table: "Question",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Question",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Exam",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    PassThresholdPoints = table.Column<int>(type: "int", nullable: false),
                    SectionPartId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserExamState",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SectionPartId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExamState", x => new { x.UserId, x.ExamId });
                    table.ForeignKey(
                        name: "FK_UserExamState_Exam_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExamState_SectionPart_SectionPartId",
                        column: x => x.SectionPartId,
                        principalTable: "SectionPart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExamState_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionPart_ExamId",
                table: "SectionPart",
                column: "ExamId",
                unique: true,
                filter: "[ExamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ExamId",
                table: "Question",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ImageId",
                table: "Question",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamState_ExamId",
                table: "UserExamState",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamState_SectionPartId",
                table: "UserExamState",
                column: "SectionPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Exam_ExamId",
                table: "Question",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_File_ImageId",
                table: "Question",
                column: "ImageId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionPart_Exam_ExamId",
                table: "SectionPart",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Exam_ExamId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_File_ImageId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionPart_Exam_ExamId",
                table: "SectionPart");

            migrationBuilder.DropTable(
                name: "UserExamState");

            migrationBuilder.DropTable(
                name: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_SectionPart_ExamId",
                table: "SectionPart");

            migrationBuilder.DropIndex(
                name: "IX_Question_ExamId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ImageId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "SectionPart");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "PassThresholdPoints",
                table: "SectionPart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseMajor",
                columns: table => new
                {
                    AssignedMajorsId = table.Column<int>(type: "int", nullable: false),
                    CoursesAssignedToId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMajor", x => new { x.AssignedMajorsId, x.CoursesAssignedToId });
                    table.ForeignKey(
                        name: "FK_CourseMajor_Course_CoursesAssignedToId",
                        column: x => x.CoursesAssignedToId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMajor_Major_AssignedMajorsId",
                        column: x => x.AssignedMajorsId,
                        principalTable: "Major",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSectionPartExamState",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SectionPartId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSectionPartExamState", x => new { x.UserId, x.SectionPartId });
                    table.ForeignKey(
                        name: "FK_UserSectionPartExamState_SectionPart_SectionPartId",
                        column: x => x.SectionPartId,
                        principalTable: "SectionPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSectionPartExamState_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMajor_CoursesAssignedToId",
                table: "CourseMajor",
                column: "CoursesAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSectionPartExamState_SectionPartId",
                table: "UserSectionPartExamState",
                column: "SectionPartId");
        }
    }
}
