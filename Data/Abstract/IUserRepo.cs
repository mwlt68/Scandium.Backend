using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scandium.Data.Abstract;
using Scandium.Model;

namespace Scandium.Data
{
    public interface IUserRepo : IGenericRepository<User>
    {
    }
}