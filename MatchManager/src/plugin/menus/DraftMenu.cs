using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using MatchManager.api.plugin;
using MatchManager.plugin.extensions;

namespace MatchManager.plugin.menus;

public sealed class DraftMenu : CenterHtmlMenu
{
    public DraftMenu(BasePlugin baseplugin, IMatchManager plugin) : base("Pick a Player to Draft", baseplugin)
   {
       var taken = plugin.getTeamsService().GetTeams().SelectMany(team => team.TeamMembers).ToHashSet();
       var availablePlayers = Utilities.GetPlayers().Where(p => !p.IsBot && p.IsReal() && !taken.Contains(p)).ToList();

       foreach (var player in availablePlayers)
       {
           AddMenuOption(player.PlayerName, (executor, option) =>
           {
               var selectedPlayer = availablePlayers.First(p => p.PlayerName == option.Text);
               plugin.getAnnouncer().Announce("draft_chose_player", executor.PlayerName, selectedPlayer.PlayerName);
               plugin.getTeamsService().AddPlayerToCaptainTeam(executor, selectedPlayer);
               MenuManager.CloseActiveMenu(executor);
           });
       }
   }
}