namespace BitzArt.Json.TypedValues.Tests;

public record Apple : Fruit
{
    private readonly string _color;

    public Apple(string color)
    {
        _color = color;
    }

    public override string Type => nameof(Apple);

    public override string Color => _color;

    public string Variety { get; set; } = "Fuji";
}
