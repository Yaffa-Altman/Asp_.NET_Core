using CoreProject.Models;
using CoreProject.interfaces;
using Serilog;

namespace CoreProject.Services;

public class GenericService<T> : IGenericService<T> where T : GenericId
{
    private JsonService<T> jsonService;
    private readonly ILogger<GenericService<T>> _logger;
    List<T> Items { get; }
    public GenericService(IHostEnvironment env, ILogger<GenericService<T>> logger){
        _logger.LogInformation($"start GenericService<{typeof(T).Name}> Constructor");
        jsonService = new JsonService<T>(env);
        Items = jsonService.GetItems();
        _logger = logger;
        _logger.LogInformation($"end GenericService<{typeof(T).Name}> Constructor");
    }

    public T Get(int id) {
        _logger.LogInformation($"in GenericService<{typeof(T).Name}> Get{"+id+"}");
        return Items.FirstOrDefault(p => p.Id == id);
    }
    public List<T> Get() {
        // Console.WriteLine("!!!!!!!!!!!!");
        // Console.WriteLine(Items.Count);
        // Items.ForEach(item => Console.WriteLine(item.Name));
        // Console.WriteLine(Items)
        _logger.LogInformation($"in GenericService<{typeof(T).Name}> Get");
        return Items; 
    } 

    public int Add(T item)
    {
        _logger.LogInformation($"start GenericService<{typeof(T).Name}> Add");
        if (item.Name == null
        || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString()))){
            _logger.LogInformation($"in GenericService<{typeof(T).Name}> Add - name {typeof(T).Name} or/and password is null");
            return -1;
        }
        int maxId = Items.Max(p => p.Id);
        item.Id = maxId + 1;
        Items.Add(item);
        jsonService.saveToFile();
        _logger.LogInformation($"end GenericService<{typeof(T).Name}> Add");
        return item.Id;
    }
    public bool Update(int id, T item)
    {
        _logger.LogInformation($"start GenericService<{typeof(T).Name}> Update");
        // Console.WriteLine("-----------"+item.Id+"----"+id);
        if (item == null
            || string.IsNullOrWhiteSpace(item.Name)
            || item.Id != id
            || (typeof(T).Name == "User" && string.IsNullOrWhiteSpace(typeof(T).GetProperty("Password")?.GetValue(item)?.ToString())))
        {
            _logger.LogInformation($"in GenericService<{typeof(T).Name}> Update - {typeof(T).Name} or {typeof(T).Name} name or password (if user) is null, or id not correct.");
            return false;
        }
        T itm = Items?.FirstOrDefault(p => p.Id == id);
        if (itm == null)
        {
            _logger.LogInformation($"in GenericService<{typeof(T).Name}> Update - {typeof(T).Name} not found");
            return false;
        }
        var index = Items.IndexOf(itm);
        itm.Name = item.Name;
        Items[index] = itm;
        jsonService.saveToFile();
        _logger.LogInformation($"end GenericService<{typeof(T).Name}> Update");
        return true;
    }
    public bool Delete(int id)
    {
        _logger.LogInformation($"start GenericService<{typeof(T).Name}> Delete");
        T item = Get(id);
        if (item is null)
        {
            _logger.LogInformation($"in GenericService<{typeof(T).Name}> Delete - not found {typeof(T).Name}");
            return false;
        }
        Items.Remove(item);
        jsonService.saveToFile();
        _logger.LogInformation($"end GenericService<{typeof(T).Name}> Delete");
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
