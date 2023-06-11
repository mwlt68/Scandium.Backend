using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Extensions.EntityExtensions;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using MessageEntity = Scandium.Model.Entities.Message;

namespace Scandium.Features.Message.Insert
{
    public class Endpoint : EndpointWithMapping<Request,ServiceResponse<MessageResponseDto>, MessageEntity>
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
            var addedMessage = await messageRepository.GetByIdAsync(message.Id).ThrowIfNotFound();
            var response = MapFromEntity(addedMessage);
            await SendAsync (response);
        }

        public override MessageEntity MapToEntity(Request r) => new()
        {
            SenderId = httpContextService.GetUserIdFromClaims(),
            ReceiverId = r.ReceiverId,
            Content = r.Content,
        };
        public override ServiceResponse<MessageResponseDto> MapFromEntity(MessageEntity e) => new ServiceResponse<MessageResponseDto>(new MessageResponseDto(e));
    }
}