using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Polls_PollId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswer_Answers_AnswerId",
                table: "VoteAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswer_Questions_QuestionId",
                table: "VoteAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswer_Vote_VoteId",
                table: "VoteAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteAnswer",
                table: "VoteAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.RenameTable(
                name: "VoteAnswer",
                newName: "VoteAnswers");

            migrationBuilder.RenameTable(
                name: "Vote",
                newName: "Votes");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswer_VoteId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_VoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswer_QuestionId_VoteId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_QuestionId_VoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswer_AnswerId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_UserId_PollId",
                table: "Votes",
                newName: "IX_Votes_UserId_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_PollId",
                table: "Votes",
                newName: "IX_Votes_PollId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Answers_AnswerId",
                table: "VoteAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Questions_QuestionId",
                table: "VoteAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Votes_VoteId",
                table: "VoteAnswers",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Polls_PollId",
                table: "Votes",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Answers_AnswerId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Questions_QuestionId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Votes_VoteId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Polls_PollId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "Vote");

            migrationBuilder.RenameTable(
                name: "VoteAnswers",
                newName: "VoteAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId_PollId",
                table: "Vote",
                newName: "IX_Vote_UserId_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_PollId",
                table: "Vote",
                newName: "IX_Vote_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_VoteId",
                table: "VoteAnswer",
                newName: "IX_VoteAnswer_VoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_QuestionId_VoteId",
                table: "VoteAnswer",
                newName: "IX_VoteAnswer_QuestionId_VoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_AnswerId",
                table: "VoteAnswer",
                newName: "IX_VoteAnswer_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteAnswer",
                table: "VoteAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Polls_PollId",
                table: "Vote",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswer_Answers_AnswerId",
                table: "VoteAnswer",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswer_Questions_QuestionId",
                table: "VoteAnswer",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswer_Vote_VoteId",
                table: "VoteAnswer",
                column: "VoteId",
                principalTable: "Vote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
