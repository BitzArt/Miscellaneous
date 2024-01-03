namespace BitzArt.ConsoleTools;

public static class ConsoleEx
{
    public static void WriteTitle(string value)
        => WriteLine($"\n====== {value.ToUpper()} ======\n", ConsoleColor.Yellow);

    public static void WriteMenu(string value)
        => WriteLine($"\n---------------------------\n{value}\n---------------------------\n", ConsoleColor.Cyan);

    public static void WriteAction(string value)
        => WriteLine(value, ConsoleColor.Cyan);

    public static void WriteLine(string value, ConsoleColor color)
    {
        // Remember previous color
        var prev = Console.ForegroundColor;

        // Set new color
        Console.ForegroundColor = color;

        // Write text
        Console.WriteLine(value);

        // Reset color
        Console.ForegroundColor = prev;
    }
}
