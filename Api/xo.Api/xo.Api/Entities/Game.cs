using System.ComponentModel.DataAnnotations.Schema;
using xo.Api.Dtos.GameDtos;

namespace xo.Api.Entities
{
    public partial class Game
    {

        public Guid Game_Id { get; set; }

        public string Board { get; set; } = "#########";

        [ForeignKey(nameof(Player1))]
        public Guid Player1_Id { get; set; }
        public Player Player1 { get; set; } = null!;
        
        
        [ForeignKey(nameof(Player2))]
        public Guid? Player2_Id { get; set;}
        public Player? Player2 { get; set; } = null;

        [ForeignKey(nameof(CurrentTurn))]
        public Guid CurrentTurn_Id { get; set; }
        public Player CurrentTurn { get; set; } = null!;

        [ForeignKey(nameof(Winner))]
        public Guid? Winner_Id { get;set; }
        public Player? Winner { get; set; } = null;
        public bool IsGameOver { get; set; } = false;
    }

 
}
