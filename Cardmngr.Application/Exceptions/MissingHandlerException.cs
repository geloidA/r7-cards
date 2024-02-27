namespace Cardmngr.Application.Exceptions;

public class MissingHandlerException(params string[] handlers) 
    : Exception("One or more handlers are missing: " + string.Join(", ", handlers))
{

}
