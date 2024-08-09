using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using MatchManager.api.plugin;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.listeners;

public class JoinTeamListener
{
    private readonly IMatchManager plugin;
    public JoinTeamListener(IMatchManager plugin)
    {
        this.plugin = plugin;
        
        plugin.getBase().AddCommandListener("jointeam", OnJoinTeam);
    }

    private HookResult OnJoinTeam(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsReal()) return HookResult.Continue;
        if (plugin.getMatchService().GetState() < MatchState.Warmup) return HookResult.Continue;

        var desiredTeam = plugin.getTeamsService().GetTeams().FirstOrDefault(t => t.TeamMembers.Contains(player))?.CurrentSide ?? CsTeam.Spectator;
        var desiredTeamString = desiredTeam switch { CsTeam.Terrorist => "t", CsTeam.CounterTerrorist => "ct", _ => "spectator" };
        var selectedTeam = info.ArgByIndex(1) switch { "t" => CsTeam.Terrorist, "ct" => CsTeam.CounterTerrorist, _ => CsTeam.Spectator };

        if (desiredTeam == selectedTeam) return HookResult.Continue;

        //Done this way to close the menu
        player.ExecuteClientCommand($"jointeam {desiredTeamString}");
        return HookResult.Stop;
    }
}