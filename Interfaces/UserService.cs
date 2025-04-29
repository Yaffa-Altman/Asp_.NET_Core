using CoreProject.Models;
namespace CoreProject.interfaces;
public interface IUserService
{
    User Get(int id);
    List<User> Get();
    int Add(User shoes);
    bool Update(int id, User shoes);
    bool Delete(int id);
}