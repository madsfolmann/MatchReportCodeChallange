import type { Round } from "../../../Model/Match/Entities/Round";
import { RoundTimelineEvents } from "./RoundTimelineEvent";
import { RoundlineMinuteMarks } from "./RoundTimelineMinuteMarks";

export const RoundTimeline = ({ round }: { round: Round }) => {
  const timeline = createRoundTimeline(round);

  return (
    <div className="relative w-full h-4 bg-gray-600/40 rounded ">
      <RoundlineMinuteMarks
        durationSeconds={timeline.durationSeconds}
        percentFromSecond={timeline.percentFromSecond}
      />
      <RoundTimelineEvents
        kills={round.kills}
        bombEvents={round.bombEvents}
        percentFromTimestamp={timeline.percentFromTimestamp}
      />
    </div>
  );
};

const createRoundTimeline = (round: Round) => {
  const startMs = +new Date(round.startedAt);
  const endMs = +new Date(round.endedAt);
  const durationMs = Math.max(1, endMs - startMs);
  const durationSeconds = Math.max(1, Math.round(durationMs / 1000));

  return {
    durationSeconds,
    percentFromTimestamp: (timestamp: string) =>
      ((+new Date(timestamp) - startMs) / durationMs) * 100,
    percentFromSecond: (second: number) => (second / durationSeconds) * 100,
  };
};
