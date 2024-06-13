using Cardmngr.Domain;
using Cardmngr.Utils;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class DashboardWatchManager : KolComponentBase
{
    private bool _isPopoverOpen;
    private bool _showSelectionErrorMsg;
    private readonly string _popoverGuid = Guid.NewGuid().ToString();
    private readonly HashSet<int> _selectedProjects = [];

    [Parameter, EditorRequired] public IEnumerable<ProjectInfo> Projects { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    protected void OnSelectionProjectChanged(int projectId, bool isSelected)
    {
        if (isSelected)
        {
            _selectedProjects.Add(projectId);
        }
        else
        {
            _selectedProjects.Remove(projectId);
        }
    }
    
    private bool? AreAllProjectsSelected
    {
        get
        {
            return _selectedProjects.SetEquals(Projects.Select(x => x.Id))
                ? true
                : _selectedProjects.Count == 0
                    ? false
                    : null;
        }
        set
        {
            if (value is true)
            {
                _selectedProjects.UnionWith(Projects.Select(x => x.Id));
            }
            else if (value is false)
            {
                _selectedProjects.Clear();
            }
        }
    }

    private TimeMeasurementUnit MeasurementUnit { get; set; }
    private int ChangeInterval { get; set; } = 15;

    private string _maxUnitValue = "200";
    private string _minUnitValue = "15";
    private const string SecondMinValue = "15";
    private const string SecondMaxValue = "200";
    private const string MinuteMinValue = "1";
    private const string MinuteMaxValue = "30";

    private void ChangeMaxAndMin()
    {
        if (MeasurementUnit == TimeMeasurementUnit.Seconds)
        {
            _maxUnitValue = SecondMaxValue;
            _minUnitValue = SecondMinValue;
        }
        else
        {
            _maxUnitValue = MinuteMaxValue;
            _minUnitValue = MinuteMinValue;
        }
    }

    private void NavigateToDashboard()
    {
        if (_selectedProjects.Count < 2)
        {
            _showSelectionErrorMsg = true;
            return;
        }

        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(
            "/project-dashboards", 
            new Dictionary<string, object?>
            {
                {"measurementUnit", (int)MeasurementUnit },
                {"changeInterval", ChangeInterval },
                {"projects", _selectedProjects.ToArray() }
            }
        ));
    }
}
