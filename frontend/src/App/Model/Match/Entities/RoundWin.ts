import type { Side } from "../Types/Side";
import type { RoundWinReason } from "../Types/RoundWinReason";

export interface RoundWin {
  teamName: string;
  side: Side;
  reason: RoundWinReason;
}
