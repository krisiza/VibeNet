using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeNet.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class like : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Posts_PostId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Likes",
                type: "int",
                nullable: true,
                comment: "Post Like Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Post Comment Identifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Likes",
                type: "int",
                nullable: false,
                comment: "Like Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Comment Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Likes",
                type: "int",
                nullable: true,
                comment: "Post Comment Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Post Like Identifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Likes",
                type: "int",
                nullable: false,
                comment: "Comment Identifier",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Like Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Posts_PostId",
                table: "AspNetUsers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
