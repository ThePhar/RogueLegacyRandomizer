// 
// RogueLegacyArchipelago - EnemyObj_Energon.cs
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
    public class EnemyObj_Energon : EnemyObj
    {
        private const byte TYPE_SWORD = 0;
        private const byte TYPE_SHIELD = 1;
        private const byte TYPE_DOWNSWORD = 2;
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();
        private readonly byte m_poolSize = 10;
        private byte m_currentAttackType;
        private DS2DPool<EnergonProjectileObj> m_projectilePool;
        private SpriteObj m_shield;

        public EnemyObj_Energon(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyEnergonIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 23;
            m_shield = new SpriteObj("EnergonSwordShield_Sprite");
            m_shield.AnimationDelay = 0.1f;
            m_shield.PlayAnimation();
            m_shield.Opacity = 0.5f;
            m_shield.Scale = new Vector2(1.2f, 1.2f);
            m_projectilePool = new DS2DPool<EnergonProjectileObj>();
            for (var i = 0; i < (int) m_poolSize; i++)
            {
                var energonProjectileObj = new EnergonProjectileObj("EnergonSwordProjectile_Sprite",
                    this);
                energonProjectileObj.Visible = false;
                energonProjectileObj.CollidesWithTerrain = false;
                energonProjectileObj.PlayAnimation();
                energonProjectileObj.AnimationDelay = 0.05f;
                m_projectilePool.AddToPool(energonProjectileObj);
            }
        }

        protected override void InitializeEV()
        {
            Name = "Energon";
            MaxHealth = 25;
            Damage = 25;
            XPValue = 150;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 75f;
            TurnSpeed = 10f;
            ProjectileSpeed = 300f;
            JumpHeight = 950f;
            CooldownTime = 2f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.Energon_Basic_Scale;
            ProjectileScale = EnemyEV.Energon_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.Energon_Basic_Tint;
            MeleeRadius = 150;
            ProjectileRadius = 750;
            EngageRadius = 850;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.Energon_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                    Name = "Mastertron";
                    MaxHealth = 38;
                    Damage = 29;
                    XPValue = 250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 100f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 300f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Energon_Advanced_Scale;
                    ProjectileScale = EnemyEV.Energon_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Energon_Advanced_Tint;
                    MeleeRadius = 150;
                    EngageRadius = 850;
                    ProjectileRadius = 750;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Energon_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    Name = "Voltron";
                    MaxHealth = 61;
                    Damage = 33;
                    XPValue = 375;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 125f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 300f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Energon_Expert_Scale;
                    ProjectileScale = EnemyEV.Energon_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Energon_Expert_Tint;
                    MeleeRadius = 150;
                    ProjectileRadius = 750;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Energon_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    Name = "Prime";
                    MaxHealth = 300;
                    Damage = 39;
                    XPValue = 1500;
                    MinMoneyDropAmount = 8;
                    MaxMoneyDropAmount = 16;
                    MoneyDropChance = 1f;
                    Speed = 200f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 300f;
                    JumpHeight = 950f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.Energon_Miniboss_Scale;
                    ProjectileScale = EnemyEV.Energon_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.Energon_Miniboss_Tint;
                    MeleeRadius = 150;
                    ProjectileRadius = 750;
                    EngageRadius = 850;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.Energon_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character"));
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.2f, 0.75f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false));
            logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "FireCurrentTypeProjectile"));
            logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character"));
            logicSet4.Tag = 2;
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false));
            logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "SwitchRandomType"));
            logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character"));
            logicSet5.AddAction(new DelayLogicAction(2f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 0, 0, 100);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                    break;
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 30, 60, 10);
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

        public void FireCurrentTypeProjectile()
        {
            FireProjectile(m_currentAttackType);
        }

        public void FireProjectile(byte type)
        {
            var energonProjectileObj = m_projectilePool.CheckOut();
            energonProjectileObj.SetType(type);
            PhysicsMngr.AddObject(energonProjectileObj);
            energonProjectileObj.Target = m_target;
            energonProjectileObj.Visible = true;
            energonProjectileObj.Position = Position;
            energonProjectileObj.CurrentSpeed = ProjectileSpeed;
            energonProjectileObj.Flip = Flip;
            energonProjectileObj.Scale = ProjectileScale;
            energonProjectileObj.Opacity = 0.8f;
            energonProjectileObj.Damage = Damage;
            energonProjectileObj.PlayAnimation();
        }

        public void DestroyProjectile(EnergonProjectileObj projectile)
        {
            if (m_projectilePool.ActiveObjsList.Contains(projectile))
            {
                projectile.Visible = false;
                projectile.Scale = new Vector2(1f, 1f);
                projectile.CollisionTypeTag = 3;
                PhysicsMngr.RemoveObject(projectile);
                m_projectilePool.CheckIn(projectile);
            }
        }

        public void DestroyAllProjectiles()
        {
            ProjectileObj[] array = m_projectilePool.ActiveObjsList.ToArray();
            var array2 = array;
            for (var i = 0; i < array2.Length; i++)
            {
                var projectile = (EnergonProjectileObj) array2[i];
                DestroyProjectile(projectile);
            }
        }

        public void SwitchRandomType()
        {
            byte b;
            for (b = m_currentAttackType; b == m_currentAttackType; b = (byte) CDGMath.RandomInt(0, 2))
            {
            }
            SwitchType(b);
        }

        public void SwitchType(byte type)
        {
            m_currentAttackType = type;
            switch (type)
            {
                case 0:
                    m_shield.ChangeSprite("EnergonSwordShield_Sprite");
                    break;
                case 1:
                    m_shield.ChangeSprite("EnergonShieldShield_Sprite");
                    break;
                case 2:
                    m_shield.ChangeSprite("EnergonDownSwordShield_Sprite");
                    break;
            }
            m_shield.PlayAnimation();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in m_projectilePool.ActiveObjsList)
            {
                current.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            m_shield.Position = Position;
            m_shield.Flip = Flip;
            m_shield.Draw(camera);
            foreach (ProjectileObj current in m_projectilePool.ActiveObjsList)
            {
                current.Draw(camera);
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (collisionResponseType == 2 && m_invincibleCounter <= 0f && otherBox.AbsParent is PlayerObj)
            {
                if (m_target.Bounds.Left + m_target.Bounds.Width/2 < X)
                {
                    m_target.AccelerationX = -m_target.EnemyKnockBack.X;
                }
                else
                {
                    m_target.AccelerationX = m_target.EnemyKnockBack.X;
                }
                m_target.AccelerationY = -m_target.EnemyKnockBack.Y;
                var center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                var position = new Vector2(center.X, center.Y);
                m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, Vector2.One);
                m_levelScreen.SetLastEnemyHit(this);
                m_invincibleCounter = InvincibilityTime;
                Blink(Color.LightBlue, 0.1f);
                var projectileObj = otherBox.AbsParent as ProjectileObj;
                if (projectileObj != null)
                {
                    m_levelScreen.ProjectileManager.DestroyProjectile(projectileObj);
                }
            }
            else if (otherBox.AbsParent is EnergonProjectileObj)
            {
                var energonProjectileObj = otherBox.AbsParent as EnergonProjectileObj;
                if (energonProjectileObj != null)
                {
                    var center2 = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                    var vector = new Vector2(center2.X, center2.Y);
                    DestroyProjectile(energonProjectileObj);
                    if (energonProjectileObj.AttackType == m_currentAttackType)
                    {
                        HitEnemy(energonProjectileObj.Damage, vector, true);
                        return;
                    }
                    m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(vector, Vector2.One);
                }
            }
            else
            {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            }
        }

        public override void Kill(bool giveXP = true)
        {
            m_shield.Visible = false;
            DestroyAllProjectiles();
            base.Kill(giveXP);
        }

        public override void Reset()
        {
            m_shield.Visible = true;
            base.Reset();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_projectilePool.Dispose();
                m_projectilePool = null;
                m_shield.Dispose();
                m_shield = null;
                base.Dispose();
            }
        }
    }
}