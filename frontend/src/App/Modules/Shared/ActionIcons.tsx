import { Bomb, Clock, Skull, X } from "lucide-react";
import type { JSX } from "react";

type IconFactory = (color?: string) => JSX.Element;

export const ActionIcons: Partial<Record<string, IconFactory>> = {
  BombPlanted: (color) => (
    <Bomb className={`w-4 h-4 ${color ?? "text-yellow-400"}`} />
  ),
  BombExploded: (color) => (
    <svg
      className={`w-4 h-4 ${color ?? "text-yellow-500"}`}
      viewBox="0 0 20 20"
      fill="none"
      stroke="currentColor"
      strokeWidth={1.5}
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <circle cx="10" cy="10" r="4" fill="currentColor" />
      <g stroke="currentColor" strokeWidth={2}>
        <line x1="10" y1="1" x2="10" y2="4" />
        <line x1="10" y1="16" x2="10" y2="19" />
        <line x1="1" y1="10" x2="4" y2="10" />
        <line x1="16" y1="10" x2="19" y2="10" />
        <line x1="4" y1="4" x2="6" y2="6" />
        <line x1="16" y1="16" x2="14" y2="14" />
        <line x1="4" y1="16" x2="6" y2="14" />
        <line x1="16" y1="4" x2="14" y2="6" />
      </g>
    </svg>
  ),
  BombDefused: (color) => (
    <span className="relative inline-block w-5 h-4 align-middle">
      <Bomb
        className={`w-4 h-4 absolute left-0 top-0 ${
          color ?? "text-yellow-400"
        }`}
      />
      <X
        className={`w-3 h-3 absolute left-0 top-0 stroke-8 ${
          color ?? "text-yellow-400"
        }`}
      />
    </span>
  ),
  Headshot: (color) => (
    <Skull className={`w-4 h-4 ${color ?? "text-red-700"}`} />
  ),
  Timeout: (color) => <Clock className={`w-4 h-4 ${color ?? "text-white"}`} />,
};
