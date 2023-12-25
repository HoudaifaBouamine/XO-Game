using Microsoft.AspNetCore.Mvc;
using System.Text;
using xo.Api.Data;
using xo.Api.Dtos.GameDtos;
using xo.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataLayer>();
var app = builder.Build();


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
        Player_Id = Guid.NewGuid(),
        Name = "No-Name"
    };

    db.Players.Add(player);

    await db.SaveChangesAsync();

    return Results.Ok(player.ToDto());
});

app.MapGet("/games/join/{player_id}", async (DataLayer db, Guid player_id) =>
{

    Game? game = db.Games.Where(g => g.Player2_Id == null).FirstOrDefault();

    if(game == null)
    {
        game = new Game()
        {
            Game_Id = Guid.NewGuid(),
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

app.MapGet("/games/{game_id}", (DataLayer db,Guid game_id) =>
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

app.Run();

