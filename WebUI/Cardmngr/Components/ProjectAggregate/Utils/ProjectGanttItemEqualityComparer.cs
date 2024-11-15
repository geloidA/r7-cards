using System.Diagnostics.CodeAnalysis;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using KolBlazor.Components.Charts.Data;

namespace Cardmngr.Components.ProjectAggregate.Utils;

/// <summary>
/// Not work because gantt items has tree architecture, then top level item doesn't neet refresh 
/// it's not mean that his children too
/// </summary>
/// <param name="stateOf"></param>
public class ProjectGanttItemEqualityComparer(Func<Milestone, IProjectState> stateOf) : IEqualityComparer<GanttChartItem>
{
    public bool Equals(GanttChartItem? x, GanttChartItem? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        var isItemsEqual = x.Equals(y);
        var isDataEqual = IsDataEqual(x.Data, y.Data);

        return isItemsEqual && isDataEqual;
    }

    private bool IsDataEqual(object? dataX, object? dataY)
    {
        if (dataX is null && dataY is null)
            return true;

        if (dataX is null || dataY is null)
            return false;

        if (dataX.GetType() != dataY.GetType())
            return false;

        if (dataX is OnlyofficeTask taskX)
        {
            var taskY = (OnlyofficeTask)dataY;
            var res = taskX.Equals(taskY);
            Console.WriteLine($"{taskX} \n {taskY} \n {res}");
            return res;
        }
        else if (dataX is Milestone milestoneX)
        {
            var milestoneY = (Milestone)dataY;
            var state = stateOf(milestoneX);

            return milestoneX.Equals(milestoneY) 
                && state.GetMilestoneTasks(milestoneX)
                        .ScrambledEquals(state.GetMilestoneTasks(milestoneY));
        }
        else if (dataX is IProjectState stateX)
        {
            var stateY = (IProjectState)dataY;
            return stateX.Project.Equals(stateY.Project);
        }

        throw new NotSupportedException($"Not supported data type - {dataX.GetType().Name}");
    }

    public int GetHashCode([DisallowNull] GanttChartItem obj)
    {
        return HashCode.Combine(obj.GetHashCode(), obj.Data?.GetHashCode());
    }
}
