using Cardmngr.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cardmngr.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController(IConfiguration config) : ControllerBase
{
    private readonly IConfiguration config = config;

    [HttpPost]
    public async Task<IActionResult> PostFeedback([FromBody] Feedback feedback)
    {
        var directory = config["FeedbackDirectory"] ?? throw new NullReferenceException("FeedbackDirectory config is null");

        if (feedback.Text.Length > 300) return BadRequest("Too long feedback");

        var filename = $"{feedback.UserName}_{feedback.CreatedAt}.txt";
        var filePath = Path.GetFullPath(Path.Combine(directory, filename));

        if (!Directory.Exists(directory))
        {
            return BadRequest("Directory not found");
        }

        await System.IO.File.WriteAllTextAsync(filePath, GetFeedbackString(feedback));

        return Ok();
    }

    private static string GetFeedbackString(Feedback feedback)
    {
        return string.Format("User: {0}\tTime: {1}\nText: {2}", feedback.UserName, feedback.CreatedAt, feedback.Text);
    }
}
