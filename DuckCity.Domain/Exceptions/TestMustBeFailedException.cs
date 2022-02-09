using System.Runtime.Serialization;

namespace DuckCity.Domain.Exceptions;

public class TestMustBeFailedException : Exception
{
    public TestMustBeFailedException() : 
        base("Test must be failed")            
    {
    }

    public TestMustBeFailedException(string message)
        :base(message)
    {
    }

    public TestMustBeFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected TestMustBeFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

}