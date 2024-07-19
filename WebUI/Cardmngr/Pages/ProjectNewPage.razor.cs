using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.People;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;

namespace Cardmngr.Pages;

public partial class ProjectNewPage : ComponentBase
{
    private readonly ProjectCreateDto _project = new();
    private IEnumerable<UserProfile> _users = [];
    private IEnumerable<UserProfile>? selectedResponsible;

    [Inject] private IProjectClient ProjectClient { get; set; } = default!;
    [Inject] private IPeopleClient UserClient { get; set; } = default!;
    
    
    protected override async Task OnInitializedAsync()
    {
        _users = await UserClient.GetUsersAsync().ToListAsync().ConfigureAwait(false);
    }

    public IEnumerable<UserProfile>? SelectedResponsible
    {
        get => selectedResponsible;
        set
        {
            selectedResponsible = value;
            _project.ResponsibleId = selectedResponsible?.SingleOrDefault()?.Id;
        }
    }

    private void CreateProject()
    {
        // var prj = await ProjectClient.CreateProjectAsync(_project).ConfigureAwait(false);
    }
}