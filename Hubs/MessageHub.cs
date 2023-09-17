using Microsoft.AspNetCore.Authorization;
namespace Scandium.Hubs
{
    [Authorize]
    public class MessageHub: BaseHub<IMessageClient> 
    {
        
    }

    public interface IMessageClient
    {
        Task ReceiveMessage(Model.Dto.MessageResponseDto messageDto);
    }
}