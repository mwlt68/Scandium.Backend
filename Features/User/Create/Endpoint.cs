using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Scandium.Data;

namespace Scandium.Features.User.Create
{
    public class Endpoint : Endpoint<Request,Response,Mapper>
    {
        private readonly IUserRepo userRepo;
        public Endpoint(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/user/insert");
            AllowAnonymous();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            bool usernameUnique = !await userRepo.AnyAsync(x=> x.Username == req.Username);
            if(usernameUnique){
                var user = Map.ToEntity(req);
                var addedUser = await userRepo.AddAsync(user);
                if(addedUser != null){
                    await SendAsync(new Response(){
                        Token = "Token",
                        Id = addedUser.Id,
                        Username = addedUser.Username
                    });
                }else throw new Exception("User insert exception !");
            }
            else throw new Exception("Username must be unique !");
        }
    }
}