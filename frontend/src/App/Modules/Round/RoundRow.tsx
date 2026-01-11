import type { Round } from "../../Model/Match/Entities/Round";
import { ActionIcons } from "../Shared/ActionIcons";
import { FormatTime } from "../Shared/FormatTime";
import { RoundTimeline } from "./RoundTimeline/RoundTimeline";
import { RoundsTableCell } from "./RoundsTableCell";

export const RoundRow = ({
  round,
  borderStyle,
  columnGridTemplate,
}: {
  round: Round;
  borderStyle: string;
  columnGridTemplate: any;
}) => {
  const isEvenRoundNumber = round.number % 2 === 0;
  return (
    <div
      className={`text-white ${
        isEvenRoundNumber ? "bg-black/30" : "bg-black/10"
      } cursor-pointer text-sm border ${borderStyle} font-light transition-colors hover:bg-black/40`}
      style={{
        display: "grid",
        gridTemplateColumns: columnGridTemplate,
      }}
    >
      <RoundsTableCell
        value={
          <span className="font-black text-base tracking-wide">
            {round.number}
          </span>
        }
        rowCounter={true}
        borderStyle={borderStyle}
      />
      <RoundsTableCell
        value={
          <span className="text-xs font-mono text-gray-300">
            {FormatTime.Duration(round.duration)}
          </span>
        }
        borderStyle={borderStyle}
      />
      <RoundsTableCell
        value={
          <div
            className={`flex items-center gap-2 ${
              round.winner.side === "CounterTerrorist"
                ? "text-blue-400"
                : "text-orange-400"
            }`}
          >
            <span className={`flex items-center gap-2`}>
              <span className="font-semibold">{round.winner.teamName}</span>
              <span className={`text-xs `}>
                ({round.winner.side === "CounterTerrorist" ? "CT" : "T"})
              </span>
            </span>
          </div>
        }
        borderStyle={borderStyle}
      />
      <RoundsTableCell
        value={
          <span className="">
            {
              {
                BombDefused: "Bomb defused",
                BombExploded: "Bomb exploded",
                EliminatedAllOpponents: "All eliminated",
                Timeout: "Timeout",
              }[round.winner.reason]
            }
          </span>
        }
        borderStyle={borderStyle}
      />
      <RoundsTableCell
        value={
          <span className="flex  items-center gap-1">
            <span className="font-semibold">{round.kills.length}</span>
            <span className="text-xs text-gray-400 flex items-center gap-0.5">
              ({round.kills.filter((k) => k.isHeadshot).length}
              {ActionIcons.Headshot!()})
            </span>
          </span>
        }
        borderStyle={borderStyle}
      />
      <RoundsTableCell
        className="pr-7"
        value={<RoundTimeline round={round} />}
        borderStyle={borderStyle}
      />
    </div>
  );
};
