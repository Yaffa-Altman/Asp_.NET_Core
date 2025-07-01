using System.Text.Json;
namespace CoreProject.Services;

public class JsonService<T>{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;
    private readonly ILogger<JsonService<T>> _logger;

    public JsonService(IHostEnvironment env)
    {
        _logger.LogInformation("start JsonService Constructor");
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<JsonService<T>>();
        using (var jsonFile = File.OpenText(filePath))
        {
            Items = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        _logger.LogInformation("end JsonService Constructor");
    }

    public List<T> GetItems()
    { 
        _logger.LogInformation("start end JsonService GetItems");
        return Items;
    }

    public void saveToFile()
    {
        _logger.LogInformation("start JsonService saveToFile");
        File.WriteAllText(filePath, JsonSerializer.Serialize(Items));
        _logger.LogInformation("end JsonService saveToFile");
    }
}