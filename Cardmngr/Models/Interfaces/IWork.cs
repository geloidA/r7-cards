namespace Cardmngr.Models.Interfaces;

public interface IWork
{
    DateTime? StartDate { get; }
    DateTime? Deadline { get; }
    bool IsClosed();
}

public static class WorkExtensions
{
    public static bool IsDateBetween(this IWork work, DateTime date)
    {
        return date >= work.StartDate && date <= work.Deadline;
    }

    public static bool IsDeadlineOut(this IWork work) => !work.IsClosed() && DateTime.Now > work.Deadline;
}