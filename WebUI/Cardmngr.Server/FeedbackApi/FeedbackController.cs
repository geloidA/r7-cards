using Cardmngr.Domain.Feedback;
using Cardmngr.Server.FeedbackApi.Service;
using Cardmngr.Server.UserInfoService;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Mvc;

namespace Cardmngr.Server.FeedbackApi;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController(IFeedbackService feedbackService, IUserInfoService userInfoService) : Controller
{
    private readonly IFeedbackService feedbackService = feedbackService;
    private readonly IUserInfoService userInfoService = userInfoService;

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
        var user = await userInfoService.GetUserInfoAsync(requestGuid);

        if (user == null)
        {
            return Forbid();
        }

        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");

        return Ok(feedback with { CanEdit = feedbackService.CanManipulate(requestGuid, feedback) });
    }

    [HttpPost("{requestGuid}")]
    public async Task<ActionResult<Feedback>> CreateFeedback(string requestGuid, [FromBody] FeedbackUpdateData data)
    {
        if (string.IsNullOrEmpty(data.Title))
        {
            return BadRequest("Title is required");
        }

        throw new Exception(
            HttpContext.Request.Cookies
            .Select(c => $"{c.Key} - {c.Value}")
            .Aggregate((x, y) => $"{x}\n{y}"));

        var user = await userInfoService.GetUserInfoAsync(requestGuid);

        if (user == null)
        {
            return BadRequest("Invalid request guid");
        }

        var created = await feedbackService.CreateFeedbackAsync(data, user);

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

    [HttpDelete("{id}")]
    public async Task<ActionResult<Feedback>> DeleteFeedback(int id)
    {
        var feedback = await feedbackService.FindFeedbackAsync(id);
        if (feedback == null) return NotFound($"Can't find feedback by id - {id}");
        var deleted = await feedbackService.DeleteFeedbackAsync(feedback);
        return Ok(deleted);
    }
}
