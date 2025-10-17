using Microsoft.EntityFrameworkCore;

namespace GamesApi;

public class GameDb : DbContext
{
    public GameDb(DbContextOptions<GameDb> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
}