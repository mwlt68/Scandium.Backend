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
            var response = await MapFromEntityAsync(messages);
            await SendAsync(response);
        }

        public async Task<ServiceResponse<List<MessageResponseDto>>> MapFromEntityAsync(List<MessageEntity?> messages)
        {
            var messageResponses  =  new List<MessageResponseDto>();
            foreach (var message in messages)
            {
                if(message is not null){
                    var messageResponse = await MessageResponseDto.Get(message);
                    messageResponses.Add(messageResponse);
                }
            }
            return new ServiceResponse<List<MessageResponseDto>>(messageResponses);
        }

    }
}