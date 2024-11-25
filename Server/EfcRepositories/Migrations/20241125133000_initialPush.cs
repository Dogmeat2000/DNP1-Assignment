using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcRepositories.Migrations
{
    /// <inheritdoc />
    public partial class initialPush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_id);
                });

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    Forum_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title_txt = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp_created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_deleted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPost_id = table.Column<int>(type: "INTEGER", nullable: false),
                    LastCommentPost_id = table.Column<int>(type: "INTEGER", nullable: false),
                    LastComment_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Author_id = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentForum_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.Forum_id);
                    table.ForeignKey(
                        name: "FK_Forums_Forums_ParentForum_id",
                        column: x => x.ParentForum_id,
                        principalTable: "Forums",
                        principalColumn: "Forum_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forums_Users_Author_id",
                        column: x => x.Author_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Profile_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    User_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Profile_id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Post_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title_txt = table.Column<string>(type: "TEXT", nullable: false),
                    Body_txt = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp_created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_deleted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ParentForum_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Author_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Post_id);
                    table.ForeignKey(
                        name: "FK_Posts_Forums_ParentForum_id",
                        column: x => x.ParentForum_id,
                        principalTable: "Forums",
                        principalColumn: "Forum_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_Author_id",
                        column: x => x.Author_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Comment_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Body_txt = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp_created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp_deleted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ParentPost_id = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentForum_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Author_Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Comment_id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_ParentPost_id",
                        column: x => x.ParentPost_id,
                        principalTable: "Posts",
                        principalColumn: "Post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_Author_Id",
                        column: x => x.Author_Id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Author_Id",
                table: "Comments",
                column: "Author_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentPost_id",
                table: "Comments",
                column: "ParentPost_id");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_Author_id",
                table: "Forums",
                column: "Author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_ParentForum_id",
                table: "Forums",
                column: "ParentForum_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Author_id",
                table: "Posts",
                column: "Author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ParentForum_id",
                table: "Posts",
                column: "ParentForum_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_User_id",
                table: "UserProfiles",
                column: "User_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
