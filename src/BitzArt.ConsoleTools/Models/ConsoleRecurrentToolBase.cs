using System.Text;

namespace BitzArt.ConsoleTools;

public abstract class ConsoleRecurrentToolBase : ConsoleToolBase
{
    protected record RecurrentAction(string Title, Action Action);

    protected virtual Dictionary<ConsoleKey, RecurrentAction> Actions { get; set; } = [];

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
        
        ESC - Back
        """;

    public override void RunTool()
    {
        while (true)
        {
            ConsoleEx.WriteMenu(FullMenu);

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape) return;

            var actionExists = Actions.TryGetValue(key, out var action);
            if (!actionExists || action is null) continue;

            action.Action.Invoke();
        }
    }
}
