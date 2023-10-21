using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using Scandium.Model.BaseModels;
using Scandium.Model.Dto;
using Scandium.Services.Abstract;
using UserEntity = Scandium.Model.Entities.User;


namespace Scandium.Features.User.Searching
{
    public class Endpoint : EndpointWithMapping<Request, ServiceResponse<List<UserSearchResponseDto>>, UserEntity>
    {
        private readonly IHttpContextService httpContextService;
        private readonly IUserRepo userRepo;

        public Endpoint(IHttpContextService httpContextService, IUserRepo userRepo)
        {
            this.httpContextService = httpContextService;
            this.userRepo = userRepo;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/user/searching");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var currentUserId = httpContextService.GetUserIdFromClaims();
            var searchKey = req.Username?.Trim().ToLower();
            List<UserSearchResponseDto> users = new();
            if (!String.IsNullOrWhiteSpace(searchKey)){
                var userList = await userRepo.GetDefaultQueyable().Include(x=> x.SenderFriendshipRequests).Include(x=> x.ReceiverFriendshipRequests).Where(x => x.Username!.ToLower().Contains(searchKey) && x.Id != currentUserId).ToListAsync();

                userList.ForEach((x)=>{

                    FriendshipRequestStatus status;
                    Model.Entities.FriendshipRequest? request=null;
                    bool isFollowing = x.SenderFriendshipRequests!.Any(y=> y.IsApproved && y.ReceiverId == currentUserId) || x.ReceiverFriendshipRequests!.Any(y=> y.IsApproved && y.SenderId == currentUserId);
                    if (isFollowing)
                        status=FriendshipRequestStatus.Following;
                    else {
                        bool doesAnyRequest = x.SenderFriendshipRequests!.Any(y=> y.ReceiverId == currentUserId) || x.ReceiverFriendshipRequests!.Any(y=>  y.SenderId == currentUserId);
                        if(!doesAnyRequest)
                        {
                            status=FriendshipRequestStatus.Follow;
                            request =  x.SenderFriendshipRequests!.FirstOrDefault(y=> y.ReceiverId == currentUserId);
                            if(request is null){
                                request = x.ReceiverFriendshipRequests!.FirstOrDefault(y=>  y.SenderId == currentUserId);
                            }
                        }
                        else{
                            bool doesAnySendedRequest =  x.SenderFriendshipRequests!.Any(y=>  y.ReceiverId == currentUserId);
                            if(doesAnySendedRequest)
                            {
                                status=FriendshipRequestStatus.Approve;
                                request = x.SenderFriendshipRequests!.FirstOrDefault(y=>  y.ReceiverId == currentUserId);

                            }
                            else{
                                status=FriendshipRequestStatus.Requested;
                                request = x.ReceiverFriendshipRequests!.FirstOrDefault(y=>  y.SenderId == currentUserId);
                            }

                        }

                    }
                    var userModel = UserResponseDto.Get(x);
                    var searchModel = new UserSearchResponseDto(userModel!,status,request?.Id,request?.ReceiverId);
                    users.Add(searchModel);
                });
            }
            var response = new ServiceResponse<List<UserSearchResponseDto>>(users);
            await SendAsync(response);
        }
    }
}