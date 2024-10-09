namespace TinyUrl.AuthenticationService.Infrastructure.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}
