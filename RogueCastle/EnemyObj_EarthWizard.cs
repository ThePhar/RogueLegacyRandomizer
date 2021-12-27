// 
// RogueLegacyArchipelago - EnemyObj_EarthWizard.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class EnemyObj_EarthWizard : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();
        private readonly Vector2 MiniBossIceSize = new Vector2(1.5f, 1.5f);
        private readonly float MoveDuration = 1f;
        private readonly float SpellFireDelay = 1.5f;
        private readonly float SpellFireInterval = 0.2f;
        private readonly int SpellIceProjectileCount = 24;
        private readonly float TeleportDelay = 0.5f;
        private readonly float TeleportDuration = 1f;
        private float m_earthParticleEffectCounter = 0.5f;
        private ProjectileObj m_earthProjectileObj;
        private SpriteObj m_earthSummonInSprite;
        private SpriteObj m_earthSummonOutSprite;
        private int m_effectCycle;
        private ProjectileObj m_fireballSummon;
        private ProjectileObj m_iceballSummon;
        private Vector2 m_spellOffset = new Vector2(40f, -80f);
        private Vector2 MiniBossFireballSize = new Vector2(2f, 2f);
        public RoomObj SpawnRoom;
        private float SpellDelay = 0.3f;
        private float SpellDuration = 0.75f;

        public EnemyObj_EarthWizard(PlayerObj target, PhysicsManager physicsManager,
            ProceduralLevelScreen levelToAttachTo, EnemyDifficulty difficulty)
            : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            PlayAnimation();
            TintablePart = _objectList[0];
            Type = 5;
        }

        public Vector2 SavedStartingPos { get; set; }

        public SpriteObj EarthProjectile
        {
            get { return m_earthProjectileObj; }
        }

        protected override void InitializeEV()
        {
            Name = "Earthsor";
            MaxHealth = 32;
            Damage = 20;
            XPValue = 175;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 270f;
            TurnSpeed = 0.04f;
            ProjectileSpeed = 650f;
            JumpHeight = 300f;
            CooldownTime = 1.25f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            CanBeKnockedBack = true;
            IsWeighted = false;
            Scale = EnemyEV.EarthWizard_Basic_Scale;
            ProjectileScale = EnemyEV.EarthWizard_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.EarthWizard_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 900;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.EarthWizard_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;
                case EnemyDifficulty.Advanced:
                    SpellDelay = 0.5f;
                    SpellDuration = 1f;
                    Name = "Gravisor";
                    MaxHealth = 45;
                    Damage = 24;
                    XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 270f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 300f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.EarthWizard_Advanced_Scale;
                    ProjectileScale = EnemyEV.EarthWizard_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EarthWizard_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 900;
                    ProjectileRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EarthWizard_Advanced_KnockBack;
                    break;
                case EnemyDifficulty.Expert:
                    SpellDelay = 0.7f;
                    SpellDuration = 3.5f;
                    Name = "Terrasor";
                    MaxHealth = 72;
                    Damage = 27;
                    XPValue = 400;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 300f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 300f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.EarthWizard_Expert_Scale;
                    ProjectileScale = EnemyEV.EarthWizard_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EarthWizard_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EarthWizard_Expert_KnockBack;
                    return;
                case EnemyDifficulty.MiniBoss:
                    SpellDelay = 0.85f;
                    SpellDuration = 2f;
                    m_spellOffset = new Vector2(40f, -140f);
                    Name = "Barbatos  & Amon";
                    MaxHealth = 225;
                    Damage = 30;
                    XPValue = 1000;
                    MinMoneyDropAmount = 18;
                    MaxMoneyDropAmount = 25;
                    MoneyDropChance = 1f;
                    Speed = 225f;
                    TurnSpeed = 0.04f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 300f;
                    CooldownTime = 0.75f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.EarthWizard_Miniboss_Scale;
                    ProjectileScale = EnemyEV.EarthWizard_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EarthWizard_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 900;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EarthWizard_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        public void PublicInitializeEV()
        {
            InitializeEV();
        }

        protected override void InitializeLogic()
        {
            InitializeProjectiles();
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
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpell"));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellIn"));
            logicSet4.AddAction(new DelayLogicAction(0.5f));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellOut"));
            logicSet4.AddAction(new DelayLogicAction(SpellDelay));
            logicSet4.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            logicSet4.AddAction(new DelayLogicAction(0.2f));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpell", SpellDuration));
            logicSet4.AddAction(new DelayLogicAction(0.2f));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpellIn"));
            logicSet4.AddAction(new DelayLogicAction(0.5f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null));
            logicSet5.AddAction(new DelayLogicAction(SpellFireDelay));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(SpellFireInterval));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            logicSet5.AddAction(new DelayLogicAction(0.5f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            logicSet6.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null));
            logicSet6.AddAction(new DelayLogicAction(SpellDelay));
            logicSet6.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", SpellIceProjectileCount));
            logicSet6.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(0.5f));
            logicSet6.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null));
            logicSet6.AddAction(new DelayLogicAction(0.5f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.Tag = 2;
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport"));
            logicSet7.AddAction(new DelayLogicAction(TeleportDelay));
            logicSet7.AddAction(new PlayAnimationLogicAction("TeleportStart", "End"));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false));
            logicSet7.AddAction(new DelayLogicAction(TeleportDuration));
            logicSet7.AddAction(new TeleportLogicAction(m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.AddAction(new DelayLogicAction(0.5f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalMiniBossLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5, logicSet6, logicSet7);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            var arg_971_1 = m_generalCooldownLB;
            var array = new int[3];
            array[0] = 100;
            SetCooldownLogicBlock(arg_971_1, array);
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

        protected override void RunMinibossLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_82_1 = true;
                    var arg_82_2 = m_generalMiniBossLB;
                    var array = new int[7];
                    array[0] = 60;
                    array[1] = 10;
                    array[2] = 30;
                    RunLogicBlock(arg_82_1, arg_82_2, array);
                    return;
                }
                case 1:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalMiniBossLB;
                    var array2 = new int[7];
                    array2[0] = 100;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                {
                    var arg_42_1 = true;
                    var arg_42_2 = m_generalMiniBossLB;
                    var array3 = new int[7];
                    array3[0] = 34;
                    array3[3] = 22;
                    array3[4] = 22;
                    array3[5] = 22;
                    RunLogicBlock(arg_42_1, arg_42_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        private void InitializeProjectiles()
        {
            m_earthSummonInSprite = new ProjectileObj("WizardEarthSpellCast_Sprite");
            m_earthSummonInSprite.AnimationDelay = 0.1f;
            m_earthSummonInSprite.PlayAnimation();
            m_earthSummonInSprite.Scale = Vector2.Zero;
            m_earthSummonOutSprite = (m_earthSummonInSprite.Clone() as SpriteObj);
            m_earthSummonOutSprite.PlayAnimation();
            m_earthProjectileObj = new ProjectileObj("WizardEarthSpell_Sprite");
            m_earthProjectileObj.IsWeighted = false;
            m_earthProjectileObj.CollidesWithTerrain = false;
            m_earthProjectileObj.DestroysWithEnemy = false;
            m_earthProjectileObj.Damage = Damage;
            m_earthProjectileObj.Scale = ProjectileScale;
            m_earthProjectileObj.AnimationDelay = 0.05f;
            m_earthProjectileObj.Rotation = 0f;
            m_earthProjectileObj.CanBeFusRohDahed = false;
        }

        public void CancelEarthSpell()
        {
            Tween.StopAllContaining(m_earthSummonOutSprite, false);
            Tween.StopAllContaining(this, false);
            Tween.To(m_earthSummonOutSprite, 0.5f, Linear.EaseNone, "ScaleX", "0", "ScaleY", "0");
            if (m_earthProjectileObj.CurrentFrame != 1 &&
                m_earthProjectileObj.CurrentFrame != m_earthProjectileObj.TotalFrames)
            {
                SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Fall");
                m_earthProjectileObj.PlayAnimation("Grown", "End");
            }
            m_levelScreen.PhysicsManager.RemoveObject(m_earthProjectileObj);
        }

        public void CancelEarthSpellIn()
        {
            Tween.StopAllContaining(m_earthSummonInSprite, false);
            Tween.To(m_earthSummonInSprite, 0.5f, Linear.EaseNone, "ScaleX", "0", "ScaleY", "0");
        }

        public void CastEarthSpellIn()
        {
            SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Form");
            m_earthSummonInSprite.Scale = Vector2.Zero;
            Tween.To(m_earthSummonInSprite, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
        }

        public void CastEarthSpellOut()
        {
            m_earthSummonOutSprite.Scale = Vector2.Zero;
            m_earthSummonOutSprite.X = m_target.X;
            var num = 2147483647;
            TerrainObj terrainObj = null;
            foreach (var current in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                if (CollisionMath.Intersects(new Rectangle((int) m_target.X, (int) m_target.Y, 2, 720), current.Bounds))
                {
                    var num2 = current.Bounds.Top - m_target.TerrainBounds.Bottom;
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
                    m_earthSummonOutSprite.Y = terrainObj.Bounds.Top;
                }
                else
                {
                    Vector2 vector;
                    Vector2 vector2;
                    if (terrainObj.Width > terrainObj.Height)
                    {
                        vector = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                        vector2 = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                    }
                    else if (terrainObj.Rotation > 0f)
                    {
                        vector = CollisionMath.LowerLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                        vector2 = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                    }
                    else
                    {
                        vector = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                        vector2 = CollisionMath.LowerRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation,
                            Vector2.Zero);
                    }
                    var num3 = vector2.X - vector.X;
                    var num4 = vector2.Y - vector.Y;
                    var x = vector.X;
                    var y = vector.Y;
                    var x2 = m_earthSummonOutSprite.X;
                    var num5 = y + (x2 - x)*(num4/num3);
                    num5 -= m_earthSummonOutSprite.Bounds.Bottom - m_earthSummonOutSprite.Y;
                    m_earthSummonOutSprite.Y = (float) Math.Round(num5, MidpointRounding.ToEven);
                }
            }
            object arg_2A0_0 = m_earthSummonOutSprite;
            var arg_2A0_1 = 0.5f;
            Easing arg_2A0_2 = Back.EaseOut;
            var array = new string[6];
            array[0] = "Opacity";
            array[1] = "1";
            array[2] = "ScaleX";
            var arg_28B_0 = array;
            var arg_28B_1 = 3;
            var x3 = ProjectileScale.X;
            arg_28B_0[arg_28B_1] = x3.ToString();
            array[4] = "ScaleY";
            array[5] = "1";
            Tween.To(arg_2A0_0, arg_2A0_1, arg_2A0_2, array);
        }

        public void CastEarthSpell(float duration)
        {
            m_levelScreen.PhysicsManager.AddObject(m_earthProjectileObj);
            m_earthProjectileObj.Scale = ProjectileScale;
            m_earthProjectileObj.StopAnimation();
            m_earthProjectileObj.Position = m_earthSummonOutSprite.Position;
            m_earthProjectileObj.PlayAnimation("Start", "Grown");
            SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Attack");
            Tween.RunFunction(duration, this, "CancelEarthSpell");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Flip == SpriteEffects.None)
            {
                m_earthSummonInSprite.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
            }
            else
            {
                m_earthSummonInSprite.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
            }
            if (m_fireballSummon != null)
            {
                if (Flip == SpriteEffects.None)
                {
                    m_fireballSummon.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
                }
                else
                {
                    m_fireballSummon.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
                }
            }
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
            if (m_earthParticleEffectCounter > 0f)
            {
                m_earthParticleEffectCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_earthParticleEffectCounter <= 0f)
                {
                    if (Difficulty == EnemyDifficulty.MiniBoss)
                    {
                        if (m_effectCycle == 0)
                        {
                            m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
                        }
                        else if (m_effectCycle == 1)
                        {
                            m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
                        }
                        else
                        {
                            m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
                        }
                        m_effectCycle++;
                        if (m_effectCycle > 2)
                        {
                            m_effectCycle = 0;
                        }
                    }
                    else
                    {
                        m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
                    }
                    m_earthParticleEffectCounter = 0.15f;
                }
            }
        }

        public void CastFireball()
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = MiniBossFireballSize
            };
            if (Difficulty == EnemyDifficulty.Advanced)
            {
                projectileData.AngleOffset = CDGMath.RandomInt(-25, 25);
            }
            if (Difficulty == EnemyDifficulty.Expert)
            {
                projectileData.SpriteName = "GhostBossProjectile_Sprite";
            }
            SoundManager.Play3DSound(this, m_target, "FireWizard_Attack_01", "FireWizard_Attack_02",
                "FireWizard_Attack_03", "FireWizard_Attack_04");
            var projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileObj.Rotation = 0f;
            Tween.RunFunction(0.15f, this, "ChangeFireballState", projectileObj);
        }

        public void ChangeFireballState(ProjectileObj fireball)
        {
            fireball.CollidesWithTerrain = true;
        }

        public void SummonFireball()
        {
            ResetFireball();
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = MiniBossFireballSize
            };
            if (Difficulty == EnemyDifficulty.Expert)
            {
                projectileData.SpriteName = "GhostBossProjectile_Sprite";
            }
            SoundManager.Play3DSound(this, m_target, "Fire_Wizard_Form");
            m_fireballSummon = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            m_fireballSummon.Opacity = 0f;
            m_fireballSummon.Scale = Vector2.Zero;
            m_fireballSummon.AnimationDelay = 0.1f;
            m_fireballSummon.PlayAnimation();
            m_fireballSummon.Rotation = 0f;
            Tween.To(m_fireballSummon, 0.5f, Back.EaseOut, "Opacity", "1", "ScaleX", MiniBossFireballSize.X.ToString(),
                "ScaleY", MiniBossFireballSize.Y.ToString());
            projectileData.Dispose();
        }

        public void ResetFireball()
        {
            if (m_fireballSummon != null)
            {
                m_levelScreen.ProjectileManager.DestroyProjectile(m_fireballSummon);
                m_fireballSummon = null;
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
                Scale = MiniBossIceSize
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
                Scale = MiniBossIceSize
            };
            var num = 0f;
            float num2 = 360/numIceballs;
            for (var i = 0; i < numIceballs; i++)
            {
                projectileData.Angle = new Vector2(num, num);
                var projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                Tween.RunFunction(0.15f, this, "ChangeIceballState", projectileObj);
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
                CancelEarthSpell();
                CancelEarthSpellIn();
                m_currentActiveLB.StopLogicBlock();
            }
            base.Kill(giveXP);
        }

        public override void Draw(Camera2D camera)
        {
            m_earthSummonInSprite.Draw(camera);
            m_earthSummonOutSprite.Draw(camera);
            m_earthProjectileObj.Draw(camera);
            base.Draw(camera);
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
            Tween.StopAllContaining(this, false);
            Tween.StopAllContaining(m_earthSummonOutSprite, false);
            Tween.StopAllContaining(m_earthSummonInSprite, false);
            m_earthSummonInSprite.Scale = Vector2.Zero;
            m_earthSummonOutSprite.Scale = Vector2.Zero;
            m_earthProjectileObj.StopAnimation();
            m_earthProjectileObj.GoToFrame(m_earthProjectileObj.TotalFrames);
            ResetFireball();
            ResetIceball();
            base.ResetState();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_fireballSummon = null;
                m_iceballSummon = null;
                if (m_earthSummonInSprite != null)
                {
                    m_earthSummonInSprite.Dispose();
                    m_earthSummonInSprite = null;
                }
                if (m_earthSummonOutSprite != null)
                {
                    m_earthSummonOutSprite.Dispose();
                    m_earthSummonOutSprite = null;
                }
                if (m_earthProjectileObj != null)
                {
                    m_earthProjectileObj.Dispose();
                    m_earthProjectileObj = null;
                }
                SpawnRoom = null;
                base.Dispose();
            }
        }
    }
}
