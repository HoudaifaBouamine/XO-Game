using xo.Api.Dtos.PlayerDtos;

namespace xo.Api.Entities
{
    public partial class Player
    {

        public PlayerReadDto ToDto()
        {
            return new PlayerReadDto
            {
                Player_Id = this.Player_Id,
                Name = this.Name
            };
        }

    }
}
