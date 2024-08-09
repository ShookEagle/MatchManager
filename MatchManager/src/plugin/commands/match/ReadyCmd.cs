using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands.match;

public class ReadyCmd(IMatchManager plugin) : Command(plugin)
{
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (executor == null || !executor.IsReal()) return;
        if (plugin.getMatchService().GetState() != MatchState.Warmup)
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "warmup_not_started");
            return;
        }
        if (!plugin.getMatchService().ReadyPlayer(executor))
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "warmup_already_ready");
            return;
        }
        plugin.getAnnouncer().Announce("warmup_ready", executor.PlayerName);
    }
}