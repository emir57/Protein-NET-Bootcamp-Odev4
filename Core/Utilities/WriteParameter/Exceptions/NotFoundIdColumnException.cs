namespace WriteParameter.Exceptions
{
    public class NotFoundIdColumnException : Exception
    {
        public NotFoundIdColumnException() : base("Not found id column")
        {
        }

        public NotFoundIdColumnException(string? message) : base(message)
        {
        }

        public NotFoundIdColumnException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
