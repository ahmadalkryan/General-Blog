using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class approval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_articleApprovals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _articleId = table.Column<int>(type: "int", nullable: false),
                    _adminId = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__articleApprovals", x => x.ID);
                    table.ForeignKey(
                        name: "FK__articleApprovals_Articles__articleId",
                        column: x => x._articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__articleApprovals_User__adminId",
                        column: x => x._adminId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "_articleNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    articleId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__articleNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK__articleNotifications_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__articleNotifications_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.UpdateData(
                table: "_globalChat",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 10, 33, 47, 234, DateTimeKind.Local).AddTicks(9060));

            migrationBuilder.CreateIndex(
                name: "IX__articleApprovals__adminId",
                table: "_articleApprovals",
                column: "_adminId");

            migrationBuilder.CreateIndex(
                name: "IX__articleApprovals__articleId",
                table: "_articleApprovals",
                column: "_articleId");

            migrationBuilder.CreateIndex(
                name: "IX__articleNotifications_articleId",
                table: "_articleNotifications",
                column: "articleId");

            migrationBuilder.CreateIndex(
                name: "IX__articleNotifications_userId",
                table: "_articleNotifications",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_articleApprovals");

            migrationBuilder.DropTable(
                name: "_articleNotifications");

            migrationBuilder.UpdateData(
                table: "_globalChat",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 9, 26, 26, 291, DateTimeKind.Local).AddTicks(3478));
        }
    }
}
