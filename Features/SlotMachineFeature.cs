using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Casino;
using MelonLoader;
using BetterCasino.Config;

namespace BetterCasino.Features
{
    public static class SlotMachineFeature
    {
        private static readonly int[] ExtraBets = { 1500, 3000, 5000, 10000, 25000, 50000, 100000 };
        private static int[]? _nativeBets;
        private static Il2CppStructArray<int>? _customBets;
        private static readonly Random _rng = new();
        private static bool _loggedBets;
        private static bool _loggedWinRate;
        private static float _originalFontSize;
        private static int _lastMachineId;
        private static float _lastSpinTime;
        private static int _consecutiveWins;

        public static void Refresh()
        {
            _customBets = null;
            _loggedBets = false;
            _loggedWinRate = false;
            RefreshAllMachines();
        }

        private static void RefreshAllMachines()
        {
            try
            {
                if (!Settings.SlotMachineEnabled.Value) return;
                var bets = GetCustomBets();
                SlotMachine.BetAmounts = bets;
                var machines = UnityEngine.Object.FindObjectsOfType<SlotMachine>();
                if (machines == null) return;
                foreach (var m in machines)
                {
                    if (m == null) continue;
                    try
                    {
                        var label = m.BetAmountLabel;
                        if (label == null) continue;
                        if (_originalFontSize <= 0f)
                            _originalFontSize = label.fontSize;
                        label.enableAutoSizing = false;
                        string formatted = FormatBet(m.currentBetAmount);
                        label.fontSize = formatted.Length > 4 ? _originalFontSize * 0.7f : _originalFontSize;
                        label.text = formatted;
                    }
                    catch { }
                }
            }
            catch { }
        }

        public static string FormatBet(int amount)
        {
            if (amount <= 0) return "FREE";
            if (amount >= 1000)
            {
                float k = amount / 1000f;
                return k == (int)k ? $"${(int)k}k" : $"${k:0.#}k";
            }
            return $"${amount}";
        }

        private static void CaptureNativeBets()
        {
            if (_nativeBets != null) return;
            var native = SlotMachine.BetAmounts;
            if (native == null || native.Length == 0) return;
            _nativeBets = new int[native.Length];
            for (int i = 0; i < native.Length; i++)
                _nativeBets[i] = native[i];
        }

        private static Il2CppStructArray<int> GetCustomBets()
        {
            if (_customBets != null) return _customBets;

            CaptureNativeBets();

            var set = new SortedSet<int>();
            if (_nativeBets != null)
                foreach (int b in _nativeBets) set.Add(b);
            foreach (int b in ExtraBets) set.Add(b);

            var bets = new List<int>();
            if (Settings.FreePlay.Value)
                bets.Add(0);
            bets.AddRange(set);

            _customBets = new Il2CppStructArray<int>(bets.Count);
            for (int i = 0; i < bets.Count; i++)
                _customBets[i] = bets[i];

            return _customBets;
        }

        private static float GetEffectiveWinRate()
        {
            float rate = Settings.SlotWinRate.Value;
            if (rate < 0f) return rate;

            float timeSinceSwitch = UnityEngine.Time.time - _lastSpinTime;
            if (timeSinceSwitch < Settings.MultiMachineTimeWindow.Value && _lastMachineId != 0)
                rate *= (1f - Settings.MultiMachinePenalty.Value);

            int threshold = Settings.WinStreakThreshold.Value;
            if (_consecutiveWins >= threshold)
            {
                int excess = _consecutiveWins - threshold;
                for (int i = 0; i < excess + 1; i++)
                    rate *= Settings.WinStreakPenalty.Value;
            }

            return Math.Max(rate, 0f);
        }

        private static SlotMachine.ESymbol GetBiasedSymbol()
        {
            float rate = GetEffectiveWinRate();
            if (rate < 0f || rate > 1f)
                return (SlotMachine.ESymbol)_rng.Next(0, 6);

            if (_rng.NextDouble() < rate)
                return SlotMachine.ESymbol.Seven;

            return (SlotMachine.ESymbol)_rng.Next(0, 5);
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.Awake))]
        public static class AwakePatch
        {
            public static void Postfix(SlotMachine __instance)
            {
                if (!Settings.SlotMachineEnabled.Value) return;

                SlotMachine.BetAmounts = GetCustomBets();

                if (!_loggedBets)
                {
                    var bets = GetCustomBets();
                    MelonLogger.Msg($"[SlotMachine] Custom bet ladder applied ({bets.Length} options: {FormatBet(bets[0])} - {FormatBet(bets[bets.Length - 1])}).");
                    _loggedBets = true;
                }
            }
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.HandleInteracted))]
        public static class HandleInteractedPatch
        {
            public static void Prefix(SlotMachine __instance)
            {
                if (!Settings.SlotMachineEnabled.Value) return;

                SlotMachine.BetAmounts = GetCustomBets();
            }
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.SetBetIndex))]
        public static class SetBetIndexPatch
        {
            public static void Postfix(SlotMachine __instance)
            {
                if (!Settings.SlotMachineEnabled.Value) return;

                try
                {
                    var label = __instance.BetAmountLabel;
                    if (label != null)
                    {
                        if (_originalFontSize <= 0f)
                            _originalFontSize = label.fontSize;
                        label.enableAutoSizing = false;
                        string formatted = FormatBet(__instance.currentBetAmount);
                        label.fontSize = formatted.Length > 4 ? _originalFontSize * 0.7f : _originalFontSize;
                        label.text = formatted;
                    }
                }
                catch
                {
                }
            }
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.GetRandomSymbol))]
        public static class GetRandomSymbolPatch
        {
            public static bool Prefix(ref SlotMachine.ESymbol __result)
            {
                if (!Settings.SlotMachineEnabled.Value) return true;
                if (Settings.SlotWinRate.Value < 0f) return true;

                if (!_loggedWinRate)
                {
                    MelonLogger.Msg($"[SlotMachine] Win rate override active at {Settings.SlotWinRate.Value:P0} (HOST ONLY).");
                    _loggedWinRate = true;
                }

                __result = GetBiasedSymbol();
                return false;
            }
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.SendStartSpin))]
        public static class SendStartSpinPatch
        {
            public static void Prefix(SlotMachine __instance)
            {
                if (!Settings.SlotMachineEnabled.Value) return;

                int machineId = __instance.GetInstanceID();
                if (machineId != _lastMachineId)
                {
                    _lastSpinTime = UnityEngine.Time.time;
                    _lastMachineId = machineId;
                }
            }
        }

        [HarmonyPatch(typeof(SlotMachine), nameof(SlotMachine.DisplayOutcome))]
        public static class DisplayOutcomePatch
        {
            public static void Postfix(SlotMachine __instance, SlotMachine.EOutcome outcome, int winAmount)
            {
                if (!Settings.SlotMachineEnabled.Value) return;

                if (winAmount > 0)
                    _consecutiveWins++;
                else
                    _consecutiveWins = 0;

                try
                {
                    var label = __instance.BetAmountLabel;
                    if (label != null)
                    {
                        if (_originalFontSize <= 0f)
                            _originalFontSize = label.fontSize;
                        label.enableAutoSizing = false;
                        string formatted = winAmount > 0 ? FormatBet(winAmount) : FormatBet(__instance.currentBetAmount);
                        label.fontSize = formatted.Length > 4 ? _originalFontSize * 0.7f : _originalFontSize;
                        label.text = formatted;
                    }
                }
                catch
                {
                }
            }
        }
    }
}
