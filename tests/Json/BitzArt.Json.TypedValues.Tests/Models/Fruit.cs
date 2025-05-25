namespace BitzArt.Json.TypedValues.Tests;

public abstract record Fruit
{
    public abstract string Type { get; }

    public abstract string Color { get; }

    public override string ToString()
    {
        return $"{Type} ({Color})";
    }
}