using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiLoggingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddHttpVersionAndRequestBodyToRequestLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HttpVersion",
                table: "RequestLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "RequestLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HttpVersion",
                table: "RequestLogs");

            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "RequestLogs");
        }
    }
}
