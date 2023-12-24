using Microsoft.AspNetCore.Mvc;
using System.Text;
using xo.Api.Entities;
using xo.Api.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AppDbContext>();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/games", (AppDbContext db) =>
{
    return from g in db.Games.ToList() select g.ToDto();
});

app.MapGet("/players", (AppDbContext db) =>
{
    return from p in db.Players.ToList() select p.ToDto();
});



app.MapGet("/players/new", (AppDbContext db) =>
{
    var player = new Player
    {
        Player_Id = Guid.NewGuid(),
        Name = "No-Name"
    };

    db.Players.Add(player);

    return Results.Ok(player.ToDto());
});

app.MapGet("/games/join/{player_id}", (AppDbContext db, Guid player_id) =>
{
    GameManager gameManager = new GameManager();
    var game = gameManager.JoinGame(player_id);
    db.Games.Add(game);

    return Results.Ok( game.ToDto() );
});

app.MapGet("/games/{game_id}", (AppDbContext db,Guid game_id) =>
{
    Game? game = db.Games.Where(g => g.Game_Id == game_id).FirstOrDefault();

    if(game == null)
    {
        return Results.NotFound("Game Not Found");
    }

    return Results.Ok(game.ToDto());
});

app.MapPut("/games/play", (AppDbContext db, [FromBody] GamePlayDto play) =>
{
    Game? game = db.Games.Where(g => g.Game_Id == play.Game_Id).FirstOrDefault();
    
    if(game == null)
    {
        return Results.BadRequest("game not found");
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
    }
    else
    {
        sbuilder[position] = 'O';
    }

    game.Board = sbuilder.ToString();

});

app.Run();

class AppDbContext
{
    public List<Game> Games { get; set; } = new List<Game>()
    {
        new Game(),
        new Game()
    };
    public List<Player> Players { get; set; } = new List<Player>()
    {
        new Player(),
        new Player(),
        new Player()
    };

}

