using System;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using BetterCasino.Config;

namespace BetterCasino.Features
{
    public static class SignChangerFeature
    {
        private static float _lastCheck;
        private static bool _applied;
        private static int _changedCount;

        public static void Refresh()
        {
            _applied = false;
            _changedCount = 0;
        }

        private static void ReplaceSignText(TMP_Text sign)
        {
            string txt = sign.text;
            if (string.IsNullOrEmpty(txt)) return;

            string trimmed = txt.Trim();
            if (trimmed == "4PM-5AM")
            {
                sign.text = "OPEN 24/7";
                _changedCount++;
            }
        }

        public static void OnUpdate()
        {
            if (!Settings.SignChangerEnabled.Value) return;
            if (_applied) return;
            if (Time.time - _lastCheck < 5f) return;

            _lastCheck = Time.time;

            try
            {
                var worldSigns = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<TextMeshPro>());
                if (worldSigns != null)
                {
                    foreach (var obj in worldSigns)
                    {
                        if (obj == null) continue;
                        var sign = obj.Cast<TextMeshPro>();
                        if (sign == null || sign.text == null) continue;
                        ReplaceSignText(sign);
                    }
                }

                var uiSigns = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<TextMeshProUGUI>());
                if (uiSigns != null)
                {
                    foreach (var obj in uiSigns)
                    {
                        if (obj == null) continue;
                        var sign = obj.Cast<TextMeshProUGUI>();
                        if (sign == null || sign.text == null) continue;
                        ReplaceSignText(sign);
                    }
                }

                if (_changedCount > 0)
                {
                    MelonLogger.Msg($"[SignChanger] Changed {_changedCount} sign(s) to OPEN 24/7.");
                    _applied = true;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[SignChanger] Error: {ex.Message}");
            }
        }
    }
}
