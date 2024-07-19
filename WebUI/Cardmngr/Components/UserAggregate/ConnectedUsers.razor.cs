using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.People;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.UserAggregate;

public partial class ConnectedUsers : ComponentBase
{
    private bool isCollapsed = true;
    private string MaxWidth => isCollapsed ? "max-width: 39px; min-width: 39px;" : "max-width: 250px; min-width: 250px;";
    private string StyleScroll => isCollapsed ? "overflow: hidden;" : "overflow: auto;";

    private readonly List<UserInfo> userInfos = [];

    [Inject] private IPeopleClient UserClient { get; set; } = null!;
    
    [CascadingParameter] ProjectHubClient HubClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        foreach (var id in HubClient.ConnectedMemberIds)
        {
            userInfos.Add(await UserClient.GetUserProfileByIdAsync(id));
        }

        HubClient.ConnectedMembersChanged += async args => await OnMembersChangedAsync(args);
    }

    private async Task OnMembersChangedAsync(MembersChangedEventArgs args)
    {
        if (args.Action == MemberAction.Leave)
        {
            userInfos.RemoveAll(x => x.Id == args.UserId);
        }
        else
        {
            userInfos.Add(await UserClient.GetUserProfileByIdAsync(args.UserId));
        }

        StateHasChanged();
    }
}
