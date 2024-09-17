using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using KolBlazor;

namespace Cardmngr.Components.Header;

public partial class Toolbar : KolComponentBase, IDisposable
{
    string _r7OfficeHref = "";
    string onlyofficeUrl = "";

    protected override void OnInitialized()
    {
        onlyofficeUrl = Config.CheckKey("onlyoffice-url");
        UpdateHref(null, null);
        NavigationManager.LocationChanged += UpdateHref;
    }

    private void UpdateHref(object? sender, LocationChangedEventArgs? e)
    {
        _r7OfficeHref = NavigationManager.Uri.Contains("/project/board") 
            ? $"{onlyofficeUrl}/Products/Projects/Projects.aspx?prjID={CurrentProjectId}"
            : onlyofficeUrl;

        StateHasChanged();
    }

    int CurrentProjectId => int.Parse(NavigationManager.Uri[(NavigationManager.Uri.LastIndexOf('=') + 1)..]);

    public void Dispose()
    {
        NavigationManager.LocationChanged -= UpdateHref;
    }
}
