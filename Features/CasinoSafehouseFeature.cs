using System;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using UnityEngine;
using BetterCasino.Config;

namespace BetterCasino.Features
{
    public static class CasinoSafehouseFeature
    {
        private static bool _wasInCasino;
        private static readonly Vector3 CasinoCenter = new(23f, 1.5f, 95f);
        private const float CasinoRadius = 10f;

        public static void Refresh()
        {
            _wasInCasino = false;
        }

        public static void OnUpdate()
        {
            if (!Settings.CasinoSafehouse.Value) return;

            try
            {
                var player = Player.Local;
                if (player == null) return;

                float dist = Vector3.Distance(player.transform.position, CasinoCenter);
                bool inCasino = dist < CasinoRadius;

                if (inCasino && !_wasInCasino)
                    ClearWanted();

                _wasInCasino = inCasino;
            }
            catch { }
        }

        private static void ClearWanted()
        {
            try
            {
                var player = Player.Local;
                if (player == null) return;

                var crimeData = player.CrimeData;
                if (crimeData == null) return;

                var currentLevel = crimeData.CurrentPursuitLevel;
                if (currentLevel == PlayerCrimeData.EPursuitLevel.None) return;

                crimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
                crimeData.ClearCrimes();
                var pursuers = crimeData.Pursuers;
                if (pursuers != null)
                    pursuers.Clear();
                MelonLogger.Msg($"[Safehouse] Wanted level cleared (was {currentLevel}).");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[Safehouse] Failed to clear wanted: {ex.Message}");
            }
        }

    }
}
