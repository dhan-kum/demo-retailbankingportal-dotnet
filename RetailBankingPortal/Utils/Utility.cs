namespace RetailBankingPortal.Utils;

public static class Utility
{
    public static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C2");
    }

    public static bool ValidateAccountNumber(string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return false;

        return accountNumber.Length == 12 && accountNumber.All(char.IsDigit);
    }

    public static string GenerateTransactionId()
    {
        return Guid.NewGuid().ToString();
    }
}
