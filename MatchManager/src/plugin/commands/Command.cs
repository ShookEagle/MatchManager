using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands;

public abstract class Command(IMatchManager plugin)
{
    protected readonly IMatchManager plugin = plugin;
    public string? Description => null;
    public abstract void OnCommand(CCSPlayerController? executor, CommandInfo info);
    internal CCSPlayerController? GetSingleTarget(CommandInfo command, int argIndex = 1,
        bool print = true) {
        var matches = command.GetArgTargetResult(argIndex);

        if (!matches.Any()) {
            if (print)
                command.ReplyLocalized(plugin.getBase().Localizer, "player_not_found",
                    command.GetArg(argIndex));
            return null;
        }

        if (matches.Count() > 1) {
            if (print)
                command.ReplyLocalized(plugin.getBase().Localizer,
                    "player_found_multiple", command.GetArg(argIndex));
            return null;
        }

        var target = matches.Players.FirstOrDefault(p => p == command.CallingPlayer);
        if (target == null)
        {
            command.ReplyLocalized(plugin.getBase().Localizer, "error_player_null");
            return null;
        }

        return matches.Players.FirstOrDefault(p => p== command.CallingPlayer);
    }


}
