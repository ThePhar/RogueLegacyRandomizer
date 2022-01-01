//
//  RogueLegacyArchipelago - SlotData.cs
//  Last Modified 2021-12-29
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Archipelago
{
    public class SlotData : ReadOnlyDictionary<string, object>
    {
        public SlotData(IDictionary<string, object> dictionary, string seed, int slot, string name) : base(
            dictionary)
        {
            dictionary.Add("seed", seed);
            dictionary.Add("slot", slot);
            dictionary.Add("name", name);
        }

        public string Seed
        {
            get { return (string) this["seed"]; }
        }

        public int Slot
        {
            get { return (int) this["slot"]; }
        }

        public string Name
        {
            get { return (string) this["name"]; }
        }

        public bool DeathLink
        {
            get { return Convert.ToInt32(this["death_link"]) == 1; }
        }

        public bool IsFemale
        {
            get { return Convert.ToInt32(this["starting_gender"]) == 1; }
        }

        public int Difficulty
        {
            get { return Convert.ToInt32(this["new_game_plus"]); }
        }

        public int FairyChestsPerZone
        {
            get { return Convert.ToInt32(this["fairy_chests_per_zone"]); }
        }

        public int ChestsPerZone
        {
            get { return Convert.ToInt32(this["chests_per_zone"]); }
        }

        public bool RequirePurchasing
        {
            get { return Convert.ToInt32(this["require_purchasing"]) == 1; }
        }

        public float GoldGainMultiplier
        {
            get
            {
                var option = Convert.ToInt32(this["gold_gain_multiplier"]);

                switch (option)
                {
                    case 1:
                        return 0.25f;

                    case 2:
                        return 0.5f;

                    case 3:
                        return 2f;

                    case 4:
                        return 4f;

                    default:
                        return 1f;
                }
            }
        }

        public int NumberOfChildren
        {
            get { return Convert.ToInt32(this["number_of_children"]); }
        }

        public bool DisableCharon
        {
            get { return Convert.ToInt32(this["disable_charon"]) == 1; }
        }
    }
}
