using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMarket.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleModelUserModel");

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "Roles",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserModelId",
                table: "Roles",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserModelId",
                table: "Roles",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserModelId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserModelId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "RoleModelUserModel",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UsersId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModelUserModel", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleModelUserModel_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleModelUserModel_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModelUserModel_UsersId",
                table: "RoleModelUserModel",
                column: "UsersId");
        }
    }
}
