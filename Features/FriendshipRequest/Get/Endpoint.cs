using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Get
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<List<FriendshipResponseDto>>, FriendshipRequestEntity>
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
            Routes("/friendship");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var friendshipRequests=new List<FriendshipRequestEntity>();
            if (req.isOnlyAccepted)
                friendshipRequests = await friendshipRequestRepository.GetAllAcceptedAsync(currentUserId);
            else 
                friendshipRequests= await friendshipRequestRepository.GetListAsync(x => !x.IsApproved && x.ReceiverId == currentUserId);
            var friendshipDtos = friendshipRequests.Where(x => x != null).Select(x => new FriendshipResponseDto(x!)).ToList();
            var response = new ServiceResponse<List<FriendshipResponseDto>>(friendshipDtos);
            await SendAsync(response);
        }
    }

    public class Request 
    {
        public bool isOnlyAccepted { get; set; }
    }
}