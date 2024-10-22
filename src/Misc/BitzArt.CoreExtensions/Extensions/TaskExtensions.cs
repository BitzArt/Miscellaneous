namespace BitzArt;

/// <summary>
/// Extension methods for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Awaits the <paramref name="task"/> until it is completed or cancelled, while ignoring cancellation exceptions.
    /// </summary>
    /// <param name="task">The task to await.</param>
    /// <param name="ignoreCancellation">Whether to ignore cancellation exceptions.</param>
    /// <param name="byTaskStatus">Determines whether to check for task cancellation by task status (if <see langword="true"/>) or by <see cref="OperationCanceledException"/> (if <see langword="false"/>).</param>
    public static Task IgnoreCancellation(this Task task, bool ignoreCancellation = true, bool byTaskStatus = true)
    {
        if (!ignoreCancellation) return task;

        return byTaskStatus
            ? IgnoreCancellationByTaskCancelledAsync(task)
            : IgnoreCancellationByExceptionAsync(task);
    }

    private static async Task IgnoreCancellationByTaskCancelledAsync(Task task)
    {
        try
        {
            await task;
        }
        catch
        {
            if (task.IsCanceled) return;
            throw;
        }
    }

    private static async Task IgnoreCancellationByExceptionAsync(Task task)
    {
        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch
        {
            throw;
        }
    }
}
