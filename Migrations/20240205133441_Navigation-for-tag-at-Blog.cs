using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevDiaries.Web.Migrations
{
    /// <inheritdoc />
    public partial class NavigationfortagatBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tag_BlogId",
                table: "Tag",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Blogs_BlogId",
                table: "Tag",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Blogs_BlogId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_BlogId",
                table: "Tag");
        }
    }
}
