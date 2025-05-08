using System.Text.Json.Serialization;
using BitzArt.ApiExceptions;

namespace BitzArt.Messages;

/// <summary>
/// Represents a response message.
/// </summary>
public class ResponseMessage
{
    private object? _data;

    /// <summary>
    /// Response status code.
    /// </summary>
    [JsonPropertyName("status")]
    public ApiStatusCode? StatusCode { get; set; }

    /// <summary>
    /// Response data.
    /// </summary>
    [JsonPropertyName("data")]
    public virtual object? Data
    {
        get => GetDataInternal();
        set => SetDataInternal(value);
    }

    private protected virtual object? GetDataInternal() => _data;
    private protected virtual void SetDataInternal(object? value) => _data = value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage"/> class with a custom status code.
    /// </summary>
    /// <param name="statusCode"></param>
    public ResponseMessage(int statusCode) : this()
    {
        StatusCode = (ApiStatusCode)statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage"/> class with custom status code.
    /// </summary>
    /// <param name="apiStatusCode"></param>
    public ResponseMessage(ApiStatusCode apiStatusCode) : this((int)apiStatusCode) { }

    /// <summary>
    /// Initializes a new empty instance of the <see cref="ResponseMessage"/> class.
    /// This constructor is used in deserialization.
    /// </summary>
    public ResponseMessage() { }
}

/// <summary>
/// Represents a response message.
/// </summary>
/// <typeparam name="TData"></typeparam>
public class ResponseMessage<TData> : ResponseMessage
    where TData : class
{
    private TData? _data;

    /// <summary>
    /// Response data.
    /// </summary>
    [JsonPropertyName("data")]
    public new TData? Data
    {
        get => _data;
        set => _data = value;
    }

    private protected override object? GetDataInternal() => _data;
    private protected override void SetDataInternal(object? value)
    {
        if (value is null)
        {
            _data = null;
            return;
        }

        if (value is not TData dataCasted)
            throw new InvalidOperationException(
                $"An attempt was made to set the data of an incorrect type '{value.GetType().Name}' to the ResponseMessage<{typeof(TData).Name}>");

        _data = dataCasted;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage{TData}"/> class with a custom status code.
    /// </summary>
    /// <param name="apiStatusCode">The status code to be sent in the response.</param>
    /// <param name="data">The data to be sent in the response.</param>
    public ResponseMessage(ApiStatusCode apiStatusCode, TData data) : this((int)apiStatusCode, data)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage{TData}"/> class with a custom status code.
    /// </summary>
    /// <param name="statusCode">The status code to be sent in the response.</param>
    /// <param name="data">The data to be sent in the response.</param>
    public ResponseMessage(int statusCode, TData data) : base(statusCode)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMessage{TData}"/> class with a status code of 200.
    /// </summary>
    /// <param name="data">The data to be sent in the response.</param>
    public ResponseMessage(TData data) : this(ApiStatusCode.OK, data) { }

    /// <summary>
    /// Initializes a new empty instance of the <see cref="ResponseMessage{TData}"/>.
    /// This constructor is used in deserialization.
    /// </summary>
    public ResponseMessage() { }
}