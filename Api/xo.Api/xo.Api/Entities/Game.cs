using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace xo.Api.Entities
{
    public partial class Game
    {

        public Guid Game_Id { get; set; }

        public string Board { get; set; } = "123456789";

        [ForeignKey(nameof(Player1))]
        public Guid Player1_Id { get; set; }
        public Player Player1 { get; set; } = null!;
        
        
        [ForeignKey(nameof(Player2))]
        public Guid Player2_Id { get; set;}
        public Player? Player2 { get; set; } = null;

        [ForeignKey(nameof(CurrentTurn))]
        public Guid CurrentTurn_Id { get; set; }
        public Player CurrentTurn { get; set; } = null!;

        [ForeignKey(nameof(Winner))]
        public Guid Winner_Id { get;set; }
        public Player? Winner { get; set; } = null;

    }

    public partial class Game
    {
        public GameReadDto ToDto()
        {
            return new GameReadDto()
            {
                Game_Id = this.Game_Id,
                Board = this.Board,
                Player1 = this.Player1,
                Winner = this.Winner,
                CurrentTurn = this.CurrentTurn,
                Player2 = this.Player2,
            };
        }
    }


    public class GameJoinDto
    {
        public Guid Player_Id { get; set; }
    }

    public class GameReadDto
    {
        public Guid Game_Id { get; set; }
        public string Board { get; set; } = "#########";
        public Player Player1 { get; set; } = null!;
        public Player? Player2 { get; set; } = null;
        public Player CurrentTurn { get; set; } = null!;
        public Player? Winner { get; set; } = null;
    }

    public class GamePlayDto
    {
        required public Guid Game_Id { get; set; }
        required public Guid Player_Id { get; set; }
        required public Point Position { get; set; }

    }
}
