using FastEndpoints;
using Scandium.Data;
using Scandium.Data.Abstract;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using UserEntity = Scandium.Model.Entities.User;


namespace Scandium.Features.User.Searching
{
    public class Endpoint: EndpointWithMapping<Request, ServiceResponse<List<UserResponseDto>>,UserEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IFriendshipRequestRepository friendshipRequestRepository;
        private readonly IUserRepo userRepo;

        public Endpoint(IHttpContextService httpContextService, IUserRepo userRepo, IFriendshipRequestRepository friendshipRequestRepository)
        {
            this.httpContextService = httpContextService;
            this.friendshipRequestRepository = friendshipRequestRepository;
            this.userRepo = userRepo;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/user/searching");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var searchKey= req.Username?.Trim().ToLower() ?? String.Empty;
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var users = await userRepo.GetListAsync(x=> x.Username!.ToLower().Contains(searchKey) && x.Id != currentUserId && !x.ReceiverFriendshipRequests!.Any(x=> x.SenderId == currentUserId));
            var userDtos =users.Select(r => UserResponseDto.Get(r)!).ToList();
            var response = new ServiceResponse<List<UserResponseDto>>(userDtos);
            await SendAsync(response);
        }
    }
}