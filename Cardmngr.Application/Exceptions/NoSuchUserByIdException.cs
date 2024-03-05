namespace Cardmngr.Application.Exceptions;

public class NoSuchUserByIdException(string id) : Exception("No such user by id: " + id)
{

}
