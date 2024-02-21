using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Mvc;

namespace Cardmngr.Server.FeedbackApi;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController(IFeedbackService feedbackService) : Controller
{
    private readonly IFeedbackService feedbackService = feedbackService;

    [HttpGet("all/{requestGuid}")]
    public async IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid)
    {
        await foreach (var feedback in feedbackService.GetFeedbacks(requestGuid))
        {
            yield return feedback;
        }
    }

    [HttpGet("{id}/{requestGuid}")]
    public async Task<ActionResult<Feedback>> GetFeedback(int id, string requestGuid)
    {
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");

        return Ok(feedback with { CanEdit = feedbackService.CanManipulate(requestGuid, feedback) });
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] FeedbackCreateRequestData request)
    {
        if (request.User == null)
        {
            return BadRequest("User is required");
        }

        if (string.IsNullOrEmpty(request.Data.Title))
        {
            return BadRequest("Title is required");
        }

        var created = await feedbackService.CreateFeedbackAsync(request.Data, request.User);

        return Created("", created);
    }

    [HttpPut("{requestGuid}/{id}")]
    public async Task<ActionResult<Feedback>> UpdateFeedback(string requestGuid, int id, [FromBody] FeedbackUpdateData data)
    {
        if (string.IsNullOrEmpty(data.Title))
        {
            return BadRequest("Title is required");
        }

        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");

        if (!feedbackService.CanManipulate(requestGuid, feedback)) return Forbid();
        
        var updated = await feedbackService.UpdateFeedbackAsync(feedback, data, requestGuid);

        return Ok(updated);
    }

    [HttpPut("status/{requestGuid}/{id}")]
    public async Task<ActionResult<Feedback>> UpdateFeedbackStatus(string requestGuid, int id, [FromBody] FeedbackStatus status)
    {
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");

        var updated = await feedbackService.UpdateFeedbackStatusAsync(feedback, status, requestGuid);

        if (updated == null) return Forbid();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Feedback>> DeleteFeedback(int id)
    {
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");
        var deleted = await feedbackService.DeleteFeedbackAsync(feedback);
        return Ok(deleted);
    }
}
