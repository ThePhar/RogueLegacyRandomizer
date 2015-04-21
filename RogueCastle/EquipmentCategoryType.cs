using System;
namespace RogueCastle
{
	public class EquipmentCategoryType
	{
		public const int Sword = 0;
		public const int Helm = 1;
		public const int Chest = 2;
		public const int Limbs = 3;
		public const int Cape = 4;
		public const int Total = 5;
		public static string ToString(int equipmentType)
		{
			switch (equipmentType)
			{
			case 0:
				return "Sword";
			case 1:
				return "Helm";
			case 2:
				return "Chest";
			case 3:
				return "Limbs";
			case 4:
				return "Cape";
			default:
				return "None";
			}
		}
		public static string ToString2(int equipmentType)
		{
			switch (equipmentType)
			{
			case 0:
				return "Sword";
			case 1:
				return "Helm";
			case 2:
				return "Chestplate";
			case 3:
				return "Bracers";
			case 4:
				return "Cape";
			default:
				return "None";
			}
		}
	}
}
