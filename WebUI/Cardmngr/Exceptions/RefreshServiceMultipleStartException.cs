using Cardmngr.Utils;

namespace Cardmngr.Exceptions;

public class RefreshServiceMultipleStartException() : Exception("Refresh service is already started")
{

}
