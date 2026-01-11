import type { Player } from "../../Model/Match/Entities/Player";
import { PlayerCard } from "../Player/PlayerCard";

export const MatchPlayers = ({
  players,
  teamLeft,
}: {
  players: Player[];
  teamLeft: string;
}) => {
  const { leftSide, rightSide } = arrangeTeamsByKills(players, teamLeft);

  return (
    <div className="flex w-full justify-evenly items-center ">
      <div className="flex space-x-4">
        {leftSide.reverse().map((player) => (
          <PlayerCard key={player.steamId} player={player} />
        ))}
      </div>
      <div className="flex space-x-4">
        {rightSide.map((player) => (
          <PlayerCard key={player.steamId} player={player} />
        ))}
      </div>
    </div>
  );
};

function arrangeTeamsByKills(players: Player[], teamLeft: string) {
  const leftSide = players
    .filter((p) => p.teamName === teamLeft)
    .sort((a, b) => b.kills - a.kills);
  const rightSide = players
    .filter((p) => p.teamName !== teamLeft)
    .sort((a, b) => b.kills - a.kills);

  return { leftSide, rightSide };
}
