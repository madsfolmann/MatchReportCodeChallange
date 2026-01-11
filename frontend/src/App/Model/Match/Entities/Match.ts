import type { Map } from "../Types/Map";
import type { Round } from "./Round";
import type { Team } from "./Team";
import type { Player } from "./Player";
import type { WeaponStats } from "./WeaponStats";

export interface Match {
  map: Map;
  startedAt: string;
  endedAt: string;
  duration: number;
  rounds: Round[];
  teams: Team[];
  players: Player[];
  winnerTeamName?: string;
  totalRounds: number;
  averageRoundDuration: string;
  weaponStats: WeaponStats[];
}
