namespace NoviCode;

public static class CacheKeys
{
    public static string Wallet(Guid id) => $"wallet:{id}";
    public static string PlayerWallets(Guid playerId) => $"wallets:player:{playerId}";
    public const string AllWallets = "wallets:all";

    public static string Player(Guid id) => $"player:{id}";
    public const string AllPlayers = "players:all";
}

public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan CacheTtl { get; }
}