using System;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class EquipmentData
	{
		public int BonusDamage;
		public int BonusMagic;
		public int Weight;
		public int BonusMana;
		public int BonusHealth;
		public int BonusArmor;
		public int Cost = 9999;
		public Color FirstColour = Color.White;
		public Color SecondColour = Color.White;
		public Vector2[] SecondaryAttribute;
		public byte ChestColourRequirement;
		public byte LevelRequirement;
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
