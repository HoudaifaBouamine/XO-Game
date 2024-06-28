using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using xo.Api.Dtos.GameDtos;

namespace xo.Api.Entities
{
    public partial class Game
    {
        [Key]
        public int Game_Id { get; set; }

        public string Board { get; set; } = "#########";

        [ForeignKey(nameof(Player1))]
        public int Player1_Id { get; set; }
        public Player Player1 { get; set; } = null!;
        
        
        [ForeignKey(nameof(Player2))]
        public int? Player2_Id { get; set;}
        public Player? Player2 { get; set; } = null;

        [ForeignKey(nameof(CurrentTurn))]
        public int CurrentTurn_Id { get; set; }
        public Player CurrentTurn { get; set; } = null!;

        [ForeignKey(nameof(Winner))]
        public int? Winner_Id { get;set; }
        public Player? Winner { get; set; } = null;
        public bool IsGameOver { get; set; } = false;
    }

 
}
