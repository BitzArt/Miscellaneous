namespace BitzArt.Probes;

public static class ProbeMessages
{
    private static readonly string[] All =
    {
        "It's good to be alive!",
        "As seen on TV!",
        "Limited edition!",
        "One of a kind!",
        "It's a service!",
        "Exclusive!",
        "Closed source!",
        "Classy!",
        "Wow!",
        "Not on Steam!",
        "Awesome community!",
        "Enhanced!",
        "Absolutely no memes!",
        "Fat free!",
        "Cloud computing!",
        "日本ハロー！",
        "한국 안녕하세요!",
        "Helo Cymru!",
        "Cześć Polsko!",
        "你好中国！",
        "Γεια σου Ελλάδα!",
        "l33t!",
        "|-|3110!",
        "Look mum, I'm in a probe!",
        "Water proof!",
        "Uninflammable!",
        "Tell your friends!",
        "Random probe!",
        "Try it!",
    };

    private static readonly int Count = All.Length;

    private static readonly Random RNG = new();

    public static string GetOne() => All[RNG.Next(0, Count)];
}
