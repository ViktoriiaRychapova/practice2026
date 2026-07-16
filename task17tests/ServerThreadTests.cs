using NUnit.Framework;
using System.Threading;

[TestFixture]
public class ServerThreadTests
{
    [Test]
    public void HardStop_Should_Immediately_Stop_Thread()
    {
        var server = new ServerThread();

        for (int i = 0; i < 5; i++)
        {
            server.EnqueueCommand(new SampleCommand());
        }

        server.EnqueueCommand(new HardStopCommand(server));

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        server.Wait();
        stopwatch.Stop();

        Assert.Less(stopwatch.ElapsedMilliseconds, 500, "HardStop не сработал мгновенно");
    }

    [Test]
    public void SoftStop_Should_Wait_For_All_Commands()
    {
        var server = new ServerThread();

        for (int i = 0; i < 3; i++)
        {
            server.EnqueueCommand(new SampleCommand());
        }

        server.EnqueueCommand(new SoftStopCommand(server));

        server.EnqueueCommand(new SampleCommand());

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        server.Wait();
        stopwatch.Stop();

        Assert.GreaterOrEqual(stopwatch.ElapsedMilliseconds, 3000, "SoftStop завершился раньше времени");
    }
}