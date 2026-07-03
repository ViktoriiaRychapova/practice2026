using Task11;
using Xunit;

namespace Task11Tests
{
    public class GeneratorTests
    {
        [Fact]
        public void TestCalculatorGeneration()
        {
            string code = @"
                using Task11;
                public class Calculator : ICalculator
                {
                    public int Add(int a, int b) => a + b;
                    public int Minus(int a, int b) => a - b;
                    public int Mul(int a, int b) => a * b;
                    public int Div(int a, int b) => a / b;
                }";

            var generator = new CodeGenerator();
            
            ICalculator calculator = generator.GenerateCalculator(code);

            Assert.Equal(10, calculator.Add(7, 3));
            Assert.Equal(5, calculator.Minus(10, 5));
            Assert.Equal(20, calculator.Mul(4, 5));
            Assert.Equal(2, calculator.Div(10, 5));
        }
    }
}