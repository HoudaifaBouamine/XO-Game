namespace xo.Api.Entities
{
    public partial class Player
    {

        public Guid Player_Id { get; set; }

        public string Name { get; set; } = "";

    }

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

    public class PlayerDto
    {
        public Guid Player_Id { get; set; }
        public string Name { get; set; } = "";

    }
}
