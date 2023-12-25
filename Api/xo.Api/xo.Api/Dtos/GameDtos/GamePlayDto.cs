using System.Drawing;

namespace xo.Api.Dtos.GameDtos
{
    public class GamePlayDto
    {
        required public Guid Game_Id { get; set; }
        required public Guid Player_Id { get; set; }
        required public Point Position { get; set; }

    }
}
