using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Scandium.Data.Abstract;
using Scandium.Exceptions;
using Scandium.Hubs;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Approve
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<FriendshipResponseDto>, FriendshipRequestEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IFriendshipRequestRepository friendshipRequestRepository;
        private readonly IHubContext<FriendshipRequestHub, IFriendshipRequesClient> friendshipHub;


        public Endpoint(IHttpContextService httpContextService, IFriendshipRequestRepository friendshipRequestRepository, IHubContext<FriendshipRequestHub, IFriendshipRequesClient> friendshipHub)
        {
            this.httpContextService = httpContextService;
            this.friendshipRequestRepository = friendshipRequestRepository;
            this.friendshipHub = friendshipHub;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/friendship/approve");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var friendshipRequest = await friendshipRequestRepository.GetAsync(x => x.Id == req.FriendshipRequestId && !x.IsApproved && x.ReceiverId == currentUserId);
            if (friendshipRequest != null)
            {
                friendshipRequest.IsApproved = true;
                await friendshipRequestRepository.UpdateAsync(friendshipRequest);
                await RunApproveFriendshipRequest(friendshipRequest);
                var response = MapFromEntity(friendshipRequest);
                await SendAsync(response);
            }
            else
                throw new NotFoundException("Friendship request not found !");
        }

        private async Task RunApproveFriendshipRequest(FriendshipRequestEntity addedMessage)
        {
            var dto = new FriendshipResponseDto(addedMessage);
            await friendshipHub.Clients.Group(addedMessage.SenderId.ToString()).ApproveFriendshipRequest(dto);
        }
        public override ServiceResponse<FriendshipResponseDto> MapFromEntity(FriendshipRequestEntity e) => new ServiceResponse<FriendshipResponseDto>(new FriendshipResponseDto(e));
    }
}