using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Scandium.Hubs;

namespace Scandium.Features.Message.Test
{
    public class Endpoint : Endpoint<Request>
    {
        private readonly IHubContext<MessageHub, IMessageClient> chatHub;

        public Endpoint( IHubContext<MessageHub, IMessageClient> chatHub)
        {
            this.chatHub = chatHub;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/message/hub");
            AllowAnonymous();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            await chatHub.Clients.All.ReceiveMessage(req.Message);
            await SendAsync(req.Message);
        }

    }

    public class Request {
        public string Message {get;set;}
    }
}