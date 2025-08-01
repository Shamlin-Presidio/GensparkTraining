using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class Initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishedDate",
                table: "News",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ContactUs",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "ContactMessageId",
                table: "ContactUs",
                newName: "ContactUsId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "News",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "ContactUs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_News_UserId",
                table: "News",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Users_UserId",
                table: "News",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Users_UserId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_UserId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "News");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "News",
                newName: "PublishedDate");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "ContactUs",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "ContactUsId",
                table: "ContactUs",
                newName: "ContactMessageId");
        }
    }
}
