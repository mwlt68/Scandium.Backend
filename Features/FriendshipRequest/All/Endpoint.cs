using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Exceptions;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.All
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
            Verbs(Http.POST);
            Routes("/friendship/all");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var friendships = await friendshipRequestRepository.GetAllAcceptedAsync(currentUserId);
            var friendshipDtos = friendships.Where(x => x != null).Select(x => new FriendshipRequestDto(x!)).ToList();
            var response = new ServiceResponse<List<FriendshipRequestDto>>(friendshipDtos);
            await SendAsync(response);
        }
    }

    public class Request
    {
    }
}