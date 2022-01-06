// 
// RogueLegacyArchipelago - EnemyObj_Portrait.cs
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
using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public class EnemyObj_Portrait : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Portrait(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyPortrait_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 32;
            var spriteName = "FramePicture" + CDGMath.RandomInt(1, 16) + "_Sprite";
            GetChildAt(0).ChangeSprite(spriteName);
            GetChildAt(0);
            DisableCollisionBoxRotations = false;
        }

        public bool Shake { get; set; }
        public bool Chasing { get; set; }

        protected override void InitializeEV()
        {
            LockFlip = true;
            Name = "Doomvas";
            MaxHealth = 35;
            Damage = 21;
            XPValue = 50;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 500f;
            TurnSpeed = 0.03f;
            ProjectileSpeed = 1020f;
            JumpHeight = 600f;
            CooldownTime = 2f;
            AnimationDelay = 0.0166666675f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            CanBeKnockedBack = false;
            IsWeighted = false;
            Scale = EnemyEV.Portrait_Basic_Scale;
            ProjectileScale = EnemyEV.Portrait_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Portrait_Basic_Tint;
            MeleeRadius = 25;
            ProjectileRadius = 100;
            EngageRadius = 300;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Portrait_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;

                case EnemyDifficulty.Advanced:
                    Name = "Doomtrait";
                    MaxHealth = 43;
                    Damage = 25;
                    XPValue = 75;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 550f;
                    TurnSpeed = 0.0425f;
                    ProjectileSpeed = 400f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0166666675f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Portrait_Advanced_Scale;
                    ProjectileScale = EnemyEV.Portrait_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Portrait_Advanced_Tint;
                    MeleeRadius = 25;
                    EngageRadius = 300;
                    ProjectileRadius = 100;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Portrait_Advanced_KnockBack;
                    break;

                case EnemyDifficulty.Expert:
                    Name = "Doomscape";
                    MaxHealth = 61;
                    Damage = 27;
                    XPValue = 100;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 1f;
                    Speed = 600f;
                    TurnSpeed = 0.03f;
                    ProjectileSpeed = 400f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0166666675f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Portrait_Expert_Scale;
                    ProjectileScale = EnemyEV.Portrait_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Portrait_Expert_Tint;
                    MeleeRadius = 25;
                    ProjectileRadius = 100;
                    EngageRadius = 300;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Portrait_Expert_KnockBack;
                    return;

                case EnemyDifficulty.MiniBoss:
                    Name = "Sallos";
                    MaxHealth = 215;
                    Damage = 28;
                    XPValue = 1000;
                    MinMoneyDropAmount = 11;
                    MaxMoneyDropAmount = 15;
                    MoneyDropChance = 1f;
                    Speed = 515f;
                    TurnSpeed = 0.03f;
                    ProjectileSpeed = 250f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0166666675f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Portrait_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Portrait_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Portrait_Miniboss_Tint;
                    MeleeRadius = 25;
                    ProjectileRadius = 100;
                    EngageRadius = 300;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Portrait_Miniboss_KnockBack;
                    return;

                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Shake", true));
            logicSet.AddAction(new DelayLogicAction(1f));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Shake", false));
            logicSet.AddAction(new DelayLogicAction(1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.75f));
            ThrowAdvancedProjectiles(logicSet3, true);
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.75f));
            ThrowExpertProjectiles(logicSet4, true);
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.25f));
            ThrowProjectiles(logicSet5, true);
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2);
            m_generalAdvancedLB.AddLogicSet(logicSet, logicSet3);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet4);
            m_generalMiniBossLB.AddLogicSet(logicSet, logicSet5);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            base.InitializeLogic();
            CollisionBoxes.Clear();
            CollisionBoxes.Add(new CollisionBox((int) (-18f * ScaleX), (int) (-24f * ScaleY), (int) (36f * ScaleX),
                (int) (48f * ScaleY), 2, this));
            CollisionBoxes.Add(new CollisionBox((int) (-15f * ScaleX), (int) (-21f * ScaleY), (int) (31f * ScaleX),
                (int) (44f * ScaleY), 1, this));
            if (Difficulty == EnemyDifficulty.MiniBoss)
            {
                (GetChildAt(0) as SpriteObj).ChangeSprite("GiantPortrait_Sprite");
                Scale = new Vector2(2f, 2f);
                AddChild(new SpriteObj("Portrait" + CDGMath.RandomInt(0, 7) + "_Sprite")
                {
                    OverrideParentScale = true
                });
                CollisionBoxes.Clear();
                CollisionBoxes.Add(new CollisionBox(-124, -176, 250, 354, 2, this));
                CollisionBoxes.Add(new CollisionBox(-124, -176, 250, 354, 1, this));
            }
        }

        private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
            projectileData.Angle = new Vector2(135f, 135f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(45f, 45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-45f, -45f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-135f, -135f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Dispose();
        }

        private void ThrowExpertProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
            projectileData.Angle = new Vector2(0f, 0f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(90f, 90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(180f, 180f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Dispose();
        }

        private void ThrowAdvancedProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            var projectileData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(0f, 0f),
                CollidesWithTerrain = false,
                Scale = ProjectileScale
            };
            ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
            projectileData.Angle = new Vector2(90f, 90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Angle = new Vector2(-90f, -90f);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            projectileData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            if (!Chasing)
            {
                switch (State)
                {
                    case 0:
                        break;

                    case 1:
                    {
                        var arg_42_1 = true;
                        var arg_42_2 = m_generalBasicLB;
                        var array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_42_1, arg_42_2, array);
                        return;
                    }

                    case 2:
                    case 3:
                        ChasePlayer();
                        return;

                    default:
                        return;
                }
            }
            else
            {
                RunLogicBlock(true, m_generalBasicLB, 0, 100);
            }
        }

        protected override void RunAdvancedLogic()
        {
            if (!Chasing)
            {
                switch (State)
                {
                    case 0:
                        break;

                    case 1:
                    {
                        var arg_42_1 = true;
                        var arg_42_2 = m_generalAdvancedLB;
                        var array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_42_1, arg_42_2, array);
                        return;
                    }

                    case 2:
                    case 3:
                        ChasePlayer();
                        return;

                    default:
                        return;
                }
            }
            else
            {
                RunLogicBlock(true, m_generalAdvancedLB, 0, 100);
            }
        }

        protected override void RunExpertLogic()
        {
            if (!Chasing)
            {
                switch (State)
                {
                    case 0:
                        break;

                    case 1:
                    {
                        var arg_42_1 = true;
                        var arg_42_2 = m_generalExpertLB;
                        var array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_42_1, arg_42_2, array);
                        return;
                    }

                    case 2:
                    case 3:
                        ChasePlayer();
                        return;

                    default:
                        return;
                }
            }
            else
            {
                RunLogicBlock(true, m_generalExpertLB, 0, 100);
            }
        }

        protected override void RunMinibossLogic()
        {
            if (!Chasing)
            {
                switch (State)
                {
                    case 0:
                        break;

                    case 1:
                    {
                        var arg_43_1 = true;
                        var arg_43_2 = m_generalMiniBossLB;
                        var array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_43_1, arg_43_2, array);
                        return;
                    }

                    case 2:
                    case 3:
                        Chasing = true;
                        return;

                    default:
                        return;
                }
            }
            else
            {
                RunLogicBlock(true, m_generalMiniBossLB, 0, 100);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (!Chasing)
            {
                if (Difficulty != EnemyDifficulty.MiniBoss)
                {
                    if (Shake)
                    {
                        Rotation = (float) Math.Sin(Game.TotalGameTimeSeconds * 15f) * 2f;
                    }
                    else
                    {
                        Rotation = 0f;
                    }
                }
            }
            else
            {
                if (Difficulty == EnemyDifficulty.MiniBoss)
                {
                    Rotation += 420f * num;
                }
                else
                {
                    Rotation += 600f * num;
                }

                var spriteObj = GetChildAt(0) as SpriteObj;
                if (spriteObj.SpriteName != "EnemyPortrait" + (int) Difficulty + "_Sprite")
                {
                    ChangePortrait();
                }
            }

            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            ChasePlayer();
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj)
            {
                ChasePlayer();
            }

            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public void ChasePlayer()
        {
            if (!Chasing)
            {
                if (m_currentActiveLB != null)
                {
                    m_currentActiveLB.StopLogicBlock();
                }

                Chasing = true;
                if (m_target.X < X)
                {
                    Orientation = 0f;
                    return;
                }

                Orientation = MathHelper.ToRadians(180f);
            }
        }

        public override void Reset()
        {
            Chasing = false;
            base.Reset();
        }

        public void ChangePortrait()
        {
            SoundManager.PlaySound("FinalBoss_St2_BlockLaugh");
            var spriteObj = GetChildAt(0) as SpriteObj;
            spriteObj.ChangeSprite("EnemyPortrait" + (int) Difficulty + "_Sprite");
            if (Difficulty == EnemyDifficulty.MiniBoss)
            {
                GetChildAt(1).Visible = false;
            }
        }
    }
}