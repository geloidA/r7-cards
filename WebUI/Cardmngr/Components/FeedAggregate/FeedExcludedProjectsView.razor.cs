using Cardmngr.Services;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedExcludedProjectsView : KolComponentBase
{
    [Parameter] public HashSet<string> ExcludedProjects { get; set; } = default!;
    [Parameter] public FeedFilterService FeedFilterService { get; set; } = default!;

    private void IncludeProject(string name) => ExcludedProjects.Remove(name);
}
