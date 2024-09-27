using Cardmngr.Application.Clients.Project;
using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.UserAggregate;

public partial class UserProjectsView : KolComponentBase
{
    [Inject] private IProjectClient ProjectClient { get; set; } = default!;
    
    [Parameter] public List<Project>? UserProjects { get; set; }
    [Parameter] public int MaxHeight { get; set; }
}