using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectMessage : KolComponentBase
{
    private readonly Dictionary<string, FeedMsgType> _msgTypes = FeedMsgTypes.Types.ToDictionary(x => x.Type, x => x);

    [Parameter, EditorRequired]
    public Feed Feed { get; set; } = null!;
}
