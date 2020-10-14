using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.Migrations
{
    public partial class Zhihu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DG_ZhQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FollowerCount = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    GoodQuestionCount = table.Column<int>(nullable: false),
                    AnswerTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    FistTime = table.Column<bool>(nullable: false),
                    Subscribes = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateMonitorTime = table.Column<DateTime>(nullable: false),
                    MonitorUpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_ZhQuestion", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DG_ZhQuestion");
        }
    }
}
