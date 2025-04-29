using System.Text.Json;
using CoreProject.Models;
using CoreProject.interfaces;
namespace CoreProject.Services;
public class GenericService<T> : IGenericService<T> where T : GenericId
{
    List<T> Items { get; }
    private static string fileName = $"{typeof(T).Name}.json";
    private string filePath;

    public GenericService(IHostEnvironment env)
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

    private void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(Items));
    }
    public T Get(int id) => Items.FirstOrDefault(p => p.Id == id);
    public List<T> Get() => Items;
    public int Add(T item)
    {
        if (item.Id == null || item.Name == null
        || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString())))
            return -1;
        int maxId = Items.Max(p => p.Id);
        item.Id = maxId + 1;
        Items.Add(item);
        saveToFile();
        return item.Id;
    }
    public bool Update(int id, T item)
    {
        if (item == null
            || string.IsNullOrWhiteSpace(item.Name)
            || item.Id != id
            || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString())))
        {
            return false;
        }

        T shs = Items.FirstOrDefault(p => p.Id == id);
        if (shs == null)
            return false;

        var index = Items.IndexOf(shs);
        Items[index] = item;
        saveToFile();
        return true;
    }
    public bool Delete(int id)
    {
        T item = Get(id);
        if (item is null)
            return false;
        Items.Remove(item);
        saveToFile();
        return true;
    }
}

public static class GenericUtilities
{
    public static void AddItemsConst<T>(this IServiceCollection services) where T : GenericId
    {
        services.AddSingleton<IGenericService<T>, GenericService<T>>();
    }
}
