using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands.match;

public class ForceReadyCmd(IMatchManager plugin) : Command(plugin)
{
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (plugin.getMatchService().GetState() != MatchState.Warmup)
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "warmup_not_started");
            return;
        }
        plugin.getMatchService().InitiateMatch();
    }
}