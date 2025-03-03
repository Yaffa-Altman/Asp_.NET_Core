using _2025_02_18.Models;
namespace _2025_02_25.interfaces;
public interface IShoesService
{
    MyShoes Get(int id);
    List<MyShoes> Get();
    int Add(MyShoes shoes);
    bool Update(int id, MyShoes shoes);
    bool Delete(int id);
}