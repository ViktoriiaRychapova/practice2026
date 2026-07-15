using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace task13
{
    public class IntegralCalculator
    {
        private readonly Func<double, double> _function;
        private readonly double _lowerBound;
        private readonly double _upperBound;

        public IntegralCalculator(Func<double, double> function, double lowerBound, double upperBound)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
            _lowerBound = lowerBound;
            _upperBound = upperBound;
        }

        public double Solve(double step, int threadCount = 1)
        {
            if (step <= 0)
                throw new ArgumentException("Step must be positive", nameof(step));
            if (threadCount <= 0)
                throw new ArgumentException("Thread count must be positive", nameof(threadCount));

            int steps = (int)Math.Ceiling((_upperBound - _lowerBound) / step);
            double actualStep = (_upperBound - _lowerBound) / steps;
            
            double result = 0;
            object lockObject = new object();

            if (threadCount == 1)
            {
                for (int i = 0; i < steps; i++)
                {
                    double x = _lowerBound + i * actualStep;
                    result += _function(x) * actualStep;
                }
            }
            else
            {
                Parallel.For(0, steps, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, () => 0.0,
                    (i, state, localResult) =>
                    {
                        double x = _lowerBound + i * actualStep;
                        localResult += _function(x) * actualStep;
                        return localResult;
                    },
                    localResult =>
                    {
                        lock (lockObject)
                        {
                            result += localResult;
                        }
                    });
            }

            return result;
        }

        public (double result, TimeSpan elapsedTime) SolveWithTime(double step, int threadCount = 1)
        {
            var sw = Stopwatch.StartNew();
            double result = Solve(step, threadCount);
            sw.Stop();
            return (result, sw.Elapsed);
        }
    }
}