namespace Scandium.Model.Dto
{
    public class UserResponseDto
    {
        public UserResponseDto(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }

        public Guid Id { get; set; }
        public string? Username { get; set; }
    }
}