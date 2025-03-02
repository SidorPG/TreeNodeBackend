using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class stroreDataInEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "journal_messages");

            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "journal_events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "journal_events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionStackTrace",
                table: "journal_events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "journal_events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "journal_events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestQuery",
                table: "journal_events",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "journal_events");

            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "journal_events");

            migrationBuilder.DropColumn(
                name: "ExceptionStackTrace",
                table: "journal_events");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "journal_events");

            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "journal_events");

            migrationBuilder.DropColumn(
                name: "RequestQuery",
                table: "journal_events");

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "journal_messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
