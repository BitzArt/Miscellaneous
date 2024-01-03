using BitzArt.Console;
using BitzArt.SpectreConsole.Menus;

namespace BitzArt.SpectreConsole;

internal class MainMenu : ConsoleRecurrentMenuBase
{
    public override string Title => "Main Menu";

    private readonly SubMenu1 _submenu1;
    private readonly SubMenu2 _submenu2;

    public MainMenu(SubMenu1 submenu1, SubMenu2 submenu2)
    {
        _submenu1 = submenu1;
        _submenu2 = submenu2;

        AddAction(ConsoleKey.D1, "Test 1", _submenu1.Run);
        AddAction(ConsoleKey.D2, "Test 2", _submenu2.Run);
    }
}
