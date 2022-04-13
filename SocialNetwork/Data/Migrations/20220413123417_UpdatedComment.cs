using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.Data.Migrations
{
    public partial class UpdatedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");
        }
    }
}
