using DS2DEngine;

namespace RogueCastle
{
	public class WordBuilder
	{
		private static string[] intro = new string[]
		{
			"Sir Skunky the Knight\n"
		};
		private static string[] middle = new string[]
		{
			"Gigantism - You are larger\n"
		};
		private static string[] end = new string[]
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
			textObj.Text = intro[CDGMath.RandomInt(0, intro.Length - 1)] + middle[CDGMath.RandomInt(0, middle.Length - 1)] + end[CDGMath.RandomInt(0, end.Length - 1)];
			if (wordWrap > 0)
			{
				textObj.WordWrap(wordWrap);
			}
		}
	}
}
