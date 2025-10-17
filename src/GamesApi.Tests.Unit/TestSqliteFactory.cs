// C#

using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GamesApi.Tests.Unit;

public sealed class TestSqliteDatabase : IDisposable
{
    public DbContextOptions<GameDb> Options { get; }

    private SqliteConnection _connection;
    private bool _disposed;

    private TestSqliteDatabase(SqliteConnection connection, DbContextOptions<GameDb> options)
    {
        _connection = connection;
        Options = options;
    }

    public static TestSqliteDatabase Create(params Game[] seed)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<GameDb>()
            .UseSqlite(connection)
            .Options;

        using (var ctx = new GameDb(options))
        {
            ctx.Database.EnsureCreated();

            if (seed.Length > 0)
            {
                ctx.Games.AddRange(seed);
                ctx.SaveChanges();
            }
        }

        return new TestSqliteDatabase(connection, options);
    }

    public GameDb CreateContext() => new GameDb(Options);

    public void Dispose()
    {
        if (_disposed) return;
        _connection.Close();
        _connection.Dispose();
        _disposed = true;
    }
}