// 
// RogueLegacyArchipelago - LegacySlotData.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Archipelago.Legacy
{
    public class LegacySlotData : ReadOnlyDictionary<string, object>
    {
        public LegacySlotData(IDictionary<string, object> dictionary, string seed, int slot) : base(dictionary)
        {
            dictionary.Add("seed", seed);
            dictionary.Add("slot", slot);
        }

        /// <summary>
        /// The seed number for this session.
        /// </summary>
        public string Seed
        {
            get { return (string) this["seed"]; }
        }

        /// <summary>
        /// The slot number for this player.
        /// </summary>
        public int Slot
        {
            get { return (int) this["slot"]; }
        }

        /// <summary>
        /// Determines the starting gender of the player's first character.
        /// </summary>
        public StartingGender StartingGender
        {
            get { return (StartingGender) Convert.ToInt32(this["starting_gender"]); }
        }

        /// <summary>
        /// Determines how many times the castle has been beaten, which scales enemy levels accordingly.
        /// </summary>
        public int Difficulty
        {
            get { return Convert.ToInt32(this["new_game_plus"]); }
        }

        /// <summary>
        /// Determines if the player should be killed when other players with DeathLink enabled die and vice-versa.
        /// </summary>
        public bool DeathLink
        {
            get { return Convert.ToInt32(this["death_link"]) == 1; }
        }
    }

    public enum StartingGender
    {
        Sir  = 0,
        Lady = 1,
    }
}
