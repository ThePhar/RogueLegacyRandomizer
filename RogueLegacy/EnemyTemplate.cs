// Rogue Legacy Randomizer - EnemyTemplate.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class EnemyTemplate : EnemyObj
    {
        public EnemyTemplate(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemySpriteNameGoesHere", target, physicsManager, levelToAttachTo, difficulty) { }

        protected override void InitializeEV()
        {
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
    }
}
