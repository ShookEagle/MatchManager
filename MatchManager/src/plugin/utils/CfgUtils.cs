using CounterStrikeSharp.API;

namespace MatchManager.plugin.utils;

public static class CfgUtils
{
    public static void ExecWarmupSettings()
    {
        Server.ExecuteCommand("exec matchmanager/warmup.cfg");
    }
    
    public static void ExecKnifeSettings()
    {
        Server.ExecuteCommand("exec matchmanager/knife.cfg");
    }
    
    public static void ExecMatchSettings()
    {
        Server.ExecuteCommand("exec matchmanager/match.cfg");
    }
}