using BitzArt.ApiExceptions;

namespace BitzArt.Messages;

internal static class ApiExceptionExtensions
{
    public static ResponseMessage<Problem> ToResponseMessage(this ApiExceptionBase ex)
        => new(ex.StatusCode, new Problem(ex));
}
