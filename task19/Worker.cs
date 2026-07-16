using System;
using System.Threading;

namespace task19
{
    public class Worker
    {
        private Scheduler _scheduler;
        private CancellationToken _token;
        private Thread _thread;

        public Worker(Scheduler scheduler, CancellationToken token)
        {
            _scheduler = scheduler;
            _token = token;
            _thread = new Thread(Run);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Join()
        {
            _thread.Join();
        }

        public void Run()
        {
            while (!_token.IsCancellationRequested)
            {
                if (_scheduler.HasCommands())
                {
                    var cmd = _scheduler.GetNext();
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
}