using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Components.ProjectAggregate.States;

public partial class SummaryInfoProjectState : ProjectStateBase, IRefresheableProjectState, IDisposable
{
    private int _previousId;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] public int Id { get; set; }

    [Inject] public RefreshService RefreshService { get; set; } = null!;

    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;

    public event Action<SummaryInfoProjectState>? OnAfterIdChanged;

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += () => OnRefreshModelAsync().Forget();
    }

    private async Task OnRefreshModelAsync()
    {
        var tasks = await GetFilteredTasksAsync().ToListAsync();
                
        if (tasks.Count == 0)
        {
            await SetModelAsync(new ProjectStateDto { Project = await ProjectClient.GetProjectAsync(Id) });
        }
        else
        {
            await SetModelAsync(await ProjectClient.CreateProjectWithTasksAsync(tasks));
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (_previousId != Id)
        {
            try
            {
                await OnRefreshModelAsync();

                OnAfterIdChanged?.Invoke(this);
            }            
            catch (OperationCanceledException) 
            {
                return;
            }

            _previousId = Id;
        }

        Initialized = true;
    }

    private IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync()
    {        
        return TaskClient.GetEntitiesAsync(TaskFilterBuilder.Instance
            .ProjectId(Id)
            .DeadlineOutside(7));
    }

    public override void Dispose()
    {
        RefreshService.Dispose();
        base.Dispose();
    }
}
