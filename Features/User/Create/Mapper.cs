using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scandium.Helpers;

namespace Scandium.Features.User.Create
{
    public class Mapper : FastEndpoints.Mapper<Request, Response, Scandium.Model.User>
    {
        public override Scandium.Model.User  ToEntity(Request r) => new Model.User(){
            Username = r.Username,
            Password = MD5HashHelper.Create(r.Password!)
        };
    }
}