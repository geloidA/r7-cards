using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Microsoft.JSInterop;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class ProjectSummaryInfo : ComponentBase, IDisposable
{
    private const int ScrollDuration = 20000;
    private ElementReference _deadlineoutTasksRef;
    private ElementReference _deadlineoutSoonTasksRef;

    private ICollection<OnlyofficeTask> _deadlineoutTasks = null!;
    private ICollection<OnlyofficeTask> _deadlineoutSoonTasks = null!;
    private IRefresheableProjectState? _refreshableState;

    [CascadingParameter] IProjectState State { get; set; } = null!;

    [Inject] IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter] public EventCallback CanSwitch { get; set; }

    protected override void OnInitialized()
    {
        if (State is IRefresheableProjectState refresheable)
        {
            _refreshableState = refresheable;
            _refreshableState.RefreshService.Refreshed += RefreshState;
        }
        
        RefreshState();
    }

    private DotNetObjectReference<ProjectSummaryInfo>? _dotNetObjectReference;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetObjectReference = DotNetObjectReference.Create(this);

            await JSRuntime.InvokeVoidAsync("scrollToBottom", 
                _deadlineoutTasksRef, 
                ScrollDuration,
                _dotNetObjectReference);

            await JSRuntime.InvokeVoidAsync("scrollToBottom", 
                _deadlineoutSoonTasksRef, 
                ScrollDuration,
                _dotNetObjectReference);
        }
    }

    private int _invokeCounter = 0;

    [JSInvokable]
    public void ScrollFinished()
    {
        _invokeCounter++;
        if (_invokeCounter == 2)
        {
            CanSwitch.InvokeAsync();
        }
    }

    private void RefreshState()
    {
        _deadlineoutTasks = [.. State.Tasks.Where(x => x.IsDeadlineOut())];
        _deadlineoutSoonTasks = [.. State.Tasks.Where(x => !x.IsDeadlineOut() && x.IsSevenDaysDeadlineOut())];
        StateHasChanged();
    }

    public void Dispose()
    {
        _dotNetObjectReference?.Dispose();
    }
}
