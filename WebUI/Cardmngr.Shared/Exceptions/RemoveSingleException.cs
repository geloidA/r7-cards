namespace Cardmngr.Shared.Exceptions;

public class RemoveSingleException(int count) : Exception($"Can't remove single element. Actual count: {count}")
{

}
