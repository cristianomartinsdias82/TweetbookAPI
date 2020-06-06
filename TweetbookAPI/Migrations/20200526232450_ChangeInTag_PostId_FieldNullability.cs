using Microsoft.EntityFrameworkCore.Migrations;

namespace TweetbookAPI.Migrations
{
    public partial class ChangeInTag_PostId_FieldNullability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
