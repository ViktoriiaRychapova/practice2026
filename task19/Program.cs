using System;
using System.Threading;

namespace task19
{
    class Program
    {
        static void Main()
        {
            var scheduler = new Scheduler();

            for(int i = 1; i <= 5; i++)
            {
                scheduler.Add(new TestCommand(i));
            }

            var cts = new CancellationTokenSource();
            var worker = new Worker(scheduler, cts.Token);
            worker.Start();

            for(int i=0; i<3; i++)
            {
                Thread.Sleep(100);
            }

            cts.Cancel();

            worker.Join();

            Console.WriteLine("Работа завершена");
        }
    }
}