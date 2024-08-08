using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands.teams;

public class CaptainCmd(IMatchManager plugin) : Command(plugin)
{
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (!executor.IsEc()) return;
        var target = GetSingleTarget(info);
        if (target == null) return;

        if (!plugin.getTeamsService().SetTeamCaptain(target))
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "command_captain_unable_to_set");
            return;
        }
        info.ReplyLocalized(plugin.getBase().Localizer, "command_captain_set", target.PlayerName);
        plugin.getAnnouncer().Announce("command_captain_set_all", target.PlayerName);
    }
}