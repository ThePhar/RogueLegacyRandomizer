/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public class EnemyObj_Turret : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        protected override void InitializeEV()
        {
            Scale = new Vector2(2f, 2f);
            AnimationDelay = 0.1f;
            Speed = 0f;
            MaxHealth = 10;
            EngageRadius = 30;
            ProjectileRadius = 20;
            MeleeRadius = 10;
            CooldownTime = 2f;
            KnockBack = new Vector2(1f, 2f);
            Damage = 25;
            JumpHeight = 20.5f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            XPValue = 2;
            CanBeKnockedBack = false;
            LockFlip = true;
            IsWeighted = false;
            IsCollidable = false;
            Name = "Wall Turret";
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
            float rotation = Rotation;
            float num = ParseTagToFloat("delay");
            float num2 = ParseTagToFloat("speed");
            if (num == 0f)
            {
                Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
                num = 10000f;
            }
            ProjectileData projectileData = new ProjectileData(this)
            {
                SpriteName = "TurretProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(num2, num2),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(rotation, rotation),
                CollidesWithTerrain = true,
                Scale = ProjectileScale
            };
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
            logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02",
                "Turret_Attack03"));
            logicSet.AddAction(new DelayLogicAction(num));
            m_generalBasicLB.AddLogicSet(logicSet);
            m_generalAdvancedLB.AddLogicSet(logicSet);
            m_generalExpertLB.AddLogicSet(logicSet);
            m_generalMiniBossLB.AddLogicSet(logicSet);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            projectileData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            /*switch (State)
            {
                case 0:
                case 1:
                case 2:
                case 3:*/
                    //IL_1D:
                    RunLogicBlock(false, m_generalBasicLB, 100);
                    //return;
            //}
            //goto IL_1D;
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

        public EnemyObj_Turret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("EnemyTurretFire_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            IsCollidable = false;
            ForceDraw = true;
            Type = 17;
            StopAnimation();
            PlayAnimationOnRestart = false;
            NonKillable = true;
        }
    }
}