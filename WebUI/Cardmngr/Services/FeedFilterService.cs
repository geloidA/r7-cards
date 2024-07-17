using Blazored.LocalStorage;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Services;

public class FeedFilterService(IServiceProvider serviceProvider) : IFeedFilterService
{
    private bool _configured = false;
    private FeedSettings _settings = default!;

    public IEnumerable<string> ExcludedItems => _settings.ExcludedItems;

    public IEnumerable<string> ExcludedProjects => _settings.ExcludedProjects;

    public bool Configured => _configured;

    public async Task ConfigureAsync()
    {
        if (_configured) return;

        using var scope = serviceProvider.CreateScope();
        var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        var settings = await localStorage.GetItemAsync<FeedSettings>("feed-settings").ConfigureAwait(false);

        if (settings is null)
        {
            await localStorage.SetItemAsync("feed-settings", new FeedSettings()).ConfigureAwait(false);
        }
        
        _settings = settings ?? new FeedSettings();

        _configured = true;
    }

    public event Action? FilterChanged;

    public IEnumerable<Feed> Filter(IEnumerable<Feed> feeds) => feeds.Where(Filter);

    public bool Filter(Feed feed)
    {
        if (feed.Value.ExtraLocation is null || feed.Value.Item is null) return false;

        return 
            !_settings.ExcludedProjects.Contains(feed.Value.ExtraLocation) &&
            !_settings.ExcludedItems.Contains(feed.Value.Item);
    }

    public async Task ExcludeProjectAsync(string project)
    {
        if (_settings.ExcludedProjects.Add(project))
        {
            using var scope = serviceProvider.CreateScope();
            var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
            await localStorage.SetItemAsync("feed-settings", _settings).ConfigureAwait(false);
            FilterChanged?.Invoke();
        }
    }

    public async Task ExcludeItemAsync(string item)
    {
        if (_settings.ExcludedItems.Add(item))
        {
            using var scope = serviceProvider.CreateScope();
            var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
            await localStorage.SetItemAsync("feed-settings", _settings).ConfigureAwait(false);
            FilterChanged?.Invoke();
        }
    }

    public async Task IncludeProjectAsync(string project)
    {
        if (_settings.ExcludedProjects.Remove(project))
        {
            using var scope = serviceProvider.CreateScope();
            var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
            await localStorage.SetItemAsync("feed-settings", _settings).ConfigureAwait(false);
            FilterChanged?.Invoke();
        }
    }

    public async Task IncludeItemAsync(string item)
    {
        if (_settings.ExcludedItems.Remove(item))
        {
            using var scope = serviceProvider.CreateScope();
            var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
            await localStorage.SetItemAsync("feed-settings", _settings).ConfigureAwait(false);
            FilterChanged?.Invoke();
        }
    }

    public async Task UpdateSettingsAsync(FeedSettings settings)
    {
        using var scope = serviceProvider.CreateScope();
        var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
        _settings = settings;
        await localStorage.SetItemAsync("feed-settings", _settings).ConfigureAwait(false);
        FilterChanged?.Invoke();
    }
}

public interface IFeedFilterService
{
    event Action FilterChanged;
    IEnumerable<Feed> Filter(IEnumerable<Feed> feeds);
    bool Filter(Feed feed);

    bool Configured { get; }

    Task ConfigureAsync();
}

public class FeedSettings
{
    public HashSet<string> ExcludedProjects { get; set; } = [];
    public HashSet<string> ExcludedItems { get; set; } = [];
}
