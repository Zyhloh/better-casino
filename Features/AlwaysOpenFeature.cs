using HarmonyLib;
using Il2CppScheduleOne.Map;
using MelonLoader;
using BetterCasino.Config;

namespace BetterCasino.Features
{
    public static class AlwaysOpenFeature
    {
        private static bool _logged;

        public static void Refresh()
        {
            _logged = false;
        }

        [HarmonyPatch(typeof(TimedAccessZone), nameof(TimedAccessZone.GetIsOpen))]
        public static class GetIsOpenPatch
        {
            public static bool Prefix(ref bool __result)
            {
                if (!Settings.AlwaysOpen.Value) return true;

                if (!_logged)
                {
                    MelonLogger.Msg("[AlwaysOpen] Casino forced open.");
                    _logged = true;
                }

                __result = true;
                return false;
            }
        }
    }
}
