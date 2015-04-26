/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public struct FamilyTreeNode
    {
        public byte Age;
        public bool BeatenABoss;
        public byte ChestPiece;
        public byte ChildAge;
        public byte Class;
        public byte HeadPiece;
        public bool IsFemale;
        public string Name;
        public int NumEnemiesBeaten;
        public byte ShoulderPiece;
        public Vector2 Traits;
    }
}