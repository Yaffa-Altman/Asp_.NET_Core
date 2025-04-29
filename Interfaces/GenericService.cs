using CoreProject.Models;
namespace CoreProject.interfaces;
public interface IGenericService<T>
{
    T Get(int id);
    List<T> Get();
    String Add(T shoes);
    bool Update(int id, T shoes);
    bool Delete(int id);
}