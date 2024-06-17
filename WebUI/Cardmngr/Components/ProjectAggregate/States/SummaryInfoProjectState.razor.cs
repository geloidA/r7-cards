using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Common;

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

    protected override void OnInitialized()
    {
        RefreshService.Refreshed += OnRefreshModelAsync;
    }

    private async void OnRefreshModelAsync()
    {
        var tasks = await GetFilteredTasksAsync().ToListAsync();
                
        if (tasks.Count == 0)
        {
            await SetModelAsync(await ProjectClient.GetProjectAsync(Id));
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
                var tasks = await GetFilteredTasksAsync().ToListAsync();
                
                if (tasks.Count == 0)
                {
                    await SetModelAsync(await ProjectClient.GetProjectAsync(Id));
                }
                else
                {
                    await SetModelAsync(await ProjectClient.CreateProjectWithTasksAsync(tasks));
                }
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
