using CoreProject.Models;
namespace CoreProject.interfaces;
public interface IShoesService
{
    MyShoes Get(int id);
    List<MyShoes> Get();
    int Add(MyShoes shoes);
    bool Update(int id, MyShoes shoes);
    bool Delete(int id);
}