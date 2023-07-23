using FastEndpoints;
using Scandium.Data.Abstract;
using Scandium.Exceptions;
using Scandium.Extensions.EntityExtensions;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using FriendshipRequestEntity = Scandium.Model.Entities.FriendshipRequest;

namespace Scandium.Features.FriendshipRequest.Insert
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<FriendshipRequestDto>, FriendshipRequestEntity>
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
            var currentUser = httpContextService.GetUserIdFromClaims();
            var isRequestExist = await friendshipRequestRepository.AnyAsync(x => (x.SenderId == currentUser && x.ReceiverId == req.ReceiverId) || (x.SenderId == req.ReceiverId && x.IsApproved));
            if (!isRequestExist)
            {
                var request = MapToEntity(req);
                await friendshipRequestRepository.AddAsync(request);
                var addedRequest = await friendshipRequestRepository.GetByIdAsync(request.Id).ThrowIfNotFound();
                var response = MapFromEntity(addedRequest);
                await SendAsync(response);
            }
            else throw new BadRequestException("This user is already your friend!");
        }

        public override FriendshipRequestEntity MapToEntity(Request r) => new()
        {
            SenderId = httpContextService.GetUserIdFromClaims(),
            ReceiverId = r.ReceiverId,
        };
        public override ServiceResponse<FriendshipRequestDto> MapFromEntity(FriendshipRequestEntity e) => new ServiceResponse<FriendshipRequestDto>(new FriendshipRequestDto(e));
    }
}