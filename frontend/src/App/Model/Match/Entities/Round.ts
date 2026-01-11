import type { Side } from "../Types/Side";
import type { RoundWin } from "./RoundWin";
import type { KillEvent } from "./KillEvent";
import type { BombEvent } from "./BombEvent";

export interface Round {
  number: number;
  startedAt: string;
  endedAt: string;
  duration: number;
  winner: RoundWin;
  playerSideBySteamId: Record<string, Side>;
  kills: KillEvent[];
  bombEvents: BombEvent[];
}
