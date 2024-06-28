using System.ComponentModel.DataAnnotations;
using xo.Api.Dtos.PlayerDtos;

namespace xo.Api.Entities
{
    public partial class Player
    {
        [Key]
        public int Player_Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string HashedPassword { get; set; } = string.Empty;

    }

}
