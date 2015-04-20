/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;

namespace RogueCastle
{
	internal class SpellEV
	{
		public const string AXE_Name = "Axe";
		public const int AXE_Cost = 15;
		public const float AXE_Damage = 1f;
		public const float AXE_XVal = 0f;
		public const float AXE_YVal = 0f;
		public const int AXE_Rarity = 1;
		public const string DAGGER_Name = "Dagger";
		public const int DAGGER_Cost = 10;
		public const float DAGGER_Damage = 1f;
		public const float DAGGER_XVal = 0f;
		public const float DAGGER_YVal = 0f;
		public const int DAGGER_Rarity = 1;
		public const string TIMEBOMB_Name = "Runic Trigger";
		public const int TIMEBOMB_Cost = 15;
		public const float TIMEBOMB_Damage = 1.5f;
		public const float TIMEBOMB_XVal = 1f;
		public const float TIMEBOMB_YVal = 0f;
		public const int TIMEBOMB_Rarity = 1;
		public const string TIMESTOP_Name = "Stop Watch";
		public const int TIMESTOP_Cost = 15;
		public const float TIMESTOP_Damage = 0f;
		public const float TIMESTOP_XVal = 3f;
		public const float TIMESTOP_YVal = 0f;
		public const int TIMESTOP_Rarity = 2;
		public const string NUKE_Name = "Nuke";
		public const int NUKE_Cost = 40;
		public const float NUKE_Damage = 0.75f;
		public const float NUKE_XVal = 0f;
		public const float NUKE_YVal = 0f;
		public const int NUKE_Rarity = 3;
		public const string TRANSLOCATER_Name = "Quantum Translocater";
		public const int TRANSLOCATER_Cost = 5;
		public const float TRANSLOCATER_Damage = 0f;
		public const float TRANSLOCATER_XVal = 0f;
		public const float TRANSLOCATER_YVal = 0f;
		public const int TRANSLOCATER_Rarity = 3;
		public const string DISPLACER_Name = "Displacer";
		public const int DISPLACER_Cost = 10;
		public const float DISPLACER_Damage = 0f;
		public const float DISPLACER_XVal = 0f;
		public const float DISPLACER_YVal = 0f;
		public const int DISPLACER_Rarity = 0;
		public const string BOOMERANG_Name = "Cross";
		public const int BOOMERANG_Cost = 15;
		public const float BOOMERANG_Damage = 1f;
		public const float BOOMERANG_XVal = 18f;
		public const float BOOMERANG_YVal = 0f;
		public const int BOOMERANG_Rarity = 2;
		public const string DUAL_BLADES_Name = "Spark";
		public const int DUAL_BLADES_Cost = 15;
		public const float DUAL_BLADES_Damage = 1f;
		public const float DUAL_BLADES_XVal = 0f;
		public const float DUAL_BLADES_YVal = 0f;
		public const int DUAL_BLADES_Rarity = 1;
		public const string CLOSE_Name = "Katana";
		public const int CLOSE_Cost = 15;
		public const float CLOSE_Damage = 0.5f;
		public const float CLOSE_XVal = 2.1f;
		public const float CLOSE_YVal = 0f;
		public const int CLOSE_Rarity = 2;
		public const string DAMAGE_SHIELD_Name = "Leaf";
		public const int DAMAGE_SHIELD_Cost = 15;
		public const float DAMAGE_SHIELD_Damage = 1f;
		public const float DAMAGE_SHIELD_XVal = 9999f;
		public const float DAMAGE_SHIELD_YVal = 5f;
		public const int DAMAGE_SHIELD_Rarity = 2;
		public const string BOUNCE_Name = "Chaos";
		public const int BOUNCE_Cost = 30;
		public const float BOUNCE_Damage = 0.4f;
		public const float BOUNCE_XVal = 3.5f;
		public const float BOUNCE_YVal = 0f;
		public const int BOUNCE_Rarity = 3;
		public const string LASER_Name = "Laser";
		public const int LASER_Cost = 15;
		public const float LASER_Damage = 1f;
		public const float LASER_XVal = 5f;
		public const float LASER_YVal = 0f;
		public const int LASER_Rarity = 3;
		public const string DRAGONFIRE_Name = "Dragon Fire";
		public const int DRAGONFIRE_Cost = 15;
		public const float DRAGONFIRE_Damage = 1f;
		public const float DRAGONFIRE_XVal = 0.35f;
		public const float DRAGONFIRE_YVal = 0f;
		public const int DRAGONFIRE_Rarity = 3;
		public const string RAPIDDAGGER_Name = "Rapid Dagger";
		public const int RAPIDDAGGER_Cost = 30;
		public const float RAPIDDAGGER_Damage = 0.75f;
		public const float RAPIDDAGGER_XVal = 0f;
		public const float RAPIDDAGGER_YVal = 0f;
		public const int RAPIDDAGGER_Rarity = 1;
		public const string DRAGONFIRENEO_Name = "Dragon Fire Neo";
		public const int DRAGONFIRENEO_Cost = 0;
		public const float DRAGONFIRENEO_Damage = 1f;
		public const float DRAGONFIRENEO_XVal = 0.75f;
		public const float DRAGONFIRENEO_YVal = 0f;
		public const int DRAGONFIRENEO_Rarity = 3;
		public static ProjectileData GetProjData(byte spellType, PlayerObj player)
		{
			ProjectileData projectileData = new ProjectileData(player)
			{
				SpriteName = "BoneProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = 0,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = Vector2.One,
				ShowIcon = false
			};
			switch (spellType)
			{
			case 1:
				projectileData.SpriteName = "SpellDagger_Sprite";
				projectileData.Angle = Vector2.Zero;
				projectileData.SourceAnchor = new Vector2(50f, 0f);
				projectileData.Speed = new Vector2(1750f, 1750f);
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = true;
				projectileData.DestroysWithTerrain = true;
				projectileData.Scale = new Vector2(2.5f, 2.5f);
				break;
			case 2:
				projectileData.SpriteName = "SpellAxe_Sprite";
				projectileData.Angle = new Vector2(-74f, -74f);
				projectileData.Speed = new Vector2(1050f, 1050f);
				projectileData.SourceAnchor = new Vector2(50f, -50f);
				projectileData.IsWeighted = true;
				projectileData.RotationSpeed = 10f;
				projectileData.CollidesWithTerrain = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.Scale = new Vector2(3f, 3f);
				break;
			case 3:
				projectileData.SpriteName = "SpellTimeBomb_Sprite";
				projectileData.Angle = new Vector2(-35f, -35f);
				projectileData.Speed = new Vector2(500f, 500f);
				projectileData.SourceAnchor = new Vector2(50f, -50f);
				projectileData.IsWeighted = true;
				projectileData.RotationSpeed = 0f;
				projectileData.StartingRotation = 0f;
				projectileData.CollidesWithTerrain = true;
				projectileData.DestroysWithTerrain = false;
				projectileData.CollidesWith1Ways = true;
				projectileData.Scale = new Vector2(3f, 3f);
				break;
			case 4:
			case 6:
				break;
			case 5:
				projectileData.SpriteName = "SpellNuke_Sprite";
				projectileData.Angle = new Vector2(-65f, -65f);
				projectileData.Speed = new Vector2(500f, 500f);
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.ChaseTarget = false;
				projectileData.DestroysWithEnemy = true;
				projectileData.Scale = new Vector2(2f, 2f);
				break;
			case 7:
				projectileData.SourceAnchor = new Vector2(0f, 0f);
				projectileData.SpriteName = "SpellDisplacer_Sprite";
				projectileData.Angle = new Vector2(0f, 0f);
				projectileData.Speed = Vector2.Zero;
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = true;
				projectileData.DestroysWithTerrain = false;
				projectileData.CollidesWith1Ways = true;
				projectileData.Scale = new Vector2(2f, 2f);
				break;
			case 8:
				projectileData.SpriteName = "SpellBoomerang_Sprite";
				projectileData.Angle = new Vector2(0f, 0f);
				projectileData.SourceAnchor = new Vector2(50f, -10f);
				projectileData.Speed = new Vector2(790f, 790f);
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 25f;
				projectileData.CollidesWithTerrain = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.Scale = new Vector2(3f, 3f);
				break;
			case 9:
				projectileData.SpriteName = "SpellDualBlades_Sprite";
				projectileData.Angle = new Vector2(-55f, -55f);
				projectileData.SourceAnchor = new Vector2(50f, 30f);
				projectileData.Speed = new Vector2(1000f, 1000f);
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 30f;
				projectileData.CollidesWithTerrain = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.Scale = new Vector2(2f, 2f);
				break;
			case 10:
				projectileData.SpriteName = "SpellClose_Sprite";
				projectileData.SourceAnchor = new Vector2(120f, -60f);
				projectileData.Speed = new Vector2(0f, 0f);
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.DestroysWithEnemy = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.CollidesWithTerrain = false;
				projectileData.Scale = new Vector2(2.5f, 2.5f);
				projectileData.LockPosition = true;
				break;
			case 11:
				projectileData.SpriteName = "SpellDamageShield_Sprite";
				projectileData.Angle = new Vector2(-65f, -65f);
				projectileData.Speed = new Vector2(3.25f, 3.25f);
				projectileData.Target = player;
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = false;
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.Scale = new Vector2(3f, 3f);
				projectileData.DestroyOnRoomTransition = false;
				break;
			case 12:
				projectileData.SpriteName = "SpellBounce_Sprite";
				projectileData.Angle = new Vector2(-135f, -135f);
				projectileData.Speed = new Vector2(785f, 785f);
				projectileData.IsWeighted = false;
				projectileData.StartingRotation = -135f;
				projectileData.FollowArc = false;
				projectileData.RotationSpeed = 20f;
				projectileData.SourceAnchor = new Vector2(-10f, -10f);
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.CollidesWithTerrain = true;
				projectileData.Scale = new Vector2(3.25f, 3.25f);
				break;
			case 13:
				projectileData.SpriteName = "TurretProjectile_Sprite";
				projectileData.Angle = Vector2.Zero;
				projectileData.SourceAnchor = new Vector2(50f, 0f);
				projectileData.Speed = new Vector2(1100f, 1100f);
				projectileData.Lifespan = 0.35f;
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = true;
				projectileData.DestroysWithTerrain = true;
				projectileData.Scale = new Vector2(2.5f, 2.5f);
				break;
			case 14:
				projectileData.SpriteName = "LaserSpell_Sprite";
				projectileData.Angle = new Vector2(0f, 0f);
				projectileData.Speed = new Vector2(0f, 0f);
				projectileData.IsWeighted = false;
				projectileData.IsCollidable = false;
				projectileData.StartingRotation = 0f;
				projectileData.FollowArc = false;
				projectileData.RotationSpeed = 0f;
				projectileData.DestroysWithTerrain = false;
				projectileData.DestroysWithEnemy = false;
				projectileData.CollidesWithTerrain = false;
				projectileData.LockPosition = true;
				break;
			case 15:
				projectileData.SpriteName = "TurretProjectile_Sprite";
				projectileData.Angle = Vector2.Zero;
				projectileData.SourceAnchor = new Vector2(50f, 0f);
				projectileData.Speed = new Vector2(1750f, 1750f);
				projectileData.Lifespan = 0.75f;
				projectileData.IsWeighted = false;
				projectileData.RotationSpeed = 0f;
				projectileData.CollidesWithTerrain = true;
				projectileData.DestroysWithTerrain = true;
				projectileData.Scale = new Vector2(2.75f, 2.75f);
				break;
			default:
				if (spellType == 100)
				{
					projectileData.SpriteName = "LaserSpell_Sprite";
					projectileData.Angle = new Vector2(0f, 0f);
					projectileData.Speed = new Vector2(0f, 0f);
					projectileData.IsWeighted = false;
					projectileData.IsCollidable = false;
					projectileData.StartingRotation = 0f;
					projectileData.FollowArc = false;
					projectileData.RotationSpeed = 0f;
					projectileData.DestroysWithTerrain = false;
					projectileData.DestroysWithEnemy = false;
					projectileData.CollidesWithTerrain = false;
					projectileData.LockPosition = true;
				}
				break;
			}
			return projectileData;
		}
		public static int GetManaCost(byte spellType)
		{
			switch (spellType)
			{
			case 1:
				return 10;
			case 2:
				return 15;
			case 3:
				return 15;
			case 4:
				return 15;
			case 5:
				return 40;
			case 6:
				return 5;
			case 7:
				return 10;
			case 8:
				return 15;
			case 9:
				return 15;
			case 10:
				return 15;
			case 11:
				return 15;
			case 12:
				return 30;
			case 13:
				return 15;
			case 14:
				return 30;
			case 15:
				return 0;
			default:
				if (spellType != 100)
				{
					return 0;
				}
				return 15;
			}
		}
		public static int GetRarity(byte spellType)
		{
			switch (spellType)
			{
			case 1:
				return 1;
			case 2:
				return 1;
			case 3:
				return 1;
			case 4:
				return 2;
			case 5:
				return 3;
			case 6:
				return 3;
			case 7:
				return 0;
			case 8:
				return 2;
			case 9:
				return 1;
			case 10:
				return 2;
			case 11:
				return 2;
			case 12:
				return 3;
			case 13:
				return 3;
			case 14:
				return 1;
			case 15:
				return 3;
			default:
				if (spellType != 100)
				{
					return 0;
				}
				return 3;
			}
		}
		public static float GetDamageMultiplier(byte spellType)
		{
			switch (spellType)
			{
			case 1:
				return 1f;
			case 2:
				return 1f;
			case 3:
				return 1.5f;
			case 4:
				return 0f;
			case 5:
				return 0.75f;
			case 6:
				return 0f;
			case 7:
				return 0f;
			case 8:
				return 1f;
			case 9:
				return 1f;
			case 10:
				return 0.5f;
			case 11:
				return 1f;
			case 12:
				return 0.4f;
			case 13:
				return 1f;
			case 14:
				return 0.75f;
			case 15:
				return 1f;
			default:
				if (spellType != 100)
				{
					return 0f;
				}
				return 1f;
			}
		}
		public static float GetXValue(byte spellType)
		{
			switch (spellType)
			{
			case 1:
				return 0f;
			case 2:
				return 0f;
			case 3:
				return 1f;
			case 4:
				return 3f;
			case 5:
				return 0f;
			case 6:
				return 0f;
			case 7:
				return 0f;
			case 8:
				return 18f;
			case 9:
				return 0f;
			case 10:
				return 2.1f;
			case 11:
				return 9999f;
			case 12:
				return 3.5f;
			case 13:
				return 0.35f;
			case 14:
				break;
			case 15:
				return 0.75f;
			default:
				if (spellType == 100)
				{
					return 5f;
				}
				break;
			}
			return 0f;
		}
		public static float GetYValue(byte spellType)
		{
			switch (spellType)
			{
			case 1:
				return 0f;
			case 2:
				return 0f;
			case 3:
				return 0f;
			case 4:
				return 0f;
			case 5:
				return 0f;
			case 6:
				return 0f;
			case 7:
				return 0f;
			case 8:
				return 0f;
			case 9:
				return 0f;
			case 10:
				return 0f;
			case 11:
				return 5f;
			case 12:
				return 0f;
			case 13:
				return 0f;
			case 14:
				break;
			case 15:
				return 0f;
			default:
				if (spellType == 100)
				{
					return 0f;
				}
				break;
			}
			return 0f;
		}
	}
}
