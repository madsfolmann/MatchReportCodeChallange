import type { Round } from "../../../Model/Match/Entities/Round";
import { ActionIcons } from "../../Shared/ActionIcons";

export const RoundTimelineEvents = ({
  kills,
  bombEvents,
  percentFromTimestamp,
}: {
  kills: Round["kills"];
  bombEvents: Round["bombEvents"];
  percentFromTimestamp: (timestamp: string) => number;
}) => {
  return (
    <>
      {kills.map((kill, index) => {
        const leftPercent = percentFromTimestamp(kill.timestamp);

        return (
          <div
            key={`kill-${index}`}
            className="absolute top-0"
            style={{ left: `${leftPercent}%` }}
          >
            {kill.isHeadshot ? (
              ActionIcons.Headshot!()
            ) : (
              <div className="w-2 h-2 bg-red-700 rounded" />
            )}
          </div>
        );
      })}

      {bombEvents.map((event, index) => {
        const leftPercent = percentFromTimestamp(event.timestamp);
        const bombEventLabels: Record<string, string> = {
          Planted: "BombPlanted",
          Defused: "BombDefused",
        };

        if (!(event.action in bombEventLabels)) {
          return null;
        }

        return (
          <div
            key={`bomb-${index}`}
            className="absolute top-0"
            style={{ left: `${leftPercent}%` }}
          >
            {ActionIcons[bombEventLabels[event.action]]!()}
          </div>
        );
      })}
    </>
  );
};
