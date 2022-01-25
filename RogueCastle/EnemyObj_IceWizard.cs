// 
//  Rogue Legacy Randomizer - EnemyObj_IceWizard.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;
using RogueCastle.Screens;
using Tweener;
using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public class EnemyObj_IceWizard : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly float MoveDuration = 1f;
        private readonly float TeleportDelay = 0.5f;
        private readonly float TeleportDuration = 1f;
        private Vector2 IceScale = Vector2.One;
        private ProjectileObj m_iceballSummon;
        private float m_iceParticleEffectCounter = 0.5f;
        private Vector2 m_spellOffset = new Vector2(40f, -80f);
        private float SpellDelay = 0.8f;
        private int SpellProjectileCount = 7;

        public EnemyObj_IceWizard(PlayerObj target, PhysicsManager physicsManager,
            ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            PlayAnimation();
            TintablePart = _objectList[0];
            Type = 11;
        }

        protected override void InitializeEV()
        {
            SpellProjectileCount = 7;
            Name = "Frosten";
            MaxHealth = 32;
            Damage = 20;
            XPValue = 175;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 270f;
            TurnSpeed = 0.04f;
            ProjectileSpeed = 500f;
            JumpHeight = 300f;
            CooldownTime = 1.25f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            CanBeKnockedBack = true;
            IsWeighted = false;
            Scale = EnemyEV.IceWizard_Basic_Scale;
            ProjectileScale = EnemyEV.IceWizard_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.IceWizard_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 900;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.IceWizard_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;

                case EnemyDifficulty.Advanced:
                    SpellProjectileCount = 14;
                    Name = "Icen";
                    MaxHealth = 45;
                    Damage = 28;
                    XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 270f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 500f;
                    JumpHeight = 300f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.IceWizard_Advanced_Scale;
                    ProjectileScale = EnemyEV.IceWizard_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.IceWizard_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 900;
                    ProjectileRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.IceWizard_Advanced_KnockBack;
                    m_spellOffset = new Vector2(40f, -100f);
                    IceScale = new Vector2(1.5f, 1.5f);
                    break;

                case EnemyDifficulty.Expert:
                    SpellProjectileCount = 8;
                    SpellDelay = 1f;
                    m_spellOffset = new Vector2(40f, -130f);
                    Name = "Glacien";
                    MaxHealth = 72;
                    Damage = 32;
                    XPValue = 400;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 300f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 600f;
                    JumpHeight = 300f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.IceWizard_Expert_Scale;
                    ProjectileScale = EnemyEV.IceWizard_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.IceWizard_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.IceWizard_Expert_KnockBack;
                    IceScale = new Vector2(2f, 2f);
                    return;

                case EnemyDifficulty.MiniBoss:
                    Name = "Luna Mage";
                    MaxHealth = 240;
                    Damage = 40;
                    XPValue = 1000;
                    MinMoneyDropAmount = 18;
                    MaxMoneyDropAmount = 25;
                    MoneyDropChance = 1f;
                    Speed = 375f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 500f;
                    JumpHeight = 300f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.IceWizard_Miniboss_Scale;
                    ProjectileScale = EnemyEV.IceWizard_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.IceWizard_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.IceWizard_Miniboss_KnockBack;
                    return;

                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character"));
            logicSet.AddAction(new ChaseLogicAction(m_target, new Vector2(-255f, -175f), new Vector2(255f, -75f), true,
                MoveDuration));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character"));
            logicSet2.AddAction(new ChaseLogicAction(m_target, false, 1f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.5f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null));
            logicSet4.AddAction(new DelayLogicAction(SpellDelay));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", SpellProjectileCount));
            logicSet4.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            logicSet4.AddAction(new DelayLogicAction(0.5f));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null));
            logicSet4.AddAction(new DelayLogicAction(0.5f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null));
            logicSet5.AddAction(new DelayLogicAction(SpellDelay));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new DelayLogicAction(0.135f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            logicSet5.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            logicSet5.AddAction(new DelayLogicAction(0.5f));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null));
            logicSet5.AddAction(new DelayLogicAction(0.5f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport"));
            logicSet6.AddAction(new DelayLogicAction(TeleportDelay));
            logicSet6.AddAction(new PlayAnimationLogicAction("TeleportStart", "End"));
            logicSet6.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false));
            logicSet6.AddAction(new DelayLogicAction(TeleportDuration));
            logicSet6.AddAction(new TeleportLogicAction(m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)));
            logicSet6.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false));
            logicSet6.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.AddAction(new DelayLogicAction(0.5f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet6);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet6);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5, logicSet6);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            var arg_6EB_1 = m_generalCooldownLB;
            var array = new int[3];
            array[0] = 100;
            SetCooldownLogicBlock(arg_6EB_1, array);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_6E_1 = true;
                    var arg_6E_2 = m_generalBasicLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }

                case 1:
                {
                    var arg_53_1 = true;
                    var arg_53_2 = m_generalBasicLB;
                    var array2 = new int[5];
                    array2[0] = 100;
                    RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }

                case 2:
                case 3:
                {
                    var arg_38_1 = true;
                    var arg_38_2 = m_generalBasicLB;
                    var array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    RunLogicBlock(arg_38_1, arg_38_2, array3);
                    return;
                }

                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_6E_1 = true;
                    var arg_6E_2 = m_generalBasicLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }

                case 1:
                {
                    var arg_53_1 = true;
                    var arg_53_2 = m_generalBasicLB;
                    var array2 = new int[5];
                    array2[0] = 100;
                    RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }

                case 2:
                case 3:
                {
                    var arg_38_1 = true;
                    var arg_38_2 = m_generalBasicLB;
                    var array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    RunLogicBlock(arg_38_1, arg_38_2, array3);
                    return;
                }

                default:
                    return;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_6E_1 = true;
                    var arg_6E_2 = m_generalExpertLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array);
                    return;
                }

                case 1:
                {
                    var arg_53_1 = true;
                    var arg_53_2 = m_generalExpertLB;
                    var array2 = new int[5];
                    array2[0] = 100;
                    RunLogicBlock(arg_53_1, arg_53_2, array2);
                    return;
                }

                case 2:
                case 3:
                {
                    var arg_38_1 = true;
                    var arg_38_2 = m_generalExpertLB;
                    var array3 = new int[5];
                    array3[0] = 40;
                    array3[3] = 60;
                    RunLogicBlock(arg_38_1, arg_38_2, array3);
                    return;
                }

                default:
                    return;
            }
        }

        protected override void RunMinibossLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_iceballSummon != null)
            {
                if (Flip == SpriteEffects.None)
                {
                    m_iceballSummon.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
                }
                else
                {
                    m_iceballSummon.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
                }
            }

            if (m_iceParticleEffectCounter > 0f)
            {
                m_iceParticleEffectCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_iceParticleEffectCounter <= 0f)
                {
                    m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
                    m_iceParticleEffectCounter = 0.15f;
                }
            }
        }

        public void SummonIceball()
        {
            ResetIceball();
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "WizardIceSpell_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                Scale = IceScale,
                LockPosition = true
            };
            SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Form");
            m_iceballSummon = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            m_iceballSummon.PlayAnimation("Start", "Grown");
            projectileData.Dispose();
        }

        public void ShatterIceball(int numIceballs)
        {
            SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Attack_Glass");
            if (m_iceballSummon.SpriteName == "WizardIceSpell_Sprite")
            {
                m_iceballSummon.PlayAnimation("Grown", "End");
            }

            var projectileData = new ProjectileData(this)
            {
                SpriteName = "WizardIceProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            var num = 0f;
            float num2 = 360 / numIceballs;
            for (var i = 0; i < numIceballs; i++)
            {
                projectileData.Angle = new Vector2(num, num);
                var projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                Tween.RunFunction(0.15f, this, "ChangeIceballState", projectileObj);
                num += num2;
            }

            projectileData.Dispose();
        }

        public void ShatterExpertIceball(int numIceballs)
        {
            SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Attack");
            if (m_iceballSummon.SpriteName == "WizardIceSpell_Sprite")
            {
                m_iceballSummon.PlayAnimation("Grown", "End");
            }

            var projectileData = new ProjectileData(this)
            {
                SpriteName = "WizardIceProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            var num = 60f;
            for (var i = 0; i < numIceballs; i++)
            {
                float num2 = CDGMath.RandomInt(0, 360);
                projectileData.Angle = new Vector2(num2, num2);
                var projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                Tween.RunFunction(0.15f, this, "ChangeIceballState", projectileObj);
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
            if (m_iceballSummon != null)
            {
                m_levelScreen.ProjectileManager.DestroyProjectile(m_iceballSummon);
                m_iceballSummon = null;
            }
        }

        public override void Kill(bool giveXP = true)
        {
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
            {
                m_currentActiveLB.StopLogicBlock();
                ResetIceball();
            }

            base.Kill(giveXP);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj)
            {
                CurrentSpeed = 0f;
            }

            if (collisionResponseType != 1)
            {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                return;
            }

            if (!(otherBox.AbsParent is PlayerObj))
            {
                var physicsObj = otherBox.AbsParent as IPhysicsObj;
                if (physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.CollidesLeft &&
                    physicsObj.CollidesRight)
                {
                    Position += CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation,
                        Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                }
            }
        }

        public override void ResetState()
        {
            ResetIceball();
            base.ResetState();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_iceballSummon = null;
                base.Dispose();
            }
        }
    }
}