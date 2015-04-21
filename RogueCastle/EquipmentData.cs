using Microsoft.Xna.Framework;
using System;
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
			if (this.SecondaryAttribute != null)
			{
				Array.Clear(this.SecondaryAttribute, 0, this.SecondaryAttribute.Length);
			}
			this.SecondaryAttribute = null;
		}
	}
}
