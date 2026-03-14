using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConnectedChannel",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectedUserId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConnectedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ConnectedUserId",
                table: "Users",
                column: "ConnectedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ConnectedUsers_ConnectedUserId",
                table: "Users",
                column: "ConnectedUserId",
                principalTable: "ConnectedUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ConnectedUsers_ConnectedUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ConnectedUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_ConnectedUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConnectedChannel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConnectedUserId",
                table: "Users");
        }
    }
}
