using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

namespace GamesApi;

public static class GamesEndpoints
{
    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var gamesRoot = app.MapGroup("/api/game")
            .WithTags("game")
            .WithDescription("Lookup and Find Games")
            .WithOpenApi();

        gamesRoot.MapGet("/", GetGames)
            .Produces<List<Game>>(statusCode: StatusCodes.Status200OK)
            .WithSummary("Lookup all Games")
            .WithDescription("\n    GET /game");

        gamesRoot.MapGet("/{id}", GetGameById)
            .Produces<Game>(statusCode: StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Lookup a Game by its Id")
            .WithDescription("\n    GET /game/{id}");

        gamesRoot.MapPost("/", CreateGame)
            .Produces<Game>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithSummary("Create a new Game")
            .WithDescription("\n    POST /game");

        gamesRoot.MapPut("/{id}", UpdateGame)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update a Game")
            .WithDescription("\n    PUT /game/{id}");

        gamesRoot.MapDelete("/{id}", DeleteGame)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete a Game")
            .WithDescription("\n    DELETE /game/{id}");

        return app;
    }

    public static async Task<IResult> GetGames([FromServices] GameDb db)
    {
        return TypedResults.Ok(await db.Games.ToListAsync());
    }

    public static async Task<IResult> GetGameById([FromRoute] int id, [FromServices] GameDb db)
    {
        var game = await db.Games.FindAsync(id);
        return game is null ? TypedResults.NotFound($"Game with id {id} not found.") : TypedResults.Ok(game);
    }

    public static async Task<IResult> CreateGame([FromBody] Game game, [FromServices] GameDb db)
    {
        db.Games.Add(game);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/api/game/{game.Id}", game);
    }

    public static async Task<IResult> UpdateGame([FromRoute] int id, [FromBody] Game inputGame,
        [FromServices] GameDb db)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return TypedResults.NotFound($"Game with id {id} not found.");

        game.Name = inputGame.Name;
        game.Description = inputGame.Description;
        game.Developer = inputGame.Developer;
        game.Engine = inputGame.Engine;
        game.Genres = inputGame.Genres;
        game.CoverImageUrl = inputGame.CoverImageUrl;
        game.ReleaseDate = inputGame.ReleaseDate;
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteGame([FromRoute] int id, [FromServices] GameDb db)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return TypedResults.NotFound($"Game with id {id} not found.");

        db.Games.Remove(game);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}