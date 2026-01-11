import type { Weapon } from "../Types/Weapon";

export interface WeaponStats {
  weapon: Weapon;
  kills: number;
  headshots: number;
}
