import type { Match } from "../../Model/Match/Entities/Match";
import { RoundRow } from "./RoundRow";
import { RoundsTableCell } from "./RoundsTableCell";

export const RoundsTable = ({ match }: { match: Match }) => {
  const colWeights = [0.2, 0.3, 0.7, 0.7, 0.3, 5];

  const borderStyle = "border-gray-400/50";

  const columnTemplate = (weights: number[]) =>
    weights.map((w) => `${w}fr`).join(" ");

  const columnGridTemplate = columnTemplate(colWeights);

  return (
    <div className="relative w-full ">
      <div className={` border-2 ${borderStyle} rounded-2xl overflow-hidden`}>
        <div
          className={`text-white text-sm border ${borderStyle} font-bold rounded-tl-lg rounded-tr-lg`}
          style={{
            display: "grid",
            gridTemplateColumns: columnGridTemplate,
          }}
        >
          <RoundsTableCell borderStyle={borderStyle} />
          <RoundsTableCell value={"Time"} borderStyle={borderStyle} />
          <RoundsTableCell value={"Winner"} borderStyle={borderStyle} />
          <RoundsTableCell value={"Reason"} borderStyle={borderStyle} />
          <RoundsTableCell value={"Kills"} borderStyle={borderStyle} />
          <RoundsTableCell value={"Timeline"} borderStyle={borderStyle} />
        </div>
        <div className="relative overflow-hidden ">
          <div
            className="overflow-y-auto max-h-80"
            style={{
              scrollbarWidth: "none",
              msOverflowStyle: "none",
            }}
          >
            {[...match.rounds]
              .sort((a, b) => b.number - a.number)
              .map((round) => (
                <RoundRow
                  key={round.number}
                  round={round}
                  borderStyle={borderStyle}
                  columnGridTemplate={columnGridTemplate}
                />
              ))}
          </div>
        </div>
      </div>
    </div>
  );
};
