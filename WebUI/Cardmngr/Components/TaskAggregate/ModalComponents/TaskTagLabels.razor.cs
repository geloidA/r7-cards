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
    private IEnumerable<TaskTag> SelectedTags = null!;

    [Parameter] public List<TaskTag> TaskTags { get; set; } = null!;
    [Parameter] public OnlyofficeTask OnlyofficeTask { get; set; } = null!;
    [Parameter] public bool IsAdd { get; set; }

    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        SelectedTags = TaskTags;
    }

    private void OnSearch(OptionsSearchEventArgs<TaskTag> e)
    {
        e.Items = TagColorGetter.Tags?
            .Where(x => x.Name.Contains(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Name);
    }

    private async Task SelectedTagsChanged(IEnumerable<TaskTag> selectedTags)
    {
        var deleted = TaskTags.Except(selectedTags, comparer).ToList();
        var added = selectedTags.Except(TaskTags, comparer);

        if (deleted.Count != 0)
        {
            var deletedTasks = deleted.Select(x => RemoveTag(x));
            await Task.WhenAll(deletedTasks);
        }

        if (added.Any())
        {
            var addedTasks = added.Select(x => AddTag(x));
            await Task.WhenAll(addedTasks);
        }
    }

    private bool CanCreate => !string.IsNullOrEmpty(newTagText) && !TagColorGetter.Contains(newTagText);

    private async Task CreateTag()
    {
        if (CanCreate)
        {
            var created = await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTagText);
            TagColorGetter.GetColor(created);
            TaskTags?.Add(created);
            StateHasChanged();
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        if (!IsAdd)
        {
            await TaskClient.CreateTagAsync(OnlyofficeTask.Id, newTag.Name);
        }

        TaskTags?.Add(newTag);
    }

    private async Task RemoveTag(TaskTag tag)
    {
        if (Disabled) return;
        
        if (!IsAdd)
        {
            await TaskClient.RemoveTagAsync(tag.Id);
        }

        TagColorGetter.RemoveTag(tag);
        TaskTags?.Remove(tag);
    }
}
