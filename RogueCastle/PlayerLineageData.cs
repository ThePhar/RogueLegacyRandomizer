/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public struct PlayerLineageData
	{
		public string Name;
		public byte Spell;
		public byte Class;
		public byte Age;
		public byte ChildAge;
		public bool IsFemale;
		public byte HeadPiece;
		public byte ChestPiece;
		public byte ShoulderPiece;
		public Vector2 Traits;
	}
}
