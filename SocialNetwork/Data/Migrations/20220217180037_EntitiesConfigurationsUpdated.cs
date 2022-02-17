using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.Data.Migrations
{
    public partial class EntitiesConfigurationsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_TargetProfileId1",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_TargetProfileId1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TargetProfileId1",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "TargetProfileId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TargetProfileId",
                table: "Events",
                column: "TargetProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_TargetProfileId",
                table: "Events",
                column: "TargetProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_TargetProfileId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_TargetProfileId",
                table: "Events");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetProfileId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetProfileId1",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_TargetProfileId1",
                table: "Events",
                column: "TargetProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_TargetProfileId1",
                table: "Events",
                column: "TargetProfileId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
