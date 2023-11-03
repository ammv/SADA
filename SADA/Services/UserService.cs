using DataLayer;
using System.Data.SqlClient;
using System.Linq;

namespace SADA.Services
{
    class UserService : IUserService
    {
        public bool CheckPassword(User user, string password)
        {
            if (user == null || password == null)
            {
                return false;
            }
            using (var ctx = new SADAEntities())
            {
                byte[] hash = ctx.Database.SqlQuery<byte[]>($"SELECT dbo.HashSalt(@Password, @Salt)",
                    new SqlParameter("Password", password),
                    new SqlParameter("Salt", user.Salt)).Single();

                return user.PasswordHash.SequenceEqual(hash);
            }
        }

        public User GetUser(string login)
        {
            using (var ctx = new SADAEntities())
            {
                return ctx.User.FirstOrDefault(u => u.Login == login);
            }
        }
    }
}
