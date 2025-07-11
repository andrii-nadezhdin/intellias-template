namespace Intellias.Template.Application.Exceptions
{
    using System;
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }
        public ApplicationException(string message) : base(message) { }
    }
}
