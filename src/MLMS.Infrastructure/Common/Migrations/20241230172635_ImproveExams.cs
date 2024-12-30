using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLMS.Infrastructure.Common.Migrations
{
    /// <inheritdoc />
    public partial class ImproveExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_SectionPart_SectionPartId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_User_ApplicationUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_User_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamState_SectionPart_SectionPartId",
                table: "UserExamState");

            migrationBuilder.DropIndex(
                name: "IX_UserExamState_SectionPartId",
                table: "UserExamState");

            migrationBuilder.DropIndex(
                name: "IX_Question_SectionPartId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "SectionPartId",
                table: "UserExamState");

            migrationBuilder.DropColumn(
                name: "SectionPartId",
                table: "Question");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_ApplicationUserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_ApplicationUserId");

            migrationBuilder.AlterColumn<long>(
                name: "ExamId",
                table: "Question",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Choice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ExamSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckpointQuestionId = table.Column<long>(type: "bigint", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSession_Exam_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSession_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSessionQuestionChoice",
                columns: table => new
                {
                    ExamSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ChoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSessionQuestionChoice", x => new { x.ExamSessionId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ExamSessionQuestionChoice_Choice_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamSessionQuestionChoice_ExamSession_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalTable: "ExamSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSessionQuestionChoice_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSession_ExamId",
                table: "ExamSession",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSession_UserId",
                table: "ExamSession",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessionQuestionChoice_ChoiceId",
                table: "ExamSessionQuestionChoice",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessionQuestionChoice_QuestionId",
                table: "ExamSessionQuestionChoice",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_ApplicationUserId",
                table: "RefreshToken",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_ApplicationUserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UserId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "ExamSessionQuestionChoice");

            migrationBuilder.DropTable(
                name: "ExamSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Choice");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_ApplicationUserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_ApplicationUserId");

            migrationBuilder.AddColumn<long>(
                name: "SectionPartId",
                table: "UserExamState",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ExamId",
                table: "Question",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "SectionPartId",
                table: "Question",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamState_SectionPartId",
                table: "UserExamState",
                column: "SectionPartId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SectionPartId",
                table: "Question",
                column: "SectionPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_SectionPart_SectionPartId",
                table: "Question",
                column: "SectionPartId",
                principalTable: "SectionPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_User_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_User_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamState_SectionPart_SectionPartId",
                table: "UserExamState",
                column: "SectionPartId",
                principalTable: "SectionPart",
                principalColumn: "Id");
        }
    }
}
