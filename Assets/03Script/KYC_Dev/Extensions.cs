public static class Extensions
{
    /// <summary>
    /// 숫자에 단위를 표기하여 출력 (소숫점 2째자리까지)
    /// K = 1000 / M = 1000000 / B = 1000000000
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string TextFormatCurrency(this ulong amount)
    {
        if (amount < 1000) return amount.ToString("0");
        if (amount < 1000000) return (amount / (double)1000).ToString("0.##") + "K";
        if (amount < 1000000000) return (amount / (double)1000000).ToString("0.##") + "M";
        return (amount / (double)1000000000).ToString("0.##") + "B";
    }

    /// <summary>
    /// 천단위 숫자에 콤마 붙여서 출력
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string TextFormatComma(this ulong amount)
    {
        return string.Format("{0:#,###}", amount);
    }
    
    /// <summary>
    /// 숫자에 단위를 표기하여 출력 (소숫점 2째자리까지)
    /// K = 1000 / M = 1000000 / B = 1000000000
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string TextFormatCurrency(this int amount)
    {
        if (amount < 1000) return amount.ToString("0");
        if (amount < 1000000) return (amount / (double)1000).ToString("0.##") + "K";
        if (amount < 1000000000) return (amount / (double)1000000).ToString("0.##") + "M";
        return (amount / (double)1000000000).ToString("0.##") + "B"; // 십억 단위
    }

    /// <summary>
    /// 천단위 숫자에 콤마 붙여서 출력
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string TextFormatComma(this int amount)
    {
        return string.Format("{0:#,###}", amount);
    }
}
