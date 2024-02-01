namespace Cardmngr.Extensions;

public static class CommonExtensions
{
    public static double CountDaysTo(this DateTime from, DateTime to)
    {
        if (from > to) throw new ArgumentException("start can'be bigger than end");
        return Math.Round((to - from).TotalDays);        
    }
}
