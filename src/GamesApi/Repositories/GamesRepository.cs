using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace GamesApi;

public class GamesRepository : IGamesRepository
{
    private  static readonly Game[] _games = new []
    {
        new Game
        {
            Id = 1, Name = "The Legend of Zelda: Breath of the Wild",
            Genres = new List<Genre>() { Genre.Action, Genre.Adventure },
            Description = "An open-world adventure game set in the kingdom of Hyrule.", Developer = "Nintendo",
            Engine = "Havok", ReleaseDate = new DateOnly(2017, 3, 3),
            CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/0/0b/The_Legend_of_Zelda_Breath_of_the_Wild.jpg"
        },
        new Game
        {
            Id = 2, Name = "God of War",
            Genres = new List<Genre>() { Genre.Action, Genre.Adventure },
            Description = "A mythological action-adventure game following Kratos and his son Atreus.",
            Developer = "Santa Monica Studio", Engine = "Havok", ReleaseDate = new DateOnly(2018, 4, 20),
            CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/a/a7/God_of_War_4_cover.jpg"
        },
        new Game
        {
            Id = 3, Name = "Minecraft",
            Genres = new List<Genre>() { Genre.Sandbox, Genre.Adventure },
            Description = "A sandbox game that allows players to build and explore virtual worlds made of blocks",
            Developer = "Mojang Studios", Engine = "Custom", ReleaseDate = new DateOnly(2011, 11, 18),
            CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/51/Minecraft_cover.png"
        },
    };

    private readonly GameDb _db;

    public GamesRepository(ILogger<GamesRepository> logger, GameDb context)
    {
        _db = context;
    }

    public Task<List<Game>> GetAllGames() => _db.Games.ToListAsync();

    public List<Game> GetAllGamesMock() => _games.ToList();
}