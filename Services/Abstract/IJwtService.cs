using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scandium.Services.Abstract
{
    public interface IJwtService
    {
        String Create(int userId);
    }
}