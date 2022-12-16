using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.net.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_Bookmarks_BookmarkId",
                table: "Upvotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes");

            migrationBuilder.DropIndex(
                name: "IX_Upvotes_UserId",
                table: "Upvotes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Upvotes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Upvotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "Upvotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes",
                columns: new[] { "UserId", "BookmarkId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_Bookmarks_BookmarkId",
                table: "Upvotes",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_Bookmarks_BookmarkId",
                table: "Upvotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes");

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkId",
                table: "Upvotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Upvotes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Upvotes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Upvotes_UserId",
                table: "Upvotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_Bookmarks_BookmarkId",
                table: "Upvotes",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id");
        }
    }
}
