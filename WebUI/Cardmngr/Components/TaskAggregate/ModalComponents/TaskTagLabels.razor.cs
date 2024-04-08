using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskTagLabels : ComponentBase
{
    private readonly IEqualityComparer<TaskTag> comparer = new TaskTagNameEqualityComparer();
    private string newTagText = "";
    private IEnumerable<TaskTag> SelectedTags = [];

    [CascadingParameter] List<TaskTag>? TaskTags { get; set; }
    [CascadingParameter] OnlyofficeTask OnlyofficeTask { get; set; } = null!;

    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        SelectedTags = TaskTags ?? [];
    }

    private void OnSearch(OptionsSearchEventArgs<TaskTag> e)
    {
        e.Items = TagColorGetter.Tags?
            .Where(x => x.Name.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Name);
    }

    private async Task SelectedTagsChanged(IEnumerable<TaskTag> selectedTags)
    {
        var deleted = SelectedTags.Except(selectedTags, comparer);
        var added = selectedTags.Except(SelectedTags, comparer);

        if (deleted is { })
        {
            var deletedTasks = deleted.Select(x => RemoveTag(x));
            await Task.WhenAll(deletedTasks);
        }

        if (added is { })
        {
            var addedTasks = added.Select(x => AddTag(x));
            await Task.WhenAll(addedTasks);
        }

        SelectedTags = selectedTags;
    }

    private async Task CreateTag()
    {
        if (!string.IsNullOrEmpty(newTagText) && !TagColorGetter.Contains(newTagText))
        {
            var created = await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTagText);
            TaskTags?.Add(created);
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTag.Name);
        TaskTags?.Add(newTag);
    }

    private async Task RemoveTag(TaskTag tag)
    {
        if (Disabled) return;
        
        await TaskClient.RemoveTagAsync(tag.Id);
        TagColorGetter.RemoveTag(tag);
        TaskTags?.Remove(tag);
    }
}
