using SonarQubeDemo;

var calculator = new Calculator();

Console.WriteLine("SonarQube Analysis Demo");
Console.WriteLine($"2 + 3 = {calculator.Add(2, 3)}");
Console.WriteLine($"10 - 4 = {calculator.Subtract(10, 4)}");
Console.WriteLine($"6 * 7 = {calculator.Multiply(6, 7)}");
Console.WriteLine($"20 / 4 = {calculator.Divide(20, 4)}");
