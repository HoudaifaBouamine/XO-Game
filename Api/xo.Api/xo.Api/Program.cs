
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using xo.Api.Data;
using xo.Api.Dtos.GameDtos;
using xo.Api.Dtos.PlayerDtos;
using xo.Api.Entities;
using xo.Api.Services.AuthenticationService;
using xo.Api.Services.SecurityService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataLayer>();
builder.Services.AddCors();
builder.Services.AddAuthentication(AuthService.Scheme.Cookie)
    .AddCookie(AuthService.Scheme.Cookie);

builder.Services.AddAuthorization(conf =>
{
    conf.AddPolicy(AuthService.Policy.UserExist, p =>
    {
        p.AddAuthenticationSchemes(AuthService.Scheme.Cookie);
        p.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

app.UseCors(conf =>
{
    conf.AllowAnyHeader();
    conf.AllowAnyMethod();
    conf.AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/games", (DataLayer db) =>
{
    return from g in db.Games.ToList() select g.ToDto(db.Players);
});

app.MapGet("/players", (DataLayer db) =>
{
    return from p in db.Players.ToList() select p.ToDto();
});



app.MapGet("/players/new", async (DataLayer db) =>
{
    var player = new Player
    {
        Player_Id = default,
        Name = "No-Name"
    };

    db.Players.Add(player);

    await db.SaveChangesAsync();

    return Results.Ok(player.ToDto());
});

app.MapGet("/games/join/{player_id}", async (DataLayer db, int player_id) =>
{

    Game? game = db.Games.Where(g => g.Player2_Id == null).FirstOrDefault();

    if(game == null)
    {
        game = new Game()
        {
            Game_Id = default,
            Player1_Id = player_id,
            CurrentTurn_Id = player_id
        };

        db.Games.Add(game);
    }
    else
    {
        game.Player2_Id = player_id;
    }


    await db.SaveChangesAsync();

    return Results.Ok( game.ToDto(db.Players) );
});

app.MapGet("/games/{game_id}", (DataLayer db,int game_id) =>
{
    Game? game = db.Games.Where(g => g.Game_Id == game_id).FirstOrDefault();

    if(game == null)
    {
        return Results.NotFound("Game Not Found");
    }

    return Results.Ok(game.ToDto(db.Players));
});

app.MapPut("/games/play", async (DataLayer db, [FromBody] GamePlayDto play) =>
{
    Game? game = db.Games.Where(g => g.Game_Id == play.Game_Id).FirstOrDefault();
    
    if(game == null)
    {
        return Results.BadRequest("game not found");
    }

    if(game.Winner_Id is not null || game.IsGameOver == true)
    {
        return Results.BadRequest("game over");
    }

    if(game.CurrentTurn_Id != play.Player_Id)
    {
        return Results.BadRequest("not the player turn");
    }

    int position = play.Position.X + play.Position.Y * 3;

    char posToPlay = game.Board[position];

    if (posToPlay != '#')
    {
        return Results.BadRequest("cell is not empty");
    }

    var sbuilder = new StringBuilder(game.Board);

    if (game.Player1_Id == play.Player_Id)
    {
        sbuilder[position] = 'X';
        game.CurrentTurn_Id = game.Player2_Id!.Value;
    }
    else
    {
        sbuilder[position] = 'O';
        game.CurrentTurn_Id = game.Player1_Id;
    }

    game.Board = sbuilder.ToString();

    var winner = Game.CheckWinner(game.Board);

    if (winner == Game.enWinner.X_Win)
    {
        game.Winner_Id = game.Player1_Id;
        game.IsGameOver = true;
    }
    else if (winner == Game.enWinner.O_Win)
    {
        game.Winner_Id = game.Player2_Id;
        game.IsGameOver = true;
    }
    else if(winner == Game.enWinner.Draw)
    {
        game.IsGameOver = true;
    }

    await db.SaveChangesAsync();

    return Results.Ok(game.ToDto(db.Players));
});



app.MapPost("/v2/register", async (DataLayer db,[FromBody] PlayerRegisterDto playerRegister) =>
{
    Player? player = await db.Players.Where(p=>p.Name == playerRegister.Name).FirstOrDefaultAsync();

    if(player is not null)
    {
        return Results.BadRequest("Player with this name already exist");
    }

    player = new Player
    {
        Player_Id = default,
        Name = playerRegister.Name,
        HashedPassword = SecurityService.HashPassword( playerRegister.Password ),
    };

    db.Players.Add(player);
    await db.SaveChangesAsync();

    return Results.Created($"/players/{player.Player_Id}",player.ToDto());
});

app.MapPost("/v2/login", async (HttpContext ctx,DataLayer db, [FromBody] PlayerLoginDto playerLogin) =>
{
    Player? player = await db.Players.Where(p => p.Name == playerLogin.Name).FirstOrDefaultAsync();

    if(player is null) 
    {
        return Results.Unauthorized();
    }

    if (! SecurityService.VerifyPassword(player.HashedPassword, playerLogin.Password))
    {
        return Results.Unauthorized();
    }

    PlayerReadDto playerDto = player.ToDto();

    await ctx.SignInAsync(AuthService.Scheme.Cookie, AuthService.LoginPlayer(playerDto,AuthService.Scheme.Cookie));

    return Results.Ok(playerDto);
});

app.MapGet("/v2/games/join", async (HttpContext ctx,DataLayer db) =>
{
    int player_id = Convert.ToInt32( ctx.User.FindFirst(AuthService.Claims.PlayerId)!.Value );

    Game? game = db.Games.Where(g => g.Player2_Id == null).FirstOrDefault();

    if (game == null)
    {
        game = new Game()
        {
            Game_Id = default,
            Player1_Id = player_id,
            CurrentTurn_Id = player_id
        };

        db.Games.Add(game);
    }
    else
    {
        game.Player2_Id = player_id;
    }


    await db.SaveChangesAsync();

    return Results.Ok(game.ToDto(db.Players));
}).RequireAuthorization(AuthService.Policy.UserExist);

app.MapGet("/v2/try-login-with-cookie", async (HttpContext ctx,DataLayer db) =>
{
    var id_claim = ctx.User.FindFirst(AuthService.Claims.PlayerId);

    if(id_claim is null)
    {
        return Results.NotFound("id claim not found");
    }

    int id = Convert.ToInt32(id_claim.Value);

    Player? player = await db.Players.Where(p => p.Player_Id == id).FirstOrDefaultAsync();

    if(player is null) 
    {
        return Results.NotFound("user with this cookie not found");   
    } 

    return Results.Ok(player.ToDto());

}).RequireAuthorization(AuthService.Policy.UserExist);

app.MapGet("/v2/games/join/{player_id}", async (DataLayer db, int player_id) =>
{

    Game? game = db.Games.Where(g => g.Player2_Id == null).FirstOrDefault();

    if (game == null)
    {
        game = new Game()
        {
            Game_Id = default,
            Player1_Id = player_id,
            CurrentTurn_Id = player_id
        };

        db.Games.Add(game);
    }
    else
    {
        game.Player2_Id = player_id;

        if (new Random().Next() % 2 == 0)
        {
            game.CurrentTurn_Id = Convert.ToInt32(game.Player2_Id);
        }
        else
        {
            game.CurrentTurn_Id = game.Player1_Id;
        }
    }


    await db.SaveChangesAsync();

    return Results.Ok(game.ToDto(db.Players));
}).RequireAuthorization(AuthService.Policy.UserExist);

app.MapPut("/v2/games/play", async (DataLayer db, [FromBody] GamePlayDto play) =>
{
    Game? game = db.Games.Where(g => g.Game_Id == play.Game_Id).FirstOrDefault();

    if (game == null)
    {
        return Results.BadRequest("game not found");
    }

    if (game.Winner_Id is not null || game.IsGameOver == true)
    {
        return Results.BadRequest("game over");
    }

    if (game.CurrentTurn_Id != play.Player_Id)
    {
        return Results.BadRequest("not the player turn");
    }

    int position = play.Position.X + play.Position.Y * 3;

    char posToPlay = game.Board[position];

    if (posToPlay != '#')
    {
        return Results.BadRequest("cell is not empty");
    }

    var sbuilder = new StringBuilder(game.Board);

    if (game.Player1_Id == play.Player_Id)
    {
        sbuilder[position] = 'X';
        game.CurrentTurn_Id = game.Player2_Id!.Value;
    }
    else
    {
        sbuilder[position] = 'O';
        game.CurrentTurn_Id = game.Player1_Id;
    }

    game.Board = sbuilder.ToString();

    var winner = Game.CheckWinner(game.Board);

    if (winner == Game.enWinner.X_Win)
    {
        game.Winner_Id = game.Player1_Id;
        game.IsGameOver = true;
    }
    else if (winner == Game.enWinner.O_Win)
    {
        game.Winner_Id = game.Player2_Id;
        game.IsGameOver = true;
    }
    else if (winner == Game.enWinner.Draw)
    {
        game.IsGameOver = true;
    }

    await db.SaveChangesAsync();

    return Results.Ok(game.ToDto(db.Players));
}).RequireAuthorization(AuthService.Policy.UserExist);


// Test
app.MapGet("/v2/games-list-auth", (DataLayer db) =>
{
    return db.Games.ToList();
}).RequireAuthorization(AuthService.Policy.UserExist);

app.MapGet("/v2/games-list-no-auth", (DataLayer db) =>
{
    return db.Games.ToList();
});

app.Run("http://localhost:5000");
