using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public struct FamilyTreeNode
	{
		public string Name;
		public byte Age;
		public byte ChildAge;
		public byte Class;
		public byte HeadPiece;
		public byte ChestPiece;
		public byte ShoulderPiece;
		public int NumEnemiesBeaten;
		public bool BeatenABoss;
		public bool IsFemale;
		public Vector2 Traits;
	}
}
