// 
// RogueLegacyArchipelago - EnemyObj_Mimic.cs
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
    public class EnemyObj_Mimic : EnemyObj
    {
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private FrameSoundObj m_closeSound;
        private bool m_isAttacking;

        public EnemyObj_Mimic(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyMimicIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 33;
            OutlineWidth = 0;
            m_closeSound = new FrameSoundObj(this, m_target, 1, "Chest_Snap");
        }

        protected override void InitializeEV()
        {
            Name = "Mimic";
            MaxHealth = 35;
            Damage = 20;
            XPValue = 75;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 1;
            MoneyDropChance = 0.4f;
            Speed = 400f;
            TurnSpeed = 10f;
            ProjectileSpeed = 775f;
            JumpHeight = 550f;
            CooldownTime = 2f;
            AnimationDelay = 0.05f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = true;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Mimic_Basic_Scale;
            ProjectileScale = EnemyEV.Mimic_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Mimic_Basic_Tint;
            MeleeRadius = 10;
            ProjectileRadius = 20;
            EngageRadius = 975;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Mimic_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Advanced:
                    Name = "Mimicant";
                    MaxHealth = 40;
                    Damage = 23;
                    XPValue = 125;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 600f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1100f;
                    JumpHeight = 650f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Mimic_Advanced_Scale;
                    ProjectileScale = EnemyEV.Mimic_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Advanced_Tint;
                    MeleeRadius = 10;
                    EngageRadius = 975;
                    ProjectileRadius = 20;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Mimic_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    Name = "Mimicrunch";
                    MaxHealth = 70;
                    Damage = 25;
                    XPValue = 225;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 3;
                    MoneyDropChance = 1f;
                    Speed = 750f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 925f;
                    JumpHeight = 550f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Mimic_Expert_Scale;
                    ProjectileScale = EnemyEV.Mimic_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Expert_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Mimic_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    Name = "Chesticles";
                    MaxHealth = 100;
                    Damage = 32;
                    XPValue = 750;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 0f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 900f;
                    JumpHeight = 750f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.05f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = true;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Mimic_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Mimic_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Mimic_Miniboss_Tint;
                    MeleeRadius = 10;
                    ProjectileRadius = 20;
                    EngageRadius = 975;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Mimic_Miniboss_KnockBack;
                    break;
            }
            LockFlip = true;
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicShake_Character", false, false));
            logicSet.AddAction(new PlayAnimationLogicAction(false));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicIdle_Character", true, false));
            logicSet.AddAction(new DelayLogicAction(3f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new GroundCheckLogicAction());
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet2.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyMimicAttack_Character"));
            logicSet2.AddAction(new MoveDirectionLogicAction());
            logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, "Chest_Open_Large"));
            logicSet2.AddAction(new JumpLogicAction());
            logicSet2.AddAction(new DelayLogicAction(0.3f));
            logicSet2.AddAction(new GroundCheckLogicAction());
            new LogicSet(this);
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2);
            logicBlocksToDispose.Add(m_generalBasicLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            if (!m_isAttacking)
                RunLogicBlock(false, m_generalBasicLB, 100, 0);
            else
                RunLogicBlock(false, m_generalBasicLB, 0, 100);
        }

        protected override void RunAdvancedLogic()
        {
            if (!m_isAttacking)
                RunLogicBlock(false, m_generalBasicLB, 100, 0);
            else
                RunLogicBlock(false, m_generalBasicLB, 0, 100);
        }

        protected override void RunExpertLogic()
        {
            if (!m_isAttacking)
                RunLogicBlock(false, m_generalBasicLB, 100, 0);
            else
                RunLogicBlock(false, m_generalBasicLB, 0, 100);
        }

        protected override void RunMinibossLogic()
        {
            if (!m_isAttacking)
                RunLogicBlock(false, m_generalBasicLB, 100, 0);
            else
                RunLogicBlock(false, m_generalBasicLB, 0, 100);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            if (!m_isAttacking)
            {
                m_currentActiveLB.StopLogicBlock();
                m_isAttacking = true;
                LockFlip = false;
            }
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj && !m_isAttacking)
            {
                m_currentActiveLB.StopLogicBlock();
                m_isAttacking = true;
                LockFlip = false;
            }
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpriteName == "EnemyMimicAttack_Character")
            {
                m_closeSound.Update();
            }
            base.Update(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_closeSound.Dispose();
                m_closeSound = null;
                base.Dispose();
            }
        }
    }
}