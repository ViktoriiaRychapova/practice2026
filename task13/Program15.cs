using System;

namespace task15
{
    class Program
    {
        static void Main(string[] args)
        {
            var tester = new PerformanceTester();
            tester.RunTests();

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}