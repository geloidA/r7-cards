using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectMessage : KolComponentBase
{
    private IList<UserInfo>? _taskResponsibles;

    private readonly Dictionary<string, FeedMsgType> _msgTypes = FeedMsgTypes.Types.ToDictionary(x => x.Type, x => x);
    [CascadingParameter] private Dictionary<string, UserInfo> FeedUsers { get; set; } = null!;

    [Inject] private ITaskClient TaskClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (Feed.Value.Item == "task")
            {
                var task = await TaskClient
                    .GetAsync(int.Parse(Feed.Value.ItemId))
                    .ConfigureAwait(false);

                _taskResponsibles = task.Responsibles;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }        
    }

    [Parameter, EditorRequired]
    public Feed Feed { get; set; } = null!;

    private RenderFragment RenderMessage => RenderMessageWithInfo(_msgTypes[Feed.Value.Item]);
}
