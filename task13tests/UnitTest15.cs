using NUnit.Framework;
using task13;
using System;

namespace task15tests
{
    [TestFixture]
    public class IntegralCalculatorTests
    {
        [Test]
        public void TestSolve_WithStep1e1_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            double result = calculator.Solve(1e-1);
            
            Assert.That(result, Is.EqualTo(0).Within(1e-3));
        }

        [Test]
        public void TestSolve_WithStep1e2_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            double result = calculator.Solve(1e-2);
            
            Assert.That(result, Is.EqualTo(0).Within(1e-4));
        }

        [Test]
        public void TestSolve_WithMultipleThreads_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            double result1 = calculator.Solve(1e-3, 1);
            double result4 = calculator.Solve(1e-3, 4);
            
            Assert.That(result1, Is.EqualTo(result4).Within(1e-6));
        }

        [Test]
        public void TestSolve_WithThreadCount1_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            double result = calculator.Solve(1e-1, 1);
            
            Assert.That(result, Is.EqualTo(0).Within(1e-3));
        }

        [Test]
        public void TestPerformance_WithMultipleThreads()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            var (result1, time1) = calculator.SolveWithTime(1e-3, 1);
            var (result4, time4) = calculator.SolveWithTime(1e-3, 4);
            
            Assert.That(result1, Is.EqualTo(result4).Within(1e-6));
            Console.WriteLine($"Single thread time: {time1.TotalMilliseconds} ms");
            Console.WriteLine($"4 threads time: {time4.TotalMilliseconds} ms");
        }

        [Test]
        public void TestSolve_WithNegativeStep_ThrowsArgumentException()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            Assert.Throws<ArgumentException>(() => calculator.Solve(-1e-3));
        }

        [Test]
        public void TestSolve_WithZeroStep_ThrowsArgumentException()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            Assert.Throws<ArgumentException>(() => calculator.Solve(0));
        }

        [Test]
        public void TestSolve_WithZeroThreadCount_ThrowsArgumentException()
        {
            var calculator = new IntegralCalculator(Math.Sin, -100, 100);
            
            Assert.Throws<ArgumentException>(() => calculator.Solve(1e-3, 0));
        }

        [Test]
        public void TestSolve_WithSingleThread_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(x => 1, 0, 1);
            
            double result = calculator.Solve(1e-3, 1);
            
            Assert.That(result, Is.EqualTo(1).Within(1e-3));
        }

        [Test]
        public void TestSolve_WithMultipleThreads_ReturnsCorrectResult()
        {
            var calculator = new IntegralCalculator(x => 1, 0, 1);
            
            double result = calculator.Solve(1e-3, 4);
            
            Assert.That(result, Is.EqualTo(1).Within(1e-3));
        }
    }
}