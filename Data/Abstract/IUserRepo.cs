using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scandium.Model;

namespace Scandium.Data
{
    public interface IUserRepo
    {
        Task SaveChanges();
        Task<User?> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task CreateUser(User cmd);

        void DeleteUser(User cmd);
    }
}