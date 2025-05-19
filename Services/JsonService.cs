using System.Text.Json;
namespace CoreProject.Services;

public class JsonService<T>{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;

    public JsonService(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        
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