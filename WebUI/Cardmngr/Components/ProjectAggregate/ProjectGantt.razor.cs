using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.MilestoneAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.Contracts;
using Cardmngr.Components.ProjectAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Utils;
using Cardmngr.Shared.Extensions;
using KolBlazor.Components.Charts.Data;
using KolBlazor.Components.Charts.Gantt;
using Microsoft.AspNetCore.Components;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class ProjectGantt : ComponentBase, IDisposable
{
    private GanttChart _chart = null!;
    private GanttDetalizationLevel _detalizationLevel = GanttDetalizationLevel.Quarter;

    [CascadingParameter] private IProjectState? State { get; set; }
    [CascadingParameter] private IProjectStateFinder? StateFinder { get; set; }

    [CascadingParameter(Name = "DetailsModal")]
    private ModalOptions DetailsModal { get; set; } = null!;

    [CascadingParameter] private IModalService Modal { get; set; } = null!;

    [Parameter] public bool Multiple { get; set; }
    [Parameter] public bool HighlightRoot { get; set; }
    [Parameter] public EventCallback<GanttChartItem> ItemExpandToggled { get; set; }
    [Parameter] public GanttItemsCreator ItemsCreator { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (Multiple)
        {
            if (StateFinder is null)
            {
                throw new InvalidOperationException("StateFinder is not set");
            }

            StateFinder.StateChanged += RefreshState;
        }
        else
        {
            if (State is null)
            {
                throw new InvalidOperationException("State is not set");
            }

            State.EventBus.SubscribeTo<StateChanged>(RefreshState);

            if (State is IFilterableProjectState filterableState)
            {
                filterableState.TaskFilter.FilterChanged += () => RefreshState(new StateChanged { State = State });
            }
        }
    }

    private string GetItemClass(GanttChartItem item)
    {
        if (item.Data is OnlyofficeTask task)
        {
            if (task.IsClosed())
            {
                var state = GetState(task);
                var defaultClose = state.Statuses.First(x => x.IsDefault && x.StatusType == StatusType.Close);
                return task.HasStatus(defaultClose) ? "gantt-task-custom-done" : "";
            }
            else if (task.IsDeadlineOut())
            {
                return "gantt-task-custom-deadline";
            }
            else if (task.IsSevenDaysDeadlineOut())
            {
                return "gantt-task-custom-warning";
            }
        }

        return "";
    }

    private IProjectState GetState(OnlyofficeTask task)
    {
        return Multiple ? StateFinder!.Find(task) : State!;
    }

    private IProjectState GetState(Milestone milestone)
    {
        return Multiple ? StateFinder!.Find(milestone) : State!;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Refresh();
        }
    }

    private IEnumerable<GanttChartItem> GetItems()
    {
        var states = Multiple
            ? StateFinder!.States
            : [State!];

        return states.Select(ItemsCreator.Create);
    }

    private GanttChartItem GetItem(string key)
    {
        var (type, id) = GanttItemsCreator.GetItemIdByKey(key);
        
        if (type == typeof(IProjectState))
        {
            return Multiple 
                ? ItemsCreator.Create(StateFinder!.States.First(x => x.Project.Id == id))
                : ItemsCreator.Create(State!);
        }

        throw new NotImplementedException();
    }

    private void RefreshState(StateChanged args)
    {
        _chart.RefreshItem(GanttItemsCreator.GetItemKey(args.State));
    }

    public void Refresh()
    {
        _chart.RefreshItems();
    }

    private void ScrollToToday()
    {
        _chart.ScrollTo(DateTime.Today);
    }

    private void OnDetalizationLevelChanged(GanttDetalizationLevel level)
    {
        _detalizationLevel = level;
    }

    private static string GetDetalizationLevelText(GanttDetalizationLevel level)
    {
        return level switch
        {
            GanttDetalizationLevel.Week => "Неделя",
            GanttDetalizationLevel.Month => "Месяц",
            GanttDetalizationLevel.Quarter => "Квартал",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private Task OnItemClicked(GanttChartItem item)
    {
        return item.Data switch
        {
            OnlyofficeTask task => OpenDetailsTaskModal(task),
            IProjectState state => OpenDetailsProjectModal(state),
            Milestone milestone => OpenDetailsMilestoneModal(milestone),
            _ => Task.CompletedTask 
        };
    }

    private async Task OpenDetailsTaskModal(OnlyofficeTask task)
    {
        await Modal.Show<TaskDetailsModal>(
            new ModalParameters
            {
                { "Model", task },
                { "State",  GetState(task) }
            },
            DetailsModal).Result;
    }

    private async Task OpenDetailsMilestoneModal(Milestone milestone)
    {
        await Modal.Show<MilestoneDetailsModal>(
            new ModalParameters
            {
                { "Model", milestone },
                { "State", GetState(milestone) }
            }, 
            DetailsModal).Result;
    }

    private async Task OpenDetailsProjectModal(IProjectState state)
    {
        await Modal.Show<ProjectDetailsModal>(
            new ModalParameters { { "State", state } },
            DetailsModal).Result;
    }

    public void Dispose()
    {
        if (Multiple)
        {
            StateFinder!.StateChanged -= RefreshState;
        }
        else
        {
            State!.EventBus.UnSubscribeFrom<StateChanged>(RefreshState);
        }
    }
}
