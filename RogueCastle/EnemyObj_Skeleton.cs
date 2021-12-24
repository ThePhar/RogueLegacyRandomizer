// 
// RogueLegacyArchipelago - EnemyObj_Skeleton.cs
// Last Modified 2021-12-24
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
using RogueCastle.TypeDefinitions;

namespace RogueCastle
{
    public class EnemyObj_Skeleton : EnemyObj
    {
        private readonly float AttackDelay = 0.1f;
        private readonly float JumpDelay = 0.25f;
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Skeleton(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemySkeletonIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 15;
        }

        protected override void InitializeEV()
        {
            Name = "Skeleton";
            MaxHealth = 27;
            Damage = 20;
            XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 80f;
            TurnSpeed = 10f;
            ProjectileSpeed = 1040f;
            JumpHeight = 925f;
            CooldownTime = 0.75f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Skeleton_Basic_Scale;
            ProjectileScale = EnemyEV.Skeleton_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Skeleton_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 500;
            EngageRadius = 700;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Skeleton_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                    Name = "Mr Bones";
                    MaxHealth = 36;
                    Damage = 26;
                    XPValue = 150;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 80f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    JumpHeight = 925f;
                    CooldownTime = 0.45f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Skeleton_Advanced_Scale;
                    ProjectileScale = EnemyEV.Skeleton_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 700;
                    ProjectileRadius = 500;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Skeleton_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    Name = "McRib";
                    MaxHealth = 68;
                    Damage = 28;
                    XPValue = 200;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 140f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    JumpHeight = 925f;
                    CooldownTime = 0.4f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Skeleton_Expert_Scale;
                    ProjectileScale = EnemyEV.Skeleton_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Skeleton_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    Name = "Berith & Halphas";
                    MaxHealth = 255;
                    Damage = 32;
                    XPValue = 1000;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 18;
                    MoneyDropChance = 1f;
                    Speed = 60f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1040f;
                    JumpHeight = 925f;
                    CooldownTime = 0.15f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = true;
                    Scale = EnemyEV.Skeleton_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Skeleton_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Skeleton_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Skeleton_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-72f, -72f),
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character"));
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new StopAnimationLogicAction());
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new DelayLogicAction(0.5f, 1f));
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet5.AddAction(new DelayLogicAction(AttackDelay));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet5.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet6.AddAction(new DelayLogicAction(AttackDelay));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projectileData.Angle = new Vector2(-85f, -85f);
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet6.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.Tag = 2;
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet7.AddAction(new DelayLogicAction(JumpDelay));
            logicSet7.AddAction(new JumpLogicAction());
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projectileData.Angle = new Vector2(-72f, -72f);
            logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet7.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.Tag = 2;
            var logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            logicSet8.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet8.AddAction(new DelayLogicAction(JumpDelay));
            logicSet8.AddAction(new JumpLogicAction());
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projectileData.Angle = new Vector2(-85f, -85f);
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet8.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet8.Tag = 2;
            var logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet9.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet9.AddAction(new DelayLogicAction(AttackDelay));
            logicSet9.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new PlayAnimationLogicAction("Attack", "End"), Types.Sequence.Parallel);
            logicSet9.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(logicSet9);
            logicSet9.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet9.Tag = 2;
            var logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            logicSet10.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet10.AddAction(new DelayLogicAction(JumpDelay));
            logicSet10.AddAction(new JumpLogicAction());
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet10.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End"), Types.Sequence.Parallel);
            logicSet10.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(logicSet10);
            logicSet10.AddAction(new DelayLogicAction(0.2f, 0.4f));
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet10.Tag = 2;
            var logicSet11 = new LogicSet(this);
            logicSet11.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet11.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet11.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet11.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet11.AddAction(new DelayLogicAction(AttackDelay));
            projectileData.Angle = new Vector2(-89f, -35f);
            projectileData.RotationSpeed = 8f;
            projectileData.SourceAnchor = new Vector2(5f, -20f);
            logicSet11.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            logicSet11.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet11.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet11.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet11.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet11.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character"));
            logicSet11.AddAction(new DelayLogicAction(0.4f, 0.9f));
            var logicSet12 = new LogicSet(this);
            logicSet12.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet12.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet12.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            logicSet12.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            logicSet12.AddAction(new DelayLogicAction(AttackDelay));
            logicSet12.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet12.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet12.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet12.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet12.AddAction(new DelayLogicAction(0.15f, 0.35f));
            logicSet12.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character"));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5, logicSet6);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5, logicSet6, logicSet7, logicSet8);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet9, logicSet10);
            m_generalMiniBossLB.AddLogicSet(logicSet, logicSet2, logicSet4, logicSet11, logicSet12);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 30, 30, 40);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-72f, -72f),
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Speed = new Vector2(ProjectileSpeed - 350f, ProjectileSpeed - 350f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Speed = new Vector2(ProjectileSpeed + 350f, ProjectileSpeed + 350f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                {
                    var arg_91_1 = true;
                    var arg_91_2 = m_generalBasicLB;
                    var array = new int[5];
                    array[0] = 40;
                    array[1] = 40;
                    array[2] = 20;
                    RunLogicBlock(arg_91_1, arg_91_2, array);
                    return;
                }
                case 2:
                    RunLogicBlock(true, m_generalBasicLB, 10, 10, 0, 40, 40);
                    return;
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 10, 10, 0, 30, 50);
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
                case 1:
                {
                    var arg_72_1 = true;
                    var arg_72_2 = m_generalAdvancedLB;
                    var array = new int[7];
                    array[0] = 40;
                    array[1] = 40;
                    array[2] = 20;
                    RunLogicBlock(arg_72_1, arg_72_2, array);
                    return;
                }
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 10, 10, 0, 15, 15, 25, 25);
                    return;
                default:
                    RunBasicLogic();
                    return;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                    RunLogicBlock(true, m_generalExpertLB, 35, 35, 0, 0, 15);
                    return;
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalExpertLB, 15, 15, 0, 35, 35);
                    return;
                default:
                    RunBasicLogic();
                    return;
            }
        }

        protected override void RunMinibossLogic()
        {
            /*switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:*/
            //IL_1D:
            if (m_levelScreen.CurrentRoom.ActiveEnemies > 1)
            {
                var arg_4A_1 = true;
                var arg_4A_2 = m_generalMiniBossLB;
                var array = new int[5];
                array[2] = 10;
                array[3] = 90;
                RunLogicBlock(arg_4A_1, arg_4A_2, array);
                return;
            }
            Console.WriteLine("RAGING");
            RunLogicBlock(true, m_generalMiniBossLB, 0, 0, 10, 0, 90);
            //return;
            //}
            //goto IL_1D;
        }

        public override void Update(GameTime gameTime)
        {
            if (Difficulty == GameTypes.EnemyDifficulty.MiniBoss && m_levelScreen.CurrentRoom.ActiveEnemies == 1)
            {
                TintablePart.TextureColor = new Color(185, 0, 15);
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
            base.HitEnemy(damage, position, isPlayer);
        }
    }
}