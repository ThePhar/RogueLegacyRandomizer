using System;
using System.Collections.Generic;

namespace Archipelago
{
    public class SlotData
    {
        private readonly Dictionary<string, object> _slotData;

        public SlotData(IDictionary<string, object> dictionary, string seed, int slot, string name)
        {
            _slotData = new Dictionary<string, object>(dictionary) { { "seed", seed }, { "slot", slot }, { "name", name } };
        }

        // Meta Information
        public string Seed => (string) _slotData["seed"];
        public int Slot => (int) _slotData["slot"];
        public string Name => (string) _slotData["name"];

        // Supported Rogue Legacy Options
        public bool DeathLink => Convert.ToInt32(_slotData["death_link"]) == 1;
        public bool IsFemale => Convert.ToInt32(_slotData["starting_gender"]) == 1;
        public byte StartingClass => Convert.ToByte(_slotData["starting_class"]);
        public int Difficulty => Convert.ToInt32(_slotData["new_game_plus"]);
        public int FairyChestsPerZone => Convert.ToInt32(_slotData["fairy_chests_per_zone"]);
        public int ChestsPerZone => Convert.ToInt32(_slotData["chests_per_zone"]);
        public bool RequirePurchasing => Convert.ToInt32(_slotData["require_purchasing"]) == 1;
        public int NumberOfChildren => Convert.ToInt32(_slotData["number_of_children"]);
        public bool DisableCharon => Convert.ToInt32(_slotData["disable_charon"]) == 1;

        public float GoldGainMultiplier => Convert.ToInt32(_slotData["gold_gain_multiplier"]) switch
        {
            1 => 0.25f,
            2 => 0.5f,
            3 => 2f,
            4 => 4f,
            _ => 1f,
        };
    }
}