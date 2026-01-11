import { User } from "lucide-react";
import type { Player } from "../../Model/Match/Entities/Player";

export const PlayerCard = ({ player }: { player: Player }) => {
  return (
    <div className="flex flex-col gap-y-3 justify-center items-center bg-yellow-400 rounded-lg w-32 h-40">
      <div className="flex gap-x-2 justify-center items-center">
        <div className="flex justify-center items-center rounded-full bg-gray-500/15 w-8 h-8">
          <User className="w-4 h-4 text-gray-700" />
        </div>
        <div className="font-bold text-xs">{player.name}</div>
      </div>
      <div className="gap-y-1 flex flex-col justify-center items-center">
        <div className="flex gap-x-1 justify-center items-center text-xs font-light">
          <StatBox label="K / D" value={`${player.kills} / ${player.deaths}`} />
          <StatBox label="Ratio" value={player.kd?.toFixed(2)} />
        </div>
        <StatBox
          label="Weapon"
          value={player.weaponWithMostKills}
          valueClass="bg-gray-500/25 rounded-full px-2 py-1 text-[11px]"
        />
      </div>
    </div>
  );
};

const StatBox = ({
  label,
  value,
  valueClass = "bg-gray-500/25 rounded-full px-2 py-1 text-xs",
}: {
  label: string;
  value: any;
  valueClass?: string;
}) => (
  <div className="flex flex-col items-center">
    <span className="text-[8px] mb-0.5">{label}</span>
    <span className={valueClass}>{value}</span>
  </div>
);
