using System.Security.Claims;
using Scandium.Exceptions;
using Scandium.Services.Abstract;

namespace Scandium.Services.Concreate
{
    public class HttpContextService: IHttpContextService
    {
        private readonly IHttpContextAccessor contextAccessor;
        public HttpContextService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        public Guid GetUserIdFromClaims()
        {
            var userIdString = contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdString != null)
            {
                bool isGuid = Guid.TryParse(userIdString,out _);
                if(isGuid)
                    return Guid.Parse(userIdString);
            }
            throw new NotFoundException("User identityfier not found");
        }
    }
}