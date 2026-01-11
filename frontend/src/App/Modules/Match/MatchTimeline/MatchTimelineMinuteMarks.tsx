import type { Match } from "../../../Model/Match/Entities/Match";

export const MatchTimelineMinuteMarks = ({ match }: { match: Match }) => {
  const matchDurationMinutes = getMatchDurationMinutes(match);
  const marks = getMinuteMarks(matchDurationMinutes);

  return (
    <>
      {marks.map((minute) => {
        const leftPercent = (minute / matchDurationMinutes) * 100;

        return (
          <div
            key={minute}
            className="absolute flex flex-col items-center"
            style={{ left: `${leftPercent}%`, top: 0 }}
          >
            <div className="h-23 w-0.5 bg-gray-500"></div>
            <div className="text-xs text-gray-400 mt-2">{minute}m</div>
          </div>
        );
      })}
    </>
  );
};

const getMatchDurationMinutes = (match: Match) =>
  Math.max(
    1,
    Math.round((+new Date(match.endedAt) - +new Date(match.startedAt)) / 60000)
  );

const getMinuteMarks = (matchDurationMinutes: number) =>
  Array.from(
    { length: Math.floor((matchDurationMinutes - 2) / 2) + 1 },
    (_, i) => 2 + i * 2
  ).filter((minute) => minute < matchDurationMinutes);
