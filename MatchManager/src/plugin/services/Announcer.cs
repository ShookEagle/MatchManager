using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MatchManager.api.plugin;
using MatchManager.api.plugin.services;
using MatchManager.plugin.extensions;
using Microsoft.Extensions.Localization;

namespace MatchManager.plugin.services;

public class Announcer(IMatchManager plugin) : IAnnouncer
{
    public void AnnounceAnonymous(CCSPlayerController admin, string local, params object[] args)
    {
        foreach (var player in Utilities.GetPlayers())
            player.PrintLocalizedChat(plugin.getBase().Localizer, local, player.IsEc() ? $"{admin.PlayerName} " : "", args);
    }
    
    public void Announce(string local, params object[] args)
    {
        foreach (var player in Utilities.GetPlayers())
            player.PrintLocalizedChat(plugin.getBase().Localizer, local, args);
    }
}