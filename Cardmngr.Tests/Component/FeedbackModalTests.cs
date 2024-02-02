using System.Security.Claims;
using BlazorBootstrap;
using Blazored.Modal;
using Bunit;
using Bunit.TestDoubles;
using Cardmngr.Components.Modals;
using Cardmngr.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Cardmngr.Tests;

public class FeedbackModalTests : TestContext
{
    private readonly Feedback feedbackPlug = new("", "");

    private readonly Mock<IFeedbackService> feedbackServiceMock = new();
    private readonly Mock<ToastService> toastServiceMock = new();

    public FeedbackModalTests()
    {
        Services
            .AddScoped(_ => feedbackServiceMock.Object)
            .AddScoped(_ => toastServiceMock.Object)
            .AddBlazoredModal()
            .AddBlazorBootstrap();
    }

    [Fact]
    public void SendFeedback_ShouldPreventSendingEmptyFeedback()
    {
        var cut = RenderComponent<FeedbackModal>();

        var sendBtn = cut.FindComponent<Button>().Find("button");

        sendBtn.Click();

        feedbackServiceMock.Verify(x => x.SendAsync(feedbackPlug), Times.Never());        
    }

    [Fact]
    public void SendFeedback_ShouldShowInvalidCssClass_WhenFeedbackIsInvalid()
    {
        var cut = RenderComponent<FeedbackModal>();

        var sendBtn = cut.FindComponent<Button>().Find("button");

        sendBtn.Click();

        Assert.Contains("h-text-area", cut.Find("textarea").ClassName);               
    }

    [Fact]
    public void Textarea_ShouldBindedToTextProperty()
    {
        var cut = RenderComponent<FeedbackModal>();

        var textarea = cut.Find("textarea");
        var text = new string('a', 20);
        textarea.Input(text);
        
        Assert.Equal(text, cut.Instance.Text);
    }

    [Fact]
    public void Textarea_ShouldPreventTooLongText()
    {
        var cut = RenderComponent<FeedbackModal>();

        var textarea = cut.Find("textarea");
        var text = new string('a', 1000);
        textarea.Input(text);

        Assert.Null(cut.Instance.Text);
    }
}
