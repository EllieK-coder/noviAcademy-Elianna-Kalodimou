namespace WorldRank;

public class Wallet
{
    public enum Currency
    {
        EUR,
        USD,
        GBP
    }

    public decimal Balance { get; private set; }
    public Currency WalletCurrency { get; private set; }
    public bool IsBlocked;

    public Wallet(decimal balance, Currency currency, bool isBlocked)
    {
        Balance = balance;
        WalletCurrency = currency;
        IsBlocked = isBlocked;
    }

    public Wallet(Currency currency)
    {
        WalletCurrency = currency;
    }

    public void SetBalance(decimal balance)
    {
        if (balance < 0)
        {
            return;
        }
        Balance = balance;
    }

    public override string ToString() =>
        $"Balance -> {Balance} Currency -> {WalletCurrency} IsBlocked -> {IsBlocked}";
}