using System;
using System.Threading;

public class Worker
{
    private readonly IScheduler _scheduler;
    private readonly CancellationToken _token;
    private readonly Thread _thread;

    public Worker(IScheduler scheduler, CancellationToken token)
    {
        _scheduler = scheduler;
        _token = token;
        _thread = new Thread(Run);
    }

    public void Start()
    {
        _thread.Start();
    }

    public void Join() => _thread.Join();

    private void Run()
    {
        while (!_token.IsCancellationRequested)
        {
            if (_scheduler.HasCommand())
            {
                var cmd = _scheduler.Select();
                if (cmd != null)
                {
                    cmd.Execute();
                }
            }
            else
            {
                Thread.Sleep(50);
            }
        }
    }
}