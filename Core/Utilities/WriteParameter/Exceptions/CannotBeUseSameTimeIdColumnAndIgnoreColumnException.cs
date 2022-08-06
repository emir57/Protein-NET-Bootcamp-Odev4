namespace WriteParameter.Exceptions
{
    public class CannotBeUseSameTimeIdColumnAndIgnoreColumnException : Exception
    {
        public CannotBeUseSameTimeIdColumnAndIgnoreColumnException() : base("Cannot be use same time IdColumnAttribute and IgnoreColumnAttribute")
        {
        }

        public CannotBeUseSameTimeIdColumnAndIgnoreColumnException(string? message) : base(message)
        {
        }

        public CannotBeUseSameTimeIdColumnAndIgnoreColumnException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
