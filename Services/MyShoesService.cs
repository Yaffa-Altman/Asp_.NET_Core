using System.Text.Json;
using _2025_02_18.Models;
using _2025_02_25.interfaces;
namespace _2025_02_18.Services;
public class MyShoesService : IShoesService
{
    List<MyShoes> Shoes { get; }
    private static string fileName = "myShoes.json";
    private string filePath;

    public MyShoesService(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

        using (var jsonFile = File.OpenText(filePath))
        {
            Shoes = JsonSerializer.Deserialize<List<MyShoes>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(Shoes));
    }
    public MyShoes Get(int id) => Shoes.FirstOrDefault(p => p.Id == id);
    public List<MyShoes> Get() => Shoes;
    public int Add(MyShoes shoes)
    {
        if (shoes == null)
            return -1;
        int maxId = Shoes.Max(p => p.Id);
        shoes.Id = maxId + 1;
        Shoes.Add(shoes);
        saveToFile();
        return shoes.Id;
    }
    public bool Update(int id, MyShoes shoes)
    {
        if (shoes == null
            || string.IsNullOrWhiteSpace(shoes.Description)
            || shoes.Id != id)
        {
            return false;
        }

        MyShoes shs = Shoes.FirstOrDefault(p => p.Id == id);
        if (shs == null)
            return false;

        var index = Shoes.IndexOf(shs);
        Shoes[index] = shoes;
        saveToFile();
        return true;
    }
    public bool Delete(int id)
    {
        MyShoes shoes = Get(id);
        if (shoes is null)
            return false;
        Shoes.Remove(shoes);
        saveToFile();
        return true;
    }
}

public static class MyShoesUtilities
{
    public static void AddShoesConst(this IServiceCollection services)
    {
        services.AddSingleton<IShoesService, MyShoesService>();
    }
}
