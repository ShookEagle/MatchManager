using CounterStrikeSharp.API.Core;

namespace MatchManager.api.plugin.services;

public interface IAnnouncer
{
    void AnnounceAnonymous(CCSPlayerController admin, string local, params object[] args);
    void Announce(string local, params object[] args);
}