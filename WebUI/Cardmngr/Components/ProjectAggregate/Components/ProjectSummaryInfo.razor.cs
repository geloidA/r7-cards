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
    private IRefreshableProjectState? _refreshableState;

    [CascadingParameter] private IProjectState State { get; set; } = null!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public EventCallback CanSwitch { get; set; }

    protected override void OnInitialized()
    {
        if (State is IRefreshableProjectState refreshable)
        {
            _refreshableState = refreshable;
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

            await JsRuntime.InvokeVoidAsync("scrollToBottom", 
                _deadlineoutTasksRef, 
                ScrollDuration,
                _dotNetObjectReference).ConfigureAwait(false);

            await JsRuntime.InvokeVoidAsync("scrollToBottom", 
                _deadlineoutSoonTasksRef, 
                ScrollDuration,
                _dotNetObjectReference).ConfigureAwait(false);
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
