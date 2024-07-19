using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Header;

public partial class ProjectNavigatorPopup : ComponentBase
{
    private IEnumerable<Project> searchedProjects = [];

    [Parameter]
    public IEnumerable<Project> Projects { get; set; } = [];
    
    [Parameter]
    public EventCallback OnProjectRefClick { get; set; }

    protected override void OnInitialized()
    {
        searchedProjects = Projects;
    }

    private void SearchTextChanged(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            searchedProjects = Projects;
            return;
        }
        
        searchedProjects = Projects.Where(x => x.Title.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }
}
