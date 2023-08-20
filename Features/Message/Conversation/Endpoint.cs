
using FastEndpoints;
using Scandium.Data;
using Scandium.Data.Abstract;
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
            var response = MapFromEntity(userId, messages, users);
            await SendAsync(response);
        }

        public ServiceResponse<ConversationResponseModel> MapFromEntity(Guid currentUserId, List<MessageEntity> messages, List<Model.Entities.User> users)
        {
            var currentUser = users.FirstOrDefault(x => x.Id == currentUserId);
            var otherUser = users.FirstOrDefault(x => x.Id != currentUserId);
            var messageDtos = messages.Select(x => new MessageDto()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                ReceiverId = x.ReceiverId,
                SenderId = x.SenderId
            }).ToList();

            var conversationResponse = new ConversationResponseModel(currentUser, otherUser, messageDtos);
            return new ServiceResponse<ConversationResponseModel>(conversationResponse);
        }
    }
}