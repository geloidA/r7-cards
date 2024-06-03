using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Exception;

namespace Cardmngr.Domain.Extensions;

public static class EnumExtensions
{
    public static StatusType ToStatusType(this Status status)
    {
        return status switch
        {
            Status.Open => StatusType.Open,
            Status.Closed => StatusType.Close,
            _ => throw new IncompatibleStatusValueException(status)
        };
    }

    public static Status ToStatus(this StatusType status) => (Status)status;

    public static Status Invert(this Status status) => status == Status.Open ? Status.Closed : Status.Open;
}
