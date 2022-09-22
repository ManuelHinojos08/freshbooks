namespace Application.Common.Exceptions;

public class ForbiddenContentException : Exception
{
    public ForbiddenContentException()
        : base($"The content you are trying to query is not available!")
    {
    }
}