using HarmonyLib;
using Il2CppScheduleOne.Casino;
using Il2CppScheduleOne.Casino.UI;
using BetterCasino.Config;

namespace BetterCasino.Features
{
    public static class TableGamesFeature
    {
        public static void Refresh()
        {
        }

        [HarmonyPatch(typeof(BlackjackGameController), nameof(BlackjackGameController.SetLocalPlayerBet))]
        public static class BlackjackBetClampPatch
        {
            public static void Prefix(ref float bet)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                if (bet < Settings.BlackjackMinBet.Value) bet = Settings.BlackjackMinBet.Value;
                if (bet > Settings.BlackjackMaxBet.Value) bet = Settings.BlackjackMaxBet.Value;
            }
        }

        [HarmonyPatch(typeof(BlackjackInterface), nameof(BlackjackInterface.GetBetFromSliderValue))]
        public static class BlackjackSliderPatch
        {
            public static bool Prefix(float sliderVal, ref float __result)
            {
                if (!Settings.TableGamesEnabled.Value) return true;

                float min = Settings.BlackjackMinBet.Value;
                float max = Settings.BlackjackMaxBet.Value;
                __result = min + sliderVal * (max - min);
                __result = (float)System.Math.Round(__result);
                return false;
            }
        }

        [HarmonyPatch(typeof(BlackjackInterface), nameof(BlackjackInterface.RefreshDisplayedBet))]
        public static class BlackjackDisplayPatch
        {
            public static void Postfix(BlackjackInterface __instance)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                try
                {
                    var slider = __instance.BetSlider;
                    var label = __instance.BetAmount;
                    if (slider == null || label == null) return;

                    float min = Settings.BlackjackMinBet.Value;
                    float max = Settings.BlackjackMaxBet.Value;
                    float bet = min + slider.value * (max - min);
                    label.text = $"${System.Math.Round(bet):N0}";
                }
                catch { }
            }
        }

        [HarmonyPatch(typeof(BlackjackGameController), nameof(BlackjackGameController.ToggleLocalPlayerReady))]
        public static class BlackjackReadyPatch
        {
            public static void Prefix(BlackjackGameController __instance)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                try
                {
                    var ui = BlackjackInterface.Instance;
                    if (ui == null || ui.BetSlider == null) return;

                    float min = Settings.BlackjackMinBet.Value;
                    float max = Settings.BlackjackMaxBet.Value;
                    float bet = (float)System.Math.Round(min + ui.BetSlider.value * (max - min));
                    __instance.LocalPlayerBet = bet;
                }
                catch { }
            }
        }

        [HarmonyPatch(typeof(RTBGameController), nameof(RTBGameController.SetLocalPlayerBet))]
        public static class RTBBetClampPatch
        {
            public static void Prefix(ref float bet)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                if (bet < Settings.RTBMinBet.Value) bet = Settings.RTBMinBet.Value;
                if (bet > Settings.RTBMaxBet.Value) bet = Settings.RTBMaxBet.Value;
            }
        }

        [HarmonyPatch(typeof(RTBInterface), nameof(RTBInterface.GetBetFromSliderValue))]
        public static class RTBSliderPatch
        {
            public static bool Prefix(float sliderVal, ref float __result)
            {
                if (!Settings.TableGamesEnabled.Value) return true;

                float min = Settings.RTBMinBet.Value;
                float max = Settings.RTBMaxBet.Value;
                __result = min + sliderVal * (max - min);
                __result = (float)System.Math.Round(__result);
                return false;
            }
        }

        [HarmonyPatch(typeof(RTBInterface), nameof(RTBInterface.RefreshDisplayedBet))]
        public static class RTBDisplayPatch
        {
            public static void Postfix(RTBInterface __instance)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                try
                {
                    var slider = __instance.BetSlider;
                    var label = __instance.BetAmount;
                    if (slider == null || label == null) return;

                    float min = Settings.RTBMinBet.Value;
                    float max = Settings.RTBMaxBet.Value;
                    float bet = min + slider.value * (max - min);
                    label.text = $"${System.Math.Round(bet):N0}";
                }
                catch { }
            }
        }

        [HarmonyPatch(typeof(RTBGameController), nameof(RTBGameController.ToggleLocalPlayerReady))]
        public static class RTBReadyPatch
        {
            public static void Prefix(RTBGameController __instance)
            {
                if (!Settings.TableGamesEnabled.Value) return;

                try
                {
                    var ui = RTBInterface.Instance;
                    if (ui == null || ui.BetSlider == null) return;

                    float min = Settings.RTBMinBet.Value;
                    float max = Settings.RTBMaxBet.Value;
                    float bet = (float)System.Math.Round(min + ui.BetSlider.value * (max - min));
                    __instance.LocalPlayerBet = bet;
                }
                catch { }
            }
        }
    }
}
