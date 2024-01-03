namespace BitzArt.ConsoleTools;

public abstract class ConsoleToolBase : IConsoleTool
{
    public abstract string Title { get; }

    public void Run()
    {
        ConsoleEx.WriteTitle(Title);

        RunTool();
    }

    public abstract void RunTool();
}
