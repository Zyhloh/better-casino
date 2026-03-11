using MelonLoader;

namespace BetterCasino.Config
{
    public static class Settings
    {
        private static MelonPreferences_Category _category = null!;

        public static MelonPreferences_Entry<bool> AlwaysOpen = null!;
        public static MelonPreferences_Entry<bool> SlotMachineEnabled = null!;
        public static MelonPreferences_Entry<int> SlotMinBet = null!;
        public static MelonPreferences_Entry<int> SlotMaxBet = null!;
        public static MelonPreferences_Entry<int> SlotBetStep = null!;
        public static MelonPreferences_Entry<bool> TableGamesEnabled = null!;
        public static MelonPreferences_Entry<int> BlackjackMinBet = null!;
        public static MelonPreferences_Entry<int> BlackjackMaxBet = null!;
        public static MelonPreferences_Entry<int> RTBMinBet = null!;
        public static MelonPreferences_Entry<int> RTBMaxBet = null!;
        public static MelonPreferences_Entry<float> SlotWinRate = null!;
        public static MelonPreferences_Entry<bool> FreePlay = null!;
        public static MelonPreferences_Entry<bool> CasinoSafehouse = null!;
        public static MelonPreferences_Entry<bool> SignChangerEnabled = null!;
        public static MelonPreferences_Entry<string> SignText = null!;
        public static MelonPreferences_Entry<float> MultiMachinePenalty = null!;
        public static MelonPreferences_Entry<float> MultiMachineTimeWindow = null!;
        public static MelonPreferences_Entry<float> WinStreakPenalty = null!;
        public static MelonPreferences_Entry<int> WinStreakThreshold = null!;

        public static void Init()
        {
            _category = MelonPreferences.CreateCategory("BetterCasino", "Better Casino");

            AlwaysOpen = _category.CreateEntry("AlwaysOpen", true, "Always Open Casino");
            SlotMachineEnabled = _category.CreateEntry("SlotMachineEnabled", true, "Slot Machine Tweaks");
            SlotMinBet = _category.CreateEntry("SlotMinBet", 10, "Slot Min Bet");
            SlotMaxBet = _category.CreateEntry("SlotMaxBet", 5000, "Slot Max Bet");
            SlotBetStep = _category.CreateEntry("SlotBetStep", 10, "Slot Bet Step");
            SlotWinRate = _category.CreateEntry("SlotWinRate", -1f, "Slot Win Rate (-1 = default)");
            FreePlay = _category.CreateEntry("FreePlay", false, "Free Slot Play");
            TableGamesEnabled = _category.CreateEntry("TableGamesEnabled", true, "Table Game Tweaks");
            BlackjackMinBet = _category.CreateEntry("BlackjackMinBet", 10, "Blackjack Min Bet");
            BlackjackMaxBet = _category.CreateEntry("BlackjackMaxBet", 5000, "Blackjack Max Bet");
            RTBMinBet = _category.CreateEntry("RTBMinBet", 10, "RTB Min Bet");
            RTBMaxBet = _category.CreateEntry("RTBMaxBet", 5000, "RTB Max Bet");
            CasinoSafehouse = _category.CreateEntry("CasinoSafehouse", true, "Casino Safehouse");
            SignChangerEnabled = _category.CreateEntry("SignChangerEnabled", true, "Sign Changer");
            SignText = _category.CreateEntry("SignText", "OPEN 24/7", "Custom Sign Text");
            MultiMachinePenalty = _category.CreateEntry("MultiMachinePenalty", 0.5f, "Multi-Machine Win Penalty (0-1)");
            MultiMachineTimeWindow = _category.CreateEntry("MultiMachineTimeWindow", 10f, "Multi-Machine Time Window (seconds)");
            WinStreakPenalty = _category.CreateEntry("WinStreakPenalty", 0.9f, "Win Streak Penalty Multiplier (0-1)");
            WinStreakThreshold = _category.CreateEntry("WinStreakThreshold", 3, "Wins Before Streak Penalty");
        }
    }
}
