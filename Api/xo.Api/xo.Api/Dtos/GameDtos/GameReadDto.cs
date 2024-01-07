using xo.Api.Dtos.PlayerDtos;
using xo.Api.Entities;

namespace xo.Api.Dtos.GameDtos
{
    public class GameReadDto
    {
        public int Game_Id { get; set; }
        public string Board { get; set; } = "#########";
        public PlayerReadDto Player1 { get; set; } = null!;
        public PlayerReadDto? Player2 { get; set; } = null;
        public PlayerReadDto CurrentTurn { get; set; } = null!;
        public PlayerReadDto? Winner { get; set; } = null;

        public bool IsGameOver { get; set; }
    }
}
