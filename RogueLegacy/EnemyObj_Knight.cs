//  RogueLegacyRandomizer - EnemyObj_Knight.cs
//  Last Modified 2023-10-26 12:01 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class EnemyObj_Knight : EnemyObj
    {
        private readonly float AttackProjectileDelay = 0.35f;
        private readonly float AttackProjectileExpertDelay = 0.425f;
        private readonly float AttackProjectileMinibossDelay = 0.5f;
        private readonly float AttackThrustDelay = 0.65f;
        private readonly float AttackThrustDelayExpert = 0.65f;
        private readonly float AttackThrustDelayMiniBoss = 0.65f;
        private readonly float AttackThrustDuration = 0.4f;
        private readonly float AttackThrustDurationExpert = 0.25f;
        private readonly float AttackThrustDurationMiniBoss = 0.25f;
        private readonly float AttackThrustSpeed = 1850f;
        private readonly float AttackThrustSpeedExpert = 1750f;
        private readonly float AttackThrustSpeedMiniBoss = 2300f;
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;

        public EnemyObj_Knight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemySpearKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            TintablePart = _objectList[1];
            Type = 12;
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        protected override void InitializeEV()
        {
            Name = "Corrupt Knight";
            MaxHealth = 40;
            Damage = 27;
            XPValue = 125;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 75f;
            TurnSpeed = 10f;
            ProjectileSpeed = 860f;
            JumpHeight = 950f;
            CooldownTime = 0.75f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Knight_Basic_Scale;
            ProjectileScale = EnemyEV.Knight_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Knight_Basic_Tint;
            MeleeRadius = 325;
            ProjectileRadius = 690;
            EngageRadius = 850;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Knight_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;

                case EnemyDifficulty.Advanced:
                    Name = "Corrupt Vanguard";
                    MaxHealth = 58;
                    Damage = 32;
                    XPValue = 185;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 75f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 860f;
                    JumpHeight = 950f;
                    CooldownTime = 0.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Knight_Advanced_Scale;
                    ProjectileScale = EnemyEV.Knight_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Advanced_Tint;
                    MeleeRadius = 325;
                    EngageRadius = 850;
                    ProjectileRadius = 690;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Knight_Advanced_KnockBack;
                    break;

                case EnemyDifficulty.Expert:
                    Name = "Corrupt Lord";
                    MaxHealth = 79;
                    Damage = 36;
                    XPValue = 250;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 125f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 780f;
                    JumpHeight = 950f;
                    CooldownTime = 0.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Knight_Expert_Scale;
                    ProjectileScale = EnemyEV.Knight_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Expert_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Knight_Expert_KnockBack;
                    return;

                case EnemyDifficulty.MiniBoss:
                    Name = "Botis";
                    MaxHealth = 265;
                    Damage = 40;
                    XPValue = 1250;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 18;
                    MoneyDropChance = 1f;
                    Speed = 200f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 780f;
                    JumpHeight = 1350f;
                    CooldownTime = 0.65f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Knight_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Knight_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Knight_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Knight_Miniboss_KnockBack;
                    return;

                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightWalk_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(0.2f, 1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightWalk_Character"));
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.2f, 1f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.2f, 1f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character"));
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet4.AddAction(new DelayLogicAction(AttackThrustDelay));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet4.AddAction(new MoveDirectionLogicAction(AttackThrustSpeed));
            logicSet4.AddAction(new RunFunctionLogicAction(_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect",
                this, 20, 0.3f));
            logicSet4.AddAction(new PlayAnimationLogicAction("AttackStart", "End"), Types.Sequence.Parallel);
            logicSet4.AddAction(new DelayLogicAction(AttackThrustDuration));
            logicSet4.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet4.AddAction(new DelayLogicAction(0.3f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet5.AddAction(new DelayLogicAction(AttackThrustDelayExpert));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet5.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedExpert));
            logicSet5.AddAction(new RunFunctionLogicAction(_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect",
                this, 20, 0.3f));
            logicSet5.AddAction(new PlayAnimationLogicAction("AttackStart", "End"), Types.Sequence.Parallel);
            logicSet5.AddAction(new DelayLogicAction(AttackThrustDurationExpert));
            logicSet5.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet5.AddAction(new DelayLogicAction(0.3f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character"));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDelayMiniBoss));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss));
            logicSet6.AddAction(new RunFunctionLogicAction(_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect",
                this, 20, 0.3f));
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End"), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss));
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character"));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet6.AddAction(new DelayLogicAction(0.25f));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss));
            logicSet6.AddAction(new RunFunctionLogicAction(_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect",
                this, 20, 0.3f));
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End"), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss));
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack_Character"));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet6.AddAction(new DelayLogicAction(0.25f));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet6.AddAction(new MoveDirectionLogicAction(AttackThrustSpeedMiniBoss));
            logicSet6.AddAction(new RunFunctionLogicAction(_levelScreen.ImpactEffectPool, "DisplayThrustDustEffect",
                this, 20, 0.3f));
            logicSet6.AddAction(new PlayAnimationLogicAction("AttackStart", "End"), Types.Sequence.Parallel);
            logicSet6.AddAction(new DelayLogicAction(AttackThrustDurationMiniBoss));
            logicSet6.AddAction(new MoveLogicAction(null, true, 0f));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet6.AddAction(new DelayLogicAction(0.3f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.Tag = 2;
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                Scale = ProjectileScale
            };
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet7.AddAction(new DelayLogicAction(AttackProjectileDelay));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnight_Projectile"));
            logicSet7.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new DelayLogicAction(0.3f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.Tag = 2;
            var logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet8.AddAction(new DelayLogicAction(AttackProjectileExpertDelay));
            ThrowThreeProjectiles(logicSet8);
            logicSet8.AddAction(new DelayLogicAction(0.3f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet8.Tag = 2;
            var logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet9.AddAction(new DelayLogicAction(AttackProjectileMinibossDelay));
            logicSet9.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnight_Projectile"));
            logicSet9.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            ThrowTwoProjectiles(logicSet9);
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet9.AddAction(new DelayLogicAction(0.05f));
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.05f));
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet9.AddAction(new DelayLogicAction(0.05f));
            ThrowTwoProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.5f));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet9.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet9.Tag = 2;
            var logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet10.AddAction(new DelayLogicAction(AttackProjectileMinibossDelay));
            logicSet10.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnight_Projectile"));
            logicSet10.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet10.AddAction(new DelayLogicAction(0.05f));
            ThrowTwoProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.05f));
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightAttack2_Character"));
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet10.AddAction(new DelayLogicAction(0.05f));
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.5f));
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet10.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet10.Tag = 2;
            var logicSet11 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySpearKnightIdle_Character"));
            logicSet11.AddAction(new MoveLogicAction(m_target, false, 300f));
            logicSet11.AddAction(new JumpLogicAction());
            logicSet11.AddAction(new DelayLogicAction(0.3f));
            logicSet11.AddAction(new GroundCheckLogicAction());
            logicSet11.AddAction(new JumpLogicAction());
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            logicSet11.AddAction(new GroundCheckLogicAction());
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet11.AddAction(new DelayLogicAction(0.1f));
            logicSet11.AddAction(new JumpLogicAction());
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            ThrowRapidProjectiles(logicSet11);
            logicSet11.AddAction(new GroundCheckLogicAction());
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet11.Tag = 2;
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet7);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5, logicSet8);
            m_generalMiniBossLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet6, logicSet9, logicSet10);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 55, 25, 20);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                Scale = ProjectileScale
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnight_Projectile"));
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(45f, 45f);
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-45f, -45f);
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            ls.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            projectileData.Dispose();
        }

        private void ThrowTwoProjectiles(LogicSet ls)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(30f, 0f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(22f, 22f)
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-22f, -22f);
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            ls.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            projectileData.Dispose();
        }

        private void ThrowRapidProjectiles(LogicSet ls)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(130f, -28f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = Vector2.Zero
            };
            ls.AddAction(new DelayLogicAction(0.2f, 0.35f));
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            ls.AddAction(new DelayLogicAction(0.2f, 0.35f));
            projectileData.SourceAnchor = new Vector2(130f, 28f);
            ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
            projectileData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_AA_1 = true;
                    var arg_AA_2 = m_generalBasicLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_AA_1, arg_AA_2, array);
                    return;
                }

                case 1:
                    RunLogicBlock(true, m_generalBasicLB, 55, 30, 0, 0, 15);
                    return;

                case 2:
                    RunLogicBlock(true, m_generalBasicLB, 20, 20, 10, 0, 50);
                    return;

                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 20, 15, 0, 0, 65);
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
                    var arg_B4_1 = true;
                    var arg_B4_2 = m_generalAdvancedLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_B4_1, arg_B4_2, array);
                    return;
                }

                case 1:
                    RunLogicBlock(true, m_generalAdvancedLB, 55, 30, 0, 0, 15);
                    return;

                case 2:
                    RunLogicBlock(true, m_generalAdvancedLB, 15, 15, 10, 15, 45);
                    return;

                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 15, 10, 0, 60, 15);
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
                    var arg_B4_1 = true;
                    var arg_B4_2 = m_generalExpertLB;
                    var array = new int[5];
                    array[2] = 100;
                    RunLogicBlock(arg_B4_1, arg_B4_2, array);
                    return;
                }

                case 1:
                    RunLogicBlock(true, m_generalExpertLB, 55, 30, 0, 0, 15);
                    return;

                case 2:
                    RunLogicBlock(true, m_generalExpertLB, 15, 15, 10, 15, 45);
                    return;

                case 3:
                    RunLogicBlock(true, m_generalExpertLB, 15, 10, 0, 60, 15);
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
                    RunLogicBlock(true, m_generalMiniBossLB, 14, 13, 11, 26, 18, 18);
                    return;

                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (SpriteName == "EnemySpearKnightWalk_Character")
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
