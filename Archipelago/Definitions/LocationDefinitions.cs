// Rogue Legacy Randomizer - LocationDefinitions.cs
// Last Modified 2022-11-30
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago.Definitions
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
        public static readonly LocationData BossCastle            = new(91100, "Castle Hamson - Boss");
        public static readonly LocationData BossForest            = new(91102, "Forest Abkhazia - Boss");
        public static readonly LocationData BossTower             = new(91104, "The Maya - Boss");
        public static readonly LocationData BossDungeon           = new(91106, "The Land of Darkness - Boss");

        // Special Locations
        public static readonly LocationData SpecialJukebox        = new(91200, "Jukebox");
        public static readonly LocationData SpecialPainting       = new(91201, "Painting");
        public static readonly LocationData SpecialCheapskate     = new(91202, "Cheapskate Elf's Game");
        public static readonly LocationData SpecialCarnival       = new(91203, "Carnival");

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
        public static readonly LocationData ChestCastle1  = new(91600, "Castle Hamson - Chest 1");
        public static readonly LocationData ChestCastle2  = new(91601, "Castle Hamson - Chest 2");
        public static readonly LocationData ChestCastle3  = new(91602, "Castle Hamson - Chest 3");
        public static readonly LocationData ChestCastle4  = new(91603, "Castle Hamson - Chest 4");
        public static readonly LocationData ChestCastle5  = new(91604, "Castle Hamson - Chest 5");
        public static readonly LocationData ChestCastle6  = new(91605, "Castle Hamson - Chest 6");
        public static readonly LocationData ChestCastle7  = new(91606, "Castle Hamson - Chest 7");
        public static readonly LocationData ChestCastle8  = new(91607, "Castle Hamson - Chest 8");
        public static readonly LocationData ChestCastle9  = new(91608, "Castle Hamson - Chest 9");
        public static readonly LocationData ChestCastle10 = new(91609, "Castle Hamson - Chest 10");
        public static readonly LocationData ChestCastle11 = new(91610, "Castle Hamson - Chest 11");
        public static readonly LocationData ChestCastle12 = new(91611, "Castle Hamson - Chest 12");
        public static readonly LocationData ChestCastle13 = new(91612, "Castle Hamson - Chest 13");
        public static readonly LocationData ChestCastle14 = new(91613, "Castle Hamson - Chest 14");
        public static readonly LocationData ChestCastle15 = new(91614, "Castle Hamson - Chest 15");
        public static readonly LocationData ChestCastle16 = new(91615, "Castle Hamson - Chest 16");
        public static readonly LocationData ChestCastle17 = new(91616, "Castle Hamson - Chest 17");
        public static readonly LocationData ChestCastle18 = new(91617, "Castle Hamson - Chest 18");
        public static readonly LocationData ChestCastle19 = new(91618, "Castle Hamson - Chest 19");
        public static readonly LocationData ChestCastle20 = new(91619, "Castle Hamson - Chest 20");
        public static readonly LocationData ChestCastle21 = new(91620, "Castle Hamson - Chest 21");
        public static readonly LocationData ChestCastle22 = new(91621, "Castle Hamson - Chest 22");
        public static readonly LocationData ChestCastle23 = new(91622, "Castle Hamson - Chest 23");
        public static readonly LocationData ChestCastle24 = new(91623, "Castle Hamson - Chest 24");
        public static readonly LocationData ChestCastle25 = new(91624, "Castle Hamson - Chest 25");
        public static readonly LocationData ChestCastle26 = new(91625, "Castle Hamson - Chest 26");
        public static readonly LocationData ChestCastle27 = new(91626, "Castle Hamson - Chest 27");
        public static readonly LocationData ChestCastle28 = new(91627, "Castle Hamson - Chest 28");
        public static readonly LocationData ChestCastle29 = new(91628, "Castle Hamson - Chest 29");
        public static readonly LocationData ChestCastle30 = new(91629, "Castle Hamson - Chest 30");
        public static readonly LocationData ChestCastle31 = new(91630, "Castle Hamson - Chest 31");
        public static readonly LocationData ChestCastle32 = new(91631, "Castle Hamson - Chest 32");
        public static readonly LocationData ChestCastle33 = new(91632, "Castle Hamson - Chest 33");
        public static readonly LocationData ChestCastle34 = new(91633, "Castle Hamson - Chest 34");
        public static readonly LocationData ChestCastle35 = new(91634, "Castle Hamson - Chest 35");
        public static readonly LocationData ChestCastle36 = new(91635, "Castle Hamson - Chest 36");
        public static readonly LocationData ChestCastle37 = new(91636, "Castle Hamson - Chest 37");
        public static readonly LocationData ChestCastle38 = new(91637, "Castle Hamson - Chest 38");
        public static readonly LocationData ChestCastle39 = new(91638, "Castle Hamson - Chest 39");
        public static readonly LocationData ChestCastle40 = new(91639, "Castle Hamson - Chest 40");
        public static readonly LocationData ChestCastle41 = new(91640, "Castle Hamson - Chest 41");
        public static readonly LocationData ChestCastle42 = new(91641, "Castle Hamson - Chest 42");
        public static readonly LocationData ChestCastle43 = new(91642, "Castle Hamson - Chest 43");
        public static readonly LocationData ChestCastle44 = new(91643, "Castle Hamson - Chest 44");
        public static readonly LocationData ChestCastle45 = new(91644, "Castle Hamson - Chest 45");
        public static readonly LocationData ChestCastle46 = new(91645, "Castle Hamson - Chest 46");
        public static readonly LocationData ChestCastle47 = new(91646, "Castle Hamson - Chest 47");
        public static readonly LocationData ChestCastle48 = new(91647, "Castle Hamson - Chest 48");
        public static readonly LocationData ChestCastle49 = new(91648, "Castle Hamson - Chest 49");
        public static readonly LocationData ChestCastle50 = new(91649, "Castle Hamson - Chest 50");

        public static readonly LocationData ChestGarden1  = new(91700, "Forest Abkhazia - Chest 1");
        public static readonly LocationData ChestGarden2  = new(91701, "Forest Abkhazia - Chest 2");
        public static readonly LocationData ChestGarden3  = new(91702, "Forest Abkhazia - Chest 3");
        public static readonly LocationData ChestGarden4  = new(91703, "Forest Abkhazia - Chest 4");
        public static readonly LocationData ChestGarden5  = new(91704, "Forest Abkhazia - Chest 5");
        public static readonly LocationData ChestGarden6  = new(91705, "Forest Abkhazia - Chest 6");
        public static readonly LocationData ChestGarden7  = new(91706, "Forest Abkhazia - Chest 7");
        public static readonly LocationData ChestGarden8  = new(91707, "Forest Abkhazia - Chest 8");
        public static readonly LocationData ChestGarden9  = new(91708, "Forest Abkhazia - Chest 9");
        public static readonly LocationData ChestGarden10 = new(91709, "Forest Abkhazia - Chest 10");
        public static readonly LocationData ChestGarden11 = new(91710, "Forest Abkhazia - Chest 11");
        public static readonly LocationData ChestGarden12 = new(91711, "Forest Abkhazia - Chest 12");
        public static readonly LocationData ChestGarden13 = new(91712, "Forest Abkhazia - Chest 13");
        public static readonly LocationData ChestGarden14 = new(91713, "Forest Abkhazia - Chest 14");
        public static readonly LocationData ChestGarden15 = new(91714, "Forest Abkhazia - Chest 15");
        public static readonly LocationData ChestGarden16 = new(91715, "Forest Abkhazia - Chest 16");
        public static readonly LocationData ChestGarden17 = new(91716, "Forest Abkhazia - Chest 17");
        public static readonly LocationData ChestGarden18 = new(91717, "Forest Abkhazia - Chest 18");
        public static readonly LocationData ChestGarden19 = new(91718, "Forest Abkhazia - Chest 19");
        public static readonly LocationData ChestGarden20 = new(91719, "Forest Abkhazia - Chest 20");
        public static readonly LocationData ChestGarden21 = new(91720, "Forest Abkhazia - Chest 21");
        public static readonly LocationData ChestGarden22 = new(91721, "Forest Abkhazia - Chest 22");
        public static readonly LocationData ChestGarden23 = new(91722, "Forest Abkhazia - Chest 23");
        public static readonly LocationData ChestGarden24 = new(91723, "Forest Abkhazia - Chest 24");
        public static readonly LocationData ChestGarden25 = new(91724, "Forest Abkhazia - Chest 25");
        public static readonly LocationData ChestGarden26 = new(91725, "Forest Abkhazia - Chest 26");
        public static readonly LocationData ChestGarden27 = new(91726, "Forest Abkhazia - Chest 27");
        public static readonly LocationData ChestGarden28 = new(91727, "Forest Abkhazia - Chest 28");
        public static readonly LocationData ChestGarden29 = new(91728, "Forest Abkhazia - Chest 29");
        public static readonly LocationData ChestGarden30 = new(91729, "Forest Abkhazia - Chest 30");
        public static readonly LocationData ChestGarden31 = new(91730, "Forest Abkhazia - Chest 31");
        public static readonly LocationData ChestGarden32 = new(91731, "Forest Abkhazia - Chest 32");
        public static readonly LocationData ChestGarden33 = new(91732, "Forest Abkhazia - Chest 33");
        public static readonly LocationData ChestGarden34 = new(91733, "Forest Abkhazia - Chest 34");
        public static readonly LocationData ChestGarden35 = new(91734, "Forest Abkhazia - Chest 35");
        public static readonly LocationData ChestGarden36 = new(91735, "Forest Abkhazia - Chest 36");
        public static readonly LocationData ChestGarden37 = new(91736, "Forest Abkhazia - Chest 37");
        public static readonly LocationData ChestGarden38 = new(91737, "Forest Abkhazia - Chest 38");
        public static readonly LocationData ChestGarden39 = new(91738, "Forest Abkhazia - Chest 39");
        public static readonly LocationData ChestGarden40 = new(91739, "Forest Abkhazia - Chest 40");
        public static readonly LocationData ChestGarden41 = new(91740, "Forest Abkhazia - Chest 41");
        public static readonly LocationData ChestGarden42 = new(91741, "Forest Abkhazia - Chest 42");
        public static readonly LocationData ChestGarden43 = new(91742, "Forest Abkhazia - Chest 43");
        public static readonly LocationData ChestGarden44 = new(91743, "Forest Abkhazia - Chest 44");
        public static readonly LocationData ChestGarden45 = new(91744, "Forest Abkhazia - Chest 45");
        public static readonly LocationData ChestGarden46 = new(91745, "Forest Abkhazia - Chest 46");
        public static readonly LocationData ChestGarden47 = new(91746, "Forest Abkhazia - Chest 47");
        public static readonly LocationData ChestGarden48 = new(91747, "Forest Abkhazia - Chest 48");
        public static readonly LocationData ChestGarden49 = new(91748, "Forest Abkhazia - Chest 49");
        public static readonly LocationData ChestGarden50 = new(91749, "Forest Abkhazia - Chest 50");

        public static readonly LocationData ChestTower1   = new(91800, "The Maya - Chest 1");
        public static readonly LocationData ChestTower2   = new(91801, "The Maya - Chest 2");
        public static readonly LocationData ChestTower3   = new(91802, "The Maya - Chest 3");
        public static readonly LocationData ChestTower4   = new(91803, "The Maya - Chest 4");
        public static readonly LocationData ChestTower5   = new(91804, "The Maya - Chest 5");
        public static readonly LocationData ChestTower6   = new(91805, "The Maya - Chest 6");
        public static readonly LocationData ChestTower7   = new(91806, "The Maya - Chest 7");
        public static readonly LocationData ChestTower8   = new(91807, "The Maya - Chest 8");
        public static readonly LocationData ChestTower9   = new(91808, "The Maya - Chest 9");
        public static readonly LocationData ChestTower10  = new(91809, "The Maya - Chest 10");
        public static readonly LocationData ChestTower11  = new(91810, "The Maya - Chest 11");
        public static readonly LocationData ChestTower12  = new(91811, "The Maya - Chest 12");
        public static readonly LocationData ChestTower13  = new(91812, "The Maya - Chest 13");
        public static readonly LocationData ChestTower14  = new(91813, "The Maya - Chest 14");
        public static readonly LocationData ChestTower15  = new(91814, "The Maya - Chest 15");
        public static readonly LocationData ChestTower16  = new(91815, "The Maya - Chest 16");
        public static readonly LocationData ChestTower17  = new(91816, "The Maya - Chest 17");
        public static readonly LocationData ChestTower18  = new(91817, "The Maya - Chest 18");
        public static readonly LocationData ChestTower19  = new(91818, "The Maya - Chest 19");
        public static readonly LocationData ChestTower20  = new(91819, "The Maya - Chest 20");
        public static readonly LocationData ChestTower21  = new(91820, "The Maya - Chest 21");
        public static readonly LocationData ChestTower22  = new(91821, "The Maya - Chest 22");
        public static readonly LocationData ChestTower23  = new(91822, "The Maya - Chest 23");
        public static readonly LocationData ChestTower24  = new(91823, "The Maya - Chest 24");
        public static readonly LocationData ChestTower25  = new(91824, "The Maya - Chest 25");
        public static readonly LocationData ChestTower26  = new(91825, "The Maya - Chest 26");
        public static readonly LocationData ChestTower27  = new(91826, "The Maya - Chest 27");
        public static readonly LocationData ChestTower28  = new(91827, "The Maya - Chest 28");
        public static readonly LocationData ChestTower29  = new(91828, "The Maya - Chest 29");
        public static readonly LocationData ChestTower30  = new(91829, "The Maya - Chest 30");
        public static readonly LocationData ChestTower31  = new(91830, "The Maya - Chest 31");
        public static readonly LocationData ChestTower32  = new(91831, "The Maya - Chest 32");
        public static readonly LocationData ChestTower33  = new(91832, "The Maya - Chest 33");
        public static readonly LocationData ChestTower34  = new(91833, "The Maya - Chest 34");
        public static readonly LocationData ChestTower35  = new(91834, "The Maya - Chest 35");
        public static readonly LocationData ChestTower36  = new(91835, "The Maya - Chest 36");
        public static readonly LocationData ChestTower37  = new(91836, "The Maya - Chest 37");
        public static readonly LocationData ChestTower38  = new(91837, "The Maya - Chest 38");
        public static readonly LocationData ChestTower39  = new(91838, "The Maya - Chest 39");
        public static readonly LocationData ChestTower40  = new(91839, "The Maya - Chest 40");
        public static readonly LocationData ChestTower41  = new(91840, "The Maya - Chest 41");
        public static readonly LocationData ChestTower42  = new(91841, "The Maya - Chest 42");
        public static readonly LocationData ChestTower43  = new(91842, "The Maya - Chest 43");
        public static readonly LocationData ChestTower44  = new(91843, "The Maya - Chest 44");
        public static readonly LocationData ChestTower45  = new(91844, "The Maya - Chest 45");
        public static readonly LocationData ChestTower46  = new(91845, "The Maya - Chest 46");
        public static readonly LocationData ChestTower47  = new(91846, "The Maya - Chest 47");
        public static readonly LocationData ChestTower48  = new(91847, "The Maya - Chest 48");
        public static readonly LocationData ChestTower49  = new(91848, "The Maya - Chest 49");
        public static readonly LocationData ChestTower50  = new(91849, "The Maya - Chest 50");

        public static readonly LocationData ChestDungeon1  = new(91900, "The Land of Darkness - Chest 1");
        public static readonly LocationData ChestDungeon2  = new(91901, "The Land of Darkness - Chest 2");
        public static readonly LocationData ChestDungeon3  = new(91902, "The Land of Darkness - Chest 3");
        public static readonly LocationData ChestDungeon4  = new(91903, "The Land of Darkness - Chest 4");
        public static readonly LocationData ChestDungeon5  = new(91904, "The Land of Darkness - Chest 5");
        public static readonly LocationData ChestDungeon6  = new(91905, "The Land of Darkness - Chest 6");
        public static readonly LocationData ChestDungeon7  = new(91906, "The Land of Darkness - Chest 7");
        public static readonly LocationData ChestDungeon8  = new(91907, "The Land of Darkness - Chest 8");
        public static readonly LocationData ChestDungeon9  = new(91908, "The Land of Darkness - Chest 9");
        public static readonly LocationData ChestDungeon10 = new(91909, "The Land of Darkness - Chest 10");
        public static readonly LocationData ChestDungeon11 = new(91910, "The Land of Darkness - Chest 11");
        public static readonly LocationData ChestDungeon12 = new(91911, "The Land of Darkness - Chest 12");
        public static readonly LocationData ChestDungeon13 = new(91912, "The Land of Darkness - Chest 13");
        public static readonly LocationData ChestDungeon14 = new(91913, "The Land of Darkness - Chest 14");
        public static readonly LocationData ChestDungeon15 = new(91914, "The Land of Darkness - Chest 15");
        public static readonly LocationData ChestDungeon16 = new(91915, "The Land of Darkness - Chest 16");
        public static readonly LocationData ChestDungeon17 = new(91916, "The Land of Darkness - Chest 17");
        public static readonly LocationData ChestDungeon18 = new(91917, "The Land of Darkness - Chest 18");
        public static readonly LocationData ChestDungeon19 = new(91918, "The Land of Darkness - Chest 19");
        public static readonly LocationData ChestDungeon20 = new(91919, "The Land of Darkness - Chest 20");
        public static readonly LocationData ChestDungeon21 = new(91920, "The Land of Darkness - Chest 21");
        public static readonly LocationData ChestDungeon22 = new(91921, "The Land of Darkness - Chest 22");
        public static readonly LocationData ChestDungeon23 = new(91922, "The Land of Darkness - Chest 23");
        public static readonly LocationData ChestDungeon24 = new(91923, "The Land of Darkness - Chest 24");
        public static readonly LocationData ChestDungeon25 = new(91924, "The Land of Darkness - Chest 25");
        public static readonly LocationData ChestDungeon26 = new(91925, "The Land of Darkness - Chest 26");
        public static readonly LocationData ChestDungeon27 = new(91926, "The Land of Darkness - Chest 27");
        public static readonly LocationData ChestDungeon28 = new(91927, "The Land of Darkness - Chest 28");
        public static readonly LocationData ChestDungeon29 = new(91928, "The Land of Darkness - Chest 29");
        public static readonly LocationData ChestDungeon30 = new(91929, "The Land of Darkness - Chest 30");
        public static readonly LocationData ChestDungeon31 = new(91930, "The Land of Darkness - Chest 31");
        public static readonly LocationData ChestDungeon32 = new(91931, "The Land of Darkness - Chest 32");
        public static readonly LocationData ChestDungeon33 = new(91932, "The Land of Darkness - Chest 33");
        public static readonly LocationData ChestDungeon34 = new(91933, "The Land of Darkness - Chest 34");
        public static readonly LocationData ChestDungeon35 = new(91934, "The Land of Darkness - Chest 35");
        public static readonly LocationData ChestDungeon36 = new(91935, "The Land of Darkness - Chest 36");
        public static readonly LocationData ChestDungeon37 = new(91936, "The Land of Darkness - Chest 37");
        public static readonly LocationData ChestDungeon38 = new(91937, "The Land of Darkness - Chest 38");
        public static readonly LocationData ChestDungeon39 = new(91938, "The Land of Darkness - Chest 39");
        public static readonly LocationData ChestDungeon40 = new(91939, "The Land of Darkness - Chest 40");
        public static readonly LocationData ChestDungeon41 = new(91940, "The Land of Darkness - Chest 41");
        public static readonly LocationData ChestDungeon42 = new(91941, "The Land of Darkness - Chest 42");
        public static readonly LocationData ChestDungeon43 = new(91942, "The Land of Darkness - Chest 43");
        public static readonly LocationData ChestDungeon44 = new(91943, "The Land of Darkness - Chest 44");
        public static readonly LocationData ChestDungeon45 = new(91944, "The Land of Darkness - Chest 45");
        public static readonly LocationData ChestDungeon46 = new(91945, "The Land of Darkness - Chest 46");
        public static readonly LocationData ChestDungeon47 = new(91946, "The Land of Darkness - Chest 47");
        public static readonly LocationData ChestDungeon48 = new(91947, "The Land of Darkness - Chest 48");
        public static readonly LocationData ChestDungeon49 = new(91948, "The Land of Darkness - Chest 49");
        public static readonly LocationData ChestDungeon50 = new(91949, "The Land of Darkness - Chest 50");

        public static readonly LocationData ChestUniversal1   = new(92000, "Chest 1");
        public static readonly LocationData ChestUniversal2   = new(92001, "Chest 2");
        public static readonly LocationData ChestUniversal3   = new(92002, "Chest 3");
        public static readonly LocationData ChestUniversal4   = new(92003, "Chest 4");
        public static readonly LocationData ChestUniversal5   = new(92004, "Chest 5");
        public static readonly LocationData ChestUniversal6   = new(92005, "Chest 6");
        public static readonly LocationData ChestUniversal7   = new(92006, "Chest 7");
        public static readonly LocationData ChestUniversal8   = new(92007, "Chest 8");
        public static readonly LocationData ChestUniversal9   = new(92008, "Chest 9");
        public static readonly LocationData ChestUniversal10  = new(92009, "Chest 10");
        public static readonly LocationData ChestUniversal11  = new(92010, "Chest 11");
        public static readonly LocationData ChestUniversal12  = new(92011, "Chest 12");
        public static readonly LocationData ChestUniversal13  = new(92012, "Chest 13");
        public static readonly LocationData ChestUniversal14  = new(92013, "Chest 14");
        public static readonly LocationData ChestUniversal15  = new(92014, "Chest 15");
        public static readonly LocationData ChestUniversal16  = new(92015, "Chest 16");
        public static readonly LocationData ChestUniversal17  = new(92016, "Chest 17");
        public static readonly LocationData ChestUniversal18  = new(92017, "Chest 18");
        public static readonly LocationData ChestUniversal19  = new(92018, "Chest 19");
        public static readonly LocationData ChestUniversal20  = new(92019, "Chest 20");
        public static readonly LocationData ChestUniversal21  = new(92020, "Chest 21");
        public static readonly LocationData ChestUniversal22  = new(92021, "Chest 22");
        public static readonly LocationData ChestUniversal23  = new(92022, "Chest 23");
        public static readonly LocationData ChestUniversal24  = new(92023, "Chest 24");
        public static readonly LocationData ChestUniversal25  = new(92024, "Chest 25");
        public static readonly LocationData ChestUniversal26  = new(92025, "Chest 26");
        public static readonly LocationData ChestUniversal27  = new(92026, "Chest 27");
        public static readonly LocationData ChestUniversal28  = new(92027, "Chest 28");
        public static readonly LocationData ChestUniversal29  = new(92028, "Chest 29");
        public static readonly LocationData ChestUniversal30  = new(92029, "Chest 30");
        public static readonly LocationData ChestUniversal31  = new(92030, "Chest 31");
        public static readonly LocationData ChestUniversal32  = new(92031, "Chest 32");
        public static readonly LocationData ChestUniversal33  = new(92032, "Chest 33");
        public static readonly LocationData ChestUniversal34  = new(92033, "Chest 34");
        public static readonly LocationData ChestUniversal35  = new(92034, "Chest 35");
        public static readonly LocationData ChestUniversal36  = new(92035, "Chest 36");
        public static readonly LocationData ChestUniversal37  = new(92036, "Chest 37");
        public static readonly LocationData ChestUniversal38  = new(92037, "Chest 38");
        public static readonly LocationData ChestUniversal39  = new(92038, "Chest 39");
        public static readonly LocationData ChestUniversal40  = new(92039, "Chest 40");
        public static readonly LocationData ChestUniversal41  = new(92040, "Chest 41");
        public static readonly LocationData ChestUniversal42  = new(92041, "Chest 42");
        public static readonly LocationData ChestUniversal43  = new(92042, "Chest 43");
        public static readonly LocationData ChestUniversal44  = new(92043, "Chest 44");
        public static readonly LocationData ChestUniversal45  = new(92044, "Chest 45");
        public static readonly LocationData ChestUniversal46  = new(92045, "Chest 46");
        public static readonly LocationData ChestUniversal47  = new(92046, "Chest 47");
        public static readonly LocationData ChestUniversal48  = new(92047, "Chest 48");
        public static readonly LocationData ChestUniversal49  = new(92048, "Chest 49");
        public static readonly LocationData ChestUniversal50  = new(92049, "Chest 50");
        public static readonly LocationData ChestUniversal51  = new(92050, "Chest 51");
        public static readonly LocationData ChestUniversal52  = new(92051, "Chest 52");
        public static readonly LocationData ChestUniversal53  = new(92052, "Chest 53");
        public static readonly LocationData ChestUniversal54  = new(92053, "Chest 54");
        public static readonly LocationData ChestUniversal55  = new(92054, "Chest 55");
        public static readonly LocationData ChestUniversal56  = new(92055, "Chest 56");
        public static readonly LocationData ChestUniversal57  = new(92056, "Chest 57");
        public static readonly LocationData ChestUniversal58  = new(92057, "Chest 58");
        public static readonly LocationData ChestUniversal59  = new(92058, "Chest 59");
        public static readonly LocationData ChestUniversal60  = new(92059, "Chest 60");
        public static readonly LocationData ChestUniversal61  = new(92060, "Chest 61");
        public static readonly LocationData ChestUniversal62  = new(92061, "Chest 62");
        public static readonly LocationData ChestUniversal63  = new(92062, "Chest 63");
        public static readonly LocationData ChestUniversal64  = new(92063, "Chest 64");
        public static readonly LocationData ChestUniversal65  = new(92064, "Chest 65");
        public static readonly LocationData ChestUniversal66  = new(92065, "Chest 66");
        public static readonly LocationData ChestUniversal67  = new(92066, "Chest 67");
        public static readonly LocationData ChestUniversal68  = new(92067, "Chest 68");
        public static readonly LocationData ChestUniversal69  = new(92068, "Chest 69");
        public static readonly LocationData ChestUniversal70  = new(92069, "Chest 70");
        public static readonly LocationData ChestUniversal71  = new(92070, "Chest 71");
        public static readonly LocationData ChestUniversal72  = new(92071, "Chest 72");
        public static readonly LocationData ChestUniversal73  = new(92072, "Chest 73");
        public static readonly LocationData ChestUniversal74  = new(92073, "Chest 74");
        public static readonly LocationData ChestUniversal75  = new(92074, "Chest 75");
        public static readonly LocationData ChestUniversal76  = new(92075, "Chest 76");
        public static readonly LocationData ChestUniversal77  = new(92076, "Chest 77");
        public static readonly LocationData ChestUniversal78  = new(92077, "Chest 78");
        public static readonly LocationData ChestUniversal79  = new(92078, "Chest 79");
        public static readonly LocationData ChestUniversal80  = new(92079, "Chest 80");
        public static readonly LocationData ChestUniversal81  = new(92080, "Chest 81");
        public static readonly LocationData ChestUniversal82  = new(92081, "Chest 82");
        public static readonly LocationData ChestUniversal83  = new(92082, "Chest 83");
        public static readonly LocationData ChestUniversal84  = new(92083, "Chest 84");
        public static readonly LocationData ChestUniversal85  = new(92084, "Chest 85");
        public static readonly LocationData ChestUniversal86  = new(92085, "Chest 86");
        public static readonly LocationData ChestUniversal87  = new(92086, "Chest 87");
        public static readonly LocationData ChestUniversal88  = new(92087, "Chest 88");
        public static readonly LocationData ChestUniversal89  = new(92088, "Chest 89");
        public static readonly LocationData ChestUniversal90  = new(92089, "Chest 90");
        public static readonly LocationData ChestUniversal91  = new(92090, "Chest 91");
        public static readonly LocationData ChestUniversal92  = new(92091, "Chest 92");
        public static readonly LocationData ChestUniversal93  = new(92092, "Chest 93");
        public static readonly LocationData ChestUniversal94  = new(92093, "Chest 94");
        public static readonly LocationData ChestUniversal95  = new(92094, "Chest 95");
        public static readonly LocationData ChestUniversal96  = new(92095, "Chest 96");
        public static readonly LocationData ChestUniversal97  = new(92096, "Chest 97");
        public static readonly LocationData ChestUniversal98  = new(92097, "Chest 98");
        public static readonly LocationData ChestUniversal99  = new(92098, "Chest 99");
        public static readonly LocationData ChestUniversal100 = new(92099, "Chest 100");
        public static readonly LocationData ChestUniversal101 = new(92100, "Chest 101");
        public static readonly LocationData ChestUniversal102 = new(92101, "Chest 102");
        public static readonly LocationData ChestUniversal103 = new(92102, "Chest 103");
        public static readonly LocationData ChestUniversal104 = new(92103, "Chest 104");
        public static readonly LocationData ChestUniversal105 = new(92104, "Chest 105");
        public static readonly LocationData ChestUniversal106 = new(92105, "Chest 106");
        public static readonly LocationData ChestUniversal107 = new(92106, "Chest 107");
        public static readonly LocationData ChestUniversal108 = new(92107, "Chest 108");
        public static readonly LocationData ChestUniversal109 = new(92108, "Chest 109");
        public static readonly LocationData ChestUniversal110 = new(92109, "Chest 110");
        public static readonly LocationData ChestUniversal111 = new(92110, "Chest 111");
        public static readonly LocationData ChestUniversal112 = new(92111, "Chest 112");
        public static readonly LocationData ChestUniversal113 = new(92112, "Chest 113");
        public static readonly LocationData ChestUniversal114 = new(92113, "Chest 114");
        public static readonly LocationData ChestUniversal115 = new(92114, "Chest 115");
        public static readonly LocationData ChestUniversal116 = new(92115, "Chest 116");
        public static readonly LocationData ChestUniversal117 = new(92116, "Chest 117");
        public static readonly LocationData ChestUniversal118 = new(92117, "Chest 118");
        public static readonly LocationData ChestUniversal119 = new(92118, "Chest 119");
        public static readonly LocationData ChestUniversal120 = new(92119, "Chest 120");
        public static readonly LocationData ChestUniversal121 = new(92120, "Chest 121");
        public static readonly LocationData ChestUniversal122 = new(92121, "Chest 122");
        public static readonly LocationData ChestUniversal123 = new(92122, "Chest 123");
        public static readonly LocationData ChestUniversal124 = new(92123, "Chest 124");
        public static readonly LocationData ChestUniversal125 = new(92124, "Chest 125");
        public static readonly LocationData ChestUniversal126 = new(92125, "Chest 126");
        public static readonly LocationData ChestUniversal127 = new(92126, "Chest 127");
        public static readonly LocationData ChestUniversal128 = new(92127, "Chest 128");
        public static readonly LocationData ChestUniversal129 = new(92128, "Chest 129");
        public static readonly LocationData ChestUniversal130 = new(92129, "Chest 130");
        public static readonly LocationData ChestUniversal131 = new(92130, "Chest 131");
        public static readonly LocationData ChestUniversal132 = new(92131, "Chest 132");
        public static readonly LocationData ChestUniversal133 = new(92132, "Chest 133");
        public static readonly LocationData ChestUniversal134 = new(92133, "Chest 134");
        public static readonly LocationData ChestUniversal135 = new(92134, "Chest 135");
        public static readonly LocationData ChestUniversal136 = new(92135, "Chest 136");
        public static readonly LocationData ChestUniversal137 = new(92136, "Chest 137");
        public static readonly LocationData ChestUniversal138 = new(92137, "Chest 138");
        public static readonly LocationData ChestUniversal139 = new(92138, "Chest 139");
        public static readonly LocationData ChestUniversal140 = new(92139, "Chest 140");
        public static readonly LocationData ChestUniversal141 = new(92140, "Chest 141");
        public static readonly LocationData ChestUniversal142 = new(92141, "Chest 142");
        public static readonly LocationData ChestUniversal143 = new(92142, "Chest 143");
        public static readonly LocationData ChestUniversal144 = new(92143, "Chest 144");
        public static readonly LocationData ChestUniversal145 = new(92144, "Chest 145");
        public static readonly LocationData ChestUniversal146 = new(92145, "Chest 146");
        public static readonly LocationData ChestUniversal147 = new(92146, "Chest 147");
        public static readonly LocationData ChestUniversal148 = new(92147, "Chest 148");
        public static readonly LocationData ChestUniversal149 = new(92148, "Chest 149");
        public static readonly LocationData ChestUniversal150 = new(92149, "Chest 150");
        public static readonly LocationData ChestUniversal151 = new(92150, "Chest 151");
        public static readonly LocationData ChestUniversal152 = new(92151, "Chest 152");
        public static readonly LocationData ChestUniversal153 = new(92152, "Chest 153");
        public static readonly LocationData ChestUniversal154 = new(92153, "Chest 154");
        public static readonly LocationData ChestUniversal155 = new(92154, "Chest 155");
        public static readonly LocationData ChestUniversal156 = new(92155, "Chest 156");
        public static readonly LocationData ChestUniversal157 = new(92156, "Chest 157");
        public static readonly LocationData ChestUniversal158 = new(92157, "Chest 158");
        public static readonly LocationData ChestUniversal159 = new(92158, "Chest 159");
        public static readonly LocationData ChestUniversal160 = new(92159, "Chest 160");
        public static readonly LocationData ChestUniversal161 = new(92160, "Chest 161");
        public static readonly LocationData ChestUniversal162 = new(92161, "Chest 162");
        public static readonly LocationData ChestUniversal163 = new(92162, "Chest 163");
        public static readonly LocationData ChestUniversal164 = new(92163, "Chest 164");
        public static readonly LocationData ChestUniversal165 = new(92164, "Chest 165");
        public static readonly LocationData ChestUniversal166 = new(92165, "Chest 166");
        public static readonly LocationData ChestUniversal167 = new(92166, "Chest 167");
        public static readonly LocationData ChestUniversal168 = new(92167, "Chest 168");
        public static readonly LocationData ChestUniversal169 = new(92168, "Chest 169");
        public static readonly LocationData ChestUniversal170 = new(92169, "Chest 170");
        public static readonly LocationData ChestUniversal171 = new(92170, "Chest 171");
        public static readonly LocationData ChestUniversal172 = new(92171, "Chest 172");
        public static readonly LocationData ChestUniversal173 = new(92172, "Chest 173");
        public static readonly LocationData ChestUniversal174 = new(92173, "Chest 174");
        public static readonly LocationData ChestUniversal175 = new(92174, "Chest 175");
        public static readonly LocationData ChestUniversal176 = new(92175, "Chest 176");
        public static readonly LocationData ChestUniversal177 = new(92176, "Chest 177");
        public static readonly LocationData ChestUniversal178 = new(92177, "Chest 178");
        public static readonly LocationData ChestUniversal179 = new(92178, "Chest 179");
        public static readonly LocationData ChestUniversal180 = new(92179, "Chest 180");
        public static readonly LocationData ChestUniversal181 = new(92180, "Chest 181");
        public static readonly LocationData ChestUniversal182 = new(92181, "Chest 182");
        public static readonly LocationData ChestUniversal183 = new(92182, "Chest 183");
        public static readonly LocationData ChestUniversal184 = new(92183, "Chest 184");
        public static readonly LocationData ChestUniversal185 = new(92184, "Chest 185");
        public static readonly LocationData ChestUniversal186 = new(92185, "Chest 186");
        public static readonly LocationData ChestUniversal187 = new(92186, "Chest 187");
        public static readonly LocationData ChestUniversal188 = new(92187, "Chest 188");
        public static readonly LocationData ChestUniversal189 = new(92188, "Chest 189");
        public static readonly LocationData ChestUniversal190 = new(92189, "Chest 190");
        public static readonly LocationData ChestUniversal191 = new(92190, "Chest 191");
        public static readonly LocationData ChestUniversal192 = new(92191, "Chest 192");
        public static readonly LocationData ChestUniversal193 = new(92192, "Chest 193");
        public static readonly LocationData ChestUniversal194 = new(92193, "Chest 194");
        public static readonly LocationData ChestUniversal195 = new(92194, "Chest 195");
        public static readonly LocationData ChestUniversal196 = new(92195, "Chest 196");
        public static readonly LocationData ChestUniversal197 = new(92196, "Chest 197");
        public static readonly LocationData ChestUniversal198 = new(92197, "Chest 198");
        public static readonly LocationData ChestUniversal199 = new(92198, "Chest 199");
        public static readonly LocationData ChestUniversal200 = new(92199, "Chest 200");

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

        public static readonly LocationData FairyUniversal1       = new(92200, "Fairy Chest 1");
        public static readonly LocationData FairyUniversal2       = new(92201, "Fairy Chest 2");
        public static readonly LocationData FairyUniversal3       = new(92202, "Fairy Chest 3");
        public static readonly LocationData FairyUniversal4       = new(92203, "Fairy Chest 4");
        public static readonly LocationData FairyUniversal5       = new(92204, "Fairy Chest 5");
        public static readonly LocationData FairyUniversal6       = new(92205, "Fairy Chest 6");
        public static readonly LocationData FairyUniversal7       = new(92206, "Fairy Chest 7");
        public static readonly LocationData FairyUniversal8       = new(92207, "Fairy Chest 8");
        public static readonly LocationData FairyUniversal9       = new(92208, "Fairy Chest 9");
        public static readonly LocationData FairyUniversal10      = new(92209, "Fairy Chest 10");
        public static readonly LocationData FairyUniversal11      = new(92210, "Fairy Chest 11");
        public static readonly LocationData FairyUniversal12      = new(92211, "Fairy Chest 12");
        public static readonly LocationData FairyUniversal13      = new(92212, "Fairy Chest 13");
        public static readonly LocationData FairyUniversal14      = new(92213, "Fairy Chest 14");
        public static readonly LocationData FairyUniversal15      = new(92214, "Fairy Chest 15");
        public static readonly LocationData FairyUniversal16      = new(92215, "Fairy Chest 16");
        public static readonly LocationData FairyUniversal17      = new(92216, "Fairy Chest 17");
        public static readonly LocationData FairyUniversal18      = new(92217, "Fairy Chest 18");
        public static readonly LocationData FairyUniversal19      = new(92218, "Fairy Chest 19");
        public static readonly LocationData FairyUniversal20      = new(92219, "Fairy Chest 20");
        public static readonly LocationData FairyUniversal21      = new(92220, "Fairy Chest 21");
        public static readonly LocationData FairyUniversal22      = new(92221, "Fairy Chest 22");
        public static readonly LocationData FairyUniversal23      = new(92222, "Fairy Chest 23");
        public static readonly LocationData FairyUniversal24      = new(92223, "Fairy Chest 24");
        public static readonly LocationData FairyUniversal25      = new(92224, "Fairy Chest 25");
        public static readonly LocationData FairyUniversal26      = new(92225, "Fairy Chest 26");
        public static readonly LocationData FairyUniversal27      = new(92226, "Fairy Chest 27");
        public static readonly LocationData FairyUniversal28      = new(92227, "Fairy Chest 28");
        public static readonly LocationData FairyUniversal29      = new(92228, "Fairy Chest 29");
        public static readonly LocationData FairyUniversal30      = new(92229, "Fairy Chest 30");
        public static readonly LocationData FairyUniversal31      = new(92230, "Fairy Chest 31");
        public static readonly LocationData FairyUniversal32      = new(92231, "Fairy Chest 32");
        public static readonly LocationData FairyUniversal33      = new(92232, "Fairy Chest 33");
        public static readonly LocationData FairyUniversal34      = new(92233, "Fairy Chest 34");
        public static readonly LocationData FairyUniversal35      = new(92234, "Fairy Chest 35");
        public static readonly LocationData FairyUniversal36      = new(92235, "Fairy Chest 36");
        public static readonly LocationData FairyUniversal37      = new(92236, "Fairy Chest 37");
        public static readonly LocationData FairyUniversal38      = new(92237, "Fairy Chest 38");
        public static readonly LocationData FairyUniversal39      = new(92238, "Fairy Chest 39");
        public static readonly LocationData FairyUniversal40      = new(92239, "Fairy Chest 40");
        public static readonly LocationData FairyUniversal41      = new(92240, "Fairy Chest 41");
        public static readonly LocationData FairyUniversal42      = new(92241, "Fairy Chest 42");
        public static readonly LocationData FairyUniversal43      = new(92242, "Fairy Chest 43");
        public static readonly LocationData FairyUniversal44      = new(92243, "Fairy Chest 44");
        public static readonly LocationData FairyUniversal45      = new(92244, "Fairy Chest 45");
        public static readonly LocationData FairyUniversal46      = new(92245, "Fairy Chest 46");
        public static readonly LocationData FairyUniversal47      = new(92246, "Fairy Chest 47");
        public static readonly LocationData FairyUniversal48      = new(92247, "Fairy Chest 48");
        public static readonly LocationData FairyUniversal49      = new(92248, "Fairy Chest 49");
        public static readonly LocationData FairyUniversal50      = new(92249, "Fairy Chest 50");
        public static readonly LocationData FairyUniversal51      = new(92250, "Fairy Chest 51");
        public static readonly LocationData FairyUniversal52      = new(92251, "Fairy Chest 52");
        public static readonly LocationData FairyUniversal53      = new(92252, "Fairy Chest 53");
        public static readonly LocationData FairyUniversal54      = new(92253, "Fairy Chest 54");
        public static readonly LocationData FairyUniversal55      = new(92254, "Fairy Chest 55");
        public static readonly LocationData FairyUniversal56      = new(92255, "Fairy Chest 56");
        public static readonly LocationData FairyUniversal57      = new(92256, "Fairy Chest 57");
        public static readonly LocationData FairyUniversal58      = new(92257, "Fairy Chest 58");
        public static readonly LocationData FairyUniversal59      = new(92258, "Fairy Chest 59");
        public static readonly LocationData FairyUniversal60      = new(92259, "Fairy Chest 60");



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
                if (data.UniversalChests)
                {
                    // Ignore Zone Chests.
                    if (location.Code is >= 91600 and <= 91949)
                        continue;

                    if (location.Code is >= 92000 and <= 92199 && location.Code >= 92000 + data.ChestsPerZone * 4)
                        continue;
                }
                else
                {
                    // Ignore Universal Chests.
                    if (location.Code is >= 92000 and <= 92199)
                        continue;

                    if (location.Code is >= 91600 and <= 91649 && location.Code >= 91600 + data.ChestsPerZone)
                        continue;
                    if (location.Code is >= 91700 and <= 91749 && location.Code >= 91700 + data.ChestsPerZone)
                        continue;
                    if (location.Code is >= 91800 and <= 91849 && location.Code >= 91800 + data.ChestsPerZone)
                        continue;
                    if (location.Code is >= 91900 and <= 91949 && location.Code >= 91900 + data.ChestsPerZone)
                        continue;
                }

                // Ignore fairy chests we don't have enabled in our seed.
                if (data.UniversalFairyChests)
                {
                    // Ignore Zone Fairy Chests.
                    if (location.Code is >= 91400 and <= 91564)
                        continue;

                    if (location.Code is >= 92200 and <= 92259 && location.Code >= 92200 + data.FairyChestsPerZone * 4)
                        continue;
                }
                else
                {
                    // Ignore Universal Fairy Chests.
                    if (location.Code is >= 92200 and <= 92259)
                        continue;

                    if (location.Code is >= 91400 and <= 91414 && location.Code >= 91400 + data.FairyChestsPerZone)
                        continue;
                    if (location.Code is >= 91450 and <= 91464 && location.Code >= 91450 + data.FairyChestsPerZone)
                        continue;
                    if (location.Code is >= 91500 and <= 91514 && location.Code >= 91500 + data.FairyChestsPerZone)
                        continue;
                    if (location.Code is >= 91550 and <= 91564 && location.Code >= 91550 + data.FairyChestsPerZone)
                        continue;
                }

                list.Add(location);
            }

            return list;
        }

        public static LocationData GetLocation(SlotData data, long code)
        {
            return GetAllLocations(data).First(location => location.Code == code);
        }

        public static LocationData GetLocation(SlotData data, string name)
        {
            return GetAllLocations(data).First(location => location.Name == name);
        }
    }
}
