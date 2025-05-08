using BitzArt.ApiExceptions;
using System.Text.Json;

namespace BitzArt.Messages;

/// <summary>
/// Extension methods for the <see cref="ResponseMessage"/> class.
/// </summary>
public static class ResponseMessageExtensions
{
    /// <summary>
    /// Determines whether the response message has a success status code.
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <returns></returns>
    public static bool IsSuccessStatusCode(this ResponseMessage responseMessage)
        => responseMessage.StatusCode.HasValue
        && responseMessage.StatusCode.Value.IsSuccessStatusCode();

    private static bool IsSuccessStatusCode(this ApiStatusCode statusCode)
        => (int)statusCode >= 200 && (int)statusCode <= 299;

    /// <summary>
    /// Casts the response message to the <see cref="Problem"/> data type.
    /// </summary>
    public static Problem ToProblem(this ResponseMessage message)
        => message.Cast<Problem>()!.Data!;

    /// <summary>
    /// Casts the response message to the specified data type,
    /// or to the <see cref="Problem"/> type if the status code is not successful.
    /// </summary>
    public static ResponseMessage CastByStatus<TData>(this ResponseMessage message)
        where TData : class
    {
        if (message.IsSuccessStatusCode()) return message.Cast<TData>();

        return message.Cast<Problem>();
    }

    /// <summary>
    /// Casts the response message to the specified data type.
    /// </summary>
    public static ResponseMessage<TData> Cast<TData>(this ResponseMessage message)
        where TData : class
    {
        var dataJsonElement = (JsonElement)message.Data!;
        var dataCasted = dataJsonElement.Deserialize<TData>()!;

        return new ResponseMessage<TData>(message.StatusCode!.Value, dataCasted);
    }
}
