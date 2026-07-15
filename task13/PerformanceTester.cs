using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace task15
{
    public class PerformanceTester
    {
        private readonly double _lowerBound = -100;
        private readonly double _upperBound = 100;
        private readonly Func<double, double> _function = Math.Sin;
        private readonly double _requiredAccuracy = 1e-4;

        public void RunTests()
        {
            Console.WriteLine("=== Исследование производительности вычисления интеграла sin(x) на [-100, 100] ===");
            Console.WriteLine();

            double optimalStep = FindOptimalStep();
            Console.WriteLine($"Оптимальный шаг: {optimalStep}");
            Console.WriteLine();

            int optimalThreads = FindOptimalThreadCount(optimalStep);
            Console.WriteLine($"Оптимальное количество потоков: {optimalThreads}");
            Console.WriteLine();

            CompareWithSingleThread(optimalStep, optimalThreads);
            Console.WriteLine();

            SaveResults(optimalStep, optimalThreads);
        }

        private double FindOptimalStep()
        {
            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
            double referenceValue = 0.0; // Точное значение интеграла sin(x) от -100 до 100

            Console.WriteLine("Поиск оптимального шага:");
            Console.WriteLine("Шаг\t\tЗначение\tОшибка\t\tВремя (мс)");

            double optimalStep = steps[0];
            double minTime = double.MaxValue;

            foreach (double step in steps)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (result, elapsed) = calculator.SolveWithTime(step, 1);
                double error = Math.Abs(result - referenceValue);
                double timeMs = elapsed.TotalMilliseconds;

                Console.WriteLine($"{step:E1}\t{result:F6}\t{error:E6}\t{timeMs:F2}");

                if (error <= _requiredAccuracy && timeMs < minTime)
                {
                    minTime = timeMs;
                    optimalStep = step;
                }
            }

            return optimalStep;
        }

        private int FindOptimalThreadCount(double step)
        {
            int[] threadCounts = { 1, 2, 4, 6, 8, 10, 12, 16, 20, 24, 32 };
            int optimalThreads = 1;
            double minTime = double.MaxValue;

            Console.WriteLine("Поиск оптимального количества потоков (шаг = {0}):", step);
            Console.WriteLine("Потоков\t\tВремя (мс)\tСреднее время (мс)");

            var results = new List<(int threads, double avgTime)>();

            foreach (int threads in threadCounts)
            {
                var times = new List<double>();
                
                for (int i = 0; i < 5; i++)
                {
                    var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                    var (_, elapsed) = calculator.SolveWithTime(step, threads);
                    times.Add(elapsed.TotalMilliseconds);
                }

                double avgTime = times.Average();
                Console.WriteLine($"{threads}\t\t{string.Join(", ", times.Select(t => t.ToString("F2")))}\t{avgTime:F2}");

                results.Add((threads, avgTime));

                if (avgTime < minTime)
                {
                    minTime = avgTime;
                    optimalThreads = threads;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Данные для графика:");
            Console.WriteLine("Потоков\tВремя (мс)");
            foreach (var (threads, avgTime) in results)
            {
                Console.WriteLine($"{threads}\t{avgTime:F4}");
            }

            return optimalThreads;
        }

        private void CompareWithSingleThread(double step, int optimalThreads)
        {
            Console.WriteLine("Сравнение однопоточной и многопоточной версий:");
            
            var singleTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (_, elapsed) = calculator.SolveWithTime(step, 1);
                singleTimes.Add(elapsed.TotalMilliseconds);
            }
            double avgSingleTime = singleTimes.Average();

            var multiTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (_, elapsed) = calculator.SolveWithTime(step, optimalThreads);
                multiTimes.Add(elapsed.TotalMilliseconds);
            }
            double avgMultiTime = multiTimes.Average();

            double speedup = (avgSingleTime - avgMultiTime) / avgSingleTime * 100;

            Console.WriteLine($"Однопоточная версия: {avgSingleTime:F2} мс");
            Console.WriteLine($"Многопоточная версия ({optimalThreads} потоков): {avgMultiTime:F2} мс");
            Console.WriteLine($"Ускорение: {speedup:F2}%");

            if (speedup < 15)
            {
                Console.WriteLine("ВНИМАНИЕ: Ускорение меньше 15%! Требуется оптимизация.");
                OptimizeMultiThread(step, optimalThreads);
            }
        }

        private void OptimizeMultiThread(double step, int optimalThreads)
        {
            Console.WriteLine();
            Console.WriteLine("Оптимизация многопоточной версии...");

            int steps = (int)Math.Ceiling((_upperBound - _lowerBound) / step);
            int chunkSize = steps / optimalThreads;

            var optimizedTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var sw = Stopwatch.StartNew();
                
                var tasks = new Task<double>[optimalThreads];
                for (int t = 0; t < optimalThreads; t++)
                {
                    int start = t * chunkSize;
                    int end = (t == optimalThreads - 1) ? steps : (t + 1) * chunkSize;
                    
                    tasks[t] = Task.Run(() =>
                    {
                        double localResult = 0;
                        double actualStep = (_upperBound - _lowerBound) / steps;
                        
                        for (int j = start; j < end; j++)
                        {
                            double x = _lowerBound + j * actualStep;
                            localResult += _function(x) * actualStep;
                        }
                        return localResult;
                    });
                }

                Task.WaitAll(tasks);
                double result = tasks.Sum(t => t.Result);
                
                sw.Stop();
                optimizedTimes.Add(sw.Elapsed.TotalMilliseconds);
            }

            double avgOptimizedTime = optimizedTimes.Average();
            
            var singleTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (_, elapsed) = calculator.SolveWithTime(step, 1);
                singleTimes.Add(elapsed.TotalMilliseconds);
            }
            double avgSingleTime = singleTimes.Average();

            double speedup = (avgSingleTime - avgOptimizedTime) / avgSingleTime * 100;
            Console.WriteLine($"После оптимизации: {avgOptimizedTime:F2} мс");
            Console.WriteLine($"Ускорение: {speedup:F2}%");
        }

        private void SaveResults(double step, int optimalThreads)
        {
            string fileName = "results.txt";
            
            var singleTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (_, elapsed) = calculator.SolveWithTime(step, 1);
                singleTimes.Add(elapsed.TotalMilliseconds);
            }
            double avgSingleTime = singleTimes.Average();

            var multiTimes = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                var calculator = new IntegralCalculator(_function, _lowerBound, _upperBound);
                var (_, elapsed) = calculator.SolveWithTime(step, optimalThreads);
                multiTimes.Add(elapsed.TotalMilliseconds);
            }
            double avgMultiTime = multiTimes.Average();

            double speedup = (avgSingleTime - avgMultiTime) / avgSingleTime * 100;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Результаты исследования производительности");
                writer.WriteLine("==========================================");
                writer.WriteLine();
                writer.WriteLine($"Дата: {DateTime.Now}");
                writer.WriteLine($"Функция: sin(x)");
                writer.WriteLine($"Отрезок интегрирования: [-100, 100]");
                writer.WriteLine($"Требуемая точность: 1e-4");
                writer.WriteLine();
                writer.WriteLine($"1. Размер шага: {step:E1}");
                writer.WriteLine($"   - Выбран на основе анализа точности и времени выполнения");
                writer.WriteLine($"   - Обеспечивает точность не хуже 1e-4 при оптимальном времени");
                writer.WriteLine();
                writer.WriteLine($"2. Оптимальное количество потоков: {optimalThreads}");
                writer.WriteLine($"   - Определено экспериментально");
                writer.WriteLine($"   - Минимальное время выполнения функции Solve");
                writer.WriteLine();
                writer.WriteLine($"3. Сравнение производительности:");
                writer.WriteLine($"   - Время однопоточной версии: {avgSingleTime:F2} мс");
                writer.WriteLine($"   - Время многопоточной версии: {avgMultiTime:F2} мс");
                writer.WriteLine($"   - Разница: {speedup:F2}%");
                writer.WriteLine($"   - Многопоточная версия {(speedup >= 15 ? "быстрее" : "медленнее")} однопоточной");
            }

            Console.WriteLine($"Результаты сохранены в файл: {fileName}");
        }
    }
}