using CounterStrikeSharp.API.Core;
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
}