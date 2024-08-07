using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;

namespace MatchManager.plugin.commands;

public abstract class Command(IMatchManager plugin)
{
    protected readonly IMatchManager plugin = plugin;
    public string? Description => null;
    public abstract void OnCommand(CCSPlayerController? executor, CommandInfo info);
}
