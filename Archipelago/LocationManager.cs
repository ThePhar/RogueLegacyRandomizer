// 
//  RogueLegacyArchipelago - LocationManager.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using System.Linq;

namespace Archipelago
{
    public static class LocationManager
    {
        private static Dictionary<int, string> _locations = new Dictionary<int, string>();

        static LocationManager()
        {
            AddLocation(91000, "Manor Ground Road");
            AddLocation(91001, "Manor Main Base");
            AddLocation(91002, "Manor Main Bottom Window");
            AddLocation(91003, "Manor Main Top Window");
            AddLocation(91004, "Manor Main Roof");
            AddLocation(91005, "Manor Left Wing Base");
            AddLocation(91006, "Manor Left Wing Window");
            AddLocation(91007, "Manor Left Wing Roof");
            AddLocation(91008, "Manor Left Big Base");
            AddLocation(91009, "Manor Left Big Upper 1");
            AddLocation(91010, "Manor Left Big Upper 2");
            AddLocation(91011, "Manor Left Big Windows");
            AddLocation(91012, "Manor Left Big Roof");
            AddLocation(91013, "Manor Left Far Base");
            AddLocation(91014, "Manor Left Far Roof");
            AddLocation(91015, "Manor Left Extension");
            AddLocation(91016, "Manor Left Tree 1");
            AddLocation(91017, "Manor Left Tree 2");
            AddLocation(91018, "Manor Right Wing Base");
            AddLocation(91019, "Manor Right Wing Window");
            AddLocation(91020, "Manor Right Wing Roof");
            AddLocation(91021, "Manor Right Big Base");
            AddLocation(91022, "Manor Right Big Upper");
            AddLocation(91023, "Manor Right Big Roof");
            AddLocation(91024, "Manor Right High Base");
            AddLocation(91025, "Manor Right High Upper");
            AddLocation(91026, "Manor Right High Tower");
            AddLocation(91027, "Manor Right Extension");
            AddLocation(91028, "Manor Right Tree");
            AddLocation(91029, "Manor Observatory Base");
            AddLocation(91030, "Manor Observatory Telescope");
            AddLocation(91100, "Khindr's Reward Chest");
            AddLocation(91102, "Alexander's Reward Chest");
            AddLocation(91104, "Ponce de Leon's Reward Chest");
            AddLocation(91106, "Herodotus's Reward Chest");
            AddLocation(91200, "Jukebox");

            for (var i = 0; i < 25; i++)
            {
                AddLocation(91300 + i, string.Format("Diary {0}", i + 1));
            }

            for (var i = 0; i < 50; i++)
            {
                AddLocation(91400 + i, string.Format("Castle Hamson Fairy Chest {0}", i + 1));
                AddLocation(91450 + i, string.Format("Forest Abkhazia Fairy Chest {0}", i + 1));
                AddLocation(91500 + i, string.Format("The Maya Fairy Chest {0}", i + 1));
                AddLocation(91550 + i, string.Format("The Land of Darkness Fairy Chest {0}", i + 1));
            }

            for (var i = 0; i < 100; i++)
            {
                AddLocation(91600 + i, string.Format("Castle Hamson Chest {0}", i + 1));
                AddLocation(91700 + i, string.Format("Forest Abkhazia Chest {0}", i + 1));
                AddLocation(91800 + i, string.Format("The Maya Chest {0}", i + 1));
                AddLocation(91900 + i, string.Format("The Land of Darkness Chest {0}", i + 1));
            }
        }

        public static void AddLocation(int code, string name)
        {
            _locations.Add(code, name);
        }

        public static void Clear()
        {
            _locations = new Dictionary<int, string>();
        }

        public static int GetCodeByName(string location)
        {
            return _locations.First(kp => kp.Value == location).Key;
        }

        public static string GetNameByCode(int code)
        {
            string name;
            _locations.TryGetValue(code, out name);

            // Return our name if it was in our table otherwise get a safe default.
            return name != "" ? name : "Unknown Location";
        }
    }
}
