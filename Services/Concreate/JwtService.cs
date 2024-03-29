using System.Security.Claims;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using Scandium.Model.Settings;
using Scandium.Services.Abstract;

namespace Scandium.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings? jwtSettings;
        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }
        public string Create(Guid userId)
        {
            if(jwtSettings?.Key == null)
                throw new Exception("JWT key not found !");
                
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier,userId.ToString());
            var jwtToken = JWTBearer.CreateToken(
                signingKey: jwtSettings.Key,
                expireAt: DateTime.UtcNow.AddDays(jwtSettings.ExpireInDay),
                claims: new[] {userIdClaim});
            return jwtToken;
        }
    }
}