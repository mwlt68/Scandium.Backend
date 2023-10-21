
namespace Scandium.Exceptions
{
    public class BadRequestException : CustomBaseException
    {
        public BadRequestException(string? title = "BadRequest",string? content = "Bad Request Error !") : base(title,content) { }
    }
}