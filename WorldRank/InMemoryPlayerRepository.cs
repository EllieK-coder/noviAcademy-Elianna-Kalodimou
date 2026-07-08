using WorldRank;

public class InMemoryPlayerRepository : IPlayerRepository
{
    public void AddPlayer(Player p) => throw new NotImplementedException();

    public void DeletePlayer(int playerid) => throw new NotImplementedException();

    public void FindPlayer(int playerid) => throw new NotImplementedException();

    public Dictionary<int, Player> GroupPlayersByScore() => throw new NotImplementedException();
}
