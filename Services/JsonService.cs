using System.Text.Json;
using Serilog;
namespace CoreProject.Services;

public class JsonService<T>{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;

    public JsonService(IHostEnvironment env)
    {
        Log.Information("start JsonService Constructor");
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        
        using (var jsonFile = File.OpenText(filePath))
        {
            Items = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        Log.Information("end JsonService Constructor");
    }

    public List<T> GetItems()
    { 
        Log.Information("start end JsonService GetItems");
        return Items;
    }

    public void saveToFile()
    {
        Log.Information("start JsonService saveToFile");
        File.WriteAllText(filePath, JsonSerializer.Serialize(Items));
        Log.Information("end JsonService saveToFile");
    }
}