namespace Application.Common.Extensions;

public static class ExceptionExtensions
{
    public static Exception Unwrap(this Exception @this)
    {
        var ret = @this;

        while (ret.InnerException != null)
            ret = ret.InnerException;

        return ret;
    }
}