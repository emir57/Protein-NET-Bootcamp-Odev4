namespace WriteParameter.Exceptions
{
    public class NoSelectedSchemaException : Exception
    {
        public NoSelectedSchemaException() : base("No selected schema")
        {
        }

        public NoSelectedSchemaException(string? message) : base(message)
        {
        }

        public NoSelectedSchemaException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
