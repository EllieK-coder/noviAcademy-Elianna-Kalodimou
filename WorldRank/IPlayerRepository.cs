namespace WorldRank;

public interface IPlayerRepository
{
    void AddPlayer(Player p);
    void FindPlayer(int playerid);
    void DeletePlayer(int playerid);

    Dictionary<int, Player> GroupPlayersByScore();
}