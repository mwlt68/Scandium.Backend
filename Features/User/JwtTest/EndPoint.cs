using FastEndpoints;

namespace Scandium.Features.User.JwtTest
{
    public class Endpoint : Endpoint<Request,Response>{
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/user/test");
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            await SendAsync(new Response(){
                Id =22
            });
        }

    }
}