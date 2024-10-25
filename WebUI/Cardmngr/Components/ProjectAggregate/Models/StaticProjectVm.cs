using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;

namespace Cardmngr.Components.ProjectAggregate.Models;

public class StaticProjectVm : ProjectStateBase
{
    private bool isTagsInitialized;

    public bool IsCollapsed { get; set; } = true;

    public StaticProjectVm(ProjectStateDto projectState)
    {
        SetModel(projectState);
        Initialized = true;
    }

    public void ToggleCollapsed(ITaskClient taskClient, bool silent = false)
    {
        IsCollapsed = !IsCollapsed;

        Console.WriteLine($"IsCollapsed: {IsCollapsed}");

        if (!IsCollapsed && !isTagsInitialized)
        {
            InitializeTaskTagsAsync(taskClient, silent).Forget();
            isTagsInitialized = true;
        }
    }
}
