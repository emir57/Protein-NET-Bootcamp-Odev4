namespace WriteParameter.Exceptions
{
    internal class MoreThanOneIdColumnException : Exception
    {
        public MoreThanOneIdColumnException() : base("More than one id column")
        {
        }

        public MoreThanOneIdColumnException(string? message) : base(message)
        {
        }

        public MoreThanOneIdColumnException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
