namespace Core.Exceptions
{
    public class SerilogNotFoundFolderPathException : Exception
    {
        public SerilogNotFoundFolderPathException() : base("Not found folder path")
        {
        }

        public SerilogNotFoundFolderPathException(string? message) : base(message)
        {
        }

        public SerilogNotFoundFolderPathException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
