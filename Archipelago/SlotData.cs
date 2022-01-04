using System;
using System.Collections.Generic;

namespace Archipelago
{
    public class SlotData : Dictionary<string, object>
    {
        public SlotData(IDictionary<string, object> dictionary, string seed, int slot, string name) : base(dictionary)
        {
            dictionary.Add("seed", seed);
            dictionary.Add("slot", slot);
            dictionary.Add("name", name);
        }

        // Meta Information
        public string Seed => (string) this["seed"];
        public int Slot => (int) this["slot"];
        public string Name => (string) this["name"];

        // Supported Rogue Legacy Options
        public bool DeathLink => Convert.ToInt32(this["death_link"]) == 1;
        public bool IsFemale => Convert.ToInt32(this["starting_gender"]) == 1;
        public byte StartingClass => Convert.ToByte(this["starting_class"]);
        public int Difficulty => Convert.ToInt32(this["new_game_plus"]);
        public int FairyChestsPerZone => Convert.ToInt32(this["fairy_chests_per_zone"]);
        public int ChestsPerZone => Convert.ToInt32(this["chests_per_zone"]);
        public bool RequirePurchasing => Convert.ToInt32(this["require_purchasing"]) == 1;
        public int NumberOfChildren => Convert.ToInt32(this["number_of_children"]);
        public bool DisableCharon => Convert.ToInt32(this["disable_charon"]) == 1;

        public float GoldGainMultiplier
        {
            get
            {
                return Convert.ToInt32(this["gold_gain_multiplier"]) switch
                {
                    1 => 0.25f,
                    2 => 0.5f,
                    3 => 2f,
                    4 => 4f,
                    _ => 1f,
                };
            }
        }
    }
}