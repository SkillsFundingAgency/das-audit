using System;

namespace SFA.DAS.Audit.Domain.Data
{
    public class DataStorageException : Exception
    {
        public DataStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
