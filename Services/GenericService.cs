using CoreProject.Models;
using CoreProject.interfaces;

namespace CoreProject.Services;

public class GenericService<T> : IGenericService<T> where T : GenericId
{
    private JsonService<T> jsonService;
    List<T> Items { get; }

    public GenericService(IHostEnvironment env){
        jsonService = new JsonService<T>(env);
        Items = jsonService.GetItems();
    }

    public T Get(int id) => Items.FirstOrDefault(p => p.Id == id);
    public List<T> Get() {
        Console.WriteLine("!!!!!!!!!!!!");
        Console.WriteLine(Items.Count);
        Items.ForEach(item => Console.WriteLine(item.Name));
        Console.WriteLine(Items);
        return Items; 
    } 

    public int Add(T item)
    {
        if (item.Name == null
        || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString())))
            return -1;
        int maxId = Items.Max(p => p.Id);
        item.Id = maxId + 1;
        Items.Add(item);
        jsonService.saveToFile();
        return item.Id;
    }
    public bool Update(int id, T item)
    {
        Console.WriteLine("-----------"+item.Id+"----"+id);
        if (item == null
            || string.IsNullOrWhiteSpace(item.Name)
            || item.Id != id
            || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString())))
        {
            return false;
        }
        T shs = Items?.FirstOrDefault(p => p.Id == id);
        if (shs == null)
            return false;
        var index = Items.IndexOf(shs);
        shs.Name = item.Name;
        Items[index] = shs;
        jsonService.saveToFile();
        return true;
    }
    public bool Delete(int id)
    {
        T item = Get(id);
        if (item is null)
            return false;
        Items.Remove(item);
        jsonService.saveToFile();
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
