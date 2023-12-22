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

app.MapGet("/games/join/{player_id}", (AppDbContext db, Guid player_id) =>
{
    GameManager gameManager = new GameManager();
    var game = gameManager.JoinGame(player_id);
    db.Games.Add(game);

    return Results.Ok( game.ToDto() );
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

