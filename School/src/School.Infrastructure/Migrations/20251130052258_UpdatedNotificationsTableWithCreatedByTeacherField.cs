using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNotificationsTableWithCreatedByTeacherField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByTeacherId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedByTeacherId",
                table: "Notifications",
                column: "CreatedByTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_CreatedByTeacherId",
                table: "Notifications",
                column: "CreatedByTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_CreatedByTeacherId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CreatedByTeacherId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedByTeacherId",
                table: "Notifications");
        }
    }
}
