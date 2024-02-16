using Cardmngr.Domain.Enums;

namespace Cardmngr.Shared.Extensions;

public class UnknownStatusTypeException(StatusType statusType) : Exception($"Unknown status type: {statusType}")
{

}
