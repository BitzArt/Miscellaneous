namespace BitzArt;

/// <summary>
/// Extensions for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Executes the <see cref="Task"/> and ignores cancellation exceptions.
    /// </summary>
    public static async Task IgnoreCancellationAsync(Task task)
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
}
