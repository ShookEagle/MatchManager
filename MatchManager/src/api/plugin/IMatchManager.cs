using CounterStrikeSharp.API.Core;
using MatchManager.api.plugin.services;

namespace MatchManager.api.plugin;

public interface IMatchManager
{
    BasePlugin getBase();
    IMatchService getMatchService();
    ITeamsService getTeamsService();
    IAnnouncer getAnnouncer();
}