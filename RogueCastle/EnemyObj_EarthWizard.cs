using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_EarthWizard : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private float SpellDelay = 0.3f;
		private float SpellDuration = 0.75f;
		private int SpellIceProjectileCount = 24;
		private float SpellFireDelay = 1.5f;
		private float SpellFireInterval = 0.2f;
		private Vector2 MiniBossFireballSize = new Vector2(2f, 2f);
		private Vector2 MiniBossIceSize = new Vector2(1.5f, 1.5f);
		private ProjectileObj m_fireballSummon;
		private ProjectileObj m_iceballSummon;
		private SpriteObj m_earthSummonInSprite;
		private SpriteObj m_earthSummonOutSprite;
		private ProjectileObj m_earthProjectileObj;
		public RoomObj SpawnRoom;
		private Vector2 m_spellOffset = new Vector2(40f, -80f);
		private float TeleportDelay = 0.5f;
		private float TeleportDuration = 1f;
		private float MoveDuration = 1f;
		private float m_earthParticleEffectCounter = 0.5f;
		private int m_effectCycle;
		public Vector2 SavedStartingPos
		{
			get;
			set;
		}
		public SpriteObj EarthProjectile
		{
			get
			{
				return this.m_earthProjectileObj;
			}
		}
		protected override void InitializeEV()
		{
			base.Name = "Earthsor";
			this.MaxHealth = 32;
			base.Damage = 20;
			base.XPValue = 175;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 270f;
			this.TurnSpeed = 0.04f;
			this.ProjectileSpeed = 650f;
			base.JumpHeight = 300f;
			this.CooldownTime = 1.25f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = true;
			base.CanBeKnockedBack = true;
			base.IsWeighted = false;
			this.Scale = EnemyEV.EarthWizard_Basic_Scale;
			base.ProjectileScale = EnemyEV.EarthWizard_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.EarthWizard_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 700;
			this.EngageRadius = 900;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.EarthWizard_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.SpellDelay = 0.5f;
				this.SpellDuration = 1f;
				base.Name = "Gravisor";
				this.MaxHealth = 45;
				base.Damage = 24;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 270f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 300f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.EarthWizard_Advanced_Scale;
				base.ProjectileScale = EnemyEV.EarthWizard_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.EarthWizard_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 900;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.EarthWizard_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.SpellDelay = 0.7f;
				this.SpellDuration = 3.5f;
				base.Name = "Terrasor";
				this.MaxHealth = 72;
				base.Damage = 27;
				base.XPValue = 400;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 300f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 300f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.EarthWizard_Expert_Scale;
				base.ProjectileScale = EnemyEV.EarthWizard_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.EarthWizard_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.EarthWizard_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				this.SpellDelay = 0.85f;
				this.SpellDuration = 2f;
				this.m_spellOffset = new Vector2(40f, -140f);
				base.Name = "Barbatos  & Amon";
				this.MaxHealth = 225;
				base.Damage = 30;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 18;
				this.MaxMoneyDropAmount = 25;
				this.MoneyDropChance = 1f;
				base.Speed = 225f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 300f;
				this.CooldownTime = 0.75f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.EarthWizard_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.EarthWizard_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.EarthWizard_Miniboss_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.EarthWizard_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		public void PublicInitializeEV()
		{
			this.InitializeEV();
		}
		protected override void InitializeLogic()
		{
			this.InitializeProjectiles();
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new ChaseLogicAction(this.m_target, new Vector2(-255f, -175f), new Vector2(255f, -75f), true, this.MoveDuration, -1f), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChaseLogicAction(this.m_target, false, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpell", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellIn", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellOut", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpell", new object[]
			{
				this.SpellDuration
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpellIn", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", new object[]
			{
				this.SpellIceProjectileCount
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.TeleportDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("TeleportStart", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.TeleportDuration, false), Types.Sequence.Serial);
			logicSet7.AddAction(new TeleportLogicAction(this.m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5,
				logicSet6,
				logicSet7
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			LogicBlock arg_971_1 = this.m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 100;
			base.SetCooldownLogicBlock(arg_971_1, array);
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = this.m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				base.RunLogicBlock(arg_38_1, arg_38_2, array3);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = this.m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				base.RunLogicBlock(arg_38_1, arg_38_2, array3);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = this.m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				base.RunLogicBlock(arg_38_1, arg_38_2, array3);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_82_1 = true;
				LogicBlock arg_82_2 = this.m_generalMiniBossLB;
				int[] array = new int[7];
				array[0] = 60;
				array[1] = 10;
				array[2] = 30;
				base.RunLogicBlock(arg_82_1, arg_82_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalMiniBossLB;
				int[] array2 = new int[7];
				array2[0] = 100;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_42_1 = true;
				LogicBlock arg_42_2 = this.m_generalMiniBossLB;
				int[] array3 = new int[7];
				array3[0] = 34;
				array3[3] = 22;
				array3[4] = 22;
				array3[5] = 22;
				base.RunLogicBlock(arg_42_1, arg_42_2, array3);
				return;
			}
			default:
				return;
			}
		}
		public EnemyObj_EarthWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.PlayAnimation(true);
			this.TintablePart = this._objectList[0];
			this.Type = 5;
		}
		private void InitializeProjectiles()
		{
			this.m_earthSummonInSprite = new ProjectileObj("WizardEarthSpellCast_Sprite");
			this.m_earthSummonInSprite.AnimationDelay = 0.1f;
			this.m_earthSummonInSprite.PlayAnimation(true);
			this.m_earthSummonInSprite.Scale = Vector2.Zero;
			this.m_earthSummonOutSprite = (this.m_earthSummonInSprite.Clone() as SpriteObj);
			this.m_earthSummonOutSprite.PlayAnimation(true);
			this.m_earthProjectileObj = new ProjectileObj("WizardEarthSpell_Sprite");
			this.m_earthProjectileObj.IsWeighted = false;
			this.m_earthProjectileObj.CollidesWithTerrain = false;
			this.m_earthProjectileObj.DestroysWithEnemy = false;
			this.m_earthProjectileObj.Damage = base.Damage;
			this.m_earthProjectileObj.Scale = base.ProjectileScale;
			this.m_earthProjectileObj.AnimationDelay = 0.05f;
			this.m_earthProjectileObj.Rotation = 0f;
			this.m_earthProjectileObj.CanBeFusRohDahed = false;
		}
		public void CancelEarthSpell()
		{
			Tween.StopAllContaining(this.m_earthSummonOutSprite, false);
			Tween.StopAllContaining(this, false);
			Tween.To(this.m_earthSummonOutSprite, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			if (this.m_earthProjectileObj.CurrentFrame != 1 && this.m_earthProjectileObj.CurrentFrame != this.m_earthProjectileObj.TotalFrames)
			{
				SoundManager.Play3DSound(this, this.m_target, "Earth_Wizard_Fall");
				this.m_earthProjectileObj.PlayAnimation("Grown", "End", false);
			}
			this.m_levelScreen.PhysicsManager.RemoveObject(this.m_earthProjectileObj);
		}
		public void CancelEarthSpellIn()
		{
			Tween.StopAllContaining(this.m_earthSummonInSprite, false);
			Tween.To(this.m_earthSummonInSprite, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
		}
		public void CastEarthSpellIn()
		{
			SoundManager.Play3DSound(this, this.m_target, "Earth_Wizard_Form");
			this.m_earthSummonInSprite.Scale = Vector2.Zero;
			Tween.To(this.m_earthSummonInSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
		}
		public void CastEarthSpellOut()
		{
			this.m_earthSummonOutSprite.Scale = Vector2.Zero;
			this.m_earthSummonOutSprite.X = this.m_target.X;
			int num = 2147483647;
			TerrainObj terrainObj = null;
			foreach (TerrainObj current in this.m_levelScreen.CurrentRoom.TerrainObjList)
			{
				if (CollisionMath.Intersects(new Rectangle((int)this.m_target.X, (int)this.m_target.Y, 2, 720), current.Bounds))
				{
					int num2 = current.Bounds.Top - this.m_target.TerrainBounds.Bottom;
					if (num2 < num)
					{
						num = num2;
						terrainObj = current;
					}
				}
			}
			if (terrainObj != null)
			{
				if (terrainObj.Rotation == 0f)
				{
					this.m_earthSummonOutSprite.Y = (float)terrainObj.Bounds.Top;
				}
				else
				{
					Vector2 vector;
					Vector2 vector2;
					if (terrainObj.Width > terrainObj.Height)
					{
						vector = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					else if (terrainObj.Rotation > 0f)
					{
						vector = CollisionMath.LowerLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					else
					{
						vector = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.LowerRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					float num3 = vector2.X - vector.X;
					float num4 = vector2.Y - vector.Y;
					float x = vector.X;
					float y = vector.Y;
					float x2 = this.m_earthSummonOutSprite.X;
					float num5 = y + (x2 - x) * (num4 / num3);
					num5 -= (float)this.m_earthSummonOutSprite.Bounds.Bottom - this.m_earthSummonOutSprite.Y;
					this.m_earthSummonOutSprite.Y = (float)Math.Round((double)num5, MidpointRounding.ToEven);
				}
			}
			object arg_2A0_0 = this.m_earthSummonOutSprite;
			float arg_2A0_1 = 0.5f;
			Easing arg_2A0_2 = new Easing(Back.EaseOut);
			string[] array = new string[6];
			array[0] = "Opacity";
			array[1] = "1";
			array[2] = "ScaleX";
			string[] arg_28B_0 = array;
			int arg_28B_1 = 3;
			float x3 = base.ProjectileScale.X;
			arg_28B_0[arg_28B_1] = x3.ToString();
			array[4] = "ScaleY";
			array[5] = "1";
			Tween.To(arg_2A0_0, arg_2A0_1, arg_2A0_2, array);
		}
		public void CastEarthSpell(float duration)
		{
			this.m_levelScreen.PhysicsManager.AddObject(this.m_earthProjectileObj);
			this.m_earthProjectileObj.Scale = base.ProjectileScale;
			this.m_earthProjectileObj.StopAnimation();
			this.m_earthProjectileObj.Position = this.m_earthSummonOutSprite.Position;
			this.m_earthProjectileObj.PlayAnimation("Start", "Grown", false);
			SoundManager.Play3DSound(this, this.m_target, "Earth_Wizard_Attack");
			Tween.RunFunction(duration, this, "CancelEarthSpell", new object[0]);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (this.Flip == SpriteEffects.None)
			{
				this.m_earthSummonInSprite.Position = new Vector2(base.X + this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
			}
			else
			{
				this.m_earthSummonInSprite.Position = new Vector2(base.X - this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
			}
			if (this.m_fireballSummon != null)
			{
				if (this.Flip == SpriteEffects.None)
				{
					this.m_fireballSummon.Position = new Vector2(base.X + this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
				}
				else
				{
					this.m_fireballSummon.Position = new Vector2(base.X - this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
				}
			}
			if (this.m_iceballSummon != null)
			{
				if (this.Flip == SpriteEffects.None)
				{
					this.m_iceballSummon.Position = new Vector2(base.X + this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
				}
				else
				{
					this.m_iceballSummon.Position = new Vector2(base.X - this.m_spellOffset.X, base.Y + this.m_spellOffset.Y);
				}
			}
			if (this.m_earthParticleEffectCounter > 0f)
			{
				this.m_earthParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_earthParticleEffectCounter <= 0f)
				{
					if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
					{
						if (this.m_effectCycle == 0)
						{
							this.m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
						}
						else if (this.m_effectCycle == 1)
						{
							this.m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
						}
						else
						{
							this.m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
						}
						this.m_effectCycle++;
						if (this.m_effectCycle > 2)
						{
							this.m_effectCycle = 0;
						}
					}
					else
					{
						this.m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
					}
					this.m_earthParticleEffectCounter = 0.15f;
				}
			}
		}
		public void CastFireball()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardFireballProjectile_Sprite",
				SourceAnchor = this.m_spellOffset,
				Target = this.m_target,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.MiniBossFireballSize
			};
			if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				projectileData.AngleOffset = (float)CDGMath.RandomInt(-25, 25);
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
			}
			SoundManager.Play3DSound(this, this.m_target, new string[]
			{
				"FireWizard_Attack_01",
				"FireWizard_Attack_02",
				"FireWizard_Attack_03",
				"FireWizard_Attack_04"
			});
			ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileObj.Rotation = 0f;
			Tween.RunFunction(0.15f, this, "ChangeFireballState", new object[]
			{
				projectileObj
			});
		}
		public void ChangeFireballState(ProjectileObj fireball)
		{
			fireball.CollidesWithTerrain = true;
		}
		public void SummonFireball()
		{
			this.ResetFireball();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardFireballProjectile_Sprite",
				SourceAnchor = this.m_spellOffset,
				Target = this.m_target,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.MiniBossFireballSize
			};
			if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
			}
			SoundManager.Play3DSound(this, this.m_target, "Fire_Wizard_Form");
			this.m_fireballSummon = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			this.m_fireballSummon.Opacity = 0f;
			this.m_fireballSummon.Scale = Vector2.Zero;
			this.m_fireballSummon.AnimationDelay = 0.1f;
			this.m_fireballSummon.PlayAnimation(true);
			this.m_fireballSummon.Rotation = 0f;
			Tween.To(this.m_fireballSummon, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Opacity",
				"1",
				"ScaleX",
				this.MiniBossFireballSize.X.ToString(),
				"ScaleY",
				this.MiniBossFireballSize.Y.ToString()
			});
			projectileData.Dispose();
		}
		public void ResetFireball()
		{
			if (this.m_fireballSummon != null)
			{
				this.m_levelScreen.ProjectileManager.DestroyProjectile(this.m_fireballSummon);
				this.m_fireballSummon = null;
			}
		}
		public void SummonIceball()
		{
			this.ResetIceball();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardIceSpell_Sprite",
				SourceAnchor = this.m_spellOffset,
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.MiniBossIceSize
			};
			SoundManager.Play3DSound(this, this.m_target, "Ice_Wizard_Form");
			this.m_iceballSummon = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			this.m_iceballSummon.PlayAnimation("Start", "Grown", false);
			projectileData.Dispose();
		}
		public void ShatterIceball(int numIceballs)
		{
			SoundManager.Play3DSound(this, this.m_target, "Ice_Wizard_Attack_Glass");
			if (this.m_iceballSummon.SpriteName == "WizardIceSpell_Sprite")
			{
				this.m_iceballSummon.PlayAnimation("Grown", "End", false);
			}
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardIceProjectile_Sprite",
				SourceAnchor = this.m_spellOffset,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.MiniBossIceSize
			};
			float num = 0f;
			float num2 = (float)(360 / numIceballs);
			for (int i = 0; i < numIceballs; i++)
			{
				projectileData.Angle = new Vector2(num, num);
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				Tween.RunFunction(0.15f, this, "ChangeIceballState", new object[]
				{
					projectileObj
				});
				num += num2;
			}
			projectileData.Dispose();
		}
		public void ChangeIceballState(ProjectileObj iceball)
		{
			iceball.CollidesWithTerrain = true;
		}
		public void ResetIceball()
		{
			if (this.m_iceballSummon != null)
			{
				this.m_levelScreen.ProjectileManager.DestroyProjectile(this.m_iceballSummon);
				this.m_iceballSummon = null;
			}
		}
		public override void Kill(bool giveXP = true)
		{
			if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
			{
				this.CancelEarthSpell();
				this.CancelEarthSpellIn();
				this.m_currentActiveLB.StopLogicBlock();
			}
			base.Kill(giveXP);
		}
		public override void Draw(Camera2D camera)
		{
			this.m_earthSummonInSprite.Draw(camera);
			this.m_earthSummonOutSprite.Draw(camera);
			this.m_earthProjectileObj.Draw(camera);
			base.Draw(camera);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (otherBox.AbsParent is PlayerObj)
			{
				base.CurrentSpeed = 0f;
			}
			if (collisionResponseType != 1)
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				return;
			}
			if (!(otherBox.AbsParent is PlayerObj))
			{
				IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
				if (physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.CollidesLeft && physicsObj.CollidesRight)
				{
					base.Position += CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
				}
			}
		}
		public override void ResetState()
		{
			Tween.StopAllContaining(this, false);
			Tween.StopAllContaining(this.m_earthSummonOutSprite, false);
			Tween.StopAllContaining(this.m_earthSummonInSprite, false);
			this.m_earthSummonInSprite.Scale = Vector2.Zero;
			this.m_earthSummonOutSprite.Scale = Vector2.Zero;
			this.m_earthProjectileObj.StopAnimation();
			this.m_earthProjectileObj.GoToFrame(this.m_earthProjectileObj.TotalFrames);
			this.ResetFireball();
			this.ResetIceball();
			base.ResetState();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_fireballSummon = null;
				this.m_iceballSummon = null;
				if (this.m_earthSummonInSprite != null)
				{
					this.m_earthSummonInSprite.Dispose();
					this.m_earthSummonInSprite = null;
				}
				if (this.m_earthSummonOutSprite != null)
				{
					this.m_earthSummonOutSprite.Dispose();
					this.m_earthSummonOutSprite = null;
				}
				if (this.m_earthProjectileObj != null)
				{
					this.m_earthProjectileObj.Dispose();
					this.m_earthProjectileObj = null;
				}
				this.SpawnRoom = null;
				base.Dispose();
			}
		}
	}
}
