/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
	public class GameTypes
	{
		public enum EnemyDifficulty
		{
			BASIC,
			ADVANCED,
			EXPERT,
			MINIBOSS
		}
		public enum DoorType
		{
			NULL,
			OPEN,
			LOCKED,
			BLOCKED
		}
		public enum LevelType
		{
			NONE,
			CASTLE,
			GARDEN,
			DUNGEON,
			TOWER
		}
		public enum WeaponType
		{
			NONE,
			DAGGER,
			SWORD,
			SPEAR,
			AXE
		}
		public enum ArmorType
		{
			NONE,
			HEAD,
			BODY,
			RING,
			FOOT,
			HAND,
			ALL
		}
		public enum EquipmentType
		{
			NONE,
			WEAPON,
			ARMOR
		}
		public enum StatType
		{
			STRENGTH,
			HEALTH,
			ENDURANCE,
			EQUIPLOAD
		}
		public enum SkillType
		{
			STRENGTH,
			HEALTH,
			DEFENSE
		}
		public const int CollisionType_NULL = 0;
		public const int CollisionType_WALL = 1;
		public const int CollisionType_PLAYER = 2;
		public const int CollisionType_ENEMY = 3;
		public const int CollisionType_ENEMYWALL = 4;
		public const int CollisionType_WALL_FOR_PLAYER = 5;
		public const int CollisionType_WALL_FOR_ENEMY = 6;
		public const int CollisionType_PLAYER_TRIGGER = 7;
		public const int CollisionType_ENEMY_TRIGGER = 8;
		public const int CollisionType_GLOBAL_TRIGGER = 9;
		public const int CollisionType_GLOBAL_DAMAGE_WALL = 10;
		public const int LogicSetType_NULL = 0;
		public const int LogicSetType_NONATTACK = 1;
		public const int LogicSetType_ATTACK = 2;
		public const int LogicSetType_CD = 3;
	}
}
