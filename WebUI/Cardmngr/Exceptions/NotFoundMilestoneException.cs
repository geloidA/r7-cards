namespace Cardmngr.Exceptions;

public class NotFoundMilestoneException(int id) : Exception($"Can't find milestone by id - {id}")
{

}
