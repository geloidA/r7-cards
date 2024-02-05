using Cardmngr.Models;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Extensions;

public static class CookieStateProviderExtensions
{
    public static bool IsCanEdit(this CookieStateProvider provider, TaskModel task) 
    {
        var userId = provider["UserId"];

        return bool.Parse(provider["IsAdmin"]) ||
            task.CreatedBy?.Id == userId || 
            task.Responsibles.Any(x => x.Id == userId);
    }

    public static bool IsCanEdit(this CookieStateProvider provider, MilestoneModel task) 
    {
        var userId = provider["UserId"];

        return bool.Parse(provider["IsAdmin"]) ||
            task.CreatedBy?.Id == userId || 
            task.Responsible?.Id == userId;
    }
}
