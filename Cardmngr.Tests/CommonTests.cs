using System.Collections;
using Cardmngr.Utils;
using Cardmngr.Extensions;

namespace Cardmngr.Tests;

public class CommonTests
{
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
