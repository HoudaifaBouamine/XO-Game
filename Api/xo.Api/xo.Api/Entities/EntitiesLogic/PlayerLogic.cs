using xo.Api.Dtos.PlayerDtos;

namespace xo.Api.Entities
{
    public partial class Player
    {

        public PlayerDto ToDto()
        {
            return new PlayerDto
            {
                Player_Id = this.Player_Id,
                Name = this.Name
            };
        }

    }
}
