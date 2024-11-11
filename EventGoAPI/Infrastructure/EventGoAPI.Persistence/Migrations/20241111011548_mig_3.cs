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
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points",
                columns: new[] { "UserId", "EventId" },
                principalTable: "Participants",
                principalColumns: new[] { "EventId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points",
                columns: new[] { "UserId", "EventId" },
                principalTable: "Participants",
                principalColumns: new[] { "EventId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
