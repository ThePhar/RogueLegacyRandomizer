// 
// RogueLegacyArchipelago - EnemyObj_Chicken.cs
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
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;

namespace RogueCastle
{
    public class EnemyObj_Chicken : EnemyObj
    {
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();

        public EnemyObj_Chicken(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyChickenRun_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 26;
        }

        protected override void InitializeEV()
        {
            LockFlip = true;
            Name = "Kentucky";
            MaxHealth = 1;
            Damage = 8;
            XPValue = 100;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0f;
            Speed = 350f;
            TurnSpeed = 10f;
            ProjectileSpeed = 850f;
            JumpHeight = 950f;
            CooldownTime = 2f;
            AnimationDelay = 0.0333333351f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = true;
            CanBeKnockedBack = true;
            IsWeighted = false;
            Scale = EnemyEV.Chicken_Basic_Scale;
            ProjectileScale = EnemyEV.Chicken_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Chicken_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 30;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Chicken_Basic_KnockBack;
            switch (Difficulty)
            {
                case EnemyDifficulty.Advanced:
                    Name = "Fried";
                    MaxHealth = 1;
                    Damage = 11;
                    XPValue = 175;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 1;
                    MoneyDropChance = 0f;
                    Speed = 375f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.Chicken_Advanced_Scale;
                    ProjectileScale = EnemyEV.Chicken_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 30;
                    ProjectileRadius = 20;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Chicken_Advanced_KnockBack;
                    break;
                case EnemyDifficulty.Expert:
                    Name = "Chicken";
                    MaxHealth = 1;
                    Damage = 14;
                    XPValue = 350;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0f;
                    Speed = 400f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.Chicken_Expert_Scale;
                    ProjectileScale = EnemyEV.Chicken_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Chicken_Expert_KnockBack;
                    break;
                case EnemyDifficulty.MiniBoss:
                    Name = "Delicious";
                    MaxHealth = 1;
                    Damage = 35;
                    XPValue = 800;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 5;
                    MoneyDropChance = 0f;
                    Speed = 750f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 850f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.0333333351f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = false;
                    Scale = EnemyEV.Chicken_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Chicken_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Chicken_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 30;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Chicken_Miniboss_KnockBack;
                    break;
                default:
                    Scale = new Vector2(2f, 2f);
                    break;
            }
            IsWeighted = true;
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character"));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.FlipHorizontally));
            logicSet.AddAction(new MoveDirectionLogicAction());
            logicSet.AddAction(new DelayLogicAction(0.5f, 1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character"));
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.None));
            logicSet2.AddAction(new MoveDirectionLogicAction());
            logicSet2.AddAction(new DelayLogicAction(0.5f, 1f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2);
            logicBlocksToDispose.Add(m_generalBasicLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            RunLogicBlock(true, m_generalBasicLB, 50, 50);
        }

        protected override void RunAdvancedLogic()
        {
            RunLogicBlock(true, m_generalBasicLB, 50, 50);
        }

        protected override void RunExpertLogic()
        {
            RunLogicBlock(true, m_generalBasicLB, 50, 50);
        }

        protected override void RunMinibossLogic()
        {
            RunLogicBlock(true, m_generalBasicLB, 50, 50);
        }

        public void MakeCollideable()
        {
            IsCollidable = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_levelScreen != null && m_levelScreen.CurrentRoom != null && !IsKilled &&
                !CollisionMath.Intersects(TerrainBounds, m_levelScreen.CurrentRoom.Bounds))
            {
                Kill();
            }
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            SoundManager.Play3DSound(this, m_target, "Chicken_Cluck_01", "Chicken_Cluck_02", "Chicken_Cluck_03");
            base.HitEnemy(damage, collisionPt, isPlayer);
        }
    }
}
