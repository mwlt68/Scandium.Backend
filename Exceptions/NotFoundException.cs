namespace Scandium.Exceptions
{
    public class NotFoundException : CustomBaseException
    {
        public NotFoundException(string? message = "Not Found Error !") : base("NotFound",message) { }
        public NotFoundException(Type type) : base($"{type.Name}NotFound",$"{type.Name} Not Found")  { }
    }
}