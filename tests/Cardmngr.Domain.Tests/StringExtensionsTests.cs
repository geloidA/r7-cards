using Cardmngr.Domain.Extensions;
using FluentAssertions;

namespace Cardmngr.Domain.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData("<p>#bob</p>", "bob")]
    [InlineData("<p>#bob </p>", "bob")]
    [InlineData("<p>#bob jinjerik</p>", "bob")]
    [InlineData("<p>#bob#k\n</p>", "bob#k")]
    [InlineData("<p>#sdfg</p>", "sdfg")]
    [InlineData("<p>#bob#k\t</p>", "bob#k")]
    public void TryParseToTagName_ShouldReturnCorrentTag(string commentText, string expectedName)
    {
        if (commentText.TryParseToTagName(out var tagName))
        {
            tagName.Should().Be(expectedName);
        }
        else
        {
            tagName.Should().BeNull();
        }
    }

    [Theory]
    [InlineData("<p>bob</p>")]
    [InlineData("<p>#</p>")]
    [InlineData("#")]
    [InlineData("")]
    [InlineData(null)]
    public void TryParseToTagName_ShouldBeFalse_WhenWrong(string? wrongText)
    {
        wrongText.TryParseToTagName(out _).Should().BeFalse();
    }
}