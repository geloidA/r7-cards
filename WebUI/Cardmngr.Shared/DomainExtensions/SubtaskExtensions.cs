using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Helpers;

namespace Cardmngr.Shared.DomainExtensions
{
    public static class SubtaskExtensions
    {
        /// <summary>
        /// Compares with updated values and return subtasks, that need to update
        /// </summary>
        /// <param name="updated">Already updated values</param>
        /// <remarks>Use <see cref="SubtaskUpdateEqualityComparer"/> for comparing.</remarks>
        /// <returns></returns>
        public static IEnumerable<Subtask> NeedUpdate(this IEnumerable<Subtask> source, IEnumerable<Subtask> updated)
        {
            var comparer = new SubtaskUpdateEqualityComparer();

            return source
                .Join(updated, 
                    first => first.Id, 
                    second => second.Id, 
                    (first, second) => (first, second))
                .Where(x => !comparer.Equals(x.first, x.second))
                .Select(x => x.second);
        }
    }
}
