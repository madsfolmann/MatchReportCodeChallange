import type { Match } from "../../../Model/Match/Entities/Match";
import type { Team } from "../../../Model/Match/Entities/Team";
import { FormatTime } from "../../Shared/FormatTime";
import { MatchBoardTeam } from "./MatchBoardTeam";

export const MatchBoard = ({
  match,
  leftTeam,
  rightTeam,
}: {
  match: Match;
  leftTeam: Team;
  rightTeam: Team;
}) => {
  const leftTeamScore = match.rounds.filter(
    (round) => round.winner.teamName === leftTeam?.name
  ).length;
  const rightTeamScore = match.rounds.filter(
    (round) => round.winner.teamName === rightTeam?.name
  ).length;

  return (
    <div className="flex flex-row justify-between items-center w-full px-10">
      {leftTeam && (
        <MatchBoardTeam
          teamName={leftTeam.name}
          score={leftTeamScore}
          left={true}
          isWinner={leftTeam.name === match.winnerTeamName}
        />
      )}

      <div className="flex text-white">
        <div className="flex flex-col items-center gap-3">
          <div className="font-black font-stretch-150% text-4xl text-yellow-300 uppercase">
            {match.map}
          </div>
          {match.winnerTeamName ? (
            <div className="text-white font-black uppercase text-lg">
              Game Over
            </div>
          ) : (
            <div className="text-white font-black uppercase text-lg">
              <span className="font-light">Current round:</span>{" "}
              {match.totalRounds}
            </div>
          )}
          <div className="text-white font-black uppercase">
            {FormatTime.Duration(match.duration)}
          </div>
        </div>
      </div>

      {rightTeam && (
        <MatchBoardTeam
          teamName={rightTeam.name}
          score={rightTeamScore}
          left={false}
          isWinner={rightTeam.name === match.winnerTeamName}
        />
      )}
    </div>
  );
};
