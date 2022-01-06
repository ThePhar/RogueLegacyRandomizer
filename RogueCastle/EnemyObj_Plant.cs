// 
// RogueLegacyArchipelago - EnemyObj_Plant.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public class EnemyObj_Plant : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Plant(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyPlantIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 22;
        }

        protected override void InitializeEV()
        {
            Name = "Bud";
            MaxHealth = 20;
            Damage = 20;
            XPValue = 25;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 125f;
            TurnSpeed = 10f;
            ProjectileSpeed = 900f;
            JumpHeight = 900f;
            CooldownTime = 2f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Plant_Basic_Scale;
            ProjectileScale = EnemyEV.Plant_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Plant_Basic_Tint;
            MeleeRadius = 325;
            ProjectileRadius = 690;
            EngageRadius = 850;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Plant_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Advanced:
                    Name = "Plantite";
                    MaxHealth = 28;
                    Damage = 23;
                    XPValue = 50;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 150f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1000f;
                    JumpHeight = 900f;
                    CooldownTime = 1.75f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Plant_Advanced_Scale;
                    ProjectileScale = EnemyEV.Plant_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Plant_Advanced_Tint;
                    MeleeRadius = 325;
                    EngageRadius = 850;
                    ProjectileRadius = 690;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Plant_Advanced_KnockBack;
                    break;

                case EnemyDifficulty.Expert:
                    Name = "Flowermon";
                    MaxHealth = 53;
                    Damage = 26;
                    XPValue = 175;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 200f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1000f;
                    JumpHeight = 900f;
                    CooldownTime = 1.25f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Plant_Expert_Scale;
                    ProjectileScale = EnemyEV.Plant_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Plant_Expert_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Plant_Expert_KnockBack;
                    break;

                case EnemyDifficulty.MiniBoss:
                    Name = "Stolas & Focalor";
                    MaxHealth = 165;
                    Damage = 28;
                    XPValue = 500;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 18;
                    MoneyDropChance = 1f;
                    Speed = 450f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 700f;
                    JumpHeight = 900f;
                    CooldownTime = 0.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Plant_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Plant_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Plant_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 690;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Plant_Miniboss_KnockBack;
                    break;
            }

            _objectList[1].TextureColor = new Color(201, 59, 136);
        }

        protected override void InitializeLogic()
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "PlantProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = ProjectileScale
            };
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Squirm_01",
                "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03"));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            logicSet.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1));
            logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Attack_01"));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-75f, -75f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-105f, -105f);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet.Tag = 2;
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            logicSet2.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1));
            logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Attack_01"));
            projectileData.Angle = new Vector2(-60f, -60f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-75f, -75f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-105f, -105f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-120f, -120f);
            logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet2.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet2.Tag = 2;
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            logicSet3.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Attack_01"));
            projectileData.Angle = new Vector2(-45f, -45f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-60f, -60f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-85f, -85f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-95f, -95f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-75f, -75f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-105f, -105f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-120f, -120f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-135f, -135f);
            logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet3.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet3.Tag = 2;
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            logicSet4.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Attack_01"));
            projectileData.Angle = new Vector2(-60f, -60f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-87f, -87f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-93f, -93f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-75f, -75f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-105f, -105f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-120f, -120f);
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet4.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new DelayLogicAction(0.25f));
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Squirm_01",
                "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            logicSet6.AddAction(new MoveLogicAction(m_target, true));
            logicSet6.AddAction(new DelayLogicAction(0.25f, 0.45f));
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Squirm_01",
                "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            logicSet7.AddAction(new MoveLogicAction(m_target, false));
            logicSet7.AddAction(new DelayLogicAction(0.25f, 0.45f));
            var logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Enemy_Venus_Squirm_01",
                "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character"));
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet8.AddAction(new DelayLogicAction(0.25f, 0.45f));
            m_generalBasicLB.AddLogicSet(logicSet);
            m_generalAdvancedLB.AddLogicSet(logicSet2);
            m_generalExpertLB.AddLogicSet(logicSet3);
            m_generalMiniBossLB.AddLogicSet(logicSet4);
            m_generalCooldownLB.AddLogicSet(logicSet5);
            m_generalCooldownExpertLB.AddLogicSet(logicSet6, logicSet7, logicSet8);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_generalCooldownExpertLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 100);
            if (Difficulty == EnemyDifficulty.MiniBoss)
            {
                var arg_AA5_1 = m_generalCooldownExpertLB;
                var array = new int[3];
                array[0] = 50;
                array[1] = 50;
                SetCooldownLogicBlock(arg_AA5_1, array);
            }

            projectileData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                    break;

                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 100);
                    break;

                default:
                    return;
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case 0:
                    break;

                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 100);
                    break;

                default:
                    return;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case 0:
                    break;

                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalExpertLB, 100);
                    break;

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
                    RunLogicBlock(true, m_generalMiniBossLB, 100);
                    return;

                default:
                    return;
            }
        }
    }
}