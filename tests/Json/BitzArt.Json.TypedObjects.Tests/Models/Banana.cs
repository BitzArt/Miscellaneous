namespace BitzArt.Json.TypedObjects.Tests;

public record Banana : Fruit
{
    public override string Type => nameof(Banana);

    public override string Color => "Yellow";

    public string Variety { get; set; } = "Cavendish";
}
