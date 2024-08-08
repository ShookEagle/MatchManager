using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Menu;
using MatchManager.plugin.utils;
using Microsoft.Extensions.Localization;

namespace MatchManager.plugin.extensions;

public static class PlayerExtensions
{
    public static bool IsReal(this CCSPlayerController? player)
    {
        return (player != null && !player.IsBot && !player.IsHLTV &&
                player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected);
    }

    public static bool IsEc(this CCSPlayerController? player)
    {
        return AdminManager.PlayerInGroup(player, "#ego/manager");
    }

    public static void PrintLocalizedChat(this CCSPlayerController? controller, IStringLocalizer localizer, string local,
        params object[] args)
    {
        if (controller == null || !controller.IsReal()) return;
        string message = localizer[local, args];
        message = message.Replace("%prefix%", localizer["prefix"]);
        message = StringUtils.ReplaceChatColors(message);
        controller.PrintToChat(message);
    }

    public static void PrintLocalizedConsole(this CCSPlayerController? controller, IStringLocalizer localizer,
        string local, params object[] args)
    {
        if (controller == null || !controller.IsReal()) return;
        string message = localizer[local, args];
        message = message.Replace("%prefix%", localizer["prefix"]);
        message = StringUtils.ReplaceChatColors(message);
        controller.PrintToConsole(message);
    }

    public static void OpenMenu(this CCSPlayerController? controller, CenterHtmlMenu menu)
    {
        if (controller == null || !controller.IsReal()) return;
        menu.Open(controller);
    }
}