using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

namespace games_api;

public static class GamesEndpoints
{
    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/game")
            .WithTags("game")
            .WithDescription("Lookup and Find Games")
            .WithOpenApi();

        _ = root.MapGet("/", GetGames)
            .Produces<List<Game>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Games")
            .WithDescription("\n    GET /game");

        _ = root.MapGet("/{id}", GetGameById)
            .Produces<Game>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a Game by its Id")
            .WithDescription("\n    GET /game/{id}");

        _ = root.MapPost("/", CreateGame)
            .Produces<Game>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new Game")
            .WithDescription("\n    POST /game");

        _ = root.MapPut("/{id}", UpdateGame)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a Game")
            .WithDescription("\n    PUT /game/{id}");

        _ = root.MapDelete("/{id}", DeleteGame)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Game")
            .WithDescription("\n    DELETE /game/{id}");

        return app;
    }

    public static async Task<IResult> GetGames([FromServices] GameDb db)
    {
        try
        {
            return Results.Ok(await db.Games.ToListAsync());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetGameById([FromRoute] int id, [FromServices] GameDb db)
    {
        try
        {
            var game = await db.Games.FindAsync(id);
            return game is null ? Results.NotFound($"Game with id {id} not found.") : Results.Ok(game);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> CreateGame([FromBody] Game game, [FromServices] GameDb db)
    {
        try
        {
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return Results.Created($"/api/game/{game.Id}", game);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> UpdateGame([FromRoute] int id, [FromBody] Game inputGame,
        [FromServices] GameDb db)
    {
        try
        {
            var game = await db.Games.FindAsync(id);
            if (game is null) return Results.NotFound($"Game with id {id} not found.");

            game.Name = inputGame.Name;
            game.Description = inputGame.Description;
            game.Developer = inputGame.Developer;
            game.Engine = inputGame.Engine;
            game.Genres = inputGame.Genres;
            game.ReleaseDate = inputGame.ReleaseDate;
            game.CoverImageUrl = inputGame.CoverImageUrl;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> DeleteGame([FromRoute] int id, [FromServices] GameDb db)
    {
        try
        {
            var game = await db.Games.FindAsync(id);
            if (game is null) return Results.NotFound($"Game with id {id} not found.");

            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}