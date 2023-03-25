using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Services.Abstract;
using MessageEntity = Scandium.Model.Entities.Message;

namespace Scandium.Features.Message.Create
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
            Verbs(Http.POST);
            Routes("/message/insert");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var message = MapToEntity(req);
            await messageRepository.AddAsync(message);
            var addedMessage = await messageRepository.GetByIdThrowAsync(message.Id);
            var response = MapFromEntity(addedMessage);
            await SendAsync(response);
        }

        public override MessageEntity MapToEntity(Request r) => new()
        {
            SenderId = httpContextService.GetUserIdFromClaims(),
            ReceiverId = r.ReceiverId,
            Content = r.Content,
        };
        public override Response MapFromEntity(MessageEntity e) => new()
        {
            Content = e.Content,
            CreateDate = e.CreatedAt,
            Id = e.Id,
            ReceiverId = e.ReceiverId,
            ReceiverUsername = e.Receiver.Username,
            SenderId = e.SenderId,
            SenderUsername = e.Sender.Username
        };
    }
}