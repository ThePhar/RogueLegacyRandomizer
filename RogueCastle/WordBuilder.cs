// 
// RogueLegacyArchipelago - WordBuilder.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using RogueCastle.Structs;

namespace RogueCastle
{
    public class WordBuilder
    {
        private static readonly string[] intro =
        {
            "Sir Skunky the Knight\n"
        };

        private static readonly string[] middle =
        {
            "Gigantism - You are larger\n"
        };

        private static readonly string[] end =
        {
            "Dextrocardia - HP/MP pools are swapped\n"
        };

        public static string BuildDungeonName(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Castle:
                    return "Castle Hamson";
                case LevelType.Garden:
                    return "Forest Abkhazia";
                case LevelType.Dungeon:
                    return "The Land of Darkness";
                case LevelType.Tower:
                    return "The Maya";
                default:
                    return "";
            }
        }

        public static void RandomIntro(TextObj textObj, int wordWrap = 0)
        {
            textObj.Text = intro[CDGMath.RandomInt(0, intro.Length - 1)] +
                           middle[CDGMath.RandomInt(0, middle.Length - 1)] + end[CDGMath.RandomInt(0, end.Length - 1)];
            if (wordWrap > 0)
            {
                textObj.WordWrap(wordWrap);
            }
        }
    }
}
