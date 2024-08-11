using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using MatchManager.api.plugin;
using MatchManager.api.plugin.services;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;
using MatchManager.plugin.menus;
using MatchManager.plugin.utils;

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
        CfgUtils.ExecWarmupSettings();
    }

    public void InitiateWarmup()
    {
        _state = MatchState.Warmup;
        var teams = plugin.getTeamsService().GetTeams();
        CfgUtils.ExecWarmupSettings();

        foreach (var team in teams)
        {
            foreach (var player in team.TeamMembers)
            {
                player.SwitchTeam(team.CurrentSide);
            }
        }

        plugin.getBase().AddTimer(13f, () =>
        {
            plugin.getAnnouncer().Announce("warmup_ready_up", _readyPlayers.Count.ToString(), TimerFlags.REPEAT);
        });
        
        ServerExtensions.GetGameRules(out var gamerules, out var proxy);
        if (gamerules == null || proxy == null) return;
        
        gamerules.WarmupPeriod = true;
        Utilities.SetStateChanged(proxy, "CCSGameRulesProxy", "m_pGameRules");
    }

    public void InitiateMatch()
    {
        _state = MatchState.MainMatch;
        _readyPlayers.Clear();
        plugin.getBase().Timers.Clear();
        
        ServerExtensions.GetGameRules(out var gamerules, out var proxy);
        if (gamerules == null || proxy == null) return;
        
        gamerules.WarmupPeriod = false;
        Utilities.SetStateChanged(proxy, "CCSGameRulesProxy", "m_pGameRules");
        CfgUtils.ExecMatchSettings();
        KnifeRound();
    }

    public void BeginMain()
    {
        plugin.getBase().Timers.Clear();
        CfgUtils.ExecMatchSettings();
        plugin.getBase().DeregisterEventHandler<EventRoundEnd>(RoundListener);
        Server.ExecuteCommand("mp_restartgame 1");
        plugin.getBase().RegisterEventHandler<EventStartHalftime>(HalfTime);
    }

    private void KnifeRound()
    {
        CfgUtils.ExecKnifeSettings();
        Server.ExecuteCommand("mp_restartgame 1");
        plugin.getAnnouncer().Announce("knife_begin");
        plugin.getBase().RegisterEventHandler<EventRoundEnd>(RoundListener, HookMode.Pre);
    }

    private HookResult RoundListener(EventRoundEnd @event, GameEventInfo info)
    {
        var winningCaptain = plugin.getTeamsService().GetTeams().First(t => t.CurrentSide == (CsTeam)@event.Winner).TeamCaptain;
        if (winningCaptain == null) return HookResult.Stop;
        
        plugin.getAnnouncer().Announce("knife_choosing_side", winningCaptain.PlayerName);
        
        winningCaptain.OpenMenu(new SwitchStayMenu(plugin.getBase(), plugin));
        
        return HookResult.Stop;
    }

    private HookResult HalfTime(EventStartHalftime @event, GameEventInfo info)
    {
        plugin.getTeamsService().SwitchSides();
        return HookResult.Continue;
    }
}