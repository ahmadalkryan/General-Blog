using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class بهقسف : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_globalChat",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__globalChat", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "_golbalMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senderId = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__golbalMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK__golbalMessages_User_senderId",
                        column: x => x.senderId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(Max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Articles_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "_notification",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageNotification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    receiverId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK__notification_User_receiverId",
                        column: x => x.receiverId,
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__notification__golbalMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "_golbalMessages",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ArticleQuestions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(Max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(Max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    articleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleQuestions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleQuestions_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleQuestions_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ArticleSummaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(Max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    articleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSummaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleSummaries_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    articleId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    articleId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_likes_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "_globalChat",
                columns: new[] { "ID", "CreatedAt", "Description", "Name" },
                values: new object[] { 2, new DateTime(2025, 12, 2, 9, 26, 26, 291, DateTimeKind.Local).AddTicks(3478), "المحادثة المفتوحة لجميع المستخدمين", " GolbalChat For system" });

            migrationBuilder.CreateIndex(
                name: "IX__golbalMessages_senderId",
                table: "_golbalMessages",
                column: "senderId");

            migrationBuilder.CreateIndex(
                name: "IX__notification_IsRead",
                table: "_notification",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX__notification_MessageId",
                table: "_notification",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX__notification_receiverId",
                table: "_notification",
                column: "receiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleQuestions_articleId",
                table: "ArticleQuestions",
                column: "articleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleQuestions_userId",
                table: "ArticleQuestions",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_categoryId",
                table: "Articles",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_userId",
                table: "Articles",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSummaries_articleId",
                table: "ArticleSummaries",
                column: "articleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_articleId",
                table: "Comments",
                column: "articleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_userId",
                table: "Comments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_articleId",
                table: "likes",
                column: "articleId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_userId",
                table: "likes",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_userId",
                table: "UserProfile",
                column: "userId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_globalChat");

            migrationBuilder.DropTable(
                name: "_notification");

            migrationBuilder.DropTable(
                name: "ArticleQuestions");

            migrationBuilder.DropTable(
                name: "ArticleSummaries");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "_golbalMessages");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
