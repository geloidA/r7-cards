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

public sealed partial class SummaryInfoProjectState : ProjectStateBase, IRefreshableProjectState
{
    private int _previousId;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] public int Id { get; set; }

    [Inject] public RefreshService RefreshService { get; set; } = null!;

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;
    [Inject] private ITaskClient TaskClient { get; set; } = null!;

    public event Action<SummaryInfoProjectState>? OnAfterIdChanged;

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += () => OnRefreshModelAsync().Forget();
    }

    private async Task OnRefreshModelAsync()
    {
        var tasks = await GetFilteredTasksAsync().ToListAsync().ConfigureAwait(false);
                
        if (tasks.Count == 0)
        {
            await SetModelAsync(new ProjectStateDto { Project = await ProjectClient.GetProjectAsync(Id).ConfigureAwait(false) }).ConfigureAwait(false);
        }
        else
        {
            await SetModelAsync(await ProjectClient.CollectProjectWithTasksAsync(tasks).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        Initialized = false;

        if (_previousId != Id)
        {
            try
            {
                await OnRefreshModelAsync().ConfigureAwait(false);

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
