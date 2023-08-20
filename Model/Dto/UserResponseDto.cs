using Scandium.Model.Entities;

namespace Scandium.Model.Dto
{
    public class UserResponseDto
    {
        public static UserResponseDto? Get(User? user)
        {
            if (user == null) return null;
            else return new UserResponseDto()
            {
                Id = user.Id,
                Username = user.Username
            };

        }

        public Guid Id { get; set; }
        public string? Username { get; set; }
    }
}