using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Domain.Enums;
using Cardmngr.FeedbackService.Services;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Cardmngr.FeedbackService.Extensions;

public static class FeedbackApi
{
    private static readonly Serilog.ILogger logger = Log.ForContext(typeof(FeedbackApi));

    public static IEndpointRouteBuilder MapFeedbackApi(this IEndpointRouteBuilder app)
    {
        var feedbacks = app.MapGroup("/api/feedback");
        feedbacks.MapGet("/all/{requestGuid}", GetFeedbacks);
        feedbacks.MapGet("/{id}/{requestGuid}", GetFeedback);

        feedbacks.MapPost("/", CreateFeedback);
        feedbacks.MapPost("/like/{id}/{requestGuid}", LikeFeedbackAsync);
        feedbacks.MapPost("/dislike/{id}/{requestGuid}", DislikeFeedbackAsync);

        feedbacks.MapPut("/{requestGuid}/{id}", UpdateFeedbackAsync);
        feedbacks.MapPut("/status/{requestGuid}/{id}", UpdateFeedbackStatusAsync);

        feedbacks.MapDelete("/{id}", DeleteFeedbackAsync);

        return app;
    }

    public static IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid, IFeedbackService feedbackService)
    {
        logger.Information("GetFeedbacks by {requestGuid}", requestGuid);

        return feedbackService.GetFeedbacks(requestGuid);
    }

    public static async Task<IResult> GetFeedback(
        int id, 
        string requestGuid,
        IFeedbackService feedbackService)
    {
        logger.Information("GetFeedback. Id: {id}, RequestGuid: {requestGuid}", id, requestGuid);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null)
        {
            logger.Information("Can't find feedback by id - {id}. RequestGuid: {requestGuid}", id, requestGuid);
            return Results.NotFound($"Can't find feedback by id - {id}");
        }

        return Results.Ok(feedback with { CanEdit = feedbackService.CanManipulate(requestGuid, feedback) });
    }

    public static async Task<IResult> CreateFeedback(
        [FromBody] FeedbackCreateRequestData request,
        IFeedbackService feedbackService)
    {
        logger.Information("CreateFeedback. Request: {request}", request);
        if (request.User == null)
        {
            logger.Error("User is required");
            return Results.BadRequest("User is required");
        }

        if (string.IsNullOrEmpty(request.Data.Title))
        {
            logger.Error("Title is required");
            return Results.BadRequest("Title is required");
        }

        var created = await feedbackService.CreateFeedbackAsync(request.Data, request.User);

        return Results.Created("", created);
    }

    public static async Task<IResult> LikeFeedbackAsync(
        int id, 
        string requestGuid,
        IFeedbackService feedbackService)
    {
        logger.Information("ToggleLikeFeedback. Id: {feedbackId}", id);

        var feedback = await feedbackService.FindFeedbackAsync(id);

        if (feedback == null)
        {
            logger.Information($"Can't find feedback by id - {id}");
            return Results.NotFound($"Can't find feedback by id - {id}");
        }

        var updated = await feedbackService.ToggleFeedbackLikeAsync(feedback, requestGuid);

        return Results.Ok(updated);
    }

    public static async Task<IResult> DislikeFeedbackAsync(
        int id, 
        string requestGuid,
        IFeedbackService feedbackService)
    {
        logger.Information("ToggleDislikeFeedback. Id: {feedbackId}", id);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null)
        {
            logger.Information($"Can't find feedback by id - {id}");
            return Results.NotFound($"Can't find feedback by id - {id}");
        }

        var updated = await feedbackService.ToggleFeedbackDislikeAsync(feedback, requestGuid);

        return Results.Ok(updated);
    }

    public static async Task<IResult> UpdateFeedbackAsync(
        string requestGuid, 
        int id, 
        [FromBody] FeedbackUpdateData data,
        IFeedbackService feedbackService)
    {
        logger.Information("UpdateFeedback. Id: {id}, RequestGuid: {requestGuid}, Data: {data}", requestGuid, id, data);
        if (string.IsNullOrEmpty(data.Title))
        {
            logger.Error("Title is required");
            return Results.BadRequest("Title is required");
        }

        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null)
        {
            logger.Information($"Can't find feedback by id - {id}");
            return Results.NotFound($"Can't find feedback by id - {id}");
        }

        if (!feedbackService.CanManipulate(requestGuid, feedback))
        {
            logger.Information("Can't manipulate feedback");
            return Results.Forbid();
        }

        var updated = await feedbackService.UpdateFeedbackAsync(feedback, data, requestGuid);

        return Results.Ok(updated);
    }

    public static async Task<IResult> UpdateFeedbackStatusAsync(
        string requestGuid, 
        int id, 
        [FromBody] FeedbackStatus status,
        IFeedbackService feedbackService)
    {
        logger.Information("UpdateFeedbackStatus. Id: {id}, RequestGuid: {requestGuid}, Status: {status}", requestGuid, id, status);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null)
        {
            logger.Information($"Can't find feedback by id - {id}");
            return Results.NotFound($"Can't find feedback by id - {id}");
        }

        var updated = await feedbackService.UpdateFeedbackStatusAsync(feedback, status, requestGuid);

        if (updated == null)
        {
            logger.Information("Can't manipulate feedback");
            return Results.Forbid();
        }

        return Results.Ok(updated);
    }

    public static async Task<IResult> DeleteFeedbackAsync(int id, IFeedbackService feedbackService)
    {
        logger.Information("DeleteFeedback. Id: {id}", id);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null)
        {
            logger.Information($"Can't find feedback by id - {id}");
            return Results.NotFound($"Can't find feedback by id - {id}");
        }
        var deleted = await feedbackService.DeleteFeedbackAsync(feedback);
        return Results.Ok(deleted);
    }
}
