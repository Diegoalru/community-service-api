using System;

namespace community_service_api.Exceptions;

public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message)
    {
    }

    public DataAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}