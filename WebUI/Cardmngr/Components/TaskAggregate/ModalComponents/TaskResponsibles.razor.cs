using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskResponsibles : ComponentBase
{
    private string _searchText = "";
    private IList<UserInfo> _selectedResponsibles = [];
    private IList<UserInfo> _searchResponsibles = [];

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] TaskUpdateData Task { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        RefreshState();
        OnSearch("");
    }

    private void OnSearch(string searchText)
    {
        _searchResponsibles = [.. State.Team
            .ExceptBy(_selectedResponsibles.Select(x => x.Id), x => x.Id)
            .Where(x => x.DisplayName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.DisplayName)];
        
        _searchText = searchText;
    }

    private void SelectUser(UserInfo user)
    {
        _selectedResponsibles.Add(user);
        Task.Responsibles = _selectedResponsibles.Select(x => x.Id).ToList();

        OnSearch(_searchText);
    }

    private void RemoveUser(UserInfo user)
    {
        _selectedResponsibles.Remove(user);
        Task.Responsibles = _selectedResponsibles.Select(x => x.Id).ToList();

        OnSearch(_searchText);
    }

    private void RefreshState()
    {
        _selectedResponsibles = State.Team.Where(x => Task.Responsibles.Any(id => id == x.Id))
            .Cast<UserInfo>()
            .ToList();
    }
}
