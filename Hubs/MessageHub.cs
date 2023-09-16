using Microsoft.AspNetCore.Authorization;
namespace Scandium.Hubs
{
    [Authorize]
    public class MessageHub: BaseHub<IMessageClient> 
    {
        
    }

    public interface IMessageClient
    {
        Task ReceiveMessage(Scandium.Model.Dto.MessageDto messageDto);
    }
}