using System.ComponentModel.DataAnnotations;

namespace games_api;

/// <summary>
/// Represents a video game with its details.
/// </summary>
public class Game
{
    public int Id { get; set; }
    [MaxLength(64)] public required string Name { get; set; }
    [MaxLength(256)] public required string Description { get; set; }
    [MaxLength(64)] public string? Engine { get; set; }
    [MaxLength(64)] public required string Developer { get; set; }
    [MaxLength(4)] public required IEnumerable<Genre> Genres { get; set; }

    [MaxLength(256)] public required string? CoverImageUrl { get; set; }
    public DateOnly ReleaseDate { get; set; }
}