// 
// RogueLegacyArchipelago - SlotData.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;

namespace RogueCastle.Archipelago
{
    public class SlotData
    {
        private readonly Dictionary<string, object> m_slotData;

        public InitialGenderOption InitialGender
        {
            get
            {
                return (InitialGenderOption) int.Parse(m_slotData["initial_gender"].ToString());
            }
        }

        public DifficultyOption Difficulty
        {
            get
            {
                return (DifficultyOption) int.Parse(m_slotData["difficulty"].ToString());
            }
        }

        public bool DeathLink
        {
            get
            {
                return int.Parse(m_slotData["death_link"].ToString()) == 1;
            }
        }

        public SlotData(Dictionary<string, object> slotData)
        {
            m_slotData = slotData;
        }

        public enum InitialGenderOption
        {
            Male = 0,
            Female = 1,
        }

        public enum DifficultyOption
        {
            Normal = 0,
            NewGamePlus = 1,
            NewGamePlus2 = 2,
            NewGamePlus3 = 3,
        }
    }
}
