/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class EnemyObj_Blob : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalBossCooldownLB = new LogicBlock();
		private LogicBlock m_generalNeoLB = new LogicBlock();
		private int NumHits;
		private Vector2 BlobSizeChange = new Vector2(0.4f, 0.4f);
		private float BlobSpeedChange = 1.2f;
		private float ExpertBlobProjectileDuration = 5f;
		private float JumpDelay = 0.4f;
		public RoomObj SpawnRoom;
		private int m_bossCoins = 40;
		private int m_bossMoneyBags = 16;
		private int m_bossDiamonds = 8;
		private int m_bossEarthWizardLevelReduction = 12;
		private bool m_isNeo;
		public bool MainBlob
		{
			get;
			set;
		}
		public Vector2 SavedStartingPos
		{
			get;
			set;
		}
		public bool IsNeo
		{
			get
			{
				return m_isNeo;
			}
			set
			{
				m_isNeo = value;
				if (value)
				{
					HealthGainPerLevel = 0;
					DamageGainPerLevel = 0;
					MoneyDropChance = 0f;
					ItemDropChance = 0f;
					m_saveToEnemiesKilledList = false;
				}
			}
		}
		protected override void InitializeEV()
		{
			SetNumberOfHits(2);
			BlobSizeChange = new Vector2(0.725f, 0.725f);
			BlobSpeedChange = 2f;
			Name = "Bloob";
			MaxHealth = 14;
			Damage = 13;
			XPValue = 25;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.225f;
			Speed = 50f;
			TurnSpeed = 10f;
			ProjectileSpeed = 0f;
			JumpHeight = 975f;
			CooldownTime = 2f;
			AnimationDelay = 0.05f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = true;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.Blob_Basic_Scale;
			ProjectileScale = EnemyEV.Blob_Basic_ProjectileScale;
			MeleeRadius = 225;
			ProjectileRadius = 500;
			EngageRadius = 750;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Blob_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				SetNumberOfHits(3);
				BlobSizeChange = new Vector2(0.6f, 0.6f);
				BlobSpeedChange = 2.25f;
				Name = "Bloobite";
				MaxHealth = 18;
				Damage = 14;
				XPValue = 29;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 1;
				MoneyDropChance = 0.2f;
				Speed = 80f;
				TurnSpeed = 10f;
				ProjectileSpeed = 0f;
				JumpHeight = 975f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Blob_Advanced_Scale;
				ProjectileScale = EnemyEV.Blob_Advanced_ProjectileScale;
				MeleeRadius = 225;
				EngageRadius = 750;
				ProjectileRadius = 500;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Blob_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				SetNumberOfHits(4);
				BlobSizeChange = new Vector2(0.65f, 0.65f);
				BlobSpeedChange = 2.25f;
				Name = "Bloobasaurus Rex";
				MaxHealth = 22;
				Damage = 16;
				XPValue = 35;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 1;
				MoneyDropChance = 0.1f;
				Speed = 90f;
				TurnSpeed = 10f;
				ProjectileSpeed = 0f;
				JumpHeight = 975f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Blob_Expert_Scale;
				ProjectileScale = EnemyEV.Blob_Expert_ProjectileScale;
				MeleeRadius = 225;
				ProjectileRadius = 500;
				EngageRadius = 750;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Blob_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				SetNumberOfHits(5);
				BlobSizeChange = new Vector2(0.6f, 0.6f);
				BlobSpeedChange = 2f;
				ForceDraw = true;
				Name = "Herodotus";
				MaxHealth = 32;
				Damage = 16;
				XPValue = 70;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 0.1f;
				Speed = 110f;
				TurnSpeed = 10f;
				ProjectileSpeed = 0f;
				JumpHeight = 975f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Blob_Miniboss_Scale;
				ProjectileScale = EnemyEV.Blob_Miniboss_ProjectileScale;
				MeleeRadius = 225;
				ProjectileRadius = 500;
				EngageRadius = 750;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Blob_Miniboss_KnockBack;
				if (LevelEV.WEAKEN_BOSSES)
				{
					MaxHealth = 1;
					SetNumberOfHits(1);
				}
				break;
			}
			if (Difficulty == GameTypes.EnemyDifficulty.BASIC)
			{
				_objectList[0].TextureColor = Color.Green;
				_objectList[2].TextureColor = Color.LightGreen;
				_objectList[2].Opacity = 0.8f;
				(_objectList[1] as SpriteObj).OutlineColour = Color.Red;
				_objectList[1].TextureColor = Color.Red;
			}
			else if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				_objectList[0].TextureColor = Color.Yellow;
				_objectList[2].TextureColor = Color.LightYellow;
				_objectList[2].Opacity = 0.8f;
				(_objectList[1] as SpriteObj).OutlineColour = Color.Pink;
				_objectList[1].TextureColor = Color.Pink;
			}
			else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				_objectList[0].TextureColor = Color.Red;
				_objectList[2].TextureColor = Color.Pink;
				_objectList[2].Opacity = 0.8f;
				(_objectList[1] as SpriteObj).OutlineColour = Color.Yellow;
				_objectList[1].TextureColor = Color.Yellow;
			}
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				m_resetSpriteName = "EnemyBlobBossIdle_Character";
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new MoveLogicAction(m_target, true));
			logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Move01", "Blob_Move02", "Blob_Move03", "Blank", "Blank", "Blank", "Blank", "Blank"));
			logicSet.AddAction(new DelayLogicAction(1.1f, 1.9f));
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveLogicAction(m_target, false));
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Move01", "Blob_Move02", "Blob_Move03", "Blank", "Blank", "Blank", "Blank", "Blank"));
			logicSet2.AddAction(new DelayLogicAction(1f, 1.5f));
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet3.AddAction(new DelayLogicAction(0.5f, 0.9f));
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet4.AddAction(new GroundCheckLogicAction());
			logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump"));
			logicSet4.AddAction(new DelayLogicAction(JumpDelay));
			logicSet4.AddAction(new MoveLogicAction(m_target, true, Speed * 6.75f));
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Jump"));
			logicSet4.AddAction(new JumpLogicAction());
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character"));
			logicSet4.AddAction(new DelayLogicAction(0.2f));
			logicSet4.AddAction(new GroundCheckLogicAction());
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Land"));
			logicSet4.AddAction(new MoveLogicAction(m_target, true, Speed));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Jump"));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character"));
			logicSet4.AddAction(new DelayLogicAction(0.2f));
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet4.Tag = 2;
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "SpellDamageShield_Sprite",
				SourceAnchor = new Vector2(0f, 10f),
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				Angle = new Vector2(0f, 0f),
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = ProjectileScale,
				Lifespan = ExpertBlobProjectileDuration,
				LockPosition = true
			};
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet5.AddAction(new GroundCheckLogicAction());
			logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump"));
			logicSet5.AddAction(new DelayLogicAction(JumpDelay));
			logicSet5.AddAction(new MoveLogicAction(m_target, true, Speed * 6.75f));
			logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Jump"));
			logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
			logicSet5.AddAction(new JumpLogicAction());
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character"));
			logicSet5.AddAction(new DelayLogicAction(0.2f));
			logicSet5.AddAction(new GroundCheckLogicAction());
			logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Land"));
			logicSet5.AddAction(new MoveLogicAction(m_target, true, Speed));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Jump"));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character"));
			logicSet5.AddAction(new DelayLogicAction(0.2f));
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet6.AddAction(new GroundCheckLogicAction());
			logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump"));
			logicSet6.AddAction(new DelayLogicAction(JumpDelay));
			logicSet6.AddAction(new MoveLogicAction(m_target, true, Speed * 6.75f));
			logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Jump"));
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
			logicSet6.AddAction(new JumpLogicAction());
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character"));
			logicSet6.AddAction(new DelayLogicAction(0.2f));
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
			logicSet6.AddAction(new DelayLogicAction(0.2f));
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
			logicSet6.AddAction(new DelayLogicAction(0.2f));
			logicSet6.AddAction(new GroundCheckLogicAction());
			logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Land"));
			logicSet6.AddAction(new MoveLogicAction(m_target, true, Speed));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false));
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Jump"));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character"));
			logicSet6.AddAction(new DelayLogicAction(0.2f));
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new MoveLogicAction(m_target, true));
			logicSet7.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Move01", "Blob_Move02", "Blob_Move03", "Blank", "Blank", "Blank", "Blank", "Blank"));
			logicSet7.AddAction(new DelayLogicAction(1.1f, 1.9f));
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new MoveLogicAction(m_target, false));
			logicSet8.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Move01", "Blob_Move02", "Blob_Move03", "Blank", "Blank", "Blank", "Blank", "Blank"));
			logicSet8.AddAction(new DelayLogicAction(1f, 1.5f));
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet9.AddAction(new DelayLogicAction(0.5f, 0.9f));
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet10.AddAction(new GroundCheckLogicAction());
			logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false));
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump"));
			logicSet10.AddAction(new DelayLogicAction(JumpDelay));
			logicSet10.AddAction(new MoveLogicAction(m_target, true, Speed * 6.75f));
			logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Jump"));
			logicSet10.AddAction(new JumpLogicAction());
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character"));
			logicSet10.AddAction(new DelayLogicAction(0.2f));
			logicSet10.AddAction(new GroundCheckLogicAction());
			logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, "Blob_Land"));
			logicSet10.AddAction(new MoveLogicAction(m_target, true, Speed));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false));
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Jump"));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossIdle_Character"));
			logicSet10.AddAction(new DelayLogicAction(0.2f));
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet10.Tag = 2;
			LogicSet logicSet11 = new LogicSet(this);
			logicSet11.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character"));
			logicSet11.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyMove1", "FairyMove2", "FairyMove3"));
			logicSet11.AddAction(new ChaseLogicAction(m_target, true, 1f));
			m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4);
			m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5);
			m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet6);
			m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
			m_generalMiniBossLB.AddLogicSet(logicSet7, logicSet8, logicSet9, logicSet10);
			m_generalNeoLB.AddLogicSet(logicSet11);
			m_generalBossCooldownLB.AddLogicSet(logicSet7, logicSet8, logicSet9);
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			logicBlocksToDispose.Add(m_generalNeoLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			logicBlocksToDispose.Add(m_generalBossCooldownLB);
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				SetCooldownLogicBlock(m_generalBossCooldownLB, 40, 40, 20);
				ChangeSprite("EnemyBlobBossIdle_Character");
			}
			else
			{
				SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20);
			}
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			if (m_isTouchingGround)
			{
				switch (State)
				{
				case 0:
					RunLogicBlock(true, m_generalBasicLB, 10, 10, 75, 5);
					break;
				case 1:
				case 2:
				case 3:
					RunLogicBlock(true, m_generalBasicLB, 45, 0, 0, 55);
					return;
				default:
					return;
				}
			}
		}
		protected override void RunAdvancedLogic()
		{
			if (m_isTouchingGround)
			{
				switch (State)
				{
				case 0:
					RunLogicBlock(true, m_generalAdvancedLB, 10, 10, 75, 5);
					break;
				case 1:
				case 2:
				case 3:
					RunLogicBlock(true, m_generalAdvancedLB, 45, 0, 0, 55);
					return;
				default:
					return;
				}
			}
		}
		protected override void RunExpertLogic()
		{
			if (m_isTouchingGround)
			{
				switch (State)
				{
				case 0:
					RunLogicBlock(true, m_generalExpertLB, 10, 10, 75, 5);
					break;
				case 1:
				case 2:
				case 3:
					RunLogicBlock(true, m_generalExpertLB, 45, 0, 0, 55);
					return;
				default:
					return;
				}
			}
		}
		protected override void RunMinibossLogic()
		{
			if (m_isTouchingGround)
			{
				switch (State)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					if (!IsNeo)
					{
						RunLogicBlock(true, m_generalMiniBossLB, 45, 0, 0, 55);
						return;
					}
					break;
				default:
					return;
				}
			}
			else if (IsNeo)
			{
				AnimationDelay = 0.1f;
				RunLogicBlock(true, m_generalNeoLB, 100);
			}
		}
		public EnemyObj_Blob(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyBlobIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			MainBlob = true;
			TintablePart = _objectList[0];
			PlayAnimation();
			m_invincibleCounter = 0.5f;
			Type = 2;
		}
		public void SetNumberOfHits(int hits)
		{
			NumHits = hits;
		}
		private void CreateBlob(GameTypes.EnemyDifficulty difficulty, int numHits, bool isNeo = false)
		{
			EnemyObj_Blob enemyObj_Blob = new EnemyObj_Blob(null, null, null, difficulty);
			enemyObj_Blob.InitializeEV();
			enemyObj_Blob.Position = Position;
			if (m_target.X < enemyObj_Blob.X)
			{
				enemyObj_Blob.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_Blob.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_Blob.Level = Level;
			m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Blob);
			enemyObj_Blob.Scale = new Vector2(ScaleX * BlobSizeChange.X, ScaleY * BlobSizeChange.Y);
			enemyObj_Blob.SetNumberOfHits(numHits);
			enemyObj_Blob.Speed *= BlobSpeedChange;
			enemyObj_Blob.MainBlob = false;
			enemyObj_Blob.SavedStartingPos = enemyObj_Blob.Position;
			enemyObj_Blob.IsNeo = isNeo;
			if (isNeo)
			{
				enemyObj_Blob.Name = Name;
				enemyObj_Blob.IsWeighted = false;
				enemyObj_Blob.TurnSpeed = TurnSpeed;
				enemyObj_Blob.Speed = Speed * BlobSpeedChange;
				enemyObj_Blob.Level = Level;
				enemyObj_Blob.MaxHealth = MaxHealth;
				enemyObj_Blob.CurrentHealth = enemyObj_Blob.MaxHealth;
				enemyObj_Blob.Damage = Damage;
				enemyObj_Blob.ChangeNeoStats(BlobSizeChange.X, BlobSpeedChange, numHits);
			}
			int num = CDGMath.RandomInt(-500, -300);
			int num2 = CDGMath.RandomInt(300, 700);
			if (enemyObj_Blob.X < m_target.X)
			{
				enemyObj_Blob.AccelerationX += -(m_target.EnemyKnockBack.X + num);
			}
			else
			{
				enemyObj_Blob.AccelerationX += m_target.EnemyKnockBack.X + num;
			}
			enemyObj_Blob.AccelerationY += -(m_target.EnemyKnockBack.Y + num2);
			if (enemyObj_Blob.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				for (int i = 0; i < NumChildren; i++)
				{
					enemyObj_Blob.GetChildAt(i).Opacity = GetChildAt(i).Opacity;
					enemyObj_Blob.GetChildAt(i).TextureColor = GetChildAt(i).TextureColor;
				}
				enemyObj_Blob.ChangeSprite("EnemyBlobBossAir_Character");
			}
			else
			{
				enemyObj_Blob.ChangeSprite("EnemyBlobAir_Character");
			}
			enemyObj_Blob.PlayAnimation();
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_Blob.InitializeDebugRadii();
			}
			enemyObj_Blob.SaveToFile = false;
			enemyObj_Blob.SpawnRoom = m_levelScreen.CurrentRoom;
			enemyObj_Blob.GivesLichHealth = false;
		}
		public void CreateWizard()
		{
			EnemyObj_EarthWizard enemyObj_EarthWizard = new EnemyObj_EarthWizard(null, null, null, GameTypes.EnemyDifficulty.ADVANCED);
			enemyObj_EarthWizard.PublicInitializeEV();
			enemyObj_EarthWizard.Position = Position;
			if (m_target.X < enemyObj_EarthWizard.X)
			{
				enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_EarthWizard.Level = Level;
			enemyObj_EarthWizard.Level -= m_bossEarthWizardLevelReduction;
			m_levelScreen.AddEnemyToCurrentRoom(enemyObj_EarthWizard);
			enemyObj_EarthWizard.SavedStartingPos = enemyObj_EarthWizard.Position;
			int num = CDGMath.RandomInt(-500, -300);
			int num2 = CDGMath.RandomInt(300, 700);
			if (enemyObj_EarthWizard.X < m_target.X)
			{
				enemyObj_EarthWizard.AccelerationX += -(m_target.EnemyKnockBack.X + num);
			}
			else
			{
				enemyObj_EarthWizard.AccelerationX += m_target.EnemyKnockBack.X + num;
			}
			enemyObj_EarthWizard.AccelerationY += -(m_target.EnemyKnockBack.Y + num2);
			enemyObj_EarthWizard.PlayAnimation();
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_EarthWizard.InitializeDebugRadii();
			}
			enemyObj_EarthWizard.SaveToFile = false;
			enemyObj_EarthWizard.SpawnRoom = m_levelScreen.CurrentRoom;
			enemyObj_EarthWizard.GivesLichHealth = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				if (!m_isTouchingGround && IsWeighted && CurrentSpeed == 0f && SpriteName == "EnemyBlobBossAir_Character")
				{
					CurrentSpeed = Speed;
				}
				if (!m_currentActiveLB.IsActive && m_isTouchingGround && SpriteName != "EnemyBlobBossIdle_Character")
				{
					ChangeSprite("EnemyBlobBossIdle_Character");
					PlayAnimation();
				}
			}
			else
			{
				if (!m_isTouchingGround && IsWeighted && CurrentSpeed == 0f && SpriteName == "EnemyBlobAir_Character")
				{
					CurrentSpeed = Speed;
				}
				if (!m_currentActiveLB.IsActive && m_isTouchingGround && SpriteName != "EnemyBlobIdle_Character")
				{
					ChangeSprite("EnemyBlobIdle_Character");
					PlayAnimation();
				}
			}
			if (IsNeo)
			{
				foreach (EnemyObj current in m_levelScreen.CurrentRoom.EnemyList)
				{
					if (current != this && current is EnemyObj_Blob)
					{
						float num = Vector2.Distance(Position, current.Position);
						if (num < 150f)
						{
							Vector2 facePosition = 2f * Position - current.Position;
							CDGMath.TurnToFace(this, facePosition);
						}
					}
				}
				foreach (EnemyObj current2 in m_levelScreen.CurrentRoom.TempEnemyList)
				{
					if (current2 != this && current2 is EnemyObj_Blob)
					{
						float num2 = Vector2.Distance(Position, current2.Position);
						if (num2 < 150f)
						{
							Vector2 facePosition2 = 2f * Position - current2.Position;
							CDGMath.TurnToFace(this, facePosition2);
						}
					}
				}
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true)
		{
			if (m_target != null && m_target.CurrentHealth > 0 && !m_bossVersionKilled)
			{
				base.HitEnemy(damage, position, isPlayer);
				if (CurrentHealth <= 0)
				{
					CurrentHealth = MaxHealth;
					NumHits--;
					if (!IsKilled && NumHits > 0)
					{
						if (!IsNeo)
						{
							if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
							{
								m_flipTween.StopTween(false);
							}
							ScaleX = ScaleY;
							CreateBlob(Difficulty, NumHits);
							Scale = new Vector2(ScaleX * BlobSizeChange.X, ScaleY * BlobSizeChange.Y);
							Speed *= BlobSpeedChange;
							if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
							{
								CreateWizard();
							}
						}
						else
						{
							if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
							{
								m_flipTween.StopTween(false);
							}
							ScaleX = ScaleY;
							CreateBlob(Difficulty, NumHits, true);
							Scale = new Vector2(ScaleX * BlobSizeChange.X, ScaleY * BlobSizeChange.Y);
							Speed *= BlobSpeedChange;
						}
					}
				}
				if (NumHits <= 0)
				{
					Kill();
				}
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !m_bossVersionKilled)
			{
				PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
				if (playerObj != null && otherBox.Type == 1 && !playerObj.IsInvincible && playerObj.State == 8)
				{
					playerObj.HitPlayer(this);
				}
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public override void Kill(bool giveXP = true)
		{
			if (Difficulty != GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.Kill(giveXP);
				return;
			}
			if (m_target.CurrentHealth > 0)
			{
				BlobBossRoom blobBossRoom = m_levelScreen.CurrentRoom as BlobBossRoom;
				BlobChallengeRoom blobChallengeRoom = m_levelScreen.CurrentRoom as BlobChallengeRoom;
				if (((blobBossRoom != null && blobBossRoom.NumActiveBlobs == 1) || (blobChallengeRoom != null && blobChallengeRoom.NumActiveBlobs == 1)) && !m_bossVersionKilled)
				{
					Game.PlayerStats.BlobBossBeaten = true;
					SoundManager.StopMusic();
					m_bossVersionKilled = true;
					m_target.LockControls();
					m_levelScreen.PauseScreen();
					m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
					m_levelScreen.RunWhiteSlashEffect();
					Tween.RunFunction(1f, this, "Part2");
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Eyeball_Freeze");
					GameUtil.UnlockAchievement("FEAR_OF_SLIME");
					if (IsNeo)
					{
						Tween.To(m_target.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "Zoom", "1", "X", m_target.X.ToString(), "Y", m_target.Y.ToString());
						Tween.AddEndHandlerToLastTween(this, "LockCamera");
						return;
					}
				}
				else
				{
					base.Kill(giveXP);
				}
			}
		}
		public void LockCamera()
		{
			m_target.AttachedLevel.CameraLockedToPlayer = true;
		}
		public void Part2()
		{
			m_levelScreen.UnpauseScreen();
			m_target.UnlockControls();
			if (m_currentActiveLB != null)
			{
				m_currentActiveLB.StopLogicBlock();
			}
			m_target.CurrentSpeed = 0f;
			m_target.ForceInvincible = true;
			foreach (EnemyObj current in m_levelScreen.CurrentRoom.TempEnemyList)
			{
				if (!current.IsKilled)
				{
					current.Kill();
				}
			}
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Blob_Death");
			base.Kill();
			if (!IsNeo)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < m_bossCoins; i++)
				{
					list.Add(0);
				}
				for (int j = 0; j < m_bossMoneyBags; j++)
				{
					list.Add(1);
				}
				for (int k = 0; k < m_bossDiamonds; k++)
				{
					list.Add(2);
				}
				CDGMath.Shuffle(list);
				float num = 0f;
				for (int l = 0; l < list.Count; l++)
				{
					Vector2 position = Position;
					if (list[l] == 0)
					{
						Tween.RunFunction(l * num, m_levelScreen.ItemDropManager, "DropItemWide", position, 1, 10);
					}
					else if (list[l] == 1)
					{
						Tween.RunFunction(l * num, m_levelScreen.ItemDropManager, "DropItemWide", position, 10, 100);
					}
					else
					{
						Tween.RunFunction(l * num, m_levelScreen.ItemDropManager, "DropItemWide", position, 11, 500);
					}
				}
			}
		}
		public override void Reset()
		{
			if (!MainBlob)
			{
				m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
				Dispose();
				return;
			}
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				Speed = 50f;
				Scale = EnemyEV.Blob_Basic_Scale;
				NumHits = 2;
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				Speed = 80f;
				Scale = EnemyEV.Blob_Advanced_Scale;
				NumHits = 3;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Speed = 90f;
				Scale = EnemyEV.Blob_Expert_Scale;
				NumHits = 4;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Speed = 110f;
				NumHits = 6;
				break;
			}
			base.Reset();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				SpawnRoom = null;
				base.Dispose();
			}
		}
		public void ChangeNeoStats(float blobSizeChange, float blobSpeedChange, int numHits)
		{
			NumHits = numHits;
			BlobSizeChange = new Vector2(blobSizeChange, blobSizeChange);
			BlobSpeedChange = blobSpeedChange;
		}
	}
}
