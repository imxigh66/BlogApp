using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "UserIdentifier",
                table: "Likes",
                newName: "ClientIpAddress");

            migrationBuilder.RenameColumn(
                name: "LikedAt",
                table: "Likes",
                newName: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Likes",
                newName: "LikedAt");

            migrationBuilder.RenameColumn(
                name: "ClientIpAddress",
                table: "Likes",
                newName: "UserIdentifier");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
