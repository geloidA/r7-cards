using Cardmngr.Utils;

namespace Cardmngr.Tests;

public class CommonTests
{
    [Theory]
    [InlineData(1, "день")]
    [InlineData(2, "дня")]
    [InlineData(3, "дня")]
    [InlineData(4, "дня")]
    [InlineData(5, "дней")]
    [InlineData(6, "дней")]
    [InlineData(7, "дней")]
    [InlineData(8, "дней")]
    [InlineData(9, "дней")]
    [InlineData(0, "дней")]
    [InlineData(123, "дня")]
    public static void GetDayNameByDayCount_Test(int dayCount, string expected)
    {
        var actual = Common.GetDayNameByDayCount(dayCount);
        Assert.Equal(expected, actual);
    }
}
