using DataLayer;

namespace SADA.Services
{
    public interface IUserService
    {
        User GetUser(string login);

        bool CheckPassword(User user, string password);
    }
}