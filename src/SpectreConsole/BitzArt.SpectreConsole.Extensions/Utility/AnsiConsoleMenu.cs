using Spectre.Console;

namespace BitzArt.Console;

public static class AnsiConsoleMenu
{
    public static void WriteTitle(string value)
        => WriteLine($"\n====== {value.ToUpper()} ======\n", "yellow");

    public static void WriteMenuItems(string value)
        => WriteLine($"\n---------------------------\n{value}\n---------------------------\n", "cyan");

    public static void WriteAction(string value)
        => WriteLine(value, "cyan");

    private static void WriteLine(string value, string color)
    {
        AnsiConsole.MarkupLine($"[{color}]{value}[/]");
    }
}
