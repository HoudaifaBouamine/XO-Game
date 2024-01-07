using xo.Api.Entities;

namespace xo.Api.Dtos.GameDtos
{
    public class GameReadDto
    {
        public int Game_Id { get; set; }
        public string Board { get; set; } = "#########";
        public Player Player1 { get; set; } = null!;
        public Player? Player2 { get; set; } = null;
        public Player CurrentTurn { get; set; } = null!;
        public Player? Winner { get; set; } = null;

        public bool IsGameOver { get; set; }
    }
}
