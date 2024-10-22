namespace BitzArt.CoreExtensions.Tests;

public class TaskExtensionsTests
{
    [Fact]
    public async Task IgnoreCancellation_WhenTrue_ShouldHandleExceptionByIgnoring()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(10);

        await Task.Delay(100, cts.Token).IgnoreCancellation(true);
    }

    [Fact]
    public async Task IgnoreCancellation_WhenFalse_ShouldDoNothing()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(10);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(()
            => Task.Delay(100, cts.Token).IgnoreCancellation(false));
    }

    [Fact]
    public async Task IgnoreCancellation_WhenTrueAndInnerOperationCancelled_ShouldHandleExceptionByIgnoring()
    {
        await TestMethodAsync().IgnoreCancellation(true);
    }

    [Fact]
    public async Task IgnoreCancellation_WhenFalseAndInnerOperationCancelled_ShouldDoNothing()
    {
        await Assert.ThrowsAnyAsync<OperationCanceledException>(()
            => TestMethodAsync().IgnoreCancellation(false));
    }

    private async Task TestMethodAsync()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(10);

        await Task.Delay(100, cts.Token);
    }
}
