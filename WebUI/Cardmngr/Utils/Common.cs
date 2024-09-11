namespace Cardmngr.Utils;

public class Common
{
    public static string GetDayNameByDayCount(int dayCount)
    {
        return GetLastDigit(dayCount) switch
        {            
            1 => "день",
            2 => "дня",
            3 => "дня",
            4 => "дня",
            _ => "дней"
        };
    }

    public static string GetDayNameByDayCount(double dayCount)
    {
        return GetLastDigit((int)dayCount) switch
        {
            1 => "день",
            2 => "дня",
            3 => "дня",
            4 => "дня",
            _ => "дней"
        };
    }
    
    public static int GetLastDigit(int number)
    {
        var str = number.ToString();
        return int.Parse(str[^1].ToString());
    }
}
