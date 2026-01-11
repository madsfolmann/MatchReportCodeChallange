import type { Weapon } from "../Types/Weapon";

export interface KillEvent {
  killerSteamId: string;
  victimSteamId: string;
  timestamp: string;
  isHeadshot: boolean;
  weapon: Weapon;
}
