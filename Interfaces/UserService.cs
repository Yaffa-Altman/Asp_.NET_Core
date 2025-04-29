using _2025_02_18.Models;
namespace _2025_02_25.interfaces;
public interface IUserService
{
    User Get(int id);
    List<User> Get();
    int Add(User shoes);
    bool Update(int id, User shoes);
    bool Delete(int id);
}