using System.Net;

namespace School.Application.Common.Errors
{
    public class UnauthorizedException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
