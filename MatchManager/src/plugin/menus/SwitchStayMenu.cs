using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using MatchManager.api.plugin;

namespace MatchManager.plugin.menus;

public sealed class SwitchStayMenu : CenterHtmlMenu
{
    public SwitchStayMenu(BasePlugin baseplugin, IMatchManager plugin) : base("Pick a side to start", baseplugin)
    {
        AddMenuOption("Switch", (executor ,_) =>
        {
            plugin.getTeamsService().SwitchSides();
            plugin.getAnnouncer().Announce("knife_switch", executor.PlayerName);
            plugin.getMatchService().BeginMain();
        });
        AddMenuOption("Stay", (executor, _) =>
        {
            plugin.getAnnouncer().Announce("knife_stay", executor.PlayerName);
            plugin.getMatchService().BeginMain();
        });
    }
}