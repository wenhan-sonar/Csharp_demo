namespace SonarQubeDemo.Tests;

using SonarQubeDemo;
using Xunit;

public class CalculatorTests
{
    private readonly Calculator _calculator = new();

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, 0, 0)]
    public void Add_ReturnsCorrectSum(int a, int b, int expected)
    {
        Assert.Equal(expected, _calculator.Add(a, b));
    }

    [Fact]
    public void Subtract_ReturnsCorrectDifference()
    {
        Assert.Equal(5, _calculator.Subtract(10, 5));
    }

    [Fact]
    public void Multiply_ReturnsCorrectProduct()
    {
        Assert.Equal(42, _calculator.Multiply(6, 7));
    }

    [Fact]
    public void Divide_ReturnsCorrectQuotient()
    {
        Assert.Equal(5.0, _calculator.Divide(20, 4));
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
    }
}
