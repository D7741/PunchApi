using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserPunchApi.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftNameToSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShiftName",
                table: "Schedules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftName",
                table: "Schedules");
        }
    }
}
