using System.Collections.Generic;
using Randomizer.Definitions;

namespace RogueLegacy.Systems;

public static class ManorContainer
{
    static ManorContainer()
    {
        // Define a lookup table between our internal identifiers and the ones used by the AP service.
        ArchipelagoLocationTable = new Dictionary<ManorPiece, int>
        {
            { ManorPiece.GroundRoad, (int) LocationCode.MANOR_GROUND_ROAD },
            { ManorPiece.MainBase, (int) LocationCode.MANOR_MAIN_BASE },
            { ManorPiece.MainWindowBottom, (int) LocationCode.MANOR_MAIN_BOTTOM_WINDOW },
            { ManorPiece.MainWindowTop, (int) LocationCode.MANOR_MAIN_TOP_WINDOW },
            { ManorPiece.MainRoof, (int) LocationCode.MANOR_MAIN_ROOFTOP },
            { ManorPiece.LeftWingBase, (int) LocationCode.MANOR_LEFT_WING_BASE },
            { ManorPiece.LeftWingWindow, (int) LocationCode.MANOR_LEFT_WING_WINDOW },
            { ManorPiece.LeftWingRoof, (int) LocationCode.MANOR_LEFT_WING_ROOFTOP },
            { ManorPiece.LeftBigBase, (int) LocationCode.MANOR_LEFT_BIG_BASE },
            { ManorPiece.LeftBigUpper1, (int) LocationCode.MANOR_LEFT_BIG_UPPER1 },
            { ManorPiece.LeftBigUpper2, (int) LocationCode.MANOR_LEFT_BIG_UPPER2 },
            { ManorPiece.LeftBigWindows, (int) LocationCode.MANOR_LEFT_BIG_WINDOWS },
            { ManorPiece.LeftBigRoof, (int) LocationCode.MANOR_LEFT_BIG_ROOFTOP },
            { ManorPiece.LeftFarBase, (int) LocationCode.MANOR_LEFT_FAR_BASE },
            { ManorPiece.LeftFarRoof, (int) LocationCode.MANOR_LEFT_FAR_ROOF },
            { ManorPiece.LeftExtension, (int) LocationCode.MANOR_LEFT_EXTENSION },
            { ManorPiece.LeftTree1, (int) LocationCode.MANOR_LEFT_TREE1 },
            { ManorPiece.LeftTree2, (int) LocationCode.MANOR_LEFT_TREE2 },
            { ManorPiece.RightWingBase, (int) LocationCode.MANOR_RIGHT_WING_BASE },
            { ManorPiece.RightWingWindow, (int) LocationCode.MANOR_RIGHT_WING_WINDOW },
            { ManorPiece.RightWingRoof, (int) LocationCode.MANOR_RIGHT_WING_ROOFTOP },
            { ManorPiece.RightBigBase, (int) LocationCode.MANOR_RIGHT_BIG_BASE },
            { ManorPiece.RightBigUpper, (int) LocationCode.MANOR_RIGHT_BIG_UPPER },
            { ManorPiece.RightBigRoof, (int) LocationCode.MANOR_RIGHT_BIG_ROOFTOP },
            { ManorPiece.RightHighBase, (int) LocationCode.MANOR_RIGHT_HIGH_BASE },
            { ManorPiece.RightHighUpper, (int) LocationCode.MANOR_RIGHT_HIGH_UPPER },
            { ManorPiece.RightHighTower, (int) LocationCode.MANOR_RIGHT_HIGH_TOWER },
            { ManorPiece.RightExtension, (int) LocationCode.MANOR_RIGHT_EXTENSION },
            { ManorPiece.RightTree, (int) LocationCode.MANOR_RIGHT_TREE },
            { ManorPiece.ObservatoryBase, (int) LocationCode.MANOR_OBSERVATORY_BASE },
            { ManorPiece.ObservatoryTelescope, (int) LocationCode.MANOR_OBSERVATORY_TELESCOPE }
        };
    }

    public static Dictionary<ManorPiece, int> ArchipelagoLocationTable { get; }
}

public enum ManorPiece
{
    None                 = -1,
    GroundRoad           = 24,
    MainBase             = 26,
    MainWindowBottom     = 28,
    MainWindowTop        = 27,
    MainRoof             = 25,
    LeftWingBase         = 21,
    LeftWingWindow       = 22,
    LeftWingRoof         = 20,
    LeftBigBase          = 19,
    LeftBigUpper1        = 18,
    LeftBigUpper2        = 16,
    LeftBigWindows       = 17,
    LeftBigRoof          = 15,
    LeftFarBase          = 14,
    LeftFarRoof          = 13,
    LeftExtension        = 12,
    LeftTree1            = 29,
    LeftTree2            = 30,
    RightWingBase        = 10,
    RightWingWindow      = 11,
    RightWingRoof        = 9,
    RightBigBase         = 8,
    RightBigUpper        = 7,
    RightBigRoof         = 6,
    RightHighBase        = 4,
    RightHighUpper       = 3,
    RightHighTower       = 2,
    RightExtension       = 5,
    RightTree            = 31,
    ObservatoryBase      = 1,
    ObservatoryTelescope = 0
}
