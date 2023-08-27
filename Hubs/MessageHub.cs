using Microsoft.AspNetCore.SignalR;

namespace Scandium.Hubs
{
    public class MessageHub: Hub<IMessageClient> 
    {
        public async Task SendMessage(string message)
        {
            Console.WriteLine("Message: "+ message);
            await Clients.All.ReceiveMessage(message);
        }
    }

    public interface IMessageClient
    {
        Task ReceiveMessage(string message);
    }
}