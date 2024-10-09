namespace TinyUrl.AuthenticationService.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message): base(message)
        {
            
        }
    }
}
