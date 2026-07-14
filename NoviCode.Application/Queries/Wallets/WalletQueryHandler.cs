using NoviCode.Commands;

namespace NoviCode;

public sealed class GetWalletByIdHandler : IQueryHandler<GetWalletByIdQuery, Wallet?>
{
    private readonly IWalletRepository _wallets;
    public GetWalletByIdHandler(IWalletRepository wallets) => _wallets = wallets;

    public Task<Wallet?> HandleAsync(GetWalletByIdQuery query, CancellationToken ct = default)
        => _wallets.GetByIdAsync(query.Id, ct);
}

public sealed class GetWalletsByPlayerHandler : IQueryHandler<GetWalletsByPlayerQuery, IReadOnlyList<Wallet>>
{
    private readonly IWalletRepository _wallets;
    public GetWalletsByPlayerHandler(IWalletRepository wallets) => _wallets = wallets;

    public Task<IReadOnlyList<Wallet>> HandleAsync(GetWalletsByPlayerQuery query, CancellationToken ct = default)
        => _wallets.GetByPlayerAsync(query.PlayerId, ct);
}

public sealed class GetAllWalletsHandler : IQueryHandler<GetAllWalletsQuery, IReadOnlyList<Wallet>>
{
    private readonly IWalletRepository _wallets;
    public GetAllWalletsHandler(IWalletRepository wallets) => _wallets = wallets;

    public Task<IReadOnlyList<Wallet>> HandleAsync(GetAllWalletsQuery query, CancellationToken ct = default)
        => _wallets.GetAllAsync(ct);
}