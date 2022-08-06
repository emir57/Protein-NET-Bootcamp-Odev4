namespace WriteParameter.Exceptions
{
    public class NoSelectedTableException : Exception
    {
        public NoSelectedTableException() : this("No selected table")
        {

        }

        public NoSelectedTableException(string? message) : base(message)
        {
        }

        public NoSelectedTableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
