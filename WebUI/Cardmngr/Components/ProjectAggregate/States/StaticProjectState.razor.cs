﻿using Cardmngr.Application.Clients;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.States;

public sealed partial class StaticProjectState : ProjectStateBase, IProjectState
{
    private string CssHeight => ViewModel?.IsCollapsed ?? true 
        ? "height: auto;"
        : "height: 650px;";

    private Icon CollapsedIcon => ViewModel?.IsCollapsed ?? true
        ? new Icons.Regular.Size16.ChevronDown()
        : new Icons.Regular.Size16.ChevronUp();

    [Parameter] public StaticProjectVm? ViewModel { get; set; }
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] AppProjectsInfoService ProjectSummaryService { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        toggleCollapsedFunc = ToggleCollapsed;
        if (ViewModel is { IsCollapsed: false })
        {
            await SetModelAsync(await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks));
            Initialized = true;
        }

        isFollow = ProjectSummaryService.IsFollow(ViewModel?.ProjectInfo.Id ?? -1);

        SummaryService.OnProjectsChanged += ToggleIfSelected;
    }

    private async void ToggleIfSelected()
    {
        await Task.Delay(5); // wait for correct toggling
        if (ViewModel is { } && SummaryService.FilterManager.ProjectId == ViewModel.ProjectInfo.Id)
        {
            await ToggleCollapsed();
            StateHasChanged();
        }
    }

    private Func<Task> toggleCollapsedFunc = null!;

    private bool isTagsInitialized;

    private async Task ToggleCollapsed()
    {
        if (ViewModel is { })
        {
            ViewModel.IsCollapsed = !ViewModel.IsCollapsed;

            if (!ViewModel.IsCollapsed)
            {
                Initialized = false;
                var model = await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks);
                await SetModelAsync(model, !isTagsInitialized);
                Initialized = true;

                isTagsInitialized = true;
            }
        }
    }

    private bool isFollow;

    private Icon IconFollow => isFollow ? new Icons.Filled.Size16.Star() : new Icons.Regular.Size16.Star();

    public async Task ToggleFollowAsync()
    {
        if (ViewModel?.ProjectInfo is { })
        {
            await ProjectClient.FollowProjectAsync(ViewModel.ProjectInfo.Id);
            
            if (isFollow = !isFollow)
            {
                ProjectSummaryService.Follow(ViewModel.ProjectInfo.Id);
            }
            else
            {
                ProjectSummaryService.Unfollow(ViewModel.ProjectInfo.Id);
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        SummaryService.OnProjectsChanged -= ToggleIfSelected;
    }
}