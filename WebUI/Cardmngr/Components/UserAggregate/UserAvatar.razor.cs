﻿using Cardmngr.Application.Clients;
using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.UserAggregate;

public partial class UserAvatar : KolComponentBase
{
    [Inject] IUserClient UserClient { get; set; } = null!;

    [Parameter] public UserInfo? User { get; set; }
    [Parameter] public bool ShowTooltip { get; set; }
    [Parameter] public bool ShowName { get; set; }
    [Parameter] public int Size { get; set; } = 32;
    [Parameter] public string? UserId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (User is null && UserId is null)
            throw new ArgumentException($"You need initialize {nameof(User)} or {nameof(UserId)}");

        if (UserId is { })
        {
            User = await UserClient.GetUserProfileByIdAsync(UserId);
        }
    }
}