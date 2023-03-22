using Scandium.Data.Concreate;
using Scandium.Model;

namespace Scandium.Data
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        public UserRepo(AppDbContext context) : base(context)
        {
        }
    }
}