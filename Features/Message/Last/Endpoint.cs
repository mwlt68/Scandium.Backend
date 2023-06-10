using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using MessageEntity = Scandium.Model.Entities.Message;

namespace Scandium.Features.Message.Last
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<List<MessageResponseDto>> , MessageEntity>
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
            Routes("/message/last");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userId = httpContextService.GetUserIdFromClaims();
            var messages = await messageRepository.GetLastMessagesAsync(userId);
            var response = MapFromEntity(messages);
            await SendAsync(response);
        }

        public ServiceResponse<List<MessageResponseDto>> MapFromEntity(List<MessageEntity?> es) => new ServiceResponse<List<MessageResponseDto>>(es.Where(x=> x!= null).Select(e => new MessageResponseDto(e!)).ToList());

    }
}