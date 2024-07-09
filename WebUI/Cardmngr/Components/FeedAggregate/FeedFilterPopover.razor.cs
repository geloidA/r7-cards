using Cardmngr.Services;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedFilterPopover : KolComponentBase
{
    private bool _loading = true;
    private FeedFilterService _feedFilterService = default!;
    private FeedSettings _feedSettings = default!;

    [Inject] IFeedFilterService FeedFilterService { get; set; } = default!;

    [Parameter]
    public EventCallback OnApply { get; set; } = default!;

    protected override void OnInitialized()
    {        
        _feedFilterService = (FeedFilterService)FeedFilterService;

        _feedSettings = new FeedSettings
        {
            ExcludedItems = _feedFilterService.ExcludedItems.ToHashSet(),
            ExcludedProjects = _feedFilterService.ExcludedProjects.ToHashSet()
        };

        _loading = false;
    }

    private void OnTypeCheckChanged(string type, bool isChecked)
    {
        if (isChecked)
        {
            _feedSettings.ExcludedItems.Remove(type);
        }
        else
        {
            _feedSettings.ExcludedItems.Add(type);
        }
    }

    private void ResetSettings()
    {
        _feedSettings = new FeedSettings();

        StateHasChanged();
    }

    private async Task OnApplyClick()
    {
        await _feedFilterService.UpdateSettingsAsync(_feedSettings);
        await OnApply.InvokeAsync();
    }
}