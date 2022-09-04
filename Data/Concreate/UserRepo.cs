using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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