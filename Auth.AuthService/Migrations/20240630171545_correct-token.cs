using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.AuthService.Migrations
{
    /// <inheritdoc />
    public partial class correcttoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "Users",
                newName: "EmailComfirmed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailComfirmed",
                table: "Users",
                newName: "EmailConfirmed");
        }
    }
}
