using Newtonsoft.Json;
using Serilog;
using SonarQubeDemo;

// Configure Serilog for structured console logging.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var calculator = new Calculator();

    var results = new Dictionary<string, double>
    {
        ["2 + 3"] = calculator.Add(2, 3),
        ["10 - 4"] = calculator.Subtract(10, 4),
        ["6 * 7"] = calculator.Multiply(6, 7),
        ["20 / 4"] = calculator.Divide(20, 4),
    };

    Log.Information("SonarQube Analysis Demo");
    foreach (var (expression, value) in results)
    {
        Log.Information("{Expression} = {Result}", expression, value);
    }

    // Serialize the results to JSON using Newtonsoft.Json.
    var json = JsonConvert.SerializeObject(results, Formatting.Indented);
    Log.Information("Results as JSON:{NewLine}{Json}", Environment.NewLine, json);
}
finally
{
    Log.CloseAndFlush();
}
