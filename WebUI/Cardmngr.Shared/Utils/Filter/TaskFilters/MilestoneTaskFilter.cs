using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class MilestoneTaskFilter() : FilterByMultipleItemBase<Milestone, OnlyofficeTask>(FilterType.Exist)
{
    public override bool FilterItem(Milestone filterItem, OnlyofficeTask item)
    {
        return item.MilestoneId == filterItem.Id;
    }
}
