using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Cardmngr.Server.FeedbackApi;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController(IFeedbackService feedbackService) : Controller
{
    private readonly IFeedbackService feedbackService = feedbackService;
    private readonly Serilog.ILogger logger = Log.ForContext<FeedbackController>();

    [HttpGet("all/{requestGuid}")]
    public async IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid)
    {
        logger.Information("GetFeedbacks by {requestGuid}", requestGuid);

        await foreach (var feedback in feedbackService.GetFeedbacks(requestGuid))
        {
            yield return feedback;
        }
    }

    [HttpGet("{id}/{requestGuid}")]
    public async Task<ActionResult<Feedback>> GetFeedback(int id, string requestGuid)
    {
        logger.Information("GetFeedback. Id: {id}, RequestGuid: {requestGuid}", id, requestGuid);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) 
        {
            logger.Information("Can't find feedback by id - {id}. RequestGuid: {requestGuid}", id, requestGuid);
            return NotFound($"Can't find feedback by id - {id}");
        }

        return Ok(feedback with { CanEdit = feedbackService.CanManipulate(requestGuid, feedback) });
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] FeedbackCreateRequestData request)
    {
        logger.Information("CreateFeedback. Request: {request}", request);
        if (request.User == null)
        {
            logger.Error("User is required");
            return BadRequest("User is required");
        }

        if (string.IsNullOrEmpty(request.Data.Title))
        {
            logger.Error("Title is required");
            return BadRequest("Title is required");
        }

        var created = await feedbackService.CreateFeedbackAsync(request.Data, request.User);

        return Created("", created);
    }

    [HttpPost("like/{feedbackId}/{requestGuid}")]
    public async Task<ActionResult<Feedback>> ToggleLikeFeedbackAsync(int feedbackId, string requestGuid)
    {
        logger.Information("ToggleLikeFeedback. Id: {feedbackId}", feedbackId);

        var feedback = await feedbackService.FindFeedbackAsync(feedbackId);

        if (feedback == null) 
        {
            logger.Information($"Can't find feedback by id - {feedbackId}");
            return NotFound($"Can't find feedback by id - {feedbackId}");
        }

        var updated = await feedbackService.ToggleFeedbackLikeAsync(feedback, requestGuid);

        return Ok(updated);
    }

    [HttpPut("{requestGuid}/{id}")]
    public async Task<ActionResult<Feedback>> UpdateFeedback(string requestGuid, int id, [FromBody] FeedbackUpdateData data)
    {
        logger.Information("UpdateFeedback. Id: {id}, RequestGuid: {requestGuid}, Data: {data}", requestGuid, id, data);
        if (string.IsNullOrEmpty(data.Title))
        {
            logger.Error("Title is required");
            return BadRequest("Title is required");
        }

        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) 
        {
            logger.Information($"Can't find feedback by id - {id}");
            return NotFound($"Can't find feedback by id - {id}");
        }

        if (!feedbackService.CanManipulate(requestGuid, feedback)) 
        {
            logger.Information("Can't manipulate feedback");
            return Forbid();
        }
        
        var updated = await feedbackService.UpdateFeedbackAsync(feedback, data, requestGuid);

        return Ok(updated);
    }

    [HttpPut("status/{requestGuid}/{id}")]
    public async Task<ActionResult<Feedback>> UpdateFeedbackStatus(string requestGuid, int id, [FromBody] FeedbackStatus status)
    {
        logger.Information("UpdateFeedbackStatus. Id: {id}, RequestGuid: {requestGuid}, Status: {status}", requestGuid, id, status);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) 
        {
            logger.Information($"Can't find feedback by id - {id}");
            return NotFound($"Can't find feedback by id - {id}");
        }

        var updated = await feedbackService.UpdateFeedbackStatusAsync(feedback, status, requestGuid);

        if (updated == null) 
        {
            logger.Information("Can't manipulate feedback");
            return Forbid();
        }

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Feedback>> DeleteFeedback(int id)
    {
        logger.Information("DeleteFeedback. Id: {id}", id);
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) 
        {
            logger.Information($"Can't find feedback by id - {id}");
            return NotFound($"Can't find feedback by id - {id}");
        }
        var deleted = await feedbackService.DeleteFeedbackAsync(feedback);
        return Ok(deleted);
    }
}
