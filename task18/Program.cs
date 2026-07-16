using System;
using System.Threading;

class Program
{
    static void Main()
    {
        var scheduler = new RoundRobinScheduler();

        // добавляем команды
        for (int i=0; i<5; i++)
        {
            scheduler.Add(new LongRunningCommand(20));
        }

        var cts = new System.Threading.CancellationTokenSource();

        var worker = new Worker(scheduler, cts.Token);
        worker.Start();

        Console.WriteLine("Нажмите Enter для остановки");
        Console.ReadLine();

        cts.Cancel();
        worker.Join();

        Console.WriteLine("Работа завершена");
    }
}