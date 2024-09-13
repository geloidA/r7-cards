using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskTagLabels : ComponentBase
{
    private bool _popoverOpen;
    private string _popoverGuid = null!;
    private string newTagText = "";    
    private IList<TaskTag> searchedTags = null!;
    private readonly IEqualityComparer<TaskTag> comparer = new TaskTagNameEqualityComparer();

    [Parameter] public List<TaskTag> TaskTags { get; set; } = null!;
    [Parameter] public OnlyofficeTask OnlyofficeTask { get; set; } = null!;
    [Parameter] public bool IsAdd { get; set; }

    [Inject] ITagColorManager TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        searchedTags = TagColorGetter.Tags.Except(TaskTags, comparer).ToList();
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
        if (CanCreate)
        {
            var created = await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTagText);
            TagColorGetter.GetColor(created);
            TaskTags?.Add(created);
            _popoverOpen = false;

            StateHasChanged();
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        if (!IsAdd)
        {
            await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTag.Name);
        }

        TaskTags.Add(newTag);

        searchedTags = TagColorGetter.Tags.Except(TaskTags, comparer).ToList();
        StateHasChanged();
    }

    private async Task RemoveTag(TaskTag tag)
    {
        if (Disabled) return;
        
        if (!IsAdd)
        {
            await TaskClient.RemoveTagAsync(tag.Id);
        }

        TagColorGetter.RemoveTag(tag);
        TaskTags.Remove(tag);

        searchedTags = TagColorGetter.Tags.Except(TaskTags, comparer).ToList();
    }

    private void OpenPopover()
    {
        if (OnlyofficeTask?.CanDelete ?? false) // TODO: 
        {
            _popoverOpen = true;
        }
    }

    private void OnOpenChanged(bool open)
    {
        _popoverOpen = open;

        if (!open)
        {
            newTagText = "";
        }
    }
}
