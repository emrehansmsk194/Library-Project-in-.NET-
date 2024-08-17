using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookS",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookS_UserId",
                table: "BookS",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookS_AspNetUsers_UserId",
                table: "BookS",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookS_AspNetUsers_UserId",
                table: "BookS");

            migrationBuilder.DropIndex(
                name: "IX_BookS_UserId",
                table: "BookS");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookS");
        }
    }
}
