using Cardmngr.Extensions;
using Cardmngr.Models.Base;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class MilestoneModel : MilestoneModelBase<MilestoneDto>
{
    public MilestoneModel(MilestoneDto milestone, IProjectModel project) : base(milestone, project) 
    {
        
    }

    private MilestoneModel(MilestoneModel source) : base(source.Project)
    {
        Id = source.Id;
        Title = source.Title;
        Description = source.Description;
        Deadline = source.Deadline;
        IsKey = source.IsKey;
        IsNotify = source.IsNotify;
        Status = source.Status;
        Responsible = source.Responsible != null ? new User(source.Responsible) : null;
        Created = source.Created;
        CreatedBy = source.CreatedBy != null ? new User(source.CreatedBy) : null;;
        Updated = source.Updated;
        CanEdit = source.CanEdit;
        CanDelete = source.CanDelete;
    }

    public override IEditableModel<MilestoneDto> EditableModel => new MilestoneModel(this);

    public override void Update(MilestoneDto source)
    {
        Status = source.Status.ToMilestoneStatus();
        base.Update(source);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not MilestoneModel milestone)
            return false;
        return milestone.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    protected override Status GetStatusFromApiModel(MilestoneDto apiModel) => apiModel.Status.ToMilestoneStatus();
}
