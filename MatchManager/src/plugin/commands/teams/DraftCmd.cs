using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands.teams;

public class DraftCmd(IMatchManager plugin) : Command(plugin)
{
    
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (!executor.IsEc()) return;

        if (plugin.getTeamsService().IsCaptainDraft())
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "command_draft_already_started");
            return;
        }
        
        var Teams = plugin.getTeamsService().GetTeams();
        switch (Teams.Count)
        {
            case 2: plugin.getTeamsService().BeginDraft();
                break;
            case 1: plugin.getTeamsService().CaptainDraft(1);
                break;
            case 0: plugin.getTeamsService().CaptainDraft(2);
                plugin.getTeamsService().InitializeNewTeams();
                break;
        }
    }
}