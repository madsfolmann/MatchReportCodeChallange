export const RoundlineMinuteMarks = ({
  durationSeconds,
  percentFromSecond,
}: {
  durationSeconds: number;
  percentFromSecond: (second: number) => number;
}) => {
  const marks = getMarksEvery5Second(durationSeconds);

  return (
    <>
      {marks.map((second) => {
        const leftPercent = percentFromSecond(second);

        return (
          <div
            key={`second-${second}`}
            className="absolute top-0 flex flex-col items-center"
            style={{ left: `${leftPercent}%` }}
          >
            <div className="h-2 w-px bg-gray-500"></div>
            <div className="text-[8px] text-gray-400 mt-1">{second}s</div>
          </div>
        );
      })}
    </>
  );
};

const getMarksEvery5Second = (roundDurationSeconds: number) => {
  const count = Math.floor(roundDurationSeconds / 5) + 1;
  return Array.from({ length: count }, (_, i) => i * 5);
};
