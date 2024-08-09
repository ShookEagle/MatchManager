using CounterStrikeSharp.API.Core;
using MatchManager.plugin.enums;

namespace MatchManager.api.plugin.services;

public interface IMatchService
{
    MatchState GetState();
    void SetState(MatchState state);
    bool ReadyPlayer(CCSPlayerController player);
    void InitiatePreMatch();
    void InitiateWarmup();
    void InitiateMatch();
}