import type { Weapon } from "../Types/Weapon";

export interface Player {
  steamId: string;
  name: string;
  teamName: string;
  kills: number;
  deaths: number;
  weaponWithMostKills?: Weapon;
  kd: number;
}
