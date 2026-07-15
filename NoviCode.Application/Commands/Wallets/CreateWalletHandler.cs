using NoviCode.Commands;

namespace NoviCode;

public sealed class CreateWalletHandler : ICommandHandler<CreateWalletCommand, Wallet?>
{
    private readonly IWalletRepository _wallets;
    public CreateWalletHandler(IWalletRepository wallets) => _wallets = wallets;

    public async Task<Wallet?> HandleAsync(CreateWalletCommand command, CancellationToken ct = default)
    {
        var wallet = new Wallet(command.PlayerId, command.Currency);
        await _wallets.AddAsync(wallet, ct);
        return wallet;
    }
}

public sealed class DepositHandler : ICommandHandler<DepositCommand, Wallet?>
{
    private readonly IWalletRepository _wallets;
    public DepositHandler(IWalletRepository wallets) => _wallets = wallets;

    public async Task<Wallet?> HandleAsync(DepositCommand command, CancellationToken ct = default)
    {
        var wallet = await _wallets.GetByIdAsync(command.WalletId, ct);
        if (wallet is null) return null;

        wallet.Deposit(command.Amount); // may throw WalletBlockedException / InvalidAmountException
        await _wallets.SaveChangesAsync(ct);
        return wallet;
    }
}

public sealed class ApplyFundsHandler : ICommandHandler<ApplyFundsCommand, Wallet?>
{
    private readonly IWalletRepository _wallets;
    public ApplyFundsHandler(IWalletRepository wallets) => _wallets = wallets;

    public async Task<Wallet?> HandleAsync(ApplyFundsCommand command, CancellationToken ct = default)
    {
        command.Strategy.Execute(command.Wallet, command.Amount); // strategy pattern preserved
        await _wallets.SaveChangesAsync(ct);
        return command.Wallet;
    }
}

public sealed class SetWalletBlockedHandler : ICommandHandler<SetWalletBlockedCommand, Wallet?>
{
    private readonly IWalletRepository _wallets;
    public SetWalletBlockedHandler(IWalletRepository wallets) => _wallets = wallets;

    public async Task<Wallet?> HandleAsync(SetWalletBlockedCommand command, CancellationToken ct = default)
    {
        if (command.Blocked) command.Wallet.Block();
        else command.Wallet.Unblock();

        await _wallets.SaveChangesAsync(ct);
        return command.Wallet;
    }
}