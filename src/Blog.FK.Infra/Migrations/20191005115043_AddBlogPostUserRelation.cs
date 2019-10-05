using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.FK.Infra.Migrations
{
    public partial class AddBlogPostUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "User_Id",
                table: "TFKBlogPost",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TFKBlogPost_User_Id",
                table: "TFKBlogPost",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TFKBlogPost_TFKUser_User_Id",
                table: "TFKBlogPost",
                column: "User_Id",
                principalTable: "TFKUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TFKBlogPost_TFKUser_User_Id",
                table: "TFKBlogPost");

            migrationBuilder.DropIndex(
                name: "IX_TFKBlogPost_User_Id",
                table: "TFKBlogPost");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "TFKBlogPost");
        }
    }
}
