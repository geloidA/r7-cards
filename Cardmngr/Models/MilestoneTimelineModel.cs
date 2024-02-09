using System.Collections;
using Cardmngr.Models.EventArgs;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

internal class MilestoneTimelineModel(IEnumerable<Milestone> milestones, IProjectModel project) : IObservableCollection<IMilestoneModel>
{
    private readonly Dictionary<IMilestoneModel, bool> timelineItems = milestones
        .Select(m => new KeyValuePair<IMilestoneModel, bool>(new MilestoneModel(m, project), false))
        .ToDictionary();

    public void ToggleMilestone(IMilestoneModel milestone)
    {
        if (!timelineItems.TryGetValue(milestone, out bool value)) throw new InvalidOperationException("Milestone does not exist");

        timelineItems[milestone] = !value;

        OnSelectedMilestonesChanged();
    }

    public IEnumerable<IMilestoneModel> SelectedMilestones => timelineItems.Where(x => x.Value).Select(x => x.Key);

    public bool IsSelected(IMilestoneModel milestone) => timelineItems[milestone];

    public int Count => timelineItems.Count;
    
    public event Action? SelectedMilestonesChanged;
    public event Action<CollectionEventArgs<IMilestoneModel>>? CollectionChanged;
    
    private void OnCollectionChanged(IMilestoneModel item, CollectionAction action) 
        => CollectionChanged?.Invoke(new CollectionEventArgs<IMilestoneModel>(this, item, action));

    private void OnSelectedMilestonesChanged() => SelectedMilestonesChanged?.Invoke();


    public IEnumerator<IMilestoneModel> GetEnumerator() 
    {
        foreach (var item in timelineItems.Keys)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(IMilestoneModel item)
    {
        timelineItems[item] = false;
        OnCollectionChanged(item, CollectionAction.Add);
    }

    public bool Remove(IMilestoneModel item)
    {
        if (timelineItems.Remove(item))
        {
            OnCollectionChanged(item, CollectionAction.Remove);
            return true;
        }
        return false;
    }
}
