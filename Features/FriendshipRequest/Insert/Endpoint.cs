using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Insert
{
    public class Endpoint : EndpointWithMapping<Request,ServiceResponse<FriendshipRequestDto>, FriendshipRequestEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IFriendshipRequestRepository friendshipRequestRepository;

        public Endpoint(IHttpContextService httpContextService, IFriendshipRequestRepository friendshipRequestRepository)
        {
            this.httpContextService = httpContextService;
            this.friendshipRequestRepository = friendshipRequestRepository;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/friendship/insert");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var request = MapToEntity(req);
            await friendshipRequestRepository.AddAsync(request);
            var addedRequest = await friendshipRequestRepository.GetByIdThrowAsync(request.Id);
            var response = MapFromEntity(addedRequest);
            await SendAsync(response);
        }

        public override FriendshipRequestEntity MapToEntity(Request r) => new()
        {
            SenderId = httpContextService.GetUserIdFromClaims(),
            ReceiverId = r.ReceiverId,
        };
        public override ServiceResponse<FriendshipRequestDto> MapFromEntity(FriendshipRequestEntity e) => new ServiceResponse<FriendshipRequestDto>(new FriendshipRequestDto(e));
    }
}