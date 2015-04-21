using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_FireWizard : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private float SpellDelay = 0.7f;
		private float SpellInterval = 0.5f;
		private ProjectileObj m_fireballSummon;
		private Vector2 m_spellOffset = new Vector2(40f, -80f);
		private float TeleportDelay = 0.5f;
		private float TeleportDuration = 1f;
		private float MoveDuration = 1f;
		private float m_fireParticleEffectCounter = 0.5f;
		protected override void InitializeEV()
		{
			this.SpellInterval = 0.5f;
			base.Name = "Flamelock";
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
			this.Scale = EnemyEV.FireWizard_Basic_Scale;
			base.ProjectileScale = EnemyEV.FireWizard_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.FireWizard_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 700;
			this.EngageRadius = 900;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.FireWizard_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.SpellInterval = 0.15f;
				base.Name = "Blazelock";
				this.MaxHealth = 45;
				base.Damage = 28;
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
				this.Scale = EnemyEV.FireWizard_Advanced_Scale;
				base.ProjectileScale = EnemyEV.FireWizard_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.FireWizard_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 900;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.FireWizard_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.m_spellOffset = new Vector2(40f, -130f);
				this.SpellDelay = 1f;
				this.SpellInterval = 1f;
				base.Name = "Sollock";
				this.MaxHealth = 72;
				base.Damage = 35;
				base.XPValue = 400;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 270f;
				this.TurnSpeed = 0.04f;
				this.ProjectileSpeed = 300f;
				base.JumpHeight = 300f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.FireWizard_Expert_Scale;
				base.ProjectileScale = EnemyEV.FireWizard_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.FireWizard_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.FireWizard_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Sol Mage";
				this.MaxHealth = 240;
				base.Damage = 40;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 18;
				this.MaxMoneyDropAmount = 25;
				this.MoneyDropChance = 1f;
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
				this.Scale = EnemyEV.FireWizard_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.FireWizard_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.FireWizard_Miniboss_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.FireWizard_Miniboss_KnockBack;
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
			logicSet2.AddAction(new ChaseLogicAction(this.m_target, false, this.MoveDuration, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
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
			logicSet6.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.SpellDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.SpellInterval, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
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
				logicSet5,
				logicSet7
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
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
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			LogicBlock arg_738_1 = this.m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 100;
			base.SetCooldownLogicBlock(arg_738_1, array);
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
				LogicBlock arg_6E_2 = this.m_generalAdvancedLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalAdvancedLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalAdvancedLB;
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
		public EnemyObj_FireWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 9;
			base.PlayAnimation(true);
			this.TintablePart = this._objectList[0];
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
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
			if (this.m_fireParticleEffectCounter > 0f)
			{
				this.m_fireParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_fireParticleEffectCounter <= 0f)
				{
					this.m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
					this.m_fireParticleEffectCounter = 0.15f;
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
				Scale = base.ProjectileScale
			};
			if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				projectileData.AngleOffset = (float)CDGMath.RandomInt(-25, 25);
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
				projectileData.CollidesWithTerrain = false;
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
			if (base.Difficulty != GameTypes.EnemyDifficulty.EXPERT)
			{
				Tween.RunFunction(0.15f, this, "ChangeFireballState", new object[]
				{
					projectileObj
				});
			}
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
				DestroysWithEnemy = false,
				Scale = base.ProjectileScale
			};
			if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
				projectileData.CollidesWithTerrain = false;
			}
			SoundManager.Play3DSound(this, this.m_target, "Fire_Wizard_Form");
			this.m_fireballSummon = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			this.m_fireballSummon.Opacity = 0f;
			this.m_fireballSummon.Scale = Vector2.Zero;
			this.m_fireballSummon.AnimationDelay = 0.1f;
			this.m_fireballSummon.PlayAnimation(true);
			this.m_fireballSummon.Rotation = 0f;
			object arg_186_0 = this.m_fireballSummon;
			float arg_186_1 = 0.5f;
			Easing arg_186_2 = new Easing(Back.EaseOut);
			string[] array = new string[6];
			array[0] = "Opacity";
			array[1] = "1";
			array[2] = "ScaleX";
			string[] arg_165_0 = array;
			int arg_165_1 = 3;
			float x = base.ProjectileScale.X;
			arg_165_0[arg_165_1] = x.ToString();
			array[4] = "ScaleY";
			string[] arg_184_0 = array;
			int arg_184_1 = 5;
			float y = base.ProjectileScale.Y;
			arg_184_0[arg_184_1] = y.ToString();
			Tween.To(arg_186_0, arg_186_1, arg_186_2, array);
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
		public override void Kill(bool giveXP = true)
		{
			if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
			{
				this.m_currentActiveLB.StopLogicBlock();
				this.ResetFireball();
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
			this.ResetFireball();
			base.ResetState();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_fireballSummon = null;
				base.Dispose();
			}
		}
	}
}
