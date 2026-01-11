import type { BombAction } from "../Types/BombAction";
import type { Bombsite } from "../Types/BombSite";

export interface BombEvent {
  playerSteamId: string;
  action: BombAction;
  bombsite?: Bombsite;
  timestamp: string;
}
