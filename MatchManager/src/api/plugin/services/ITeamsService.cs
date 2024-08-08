using CounterStrikeSharp.API.Core;
using MatchManager.plugin.models;

namespace MatchManager.api.plugin.services;

public interface ITeamsService
{
    void InitializeNewTeams();
    HashSet<Team> GetTeams();
    bool SetTeamCaptain(CCSPlayerController player);
    void AddPlayerToCaptainTeam(CCSPlayerController captain, CCSPlayerController player);
    void BeginDraft();
    void AddDraftNumber(CCSPlayerController player, int number);
    void CaptainDraft(int captainsToDraft);
    bool IsCaptainDraft();
    void LoadTeamsFromJson(string file);
}