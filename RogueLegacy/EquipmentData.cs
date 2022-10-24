// Rogue Legacy Randomizer - EquipmentData.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class EquipmentData
    {
        public int BonusArmor;
        public int BonusDamage;
        public int BonusHealth;
        public int BonusMagic;
        public int BonusMana;
        public byte ChestColourRequirement;
        public int Cost = 9999;
        public Color FirstColour = Color.White;
        public byte LevelRequirement;
        public Vector2[] SecondaryAttribute;
        public Color SecondColour = Color.White;
        public int Weight;

        public void Dispose()
        {
            if (SecondaryAttribute != null)
            {
                Array.Clear(SecondaryAttribute, 0, SecondaryAttribute.Length);
            }

            SecondaryAttribute = null;
        }
    }
}
