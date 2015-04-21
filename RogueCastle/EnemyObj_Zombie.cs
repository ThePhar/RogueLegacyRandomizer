/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public class EnemyObj_Zombie : EnemyObj
    {
        private LogicBlock m_basicWalkLS = new LogicBlock();
        private LogicBlock m_basicRiseLowerLS = new LogicBlock();
        public bool Risen { get; set; }
        public bool Lowered { get; set; }

        protected override void InitializeEV()
        {
            Name = "Zombie";
            MaxHealth = 24;
            Damage = 25;
            XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            Speed = 160f;
            TurnSpeed = 10f;
            ProjectileSpeed = 650f;
            JumpHeight = 900f;
            CooldownTime = 2f;
            AnimationDelay = 0.0833333358f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Zombie_Basic_Scale;
            ProjectileScale = EnemyEV.Zombie_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Zombie_Basic_Tint;
            MeleeRadius = 100;
            ProjectileRadius = 150;
            EngageRadius = 435;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Zombie_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    Name = "Zomboner";
                    MaxHealth = 39;
                    Damage = 29;
                    XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 260f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 900f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0714285746f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Zombie_Advanced_Scale;
                    ProjectileScale = EnemyEV.Zombie_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Advanced_Tint;
                    MeleeRadius = 100;
                    EngageRadius = 435;
                    ProjectileRadius = 150;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Zombie_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    Name = "Zombishnu";
                    MaxHealth = 70;
                    Damage = 33;
                    XPValue = 200;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 350f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 900f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0625f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = true;
                    Scale = EnemyEV.Zombie_Expert_Scale;
                    ProjectileScale = EnemyEV.Zombie_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Expert_Tint;
                    MeleeRadius = 100;
                    ProjectileRadius = 150;
                    EngageRadius = 435;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Zombie_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    Name = "Zomg";
                    MaxHealth = 800;
                    Damage = 40;
                    XPValue = 600;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 1f;
                    Speed = 600f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 900f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0714285746f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = true;
                    Scale = EnemyEV.Zombie_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Zombie_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Zombie_Miniboss_Tint;
                    MeleeRadius = 100;
                    ProjectileRadius = 150;
                    EngageRadius = 435;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Zombie_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyZombieWalk_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Zombie_Groan_01",
                "Zombie_Groan_02", "Zombie_Groan_03", "Blank", "Blank", "Blank", "Blank", "Blank"));
            logicSet.AddAction(new DelayLogicAction(0.5f));
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet2.AddAction(new MoveLogicAction(m_target, false, 0f));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyZombieRise_Character", false, false));
            logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Zombie_Rise"));
            logicSet2.AddAction(new PlayAnimationLogicAction(false));
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Risen", true));
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Lowered", false));
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet3.AddAction(new MoveLogicAction(m_target, false, 0f));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyZombieLower_Character", false, false));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Zombie_Lower"));
            logicSet3.AddAction(new PlayAnimationLogicAction(false));
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "Risen", false));
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "Lowered", true));
            m_basicWalkLS.AddLogicSet(logicSet);
            m_basicRiseLowerLS.AddLogicSet(logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_basicWalkLS);
            logicBlocksToDispose.Add(m_basicRiseLowerLS);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                    if (!Lowered)
                    {
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100);
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen)
                    {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    RunLogicBlock(false, m_basicWalkLS, 100);
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
                    if (!Lowered)
                    {
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100);
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen)
                    {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    RunLogicBlock(false, m_basicWalkLS, 100);
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
                    if (!Lowered)
                    {
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100);
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen)
                    {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    RunLogicBlock(false, m_basicWalkLS, 100);
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
                    if (!Lowered)
                    {
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100);
                    }
                    return;
                case 1:
                case 2:
                case 3:
                    if (!Risen)
                    {
                        bool arg_3B_1 = false;
                        LogicBlock arg_3B_2 = m_basicRiseLowerLS;
                        int[] array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_3B_1, arg_3B_2, array);
                        return;
                    }
                    RunLogicBlock(false, m_basicWalkLS, 100);
                    return;
                default:
                    return;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Zombie_Hit");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Update(GameTime gameTime)
        {
            if ((m_currentActiveLB == null || !m_currentActiveLB.IsActive) && !Risen && IsAnimating)
            {
                ChangeSprite("EnemyZombieRise_Character");
                StopAnimation();
            }
            base.Update(gameTime);
        }

        public override void ResetState()
        {
            Lowered = true;
            Risen = false;
            base.ResetState();
            ChangeSprite("EnemyZombieLower_Character");
            GoToFrame(TotalFrames);
            StopAnimation();
        }

        public override void Reset()
        {
            ChangeSprite("EnemyZombieRise_Character");
            StopAnimation();
            Lowered = true;
            Risen = false;
            base.Reset();
        }

        public EnemyObj_Zombie(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyZombieLower_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            GoToFrame(TotalFrames);
            Lowered = true;
            ForceDraw = true;
            StopAnimation();
            Type = 20;
            PlayAnimationOnRestart = false;
        }
    }
}