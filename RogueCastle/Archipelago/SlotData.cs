// 
// RogueLegacyArchipelago - SlotData.cs
// Last Modified 2021-12-26
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;

namespace RogueCastle.Archipelago
{
    public class SlotData
    {
        private readonly Dictionary<string, object> m_slotData;

        public StartingGender StartingGender
        {

            get { return (StartingGender) Convert.ToInt32(m_slotData["starting_gender"]); }
        }

        public int Difficulty
        {
            get { return Convert.ToInt32(m_slotData["new_game_plus"]); }
        }

        public bool DeathLink
        {
            get { return Convert.ToInt32(m_slotData["death_link"]) == 1; }
        }

        public SlotData(Dictionary<string, object> slotData)
        {
            m_slotData = slotData;
        }
    }

    public enum StartingGender
    {
        Sir = 0,
        Lady = 1,
    }
}
