import type { Match } from "../../../Model/Match/Entities/Match";
import { MatchTimelineMinuteMarks } from "./MatchTimelineMinuteMarks";
import { MatchTimelineRounds } from "./MatchTimelineRounds";

export const MatchTimeline = ({ match }: { match: Match }) => {
  return (
    <div className="w-full h-30 flex items-center relative bg-gray-600/40">
      <MatchTimelineMinuteMarks match={match} />
      <MatchTimelineRounds match={match} />
    </div>
  );
};
