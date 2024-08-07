using CounterStrikeSharp.API.Core;
using MatchManager.api.plugin;
using MatchManager.plugin.commands;

namespace MatchManager.plugin;

public class MatchManager : BasePlugin, IPluginConfig<MatchManagerConfig>, IMatchManager
{
    private readonly Dictionary<string, Command> commands = new();
    public override string ModuleName => "MatchManager";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "ShookEagle";
    public override string ModuleDescription => "A PUGs plugin designed for the EdgeGamers Events Server";

    public MatchManagerConfig Config { get; set; } = new();
    public void OnConfigParsed(MatchManagerConfig config)
    {
        Config = config;
    }
    
    public BasePlugin getBase()
    {
        return this;
    }

    public override void Load(bool hotReload)
    {
        LoadCommands();
    }

    private void LoadCommands()
    {
        
        
        foreach (var command in commands)
            AddCommand(command.Key, command.Value.Description ?? "No Description Provided", command.Value.OnCommand);
    }
}