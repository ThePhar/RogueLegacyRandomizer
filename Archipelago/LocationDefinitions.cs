//
//  Rogue Legacy Randomizer - LocationDefinitions.cs
//  Last Modified 2021-12-31
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago
{
    public static class LocationDefinitions
    {
        // Manor Upgrades
        public static readonly LocationData ManorGroundBase       = new LocationData(91000, "Manor Renovation - Ground Road");
        public static readonly LocationData ManorMainBase         = new LocationData(91001, "Manor Renovation - Main Base");
        public static readonly LocationData ManorMainBottomWindow = new LocationData(91002, "Manor Renovation - Main Bottom Window");
        public static readonly LocationData ManorMainTopWindow    = new LocationData(91003, "Manor Renovation - Main Top Window");
        public static readonly LocationData ManorMainRoof         = new LocationData(91004, "Manor Renovation - Main Rooftop");
        public static readonly LocationData ManorLeftWingBase     = new LocationData(91005, "Manor Renovation - Left Wing Base");
        public static readonly LocationData ManorLeftWingWindow   = new LocationData(91006, "Manor Renovation - Left Wing Window");
        public static readonly LocationData ManorLeftWingRoof     = new LocationData(91007, "Manor Renovation - Left Wing Rooftop");
        public static readonly LocationData ManorLeftBigBase      = new LocationData(91008, "Manor Renovation - Left Big Base");
        public static readonly LocationData ManorLeftBigUpper1    = new LocationData(91009, "Manor Renovation - Left Big Upper 1");
        public static readonly LocationData ManorLeftBigUpper2    = new LocationData(91010, "Manor Renovation - Left Big Upper 2");
        public static readonly LocationData ManorLeftBigWindows   = new LocationData(91011, "Manor Renovation - Left Big Windows");
        public static readonly LocationData ManorLeftBigRoof      = new LocationData(91012, "Manor Renovation - Left Big Rooftop");
        public static readonly LocationData ManorLeftFarBase      = new LocationData(91013, "Manor Renovation - Left Far Base");
        public static readonly LocationData ManorLeftFarRoof      = new LocationData(91014, "Manor Renovation - Left Far Roof");
        public static readonly LocationData ManorLeftExtension    = new LocationData(91015, "Manor Renovation - Left Extension");
        public static readonly LocationData ManorLeftTree1        = new LocationData(91016, "Manor Renovation - Left Tree 1");
        public static readonly LocationData ManorLeftTree2        = new LocationData(91017, "Manor Renovation - Left Tree 2");
        public static readonly LocationData ManorRightWingBase    = new LocationData(91018, "Manor Renovation - Right Wing Base");
        public static readonly LocationData ManorRightWingWindow  = new LocationData(91019, "Manor Renovation - Right Wing Window");
        public static readonly LocationData ManorRightWingRoof    = new LocationData(91020, "Manor Renovation - Right Wing Rooftop");
        public static readonly LocationData ManorRightBigBase     = new LocationData(91021, "Manor Renovation - Right Big Base");
        public static readonly LocationData ManorRightBigUpper    = new LocationData(91022, "Manor Renovation - Right Big Upper");
        public static readonly LocationData ManorRightBigRoof     = new LocationData(91023, "Manor Renovation - Right Big Rooftop");
        public static readonly LocationData ManorRightHighBase    = new LocationData(91024, "Manor Renovation - Right High Base");
        public static readonly LocationData ManorRightHighUpper   = new LocationData(91025, "Manor Renovation - Right High Upper");
        public static readonly LocationData ManorRightHighTower   = new LocationData(91026, "Manor Renovation - Right High Tower");
        public static readonly LocationData ManorRightExtension   = new LocationData(91027, "Manor Renovation - Right Extension");
        public static readonly LocationData ManorRightTree        = new LocationData(91028, "Manor Renovation - Right Tree");
        public static readonly LocationData ManorObservatoryBase  = new LocationData(91029, "Manor Renovation - Observatory Base");
        public static readonly LocationData ManorObservatoryScope = new LocationData(91030, "Manor Renovation - Observatory Telescope");

        // Boss Rewards
        public static readonly LocationData BossKhindr            = new LocationData(91100, "Khindr's Boss Chest");
        public static readonly LocationData BossAlexander         = new LocationData(91102, "Alexander's Boss Chest");
        public static readonly LocationData BossLeon              = new LocationData(91104, "Ponce de Leon's Boss Chest");
        public static readonly LocationData BossHerodotus         = new LocationData(91106, "Herodotus's Boss Chest");

        // Special Locations
        public static readonly LocationData SpecialJukebox        = new LocationData(91200, "Jukebox");

        // Diary Locations
        public static readonly LocationData Diary1                = new LocationData(91300, "Diary 1");
        public static readonly LocationData Diary2                = new LocationData(91301, "Diary 2");
        public static readonly LocationData Diary3                = new LocationData(91302, "Diary 3");
        public static readonly LocationData Diary4                = new LocationData(91303, "Diary 4");
        public static readonly LocationData Diary5                = new LocationData(91304, "Diary 5");
        public static readonly LocationData Diary6                = new LocationData(91305, "Diary 6");
        public static readonly LocationData Diary7                = new LocationData(91306, "Diary 7");
        public static readonly LocationData Diary8                = new LocationData(91307, "Diary 8");
        public static readonly LocationData Diary9                = new LocationData(91308, "Diary 9");
        public static readonly LocationData Diary10               = new LocationData(91309, "Diary 10");
        public static readonly LocationData Diary11               = new LocationData(91310, "Diary 11");
        public static readonly LocationData Diary12               = new LocationData(91311, "Diary 12");
        public static readonly LocationData Diary13               = new LocationData(91312, "Diary 13");
        public static readonly LocationData Diary14               = new LocationData(91313, "Diary 14");
        public static readonly LocationData Diary15               = new LocationData(91314, "Diary 15");
        public static readonly LocationData Diary16               = new LocationData(91315, "Diary 16");
        public static readonly LocationData Diary17               = new LocationData(91316, "Diary 17");
        public static readonly LocationData Diary18               = new LocationData(91317, "Diary 18");
        public static readonly LocationData Diary19               = new LocationData(91318, "Diary 19");
        public static readonly LocationData Diary20               = new LocationData(91319, "Diary 20");
        public static readonly LocationData Diary21               = new LocationData(91320, "Diary 21");
        public static readonly LocationData Diary22               = new LocationData(91321, "Diary 22");
        public static readonly LocationData Diary23               = new LocationData(91322, "Diary 23");
        public static readonly LocationData Diary24               = new LocationData(91323, "Diary 24");
        public static readonly LocationData Diary25               = new LocationData(91324, "Diary 25");

        // Chest Locations
        public static readonly LocationData ChestCastle1          = new LocationData(91600, "Castle Hamson - Chest 1");
        public static readonly LocationData ChestCastle2          = new LocationData(91601, "Castle Hamson - Chest 2");
        public static readonly LocationData ChestCastle3          = new LocationData(91602, "Castle Hamson - Chest 3");
        public static readonly LocationData ChestCastle4          = new LocationData(91603, "Castle Hamson - Chest 4");
        public static readonly LocationData ChestCastle5          = new LocationData(91604, "Castle Hamson - Chest 5");
        public static readonly LocationData ChestCastle6          = new LocationData(91605, "Castle Hamson - Chest 6");
        public static readonly LocationData ChestCastle7          = new LocationData(91606, "Castle Hamson - Chest 7");
        public static readonly LocationData ChestCastle8          = new LocationData(91607, "Castle Hamson - Chest 8");
        public static readonly LocationData ChestCastle9          = new LocationData(91608, "Castle Hamson - Chest 9");
        public static readonly LocationData ChestCastle10         = new LocationData(91609, "Castle Hamson - Chest 10");
        public static readonly LocationData ChestCastle11         = new LocationData(91610, "Castle Hamson - Chest 11");
        public static readonly LocationData ChestCastle12         = new LocationData(91611, "Castle Hamson - Chest 12");
        public static readonly LocationData ChestCastle13         = new LocationData(91612, "Castle Hamson - Chest 13");
        public static readonly LocationData ChestCastle14         = new LocationData(91613, "Castle Hamson - Chest 14");
        public static readonly LocationData ChestCastle15         = new LocationData(91614, "Castle Hamson - Chest 15");
        public static readonly LocationData ChestCastle16         = new LocationData(91615, "Castle Hamson - Chest 16");
        public static readonly LocationData ChestCastle17         = new LocationData(91616, "Castle Hamson - Chest 17");
        public static readonly LocationData ChestCastle18         = new LocationData(91617, "Castle Hamson - Chest 18");
        public static readonly LocationData ChestCastle19         = new LocationData(91618, "Castle Hamson - Chest 19");
        public static readonly LocationData ChestCastle20         = new LocationData(91619, "Castle Hamson - Chest 20");
        public static readonly LocationData ChestCastle21         = new LocationData(91620, "Castle Hamson - Chest 21");
        public static readonly LocationData ChestCastle22         = new LocationData(91621, "Castle Hamson - Chest 22");
        public static readonly LocationData ChestCastle23         = new LocationData(91622, "Castle Hamson - Chest 23");
        public static readonly LocationData ChestCastle24         = new LocationData(91623, "Castle Hamson - Chest 24");
        public static readonly LocationData ChestCastle25         = new LocationData(91624, "Castle Hamson - Chest 25");
        public static readonly LocationData ChestCastle26         = new LocationData(91625, "Castle Hamson - Chest 26");
        public static readonly LocationData ChestCastle27         = new LocationData(91626, "Castle Hamson - Chest 27");
        public static readonly LocationData ChestCastle28         = new LocationData(91627, "Castle Hamson - Chest 28");
        public static readonly LocationData ChestCastle29         = new LocationData(91628, "Castle Hamson - Chest 29");
        public static readonly LocationData ChestCastle30         = new LocationData(91629, "Castle Hamson - Chest 30");

        public static readonly LocationData ChestGarden1          = new LocationData(91700, "Forest Abkhazia - Chest 1");
        public static readonly LocationData ChestGarden2          = new LocationData(91701, "Forest Abkhazia - Chest 2");
        public static readonly LocationData ChestGarden3          = new LocationData(91702, "Forest Abkhazia - Chest 3");
        public static readonly LocationData ChestGarden4          = new LocationData(91703, "Forest Abkhazia - Chest 4");
        public static readonly LocationData ChestGarden5          = new LocationData(91704, "Forest Abkhazia - Chest 5");
        public static readonly LocationData ChestGarden6          = new LocationData(91705, "Forest Abkhazia - Chest 6");
        public static readonly LocationData ChestGarden7          = new LocationData(91706, "Forest Abkhazia - Chest 7");
        public static readonly LocationData ChestGarden8          = new LocationData(91707, "Forest Abkhazia - Chest 8");
        public static readonly LocationData ChestGarden9          = new LocationData(91708, "Forest Abkhazia - Chest 9");
        public static readonly LocationData ChestGarden10         = new LocationData(91709, "Forest Abkhazia - Chest 10");
        public static readonly LocationData ChestGarden11         = new LocationData(91710, "Forest Abkhazia - Chest 11");
        public static readonly LocationData ChestGarden12         = new LocationData(91711, "Forest Abkhazia - Chest 12");
        public static readonly LocationData ChestGarden13         = new LocationData(91712, "Forest Abkhazia - Chest 13");
        public static readonly LocationData ChestGarden14         = new LocationData(91713, "Forest Abkhazia - Chest 14");
        public static readonly LocationData ChestGarden15         = new LocationData(91714, "Forest Abkhazia - Chest 15");
        public static readonly LocationData ChestGarden16         = new LocationData(91715, "Forest Abkhazia - Chest 16");
        public static readonly LocationData ChestGarden17         = new LocationData(91716, "Forest Abkhazia - Chest 17");
        public static readonly LocationData ChestGarden18         = new LocationData(91717, "Forest Abkhazia - Chest 18");
        public static readonly LocationData ChestGarden19         = new LocationData(91718, "Forest Abkhazia - Chest 19");
        public static readonly LocationData ChestGarden20         = new LocationData(91719, "Forest Abkhazia - Chest 20");
        public static readonly LocationData ChestGarden21         = new LocationData(91720, "Forest Abkhazia - Chest 21");
        public static readonly LocationData ChestGarden22         = new LocationData(91721, "Forest Abkhazia - Chest 22");
        public static readonly LocationData ChestGarden23         = new LocationData(91722, "Forest Abkhazia - Chest 23");
        public static readonly LocationData ChestGarden24         = new LocationData(91723, "Forest Abkhazia - Chest 24");
        public static readonly LocationData ChestGarden25         = new LocationData(91724, "Forest Abkhazia - Chest 25");
        public static readonly LocationData ChestGarden26         = new LocationData(91725, "Forest Abkhazia - Chest 26");
        public static readonly LocationData ChestGarden27         = new LocationData(91726, "Forest Abkhazia - Chest 27");
        public static readonly LocationData ChestGarden28         = new LocationData(91727, "Forest Abkhazia - Chest 28");
        public static readonly LocationData ChestGarden29         = new LocationData(91728, "Forest Abkhazia - Chest 29");
        public static readonly LocationData ChestGarden30         = new LocationData(91729, "Forest Abkhazia - Chest 30");

        public static readonly LocationData ChestTower1           = new LocationData(91800, "The Maya - Chest 1");
        public static readonly LocationData ChestTower2           = new LocationData(91801, "The Maya - Chest 2");
        public static readonly LocationData ChestTower3           = new LocationData(91802, "The Maya - Chest 3");
        public static readonly LocationData ChestTower4           = new LocationData(91803, "The Maya - Chest 4");
        public static readonly LocationData ChestTower5           = new LocationData(91804, "The Maya - Chest 5");
        public static readonly LocationData ChestTower6           = new LocationData(91805, "The Maya - Chest 6");
        public static readonly LocationData ChestTower7           = new LocationData(91806, "The Maya - Chest 7");
        public static readonly LocationData ChestTower8           = new LocationData(91807, "The Maya - Chest 8");
        public static readonly LocationData ChestTower9           = new LocationData(91808, "The Maya - Chest 9");
        public static readonly LocationData ChestTower10          = new LocationData(91809, "The Maya - Chest 10");
        public static readonly LocationData ChestTower11          = new LocationData(91810, "The Maya - Chest 11");
        public static readonly LocationData ChestTower12          = new LocationData(91811, "The Maya - Chest 12");
        public static readonly LocationData ChestTower13          = new LocationData(91812, "The Maya - Chest 13");
        public static readonly LocationData ChestTower14          = new LocationData(91813, "The Maya - Chest 14");
        public static readonly LocationData ChestTower15          = new LocationData(91814, "The Maya - Chest 15");
        public static readonly LocationData ChestTower16          = new LocationData(91815, "The Maya - Chest 16");
        public static readonly LocationData ChestTower17          = new LocationData(91816, "The Maya - Chest 17");
        public static readonly LocationData ChestTower18          = new LocationData(91817, "The Maya - Chest 18");
        public static readonly LocationData ChestTower19          = new LocationData(91818, "The Maya - Chest 19");
        public static readonly LocationData ChestTower20          = new LocationData(91819, "The Maya - Chest 20");
        public static readonly LocationData ChestTower21          = new LocationData(91820, "The Maya - Chest 21");
        public static readonly LocationData ChestTower22          = new LocationData(91821, "The Maya - Chest 22");
        public static readonly LocationData ChestTower23          = new LocationData(91822, "The Maya - Chest 23");
        public static readonly LocationData ChestTower24          = new LocationData(91823, "The Maya - Chest 24");
        public static readonly LocationData ChestTower25          = new LocationData(91824, "The Maya - Chest 25");
        public static readonly LocationData ChestTower26          = new LocationData(91825, "The Maya - Chest 26");
        public static readonly LocationData ChestTower27          = new LocationData(91826, "The Maya - Chest 27");
        public static readonly LocationData ChestTower28          = new LocationData(91827, "The Maya - Chest 28");
        public static readonly LocationData ChestTower29          = new LocationData(91828, "The Maya - Chest 29");
        public static readonly LocationData ChestTower30          = new LocationData(91829, "The Maya - Chest 30");

        public static readonly LocationData ChestDungeon1         = new LocationData(91900, "The Land of Darkness - Chest 1");
        public static readonly LocationData ChestDungeon2         = new LocationData(91901, "The Land of Darkness - Chest 2");
        public static readonly LocationData ChestDungeon3         = new LocationData(91902, "The Land of Darkness - Chest 3");
        public static readonly LocationData ChestDungeon4         = new LocationData(91903, "The Land of Darkness - Chest 4");
        public static readonly LocationData ChestDungeon5         = new LocationData(91904, "The Land of Darkness - Chest 5");
        public static readonly LocationData ChestDungeon6         = new LocationData(91905, "The Land of Darkness - Chest 6");
        public static readonly LocationData ChestDungeon7         = new LocationData(91906, "The Land of Darkness - Chest 7");
        public static readonly LocationData ChestDungeon8         = new LocationData(91907, "The Land of Darkness - Chest 8");
        public static readonly LocationData ChestDungeon9         = new LocationData(91908, "The Land of Darkness - Chest 9");
        public static readonly LocationData ChestDungeon10        = new LocationData(91909, "The Land of Darkness - Chest 10");
        public static readonly LocationData ChestDungeon11        = new LocationData(91910, "The Land of Darkness - Chest 11");
        public static readonly LocationData ChestDungeon12        = new LocationData(91911, "The Land of Darkness - Chest 12");
        public static readonly LocationData ChestDungeon13        = new LocationData(91912, "The Land of Darkness - Chest 13");
        public static readonly LocationData ChestDungeon14        = new LocationData(91913, "The Land of Darkness - Chest 14");
        public static readonly LocationData ChestDungeon15        = new LocationData(91914, "The Land of Darkness - Chest 15");
        public static readonly LocationData ChestDungeon16        = new LocationData(91915, "The Land of Darkness - Chest 16");
        public static readonly LocationData ChestDungeon17        = new LocationData(91916, "The Land of Darkness - Chest 17");
        public static readonly LocationData ChestDungeon18        = new LocationData(91917, "The Land of Darkness - Chest 18");
        public static readonly LocationData ChestDungeon19        = new LocationData(91918, "The Land of Darkness - Chest 19");
        public static readonly LocationData ChestDungeon20        = new LocationData(91919, "The Land of Darkness - Chest 20");
        public static readonly LocationData ChestDungeon21        = new LocationData(91920, "The Land of Darkness - Chest 21");
        public static readonly LocationData ChestDungeon22        = new LocationData(91921, "The Land of Darkness - Chest 22");
        public static readonly LocationData ChestDungeon23        = new LocationData(91922, "The Land of Darkness - Chest 23");
        public static readonly LocationData ChestDungeon24        = new LocationData(91923, "The Land of Darkness - Chest 24");
        public static readonly LocationData ChestDungeon25        = new LocationData(91924, "The Land of Darkness - Chest 25");
        public static readonly LocationData ChestDungeon26        = new LocationData(91925, "The Land of Darkness - Chest 26");
        public static readonly LocationData ChestDungeon27        = new LocationData(91926, "The Land of Darkness - Chest 27");
        public static readonly LocationData ChestDungeon28        = new LocationData(91927, "The Land of Darkness - Chest 28");
        public static readonly LocationData ChestDungeon29        = new LocationData(91928, "The Land of Darkness - Chest 29");
        public static readonly LocationData ChestDungeon30        = new LocationData(91929, "The Land of Darkness - Chest 30");

        // Fairy Chests
        public static readonly LocationData FairyCastle1          = new LocationData(91400, "Castle Hamson - Fairy Chest 1");
        public static readonly LocationData FairyCastle2          = new LocationData(91401, "Castle Hamson - Fairy Chest 2");
        public static readonly LocationData FairyCastle3          = new LocationData(91402, "Castle Hamson - Fairy Chest 3");
        public static readonly LocationData FairyCastle4          = new LocationData(91403, "Castle Hamson - Fairy Chest 4");
        public static readonly LocationData FairyCastle5          = new LocationData(91404, "Castle Hamson - Fairy Chest 5");
        public static readonly LocationData FairyCastle6          = new LocationData(91405, "Castle Hamson - Fairy Chest 6");
        public static readonly LocationData FairyCastle7          = new LocationData(91406, "Castle Hamson - Fairy Chest 7");
        public static readonly LocationData FairyCastle8          = new LocationData(91407, "Castle Hamson - Fairy Chest 8");
        public static readonly LocationData FairyCastle9          = new LocationData(91408, "Castle Hamson - Fairy Chest 9");
        public static readonly LocationData FairyCastle10         = new LocationData(91409, "Castle Hamson - Fairy Chest 10");
        public static readonly LocationData FairyCastle11         = new LocationData(91410, "Castle Hamson - Fairy Chest 11");
        public static readonly LocationData FairyCastle12         = new LocationData(91411, "Castle Hamson - Fairy Chest 12");
        public static readonly LocationData FairyCastle13         = new LocationData(91412, "Castle Hamson - Fairy Chest 13");
        public static readonly LocationData FairyCastle14         = new LocationData(91413, "Castle Hamson - Fairy Chest 14");
        public static readonly LocationData FairyCastle15         = new LocationData(91414, "Castle Hamson - Fairy Chest 15");

        public static readonly LocationData FairyGarden1          = new LocationData(91450, "Forest Abkhazia - Fairy Chest 1");
        public static readonly LocationData FairyGarden2          = new LocationData(91451, "Forest Abkhazia - Fairy Chest 2");
        public static readonly LocationData FairyGarden3          = new LocationData(91452, "Forest Abkhazia - Fairy Chest 3");
        public static readonly LocationData FairyGarden4          = new LocationData(91453, "Forest Abkhazia - Fairy Chest 4");
        public static readonly LocationData FairyGarden5          = new LocationData(91454, "Forest Abkhazia - Fairy Chest 5");
        public static readonly LocationData FairyGarden6          = new LocationData(91455, "Forest Abkhazia - Fairy Chest 6");
        public static readonly LocationData FairyGarden7          = new LocationData(91456, "Forest Abkhazia - Fairy Chest 7");
        public static readonly LocationData FairyGarden8          = new LocationData(91457, "Forest Abkhazia - Fairy Chest 8");
        public static readonly LocationData FairyGarden9          = new LocationData(91458, "Forest Abkhazia - Fairy Chest 9");
        public static readonly LocationData FairyGarden10         = new LocationData(91459, "Forest Abkhazia - Fairy Chest 10");
        public static readonly LocationData FairyGarden11         = new LocationData(91460, "Forest Abkhazia - Fairy Chest 11");
        public static readonly LocationData FairyGarden12         = new LocationData(91461, "Forest Abkhazia - Fairy Chest 12");
        public static readonly LocationData FairyGarden13         = new LocationData(91462, "Forest Abkhazia - Fairy Chest 13");
        public static readonly LocationData FairyGarden14         = new LocationData(91463, "Forest Abkhazia - Fairy Chest 14");
        public static readonly LocationData FairyGarden15         = new LocationData(91464, "Forest Abkhazia - Fairy Chest 15");

        public static readonly LocationData FairyTower1           = new LocationData(91500, "The Maya - Fairy Chest 1");
        public static readonly LocationData FairyTower2           = new LocationData(91501, "The Maya - Fairy Chest 2");
        public static readonly LocationData FairyTower3           = new LocationData(91502, "The Maya - Fairy Chest 3");
        public static readonly LocationData FairyTower4           = new LocationData(91503, "The Maya - Fairy Chest 4");
        public static readonly LocationData FairyTower5           = new LocationData(91504, "The Maya - Fairy Chest 5");
        public static readonly LocationData FairyTower6           = new LocationData(91505, "The Maya - Fairy Chest 6");
        public static readonly LocationData FairyTower7           = new LocationData(91506, "The Maya - Fairy Chest 7");
        public static readonly LocationData FairyTower8           = new LocationData(91507, "The Maya - Fairy Chest 8");
        public static readonly LocationData FairyTower9           = new LocationData(91508, "The Maya - Fairy Chest 9");
        public static readonly LocationData FairyTower10          = new LocationData(91509, "The Maya - Fairy Chest 10");
        public static readonly LocationData FairyTower11          = new LocationData(91510, "The Maya - Fairy Chest 11");
        public static readonly LocationData FairyTower12          = new LocationData(91511, "The Maya - Fairy Chest 12");
        public static readonly LocationData FairyTower13          = new LocationData(91512, "The Maya - Fairy Chest 13");
        public static readonly LocationData FairyTower14          = new LocationData(91513, "The Maya - Fairy Chest 14");
        public static readonly LocationData FairyTower15          = new LocationData(91514, "The Maya - Fairy Chest 15");

        public static readonly LocationData FairyDungeon1         = new LocationData(91550, "The Land of Darkness - Fairy Chest 1");
        public static readonly LocationData FairyDungeon2         = new LocationData(91551, "The Land of Darkness - Fairy Chest 2");
        public static readonly LocationData FairyDungeon3         = new LocationData(91552, "The Land of Darkness - Fairy Chest 3");
        public static readonly LocationData FairyDungeon4         = new LocationData(91553, "The Land of Darkness - Fairy Chest 4");
        public static readonly LocationData FairyDungeon5         = new LocationData(91554, "The Land of Darkness - Fairy Chest 5");
        public static readonly LocationData FairyDungeon6         = new LocationData(91555, "The Land of Darkness - Fairy Chest 6");
        public static readonly LocationData FairyDungeon7         = new LocationData(91556, "The Land of Darkness - Fairy Chest 7");
        public static readonly LocationData FairyDungeon8         = new LocationData(91557, "The Land of Darkness - Fairy Chest 8");
        public static readonly LocationData FairyDungeon9         = new LocationData(91558, "The Land of Darkness - Fairy Chest 9");
        public static readonly LocationData FairyDungeon10        = new LocationData(91559, "The Land of Darkness - Fairy Chest 10");
        public static readonly LocationData FairyDungeon11        = new LocationData(91560, "The Land of Darkness - Fairy Chest 11");
        public static readonly LocationData FairyDungeon12        = new LocationData(91561, "The Land of Darkness - Fairy Chest 12");
        public static readonly LocationData FairyDungeon13        = new LocationData(91562, "The Land of Darkness - Fairy Chest 13");
        public static readonly LocationData FairyDungeon14        = new LocationData(91563, "The Land of Darkness - Fairy Chest 14");
        public static readonly LocationData FairyDungeon15        = new LocationData(91564, "The Land of Darkness - Fairy Chest 15");

        public static IEnumerable<LocationData> GetAllLocations()
        {
            return typeof(LocationDefinitions)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => field.FieldType == typeof(LocationData))
                .Select(field => (LocationData) field.GetValue(null))
                .ToList();
        }

        public static LocationData GetLocation(int code)
        {
            return GetAllLocations().First(location => location.Code == code);
        }

        public static LocationData GetLocation(string name)
        {
            return GetAllLocations().First(location => location.Name == name);
        }
    }
}
