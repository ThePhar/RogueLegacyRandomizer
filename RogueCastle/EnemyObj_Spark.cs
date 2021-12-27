// 
// RogueLegacyArchipelago - EnemyObj_Spark.cs
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

namespace RogueCastle
{
    public class EnemyObj_Spark : EnemyObj
    {
        private readonly byte m_collisionBoxSize = 10;
        private bool m_hookedToGround;

        public EnemyObj_Spark(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemySpark_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            IsWeighted = false;
            ForceDraw = true;
            Type = 24;
            NonKillable = true;
        }

        private Rectangle TopRect
        {
            get
            {
                return new Rectangle(Bounds.Left + m_collisionBoxSize, Bounds.Top, Width - m_collisionBoxSize*2,
                    m_collisionBoxSize);
            }
        }

        private Rectangle BottomRect
        {
            get
            {
                return new Rectangle(Bounds.Left + m_collisionBoxSize, Bounds.Bottom - m_collisionBoxSize,
                    Width - m_collisionBoxSize*2, m_collisionBoxSize);
            }
        }

        private Rectangle LeftRect
        {
            get
            {
                return new Rectangle(Bounds.Left, Bounds.Top + m_collisionBoxSize, m_collisionBoxSize,
                    Height - m_collisionBoxSize*2);
            }
        }

        private Rectangle RightRect
        {
            get
            {
                return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Top + m_collisionBoxSize,
                    m_collisionBoxSize, Height - m_collisionBoxSize*2);
            }
        }

