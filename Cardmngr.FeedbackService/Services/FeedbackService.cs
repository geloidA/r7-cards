using Cardmngr.Domain.Entities;
using Cardmngr.FeedbackService.Exceptions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Shared.Feedbacks;
using Cardmngr.Domain.Enums;
using System.Text.Json;

namespace Cardmngr.FeedbackService.Services;

public class FeedbackService : IFeedbackService
{
    private readonly string directory;
    private readonly string developerGuid;
    private readonly string feedbackFile;

    public FeedbackService(IConfiguration conf)
    {
        directory = conf.CheckKey("FeedbackConfig:directory"); // not null because it is checked in ConfigureOnlyofficeClient method

        developerGuid = conf.CheckKey("FeedbackConfig:developerGuid");

        feedbackFile = $"{directory}/feedbacks.json";
    }

    public async Task<Feedback> CreateFeedbackAsync(FeedbackUpdateData data, UserInfo user)
    {
        var feedbacks = await GetFeedbacksAsync();

        var created = new Feedback
        {
            Id = await IncrementCounterAsync(),
            Title = data.Title,
            Description = data.Description,
            Creator = user,
            Created = DateTime.Now
        };

        feedbacks.Add(created);

        await WriteAllTextAsync(JsonSerializer.Serialize(feedbacks));

        return created with { CanEdit = true };
    }

    public async Task<Feedback> DeleteFeedbackAsync(Feedback feedback)
    {
        var feedbacks = await GetFeedbacksAsync();

        feedbacks.Remove(feedback);

        await WriteAllTextAsync(JsonSerializer.Serialize(feedbacks));

        return feedback;
    }

    public async Task<Feedback?> FindFeedbackAsync(int id)
    {
        var feedbacks = await GetFeedbacksAsync();
        return feedbacks.FirstOrDefault(x => x.Id == id);
    }

    public async IAsyncEnumerable<Feedback> GetFeedbacks(string requestGuid)
    {
        var feedbacks = await GetFeedbacksAsync();

        foreach (var feedback in feedbacks)
        {
            yield return feedback with
            {
                CanEdit = CanManipulate(requestGuid, feedback),
                CanChangeStatus = developerGuid == requestGuid // TODO: can be hacked
            };
        }
    }

    public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback, FeedbackUpdateData data, string requestGuid)
    {
        var feedbacks = await GetFeedbacksAsync();

        if (!feedbacks.Remove(feedback))
        {
            throw new FeedbackNotFoundException(feedback);
        }

        var nextFeedback = feedback with { Title = data.Title, Description = data.Description };

        feedbacks.Add(nextFeedback);

        await WriteAllTextAsync(JsonSerializer.Serialize(feedbacks));

        return nextFeedback with { CanEdit = true, CanChangeStatus = developerGuid == requestGuid };
    }

    public async Task<Feedback?> UpdateFeedbackStatusAsync(Feedback feedback, FeedbackStatus status, string requestGuid)
    {
        if (requestGuid != developerGuid) return null;

        var feedbacks = await GetFeedbacksAsync();

        if (!feedbacks.Remove(feedback))
        {
            throw new FeedbackNotFoundException(feedback);
        }

        var updated = feedback with
        {
            Status = status,
            Finished = status == FeedbackStatus.Finished ? DateTime.Now : null
        };
        feedbacks.Add(updated);

        await WriteAllTextAsync(JsonSerializer.Serialize(feedbacks));

        return updated with { CanChangeStatus = true, CanEdit = true };
    }

    public bool CanManipulate(string guid, Feedback feedback)
    {
        if (string.IsNullOrEmpty(guid)) return false;

        return developerGuid == guid || feedback.Creator.Id == guid;
    }

    public async Task<Feedback?> ToggleFeedbackLikeAsync(Feedback feedback, string requestGuid)
    {
        return await ToggleFeedbackAsync(feedback, requestGuid, ToggleLike);

        void ToggleLike(Feedback f)
        {
            if (!f.LikedUsers.Remove(requestGuid))
            {
                f.LikedUsers.Add(requestGuid);
                f.DislikedUsers.Remove(requestGuid);
            }
        }
    }

    public async Task<Feedback?> ToggleFeedbackDislikeAsync(Feedback feedback, string requestGuid)
    {
        return await ToggleFeedbackAsync(feedback, requestGuid, ToggleDislike);

        void ToggleDislike(Feedback f)
        {
            if (!f.DislikedUsers.Remove(requestGuid))
            {
                f.DislikedUsers.Add(requestGuid);
                f.LikedUsers.Remove(requestGuid);
            }
        }
    }

    private async Task<Feedback?> ToggleFeedbackAsync(Feedback feedback, string requestGuid, Action<Feedback> toggleAction)
    {
        var feedbacks = await GetFeedbacksAsync();

        if (!feedbacks.Remove(feedback))
        {
            throw new FeedbackNotFoundException(feedback);
        }

        toggleAction(feedback);

        feedbacks.Add(feedback);

        await WriteAllTextAsync(JsonSerializer.Serialize(feedbacks));

        return feedback with
        {
            CanEdit = CanManipulate(requestGuid, feedback),
            CanChangeStatus = requestGuid == developerGuid
        };
    }

    private async Task WriteAllTextAsync(string json)
    {
        await File.WriteAllTextAsync(feedbackFile, json);
    }

    private async Task<List<Feedback>> GetFeedbacksAsync()
    {
        var json = await File.ReadAllTextAsync(feedbackFile);

        return JsonSerializer.Deserialize<List<Feedback>>(json) ?? [];
    }

    private async Task<int> IncrementCounterAsync()
    {
        var counterFilePath = $"{directory}/counter";

        var text = await File.ReadAllTextAsync($"{directory}/counter");
        var counter = int.Parse(text);
        await File.WriteAllTextAsync(counterFilePath, (counter + 1).ToString());
        return counter;
    }
}
