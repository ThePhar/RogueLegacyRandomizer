//
// RogueLegacyArchipelago - EnemyObj_BallAndChain.cs
// Last Modified 2021-12-24
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
//
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Structs;

namespace RogueCastle
{
    public class EnemyObj_BallAndChain : EnemyObj
    {
        private readonly float m_BallSpeedDivider = 1.5f;
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly int m_numChainLinks = 10;
        private float m_actualChainRadius;
        private float m_ballAngle;
        private SpriteObj m_chain;
        private float m_chainLinkDistance;
        private List<Vector2> m_chainLinks2List;
        private List<Vector2> m_chainLinksList;
        private FrameSoundObj m_walkSound;
        private FrameSoundObj m_walkSound2;

        public EnemyObj_BallAndChain(PlayerObj target, PhysicsManager physicsManager,
            ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyFlailKnight_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            BallAndChain = new ProjectileObj("EnemyFlailKnightBall_Sprite");
            BallAndChain.IsWeighted = false;
            BallAndChain.CollidesWithTerrain = false;
            BallAndChain.IgnoreBoundsCheck = true;
            BallAndChain.OutlineWidth = 2;
            BallAndChain2 = (BallAndChain.Clone() as ProjectileObj);
            m_chain = new SpriteObj("EnemyFlailKnightLink_Sprite");
            m_chainLinksList = new List<Vector2>();
            m_chainLinks2List = new List<Vector2>();
            for (var i = 0; i < m_numChainLinks; i++)
            {
                m_chainLinksList.Add(default(Vector2));
            }
            for (var j = 0; j < m_numChainLinks/2; j++)
            {
                m_chainLinks2List.Add(default(Vector2));
            }
            Type = 1;
            TintablePart = _objectList[3];
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        public float ChainSpeed { get; set; }
        public float ChainSpeed2Modifier { get; set; }
        private float ChainRadius { get; set; }
        public ProjectileObj BallAndChain { get; private set; }
        public ProjectileObj BallAndChain2 { get; private set; }

        protected override void InitializeEV()
        {
            ChainSpeed = 2.5f;
            ChainRadius = 260f;
            ChainSpeed2Modifier = 1.5f;
            Name = "Chaintor";
            MaxHealth = 40;
            Damage = 27;
            XPValue = 125;
            MinMoneyDropAmount = 1;
            MaxMoneyDropAmount = 2;
            MoneyDropChance = 0.4f;
            Speed = 100f;
            TurnSpeed = 10f;
            ProjectileSpeed = 1020f;
            JumpHeight = 600f;
            CooldownTime = 2f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.BallAndChain_Basic_Scale;
            ProjectileScale = EnemyEV.BallAndChain_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.BallAndChain_Basic_Tint;
            MeleeRadius = 225;
            ProjectileRadius = 500;
            EngageRadius = 800;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.BallAndChain_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.Advanced:
                    ChainRadius = 275f;
                    Name = "Chaintex";
                    MaxHealth = 58;
                    Damage = 32;
                    XPValue = 150;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 2;
                    MoneyDropChance = 0.5f;
                    Speed = 150f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.BallAndChain_Advanced_Scale;
                    ProjectileScale = EnemyEV.BallAndChain_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Advanced_Tint;
                    MeleeRadius = 225;
                    EngageRadius = 800;
                    ProjectileRadius = 500;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChain_Advanced_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.Expert:
                    ChainRadius = 350f;
                    ChainSpeed2Modifier = 1.5f;
                    Name = "Chaintus";
                    MaxHealth = 79;
                    Damage = 36;
                    XPValue = 200;
                    MinMoneyDropAmount = 2;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 175f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.BallAndChain_Expert_Scale;
                    ProjectileScale = EnemyEV.BallAndChain_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Expert_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChain_Expert_KnockBack;
                    break;
                case GameTypes.EnemyDifficulty.MiniBoss:
                    Name = "Pantheon";
                    MaxHealth = 300;
                    Damage = 60;
                    XPValue = 1250;
                    MinMoneyDropAmount = 1;
                    MaxMoneyDropAmount = 4;
                    MoneyDropChance = 1f;
                    Speed = 100f;
                    TurnSpeed = 10f;
                    ProjectileSpeed = 1020f;
                    JumpHeight = 600f;
                    CooldownTime = 2f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.BallAndChain_Miniboss_Scale;
                    ProjectileScale = EnemyEV.BallAndChain_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChain_Miniboss_Tint;
                    MeleeRadius = 225;
                    ProjectileRadius = 500;
                    EngageRadius = 800;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChain_Miniboss_KnockBack;
                    break;
            }
            _objectList[1].TextureColor = TintablePart.TextureColor;
            BallAndChain.Damage = Damage;
            BallAndChain.Scale = ProjectileScale;
            BallAndChain2.Damage = Damage;
            BallAndChain2.Scale = ProjectileScale;
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new DelayLogicAction(1.25f, 2.75f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(1.25f, 2.75f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new StopAnimationLogicAction());
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(1f, 1.5f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20);
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
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20);
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
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100);
                    return;
                case 1:
                case 2:
                case 3:
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20);
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
                case 1:
                case 2:
                case 3:
                    return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                if (!IsKilled && m_initialDelayCounter <= 0f)
                {
                    var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_actualChainRadius < ChainRadius)
                    {
                        m_actualChainRadius += num*200f;
                        m_chainLinkDistance = m_actualChainRadius/m_numChainLinks;
                    }
                    var num2 = 0f;
                    BallAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius,
                        new Vector2(X, Bounds.Top));
                    for (var i = 0; i < m_chainLinksList.Count; i++)
                    {
                        m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num2, new Vector2(X, Bounds.Top));
                        num2 += m_chainLinkDistance;
                    }
                    num2 = 0f;
                    if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                    {
                        BallAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle*ChainSpeed2Modifier,
                            m_actualChainRadius/2f, new Vector2(X, Bounds.Top));
                    }
                    else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                    {
                        BallAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle*ChainSpeed2Modifier,
                            -m_actualChainRadius/2f, new Vector2(X, Bounds.Top));
                    }
                    for (var j = 0; j < m_chainLinks2List.Count; j++)
                    {
                        if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                        {
                            m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle*ChainSpeed2Modifier, num2,
                                new Vector2(X, Bounds.Top));
                        }
                        else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                        {
                            m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle*ChainSpeed2Modifier, -num2,
                                new Vector2(X, Bounds.Top));
                        }
                        num2 += m_chainLinkDistance;
                    }
                    m_ballAngle += ChainSpeed*60f*num;
                    if (!IsAnimating && CurrentSpeed != 0f)
                    {
                        PlayAnimation();
                    }
                }
                if (SpriteName == "EnemyFlailKnight_Character")
                {
                    m_walkSound.Update();
                    m_walkSound2.Update();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            if (!IsKilled)
            {
                foreach (var current in m_chainLinksList)
                {
                    m_chain.Position = current;
                    m_chain.Draw(camera);
                }
                BallAndChain.Draw(camera);
                if (Difficulty > GameTypes.EnemyDifficulty.Basic)
                {
                    foreach (var current2 in m_chainLinks2List)
                    {
                        m_chain.Position = current2;
                        m_chain.Draw(camera);
                    }
                    BallAndChain2.Draw(camera);
                }
            }
            base.Draw(camera);
        }

        public override void Kill(bool giveXP = true)
        {
            m_levelScreen.PhysicsManager.RemoveObject(BallAndChain);
            var enemyObj_BouncySpike = new EnemyObj_BouncySpike(m_target, null, m_levelScreen,
                Difficulty);
            enemyObj_BouncySpike.SavedStartingPos = Position;
            enemyObj_BouncySpike.Position = Position;
            m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike);
            enemyObj_BouncySpike.Position = BallAndChain.Position;
            enemyObj_BouncySpike.Speed = ChainSpeed*200f/m_BallSpeedDivider;
            enemyObj_BouncySpike.HeadingX =
                (float) Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
            enemyObj_BouncySpike.HeadingY =
                (float) Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
            if (Difficulty > GameTypes.EnemyDifficulty.Basic)
            {
                m_levelScreen.PhysicsManager.RemoveObject(BallAndChain2);
                var enemyObj_BouncySpike2 = new EnemyObj_BouncySpike(m_target, null, m_levelScreen,
                    Difficulty);
                enemyObj_BouncySpike2.SavedStartingPos = Position;
                enemyObj_BouncySpike2.Position = Position;
                m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike2);
                enemyObj_BouncySpike2.Position = BallAndChain2.Position;
                enemyObj_BouncySpike2.Speed = ChainSpeed*200f*ChainSpeed2Modifier/m_BallSpeedDivider;
                if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                {
                    enemyObj_BouncySpike2.HeadingX =
                        (float)
                            Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle*ChainSpeed2Modifier + 90f)));
                    enemyObj_BouncySpike2.HeadingY =
                        (float)
                            Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle*ChainSpeed2Modifier + 90f)));
                }
                else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                {
                    enemyObj_BouncySpike2.HeadingX =
                        (float)
                            Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle*ChainSpeed2Modifier + 90f)));
                    enemyObj_BouncySpike2.HeadingY =
                        (float)
                            Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle*ChainSpeed2Modifier + 90f)));
                }
                enemyObj_BouncySpike2.SpawnRoom = m_levelScreen.CurrentRoom;
                enemyObj_BouncySpike2.SaveToFile = false;
                if (IsPaused)
                {
                    enemyObj_BouncySpike2.PauseEnemy();
                }
            }
            enemyObj_BouncySpike.SpawnRoom = m_levelScreen.CurrentRoom;
            enemyObj_BouncySpike.SaveToFile = false;
            if (IsPaused)
            {
                enemyObj_BouncySpike.PauseEnemy();
            }
            base.Kill(giveXP);
        }

        public override void ResetState()
        {
            base.ResetState();
            m_actualChainRadius = 0f;
            m_chainLinkDistance = m_actualChainRadius/m_numChainLinks;
            var num = 0f;
            BallAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius,
                new Vector2(X, Bounds.Top));
            for (var i = 0; i < m_chainLinksList.Count; i++)
            {
                m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num, new Vector2(X, Bounds.Top));
                num += m_chainLinkDistance;
            }
            num = 0f;
            if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
            {
                BallAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle*ChainSpeed2Modifier,
                    m_actualChainRadius/2f, new Vector2(X, Bounds.Top));
            }
            else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
            {
                BallAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle*ChainSpeed2Modifier,
                    -m_actualChainRadius/2f, new Vector2(X, Bounds.Top));
            }
            for (var j = 0; j < m_chainLinks2List.Count; j++)
            {
                if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                {
                    m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle*ChainSpeed2Modifier, num,
                        new Vector2(X, Bounds.Top));
                }
                else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                {
                    m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle*ChainSpeed2Modifier, -num,
                        new Vector2(X, Bounds.Top));
                }
                num += m_chainLinkDistance;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, m_target, "Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_chain.Dispose();
                m_chain = null;
                BallAndChain.Dispose();
                BallAndChain = null;
                BallAndChain2.Dispose();
                BallAndChain2 = null;
                m_chainLinksList.Clear();
                m_chainLinksList = null;
                m_chainLinks2List.Clear();
                m_chainLinks2List = null;
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}
