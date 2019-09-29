using System;
using Blog.FK.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.FK.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TFKBlogPost",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title_BlogPost = table.Column<string>(type: "NVARCHAR(256)", nullable: false),
                    ShortDescription_BlogPost = table.Column<string>(type: "NVARCHAR(2048)", nullable: false),
                    CreatedAt_BlogPost = table.Column<DateTime>(type: "Datetime", nullable: false),
                    UpdatedAt_BlogPost = table.Column<DateTime>(type: "Datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFKBlogPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TFKUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name_User = table.Column<string>(type: "NVARCHAR(256)", nullable: false),
                    Email_User = table.Column<string>(type: "NVARCHAR(256)", nullable: false),
                    Password_User = table.Column<string>(type: "NVARCHAR(2048)", nullable: false),
                    CreatedAt_User = table.Column<DateTime>(type: "Datetime", nullable: false),
                    UpdatedAt_User = table.Column<DateTime>(type: "Datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFKUser", x => x.Id);
                });

            //initial admin user seed
            migrationBuilder.InsertData(
                table: "TFKUser",
                columns: new[] { "Id", "Name_User", "Email_User", "Password_User", "CreatedAt_User", "UpdatedAt_User" },
                values: new object[] { Guid.NewGuid(), "Felipe Somogyi", "leaosomogyi@hotmail.com", "12345678".Cript(), DateTime.Now, DateTime.Now });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TFKBlogPost");

            migrationBuilder.DropTable(
                name: "TFKUser");
        }
    }
}
