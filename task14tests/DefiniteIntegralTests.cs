using Xunit;
using task14;
using System;

public class DefiniteIntegralTests
{
    [Fact]
    public void Test_Solve_Linear()
    {
        Func<double, double> X = x => x;
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
    }

    [Fact]
    public void Test_Solve_Sin()
    {
        Func<double, double> SIN = x => Math.Sin(x);
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
    }

    [Fact]
    public void Test_Solve_Constant()
    {
        Func<double, double> X = x => 1;
        Assert.Equal(5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }
}