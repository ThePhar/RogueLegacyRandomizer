using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
namespace RogueCastle
{
	public class EnemyObj_IceWizard : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private float SpellDelay = 0.8f;
		private int SpellProjectileCount = 7;
		private ProjectileObj m_iceballSummon;
		private Vector2 m_spellOffset = new Vector2(40f, -80f);
		private float TeleportDelay = 0.5f;
		private float TeleportDuration = 1f;
		private float MoveDuration = 1f;
		private Vector2 IceScale = Vector2.One;
		private float m_iceParticleEffectCounter = 0.5f;
		protected override void InitializeEV()
		{
			this.SpellProjectileCount = 7;
			base.Name = "Frosten";
			this.MaxHealth = 32;
			base.Damage = 20;
			base.XPValue = 175;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 270f;
			this.TurnSpeed = 0.04f;
			this.ProjectileSpeed = 500f;
			base.JumpHeight = 300f;
			this.CooldownTime = 1.25f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = true;
			base.CanBeKnockedBack = true;
			base.IsWeighted = false;
			this.Scale = EnemyEV.IceWizard_Basic_Scale;
			base.ProjectileScale = EnemyEV.IceWizard_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.IceWizard_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 700;
			this.EngageRadius = 900;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.IceWizard_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.SpellProjectileCount = 14;
				base.Name = "Icen";
				this.MaxHealth = 45;
				base.Damage = 28;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 270f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 500f;
				base.JumpHeight = 300f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.IceWizard_Advanced_Scale;
				base.ProjectileScale = EnemyEV.IceWizard_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.IceWizard_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 900;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.IceWizard_Advanced_KnockBack;
				this.m_spellOffset = new Vector2(40f, -100f);
				this.IceScale = new Vector2(1.5f, 1.5f);
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.SpellProjectileCount = 8;
				this.SpellDelay = 1f;
				this.m_spellOffset = new Vector2(40f, -130f);
				base.Name = "Glacien";
				this.MaxHealth = 72;
				base.Damage = 32;
				base.XPValue = 400;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 300f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 600f;
				base.JumpHeight = 300f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.IceWizard_Expert_Scale;
				base.ProjectileScale = EnemyEV.IceWizard_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.IceWizard_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.IceWizard_Expert_KnockBack;
				this.IceScale = new Vector2(2f, 2f);
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Luna Mage";
				this.MaxHealth = 240;
				base.Damage = 40;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 18;
				this.MaxMoneyDropAmount = 25;
				this.MoneyDropChance = 1f;
				base.Speed = 375f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 500f;
				base.JumpHeight = 300f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.IceWizard_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.IceWizard_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.IceWizard_Miniboss_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.IceWizard_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
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
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.135f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", new object[]
			{
				this.SpellProjectileCount
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.TeleportDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("TeleportStart", "End", false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.TeleportDuration, false), Types.Sequence.Serial);
			logicSet6.AddAction(new TeleportLogicAction(this.m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet6
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet6
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet5,
				logicSet6
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
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			LogicBlock arg_6EB_1 = this.m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 100;
			base.SetCooldownLogicBlock(arg_6EB_1, array);
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
				LogicBlock arg_6E_2 = this.m_generalExpertLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalExpertLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalExpertLB;
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
			case 1:
			case 2:
			case 3:
				return;
			}
		}
		public EnemyObj_IceWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.PlayAnimation(true);
			this.TintablePart = this._objectList[0];
			this.Type = 11;
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
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
			if (this.m_iceParticleEffectCounter > 0f)
			{
				this.m_iceParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_iceParticleEffectCounter <= 0f)
				{
					this.m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
					this.m_iceParticleEffectCounter = 0.15f;
				}
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
				DestroysWithEnemy = false,
				Scale = this.IceScale,
				LockPosition = true
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
				Scale = base.ProjectileScale
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
		public void ShatterExpertIceball(int numIceballs)
		{
			SoundManager.Play3DSound(this, this.m_target, "Ice_Wizard_Attack");
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
				Scale = base.ProjectileScale
			};
			float num = 60f;
			for (int i = 0; i < numIceballs; i++)
			{
				float num2 = (float)CDGMath.RandomInt(0, 360);
				projectileData.Angle = new Vector2(num2, num2);
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				Tween.RunFunction(0.15f, this, "ChangeIceballState", new object[]
				{
					projectileObj
				});
				num2 += num;
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
				this.m_currentActiveLB.StopLogicBlock();
				this.ResetIceball();
			}
			base.Kill(giveXP);
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
			this.ResetIceball();
			base.ResetState();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_iceballSummon = null;
				base.Dispose();
			}
		}
	}
}
