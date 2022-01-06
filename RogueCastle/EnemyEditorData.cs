//
// RogueLegacyArchipelago - EnemyEditorData.cs
// Last Modified 2021-12-24
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
//
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using Microsoft.Xna.Framework;
using RogueCastle.Enums;

namespace RogueCastle
{
    public struct EnemyEditorData
    {
        public Vector2 AdvancedScale;
        public Vector2 BasicScale;
        public Vector2 ExpertScale;
        public Vector2 MinibossScale;
        public string SpriteName;
        public byte Type;

        public EnemyEditorData(byte enemyType)
        {
            var enemyObj = EnemyBuilder.BuildEnemy(enemyType, null, null, null, EnemyDifficulty.Basic);
            var enemyObj2 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, EnemyDifficulty.Advanced);
            var enemyObj3 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, EnemyDifficulty.Expert);
            var enemyObj4 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, EnemyDifficulty.MiniBoss);
            Type = enemyType;
            SpriteName = enemyObj.SpriteName;
            BasicScale = enemyObj.Scale;
            AdvancedScale = enemyObj2.Scale;
            ExpertScale = enemyObj3.Scale;
            MinibossScale = enemyObj4.Scale;
        }
    }
}