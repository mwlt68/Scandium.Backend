
namespace Scandium.Exceptions
{
    public class InternalServerException : CustomBaseException
    {
        public InternalServerException(string? message = "Internal Server Error !") : base("Internal Server",message) { }
    }
}