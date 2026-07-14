using NoviCode.Commands;

namespace NoviCode;

public sealed record GetWalletByIdQuery(Guid Id)
    : IQuery<Wallet?>, ICacheableQuery
{
    public string CacheKey => CacheKeys.Wallet(Id);
    public TimeSpan CacheTtl => TimeSpan.FromSeconds(60);
}

public sealed record GetWalletsByPlayerQuery(Guid PlayerId)
    : IQuery<IReadOnlyList<Wallet>>, ICacheableQuery
{
    public string CacheKey => CacheKeys.PlayerWallets(PlayerId);
    public TimeSpan CacheTtl => TimeSpan.FromSeconds(60);
}

public sealed record GetAllWalletsQuery()
    : IQuery<IReadOnlyList<Wallet>>, ICacheableQuery
{
    public string CacheKey => CacheKeys.AllWallets;
    public TimeSpan CacheTtl => TimeSpan.FromSeconds(60);
}