/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;

namespace RogueCastle
{
	public class EnemyBuilder
	{
		public static EnemyObj BuildEnemy(int enemyType, PlayerObj player, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty, bool doNotInitialize = false)
		{
			EnemyObj enemyObj = null;
			switch (enemyType)
			{
			case 1:
				enemyObj = new EnemyObj_BallAndChain(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 2:
				enemyObj = new EnemyObj_Blob(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 3:
				enemyObj = new EnemyObj_BouncySpike(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 4:
				enemyObj = new EnemyObj_Eagle(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 5:
				enemyObj = new EnemyObj_EarthWizard(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 6:
				enemyObj = new EnemyObj_Eyeball(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 7:
				enemyObj = new EnemyObj_Fairy(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 8:
				enemyObj = new EnemyObj_Fireball(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 9:
				enemyObj = new EnemyObj_FireWizard(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 10:
				enemyObj = new EnemyObj_Horse(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 11:
				enemyObj = new EnemyObj_IceWizard(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 12:
				enemyObj = new EnemyObj_Knight(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 13:
				enemyObj = new EnemyObj_Ninja(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 14:
				enemyObj = new EnemyObj_ShieldKnight(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 15:
				enemyObj = new EnemyObj_Skeleton(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 16:
				enemyObj = new EnemyObj_SwordKnight(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 17:
				enemyObj = new EnemyObj_Turret(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 18:
				enemyObj = new EnemyObj_Wall(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 19:
				enemyObj = new EnemyObj_Wolf(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 20:
				enemyObj = new EnemyObj_Zombie(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 21:
				enemyObj = new EnemyObj_SpikeTrap(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 22:
				enemyObj = new EnemyObj_Plant(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 23:
				enemyObj = new EnemyObj_Energon(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 24:
				enemyObj = new EnemyObj_Spark(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 25:
				enemyObj = new EnemyObj_SkeletonArcher(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 26:
				enemyObj = new EnemyObj_Chicken(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 27:
				enemyObj = new EnemyObj_Platform(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 28:
				enemyObj = new EnemyObj_HomingTurret(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 29:
				enemyObj = new EnemyObj_LastBoss(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 30:
				enemyObj = new EnemyObj_Dummy(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 31:
				enemyObj = new EnemyObj_Starburst(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 32:
				enemyObj = new EnemyObj_Portrait(player, physicsManager, levelToAttachTo, difficulty);
				break;
			case 33:
				enemyObj = new EnemyObj_Mimic(player, physicsManager, levelToAttachTo, difficulty);
				break;
			}
			if (player == null && !doNotInitialize)
			{
				enemyObj.Initialize();
			}
			return enemyObj;
		}
	}
}
