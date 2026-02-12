using System.Globalization;

namespace RetailBankingPortal.Utils;

public static class Utility
{
    public static long DateToEpoch(string dateStr)
    {
        var date = DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        return new DateTimeOffset(date).ToUnixTimeMilliseconds();
    }

    public static string EpochToDate(long epoch)
    {
        var date = DateTimeOffset.FromUnixTimeMilliseconds(epoch).DateTime;
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static List<T> RemoveDuplicates<T>(List<T> list)
    {
        return new List<T>(new LinkedList<T>(list.Distinct()));
    }

    public static List<T> FindDuplicates<T, TKey>(List<T>? list, Func<T, TKey> uniqueKey)
    {
        if (list == null)
            return new List<T>();

        return list
            .GroupBy(el => uniqueKey(el) ?? (object)string.Empty)
            .Where(g => g.Count() > 1)
            .Select(g => g.First())
            .ToList();
    }
}
