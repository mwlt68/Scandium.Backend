using Scandium.Helpers;

namespace Scandium.Features.User.Insert
{
    public class Mapper : FastEndpoints.Mapper<Request, Response, Scandium.Model.Entities.User>
    {
        public override Scandium.Model.Entities.User ToEntity(Request r) => new Model.Entities.User(){
            Username = r.Username,
            Password = MD5HashHelper.Create(r.Password!)
        };
    }
}