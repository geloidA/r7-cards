namespace Cardmngr.Extensions;

public static class CommonExtensions
{
    public static float CountDaysTo(this DateTime from, DateTime? to)
    {
        ArgumentNullException.ThrowIfNull(to);
        if (from > to) throw new ArgumentException("start can'be bigger than end");
        return (float)Math.Round((to - from).Value.TotalDays);        
    }

    public static float CountDaysTo(this DateTime? from, DateTime to)
    {
        ArgumentNullException.ThrowIfNull(from);
        if (from > to) throw new ArgumentException("start can'be bigger than end");
        return (float)Math.Round((to - from).Value.TotalDays);
    }

    public static string ToShortYearString(this DateTime date) 
        => date.ToString(date.Year == DateTime.Now.Year ? "d MMM" : "d MMM yyyy");
}
