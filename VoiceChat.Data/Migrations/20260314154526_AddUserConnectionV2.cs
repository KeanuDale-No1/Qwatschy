using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConnectionV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ConnectedUsers_ConnectedUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ConnectedUsers");

            migrationBuilder.RenameColumn(
                name: "ConnectedUserId",
                table: "Users",
                newName: "ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ConnectedUserId",
                table: "Users",
                newName: "IX_Users_ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Channels_ChannelId",
                table: "Users",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Channels_ChannelId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "Users",
                newName: "ConnectedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ChannelId",
                table: "Users",
                newName: "IX_Users_ConnectedUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ConnectedUsers_ConnectedUserId",
                table: "Users",
                column: "ConnectedUserId",
                principalTable: "ConnectedUsers",
                principalColumn: "Id");
        }
    }
}
