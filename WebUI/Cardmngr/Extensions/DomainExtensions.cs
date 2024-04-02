using Cardmngr.Domain.Enums;

namespace Cardmngr.Extensions;

public static class DomainExtensions
{
    public static Onlyoffice.Api.Common.Status ToStatus(this StatusType statusType) => (Onlyoffice.Api.Common.Status)statusType;
}
