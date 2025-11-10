using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_articleId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_articleId",
                table: "Comments",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_articleId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_articleId",
                table: "Comments",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "ID");
        }
    }
}
