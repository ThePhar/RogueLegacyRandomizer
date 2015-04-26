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
    public class EnemyObj_Wall : EnemyObj
    {
        private readonly LogicBlock m_generalAdvancedLB = new LogicBlock();
        private readonly LogicBlock m_generalBasicLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private readonly LogicBlock m_generalExpertLB = new LogicBlock();
        private readonly LogicBlock m_generalMiniBossLB = new LogicBlock();

        public EnemyObj_Wall(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            CollisionTypeTag = 4;
            Type = 18;
        }

        protected override void InitializeEV()
        {
            XPValue = 5000;
            MaxHealth = 10000;
            Damage = 900;
            ProjectileDamage = 50;
            IsWeighted = false;
            Scale = new Vector2(1f, 3f);
            ProjectileScale = new Vector2(3f, 3f);
            Speed = 1.75f;
            EngageRadius = 1900;
            ProjectileRadius = 1600;
            MeleeRadius = 650;
            CooldownTime = 2f;
            KnockBack = new Vector2(5f, 6f);
            JumpHeight = 20.5f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = false;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.BASIC:
                case GameTypes.EnemyDifficulty.ADVANCED:
                case GameTypes.EnemyDifficulty.EXPERT:
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.5f, 1.25f));
            var logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.5f, 1.25f));
            var logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new MoveLogicAction(m_target, true));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ResizeProjectile", true));
            logicSet4.AddAction(new RunFunctionLogicAction(this, "ResizeProjectile", false));
            logicSet4.AddAction(new DelayLogicAction(3f));
            var logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new MoveLogicAction(m_target, true));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile"));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile"));
            logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile"));
            logicSet5.AddAction(new DelayLogicAction(1.3f));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5);
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20);
            base.InitializeLogic();
        }

        public void FireRandomProjectile()
        {
        }

        public void ResizeProjectile(bool resize)
        {
            if (resize)
            {
                ProjectileScale = new Vector2(2.5f, 2.5f);
                return;
            }
            ProjectileScale = new Vector2(2.5f, 2.5f);
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    RunLogicBlock(false, m_generalBasicLB, 0, 0, 0, 20, 80);
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
            HeadingY = 0f;
            if (HeadingX > 0f)
            {
                HeadingX = 1f;
            }
            else if (HeadingX < 0f)
            {
                HeadingX = -1f;
            }
            base.Update(gameTime);
            Y -= HeadingY;
        }
    }
}