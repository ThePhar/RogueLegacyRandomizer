using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
				return this.m_isNeo;
			}
			set
			{
				this.m_isNeo = value;
				if (value)
				{
					this.HealthGainPerLevel = 0;
					this.DamageGainPerLevel = 0;
					this.MoneyDropChance = 0f;
					this.ItemDropChance = 0f;
					this.m_saveToEnemiesKilledList = false;
				}
			}
		}
		protected override void InitializeEV()
		{
			this.SetNumberOfHits(2);
			this.BlobSizeChange = new Vector2(0.725f, 0.725f);
			this.BlobSpeedChange = 2f;
			base.Name = "Bloob";
			this.MaxHealth = 14;
			base.Damage = 13;
			base.XPValue = 25;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.225f;
			base.Speed = 50f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 0f;
			base.JumpHeight = 975f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.05f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = true;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Blob_Basic_Scale;
			base.ProjectileScale = EnemyEV.Blob_Basic_ProjectileScale;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 500;
			this.EngageRadius = 750;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Blob_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.SetNumberOfHits(3);
				this.BlobSizeChange = new Vector2(0.6f, 0.6f);
				this.BlobSpeedChange = 2.25f;
				base.Name = "Bloobite";
				this.MaxHealth = 18;
				base.Damage = 14;
				base.XPValue = 29;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 1;
				this.MoneyDropChance = 0.2f;
				base.Speed = 80f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 975f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Blob_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Blob_Advanced_ProjectileScale;
				this.MeleeRadius = 225;
				this.EngageRadius = 750;
				this.ProjectileRadius = 500;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Blob_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.SetNumberOfHits(4);
				this.BlobSizeChange = new Vector2(0.65f, 0.65f);
				this.BlobSpeedChange = 2.25f;
				base.Name = "Bloobasaurus Rex";
				this.MaxHealth = 22;
				base.Damage = 16;
				base.XPValue = 35;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 1;
				this.MoneyDropChance = 0.1f;
				base.Speed = 90f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 975f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Blob_Expert_Scale;
				base.ProjectileScale = EnemyEV.Blob_Expert_ProjectileScale;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 500;
				this.EngageRadius = 750;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Blob_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				this.SetNumberOfHits(5);
				this.BlobSizeChange = new Vector2(0.6f, 0.6f);
				this.BlobSpeedChange = 2f;
				base.ForceDraw = true;
				base.Name = "Herodotus";
				this.MaxHealth = 32;
				base.Damage = 16;
				base.XPValue = 70;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 0.1f;
				base.Speed = 110f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 975f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Blob_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Blob_Miniboss_ProjectileScale;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 500;
				this.EngageRadius = 750;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Blob_Miniboss_KnockBack;
				if (LevelEV.WEAKEN_BOSSES)
				{
					this.MaxHealth = 1;
					this.SetNumberOfHits(1);
				}
				break;
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.BASIC)
			{
				this._objectList[0].TextureColor = Color.Green;
				this._objectList[2].TextureColor = Color.LightGreen;
				this._objectList[2].Opacity = 0.8f;
				(this._objectList[1] as SpriteObj).OutlineColour = Color.Red;
				this._objectList[1].TextureColor = Color.Red;
			}
			else if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				this._objectList[0].TextureColor = Color.Yellow;
				this._objectList[2].TextureColor = Color.LightYellow;
				this._objectList[2].Opacity = 0.8f;
				(this._objectList[1] as SpriteObj).OutlineColour = Color.Pink;
				this._objectList[1].TextureColor = Color.Pink;
			}
			else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				this._objectList[0].TextureColor = Color.Red;
				this._objectList[2].TextureColor = Color.Pink;
				this._objectList[2].Opacity = 0.8f;
				(this._objectList[1] as SpriteObj).OutlineColour = Color.Yellow;
				this._objectList[1].TextureColor = Color.Yellow;
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				this.m_resetSpriteName = "EnemyBlobBossIdle_Character";
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Move01",
				"Blob_Move02",
				"Blob_Move03",
				"Blank",
				"Blank",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1.1f, 1.9f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Move01",
				"Blob_Move02",
				"Blob_Move03",
				"Blank",
				"Blank",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, 0.9f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.JumpDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Jump"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Land"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, base.Speed), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "SpellDamageShield_Sprite",
				SourceAnchor = new Vector2(0f, 10f),
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				Angle = new Vector2(0f, 0f),
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale,
				Lifespan = this.ExpertBlobProjectileDuration,
				LockPosition = true
			};
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.JumpDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Jump"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet5.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Land"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, base.Speed), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.JumpDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Jump"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet6.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobAir_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Land"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, base.Speed), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobJump_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyBlobIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Move01",
				"Blob_Move02",
				"Blob_Move03",
				"Blank",
				"Blank",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(1.1f, 1.9f, false), Types.Sequence.Serial);
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet8.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Move01",
				"Blob_Move02",
				"Blob_Move03",
				"Blank",
				"Blank",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.5f, 0.9f, false), Types.Sequence.Serial);
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet10.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "BeforeJump", false), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(this.JumpDelay, false), Types.Sequence.Serial);
			logicSet10.AddAction(new MoveLogicAction(this.m_target, true, base.Speed * 6.75f), Types.Sequence.Serial);
			logicSet10.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Jump"
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet10.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Blob_Land"
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new MoveLogicAction(this.m_target, true, base.Speed), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossJump_Character", false, false), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Jump", false), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet10.Tag = 2;
			LogicSet logicSet11 = new LogicSet(this);
			logicSet11.AddAction(new ChangeSpriteLogicAction("EnemyBlobBossAir_Character", true, true), Types.Sequence.Serial);
			logicSet11.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyMove1",
				"FairyMove2",
				"FairyMove3"
			}), Types.Sequence.Serial);
			logicSet11.AddAction(new ChaseLogicAction(this.m_target, true, 1f, -1f), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet5
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet6
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet8,
				logicSet9,
				logicSet10
			});
			this.m_generalNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet11
			});
			this.m_generalBossCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet8,
				logicSet9
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			this.logicBlocksToDispose.Add(this.m_generalNeoLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalBossCooldownLB);
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.SetCooldownLogicBlock(this.m_generalBossCooldownLB, new int[]
				{
					40,
					40,
					20
				});
				this.ChangeSprite("EnemyBlobBossIdle_Character");
			}
			else
			{
				base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
				{
					40,
					40,
					20
				});
			}
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			if (this.m_isTouchingGround)
			{
				switch (base.State)
				{
				case 0:
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						10,
						10,
						75,
						5
					});
					break;
				case 1:
				case 2:
				case 3:
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						45,
						0,
						0,
						55
					});
					return;
				default:
					return;
				}
			}
		}
		protected override void RunAdvancedLogic()
		{
			if (this.m_isTouchingGround)
			{
				switch (base.State)
				{
				case 0:
					base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
					{
						10,
						10,
						75,
						5
					});
					break;
				case 1:
				case 2:
				case 3:
					base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
					{
						45,
						0,
						0,
						55
					});
					return;
				default:
					return;
				}
			}
		}
		protected override void RunExpertLogic()
		{
			if (this.m_isTouchingGround)
			{
				switch (base.State)
				{
				case 0:
					base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
					{
						10,
						10,
						75,
						5
					});
					break;
				case 1:
				case 2:
				case 3:
					base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
					{
						45,
						0,
						0,
						55
					});
					return;
				default:
					return;
				}
			}
		}
		protected override void RunMinibossLogic()
		{
			if (this.m_isTouchingGround)
			{
				switch (base.State)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					if (!this.IsNeo)
					{
						base.RunLogicBlock(true, this.m_generalMiniBossLB, new int[]
						{
							45,
							0,
							0,
							55
						});
						return;
					}
					break;
				default:
					return;
				}
			}
			else if (this.IsNeo)
			{
				base.AnimationDelay = 0.1f;
				base.RunLogicBlock(true, this.m_generalNeoLB, new int[]
				{
					100
				});
			}
		}
		public EnemyObj_Blob(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyBlobIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.MainBlob = true;
			this.TintablePart = this._objectList[0];
			base.PlayAnimation(true);
			this.m_invincibleCounter = 0.5f;
			this.Type = 2;
		}
		public void SetNumberOfHits(int hits)
		{
			this.NumHits = hits;
		}
		private void CreateBlob(GameTypes.EnemyDifficulty difficulty, int numHits, bool isNeo = false)
		{
			EnemyObj_Blob enemyObj_Blob = new EnemyObj_Blob(null, null, null, difficulty);
			enemyObj_Blob.InitializeEV();
			enemyObj_Blob.Position = base.Position;
			if (this.m_target.X < enemyObj_Blob.X)
			{
				enemyObj_Blob.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_Blob.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_Blob.Level = base.Level;
			this.m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Blob);
			enemyObj_Blob.Scale = new Vector2(this.ScaleX * this.BlobSizeChange.X, this.ScaleY * this.BlobSizeChange.Y);
			enemyObj_Blob.SetNumberOfHits(numHits);
			enemyObj_Blob.Speed *= this.BlobSpeedChange;
			enemyObj_Blob.MainBlob = false;
			enemyObj_Blob.SavedStartingPos = enemyObj_Blob.Position;
			enemyObj_Blob.IsNeo = isNeo;
			if (isNeo)
			{
				enemyObj_Blob.Name = base.Name;
				enemyObj_Blob.IsWeighted = false;
				enemyObj_Blob.TurnSpeed = this.TurnSpeed;
				enemyObj_Blob.Speed = base.Speed * this.BlobSpeedChange;
				enemyObj_Blob.Level = base.Level;
				enemyObj_Blob.MaxHealth = this.MaxHealth;
				enemyObj_Blob.CurrentHealth = enemyObj_Blob.MaxHealth;
				enemyObj_Blob.Damage = base.Damage;
				enemyObj_Blob.ChangeNeoStats(this.BlobSizeChange.X, this.BlobSpeedChange, numHits);
			}
			int num = CDGMath.RandomInt(-500, -300);
			int num2 = CDGMath.RandomInt(300, 700);
			if (enemyObj_Blob.X < this.m_target.X)
			{
				enemyObj_Blob.AccelerationX += -(this.m_target.EnemyKnockBack.X + (float)num);
			}
			else
			{
				enemyObj_Blob.AccelerationX += this.m_target.EnemyKnockBack.X + (float)num;
			}
			enemyObj_Blob.AccelerationY += -(this.m_target.EnemyKnockBack.Y + (float)num2);
			if (enemyObj_Blob.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				for (int i = 0; i < base.NumChildren; i++)
				{
					enemyObj_Blob.GetChildAt(i).Opacity = base.GetChildAt(i).Opacity;
					enemyObj_Blob.GetChildAt(i).TextureColor = base.GetChildAt(i).TextureColor;
				}
				enemyObj_Blob.ChangeSprite("EnemyBlobBossAir_Character");
			}
			else
			{
				enemyObj_Blob.ChangeSprite("EnemyBlobAir_Character");
			}
			enemyObj_Blob.PlayAnimation(true);
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_Blob.InitializeDebugRadii();
			}
			enemyObj_Blob.SaveToFile = false;
			enemyObj_Blob.SpawnRoom = this.m_levelScreen.CurrentRoom;
			enemyObj_Blob.GivesLichHealth = false;
		}
		public void CreateWizard()
		{
			EnemyObj_EarthWizard enemyObj_EarthWizard = new EnemyObj_EarthWizard(null, null, null, GameTypes.EnemyDifficulty.ADVANCED);
			enemyObj_EarthWizard.PublicInitializeEV();
			enemyObj_EarthWizard.Position = base.Position;
			if (this.m_target.X < enemyObj_EarthWizard.X)
			{
				enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_EarthWizard.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_EarthWizard.Level = base.Level;
			enemyObj_EarthWizard.Level -= this.m_bossEarthWizardLevelReduction;
			this.m_levelScreen.AddEnemyToCurrentRoom(enemyObj_EarthWizard);
			enemyObj_EarthWizard.SavedStartingPos = enemyObj_EarthWizard.Position;
			int num = CDGMath.RandomInt(-500, -300);
			int num2 = CDGMath.RandomInt(300, 700);
			if (enemyObj_EarthWizard.X < this.m_target.X)
			{
				enemyObj_EarthWizard.AccelerationX += -(this.m_target.EnemyKnockBack.X + (float)num);
			}
			else
			{
				enemyObj_EarthWizard.AccelerationX += this.m_target.EnemyKnockBack.X + (float)num;
			}
			enemyObj_EarthWizard.AccelerationY += -(this.m_target.EnemyKnockBack.Y + (float)num2);
			enemyObj_EarthWizard.PlayAnimation(true);
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_EarthWizard.InitializeDebugRadii();
			}
			enemyObj_EarthWizard.SaveToFile = false;
			enemyObj_EarthWizard.SpawnRoom = this.m_levelScreen.CurrentRoom;
			enemyObj_EarthWizard.GivesLichHealth = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				if (!this.m_isTouchingGround && base.IsWeighted && base.CurrentSpeed == 0f && base.SpriteName == "EnemyBlobBossAir_Character")
				{
					base.CurrentSpeed = base.Speed;
				}
				if (!this.m_currentActiveLB.IsActive && this.m_isTouchingGround && base.SpriteName != "EnemyBlobBossIdle_Character")
				{
					this.ChangeSprite("EnemyBlobBossIdle_Character");
					base.PlayAnimation(true);
				}
			}
			else
			{
				if (!this.m_isTouchingGround && base.IsWeighted && base.CurrentSpeed == 0f && base.SpriteName == "EnemyBlobAir_Character")
				{
					base.CurrentSpeed = base.Speed;
				}
				if (!this.m_currentActiveLB.IsActive && this.m_isTouchingGround && base.SpriteName != "EnemyBlobIdle_Character")
				{
					this.ChangeSprite("EnemyBlobIdle_Character");
					base.PlayAnimation(true);
				}
			}
			if (this.IsNeo)
			{
				foreach (EnemyObj current in this.m_levelScreen.CurrentRoom.EnemyList)
				{
					if (current != this && current is EnemyObj_Blob)
					{
						float num = Vector2.Distance(base.Position, current.Position);
						if (num < 150f)
						{
							Vector2 facePosition = 2f * base.Position - current.Position;
							CDGMath.TurnToFace(this, facePosition);
						}
					}
				}
				foreach (EnemyObj current2 in this.m_levelScreen.CurrentRoom.TempEnemyList)
				{
					if (current2 != this && current2 is EnemyObj_Blob)
					{
						float num2 = Vector2.Distance(base.Position, current2.Position);
						if (num2 < 150f)
						{
							Vector2 facePosition2 = 2f * base.Position - current2.Position;
							CDGMath.TurnToFace(this, facePosition2);
						}
					}
				}
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true)
		{
			if (this.m_target != null && this.m_target.CurrentHealth > 0 && !this.m_bossVersionKilled)
			{
				base.HitEnemy(damage, position, isPlayer);
				if (base.CurrentHealth <= 0)
				{
					base.CurrentHealth = this.MaxHealth;
					this.NumHits--;
					if (!base.IsKilled && this.NumHits > 0)
					{
						if (!this.IsNeo)
						{
							if (this.m_flipTween != null && this.m_flipTween.TweenedObject == this && this.m_flipTween.Active)
							{
								this.m_flipTween.StopTween(false);
							}
							this.ScaleX = this.ScaleY;
							this.CreateBlob(base.Difficulty, this.NumHits, false);
							this.Scale = new Vector2(this.ScaleX * this.BlobSizeChange.X, this.ScaleY * this.BlobSizeChange.Y);
							base.Speed *= this.BlobSpeedChange;
							if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
							{
								this.CreateWizard();
							}
						}
						else
						{
							if (this.m_flipTween != null && this.m_flipTween.TweenedObject == this && this.m_flipTween.Active)
							{
								this.m_flipTween.StopTween(false);
							}
							this.ScaleX = this.ScaleY;
							this.CreateBlob(base.Difficulty, this.NumHits, true);
							this.Scale = new Vector2(this.ScaleX * this.BlobSizeChange.X, this.ScaleY * this.BlobSizeChange.Y);
							base.Speed *= this.BlobSpeedChange;
						}
					}
				}
				if (this.NumHits <= 0)
				{
					this.Kill(true);
				}
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !this.m_bossVersionKilled)
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
			if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.Kill(giveXP);
				return;
			}
			if (this.m_target.CurrentHealth > 0)
			{
				BlobBossRoom blobBossRoom = this.m_levelScreen.CurrentRoom as BlobBossRoom;
				BlobChallengeRoom blobChallengeRoom = this.m_levelScreen.CurrentRoom as BlobChallengeRoom;
				if (((blobBossRoom != null && blobBossRoom.NumActiveBlobs == 1) || (blobChallengeRoom != null && blobChallengeRoom.NumActiveBlobs == 1)) && !this.m_bossVersionKilled)
				{
					Game.PlayerStats.BlobBossBeaten = true;
					SoundManager.StopMusic(0f);
					this.m_bossVersionKilled = true;
					this.m_target.LockControls();
					this.m_levelScreen.PauseScreen();
					this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
					this.m_levelScreen.RunWhiteSlashEffect();
					Tween.RunFunction(1f, this, "Part2", new object[0]);
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Eyeball_Freeze");
					GameUtil.UnlockAchievement("FEAR_OF_SLIME");
					if (this.IsNeo)
					{
						Tween.To(this.m_target.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
						{
							"Zoom",
							"1",
							"X",
							this.m_target.X.ToString(),
							"Y",
							this.m_target.Y.ToString()
						});
						Tween.AddEndHandlerToLastTween(this, "LockCamera", new object[0]);
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
			this.m_target.AttachedLevel.CameraLockedToPlayer = true;
		}
		public void Part2()
		{
			this.m_levelScreen.UnpauseScreen();
			this.m_target.UnlockControls();
			if (this.m_currentActiveLB != null)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			this.m_target.CurrentSpeed = 0f;
			this.m_target.ForceInvincible = true;
			foreach (EnemyObj current in this.m_levelScreen.CurrentRoom.TempEnemyList)
			{
				if (!current.IsKilled)
				{
					current.Kill(true);
				}
			}
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Blob_Death");
			base.Kill(true);
			if (!this.IsNeo)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < this.m_bossCoins; i++)
				{
					list.Add(0);
				}
				for (int j = 0; j < this.m_bossMoneyBags; j++)
				{
					list.Add(1);
				}
				for (int k = 0; k < this.m_bossDiamonds; k++)
				{
					list.Add(2);
				}
				CDGMath.Shuffle<int>(list);
				float num = 0f;
				for (int l = 0; l < list.Count; l++)
				{
					Vector2 position = base.Position;
					if (list[l] == 0)
					{
						Tween.RunFunction((float)l * num, this.m_levelScreen.ItemDropManager, "DropItemWide", new object[]
						{
							position,
							1,
							10
						});
					}
					else if (list[l] == 1)
					{
						Tween.RunFunction((float)l * num, this.m_levelScreen.ItemDropManager, "DropItemWide", new object[]
						{
							position,
							10,
							100
						});
					}
					else
					{
						Tween.RunFunction((float)l * num, this.m_levelScreen.ItemDropManager, "DropItemWide", new object[]
						{
							position,
							11,
							500
						});
					}
				}
			}
		}
		public override void Reset()
		{
			if (!this.MainBlob)
			{
				this.m_levelScreen.RemoveEnemyFromRoom(this, this.SpawnRoom, this.SavedStartingPos);
				this.Dispose();
				return;
			}
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				base.Speed = 50f;
				this.Scale = EnemyEV.Blob_Basic_Scale;
				this.NumHits = 2;
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Speed = 80f;
				this.Scale = EnemyEV.Blob_Advanced_Scale;
				this.NumHits = 3;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Speed = 90f;
				this.Scale = EnemyEV.Blob_Expert_Scale;
				this.NumHits = 4;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Speed = 110f;
				this.NumHits = 6;
				break;
			}
			base.Reset();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.SpawnRoom = null;
				base.Dispose();
			}
		}
		public void ChangeNeoStats(float blobSizeChange, float blobSpeedChange, int numHits)
		{
			this.NumHits = numHits;
			this.BlobSizeChange = new Vector2(blobSizeChange, blobSizeChange);
			this.BlobSpeedChange = blobSpeedChange;
		}
	}
}
