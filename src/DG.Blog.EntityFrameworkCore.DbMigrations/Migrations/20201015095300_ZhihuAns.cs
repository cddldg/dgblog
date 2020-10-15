using Microsoft.EntityFrameworkCore.Migrations;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.Migrations
{
    public partial class ZhihuAns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Answer1CardCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer1CommentCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer1VoteupCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer2CardCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer2CommentCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer2VoteupCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer3CardCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer3CommentCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer3VoteupCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer4CardCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer4CommentCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer4VoteupCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer5CardCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer5CommentCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Answer5VoteupCount",
                table: "DG_ZhQuestion",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer1CardCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer1CommentCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer1VoteupCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer2CardCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer2CommentCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer2VoteupCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer3CardCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer3CommentCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer3VoteupCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer4CardCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer4CommentCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer4VoteupCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer5CardCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer5CommentCount",
                table: "DG_ZhQuestion");

            migrationBuilder.DropColumn(
                name: "Answer5VoteupCount",
                table: "DG_ZhQuestion");
        }
    }
}
