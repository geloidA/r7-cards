using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.MilestoneAggregate.ModalComponents;

public partial class MilestoneResponsible : ComponentBase
{
    private bool _popoverOpen;
    private UserInfo? _selectedResponsible;
    private string _searchText = "";
    private IList<UserInfo> _searchedResponsibles = null!;

    [CascadingParameter] MilestoneUpdateData Milestone { get; set; } = null!;
    [CascadingParameter] IProjectState State { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        _selectedResponsible = Milestone.Responsible is null ? null : State.GetUserById(Milestone.Responsible);
        OnSearch("");
    }

    void OnSearch(string searchText) // TODO: OnSearch and RefreshState should be merged
    {
        _searchedResponsibles = [.. State.Team
            .Where(x => x.DisplayName.Contains(searchText, StringComparison.OrdinalIgnoreCase) && x.Id != _selectedResponsible?.Id)];

        _searchText = searchText;
    }

    void SelectUser(UserInfo? user)
    {
        _selectedResponsible = user;
        Milestone.Responsible = user?.Id;

        _popoverOpen = false;
        RefreshState();
    }

    private void RefreshState()
    {
        _searchedResponsibles = [.. State.Team
            .Where(x => x.DisplayName.Contains(_searchText, StringComparison.OrdinalIgnoreCase) && x.Id != _selectedResponsible?.Id)];

        _searchText = "";
    }
}
