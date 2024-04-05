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

    private FluentAutocomplete<TaskTag> TagList = default!;
    private IEnumerable<TaskTag> SelectedTags = [];

    [CascadingParameter] List<TaskTag>? TaskTags { get; set; }
    [CascadingParameter] OnlyofficeTask Task { get; set; } = null!;

    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        SelectedTags = TaskTags ?? [];
    }

    private void OnSearch(OptionsSearchEventArgs<TaskTag> e)
    {
        e.Items = TaskTags?
            .Where(x => x.Name.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Name);
    }

    private async void SelectedTagsChanged(IEnumerable<TaskTag> selectedTags)
    {
        var deleted = SelectedTags.Except(selectedTags, comparer).SingleOrDefault();
        var added = selectedTags.Except(SelectedTags, comparer).SingleOrDefault();

        if (deleted is { })
            await AddTag(deleted);
        if (added is not null)
            await RemoveTag(added);

        SelectedTags = selectedTags;
    }

    private async Task CreateTag()
    {
        if (!string.IsNullOrEmpty(newTagText) && !TagColorGetter.Contains(newTagText))
        {
            var created = await TaskClient.CreateTagAsync(Task.Id, newTagText);
            TaskTags?.Add(created);
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        await TaskClient.CreateTagAsync(Task.Id, newTag.Name);
        
        TaskTags?.Add(newTag);
    }

    private async Task RemoveTag(TaskTag tag)
    {
        if (!tag.CanEdit) return;
        
        await TaskClient.RemoveTagAsync(tag.Id);
        TagColorGetter.RemoveTag(tag);
        TaskTags?.Remove(tag);
    }
}
