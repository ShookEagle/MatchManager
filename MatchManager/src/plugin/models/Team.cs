using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace MatchManager.plugin.models;

public class Team
{
    public required int Id;
    public required string TeamName;
    public List<CCSPlayerController> TeamMembers = new();
    public CCSPlayerController? TeamCaptain;
    public required CsTeam CurrentSide;
    public int Score = 0;
}