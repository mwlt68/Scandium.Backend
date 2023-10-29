
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Scandium.Hubs
{
    
    public abstract class BaseHub<TClient>: Hub<TClient> where TClient : class
    {

        public override async Task OnConnectedAsync()
        {
            var userId = Context?.User?.Claims?.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is not null && Context?.ConnectionId is not null ){
                await Groups.AddToGroupAsync(Context!.ConnectionId,userId);
                Console.WriteLine($"Connected { Context!.ConnectionId}");
            }
            await base.OnConnectedAsync();
            return;
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context?.User?.Claims?.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier)?.Value;
            if(Context?.ConnectionId is not null && userId is not null){
                await Groups.RemoveFromGroupAsync(Context!.ConnectionId, userId); 
                Console.WriteLine($"DisConnected {Context!.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
            return;
        }
    }

}