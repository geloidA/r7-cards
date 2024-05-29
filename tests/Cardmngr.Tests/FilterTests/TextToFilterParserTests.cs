using Cardmngr.Components.ProjectAggregate.Filter;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using FluentAssertions;

namespace Cardmngr.Tests.FilterTests;

public class TextToFilterParserTests
{
    private readonly IFilterModel _responsibleFilter;
    private readonly Func<string, IFilter> _defaultFilter;
    private readonly IFilterModel _tagFilter;
    private readonly TextToFilterParser _defaultFilterParser;

    public TextToFilterParserTests()
    {
        _responsibleFilter = new FilterModel("responsible", (text) => new ResponsibleTaskFilter(text));
        _tagFilter = new FilterModel("tag", (text) => new TagTaskFilter(text));
        _defaultFilter = x => new TitleTaskFilter(x);
        _defaultFilterParser = new TextToFilterParser([_responsibleFilter, _tagFilter], _defaultFilter);
    }

    [Fact]
    public void TextToFilterParser_ShouldThrowException_WhenNoFilters()
    {
        Assert.Throws<ArgumentException>(() => new TextToFilterParser([], _defaultFilter));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void TextToFilterParser_ShouldReturnEmpty_WhenTextInvalid(string? text)
    {
        // Arrange
        var textToFilterParser = new TextToFilterParser([_responsibleFilter], _defaultFilter);

        // Act
        var result = textToFilterParser.Parse(text!);

        // Assert
        result.Should().BeEmpty();        
    }

    [Fact]
    public void TextToFilterParser_ShouldSingleFilter_WhenNoFiltersPrefix()
    {
        // Arrange
        var textToFilterParser = new TextToFilterParser([_tagFilter], _defaultFilter);

        // Act
        var result = textToFilterParser.Parse("test");

        // Assert
        result.Should().ContainSingle();
    }

    [Fact]
    public void TextToFilterParser_ShouldReturnOneFilter_WhenOneFilter()
    {
        // Arrange
        var textToFilterParser = new TextToFilterParser([_tagFilter], _defaultFilter);

        // Act
        var result = textToFilterParser.Parse("tag:test");

        // Assert
        result.Should().ContainSingle();
    }

    [Theory]
    [InlineData("tag:test", 1)]
    [InlineData("tag:test ", 1)]
    [InlineData("tag:test df", 2)]
    [InlineData("tag:test tag:test asdf responsible:sdf", 4)]
    [InlineData("tag::test", 1)]
    public void TextToFilterParser_ShouldReturnFilters_WhenMultipleFilters(string text, int expectedCount)
    {
        // Act
        var result = _defaultFilterParser.Parse(text);

        // Assert
        result.Should().HaveCount(expectedCount);
    }
}
