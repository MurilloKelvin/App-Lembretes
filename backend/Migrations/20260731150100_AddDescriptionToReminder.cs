using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemindersDTI.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Reminders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Reminders");
        }
    }
}
