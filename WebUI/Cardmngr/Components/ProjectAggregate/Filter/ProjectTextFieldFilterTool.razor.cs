using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Components.ProjectAggregate.Filter;

public partial class ProjectTextFieldFilterTool : ComponentBase
{
    private TextToFilterParser _textToFilterParser = null!;
    private FluentTextField _fluentSearchComponent = null!;
    private bool _popupVisible = false;
    private string _inputValue = string.Empty;

    [CascadingParameter] private IFilterableProjectState State { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private readonly IList<IFilterModel> _filters = 
    [
        new FilterModel("тег", (text) => new TagTaskFilter(text), "bug, plot, mappa"),
        new FilterModel("ответственный", (text) => new ResponsibleTaskFilter(text), "Иван Иваныч"),
        new FilterModel("автор", (text) => new CreatedByTaskFilter(text), "Вася Пупкин")
    ];

    protected override void OnInitialized()
    {
        _filters.Add(new FilterModel("мои", 
            _ => new UserRelatedFilter(AuthenticationStateProvider.ToCookieProvider().UserId), 
            "Связанные c вами задачи"));

        _textToFilterParser = new TextToFilterParser(_filters, x => new TitleTaskFilter(x));
    }

    private IEnumerable<IFilter<OnlyofficeTask>> previousFilters = [];

    private void ParseInput()
    {
        State.TaskFilter.RemoveFilters(previousFilters);

        State.TaskFilter.AddFilters(previousFilters = _textToFilterParser
            .Parse(_inputValue)
            .Cast<IFilter<OnlyofficeTask>>());
    }

    private void OnFilterTipClicked(IFilterModel filterModel)
    {
        _inputValue += $"{filterModel.Prefix}:";
        _fluentSearchComponent.FocusAsync();
        ParseInput();
    }
}
