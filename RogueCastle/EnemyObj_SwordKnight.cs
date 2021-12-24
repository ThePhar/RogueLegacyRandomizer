// 
// RogueLegacyArchipelago - EnemyObj_SwordKnight.cs
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
    public class EnemyObj_SwordKnight : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly float SlashTripleDelay = 1.25f;
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;
        private float SlashDelay;
        private float TripleAttackSpeed = 500f;

        public EnemyObj_SwordKnight(PlayerObj target, PhysicsManager physicsManager,
            ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemySwordKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 16;
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        protected override void InitializeEV()
        {
            SlashDelay = 0.25f;
            Name = "Gray Knight";
            MaxHealth = 40;
            Damage = 36;
            XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 75f;
            TurnSpeed = 10f;
            ProjectileSpeed = 850f;
            JumpHeight = 950f;
            CooldownTime = 0.5f;
            AnimationDelay = 0.0833333358f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.SwordKnight_Basic_Scale;
            ProjectileScale = EnemyEV.SwordKnight_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.SwordKnight_Basic_Tint;
            MeleeRadius = 210;
            ProjectileRadius = 500;
            EngageRadius = 800;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.SwordKnight_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                    SlashDelay = 0.25f;
                    Name = "Gray Baron";
                    MaxHealth = 58;
                    Damage = 51;
                    XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 75f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    JumpHeight = 950f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.0833333358f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.SwordKnight_Advanced_Scale;
                    ProjectileScale = EnemyEV.SwordKnight_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SwordKnight_Advanced_Tint;
                    MeleeRadius = 300;
                    EngageRadius = 800;
                    ProjectileRadius = 500;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SwordKnight_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    SlashDelay = 0.25f;
                    TripleAttackSpeed = 500f;
                    Name = "Graydiator";
                    MaxHealth = 72;
                    Damage = 57;
                    XPValue = 350;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 85f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1350f;
                    JumpHeight = 950f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.0833333358f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.SwordKnight_Expert_Scale;
                    ProjectileScale = EnemyEV.SwordKnight_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SwordKnight_Expert_Tint;
                    MeleeRadius = 375;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SwordKnight_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    ForceDraw = true;
                    SlashDelay = 1.05f;
                    Name = "Graypion";
                    MaxHealth = 750;
                    Damage = 80;
                    XPValue = 800;
                    MinMoneyDropAmount = 10;
                    MaxMoneyDropAmount = 15;
                    MoneyDropChance = 1f;
                    Speed = 85f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    JumpHeight = 950f;
                    CooldownTime = 8f;
                    AnimationDelay = 0.125f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.SwordKnight_Miniboss_Scale;
                    ProjectileScale = EnemyEV.SwordKnight_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SwordKnight_Miniboss_Tint;
                    MeleeRadius = 300;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SwordKnight_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character"));
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(1f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.5f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet4.AddAction(new DelayLogicAction(SlashDelay));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet5.AddAction(new DelayLogicAction(SlashTripleDelay));
            logicSet5.AddAction(new MoveDirectionLogicAction(TripleAttackSpeed));
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0166666675f));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet5.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0833333358f));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            var data = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(60f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = true,
                Scale = ProjectileScale
            };
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet6.AddAction(new DelayLogicAction(SlashDelay));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
            logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.Tag = 2;
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet7.AddAction(new DelayLogicAction(SlashTripleDelay));
            logicSet7.AddAction(new MoveDirectionLogicAction(TripleAttackSpeed));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.008333334f));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0833333358f));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.Tag = 2;
            var logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet8.AddAction(new DelayLogicAction(SlashDelay));
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, data));
            logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet8.Tag = 2;
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    SetCooldownLogicBlock(m_generalCooldownLB, 14, 11, 75);
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                case GameTypes.EnemyDifficulty.Expert:
                    SetCooldownLogicBlock(m_generalCooldownLB, 40, 30, 30);
                    break;
                case GameTypes.EnemyDifficulty.MiniBoss:
                {
                    var arg_9C4_1 = m_generalCooldownLB;
                    var array = new int[3];
                    array[0] = 100;
                    SetCooldownLogicBlock(arg_9C4_1, array);
                    break;
                }
            }
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_73_1 = true;
                    var arg_73_2 = m_generalBasicLB;
                    var array = new int[4];
                    array[2] = 100;
                    RunLogicBlock(arg_73_1, arg_73_2, array);
                    return;
                }
                case 1:
                case 2:
                {
                    var arg_58_1 = true;
                    var arg_58_2 = m_generalBasicLB;
                    var array2 = new int[4];
                    array2[0] = 15;
                    array2[1] = 15;
                    array2[2] = 70;
                    RunLogicBlock(arg_58_1, arg_58_2, array2);
                    return;
                }
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 0, 100);
                    return;
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
                    var arg_78_1 = true;
                    var arg_78_2 = m_generalAdvancedLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_78_1, arg_78_2, array);
                    return;
                }
                case 1:
                case 2:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalAdvancedLB;
                    var array2 = new int[5];
                    array2[0] = 60;
                    array2[1] = 20;
                    array2[2] = 20;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 0, 0, 65, 35);
                    return;
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
                    var arg_78_1 = true;
                    var arg_78_2 = m_generalBasicLB;
                    var array = new int[4];
                    array[2] = 100;
                    RunLogicBlock(arg_78_1, arg_78_2, array);
                    return;
                }
                case 1:
                case 2:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalBasicLB;
                    var array2 = new int[4];
                    array2[0] = 60;
                    array2[1] = 20;
                    array2[2] = 20;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 3:
                    RunLogicBlock(true, m_generalExpertLB, 0, 0, 0, 62, 38);
                    return;
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
                {
                    var arg_33_1 = true;
                    var arg_33_2 = m_generalBasicLB;
                    var array = new int[4];
                    array[0] = 100;
                    RunLogicBlock(arg_33_1, arg_33_2, array);
                    return;
                }
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (SpriteName == "EnemySwordKnightWalk_Character")
            {
                m_walkSound.Update();
                m_walkSound2.Update();
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}