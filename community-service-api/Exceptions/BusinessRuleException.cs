using System;

namespace community_service_api.Exceptions;

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }

    public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}