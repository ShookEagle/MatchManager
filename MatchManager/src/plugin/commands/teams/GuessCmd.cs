using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using MatchManager.api.plugin;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.commands.teams;

public class GuessCmd(IMatchManager plugin) : Command(plugin)
{
    public override void OnCommand(CCSPlayerController? executor, CommandInfo info)
    {
        if (!plugin.getTeamsService().IsCaptainDraft())
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "command_draft_not_started");
            return;
        }
        if (!int.TryParse(info.ArgByIndex(1), out int number))
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "command_guess_nan");
            return;
        }

        if (number < 1 || number > 100)
        {
            info.ReplyLocalized(plugin.getBase().Localizer, "command_guess_out_of_range");
            return;
        }
        
        if (executor != null && executor.IsReal())
            plugin.getTeamsService().AddDraftNumber(executor, number);
    }
}