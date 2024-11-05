using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Cardmngr.Shared.Project;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class SummaryInfoProjectState : ProjectStateComponentBase, IRefreshableProjectState, IDisposable
{
    private int _previousId;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] public int Id { get; set; }

    [Inject] public RefreshService RefreshService { get; set; } = null!;

    [Inject] private ITaskClient TaskClient { get; set; } = null!;

    [Inject] private IProjectClient ProjectClient { get; set; } = null!;

    public event Action<SummaryInfoProjectState>? OnAfterIdChanged;

    protected override void OnInitialized()
    {
        throw new NotImplementedException();
        // base.OnInitialized();
        // RefreshService.Refreshed += () => OnRefreshModelAsync().Forget();
    }

    private async Task OnRefreshModelAsync()
    {
        var tasks = await GetFilteredTasksAsync().ToListAsync().ConfigureAwait(false);
                
        if (tasks.Count == 0)
        {
            SetModel(new ProjectStateDto { Project = await ProjectClient.GetProjectAsync(Id).ConfigureAwait(false) });
        }
        else
        {
            SetModel(await ProjectClient.CollectProjectWithTasksAsync(tasks).ConfigureAwait(false));
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

    public void Dispose()
    {
        RefreshService.Dispose();
    }
}
