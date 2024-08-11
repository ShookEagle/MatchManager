using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using MatchManager.api.plugin;
using MatchManager.api.plugin.services;
using MatchManager.plugin.enums;
using MatchManager.plugin.extensions;
using MatchManager.plugin.menus;
using MatchManager.plugin.models;

namespace MatchManager.plugin.services;

public class TeamsService(IMatchManager plugin) : ITeamsService
{
    private Random _rng = new();
    private readonly HashSet<Team> _teams = new();
    private readonly Dictionary<CCSPlayerController, int> _draftNumbers = new();
    private bool _isCaptainDraft;
    private bool _isDraft;
    private MatchState _matchState;

    public void InitializeNewTeams()
    {
        _teams.Clear();
        _teams.Add(new Team
        {
            Id = 1, TeamName = "TERRORIST", CurrentSide = CsTeam.Terrorist,
        });
        _teams.Add(new Team
        {
            Id = 2, TeamName = "COUNTER-TERRORIST", CurrentSide = CsTeam.CounterTerrorist,
        });
    }

    public HashSet<Team> GetTeams()
    {
        return _teams;
    }

    public void SwitchSides()
    {
        (_teams.First().CurrentSide, _teams.Last().CurrentSide) = (_teams.Last().CurrentSide, _teams.First().CurrentSide);
    }

    public bool SetTeamCaptain(CCSPlayerController player)
    {
        var team = _teams.FirstOrDefault(t => t.TeamCaptain == null);
        if (team == null) return false;
        team.TeamCaptain = player;
        team.TeamMembers.Add(player);
        return true;
    }

    public void AddPlayerToCaptainTeam(CCSPlayerController captain, CCSPlayerController player)
    {
        var thisTeam = _teams.FirstOrDefault(t => t.TeamCaptain == captain);
        var otherTeam = _teams.FirstOrDefault(t => t.TeamCaptain != captain);
        if (thisTeam == null || otherTeam == null) return;
        thisTeam.TeamMembers.Add(player);

        if (otherTeam.TeamMembers.Count < 5)
        {
            otherTeam.TeamCaptain.OpenMenu(new DraftMenu(plugin.getBase(), plugin));
            return;
        }

        if (otherTeam.TeamMembers.Count == 5 && thisTeam.TeamMembers.Count == 5)
        {
            _isDraft = false;
            plugin.getAnnouncer().Announce("draft_finished");
            plugin.getAnnouncer().Announce("draft_team_1", thisTeam.TeamMembers[0].PlayerName, thisTeam.TeamMembers[1].PlayerName, thisTeam.TeamMembers[2].PlayerName, thisTeam.TeamMembers[3].PlayerName, thisTeam.TeamMembers[4].PlayerName, thisTeam.CurrentSide == CsTeam.Terrorist ? "Terrorists" : "Counter-Terrorists");
            plugin.getAnnouncer().Announce("draft_team_2", otherTeam.TeamMembers[0].PlayerName, otherTeam.TeamMembers[1].PlayerName, otherTeam.TeamMembers[2].PlayerName, otherTeam.TeamMembers[3].PlayerName, otherTeam.TeamMembers[4].PlayerName, otherTeam.CurrentSide == CsTeam.Terrorist ? "Terrorists" : "Counter-Terrorists");
        }
    }

    public void BeginDraft()
    {
        plugin.getMatchService().SetState(MatchState.Draft);
        _isDraft = true;
        var starting = _teams.FirstOrDefault(t => t.Id == _rng.Next(1, 3))!.TeamCaptain!;
        plugin.getAnnouncer().Announce("draft_chose_starter", starting.PlayerName);
        starting.OpenMenu(new DraftMenu(plugin.getBase(), plugin));
    }

    public void AddDraftNumber(CCSPlayerController player, int number)
    {
        if (!_draftNumbers.TryAdd(player, number))
        {
            player.PrintLocalizedChat(plugin.getBase().Localizer, "command_guess_already_guessed");
            return;
        }

        plugin.getAnnouncer().Announce("command_guess_guess", player.PlayerName, number.ToString());
    }
    
    public void CaptainDraft(int captainsToDraft)
    {
        _isCaptainDraft = true;
        plugin.getAnnouncer().Announce("draft_rng_setup", captainsToDraft == 1 ? "player" : "2 players", captainsToDraft == 1 ? "a team captain" : "team captains");

        Server.RunOnTickAsync(Server.TickCount + 1920, () =>
        {
            _isCaptainDraft = false;
            var number = _rng.Next(1, 101);
            var closest = _draftNumbers
                .OrderBy(kv => Math.Abs(kv.Value - number))
                .Take(captainsToDraft)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            plugin.getAnnouncer().Announce("draft_captain_num", number.ToString());
            foreach (var kv in closest)
            {
                SetTeamCaptain(kv.Key);
                plugin.getAnnouncer().Announce("draft_captain_closest", kv.Key.PlayerName, kv.Value.ToString());
            }
            BeginDraft();
        });
    }
    
    public bool IsCaptainDraft()
    {
        return _isCaptainDraft;
    }

    public void LoadTeamsFromJson(string file)
    {
        
    }
}