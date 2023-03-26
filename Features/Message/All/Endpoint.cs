using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Services.Abstract;
using MessageEntity = Scandium.Model.Entities.Message;

namespace Scandium.Features.Message.All
{
    public class Endpoint : EndpointWithMapping<Request, Response, MessageEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IMessageRepository messageRepository;

        public Endpoint(IHttpContextService httpContextService, IMessageRepository messageRepository)
        {
            this.httpContextService = httpContextService;
            this.messageRepository = messageRepository;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/message/list");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userId = httpContextService.GetUserIdFromClaims();
            var messages = await messageRepository.GetAllMessagesAsync(userId);
            var response = MapFromEntity(messages);
            await SendAsync(response);
        }

        public  Response MapFromEntity(List<MessageEntity> es) => new()
        {
            Messages = es.Select(e =>
                new Create.Response()
                {
                    Content = e.Content,
                    CreateDate = e.CreatedAt,
                    Id = e.Id,
                    ReceiverId = e.ReceiverId,
                    ReceiverUsername = e.Receiver.Username,
                    SenderId = e.SenderId,
                    SenderUsername = e.Sender.Username
                }).ToList()
        };
    }
}