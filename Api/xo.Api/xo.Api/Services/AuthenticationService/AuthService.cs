using System.Security.Claims;
using xo.Api.Dtos.PlayerDtos;

namespace xo.Api.Services.AuthenticationService
{
    public class AuthService
    {

        public class Scheme
        {
            public const string Cookie = "cookie";
        }

        public class Policy
        {
            public const string UserExist = "user-exist";
        }

        public static ClaimsPrincipal LoginPlayer(PlayerReadDto player, string AuthScheme)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(Claims.PlayerName,player.Name),
                new Claim(Claims.PlayerId,player.Player_Id.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthScheme);
            return new ClaimsPrincipal(identity);
        }

        public class Claims
        {
            public const string PlayerName = "Player-Name";
            public const string PlayerId = "Player-Id";
        }
    }
}
