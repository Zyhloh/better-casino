using MelonLoader;

namespace BetterCasino.Helpers
{
    public static class LogHelper
    {
        public static void Msg(string message) => MelonLogger.Msg(message);
        public static void Warning(string message) => MelonLogger.Warning(message);
        public static void Error(string message) => MelonLogger.Error(message);
    }
}
