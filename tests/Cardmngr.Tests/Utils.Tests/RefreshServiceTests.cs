using Cardmngr.Exceptions;
using Cardmngr.Utils;
using FluentAssertions;

namespace Cardmngr.Tests.Utils.Tests;

public class RefreshServiceTests
{
    [Fact]
    public void Start_ShouldThrowMultipleStartException()
    {
        var refresher = new RefreshService();

        refresher.Start(TimeSpan.FromSeconds(1));

        Assert.Throws<RefreshServiceMultipleStartException>(() => refresher.Start(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public void Start_ShouldChangeEnabled()
    {
        var refresher = new RefreshService();

        refresher.Enabled.Should().BeFalse();

        refresher.Start(TimeSpan.FromSeconds(1));
        
        refresher.Enabled.Should().BeTrue();
    }

    [Fact]
    public void Lock_ShouldChangeEnabled()
    {
        var refresher = new RefreshService();

        refresher.Start(TimeSpan.FromSeconds(1));

        var guid = Guid.NewGuid();

        refresher.Lock(guid);

        refresher.Enabled.Should().BeFalse();
    }

    [Fact]
    public void RemoveLock_ShouldChangeEnabled()
    {
        var refresher = new RefreshService();

        refresher.Start(TimeSpan.FromSeconds(1));

        var guid = Guid.NewGuid();

        refresher.Lock(guid);

        refresher.RemoveLock(guid);

        refresher.Enabled.Should().BeTrue();
    }

    [Fact]
    public void RemoveLock_ShouldHaveNoEffect_WhenGuidNotExists()
    {
        var refresher = new RefreshService();

        refresher.Start(TimeSpan.FromSeconds(1));

        var guid = Guid.NewGuid();

        refresher.Lock(guid);

        refresher.RemoveLock(Guid.NewGuid());

        refresher.Enabled.Should().BeFalse();
    }

    [Fact]
    public void Start_ShouldThrow_WhenDisposed()
    {
        var refresher = new RefreshService();

        refresher.Start(TimeSpan.FromSeconds(1));

        refresher.Dispose();

        Assert.Throws<ObjectDisposedException>(() => refresher.Start(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public async Task Refreshed_ShouldInvokeActions()
    {
        var refresher = new RefreshService();

        var count = 0;

        refresher.Refreshed += () => count++;

        refresher.Start(TimeSpan.FromSeconds(0.5));

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        count.Should().Be(1);
    }

    [Fact]
    public async Task Refreshed_ShouldStop_WhenLocked()
    {
        var refresher = new RefreshService();

        var count = 0;

        refresher.Refreshed += () => count++;

        refresher.Start(TimeSpan.FromSeconds(0.5));

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        count.Should().Be(1);

        refresher.Lock(Guid.NewGuid());

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        count.Should().Be(1);
    }

    [Fact]
    public async Task Refreshed_ShouldContinue_WhenUnlocked()
    {
        var refresher = new RefreshService();

        var count = 0;

        refresher.Refreshed += () => count++;

        refresher.Start(TimeSpan.FromSeconds(0.5));

        var guid = Guid.NewGuid();
        refresher.Lock(guid);

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        refresher.RemoveLock(guid);

        await Task.Delay(TimeSpan.FromSeconds(0.51));

        count.Should().Be(1);
    }
}
