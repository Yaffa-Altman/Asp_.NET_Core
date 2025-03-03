using _2025_02_18.Models;
using _2025_02_25.interfaces;
namespace _2025_02_18.Services;
public class MyShoesService : IShoesService
{
    private List<MyShoes> myList;

    public MyShoesService()
    {
        myList = new List<MyShoes> {
            new MyShoes{Id = 1, Description = "High heels", isElegant = true},
            new MyShoes{Id = 2, Description = "Sneakers", isElegant = false},
            new MyShoes{Id = 3, Description = "Elegant flat shoes", isElegant = true},
            new MyShoes{Id = 4, Description = "Slippers", isElegant = false},
        };
    }
    public MyShoes Get(int id)
    {
        var shoes = myList.FirstOrDefault(p => p.Id == id);
        return shoes;
    }
    public List<MyShoes> Get()
    {
        return myList;
    }
    public int Add(MyShoes shoes)
    {
        if (shoes == null 
            || string.IsNullOrWhiteSpace(shoes.Description))
            return -1;

        int maxId = myList.Max(p => p.Id);
        shoes.Id = maxId + 1;
        myList.Add(shoes);

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
        
        MyShoes shs = myList.FirstOrDefault(p => p.Id == id);
        if (shs == null)
            return false;
        
        var index = myList.IndexOf(shs);
        myList[index] = shoes;

        return true;
    }
    public bool Delete(int id)
    {
        MyShoes shoes = myList.FirstOrDefault(p => p.Id == id);
        if (shoes == null)
            return false;

        int index = myList.IndexOf(shoes);
        myList.RemoveAt(index);

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
