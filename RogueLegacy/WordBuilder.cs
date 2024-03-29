// Rogue Legacy Randomizer - WordBuilder.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using RogueLegacy.Enums;

namespace RogueLegacy
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

        public static string BuildDungeonName(Zone zone)
        {
            switch (zone)
            {
                case Zone.Castle:
                    return "Castle Hamson";

                case Zone.Garden:
                    return "Forest Abkhazia";

                case Zone.Dungeon:
                    return "The Land of Darkness";

                case Zone.Tower:
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
