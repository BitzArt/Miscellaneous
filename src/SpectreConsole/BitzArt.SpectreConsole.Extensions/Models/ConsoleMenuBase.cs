namespace BitzArt.Console;

public abstract class ConsoleMenuBase : IConsoleMenu
{
    public abstract string Title { get; }

    public virtual void Run()
    {
        AnsiConsoleMenu.WriteTitle(Title);
    }
}
