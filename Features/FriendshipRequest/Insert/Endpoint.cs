using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Scandium.Data.Abstract;
using Scandium.Exceptions;
using Scandium.Extensions.EntityExtensions;
using Scandium.Hubs;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Insert
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<FriendshipResponseDto>, FriendshipRequestEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IFriendshipRequestRepository friendshipRequestRepository;
        private readonly IHubContext<FriendshipRequestHub, IFriendshipRequesClient> friendshipHub;

        public Endpoint(IHttpContextService httpContextService, IFriendshipRequestRepository friendshipRequestRepository,IHubContext<FriendshipRequestHub, IFriendshipRequesClient> friendshipHub)
        {
            this.httpContextService = httpContextService;
            this.friendshipRequestRepository = friendshipRequestRepository;
            this.friendshipHub = friendshipHub;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/friendship");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUser = httpContextService.GetUserIdFromClaims();
            var isRequestExist = await friendshipRequestRepository.AnyAsync(x => (x.ReceiverId == currentUser && x.SenderId == req.ReceiverId && x.IsApproved) || (x.SenderId == currentUser && x.ReceiverId == req.ReceiverId));
            if (!isRequestExist)
            {
                var request = MapToEntity(req);
                await friendshipRequestRepository.AddAsync(request);
                var addedRequest = await friendshipRequestRepository.GetByIdAsync(request.Id).ThrowIfNotFound();
                var response = MapFromEntity(addedRequest);
                await RunGetFriendshipRequest(addedRequest);
                await SendAsync(response);
            }
            else throw new BadRequestException("AlreadyFriend","This user is already your friend!");
        }

        private async Task RunGetFriendshipRequest(FriendshipRequestEntity addedMessage)
        {
            var dto  =  new FriendshipResponseDto(addedMessage);
            await friendshipHub.Clients.Group(addedMessage.ReceiverId.ToString()).GetFriendshipRequest(dto);
        }

        public override FriendshipRequestEntity MapToEntity(Request r) => new()
        {
            SenderId = httpContextService.GetUserIdFromClaims(),
            ReceiverId = r.ReceiverId,
        };
        public override ServiceResponse<FriendshipResponseDto> MapFromEntity(FriendshipRequestEntity e) => new ServiceResponse<FriendshipResponseDto>(new FriendshipResponseDto(e));
    }
}