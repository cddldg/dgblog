using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DG_Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ImgUrl = table.Column<string>(maxLength: 200, nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_ChickenSoups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_ChickenSoups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Friendlinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 20, nullable: false),
                    LinkUrl = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Friendlinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_HotNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Url = table.Column<string>(maxLength: 250, nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_HotNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AlbumId = table.Column<Guid>(nullable: false),
                    ImgUrl = table.Column<string>(maxLength: 200, nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Post_Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Post_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Author = table.Column<string>(maxLength: 10, nullable: true),
                    Url = table.Column<string>(maxLength: 100, nullable: false),
                    Html = table.Column<string>(nullable: true),
                    Markdown = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Signatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Type = table.Column<string>(maxLength: 20, nullable: false),
                    Url = table.Column<string>(maxLength: 100, nullable: false),
                    Ip = table.Column<string>(maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Signatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagName = table.Column<string>(maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DG_Wallpapers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DG_Wallpapers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DG_Albums");

            migrationBuilder.DropTable(
                name: "DG_Categories");

            migrationBuilder.DropTable(
                name: "DG_ChickenSoups");

            migrationBuilder.DropTable(
                name: "DG_Friendlinks");

            migrationBuilder.DropTable(
                name: "DG_HotNews");

            migrationBuilder.DropTable(
                name: "DG_Images");

            migrationBuilder.DropTable(
                name: "DG_Post_Tags");

            migrationBuilder.DropTable(
                name: "DG_Posts");

            migrationBuilder.DropTable(
                name: "DG_Signatures");

            migrationBuilder.DropTable(
                name: "DG_Tags");

            migrationBuilder.DropTable(
                name: "DG_Wallpapers");
        }
    }
}
