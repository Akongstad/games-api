namespace GamesApi;

public interface IGamesRepository
{
     public Task<List<Game>> GetAllGames();
     public List<Game> GetAllGamesMock();
}