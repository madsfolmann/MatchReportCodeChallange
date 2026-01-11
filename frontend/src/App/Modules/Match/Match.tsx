import { useEffect, useState } from "react";
import type { Match as MatchType } from "../../Model/Match/Entities/Match";
import { useMatchReport } from "../../Model/Match/Hooks/useMatchReport";
import { MatchTimeline } from "./MatchTimeline/MatchTimeline";
import { RoundsTable } from "../Round/RoundsTable";
import { MatchPlayers } from "./MatchPlayers";
import { MatchBoard } from "./MatchBoard/MatchBoard";

const Match = () => {
  const { fetchShowcaseMatch } = useMatchReport();
  const [match, setMatch] = useState<MatchType | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadMatch = async () => {
      try {
        const data = await fetchShowcaseMatch();
        setMatch(data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    loadMatch();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (!match) return <p>No match data</p>;

  const leftTeam = match.teams[0];
  const rightTeam = match.teams[1];

  return (
    <div className="flex justify-center w-full h-full ">
      <div className="flex flex-col w-full h-full gap-20 pt-12">
        <MatchBoard match={match} leftTeam={leftTeam} rightTeam={rightTeam} />

        <MatchPlayers players={match.players} teamLeft={leftTeam.name} />

        <MatchTimeline match={match} />

        <RoundsTable match={match} />
      </div>
    </div>
  );
};

export default Match;
