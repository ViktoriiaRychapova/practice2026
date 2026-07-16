using System;
using Xunit;
using System.Threading;
using task18;

public class SchedulerTests
{
    [Fact]
    public void Scheduler_Should_Follow_RoundRobin_Order()
    {
        var scheduler = new RoundRobinScheduler();
        var callOrder = new System.Collections.Generic.List<string>();

        var cmd1 = new TestCommand("cmd1", callOrder);
        var cmd2 = new TestCommand("cmd2", callOrder);
        var cmd3 = new TestCommand("cmd3", callOrder);

        scheduler.Add(cmd1);
        scheduler.Add(cmd2);
        scheduler.Add(cmd3);

        for (int i = 0; i < 6; i++)
        {
            var cmd = scheduler.Select() as TestCommand;
            cmd?.Execute();
        }

        string[] expectedOrder = { "cmd1", "cmd2", "cmd3", "cmd1", "cmd2", "cmd3" };
        Assert.Equal(expectedOrder, callOrder);
    }

    [Fact]
    public void LongRunningCommand_Should_Execute_Multiple_Times_Until_Finished()
    {
        var cmd = new LongRunningCommand(5);
        int executions = 0;

        while (!cmd.IsFinished())
        {
            cmd.Execute();
            executions++;
        }

        Assert.True(cmd.IsFinished());
        Assert.Equal(5, executions);
    }

    [Fact]
    public void Planner_Should_Run_LongRunningCommand_Correctly()
    {
        var scheduler = new RoundRobinScheduler();
        var command1 = new LongRunningCommand(3);
        var command2 = new LongRunningCommand(2);
        scheduler.Add(command1);
        scheduler.Add(command2);

        int totalExecutions = 0;

        // Act
        while (!command1.IsFinished() || !command2.IsFinished())
        {
            var cmd = scheduler.Select() as LongRunningCommand;
            cmd?.Execute();
            totalExecutions++;
        }

        // Assert
        Assert.True(command1.IsFinished());
        Assert.True(command2.IsFinished());
        Assert.Equal(5, totalExecutions);
    }

    [Fact]
    public void Worker_Should_Process_Diverse_LongCommands()
    {
        var scheduler = new RoundRobinScheduler();

        scheduler.Add(new LongRunningCommand(2));
        scheduler.Add(new LongRunningCommand(4));
        scheduler.Add(new LongRunningCommand(3));

        var cts = new CancellationTokenSource();
        var worker = new task18.Worker(scheduler, cts.Token);
        worker.Start();

        Thread.Sleep(1000); 
        cts.Cancel();
        worker.Join();

        Assert.All(scheduler, cmd => Assert.True(((LongRunningCommand)cmd).IsFinished()));
    }
}

public class TestCommand : ICommand
{
    private readonly string _name;
    private readonly System.Collections.Generic.List<string> _callOrder;

    public TestCommand(string name, System.Collections.Generic.List<string> callOrder)
    {
        _name = name;
        _callOrder = callOrder;
    }

    public void Execute()
    {
        _callOrder.Add(_name);
    }

    public bool IsFinished() => true;
}