using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace MatchManager.plugin.extensions;

public static class ServerExtensions
{
    public static void GetGameRules(out CCSGameRules? gamerules, out CCSGameRulesProxy? proxy)
    {
        proxy = Utilities
            .FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules")
            .FirstOrDefault();
        gamerules = proxy?.GameRules;
    }
}