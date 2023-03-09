using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMarket.Migrations
{
    /// <inheritdoc />
    public partial class erdsdc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleModelId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleModelId",
                table: "Users",
                column: "RoleModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleModelId",
                table: "Users",
                column: "RoleModelId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleModelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleModelId",
                table: "Users");
        }
    }
}
