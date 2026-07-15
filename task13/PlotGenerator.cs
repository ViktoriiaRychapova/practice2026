using System;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;

namespace task15
{
    public class PlotGenerator
    {
        public void GeneratePerformancePlot(double step)
        {
            int[] threadCounts = { 1, 2, 4, 6, 8, 10, 12, 16, 20, 24, 32 };
            var avgTimes = new List<double>();

            foreach (int threads in threadCounts)
            {
                var times = new List<double>();
                for (int i = 0; i < 5; i++)
                {
                    var calculator = new IntegralCalculator(Math.Sin, -100, 100);
                    var (_, elapsed) = calculator.SolveWithTime(step, threads);
                    times.Add(elapsed.TotalMilliseconds);
                }
                avgTimes.Add(times.Average());
            }

            var plt = new Plot(800, 600);
            plt.Title("Зависимость времени вычисления от количества потоков");
            plt.XLabel("Количество потоков");
            plt.YLabel("Время выполнения (мс)");
            
            plt.AddScatter(
                threadCounts.Select(t => (double)t).ToArray(),
                avgTimes.ToArray(),
                lineWidth: 2,
                markerSize: 8
            );

            int optimalIndex = avgTimes.IndexOf(avgTimes.Min());
            plt.AddPoint(
                threadCounts[optimalIndex],
                avgTimes[optimalIndex],
                color: System.Drawing.Color.Red,
                markerSize: 15,
                label: $"Оптимум: {threadCounts[optimalIndex]} потоков"
            );

            plt.Legend();
            plt.SaveFig("performance_plot.png");
            
            Console.WriteLine($"График сохранен в файл: performance_plot.png");
        }
    }
}