namespace School.Application.Common.Errors
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
