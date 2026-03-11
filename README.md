# Better Casino V3

MelonLoader mod for Schedule 1 that overhauls The Crimson Canary casino.

## Features

### Slot Machines
- **Extended Bets** — Adds $1.5k, $3k, $5k, $10k, $25k, $50k, and $100k on top of the game's native bet amounts. No bets are removed.
- **Free Play** — Toggle on a $0 bet option. Shows "FREE" on the machine display.
- **Win Rate Override** — Set a custom Seven bias from 0% to 100%, or leave at -1 for default game odds. Host only.
- **Anti-Exploit** — Configurable penalties for machine hopping and win streaks. Reduces effective win rate when players switch machines too fast or win too many in a row.
- **Smart Labels** — Large bet amounts show as $5k, $100k etc. so the display doesn't break.

### Table Games (Blackjack & Ride The Bus)
- **Custom Bet Range** — Set your own min/max for both games. Default: $10 - $5,000.
- **Slider Fix** — The bet slider actually maps to your configured range and the game accepts the full amount. No more $1k cap.

### Always Open
- **24/7 Casino** — The casino never closes. No more waiting for business hours.
- **Updated Signs** — The "4PM-5AM" text on the casino signs changes to "OPEN 24/7".

### Casino Safehouse
- **Wanted Cleared** — Walk into the casino with any wanted level and it gets wiped. Pursuit level reset, crimes cleared, cops stop chasing.
- **Toggleable** — Turn it off if you don't want the protection.

## Mod Manager Integration

Works with [Mod Manager & Phone App](https://www.nexusmods.com/schedule1/mods/397). All settings accessible from your in-game phone, changes apply immediately. Mod Manager is optional — without it, settings are in `UserData/MelonPreferences.cfg`.

## Configuration

Settings are in `UserData/MelonPreferences.cfg` under `[BetterCasino]`.

### General
| Setting | Default | Description |
|---------|---------|-------------|
| AlwaysOpen | true | Casino ignores business hours |
| SignChangerEnabled | true | Enable sign text replacement |
| SignText | OPEN 24/7 | Custom sign text |
| CasinoSafehouse | true | Clear wanted on casino entry |

### Slot Machines
| Setting | Default | Description |
|---------|---------|-------------|
| SlotMachineEnabled | true | Enable slot machine tweaks |
| FreePlay | false | Add $0 bet option |
| SlotWinRate | -1 | Seven bias, -1 = default odds, 0.0-1.0 = custom (host only) |

### Anti-Exploit
| Setting | Default | Description |
|---------|---------|-------------|
| MultiMachinePenalty | 0.5 | Win rate reduction when hopping machines |
| MultiMachineTimeWindow | 10 | Seconds before machine hop penalty resets |
| WinStreakPenalty | 0.9 | Multiplier per win past threshold |
| WinStreakThreshold | 3 | Consecutive wins before penalty kicks in |

### Table Games
| Setting | Default | Description |
|---------|---------|-------------|
| TableGamesEnabled | true | Enable table game tweaks |
| BlackjackMinBet | 10 | Blackjack minimum bet |
| BlackjackMaxBet | 5000 | Blackjack maximum bet |
| RTBMinBet | 10 | Ride The Bus minimum bet |
| RTBMaxBet | 5000 | Ride The Bus maximum bet |

## Multiplayer

**Host only:** Win rate override, anti-exploit penalties, always-open state.

**All players:** Bet ladder, display formatting, table game sliders, sign text, safehouse wanted clearing.

## Installation

1. Install [MelonLoader](https://melonwiki.xyz/) (latest)
2. Drop `BetterCasino.dll` into your `Mods` folder
3. Launch the game — config generates on first run
4. (Optional) Install [Mod Manager & Phone App](https://www.nexusmods.com/schedule1/mods/397) to change settings in-game

## Compatibility

- MelonLoader (IL2CPP)
- May conflict with other mods that modify `SlotMachine.BetAmounts` or casino access zones

## Credits

Created by **Zyhloh**

Thanks to BorisJohnsonLies for the sign patch and free play ideas
