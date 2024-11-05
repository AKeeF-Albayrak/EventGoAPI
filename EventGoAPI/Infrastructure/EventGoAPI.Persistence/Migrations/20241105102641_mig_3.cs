using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventGoAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Participants",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_Id",
                table: "Participants",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_Id",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Participants",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
