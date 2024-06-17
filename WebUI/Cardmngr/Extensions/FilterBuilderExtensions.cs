using Onlyoffice.Api.Common;

namespace Cardmngr.Extensions;

public static class FilterBuilderExtensions
{
    public static TaskFilterBuilder DeadlineOutside(this TaskFilterBuilder builder, int days = 0)
    {        
        return builder
            .Status(Status.Open)
            .DeadlineStop(DateTime.Now.AddDays(days));
    }
}
