using System.Text.Json;
using Serilog;
namespace CoreProject.Services;

public class JsonService<T>{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;
    private readonly ILogger<JsonService<T>> _logger;

    public JsonService(IHostEnvironment env)
    {
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
    }

    public List<T> GetItems() => Items;

    public void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(Items));
    }
}