/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;

namespace RogueCastle
{
    public class WordBuilder
    {
        private static string[] intro = new[]
        {
            "Sir Skunky the Knight\n"
        };

        private static string[] middle = new[]
        {
            "Gigantism - You are larger\n"
        };

        private static string[] end = new[]
        {
            "Dextrocardia - HP/MP pools are swapped\n"
        };

        public static string BuildDungeonName(GameTypes.LevelType levelType)
        {
            switch (levelType)
            {
                case GameTypes.LevelType.CASTLE:
                    return "Castle Hamson";
                case GameTypes.LevelType.GARDEN:
                    return "Forest Abkhazia";
                case GameTypes.LevelType.DUNGEON:
                    return "The Land of Darkness";
                case GameTypes.LevelType.TOWER:
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