using System;
using System.Threading;

namespace task14
{
    public class DefiniteIntegral
    {
        private static object lockObj = new object();

        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            double totalResult = 0.0;
            Barrier barrier = new Barrier(threadsNumber);
            Thread[] threads = new Thread[threadsNumber];

            double range = b - a;
            double segmentSize = range / threadsNumber;

            for (int i = 0; i < threadsNumber; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() =>
                {
                    double localA = a + threadIndex * segmentSize;
                    double localB = localA + segmentSize;

                    double localSum = 0.0;

                    for (double x = localA; x < localB; x += step)
                    {
                        double fx = function(x);
                        double fxNext = function(x + step);
                        localSum += (fx + fxNext) * 0.5 * step;
                    }

                    lock (lockObj)
                    {
                        totalResult += localSum;
                    }

                    barrier.SignalAndWait();
                });
                threads[i].IsBackground = true;
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return totalResult;
        }
    }
}