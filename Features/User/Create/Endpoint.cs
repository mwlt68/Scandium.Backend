using FastEndpoints;
using Scandium.Data;
using Scandium.Exceptions;
using Scandium.Services.Abstract;

namespace Scandium.Features.User.Create
{
    public class Endpoint : Endpoint<Request, Response, Mapper>
    {
        private readonly IJwtService jwtService;
        private readonly IUserRepo userRepo;
        public Endpoint(IJwtService jwtService, IUserRepo userRepo)
        {
            this.userRepo = userRepo;
            this.jwtService = jwtService;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/user/insert");
            AllowAnonymous();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            bool usernameUnique = !await userRepo.AnyAsync(x => x.Username == req.Username);
            if (usernameUnique)
            {
                var user = Map.ToEntity(req);
                var addedUser = await userRepo.AddAsync(user);
                await SendAsync(new Response()
                {
                    Token = jwtService.Create(addedUser.Id),
                    Id = addedUser.Id,
                    Username = addedUser.Username
                });
            }
            else throw new BadRequestException("Username must be unique !");
        }
    }
}