// 
// RogueLegacyArchipelago - EnemyObj_Wolf.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.TypeDefinitions;

namespace RogueCastle
{
    public class EnemyObj_Wolf : EnemyObj
    {
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly FrameSoundObj m_runFrameSound;
        private readonly float m_startDelay = 1f;
        private readonly LogicBlock m_wolfHitLB = new LogicBlock();
        private readonly float PounceDelay = 0.3f;
        private readonly float PounceLandDelay = 0.5f;
        private Color FurColour = Color.White;
        private float m_startDelayCounter;

        public EnemyObj_Wolf(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyWargIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 19;
            m_startDelayCounter = m_startDelay;
            m_runFrameSound = new FrameSoundObj(this, 1, "Wolf_Move01", "Wolf_Move02", "Wolf_Move03");
        }

        public bool Chasing { get; set; }

        protected override void InitializeEV()
        {
            Name = "Warg";
            MaxHealth = 18;
            Damage = 25;
            XPValue = 75;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 600f;
            TurnSpeed = 10f;
            ProjectileSpeed = 650f;
            JumpHeight = 1035f;
            CooldownTime = 2f;
            AnimationDelay = 0.0333333351f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Wolf_Basic_Scale;
            ProjectileScale = EnemyEV.Wolf_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Wolf_Basic_Tint;
            MeleeRadius = 50;
            ProjectileRadius = 400;
            EngageRadius = 550;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Wolf_Basic_KnockBack;
            InitialLogicDelay = 1f;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                    Name = "Wargen";
                    MaxHealth = 25;
                    Damage = 28;
                    XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 700f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 1035f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Wolf_Advanced_Scale;
                    ProjectileScale = EnemyEV.Wolf_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Advanced_Tint;
                    MeleeRadius = 50;
                    EngageRadius = 575;
                    ProjectileRadius = 400;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Wolf_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    Name = "Wargenflorgen";
                    MaxHealth = 52;
                    Damage = 32;
                    XPValue = 225;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 850f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 1035f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Wolf_Expert_Scale;
                    ProjectileScale = EnemyEV.Wolf_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Expert_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 400;
                    EngageRadius = 625;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Wolf_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    Name = "Zorg Warg";
                    MaxHealth = 500;
                    Damage = 35;
                    XPValue = 750;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 1f;
                    Speed = 925f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 1035f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Wolf_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Wolf_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Wolf_Miniboss_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 400;
                    EngageRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Wolf_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWargRun_Character"));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Chasing", true));
            logicSet.AddAction(new DelayLogicAction(1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character"));
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Chasing", false));
            logicSet2.AddAction(new DelayLogicAction(1f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new GroundCheckLogicAction());
            logicSet3.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Wolf_Attack"));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargPounce_Character"));
            logicSet3.AddAction(new DelayLogicAction(PounceDelay));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargJump_Character", false, false));
            logicSet3.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet3.AddAction(new MoveDirectionLogicAction());
            logicSet3.AddAction(new JumpLogicAction());
            logicSet3.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet3.AddAction(new GroundCheckLogicAction());
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(PounceLandDelay));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWargHit_Character", false, false));
            logicSet4.AddAction(new DelayLogicAction(0.2f));
            logicSet4.AddAction(new GroundCheckLogicAction());
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            m_wolfHitLB.AddLogicSet(logicSet4);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_wolfHitLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            if (m_startDelayCounter <= 0f)
            {
                switch (State)
                {
                    case 0:
                        if (Chasing)
                        {
                            var arg_C7_1 = false;
                            var arg_C7_2 = m_generalBasicLB;
                            var array = new int[3];
                            array[1] = 100;
                            RunLogicBlock(arg_C7_1, arg_C7_2, array);
                        }
                        break;
                    case 1:
                        if (!Chasing)
                        {
                            var arg_A1_1 = false;
                            var arg_A1_2 = m_generalBasicLB;
                            var array2 = new int[3];
                            array2[0] = 100;
                            RunLogicBlock(arg_A1_1, arg_A1_2, array2);
                        }
                        break;
                    case 2:
                    case 3:
                    {
                        if (m_target.Y < Y - m_target.Height)
                        {
                            RunLogicBlock(false, m_generalBasicLB, 0, 0, 100);
                            return;
                        }
                        var arg_7E_1 = false;
                        var arg_7E_2 = m_generalBasicLB;
                        var array3 = new int[3];
                        array3[0] = 100;
                        RunLogicBlock(arg_7E_1, arg_7E_2, array3);
                        return;
                    }
                    default:
                        return;
                }
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case 0:
                    if (Chasing)
                    {
                        var arg_B7_1 = false;
                        var arg_B7_2 = m_generalBasicLB;
                        var array = new int[3];
                        array[1] = 100;
                        RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing)
                    {
                        var arg_91_1 = false;
                        var arg_91_2 = m_generalBasicLB;
                        var array2 = new int[3];
                        array2[0] = 100;
                        RunLogicBlock(arg_91_1, arg_91_2, array2);
                    }
                    break;
                case 2:
                case 3:
                {
                    if (m_target.Y < Y - m_target.Height)
                    {
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_6E_1 = false;
                    var arg_6E_2 = m_generalBasicLB;
                    var array3 = new int[3];
                    array3[0] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array3);
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
                    if (Chasing)
                    {
                        var arg_B7_1 = false;
                        var arg_B7_2 = m_generalBasicLB;
                        var array = new int[3];
                        array[1] = 100;
                        RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing)
                    {
                        var arg_91_1 = false;
                        var arg_91_2 = m_generalBasicLB;
                        var array2 = new int[3];
                        array2[0] = 100;
                        RunLogicBlock(arg_91_1, arg_91_2, array2);
                    }
                    break;
                case 2:
                case 3:
                {
                    if (m_target.Y < Y - m_target.Height)
                    {
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_6E_1 = false;
                    var arg_6E_2 = m_generalBasicLB;
                    var array3 = new int[3];
                    array3[0] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array3);
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
                    if (Chasing)
                    {
                        var arg_B7_1 = false;
                        var arg_B7_2 = m_generalBasicLB;
                        var array = new int[3];
                        array[1] = 100;
                        RunLogicBlock(arg_B7_1, arg_B7_2, array);
                    }
                    break;
                case 1:
                    if (!Chasing)
                    {
                        var arg_91_1 = false;
                        var arg_91_2 = m_generalBasicLB;
                        var array2 = new int[3];
                        array2[0] = 100;
                        RunLogicBlock(arg_91_1, arg_91_2, array2);
                    }
                    break;
                case 2:
                case 3:
                {
                    if (m_target.Y < Y - m_target.Height)
                    {
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_6E_1 = false;
                    var arg_6E_2 = m_generalBasicLB;
                    var array3 = new int[3];
                    array3[0] = 100;
                    RunLogicBlock(arg_6E_1, arg_6E_2, array3);
                    return;
                }
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (m_startDelayCounter > 0f)
            {
                m_startDelayCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (!m_isTouchingGround && IsWeighted && CurrentSpeed == 0f && SpriteName == "EnemyWargJump_Character")
            {
                CurrentSpeed = Speed;
            }
            base.Update(gameTime);
            if (m_isTouchingGround && CurrentSpeed == 0f && !IsAnimating)
            {
                ChangeSprite("EnemyWargIdle_Character");
                PlayAnimation();
            }
            if (SpriteName == "EnemyWargRun_Character")
            {
                m_runFrameSound.Update();
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Wolf_Hit_01", "Wolf_Hit_02", "Wolf_Hit_03");
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
            {
                m_currentActiveLB.StopLogicBlock();
            }
            m_currentActiveLB = m_wolfHitLB;
            m_currentActiveLB.RunLogicBlock(100);
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void ResetState()
        {
            m_startDelayCounter = m_startDelay;
            base.ResetState();
        }

        public override void Reset()
        {
            m_startDelayCounter = m_startDelay;
            base.Reset();
        }
    }
}