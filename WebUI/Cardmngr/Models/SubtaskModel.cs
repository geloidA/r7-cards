using Cardmngr.Models.Base;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public class SubtaskModel : SubtaskModelBase
{
    public SubtaskModel(Status status) : base(status)
    {
        
    }

    public SubtaskModel(Subtask subtask) : base(subtask)
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj is not SubtaskModel other)
            return false;
        
        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
