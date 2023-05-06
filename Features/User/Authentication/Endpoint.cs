using FastEndpoints;
using Scandium.Data;
using Scandium.Exceptions;
using Scandium.Helpers;
using Scandium.Model.BaseModels;
using Scandium.Services.Abstract;

namespace Scandium.Features.User.Authentication
{
    public class Endpoint : Endpoint<Request,ServiceResponse<Response>>
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
            Verbs(Http.POST);
            Routes("/authentication");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            string hashedPassword = MD5HashHelper.Create(req.Password!);
            var user  = await userRepo.GetAsync(x=> x.Username == req.Username && x.Password == hashedPassword);
            if(user != null){
                var response = new Response(){
                    Id = user.Id,
                    Username = user.Username,
                    Token = jwtService.Create(user.Id)
                };
                await SendAsync (new ServiceResponse<Response>(response));
            }
            else throw new NotFoundException(typeof(Scandium.Model.User));

        }
    }
}