using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Exception;

public class IncompatibleStatusValueException(Status status) : System.Exception($"Incompatible status value: {status} with StatusType")
{
}
