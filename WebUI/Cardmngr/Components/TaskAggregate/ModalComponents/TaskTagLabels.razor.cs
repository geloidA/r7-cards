using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskTagLabels : ComponentBase
{
    private bool _popoverOpen;
    private string newTagText = "";
    private List<TaskTag>? _bufferTags;
    private IList<TaskTag> searchedTags = null!;
    private readonly IEqualityComparer<TaskTag> comparer = new TaskTagNameEqualityComparer();

    public List<TaskTag> TaskTags => _bufferTags ?? OnlyofficeTask!.Tags;
    [Parameter] public OnlyofficeTask? OnlyofficeTask { get; set; }

    [Inject] ITagColorManager TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] IToastService ToastService { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        if (OnlyofficeTask is null)
        {
            _bufferTags = [];
        }

        RefreshState();
        OnSearch("");
    }

    private void OnSearch(string searchText)
    {
        searchedTags = [.. TagColorGetter.Tags
            .Except(TaskTags, comparer)
            .Where(x => x.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Name)];
        
        newTagText = searchText;
    }

    private bool CanCreate => !string.IsNullOrEmpty(newTagText) && !TagColorGetter.Contains(newTagText);

    private async Task CreateTag()
    {
        if (CanCreate && OnlyofficeTask != null)
        {
            TaskTag created;

            try
            {
                created = await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTagText);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }

            TagColorGetter.GetColor(created);
            TaskTags.Add(created);
            _popoverOpen = false;
            newTagText = "";

            StateHasChanged();
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        if (OnlyofficeTask != null)
        {
            try
            {
                await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTag.Name);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }
        }

        TaskTags.Add(newTag);

        RefreshState();
        StateHasChanged();
    }

    private async Task RemoveTag(TaskTag tag)
    {
        if (Disabled) return;
        
        if (OnlyofficeTask != null)
        {
            try
            {
                await TaskClient.RemoveTagAsync(tag.Id);
            }
            catch (HttpRequestException ex)
            {
                ToastService.ShowError(ex.Message);
                return;
            }
        }

        TagColorGetter.RemoveTag(tag);
        TaskTags.Remove(tag);

        RefreshState();
    }

    private void RefreshState()
    {
        searchedTags = TagColorGetter.Tags.Except(TaskTags, comparer).ToList();
    }
}
