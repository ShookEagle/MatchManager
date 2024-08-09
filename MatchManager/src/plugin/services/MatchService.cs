using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MatchManager.api.plugin;
using MatchManager.api.plugin.services;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.services;

public class MatchService(IMatchManager plugin) : IMatchService
{
    private MatchState _state = MatchState.PreMatch;
    private readonly HashSet<CCSPlayerController> _readyPlayers = new();

    public MatchState GetState()
    {
        return _state;
    }

    public void SetState(MatchState state)
    {
        _state = state;
    }

    public bool ReadyPlayer(CCSPlayerController player)
    {
        var result = _readyPlayers.Add(player);
        if (_readyPlayers.Count == 10)
        {
            plugin.getAnnouncer().Announce("draft_finished");
            plugin.getAnnouncer().Announce("warmup_all_ready");
            InitiateMatch();
        }
        return result;
    }

    public void InitiatePreMatch()
    {
        ServerExtensions.GetGameRules(out var gamerules, out var proxy);
        if (gamerules == null || proxy == null) return;
    }

    public void InitiateWarmup()
    {
        _state = MatchState.Warmup;
        var teams = plugin.getTeamsService().GetTeams();

        foreach (var team in teams)
        {
            foreach (var player in team.TeamMembers)
            {
                player.SwitchTeam(team.CurrentSide);
            }
        }

        plugin.getBase().AddTimer(13f, () =>
        {
            plugin.getAnnouncer().Announce("warmup_ready_up", _readyPlayers.Count.ToString());
        });
        
        ServerExtensions.GetGameRules(out var gamerules, out var proxy);
        if (gamerules == null || proxy == null) return;
        
        gamerules.WarmupPeriod = true;
        Utilities.SetStateChanged(proxy, "CCSGameRulesProxy", "m_pGameRules");
    }

    public void InitiateMatch()
    {
        _readyPlayers.Clear();
        
        
    }
}