        private Rectangle TopLeftPoint
        {
            get { return new Rectangle(Bounds.Left, Bounds.Top, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle TopRightPoint
        {
            get
            {
                return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Top, m_collisionBoxSize,
                    m_collisionBoxSize);
            }
        }

        private Rectangle BottomLeftPoint
        {
            get
            {
                return new Rectangle(Bounds.Left, Bounds.Bottom - m_collisionBoxSize, m_collisionBoxSize,
                    m_collisionBoxSize);
            }
        }

        private Rectangle BottomRightPoint
        {
            get
            {
                return new Rectangle(Bounds.Right - m_collisionBoxSize, Bounds.Bottom - m_collisionBoxSize,
                    m_collisionBoxSize, m_collisionBoxSize);
            }
        }

        protected override void InitializeEV()
        {
            LockFlip = true;
            IsWeighted = false;
            Name = "Sparky";
            MaxHealth = 20;
            Damage = 20;
            XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 200f;
            TurnSpeed = 0.0275f;
            ProjectileSpeed = 525f;
            JumpHeight = 500f;
            CooldownTime = 2f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            CanBeKnockedBack = false;
            IsWeighted = false;
            Scale = EnemyEV.Spark_Basic_Scale;
            ProjectileScale = EnemyEV.Spark_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Spark_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 30;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Spark_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                    break;
                case EnemyDifficulty.Advanced:
                    Name = "Mr. Spark";
                    MaxHealth = 20;
                    Damage = 28;
                    XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 300f;
                    TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    JumpHeight = 500f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Spark_Advanced_Scale;
                    ProjectileScale = EnemyEV.Spark_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 30;
                    ProjectileRadius = 20;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Spark_Advanced_KnockBack;
                    break;
                case EnemyDifficulty.Expert:
                    Name = "Grandpa Spark";
                    MaxHealth = 20;
                    Damage = 32;
                    XPValue = 350;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 375f;
                    TurnSpeed = 0.03f;
                    ProjectileSpeed = 525f;
                    JumpHeight = 500f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Spark_Expert_Scale;
                    ProjectileScale = EnemyEV.Spark_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Spark_Expert_KnockBack;
                    return;
                case EnemyDifficulty.MiniBoss:
                    Name = "Lord Spark, King of Sparkatonia";
                    MaxHealth = 500;
                    Damage = 45;
                    XPValue = 800;
                    MinMoneyDropAmount = 6;
                    MaxMoneyDropAmount = 10;
                    MoneyDropChance = 1f;
                    Speed = 380f;
                    TurnSpeed = 0.03f;
                    ProjectileSpeed = 0f;
                    JumpHeight = 500f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = false;
                    Scale = EnemyEV.Spark_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Spark_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Spark_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Spark_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            CurrentSpeed = Speed;
            base.InitializeLogic();
        }

        public void HookToGround()
        {
            m_hookedToGround = true;
            var num = 1000f;
            TerrainObj terrainObj = null;
            foreach (var current in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                if (current.Y >= Y && current.Y - Y < num &&
                    CollisionMath.Intersects(current.Bounds,
                        new Rectangle((int) X, (int) (Y + (current.Y - Y) + 5f), Width, Height/2)))
                {
                    num = current.Y - Y;
                    terrainObj = current;
                }
            }
            if (terrainObj != null)
            {
                Y = terrainObj.Y - Height/2 + 5f;
            }
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
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
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_hookedToGround)
            {
                HookToGround();
            }
            CollisionCheckRight();
            if (!IsPaused)
            {
                Position += Heading*(CurrentSpeed*(float) gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        private void CollisionCheckRight()
        {
            var flag = false;
            var flag2 = false;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
            var flag6 = false;
            var flag7 = false;
            var flag8 = false;
            var num = 0f;
            if (Bounds.Right >= m_levelScreen.CurrentRoom.Bounds.Right)
            {
                flag6 = true;
                flag4 = true;
                flag8 = true;
            }
            else if (Bounds.Left <= m_levelScreen.CurrentRoom.Bounds.Left)
            {
                flag5 = true;
                flag3 = true;
                flag7 = true;
            }
            if (Bounds.Top <= m_levelScreen.CurrentRoom.Bounds.Top)
            {
                flag6 = true;
                flag = true;
                flag5 = true;
            }
            else if (Bounds.Bottom >= m_levelScreen.CurrentRoom.Bounds.Bottom)
            {
                flag7 = true;
                flag2 = true;
                flag8 = true;
            }
            foreach (var current in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                var b = new Rectangle((int) current.X, (int) current.Y, current.Width, current.Height);
                if (CollisionMath.RotatedRectIntersects(TopLeftPoint, 0f, Vector2.Zero, b, current.Rotation,
                    Vector2.Zero))
                {
                    flag5 = true;
                }
                if (CollisionMath.RotatedRectIntersects(TopRightPoint, 0f, Vector2.Zero, b, current.Rotation,
                    Vector2.Zero))
                {
                    flag6 = true;
                }
                if (CollisionMath.RotatedRectIntersects(BottomRightPoint, 0f, Vector2.Zero, b, current.Rotation,
                    Vector2.Zero))
                {
                    flag8 = true;
                    if (current.Rotation != 0f)
                    {
                        var vector = CollisionMath.RotatedRectIntersectsMTD(BottomRightPoint, 0f, Vector2.Zero, b,
                            current.Rotation, Vector2.Zero);
                        if (vector.X < 0f && vector.Y < 0f)
                        {
                            num = -45f;
                        }
                    }
                }
                if (CollisionMath.RotatedRectIntersects(BottomLeftPoint, 0f, Vector2.Zero, b, current.Rotation,
                    Vector2.Zero))
                {
                    flag7 = true;
                    if (current.Rotation != 0f)
                    {
                        var vector2 = CollisionMath.RotatedRectIntersectsMTD(BottomLeftPoint, 0f, Vector2.Zero, b,
                            current.Rotation, Vector2.Zero);
                        if (vector2.X > 0f && vector2.Y < 0f)
                        {
                            num = 45f;
                        }
                    }
                }
                if (CollisionMath.RotatedRectIntersects(TopRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                {
                    flag = true;
                }
                if (CollisionMath.RotatedRectIntersects(BottomRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                {
                    flag2 = true;
                }
                if (CollisionMath.RotatedRectIntersects(LeftRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                {
                    flag3 = true;
                }
                if (CollisionMath.RotatedRectIntersects(RightRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
                {
                    flag4 = true;
                }
            }
            if (flag8 && !flag6 && !flag4)
            {
                Orientation = 0f;
            }
            if (flag6 && flag8 && !flag5)
            {
                Orientation = MathHelper.ToRadians(-90f);
            }
            if (flag6 && flag5 && !flag7)
            {
                Orientation = MathHelper.ToRadians(-180f);
            }
            if (flag5 && flag3 && !flag2)
            {
                Orientation = MathHelper.ToRadians(90f);
            }
            if (flag6 && !flag && !flag4)
            {
                Orientation = MathHelper.ToRadians(-90f);
            }
            if (flag5 && !flag && !flag3)
            {
                Orientation = MathHelper.ToRadians(-180f);
            }
            if (flag7 && !flag3 && !flag4 && !flag2)
            {
                Orientation = MathHelper.ToRadians(90f);
            }
            if (flag8 && !flag2 && !flag4)
            {
                Orientation = 0f;
            }
            if (num != 0f && ((num < 0f && flag8 && flag4) || (num > 0f && !flag8)))
            {
                Orientation = MathHelper.ToRadians(num);
            }
            HeadingX = (float) Math.Cos(Orientation);
            HeadingY = (float) Math.Sin(Orientation);
        }
    }
}
