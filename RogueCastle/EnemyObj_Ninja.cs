// 
// RogueLegacyArchipelago - EnemyObj_Ninja.cs
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
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class EnemyObj_Ninja : EnemyObj
    {
        private readonly LogicBlock m_basicTeleportAttackLB = new LogicBlock();
        private readonly LogicBlock m_expertTeleportAttackLB = new LogicBlock();
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly float m_teleportDamageReduc = 0.6f;
        private readonly float PauseAfterProjectile = 0.45f;
        private readonly float PauseBeforeProjectile = 0.45f;
        private readonly float TeleportDelay = 0.35f;
        private float ChanceToTeleport = 0.35f;
        private TerrainObj m_closestCeiling;
        private SpriteObj m_log;
        private SpriteObj m_smoke;
        private RoomObj m_storedRoom;

        public EnemyObj_Ninja(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyNinjaIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 13;
            m_smoke = new SpriteObj("NinjaSmoke_Sprite");
            m_smoke.AnimationDelay = 0.05f;
            m_log = new SpriteObj("Log_Sprite");
            m_smoke.Visible = false;
            m_smoke.Scale = new Vector2(5f, 5f);
            m_log.Visible = false;
            m_log.OutlineWidth = 2;
        }

        protected override void InitializeEV()
        {
            Name = "Ninjo";
            MaxHealth = 30;
            Damage = 20;
            XPValue = 150;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 250f;
            TurnSpeed = 10f;
            ProjectileSpeed = 550f;
            JumpHeight = 600f;
            CooldownTime = 1.5f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Ninja_Basic_Scale;
            ProjectileScale = EnemyEV.Ninja_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Ninja_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 700;
            EngageRadius = 1000;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Ninja_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;
                case EnemyDifficulty.Advanced:
                    ChanceToTeleport = 0.5f;
                    Name = "Ninpo";
                    MaxHealth = 44;
                    Damage = 25;
                    XPValue = 250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 325f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 625f;
                    JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Ninja_Advanced_Scale;
                    ProjectileScale = EnemyEV.Ninja_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 1000;
                    ProjectileRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Ninja_Advanced_KnockBack;
                    break;
                case EnemyDifficulty.Expert:
                    ChanceToTeleport = 0.65f;
                    Name = "Ninopojo";
                    MaxHealth = 62;
                    Damage = 29;
                    XPValue = 450;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 400f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 700f;
                    JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Ninja_Expert_Scale;
                    ProjectileScale = EnemyEV.Ninja_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 1000;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Ninja_Expert_KnockBack;
                    return;
                case EnemyDifficulty.MiniBoss:
                    Name = "Master Ninja";
                    MaxHealth = 900;
                    Damage = 38;
                    XPValue = 1250;
                    MinMoneyDropAmount = 10;
                    MaxMoneyDropAmount = 15;
                    MoneyDropChance = 1f;
                    Speed = 150f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 600f;
                    JumpHeight = 600f;
                    CooldownTime = 1.5f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Ninja_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Ninja_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Ninja_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 700;
                    EngageRadius = 1000;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Ninja_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character"));
            logicSet.AddAction(new PlayAnimationLogicAction());
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(0.25f, 0.85f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character"));
            logicSet2.AddAction(new PlayAnimationLogicAction());
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.25f, 0.85f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character"));
            logicSet3.AddAction(new StopAnimationLogicAction());
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.25f, 0.85f));
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "ShurikenProjectile1_Sprite",
                SourceAnchor = new Vector2(15f, 0f),
                Target = m_target,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 20f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = true,
                Scale = ProjectileScale
            };
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character"));
            logicSet4.AddAction(new DelayLogicAction(PauseBeforeProjectile));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false));
            logicSet4.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02", "Ninja_ThrowStar_03"));
            logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet4.AddAction(new PlayAnimationLogicAction(4, 5));
            logicSet4.AddAction(new DelayLogicAction(PauseAfterProjectile));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character"));
            logicSet5.AddAction(new DelayLogicAction(PauseBeforeProjectile));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false));
            logicSet5.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02", "Ninja_ThrowStar_03"));
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = -10f;
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = 10f;
            logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet5.AddAction(new PlayAnimationLogicAction(4, 5));
            logicSet5.AddAction(new DelayLogicAction(PauseAfterProjectile));
            logicSet5.Tag = 2;
            var logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character"));
            logicSet6.AddAction(new DelayLogicAction(PauseBeforeProjectile));
            logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false));
            logicSet6.AddAction(new PlayAnimationLogicAction(1, 3));
            logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02", "Ninja_ThrowStar_03"));
            projectileData.AngleOffset = 0f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = -5f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = 5f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = -25f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = 25f;
            logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet6.AddAction(new PlayAnimationLogicAction(4, 5));
            logicSet6.AddAction(new DelayLogicAction(PauseAfterProjectile));
            logicSet6.Tag = 2;
            var logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateLog", null));
            logicSet7.AddAction(new DelayLogicAction(TeleportDelay));
            logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null));
            logicSet7.AddAction(new DelayLogicAction(0.15f));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true));
            logicSet7.AddAction(new DelayLogicAction(0.15f));
            logicSet7.AddAction(new GroundCheckLogicAction());
            logicSet7.AddAction(new DelayLogicAction(0.15f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.Tag = 2;
            var logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateLog", null));
            logicSet8.AddAction(new DelayLogicAction(TeleportDelay));
            logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null));
            projectileData.Target = null;
            logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Ninja_ThrowStar_01",
                "Ninja_ThrowStar_02", "Ninja_ThrowStar_03"));
            projectileData.AngleOffset = 45f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = 135f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = -45f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.AngleOffset = -135f;
            logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet8.AddAction(new DelayLogicAction(0.15f));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true));
            logicSet8.AddAction(new DelayLogicAction(0.15f));
            logicSet8.AddAction(new GroundCheckLogicAction());
            logicSet8.AddAction(new DelayLogicAction(0.15f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet8.Tag = 2;
            m_basicTeleportAttackLB.AddLogicSet(logicSet7);
            m_expertTeleportAttackLB.AddLogicSet(logicSet8);
            logicBlocksToDispose.Add(m_basicTeleportAttackLB);
            logicBlocksToDispose.Add(m_expertTeleportAttackLB);
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet6);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            var arg_906_1 = m_generalCooldownLB;
            var array = new int[3];
            array[0] = 50;
            array[1] = 50;
            SetCooldownLogicBlock(arg_906_1, array);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                {
                    var arg_7D_1 = true;
                    var arg_7D_2 = m_generalBasicLB;
                    var array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalBasicLB;
                    var array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 40, 30, 0, 30);
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
                    var arg_7D_1 = true;
                    var arg_7D_2 = m_generalAdvancedLB;
                    var array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalAdvancedLB;
                    var array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 40, 30, 0, 30);
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
                    var arg_7D_1 = true;
                    var arg_7D_2 = m_generalExpertLB;
                    var array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalExpertLB;
                    var array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalExpertLB, 40, 30, 0, 30);
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
                {
                    var arg_7D_1 = true;
                    var arg_7D_2 = m_generalBasicLB;
                    var array = new int[4];
                    array[0] = 50;
                    array[1] = 50;
                    RunLogicBlock(arg_7D_1, arg_7D_2, array);
                    return;
                }
                case 1:
                {
                    var arg_5D_1 = true;
                    var arg_5D_2 = m_generalBasicLB;
                    var array2 = new int[4];
                    array2[0] = 65;
                    array2[1] = 35;
                    RunLogicBlock(arg_5D_1, arg_5D_2, array2);
                    return;
                }
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 40, 30, 0, 30);
                    return;
                default:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Y < m_levelScreen.CurrentRoom.Y)
            {
                Y = m_levelScreen.CurrentRoom.Y;
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            if (m_target != null && m_target.CurrentHealth > 0 && m_currentActiveLB != m_basicTeleportAttackLB &&
                m_currentActiveLB != m_expertTeleportAttackLB && CDGMath.RandomFloat(0f, 1f) <= ChanceToTeleport &&
                m_closestCeiling != null)
            {
                m_closestCeiling = FindClosestCeiling();
                var num = TerrainBounds.Top - m_closestCeiling.Bounds.Bottom;
                if (m_closestCeiling != null && num > 150 && num < 700)
                {
                    m_currentActiveLB.StopLogicBlock();
                    if (Difficulty == EnemyDifficulty.Expert)
                    {
                        RunLogicBlock(false, m_expertTeleportAttackLB, 100);
                    }
                    else
                    {
                        RunLogicBlock(false, m_basicTeleportAttackLB, 100);
                    }
                    damage = (int) Math.Round(damage*(1f - m_teleportDamageReduc), MidpointRounding.AwayFromZero);
                }
            }
            base.HitEnemy(damage, position, isPlayer);
        }

        private TerrainObj FindClosestCeiling()
        {
            var num = 2147483647;
            TerrainObj result = null;
            var currentRoom = m_levelScreen.CurrentRoom;
            foreach (var current in currentRoom.TerrainObjList)
            {
                var b = new Rectangle(Bounds.Left, Bounds.Top - 2000, Bounds.Width, Bounds.Height + 2000);
                if (current.CollidesBottom && CollisionMath.Intersects(current.Bounds, b))
                {
                    var num2 = 3.40282347E+38f;
                    if (current.Bounds.Bottom < TerrainBounds.Top)
                    {
                        num2 = TerrainBounds.Top - current.Bounds.Bottom;
                    }
                    if (num2 < num)
                    {
                        num = (int) num2;
                        result = current;
                    }
                }
            }
            return result;
        }

        public void CreateLog()
        {
            m_log.Position = Position;
            m_smoke.Position = Position;
            m_smoke.Visible = true;
            m_log.Visible = true;
            m_log.Opacity = 1f;
            m_smoke.PlayAnimation(false);
            Tween.By(m_log, 0.1f, Linear.EaseNone, "delay", "0.2", "Y", "10");
            Tween.To(m_log, 0.2f, Linear.EaseNone, "delay", "0.3", "Opacity", "0");
            SoundManager.Play3DSound(this, m_target, "Ninja_Teleport");
            Visible = false;
            IsCollidable = false;
            IsWeighted = false;
            m_storedRoom = m_levelScreen.CurrentRoom;
        }

        public void CreateSmoke()
        {
            if (m_levelScreen.CurrentRoom == m_storedRoom && m_closestCeiling != null)
            {
                UpdateCollisionBoxes();
                Y = m_closestCeiling.Bounds.Bottom + (Y - TerrainBounds.Top);
                X = m_target.X;
                ChangeSprite("EnemyNinjaAttack_Character");
                Visible = true;
                AccelerationX = 0f;
                AccelerationY = 0f;
                CurrentSpeed = 0f;
                IsCollidable = true;
                m_smoke.Position = Position;
                m_smoke.Visible = true;
                m_smoke.PlayAnimation(false);
                m_closestCeiling = null;
            }
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            m_log.Draw(camera);
            m_smoke.Draw(camera);
        }

        public override void Kill(bool giveXP = true)
        {
            m_smoke.Visible = false;
            m_log.Visible = false;
            base.Kill(giveXP);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_storedRoom = null;
                m_smoke.Dispose();
                m_smoke = null;
                m_log.Dispose();
                m_log = null;
                m_closestCeiling = null;
                base.Dispose();
            }
        }
    }
}
