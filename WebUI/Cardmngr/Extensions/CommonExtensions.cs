namespace Cardmngr.Extensions;

public static class CommonExtensions
{
    public static double CountDaysTo(this DateTime from, DateTime? to)
    {
        ArgumentNullException.ThrowIfNull(to);
        if (from > to) throw new ArgumentException("start can'be bigger than end");
        return Math.Round((to - from).Value.TotalDays);        
    }

    public static double CountDaysTo(this DateTime? from, DateTime to)
    {
        ArgumentNullException.ThrowIfNull(from);
        if (from > to) throw new ArgumentException("start can'be bigger than end");
        return Math.Round((to - from).Value.TotalDays);        
    }
}
