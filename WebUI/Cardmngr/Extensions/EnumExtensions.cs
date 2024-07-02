using Cardmngr.Report;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Extensions;

public static class EnumExtensions
{    
    public static Status ToDomainStatus(this TaskStatusType statusType) => statusType switch
    {
        TaskStatusType.Open => Status.Open,
        TaskStatusType.Closed => Status.Closed,
        _ => Status.Unclassified
    };
}
