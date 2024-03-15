using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class ProjectTaskFilter() : FilterByMultipleItemBase<Domain.Entities.Project, OnlyofficeTask>(FilterType.Exist)
{
    public override bool FilterItem(Domain.Entities.Project filterItem, OnlyofficeTask item)
    {
        return item.ProjectOwner.Id == filterItem.Id;
    }
}
