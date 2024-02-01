using System.Collections;
using Cardmngr.Utils;
using Cardmngr.Extensions;

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

    [Theory]
    [ClassData(typeof(GetDaysToMethodTestData))]
    public void DateTime_CountDaysTo_ReturnCorrectData(DateTime from, DateTime to, double expected)
    {
        var actual = from.CountDaysTo(to);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DateTime_CountDaysTo_ThrowException_WhenToIsBeforeFrom()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2023, 1, 2);

        Assert.Throws<ArgumentException>(() => from.CountDaysTo(to));
    }
}

public class GetDaysToMethodTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [new DateTime(2024, 1, 1), new DateTime(2024, 1, 2), 1];
        yield return [new DateTime(2024, 5, 18), new DateTime(2024, 6, 18), 31];
        yield return [new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 2, 9, 0, 0), 1];
        yield return [new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 2, 12, 0, 0), 2];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
