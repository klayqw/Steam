using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam.Migrations
{
    /// <inheritdoc />
    public partial class EditWorkShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "workShops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_workShops_GameId",
                table: "workShops",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_workShops_Games_GameId",
                table: "workShops",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workShops_Games_GameId",
                table: "workShops");

            migrationBuilder.DropIndex(
                name: "IX_workShops_GameId",
                table: "workShops");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "workShops");
        }
    }
}
