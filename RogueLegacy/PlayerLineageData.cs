// Rogue Legacy Randomizer - PlayerLineageData.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public struct PlayerLineageData
    {
        public byte Age;
        public byte ChestPiece;
        public byte ChildAge;
        public byte Class;
        public byte HeadPiece;
        public bool IsFemale;
        public string Name;
        public byte ShoulderPiece;
        public byte Spell;
        public Vector2 Traits;
    }
}
