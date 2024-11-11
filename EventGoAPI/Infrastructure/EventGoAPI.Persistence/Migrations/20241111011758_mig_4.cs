using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventGoAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Participants_ParticipantEventId_ParticipantId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_ParticipantEventId_ParticipantId",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_Id",
                table: "Participants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                columns: new[] { "Id", "EventId" });

            migrationBuilder.CreateIndex(
                name: "IX_Points_ParticipantId_ParticipantEventId",
                table: "Points",
                columns: new[] { "ParticipantId", "ParticipantEventId" });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId",
                table: "Participants",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_ParticipantId_ParticipantEventId",
                table: "Points",
                columns: new[] { "ParticipantId", "ParticipantEventId" },
                principalTable: "Participants",
                principalColumns: new[] { "Id", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points",
                columns: new[] { "UserId", "EventId" },
                principalTable: "Participants",
                principalColumns: new[] { "Id", "EventId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Participants_ParticipantId_ParticipantEventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_ParticipantId_ParticipantEventId",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_EventId",
                table: "Participants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                columns: new[] { "EventId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Points_ParticipantEventId_ParticipantId",
                table: "Points",
                columns: new[] { "ParticipantEventId", "ParticipantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Id",
                table: "Participants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_ParticipantEventId_ParticipantId",
                table: "Points",
                columns: new[] { "ParticipantEventId", "ParticipantId" },
                principalTable: "Participants",
                principalColumns: new[] { "EventId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Participants_UserId_EventId",
                table: "Points",
                columns: new[] { "UserId", "EventId" },
                principalTable: "Participants",
                principalColumns: new[] { "EventId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
