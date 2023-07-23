using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Request
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<List<FriendshipRequestDto>>, FriendshipRequestEntity>
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
            Verbs(Http.GET);
            Routes("/friendship/request");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var friendshipRequests = await friendshipRequestRepository.GetListAsync(x => !x.IsApproved && x.ReceiverId == currentUserId);
            var response = new ServiceResponse<List<FriendshipRequestDto>>(friendshipRequests.Select(r => new FriendshipRequestDto(r)).ToList());
            await SendAsync(response);
        }
    }

    public class Request 
    {

    }
}