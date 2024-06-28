using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xo.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceGuidByIntAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Player_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Player_Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Game_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player1_Id = table.Column<int>(type: "int", nullable: false),
                    Player2_Id = table.Column<int>(type: "int", nullable: true),
                    CurrentTurn_Id = table.Column<int>(type: "int", nullable: false),
                    Winner_Id = table.Column<int>(type: "int", nullable: true),
                    IsGameOver = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Game_Id);
                    table.ForeignKey(
                        name: "FK_Games_Players_CurrentTurn_Id",
                        column: x => x.CurrentTurn_Id,
                        principalTable: "Players",
                        principalColumn: "Player_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Players_Player1_Id",
                        column: x => x.Player1_Id,
                        principalTable: "Players",
                        principalColumn: "Player_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Players_Player2_Id",
                        column: x => x.Player2_Id,
                        principalTable: "Players",
                        principalColumn: "Player_Id");
                    table.ForeignKey(
                        name: "FK_Games_Players_Winner_Id",
                        column: x => x.Winner_Id,
                        principalTable: "Players",
                        principalColumn: "Player_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentTurn_Id",
                table: "Games",
                column: "CurrentTurn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1_Id",
                table: "Games",
                column: "Player1_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2_Id",
                table: "Games",
                column: "Player2_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Winner_Id",
                table: "Games",
                column: "Winner_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
