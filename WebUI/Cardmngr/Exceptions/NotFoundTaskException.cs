using Cardmngr.Domain.Entities;

namespace Cardmngr.Exceptions;

public class NotFoundTaskException(OnlyofficeTask task) : Exception($"Can't find task - {task}")
{

}
