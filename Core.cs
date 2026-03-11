using MelonLoader;
using BetterCasino.Config;
using BetterCasino.Features;

namespace BetterCasino
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.Init();
            ModManagerBridge.TryHook();

            LoggerInstance.Msg("========================================");
            LoggerInstance.Msg($"Better Casino v{Info.Version} loaded.");
            LoggerInstance.Msg("========================================");

            LoggerInstance.Msg($"[AlwaysOpen] {(Settings.AlwaysOpen.Value ? "ENABLED" : "DISABLED")}");

            if (Settings.SlotMachineEnabled.Value)
            {
                LoggerInstance.Msg($"[SlotMachine] ENABLED | Bets: $100 - $100k | Free Play: {(Settings.FreePlay.Value ? "ON" : "OFF")}");
                if (Settings.SlotWinRate.Value >= 0f)
                    LoggerInstance.Msg($"[SlotMachine] Win Rate: {Settings.SlotWinRate.Value:P0} (HOST ONLY - requires being lobby host)");
                else
                    LoggerInstance.Msg("[SlotMachine] Win Rate: DEFAULT");
            }
            else
                LoggerInstance.Msg("[SlotMachine] DISABLED");

            if (Settings.TableGamesEnabled.Value)
            {
                LoggerInstance.Msg($"[Blackjack] ENABLED | Bets: ${Settings.BlackjackMinBet.Value}-${Settings.BlackjackMaxBet.Value}");
                LoggerInstance.Msg($"[RideTheBus] ENABLED | Bets: ${Settings.RTBMinBet.Value}-${Settings.RTBMaxBet.Value}");
            }
            else
                LoggerInstance.Msg("[TableGames] DISABLED");

            LoggerInstance.Msg($"[Safehouse] {(Settings.CasinoSafehouse.Value ? "ENABLED" : "DISABLED")}");
            LoggerInstance.Msg($"[SignChanger] {(Settings.SignChangerEnabled.Value ? "ENABLED - Text: " + Settings.SignText.Value : "DISABLED")}");
            LoggerInstance.Msg("========================================");
        }

        public override void OnUpdate()
        {
            SignChangerFeature.OnUpdate();
            CasinoSafehouseFeature.OnUpdate();
        }

        public override void OnDeinitializeMelon()
        {
            LoggerInstance.Msg("Better Casino unloaded.");
        }
    }
}
