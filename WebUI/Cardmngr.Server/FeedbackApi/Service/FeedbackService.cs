using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Server.Exceptions;
using Cardmngr.Shared.Feedbacks;
using Newtonsoft.Json;

namespace Cardmngr.Server.FeedbackApi.Service;

public class FeedbackService : IFeedbackService
{
    private readonly string directory;
    private readonly string developerGuid;
    private readonly string feedbackFile;

    public FeedbackService(IConfiguration conf)
    {
        directory = conf["FeedbackConfig:directory"]!; // not null because it is checked in ConfigureOnlyofficeClient method

        developerGuid = conf["FeedbackConfig:developerGuid"]
            ?? throw new NotConfiguredConfigException("FeedbackConfig:developerGuid");
        
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

        await WriteAllTextAsync(JsonConvert.SerializeObject(feedbacks));

        return created with { CanEdit = true };
    }

    public async Task<Feedback> DeleteFeedbackAsync(Feedback feedback)
    {
        var feedbacks = await GetFeedbacksAsync();

        feedbacks.Remove(feedback);

        await WriteAllTextAsync(JsonConvert.SerializeObject(feedbacks));

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

        await WriteAllTextAsync(JsonConvert.SerializeObject(feedbacks));

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

        await WriteAllTextAsync(JsonConvert.SerializeObject(feedbacks));

        return updated with { CanChangeStatus = true, CanEdit = true };
    }

    public bool CanManipulate(string guid, Feedback feedback)
    {
        if (string.IsNullOrEmpty(guid)) return false;

        return developerGuid == guid || feedback.Creator.Id == guid;
    }

    private async Task WriteAllTextAsync(string json)
    {
        await File.WriteAllTextAsync(feedbackFile, json);
    }

    private async Task<List<Feedback>> GetFeedbacksAsync()
    {
        var json = await File.ReadAllTextAsync(feedbackFile);

        return JsonConvert.DeserializeObject<List<Feedback>>(json)!;
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
