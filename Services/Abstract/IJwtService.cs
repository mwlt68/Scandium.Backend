

namespace Scandium.Services.Abstract
{
    public interface IJwtService
    {
        String Create(Guid userId);
    }
}