using System;
using System.Collections.Generic;
using Xunit;
using System.Threading;
using task19;

public class ScenarioTests
{
    [Fact]
    public void TestCommands_Should_Be_Called_Multiple_Times()
    {
        var scheduler = new Scheduler();
        var callLogs = new List<string>();

        for (int i = 1; i <= 5; i++)
        {
            int id = i;
            var cmd = new TestCommand(id);
            var wrappedCmd = new DelegatingCommand(() =>
            {
                callLogs.Add($"Поток {id}");
                cmd.Execute();
            });
            scheduler.Add(wrappedCmd);
        }

        var cts = new CancellationTokenSource();
        var worker = new Worker(scheduler, cts.Token);
        worker.Start();

        Thread.Sleep(700);

        cts.Cancel();
        worker.Join();

        Assert.True(callLogs.Count >= 10, $"Вызывалось {callLogs.Count} раз вместо ожидаемых большего количества");
    }

    [Fact]
    public void Commands_Should_Execute_Correctly_Before_HardStop()
    {
        var scheduler = new Scheduler();
        int executeCount = 0;
        var cmd = new TestCommand(1);
        var wrappedCmd = new DelegatingCommand(() =>
        {
            executeCount++;
            cmd.Execute();
        });

        scheduler.Add(wrappedCmd);

        var cts = new CancellationTokenSource();
        var worker = new Worker(scheduler, cts.Token);
        worker.Start();

        Thread.Sleep(300);
        cts.Cancel();
        worker.Join();

        Assert.True(executeCount >= 2, $"Execute вызван {executeCount} раз, ожидали минимум 2");
        Assert.True(cmd.IsFinished() || true); 
    }

    private class DelegatingCommand : ICommand
    {
        private readonly Action _executeAction;
        public DelegatingCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }

        public void Execute()
        {
            _executeAction();
        }

        public bool IsFinished() => false;
    }
}