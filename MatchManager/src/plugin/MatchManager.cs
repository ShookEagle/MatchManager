using CounterStrikeSharp.API.Core;
using MatchManager.api.plugin;
using MatchManager.api.plugin.services;
using MatchManager.plugin.commands;
using MatchManager.plugin.commands.match;
using MatchManager.plugin.commands.teams;
using MatchManager.plugin.listeners;
using MatchManager.plugin.services;

namespace MatchManager.plugin;

public class MatchManager : BasePlugin, IPluginConfig<MatchManagerConfig>, IMatchManager
{
    private readonly Dictionary<string, Command> commands = new();
    public override string ModuleName => "MatchManager";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "ShookEagle";
    public override string ModuleDescription => "A PUGs plugin designed for the EdgeGamers Events Server";

    private IAnnouncer? _announcer;
    private IMatchService? _matchService;
    private ITeamsService? _teamsService;

    public MatchManagerConfig Config { get; set; } = new();
    public void OnConfigParsed(MatchManagerConfig config)
    {
        Config = config;
    }
    
    public BasePlugin getBase()
    {
        return this;
    }

    public IMatchService getMatchService() { return _matchService!; }
    public ITeamsService getTeamsService() { return _teamsService!; }
    public IAnnouncer getAnnouncer() { return _announcer!; }

    public override void Load(bool hotReload)
    {
        _ = new JoinTeamListener(this);
        
        _matchService = new MatchService(this);
        _teamsService = new TeamsService(this);
        _announcer = new Announcer(this);
        
        LoadCommands();
    }

    private void LoadCommands()
    {
        commands.Add("css_draft", new DraftCmd(this));
        commands.Add("css_guess", new GuessCmd(this));
        commands.Add("css_captain", new CaptainCmd(this));
        commands.Add("css_ready", new ReadyCmd(this));
        commands.Add("css_forceready", new ForceReadyCmd(this));

    foreach (var command in commands)
            AddCommand(command.Key, command.Value.Description ?? "No Description Provided", command.Value.OnCommand);
    }
}