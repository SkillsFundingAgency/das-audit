using System;

namespace SFA.DAS.Audit.Client.Web
{
    public class InvalidContextException : Exception
    {
        public InvalidContextException(string message)
            : base(message)
        {
        }
    }
}
