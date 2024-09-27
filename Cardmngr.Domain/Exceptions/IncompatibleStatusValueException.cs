using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Exceptions;

public class IncompatibleStatusValueException(Status status) : Exception($"Incompatible status value: {status} with StatusType")
{
}
