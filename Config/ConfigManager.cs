using System;
using MelonLoader;

namespace BetterCasino.Config
{
    public static class ModManagerBridge
    {
        private static bool _hooked;

        public static void TryHook()
        {
            if (_hooked) return;

            try
            {
                HookEvents();
                _hooked = true;
                MelonLogger.Msg("Mod Manager integration active.");
            }
            catch (Exception)
            {
                MelonLogger.Msg("Mod Manager not found, using MelonPreferences only.");
            }
        }

        private static void HookEvents()
        {
            ModManagerPhoneApp.ModSettingsEvents.OnPhonePreferencesSaved += OnSettingsSaved;
            ModManagerPhoneApp.ModSettingsEvents.OnMenuPreferencesSaved += OnSettingsSaved;
        }

        private static void OnSettingsSaved()
        {
            MelonPreferences.Save();
            Features.AlwaysOpenFeature.Refresh();
            Features.SlotMachineFeature.Refresh();
            Features.TableGamesFeature.Refresh();
            Features.CasinoSafehouseFeature.Refresh();
            Features.SignChangerFeature.Refresh();
        }
    }
}
