namespace xo.Api.Dtos.PlayerDtos
{
    public class PlayerDto
    {
        public int Player_Id { get; set; }
        public string Name { get; set; } = "";

    }

    public class PlayerRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    } 
    
    public class PlayerLoginDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
