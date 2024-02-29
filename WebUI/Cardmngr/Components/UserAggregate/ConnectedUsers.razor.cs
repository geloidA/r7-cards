using AutoMapper;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Components.UserAggregate;

public partial class ConnectedUsers
{
    private bool isCollapsed = true;
    private string MaxWidth => isCollapsed ? "max-width: 39px; min-width: 39px;" : "max-width: 250px; min-width: 250px;";
    private string StyleScroll => isCollapsed ? "overflow: hidden;" : "overflow: auto;";

    private readonly List<UserInfo> userInfos = [];

    [Inject] IMapper Mapper { get; set; } = null!;
    [Inject] IPeopleApi PeopleApi { get; set; } = null!;
    
    [CascadingParameter] ProjectHubClient HubClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        foreach (var id in HubClient.ConnectedMemberIds)
        {
            var profileInfo = await PeopleApi.GetProfileByIdAsync(id);

            userInfos.Add(Mapper.Map<UserInfo>(profileInfo));
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
            var profileInfo = await PeopleApi.GetProfileByIdAsync(args.UserId);

            userInfos.Add(Mapper.Map<UserInfo>(profileInfo));
        }

        StateHasChanged();
    }
}
