using Microsoft.EntityFrameworkCore;

namespace games_api;

public class GameDb : DbContext
{
    public GameDb(DbContextOptions<GameDb> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
}