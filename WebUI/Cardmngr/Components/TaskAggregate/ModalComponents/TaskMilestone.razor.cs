using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskMilestone : ComponentBase
{
    private bool _popoverOpen;
    private Milestone? _selectedMilestone;
    private string _searchText = "";
    private IList<Milestone> _searchedMilestones = null!;

    private string CssClass => Disabled ? "" : "hover:underline";

    [CascadingParameter] TaskUpdateData Task { get; set; } = null!;
    [CascadingParameter] IProjectState State { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        RefreshState();
        OnSearch("");
    }

    void OnSearch(string searchText)
    {
        _searchedMilestones = [.. State.Milestones
            .Where(x => x.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) && x.Id != _selectedMilestone?.Id)];
        
        _searchText = searchText;
    }

    void SelectMilestone(Milestone? milestone)
    {
        _selectedMilestone = milestone;
        Task.MilestoneId = milestone?.Id;

        _popoverOpen = false;
        RefreshState();
    }

    private void RefreshState()
    {
        _searchedMilestones = [.. State.Milestones
            .Where(x => x.Title.Contains(_searchText, StringComparison.OrdinalIgnoreCase) && x.Id != _selectedMilestone?.Id)];

        _searchText = "";
    }
}
