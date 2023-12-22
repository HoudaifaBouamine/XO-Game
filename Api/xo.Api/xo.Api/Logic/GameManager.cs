using xo.Api.Entities;

namespace xo.Api.Logic
{
    public class GameManager
    {
        public Game JoinGame(Guid Player_Id)
        {
            return new Game()
            {
                Game_Id = Guid.NewGuid(),
                Player1 = new Player()
                {
                    Player_Id = Player_Id,
                    Name = "Player 1"
                }
            };
        }
    }
}
