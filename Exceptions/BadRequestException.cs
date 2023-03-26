
namespace Scandium.Exceptions
{
    public class BadRequestException : CustomBaseException
    {
        public BadRequestException(string? message = "Bad Request Error !") : base("Bad Request",message) { }
    }
}