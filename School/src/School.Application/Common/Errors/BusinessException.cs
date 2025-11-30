namespace School.Application.Common.Errors
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
        }
    }
}
