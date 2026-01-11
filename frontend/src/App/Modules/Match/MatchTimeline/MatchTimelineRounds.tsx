import type { JSX } from "react";
import type { Match } from "../../../Model/Match/Entities/Match";
import type { RoundWinReason } from "../../../Model/Match/Types/RoundWinReason";
import { ActionIcons } from "../../Shared/ActionIcons";

export const MatchTimelineRounds = ({ match }: { match: Match }) => {
  const matchStart = +new Date(match.startedAt);
  const matchDuration = +new Date(match.endedAt) - matchStart;

  return (
    <>
      {match.rounds.map((round) => {
        const roundStart = +new Date(round.startedAt);
        const roundEnd = +new Date(round.endedAt);
        const leftPercent = ((roundStart - matchStart) / matchDuration) * 100;
        const widthPercent = ((roundEnd - roundStart) / matchDuration) * 100;

        return (
          <div
            key={round.number}
            className="absolute flex flex-col items-center gap-y-2"
            style={{ left: `${leftPercent}%`, width: `${widthPercent}%` }}
          >
            <div
              className={`w-full h-8 rounded-xs flex flex-col justify-center items-center text-amber-800 ${
                round.winner.side === "CounterTerrorist"
                  ? "bg-blue-400"
                  : "bg-orange-400"
              }`}
            >
              <span className="text-sm font-bold text-black">
                {round.number}
              </span>
            </div>
            <div className="flex">
              {winReasonIcons[round.winner.reason](
                round.winner.side === "CounterTerrorist"
                  ? "text-blue-400"
                  : "text-orange-400"
              )}
            </div>
          </div>
        );
      })}
    </>
  );
};

const winReasonIcons: Record<RoundWinReason, (color: string) => JSX.Element> = {
  BombDefused: ActionIcons.BombDefused!,
  BombExploded: ActionIcons.BombExploded!,
  EliminatedAllOpponents: ActionIcons.Headshot!,
  Timeout: ActionIcons.Timeout!,
};
