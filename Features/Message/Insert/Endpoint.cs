using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Scandium.Data.Abstract;
using Scandium.Extensions.EntityExtensions;
using Scandium.Helpers;
using Scandium.Hubs;
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
        private readonly IHubContext<MessageHub, IMessageClient> chatHub;
        

        public Endpoint(IHttpContextService httpContextService, IMessageRepository messageRepository,IHubContext<MessageHub, IMessageClient> chatHub)
        {
            this.httpContextService = httpContextService;
            this.messageRepository = messageRepository;
            this.chatHub = chatHub;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/message/insert");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var message = await MapToEntityAsync(req);
            await messageRepository.AddAsync(message);
            var addedMessage = await messageRepository.GetByIdAsync(message.Id).ThrowIfNotFound();
            await RunReceiveMessage(req.ReceiverId.ToString(), addedMessage);
            var response = await MapFromEntityAsync(addedMessage);
            await SendAsync(response);
        }

        private async Task RunReceiveMessage(string receiverId , MessageEntity addedMessage)
        {
            var dto = await MessageResponseDto.Get(addedMessage);
            await chatHub.Clients.Group(receiverId).ReceiveMessage(dto);
        }
        public override async Task<MessageEntity> MapToEntityAsync(Request r)
        {
            var content = await AES.EncryptAsync(r.Content!);
            return new()
            {
                SenderId = httpContextService.GetUserIdFromClaims(),
                ReceiverId = r.ReceiverId,
                Contents =content,
            };
        }
        public override async Task<ServiceResponse<MessageResponseDto>> MapFromEntityAsync(MessageEntity e)
        {
            var messageModel = await MessageResponseDto.Get(e);
            return new  ServiceResponse<MessageResponseDto>(messageModel);
        }
    }
}