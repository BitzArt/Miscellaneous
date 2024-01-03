using Spectre.Console;
using System.Text;

namespace BitzArt.Console;

public abstract class ConsoleRecurrentMenuBase : ConsoleMenuBase
{
    protected record RecurrentAction(string Title, Action Action);

    protected virtual Dictionary<ConsoleKey, RecurrentAction> Actions { get; set; } = [];

    protected virtual bool MainMenu => false;
    private string ExitMenuItem => MainMenu ? "ESC - Exit" : "ESC - Back";

    protected void AddAction(ConsoleKey key, string title, Action action)
    {
        Actions.Add(key, new RecurrentAction(title, action));
    }

    private string GetActionMenu()
    {
        var result = new StringBuilder();

        foreach (var action in Actions)
        {
            result.AppendLine($"{action.Key} - {action.Value.Title}");
        }

        return result.ToString();
    }

    public virtual string FullMenu =>
        $"""
        {GetActionMenu()}
        
        {ExitMenuItem}
        """;

    public override void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsoleMenu.WriteTitle(Title);

            AnsiConsoleMenu.WriteMenuItems(FullMenu);

            var key = System.Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape) return;

            var actionExists = Actions.TryGetValue(key, out var action);
            if (!actionExists || action is null) continue;

            action.Action.Invoke();
        }
    }
}
