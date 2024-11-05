using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiLoggingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddResponseSizeInBytesToResponseLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ResponseSizeInBytes",
                table: "ResponseLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseSizeInBytes",
                table: "ResponseLogs");
        }
    }
}
