
namespace Scandium.Services.Abstract
{
    public interface IHttpContextService
    {
        public Guid GetUserIdFromClaims();
    }
}