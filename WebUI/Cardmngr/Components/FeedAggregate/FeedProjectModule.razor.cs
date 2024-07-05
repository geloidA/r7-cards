using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectModule : KolComponentBase
{
    [Parameter, EditorRequired] 
    public string Name { get; set; } = "";

    [Parameter, EditorRequired]
    public IEnumerable<Feed> Feeds { get; set; } = null!;
}
