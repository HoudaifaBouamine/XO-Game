using System.Drawing;

namespace xo.Api.Dtos.GameDtos
{
    public class GamePlayDto
    {
        required public int Game_Id { get; set; }
        required public int Player_Id { get; set; }
        required public Point Position { get; set; }

    }
}
