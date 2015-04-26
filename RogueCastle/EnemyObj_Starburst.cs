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
    public class EnemyObj_Starburst : EnemyObj
    {
        private readonly float FireballDelay = 0.5f;
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Starburst(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyStarburstIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 31;
        }

        protected override void InitializeEV()
        {
            Name = "Plinky";
            MaxHealth = 18;
            Damage = 15;
            XPValue = 50;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            Speed = 435f;
            TurnSpeed = 10f;
            ProjectileSpeed = 435f;
            JumpHeight = 950f;
            CooldownTime = 0f;
            AnimationDelay = 0.05f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            CanBeKnockedBack = false;
            IsWeighted = false;
            Scale = EnemyEV.Starburst_Basic_Scale;
            ProjectileScale = EnemyEV.Starburst_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Starburst_Basic_Tint;
            MeleeRadius = 325;
            ProjectileRadius = 690;
            EngageRadius = 850;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Starburst_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.BASIC:
                    break;
                case GameTypes.EnemyDifficulty.ADVANCED:
                    Name = "Planky";
                    MaxHealth = 25;
                    Damage = 18;
                    XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 435f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 435f;
                    JumpHeight = 950f;
                    CooldownTime = 0f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Starburst_Advanced_Scale;
                    ProjectileScale = EnemyEV.Starburst_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Advanced_Tint;
                    MeleeRadius = 325;
                    EngageRadius = 850;
                    ProjectileRadius = 690;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Starburst_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.EXPERT:
                    Name = "Plonky";
                    MaxHealth = 42;
                    Damage = 21;
                    XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 435f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 435f;
                    JumpHeight = 950f;
                    CooldownTime = 0f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Starburst_Expert_Scale;
                    ProjectileScale = EnemyEV.Starburst_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Expert_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Starburst_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    Name = "Ploo";
                    MaxHealth = 750;
                    Damage = 30;
                    XPValue = 1100;
                    MinMoneyDropAmount = 8;
                    MaxMoneyDropAmount = 16;
                    MoneyDropChance = 1f;
                    Speed = 435f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 370f;
                    JumpHeight = 1350f;
                    CooldownTime = 0f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Starburst_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Starburst_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Starburst_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Starburst_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "TurretProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = ProjectileScale
            };
            var logicSet = new LogicSet(this);
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            logicSet.AddAction(new DelayLogicAction(FireballDelay));
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Eyeball_ProjectileAttack"));
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character"));
            logicSet.AddAction(new DelayLogicAction(1f, 1f));
            logicSet.Tag = 2;
            var logicSet2 = new LogicSet(this);
            projectileData.Angle = new Vector2(45f, 45f);
            logicSet2.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet2.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            logicSet2.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet2.AddAction(new DelayLogicAction(FireballDelay));
            logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Eyeball_ProjectileAttack"));
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-45f, -45f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(135f, 135f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-135f, -135f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character"));
            logicSet2.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet2.AddAction(new DelayLogicAction(1f, 1f));
            logicSet2.Tag = 2;
            var logicSet3 = new LogicSet(this);
            projectileData.Angle = new Vector2(45f, 45f);
            projectileData.CollidesWithTerrain = false;
            projectileData.SpriteName = "GhostProjectile_Sprite";
            logicSet3.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            logicSet3.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet3.AddAction(new DelayLogicAction(FireballDelay));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Eyeball_ProjectileAttack"));
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-45f, -45f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(135f, 135f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-135f, -135f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(90f, 90f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(180f, 180f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(0f, 0f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character"));
            logicSet3.AddAction(new DelayLogicAction(1f, 1f));
            logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Eyeball_ProjectileAttack"));
            projectileData.Angle = new Vector2(25f, 25f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-25f, -25f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(115f, 115f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-115f, -115f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-70f, -70f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(70f, 70f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(160f, 160f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-160f, -160f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character"));
            logicSet3.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            logicSet3.AddAction(new DelayLogicAction(1.25f, 1.25f));
            logicSet3.Tag = 2;
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new DelayLogicAction(0.5f, 0.5f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet4);
            m_generalAdvancedLB.AddLogicSet(logicSet2, logicSet4);
            m_generalExpertLB.AddLogicSet(logicSet3, logicSet4);
            m_generalMiniBossLB.AddLogicSet(logicSet2, logicSet4);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        public void FireAnimation()
        {
            ChangeSprite("EnemyStarburstAttack_Character");
            (_objectList[0] as IAnimateableObj).PlayAnimation();
            (_objectList[1] as IAnimateableObj).PlayAnimation(false);
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                    RunLogicBlock(true, m_generalBasicLB, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                {
                    var arg_33_1 = true;
                    var arg_33_2 = m_generalBasicLB;
                    var array = new int[2];
                    array[0] = 100;
                    RunLogicBlock(arg_33_1, arg_33_2, array);
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
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                {
                    var arg_33_1 = true;
                    var arg_33_2 = m_generalAdvancedLB;
                    var array = new int[2];
                    array[0] = 100;
                    RunLogicBlock(arg_33_1, arg_33_2, array);
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
                    RunLogicBlock(true, m_generalExpertLB, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                {
                    var arg_33_1 = true;
                    var arg_33_2 = m_generalExpertLB;
                    var array = new int[2];
                    array[0] = 100;
                    RunLogicBlock(arg_33_1, arg_33_2, array);
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
                {
                    var arg_38_1 = true;
                    var arg_38_2 = m_generalMiniBossLB;
                    var array = new int[3];
                    array[0] = 60;
                    array[1] = 40;
                    RunLogicBlock(arg_38_1, arg_38_2, array);
                    return;
                }
                default:
                    return;
            }
        }
    }
}