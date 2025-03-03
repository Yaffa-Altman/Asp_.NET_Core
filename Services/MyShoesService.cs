using _2025_02_18.Models;
namespace _2025_02_18.Services;
public class MyShoesService
{
    private static List<MyShoes> myList;

    static MyShoesService()
    {
        myList = new List<MyShoes> {
            new MyShoes{Id = 1, Description = "High heels", isElegant = true},
            new MyShoes{Id = 2, Description = "Sneakers", isElegant = false},
            new MyShoes{Id = 3, Description = "Elegant flat shoes", isElegant = true},
            new MyShoes{Id = 4, Description = "Slippers", isElegant = false},
        };
    }
    public static MyShoes Get(int id)
    {
        var shoes = myList.FirstOrDefault(p => p.Id == id);
        return shoes;
    }
    public static List<MyShoes> Get()
    {
        return myList;
    }
    public static int Add(MyShoes shoes)
    {
        if (shoes == null 
            || string.IsNullOrWhiteSpace(shoes.Description))
            return -1;

        int maxId = myList.Max(p => p.Id);
        shoes.Id = maxId + 1;
        myList.Add(shoes);

        return shoes.Id;
    }
    public static bool Update(int id, MyShoes shoes)
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
    public static bool Delete(int id)
    {
        MyShoes shoes = myList.FirstOrDefault(p => p.Id == id);
        if (shoes == null)
            return false;

        int index = myList.IndexOf(shoes);
        myList.RemoveAt(index);

        return true;
    } 
}
