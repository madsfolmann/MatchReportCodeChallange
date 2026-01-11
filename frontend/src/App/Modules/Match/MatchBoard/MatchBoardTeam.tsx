export const MatchBoardTeam = ({
  teamName,
  score,
  isWinner,
  left,
}: {
  teamName: string;
  score: number;
  isWinner: boolean;
  left: boolean;
}) => {
  return (
    <div
      className={`flex flex-row items-center gap-4 border-4 py-3 px-5 rounded-2xl  ${
        left ? "" : "flex-row-reverse"
      } ${isWinner ? " border-yellow-300" : "border-yellow-300/0"}
`}
    >
      <div className="text-white font-extralight">{teamName}</div>
      <div className="text-3xl font-black text-yellow-300">{score}</div>
    </div>
  );
};
