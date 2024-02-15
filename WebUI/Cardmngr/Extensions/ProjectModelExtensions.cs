using Cardmngr.Models.Interfaces;

namespace Cardmngr.Extensions;

public static class ProjectModelExtensions
{
    public static int TaskCount(this IProjectModel project) => project.StatusBoard
        .Where(x => x.Status == Onlyoffice.Api.Common.Status.Open)
        .Sum(x => x.Count);

    public static int TotalTaskCount(this IProjectModel project) => project.StatusBoard.Sum(x => x.Count);

    public static IEnumerable<ITaskModel> Tasks(this IProjectModel project) => project.StatusBoard.SelectMany(x => x);
}
