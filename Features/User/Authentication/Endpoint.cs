using FastEndpoints;
using Scandium.Data;
using Scandium.Helpers;
using Scandium.Services.Abstract;

namespace Scandium.Features.User.Authentication
{
    public class Endpoint : Endpoint<Request,Response>
    {
        private readonly IUserRepo userRepo;
        private readonly IJwtService jwtService;

        public Endpoint(IUserRepo userRepo,IJwtService jwtService)
        {
            this.userRepo = userRepo;
            this.jwtService = jwtService;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/authentication");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            string hashedPassword = MD5HashHelper.Create(req.Password!);
            var user  = await userRepo.GetAsync(x=> x.Username == req.Username && x.Password == hashedPassword);
            if(user != null){
                await SendAsync (new Response(){
                    Id = user.Id,
                    Username = user.Username,
                    Token = jwtService.Create(user.Id)
                });
            }
            else throw new KeyNotFoundException("User not found !");

        }
    }
}