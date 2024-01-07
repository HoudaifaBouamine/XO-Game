using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHasedPasswordToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "Players");
        }
    }
}
