namespace Cardmngr.Extensions;

public static class TaskExtensions
{

    public static async void AndForget(this Task task)
    {
        await task;
    }
}
