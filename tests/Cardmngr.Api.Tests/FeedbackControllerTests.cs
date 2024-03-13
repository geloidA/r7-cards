using Cardmngr.Domain.Feedback;
using Cardmngr.Server.FeedbackApi;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Shared.Feedbacks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Cardmngr.Api.Tests;

public class FeedbackControllerTests
{
    [Fact]
    public async Task GetFeedbacks_ReturnsAllFeedbacks()
    {
        // Arrange
        var feedbackService = new Mock<IFeedbackService>();
        var feedbackList = new List<Feedback>
        {
            new() { Id = 1, Title = "Great work!" },
            new() { Id = 2, Title = "Very helpful" }
        };
        feedbackService.Setup(f => f.GetFeedbacks("asdf")).Returns(feedbackList.ToAsyncEnumerable());
        var controller = new FeedbackController(feedbackService.Object);

        // Act
        var result = await controller.GetFeedbacks("asdf").ToListAsync();

        // Assert
        result.Should().Equal(feedbackList);
    }

    [Fact]
    public async Task GetFeedbacks_ReturnsEmptyList_WhenNoFeedbacks()
    {
        // Arrange
        var feedbackService = new Mock<IFeedbackService>();
        var feedbackList = new List<Feedback>();
        feedbackService.Setup(f => f.GetFeedbacks("sdf")).Returns(feedbackList.ToAsyncEnumerable());
        var controller = new FeedbackController(feedbackService.Object);

        // Act
        var result = await controller.GetFeedbacks("sdf").ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFeedback_ReturnsFeedback_WhenValidIdProvided()
    {
        // Arrange
        var id = 1;
        var feedbackService = new Mock<IFeedbackService>();
        feedbackService.Setup(f => f.FindFeedbackAsync(id)).ReturnsAsync(new Feedback { Id = id, Title = "Great job!" });
        var controller = new FeedbackController(feedbackService.Object);

        // Act
        var result = await controller.GetFeedback(id, "");

        // Assert;
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var feedback = Assert.IsType<Feedback>(okResult.Value);
        feedback.Id.Should().Be(id);
        feedback.Title.Should().Be("Great job!");
    }

    [Fact]
    public async Task GetFeedback_ReturnsNotFound_WhenInvalidIdProvided()
    {
        // Arrange
        var id = 1;
        var feedbackService = new Mock<IFeedbackService>();
        feedbackService.Setup(f => f.FindFeedbackAsync(id)).ReturnsAsync((Feedback)null!);
        var controller = new FeedbackController(feedbackService.Object);

        // Act
        var result = await controller.GetFeedback(id, "");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        notFoundResult.Value.Should().Be($"Can't find feedback by id - {id}");
    }

    // Testing for the following cases:
    // - Valid data provided
    // - Empty title provided
    [Fact]
    public async Task CreateFeedback_ValidData_ReturnsFeedback()
    {
        // Arrange
        var data = new FeedbackCreateRequestData(null!, new FeedbackUpdateData { Title = "Test Title" });
        var mockFeedbackService = new Mock<IFeedbackService>();
        mockFeedbackService.Setup(x => x.CreateFeedbackAsync(data.Data, null!)).ReturnsAsync(new Feedback());

        var controller = new FeedbackController(mockFeedbackService.Object);

        // Act
        var result = await controller.CreateFeedback(data);

        // Assert
        var createdFeedback = Assert.IsType<CreatedResult>(result.Result);
        Assert.IsType<Feedback>(createdFeedback.Value);
    }

    [Fact]
    public async Task CreateFeedback_EmptyTitle_ReturnsBadRequest()
    {
        // Arrange
        var data = new FeedbackCreateRequestData(null!, new FeedbackUpdateData { Title = "" });
        var mockFeedbackService = new Mock<IFeedbackService>();
        var controller = new FeedbackController(mockFeedbackService.Object);

        // Act
        var result = await controller.CreateFeedback(data);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.Equal("Title is required", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateFeedback_TitleEmpty_ReturnsBadRequest()
    {
        // Arrange
        var mockFeedbackService = new Mock<IFeedbackService>();
        var controller = new FeedbackController(mockFeedbackService.Object);

        // Act
        var result = await controller.UpdateFeedback("guid123", 1, new FeedbackUpdateData { Title = "" });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateFeedback_FeedbackNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockFeedbackService = new Mock<IFeedbackService>();
        mockFeedbackService.Setup(x => x.FindFeedbackAsync(999)).ReturnsAsync((Feedback)null!);
        var controller = new FeedbackController(mockFeedbackService.Object);

        // Act
        var result = await controller.UpdateFeedback("guid123", 999, new FeedbackUpdateData { Title = "Updated Title" });

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateFeedback_ValidData_ReturnsOk()
    {
        // Arrange
        var mockFeedbackService = new Mock<IFeedbackService>();
        mockFeedbackService.Setup(x => x.FindFeedbackAsync(1)).ReturnsAsync(new Feedback { Id = 1, Title = "Test Title" });
        var controller = new FeedbackController(mockFeedbackService.Object);

        // Act
        var result = await controller.UpdateFeedback("guid123", 1, new FeedbackUpdateData { Title = "Updated Title" });

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        // Arrange
        var id = 1;
        var feedbackServiceMock = new Mock<IFeedbackService>();
        feedbackServiceMock.Setup(service => service.FindFeedbackAsync(id)).ReturnsAsync((Feedback)null!);
        var controller = new FeedbackController(feedbackServiceMock.Object);

        // Act
        var result = await controller.DeleteFeedback(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Can't find feedback by id - {id}", (result.Result as NotFoundObjectResult)!.Value);
    }

    [Fact]
    public async Task DeleteFeedback_ReturnsOk_WhenFeedbackDeleted()
    {
        // Arrange
        var id = 1;
        var feedback = new Feedback { Id = id };
        var feedbackServiceMock = new Mock<IFeedbackService>();
        feedbackServiceMock.Setup(service => service.FindFeedbackAsync(id)).ReturnsAsync(feedback);
        feedbackServiceMock.Setup(service => service.DeleteFeedbackAsync(feedback)).ReturnsAsync(feedback);
        var controller = new FeedbackController(feedbackServiceMock.Object);

        // Act
        var result = await controller.DeleteFeedback(id);

        // Assert
        var resultOk = Assert.IsType<OkObjectResult>(result.Result);
        resultOk.Value.Should().Be(feedback);
    }
}