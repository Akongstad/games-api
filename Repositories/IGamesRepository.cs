namespace games_api;

public interface IGamesRepository
{
     public Task<List<Game>> GetAllGames();
     public List<Game> GetAllGamesMock();
}