// 
// RogueLegacyArchipelago - EnemyObj_ShieldKnight.cs
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
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.TypeDefinitions;

namespace RogueCastle
{
    public class EnemyObj_ShieldKnight : EnemyObj
    {
        private readonly float m_blockDmgReduction = 0.6f;
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;
        private Vector2 ShieldKnockback = new Vector2(900f, 1050f);

        public EnemyObj_ShieldKnight(PlayerObj target, PhysicsManager physicsManager,
            ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyShieldKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 14;
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        protected override void InitializeEV()
        {
            LockFlip = true;
            Name = "Tall Guard";
            MaxHealth = 40;
            Damage = 31;
            XPValue = 150;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 100f;
            TurnSpeed = 0.0175f;
            ProjectileSpeed = 650f;
            JumpHeight = 950f;
            CooldownTime = 2.25f;
            AnimationDelay = 0.142857149f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.ShieldKnight_Basic_Scale;
            ProjectileScale = EnemyEV.ShieldKnight_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.ShieldKnight_Basic_Tint;
            MeleeRadius = 50;
            ProjectileRadius = 550;
            EngageRadius = 700;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.ShieldKnight_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Basic:
                    break;
                case GameTypes.EnemyDifficulty.Advanced:
                    ShieldKnockback = new Vector2(1050f, 1150f);
                    Name = "Hulk Guard";
                    MaxHealth = 58;
                    Damage = 38;
                    XPValue = 250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 175f;
                    TurnSpeed = 0.0175f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 950f;
                    CooldownTime = 2.25f;
                    AnimationDelay = 0.142857149f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.ShieldKnight_Advanced_Scale;
                    ProjectileScale = EnemyEV.ShieldKnight_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnight_Advanced_Tint;
                    MeleeRadius = 50;
                    EngageRadius = 700;
                    ProjectileRadius = 550;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnight_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    ShieldKnockback = new Vector2(1550f, 1650f);
                    Name = "Tower Guard";
                    MaxHealth = 79;
                    Damage = 43;
                    XPValue = 450;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 250f;
                    TurnSpeed = 0.0175f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 950f;
                    CooldownTime = 2.25f;
                    AnimationDelay = 0.09090909f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.ShieldKnight_Expert_Scale;
                    ProjectileScale = EnemyEV.ShieldKnight_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnight_Expert_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 550;
                    EngageRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnight_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    ShieldKnockback = new Vector2(1200f, 1350f);
                    Name = "Sentinel";
                    MaxHealth = 1;
                    Damage = 1;
                    XPValue = 1250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 250f;
                    TurnSpeed = 0.0175f;
                    ProjectileSpeed = 650f;
                    JumpHeight = 950f;
                    CooldownTime = 0f;
                    AnimationDelay = 0.142857149f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.ShieldKnight_Miniboss_Scale;
                    ProjectileScale = EnemyEV.ShieldKnight_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnight_Miniboss_Tint;
                    MeleeRadius = 50;
                    ProjectileRadius = 550;
                    EngageRadius = 700;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnight_Miniboss_KnockBack;
                    return;
                default:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightIdle_Character"));
            logicSet.AddAction(new MoveDirectionLogicAction(0f));
            logicSet.AddAction(new DelayLogicAction(0.5f, 2f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightWalk_Character"));
            logicSet2.AddAction(new MoveDirectionLogicAction());
            logicSet2.AddAction(new DelayLogicAction(0.5f, 2f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet3.AddAction(new MoveDirectionLogicAction(0f));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false));
            logicSet3.AddAction(new PlayAnimationLogicAction(1, 2));
            logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "ShieldKnight_Turn"));
            logicSet3.AddAction(new PlayAnimationLogicAction(3, TotalFrames));
            logicSet3.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false));
            logicSet3.AddAction(new MoveDirectionLogicAction());
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new MoveDirectionLogicAction(0f));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false));
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f));
            logicSet4.AddAction(new PlayAnimationLogicAction(1, 2));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "ShieldKnight_Turn"));
            logicSet4.AddAction(new PlayAnimationLogicAction(3, TotalFrames));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false));
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.09090909f));
            logicSet4.AddAction(new MoveDirectionLogicAction());
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet4);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            SetCooldownLogicBlock(m_generalBasicLB, 100);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_107_1 = true;
                    var arg_107_2 = m_generalBasicLB;
                    var array = new int[3];
                    array[0] = 100;
                    RunLogicBlock(arg_107_1, arg_107_2, array);
                    return;
                }
                case 1:
                case 2:
                case 3:
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_8E_1 = true;
                    var arg_8E_2 = m_generalBasicLB;
                    var array2 = new int[3];
                    array2[1] = 100;
                    RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_107_1 = true;
                    var arg_107_2 = m_generalBasicLB;
                    var array = new int[3];
                    array[0] = 100;
                    RunLogicBlock(arg_107_1, arg_107_2, array);
                    return;
                }
                case 1:
                case 2:
                case 3:
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                        return;
                    }
                    var arg_8E_1 = true;
                    var arg_8E_2 = m_generalBasicLB;
                    var array2 = new int[3];
                    array2[1] = 100;
                    RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalExpertLB, 0, 0, 100);
                        return;
                    }
                    var arg_107_1 = true;
                    var arg_107_2 = m_generalExpertLB;
                    var array = new int[3];
                    array[0] = 100;
                    RunLogicBlock(arg_107_1, arg_107_2, array);
                    return;
                }
                case 1:
                case 2:
                case 3:
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        RunLogicBlock(true, m_generalExpertLB, 0, 0, 100);
                        return;
                    }
                    var arg_8E_1 = true;
                    var arg_8E_2 = m_generalExpertLB;
                    var array2 = new int[3];
                    array2[1] = 100;
                    RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        var arg_E9_1 = true;
                        var arg_E9_2 = m_generalBasicLB;
                        var array = new int[3];
                        array[0] = 100;
                        RunLogicBlock(arg_E9_1, arg_E9_2, array);
                        return;
                    }
                    var arg_107_1 = true;
                    var arg_107_2 = m_generalBasicLB;
                    var array2 = new int[3];
                    array2[0] = 100;
                    RunLogicBlock(arg_107_1, arg_107_2, array2);
                    return;
                }
                case 1:
                case 2:
                case 3:
                {
                    if ((m_target.X > X && HeadingX < 0f) || (m_target.X < X && HeadingX >= 0f))
                    {
                        var arg_73_1 = true;
                        var arg_73_2 = m_generalBasicLB;
                        var array3 = new int[3];
                        array3[0] = 100;
                        RunLogicBlock(arg_73_1, arg_73_2, array3);
                        return;
                    }
                    var arg_8E_1 = true;
                    var arg_8E_2 = m_generalBasicLB;
                    var array4 = new int[3];
                    array4[0] = 100;
                    RunLogicBlock(arg_8E_1, arg_8E_2, array4);
                    return;
                }
                default:
                    return;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            var playerObj = otherBox.AbsParent as PlayerObj;
            var projectileObj = otherBox.AbsParent as ProjectileObj;
            if (collisionResponseType == 2 &&
                ((playerObj != null && m_invincibleCounter <= 0f) ||
                 (projectileObj != null && m_invincibleCounterProjectile <= 0f)) &&
                ((Flip == SpriteEffects.None && otherBox.AbsParent.AbsPosition.X > X) ||
                 (Flip == SpriteEffects.FlipHorizontally && otherBox.AbsParent.AbsPosition.X < X)) && playerObj != null &&
                playerObj.SpriteName != "PlayerAirAttack_Character")
            {
                if (CanBeKnockedBack)
                {
                    CurrentSpeed = 0f;
                    m_currentActiveLB.StopLogicBlock();
                    var arg_DD_1 = true;
                    var arg_DD_2 = m_generalBasicLB;
                    var array = new int[3];
                    array[0] = 100;
                    RunLogicBlock(arg_DD_1, arg_DD_2, array);
                }
                if (m_target.IsAirAttacking)
                {
                    m_target.IsAirAttacking = false;
                    m_target.AccelerationY = -m_target.AirAttackKnockBack;
                    m_target.NumAirBounces++;
                }
                else
                {
                    if (m_target.Bounds.Left + m_target.Bounds.Width/2 < X)
                    {
                        m_target.AccelerationX = -ShieldKnockback.X;
                    }
                    else
                    {
                        m_target.AccelerationX = ShieldKnockback.X;
                    }
                    m_target.AccelerationY = -ShieldKnockback.Y;
                }
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                var center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                var position = new Vector2(center.X, center.Y);
                m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, new Vector2(2f, 2f));
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "ShieldKnight_Block01", "ShieldKnight_Block02",
                    "ShieldKnight_Block03");
                m_invincibleCounter = InvincibilityTime;
                m_levelScreen.SetLastEnemyHit(this);
                Blink(Color.LightBlue, 0.1f);
                var projectileObj2 = otherBox.AbsParent as ProjectileObj;
                if (projectileObj2 != null)
                {
                    m_invincibleCounterProjectile = InvincibilityTime;
                    m_levelScreen.ProjectileManager.DestroyProjectile(projectileObj2);
                }
            }
            else
            {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            if (m_target != null && m_target.CurrentHealth > 0)
            {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
                if (((Flip == SpriteEffects.None && m_target.X > X) ||
                     (Flip == SpriteEffects.FlipHorizontally && m_target.X < X)) &&
                    m_target.SpriteName != "PlayerAirAttack_Character")
                {
                    damage = (int) (damage*(1f - m_blockDmgReduction));
                }
            }
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpriteName == "EnemyShieldKnightWalk_Character")
            {
                m_walkSound.Update();
                m_walkSound2.Update();
            }
            base.Update(gameTime);
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