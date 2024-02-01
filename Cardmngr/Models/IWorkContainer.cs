namespace Cardmngr.Models;

public interface IWorkContainer : IWork, IEnumerable<IWork>
{

}

public static class WorkContainerExtensions 
{
    public static int ActiveWorkCount(this IWorkContainer works) => works.Count(x => !x.IsClosed());

    public static int ClosedWorkCount(this IWorkContainer works) => works.Count(x => x.IsClosed());
}
