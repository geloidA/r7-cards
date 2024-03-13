using BlazorBootstrap;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskTagLabels : ComponentBase
{
    private readonly IEqualityComparer<TaskTag> comparer = new TaskTagNameEqualityComparer();
    private Dropdown dropdown = null!;
    private string newTagText = "";

    [CascadingParameter] List<TaskTag>? TaskTags { get; set; }
    [CascadingParameter] OnlyofficeTask Task { get; set; } = null!;

    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;
    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] ToastService ToastService { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    private IEnumerable<TaskTag> SearchTags => TagColorGetter.Tags
        .Except(TaskTags ?? [], comparer)
        .Where(x => x.Name.Contains(newTagText, StringComparison.CurrentCultureIgnoreCase));

    private void ClearSearchText() => newTagText = "";

    private async Task RemoveTag(TaskTag tag)
    {
        if (!tag.CanEdit) return;
        
        await TaskClient.RemoveTagAsync(tag.Id);
        TagColorGetter.RemoveTag(tag);
        TaskTags?.Remove(tag);
    }

    private async Task CreateTag()
    {
        if (!IsCanAddNewTag())
        {
            return;
        }

        if (!string.IsNullOrEmpty(newTagText) && !TagColorGetter.Contains(newTagText))
        {
            var created = await TaskClient.CreateTagAsync(Task.Id, newTagText);
            TaskTags?.Add(created);
            await dropdown.HideAsync();
        }
    }

    private async Task AddTag(TaskTag newTag)
    {
        if (!IsCanAddNewTag())
        {
            return;
        }

        await TaskClient.CreateTagAsync(Task.Id, newTag.Name);
        
        TaskTags?.Add(newTag);
    }

    private bool IsCanAddNewTag()
    {
        if (TaskTags?.Count > 4)
        {
            ToastService.Notify(new ToastMessage 
            { 
                Title = "Превышено количество меток", 
                Type = ToastType.Warning, 
                IconName = IconName.HandIndex,
                Message = "Нельзя добавить больше 5 меток" 
            });

            return false;
        }

        return true;
    }
}
