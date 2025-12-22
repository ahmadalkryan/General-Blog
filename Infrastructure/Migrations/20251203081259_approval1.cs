using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class approval1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX__articleApprovals__articleId",
                table: "_articleApprovals");

            migrationBuilder.UpdateData(
                table: "_globalChat",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 11, 12, 58, 507, DateTimeKind.Local).AddTicks(72));

            migrationBuilder.CreateIndex(
                name: "IX__articleApprovals__articleId",
                table: "_articleApprovals",
                column: "_articleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX__articleApprovals__articleId",
                table: "_articleApprovals");

            migrationBuilder.UpdateData(
                table: "_globalChat",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 3, 10, 33, 47, 234, DateTimeKind.Local).AddTicks(9060));

            migrationBuilder.CreateIndex(
                name: "IX__articleApprovals__articleId",
                table: "_articleApprovals",
                column: "_articleId");
        }
    }
}
