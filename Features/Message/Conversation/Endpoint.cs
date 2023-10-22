
using FastEndpoints;
using Scandium.Data;
using Scandium.Data.Abstract;
using Scandium.Helpers;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using MessageEntity = Scandium.Model.Entities.Message;

namespace Scandium.Features.Message.Conversation
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<ConversationResponseModel>, MessageEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepo userRepo;

        public Endpoint(IHttpContextService httpContextService, IMessageRepository messageRepository, IUserRepo userRepo)
        {
            this.httpContextService = httpContextService;
            this.messageRepository = messageRepository;
            this.userRepo = userRepo;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/message/conversation");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userId = httpContextService.GetUserIdFromClaims();
            var messages = await messageRepository.GetConversationAsync(userId, req.OtherUserId);
            var users = await userRepo.GetListAsync(x => x.Id == userId || x.Id == req.OtherUserId);
            var response = await MapFromEntityAsync(userId, messages, users);
            await SendAsync(response);
        }

        public async Task<ServiceResponse<ConversationResponseModel>> MapFromEntityAsync(Guid currentUserId, List<MessageEntity> messages, List<Model.Entities.User> users)
        {
            var currentUser = users.FirstOrDefault(x => x.Id == currentUserId);
            var otherUser = users.FirstOrDefault(x => x.Id != currentUserId);
            var messageDtos = new List<MessageDto>();
            foreach (var message in messages)
            {
                if (message is not null)
                {
                    var decryptedContent = await AES.DecryptAsync(message.Contents!);
                    var messageDto = new MessageDto()
                    {
                        Id = message.Id,
                        Content = decryptedContent,
                        CreatedAt = message.CreatedAt,
                        ReceiverId = message.ReceiverId,
                        SenderId = message.SenderId
                    };
                    messageDtos.Add(messageDto);
                }
            }
            var conversationResponse = new ConversationResponseModel(currentUser, otherUser, messageDtos);
            return new ServiceResponse<ConversationResponseModel>(conversationResponse);
        }
    }
}