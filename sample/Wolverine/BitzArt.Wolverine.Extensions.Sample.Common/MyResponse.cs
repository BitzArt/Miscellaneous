namespace BitzArt.Wolverine.Extensions.Sample.Common;

public record MyResponse
{
    public required string Value { get; init; }

    public required MyRequest Request { get; init; }
}