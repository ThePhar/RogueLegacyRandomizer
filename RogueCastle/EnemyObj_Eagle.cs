// 
// RogueLegacyArchipelago - EnemyObj_Eagle.cs
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
using RogueCastle.Structs;
using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public class EnemyObj_Eagle : EnemyObj
    {
        private readonly LogicBlock m_basicAttackLB = new LogicBlock();
        private readonly LogicBlock m_generalCooldownLB = new LogicBlock();
        private bool m_flyingLeft;

        public EnemyObj_Eagle(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            Type = 4;
        }

        protected override void InitializeEV()
        {
            MaxHealth = 10;
            Damage = 10;
            XPValue = 5;
            HealthGainPerLevel = 2;
            DamageGainPerLevel = 2;
            XPBonusPerLevel = 1;
            IsWeighted = false;
            Speed = 6f;
            EngageRadius = 1900;
            ProjectileRadius = 1600;
            MeleeRadius = 650;
            CooldownTime = 2f;
            CanBeKnockedBack = false;
            JumpHeight = 20.5f;
            CanFallOffLedges = true;
            TurnSpeed = 0.0175f;
            switch (Difficulty)
            {
                case EnemyDifficulty.Basic:
                case EnemyDifficulty.Advanced:
                case EnemyDifficulty.Expert:
                case EnemyDifficulty.MiniBoss:
                    return;
            }
        }

        protected override void InitializeLogic()
        {
            var logicSet = new LogicSet(this);
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
            logicSet.AddAction(new DelayLogicAction(1f));
            var logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f)));
            logicSet2.AddAction(new DelayLogicAction(1f));
            m_basicAttackLB.AddLogicSet(logicSet2, logicSet);
            logicBlocksToDispose.Add(m_basicAttackLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (m_flyingLeft)
                    {
                        var arg_3B_1 = false;
                        var arg_3B_2 = m_basicAttackLB;
                        var array = new int[2];
                        array[0] = 100;
                        RunLogicBlock(arg_3B_1, arg_3B_2, array);
                    }
                    else
                    {
                        RunLogicBlock(false, m_basicAttackLB, 0, 100);
                    }

                    if (X > m_levelScreen.CurrentRoom.Bounds.Right)
                    {
                        Y = m_levelScreen.CurrentRoom.Y +
                            CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = true;
                        return;
                    }

                    if (X < m_levelScreen.CurrentRoom.Bounds.Left)
                    {
                        Y = m_levelScreen.CurrentRoom.Y +
                            CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = false;
                    }

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
            if (!IsAnimating)
            {
                PlayAnimation();
            }

            base.Update(gameTime);
        }
    }
}