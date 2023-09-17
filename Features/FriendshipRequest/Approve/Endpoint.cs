using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Exceptions;
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

        public Endpoint(IHttpContextService httpContextService, IFriendshipRequestRepository friendshipRequestRepository)
        {
            this.httpContextService = httpContextService;
            this.friendshipRequestRepository = friendshipRequestRepository;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/friendship/approve");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var friendshipRequests = await friendshipRequestRepository.GetListAsync(x => !x.IsApproved && (x.SenderId == req.SenderId && x.ReceiverId == currentUserId) || (x.SenderId == currentUserId && x.ReceiverId == req.SenderId));

            if (friendshipRequests.Count() > 0)
            {
                var requestWillApprove = friendshipRequests.FirstOrDefault(x => x.ReceiverId == currentUserId);
                if(requestWillApprove != null){
                    requestWillApprove.IsApproved = true;
                    await friendshipRequestRepository.UpdateAsync(requestWillApprove);
                    // Remove oher request send by same user
                    if (friendshipRequests.Count() > 1)
                    {
                        foreach (var request in friendshipRequests)
                        {
                            if (request.Id != requestWillApprove.Id)
                                await friendshipRequestRepository.DeleteAsync(request.Id);
                        }
                    }
                    var response = MapFromEntity(requestWillApprove);
                    await SendAsync(response);
                    return;
                }
            }
            throw new NotFoundException("Friendship request not found !");
        }
        public override ServiceResponse<FriendshipResponseDto> MapFromEntity(FriendshipRequestEntity e) => new ServiceResponse<FriendshipResponseDto>(new FriendshipResponseDto(e));
    }
}