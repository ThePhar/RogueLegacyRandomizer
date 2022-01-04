using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago
{
    public static class LocationDefinitions
    {
        // Manor Upgrades
        public static readonly LocationData ManorGroundRoad       = new(91000, "Manor Renovation - Ground Road");
        public static readonly LocationData ManorMainBase         = new(91001, "Manor Renovation - Main Base");
        public static readonly LocationData ManorMainBottomWindow = new(91002, "Manor Renovation - Main Bottom Window");
        public static readonly LocationData ManorMainTopWindow    = new(91003, "Manor Renovation - Main Top Window");
        public static readonly LocationData ManorMainRoof         = new(91004, "Manor Renovation - Main Rooftop");
        public static readonly LocationData ManorLeftWingBase     = new(91005, "Manor Renovation - Left Wing Base");
        public static readonly LocationData ManorLeftWingWindow   = new(91006, "Manor Renovation - Left Wing Window");
        public static readonly LocationData ManorLeftWingRoof     = new(91007, "Manor Renovation - Left Wing Rooftop");
        public static readonly LocationData ManorLeftBigBase      = new(91008, "Manor Renovation - Left Big Base");
        public static readonly LocationData ManorLeftBigUpper1    = new(91009, "Manor Renovation - Left Big Upper 1");
        public static readonly LocationData ManorLeftBigUpper2    = new(91010, "Manor Renovation - Left Big Upper 2");
        public static readonly LocationData ManorLeftBigWindows   = new(91011, "Manor Renovation - Left Big Windows");
        public static readonly LocationData ManorLeftBigRoof      = new(91012, "Manor Renovation - Left Big Rooftop");
        public static readonly LocationData ManorLeftFarBase      = new(91013, "Manor Renovation - Left Far Base");
        public static readonly LocationData ManorLeftFarRoof      = new(91014, "Manor Renovation - Left Far Roof");
        public static readonly LocationData ManorLeftExtension    = new(91015, "Manor Renovation - Left Extension");
        public static readonly LocationData ManorLeftTree1        = new(91016, "Manor Renovation - Left Tree 1");
        public static readonly LocationData ManorLeftTree2        = new(91017, "Manor Renovation - Left Tree 2");
        public static readonly LocationData ManorRightWingBase    = new(91018, "Manor Renovation - Right Wing Base");
        public static readonly LocationData ManorRightWingWindow  = new(91019, "Manor Renovation - Right Wing Window");
        public static readonly LocationData ManorRightWingRoof    = new(91020, "Manor Renovation - Right Wing Rooftop");
        public static readonly LocationData ManorRightBigBase     = new(91021, "Manor Renovation - Right Big Base");
        public static readonly LocationData ManorRightBigUpper    = new(91022, "Manor Renovation - Right Big Upper");
        public static readonly LocationData ManorRightBigRoof     = new(91023, "Manor Renovation - Right Big Rooftop");
        public static readonly LocationData ManorRightHighBase    = new(91024, "Manor Renovation - Right High Base");
        public static readonly LocationData ManorRightHighUpper   = new(91025, "Manor Renovation - Right High Upper");
        public static readonly LocationData ManorRightHighTower   = new(91026, "Manor Renovation - Right High Tower");
        public static readonly LocationData ManorRightExtension   = new(91027, "Manor Renovation - Right Extension");
        public static readonly LocationData ManorRightTree        = new(91028, "Manor Renovation - Right Tree");
        public static readonly LocationData ManorObservatoryBase  = new(91029, "Manor Renovation - Observatory Base");
        public static readonly LocationData ManorObservatoryScope = new(91030, "Manor Renovation - Observatory Telescope");

        // Boss Rewards
        public static readonly LocationData BossKhindr            = new(91100, "Khindr's Boss Chest");
        public static readonly LocationData BossAlexander         = new(91102, "Alexander's Boss Chest");
        public static readonly LocationData BossLeon              = new(91104, "Ponce de Leon's Boss Chest");
        public static readonly LocationData BossHerodotus         = new(91106, "Herodotus's Boss Chest");

        // Special Locations
        public static readonly LocationData SpecialJukebox        = new(91200, "Jukebox");

        // Diary Locations
        public static readonly LocationData Diary1                = new(91300, "Diary 1");
        public static readonly LocationData Diary2                = new(91301, "Diary 2");
        public static readonly LocationData Diary3                = new(91302, "Diary 3");
        public static readonly LocationData Diary4                = new(91303, "Diary 4");
        public static readonly LocationData Diary5                = new(91304, "Diary 5");
        public static readonly LocationData Diary6                = new(91305, "Diary 6");
        public static readonly LocationData Diary7                = new(91306, "Diary 7");
        public static readonly LocationData Diary8                = new(91307, "Diary 8");
        public static readonly LocationData Diary9                = new(91308, "Diary 9");
        public static readonly LocationData Diary10               = new(91309, "Diary 10");
        public static readonly LocationData Diary11               = new(91310, "Diary 11");
        public static readonly LocationData Diary12               = new(91311, "Diary 12");
        public static readonly LocationData Diary13               = new(91312, "Diary 13");
        public static readonly LocationData Diary14               = new(91313, "Diary 14");
        public static readonly LocationData Diary15               = new(91314, "Diary 15");
        public static readonly LocationData Diary16               = new(91315, "Diary 16");
        public static readonly LocationData Diary17               = new(91316, "Diary 17");
        public static readonly LocationData Diary18               = new(91317, "Diary 18");
        public static readonly LocationData Diary19               = new(91318, "Diary 19");
        public static readonly LocationData Diary20               = new(91319, "Diary 20");
        public static readonly LocationData Diary21               = new(91320, "Diary 21");
        public static readonly LocationData Diary22               = new(91321, "Diary 22");
        public static readonly LocationData Diary23               = new(91322, "Diary 23");
        public static readonly LocationData Diary24               = new(91323, "Diary 24");
        public static readonly LocationData Diary25               = new(91324, "Diary 25");

        // Chest Locations
        public static readonly LocationData ChestCastle1          = new(91600, "Castle Hamson - Chest 1");
        public static readonly LocationData ChestCastle2          = new(91601, "Castle Hamson - Chest 2");
        public static readonly LocationData ChestCastle3          = new(91602, "Castle Hamson - Chest 3");
        public static readonly LocationData ChestCastle4          = new(91603, "Castle Hamson - Chest 4");
        public static readonly LocationData ChestCastle5          = new(91604, "Castle Hamson - Chest 5");
        public static readonly LocationData ChestCastle6          = new(91605, "Castle Hamson - Chest 6");
        public static readonly LocationData ChestCastle7          = new(91606, "Castle Hamson - Chest 7");
        public static readonly LocationData ChestCastle8          = new(91607, "Castle Hamson - Chest 8");
        public static readonly LocationData ChestCastle9          = new(91608, "Castle Hamson - Chest 9");
        public static readonly LocationData ChestCastle10         = new(91609, "Castle Hamson - Chest 10");
        public static readonly LocationData ChestCastle11         = new(91610, "Castle Hamson - Chest 11");
        public static readonly LocationData ChestCastle12         = new(91611, "Castle Hamson - Chest 12");
        public static readonly LocationData ChestCastle13         = new(91612, "Castle Hamson - Chest 13");
        public static readonly LocationData ChestCastle14         = new(91613, "Castle Hamson - Chest 14");
        public static readonly LocationData ChestCastle15         = new(91614, "Castle Hamson - Chest 15");
        public static readonly LocationData ChestCastle16         = new(91615, "Castle Hamson - Chest 16");
        public static readonly LocationData ChestCastle17         = new(91616, "Castle Hamson - Chest 17");
        public static readonly LocationData ChestCastle18         = new(91617, "Castle Hamson - Chest 18");
        public static readonly LocationData ChestCastle19         = new(91618, "Castle Hamson - Chest 19");
        public static readonly LocationData ChestCastle20         = new(91619, "Castle Hamson - Chest 20");
        public static readonly LocationData ChestCastle21         = new(91620, "Castle Hamson - Chest 21");
        public static readonly LocationData ChestCastle22         = new(91621, "Castle Hamson - Chest 22");
        public static readonly LocationData ChestCastle23         = new(91622, "Castle Hamson - Chest 23");
        public static readonly LocationData ChestCastle24         = new(91623, "Castle Hamson - Chest 24");
        public static readonly LocationData ChestCastle25         = new(91624, "Castle Hamson - Chest 25");
        public static readonly LocationData ChestCastle26         = new(91625, "Castle Hamson - Chest 26");
        public static readonly LocationData ChestCastle27         = new(91626, "Castle Hamson - Chest 27");
        public static readonly LocationData ChestCastle28         = new(91627, "Castle Hamson - Chest 28");
        public static readonly LocationData ChestCastle29         = new(91628, "Castle Hamson - Chest 29");
        public static readonly LocationData ChestCastle30         = new(91629, "Castle Hamson - Chest 30");

        public static readonly LocationData ChestGarden1          = new(91700, "Forest Abkhazia - Chest 1");
        public static readonly LocationData ChestGarden2          = new(91701, "Forest Abkhazia - Chest 2");
        public static readonly LocationData ChestGarden3          = new(91702, "Forest Abkhazia - Chest 3");
        public static readonly LocationData ChestGarden4          = new(91703, "Forest Abkhazia - Chest 4");
        public static readonly LocationData ChestGarden5          = new(91704, "Forest Abkhazia - Chest 5");
        public static readonly LocationData ChestGarden6          = new(91705, "Forest Abkhazia - Chest 6");
        public static readonly LocationData ChestGarden7          = new(91706, "Forest Abkhazia - Chest 7");
        public static readonly LocationData ChestGarden8          = new(91707, "Forest Abkhazia - Chest 8");
        public static readonly LocationData ChestGarden9          = new(91708, "Forest Abkhazia - Chest 9");
        public static readonly LocationData ChestGarden10         = new(91709, "Forest Abkhazia - Chest 10");
        public static readonly LocationData ChestGarden11         = new(91710, "Forest Abkhazia - Chest 11");
        public static readonly LocationData ChestGarden12         = new(91711, "Forest Abkhazia - Chest 12");
        public static readonly LocationData ChestGarden13         = new(91712, "Forest Abkhazia - Chest 13");
        public static readonly LocationData ChestGarden14         = new(91713, "Forest Abkhazia - Chest 14");
        public static readonly LocationData ChestGarden15         = new(91714, "Forest Abkhazia - Chest 15");
        public static readonly LocationData ChestGarden16         = new(91715, "Forest Abkhazia - Chest 16");
        public static readonly LocationData ChestGarden17         = new(91716, "Forest Abkhazia - Chest 17");
        public static readonly LocationData ChestGarden18         = new(91717, "Forest Abkhazia - Chest 18");
        public static readonly LocationData ChestGarden19         = new(91718, "Forest Abkhazia - Chest 19");
        public static readonly LocationData ChestGarden20         = new(91719, "Forest Abkhazia - Chest 20");
        public static readonly LocationData ChestGarden21         = new(91720, "Forest Abkhazia - Chest 21");
        public static readonly LocationData ChestGarden22         = new(91721, "Forest Abkhazia - Chest 22");
        public static readonly LocationData ChestGarden23         = new(91722, "Forest Abkhazia - Chest 23");
        public static readonly LocationData ChestGarden24         = new(91723, "Forest Abkhazia - Chest 24");
        public static readonly LocationData ChestGarden25         = new(91724, "Forest Abkhazia - Chest 25");
        public static readonly LocationData ChestGarden26         = new(91725, "Forest Abkhazia - Chest 26");
        public static readonly LocationData ChestGarden27         = new(91726, "Forest Abkhazia - Chest 27");
        public static readonly LocationData ChestGarden28         = new(91727, "Forest Abkhazia - Chest 28");
        public static readonly LocationData ChestGarden29         = new(91728, "Forest Abkhazia - Chest 29");
        public static readonly LocationData ChestGarden30         = new(91729, "Forest Abkhazia - Chest 30");

        public static readonly LocationData ChestTower1           = new(91800, "The Maya - Chest 1");
        public static readonly LocationData ChestTower2           = new(91801, "The Maya - Chest 2");
        public static readonly LocationData ChestTower3           = new(91802, "The Maya - Chest 3");
        public static readonly LocationData ChestTower4           = new(91803, "The Maya - Chest 4");
        public static readonly LocationData ChestTower5           = new(91804, "The Maya - Chest 5");
        public static readonly LocationData ChestTower6           = new(91805, "The Maya - Chest 6");
        public static readonly LocationData ChestTower7           = new(91806, "The Maya - Chest 7");
        public static readonly LocationData ChestTower8           = new(91807, "The Maya - Chest 8");
        public static readonly LocationData ChestTower9           = new(91808, "The Maya - Chest 9");
        public static readonly LocationData ChestTower10          = new(91809, "The Maya - Chest 10");
        public static readonly LocationData ChestTower11          = new(91810, "The Maya - Chest 11");
        public static readonly LocationData ChestTower12          = new(91811, "The Maya - Chest 12");
        public static readonly LocationData ChestTower13          = new(91812, "The Maya - Chest 13");
        public static readonly LocationData ChestTower14          = new(91813, "The Maya - Chest 14");
        public static readonly LocationData ChestTower15          = new(91814, "The Maya - Chest 15");
        public static readonly LocationData ChestTower16          = new(91815, "The Maya - Chest 16");
        public static readonly LocationData ChestTower17          = new(91816, "The Maya - Chest 17");
        public static readonly LocationData ChestTower18          = new(91817, "The Maya - Chest 18");
        public static readonly LocationData ChestTower19          = new(91818, "The Maya - Chest 19");
        public static readonly LocationData ChestTower20          = new(91819, "The Maya - Chest 20");
        public static readonly LocationData ChestTower21          = new(91820, "The Maya - Chest 21");
        public static readonly LocationData ChestTower22          = new(91821, "The Maya - Chest 22");
        public static readonly LocationData ChestTower23          = new(91822, "The Maya - Chest 23");
        public static readonly LocationData ChestTower24          = new(91823, "The Maya - Chest 24");
        public static readonly LocationData ChestTower25          = new(91824, "The Maya - Chest 25");
        public static readonly LocationData ChestTower26          = new(91825, "The Maya - Chest 26");
        public static readonly LocationData ChestTower27          = new(91826, "The Maya - Chest 27");
        public static readonly LocationData ChestTower28          = new(91827, "The Maya - Chest 28");
        public static readonly LocationData ChestTower29          = new(91828, "The Maya - Chest 29");
        public static readonly LocationData ChestTower30          = new(91829, "The Maya - Chest 30");

        public static readonly LocationData ChestDungeon1         = new(91900, "The Land of Darkness - Chest 1");
        public static readonly LocationData ChestDungeon2         = new(91901, "The Land of Darkness - Chest 2");
        public static readonly LocationData ChestDungeon3         = new(91902, "The Land of Darkness - Chest 3");
        public static readonly LocationData ChestDungeon4         = new(91903, "The Land of Darkness - Chest 4");
        public static readonly LocationData ChestDungeon5         = new(91904, "The Land of Darkness - Chest 5");
        public static readonly LocationData ChestDungeon6         = new(91905, "The Land of Darkness - Chest 6");
        public static readonly LocationData ChestDungeon7         = new(91906, "The Land of Darkness - Chest 7");
        public static readonly LocationData ChestDungeon8         = new(91907, "The Land of Darkness - Chest 8");
        public static readonly LocationData ChestDungeon9         = new(91908, "The Land of Darkness - Chest 9");
        public static readonly LocationData ChestDungeon10        = new(91909, "The Land of Darkness - Chest 10");
        public static readonly LocationData ChestDungeon11        = new(91910, "The Land of Darkness - Chest 11");
        public static readonly LocationData ChestDungeon12        = new(91911, "The Land of Darkness - Chest 12");
        public static readonly LocationData ChestDungeon13        = new(91912, "The Land of Darkness - Chest 13");
        public static readonly LocationData ChestDungeon14        = new(91913, "The Land of Darkness - Chest 14");
        public static readonly LocationData ChestDungeon15        = new(91914, "The Land of Darkness - Chest 15");
        public static readonly LocationData ChestDungeon16        = new(91915, "The Land of Darkness - Chest 16");
        public static readonly LocationData ChestDungeon17        = new(91916, "The Land of Darkness - Chest 17");
        public static readonly LocationData ChestDungeon18        = new(91917, "The Land of Darkness - Chest 18");
        public static readonly LocationData ChestDungeon19        = new(91918, "The Land of Darkness - Chest 19");
        public static readonly LocationData ChestDungeon20        = new(91919, "The Land of Darkness - Chest 20");
        public static readonly LocationData ChestDungeon21        = new(91920, "The Land of Darkness - Chest 21");
        public static readonly LocationData ChestDungeon22        = new(91921, "The Land of Darkness - Chest 22");
        public static readonly LocationData ChestDungeon23        = new(91922, "The Land of Darkness - Chest 23");
        public static readonly LocationData ChestDungeon24        = new(91923, "The Land of Darkness - Chest 24");
        public static readonly LocationData ChestDungeon25        = new(91924, "The Land of Darkness - Chest 25");
        public static readonly LocationData ChestDungeon26        = new(91925, "The Land of Darkness - Chest 26");
        public static readonly LocationData ChestDungeon27        = new(91926, "The Land of Darkness - Chest 27");
        public static readonly LocationData ChestDungeon28        = new(91927, "The Land of Darkness - Chest 28");
        public static readonly LocationData ChestDungeon29        = new(91928, "The Land of Darkness - Chest 29");
        public static readonly LocationData ChestDungeon30        = new(91929, "The Land of Darkness - Chest 30");

        // Fairy Chests
        public static readonly LocationData FairyCastle1          = new(91400, "Castle Hamson - Fairy Chest 1");
        public static readonly LocationData FairyCastle2          = new(91401, "Castle Hamson - Fairy Chest 2");
        public static readonly LocationData FairyCastle3          = new(91402, "Castle Hamson - Fairy Chest 3");
        public static readonly LocationData FairyCastle4          = new(91403, "Castle Hamson - Fairy Chest 4");
        public static readonly LocationData FairyCastle5          = new(91404, "Castle Hamson - Fairy Chest 5");
        public static readonly LocationData FairyCastle6          = new(91405, "Castle Hamson - Fairy Chest 6");
        public static readonly LocationData FairyCastle7          = new(91406, "Castle Hamson - Fairy Chest 7");
        public static readonly LocationData FairyCastle8          = new(91407, "Castle Hamson - Fairy Chest 8");
        public static readonly LocationData FairyCastle9          = new(91408, "Castle Hamson - Fairy Chest 9");
        public static readonly LocationData FairyCastle10         = new(91409, "Castle Hamson - Fairy Chest 10");
        public static readonly LocationData FairyCastle11         = new(91410, "Castle Hamson - Fairy Chest 11");
        public static readonly LocationData FairyCastle12         = new(91411, "Castle Hamson - Fairy Chest 12");
        public static readonly LocationData FairyCastle13         = new(91412, "Castle Hamson - Fairy Chest 13");
        public static readonly LocationData FairyCastle14         = new(91413, "Castle Hamson - Fairy Chest 14");
        public static readonly LocationData FairyCastle15         = new(91414, "Castle Hamson - Fairy Chest 15");

        public static readonly LocationData FairyGarden1          = new(91450, "Forest Abkhazia - Fairy Chest 1");
        public static readonly LocationData FairyGarden2          = new(91451, "Forest Abkhazia - Fairy Chest 2");
        public static readonly LocationData FairyGarden3          = new(91452, "Forest Abkhazia - Fairy Chest 3");
        public static readonly LocationData FairyGarden4          = new(91453, "Forest Abkhazia - Fairy Chest 4");
        public static readonly LocationData FairyGarden5          = new(91454, "Forest Abkhazia - Fairy Chest 5");
        public static readonly LocationData FairyGarden6          = new(91455, "Forest Abkhazia - Fairy Chest 6");
        public static readonly LocationData FairyGarden7          = new(91456, "Forest Abkhazia - Fairy Chest 7");
        public static readonly LocationData FairyGarden8          = new(91457, "Forest Abkhazia - Fairy Chest 8");
        public static readonly LocationData FairyGarden9          = new(91458, "Forest Abkhazia - Fairy Chest 9");
        public static readonly LocationData FairyGarden10         = new(91459, "Forest Abkhazia - Fairy Chest 10");
        public static readonly LocationData FairyGarden11         = new(91460, "Forest Abkhazia - Fairy Chest 11");
        public static readonly LocationData FairyGarden12         = new(91461, "Forest Abkhazia - Fairy Chest 12");
        public static readonly LocationData FairyGarden13         = new(91462, "Forest Abkhazia - Fairy Chest 13");
        public static readonly LocationData FairyGarden14         = new(91463, "Forest Abkhazia - Fairy Chest 14");
        public static readonly LocationData FairyGarden15         = new(91464, "Forest Abkhazia - Fairy Chest 15");

        public static readonly LocationData FairyTower1           = new(91500, "The Maya - Fairy Chest 1");
        public static readonly LocationData FairyTower2           = new(91501, "The Maya - Fairy Chest 2");
        public static readonly LocationData FairyTower3           = new(91502, "The Maya - Fairy Chest 3");
        public static readonly LocationData FairyTower4           = new(91503, "The Maya - Fairy Chest 4");
        public static readonly LocationData FairyTower5           = new(91504, "The Maya - Fairy Chest 5");
        public static readonly LocationData FairyTower6           = new(91505, "The Maya - Fairy Chest 6");
        public static readonly LocationData FairyTower7           = new(91506, "The Maya - Fairy Chest 7");
        public static readonly LocationData FairyTower8           = new(91507, "The Maya - Fairy Chest 8");
        public static readonly LocationData FairyTower9           = new(91508, "The Maya - Fairy Chest 9");
        public static readonly LocationData FairyTower10          = new(91509, "The Maya - Fairy Chest 10");
        public static readonly LocationData FairyTower11          = new(91510, "The Maya - Fairy Chest 11");
        public static readonly LocationData FairyTower12          = new(91511, "The Maya - Fairy Chest 12");
        public static readonly LocationData FairyTower13          = new(91512, "The Maya - Fairy Chest 13");
        public static readonly LocationData FairyTower14          = new(91513, "The Maya - Fairy Chest 14");
        public static readonly LocationData FairyTower15          = new(91514, "The Maya - Fairy Chest 15");

        public static readonly LocationData FairyDungeon1         = new(91550, "The Land of Darkness - Fairy Chest 1");
        public static readonly LocationData FairyDungeon2         = new(91551, "The Land of Darkness - Fairy Chest 2");
        public static readonly LocationData FairyDungeon3         = new(91552, "The Land of Darkness - Fairy Chest 3");
        public static readonly LocationData FairyDungeon4         = new(91553, "The Land of Darkness - Fairy Chest 4");
        public static readonly LocationData FairyDungeon5         = new(91554, "The Land of Darkness - Fairy Chest 5");
        public static readonly LocationData FairyDungeon6         = new(91555, "The Land of Darkness - Fairy Chest 6");
        public static readonly LocationData FairyDungeon7         = new(91556, "The Land of Darkness - Fairy Chest 7");
        public static readonly LocationData FairyDungeon8         = new(91557, "The Land of Darkness - Fairy Chest 8");
        public static readonly LocationData FairyDungeon9         = new(91558, "The Land of Darkness - Fairy Chest 9");
        public static readonly LocationData FairyDungeon10        = new(91559, "The Land of Darkness - Fairy Chest 10");
        public static readonly LocationData FairyDungeon11        = new(91560, "The Land of Darkness - Fairy Chest 11");
        public static readonly LocationData FairyDungeon12        = new(91561, "The Land of Darkness - Fairy Chest 12");
        public static readonly LocationData FairyDungeon13        = new(91562, "The Land of Darkness - Fairy Chest 13");
        public static readonly LocationData FairyDungeon14        = new(91563, "The Land of Darkness - Fairy Chest 14");
        public static readonly LocationData FairyDungeon15        = new(91564, "The Land of Darkness - Fairy Chest 15");

        public static IEnumerable<LocationData> GetAllLocations(SlotData data)
        {
            var list = new List<LocationData>();
            foreach (var field in typeof(LocationDefinitions).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.FieldType != typeof(LocationData))
                {
                    continue;
                }

                var location = (LocationData) field.GetValue(null);

                // Ignore chests we don't have enabled in our seed.
                if (location.Code is >= 91600 and <= 91629 && location.Code >= 91600 + data.ChestsPerZone)
                    continue;
                if (location.Code is >= 91700 and <= 91729 && location.Code >= 91700 + data.ChestsPerZone)
                    continue;
                if (location.Code is >= 91800 and <= 91829 && location.Code >= 91800 + data.ChestsPerZone)
                    continue;
                if (location.Code is >= 91900 and <= 91929 && location.Code >= 91900 + data.ChestsPerZone)
                    continue;

                // Ignore fairy chests we don't have enabled in our seed.
                if (location.Code is >= 91400 and <= 91414 && location.Code >= 91400 + data.FairyChestsPerZone)
                    continue;
                if (location.Code is >= 91450 and <= 91464 && location.Code >= 91450 + data.FairyChestsPerZone)
                    continue;
                if (location.Code is >= 91500 and <= 91514 && location.Code >= 91500 + data.FairyChestsPerZone)
                    continue;
                if (location.Code is >= 91550 and <= 91564 && location.Code >= 91550 + data.FairyChestsPerZone)
                    continue;

                list.Add(location);
            }

            return list;
        }

        public static LocationData GetLocation(SlotData data, int code)
        {
            return GetAllLocations(data).First(location => location.Code == code);
        }

        public static LocationData GetLocation(SlotData data, string name)
        {
            return GetAllLocations(data).First(location => location.Name == name);
        }
    }
}