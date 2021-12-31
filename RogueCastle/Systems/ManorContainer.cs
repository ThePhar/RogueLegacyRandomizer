// 
//  RogueLegacyArchipelago - ManorContainer.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using Archipelago;

namespace RogueCastle.Systems
{
    public static class ManorContainer
    {
        static ManorContainer()
        {
            // Define a lookup table between our internal identifiers and the ones used by the AP service.
            ArchipelagoLocationTable = new Dictionary<ManorPiece, LocationCode>
            {
                { ManorPiece.GroundRoad, LocationCode.ManorGroundRoad },
                { ManorPiece.MainBase, LocationCode.ManorMainBase },
                { ManorPiece.MainWindowBottom, LocationCode.ManorMainWindowBottom },
                { ManorPiece.MainWindowTop, LocationCode.ManorMainWindowTop },
                { ManorPiece.MainRoof, LocationCode.ManorMainRoof },
                { ManorPiece.LeftWingBase, LocationCode.ManorLeftWingBase },
                { ManorPiece.LeftWingWindow, LocationCode.ManorLeftWingWindow },
                { ManorPiece.LeftWingRoof, LocationCode.ManorLeftWingRoof },
                { ManorPiece.LeftBigBase, LocationCode.ManorLeftBigBase },
                { ManorPiece.LeftBigUpper1, LocationCode.ManorLeftBigUpper1 },
                { ManorPiece.LeftBigUpper2, LocationCode.ManorLeftBigUpper2 },
                { ManorPiece.LeftBigWindows, LocationCode.ManorLeftBigWindows },
                { ManorPiece.LeftBigRoof, LocationCode.ManorLeftBigRoof },
                { ManorPiece.LeftFarBase, LocationCode.ManorLeftFarBase },
                { ManorPiece.LeftFarRoof, LocationCode.ManorLeftFarRoof },
                { ManorPiece.LeftExtension, LocationCode.ManorLeftExtension },
                { ManorPiece.LeftTree1, LocationCode.ManorLeftTree1 },
                { ManorPiece.LeftTree2, LocationCode.ManorLeftTree2 },
                { ManorPiece.RightWingBase, LocationCode.ManorRightWingBase },
                { ManorPiece.RightWingWindow, LocationCode.ManorRightWingWindow },
                { ManorPiece.RightWingRoof, LocationCode.ManorRightWingRoof },
                { ManorPiece.RightBigBase, LocationCode.ManorRightBigBase },
                { ManorPiece.RightBigUpper, LocationCode.ManorRightBigUpper },
                { ManorPiece.RightBigRoof, LocationCode.ManorRightBigRoof },
                { ManorPiece.RightHighBase, LocationCode.ManorRightHighBase },
                { ManorPiece.RightHighUpper, LocationCode.ManorRightHighUpper },
                { ManorPiece.RightHighTower, LocationCode.ManorRightHighTower },
                { ManorPiece.RightExtension, LocationCode.ManorRightExtension },
                { ManorPiece.RightTree, LocationCode.ManorRightTree },
                { ManorPiece.ObservatoryBase, LocationCode.ManorObservatoryBase },
                { ManorPiece.ObservatoryTelescope, LocationCode.ManorObservatoryTelescope }
            };
        }

        public static Dictionary<ManorPiece, LocationCode> ArchipelagoLocationTable { get; private set; }
    }

    public enum ManorPiece
    {
        None = -1,
        GroundRoad = 24,
        MainBase = 26,
        MainWindowBottom = 28,
        MainWindowTop = 27,
        MainRoof = 25,
        LeftWingBase = 21,
        LeftWingWindow = 22,
        LeftWingRoof = 20,
        LeftBigBase = 19,
        LeftBigUpper1 = 18,
        LeftBigUpper2 = 16,
        LeftBigWindows = 17,
        LeftBigRoof = 15,
        LeftFarBase = 14,
        LeftFarRoof = 13,
        LeftExtension = 12,
        LeftTree1 = 29,
        LeftTree2 = 30,
        RightWingBase = 10,
        RightWingWindow = 11,
        RightWingRoof = 9,
        RightBigBase = 8,
        RightBigUpper = 7,
        RightBigRoof = 6,
        RightHighBase = 4,
        RightHighUpper = 3,
        RightHighTower = 2,
        RightExtension = 5,
        RightTree = 31,
        ObservatoryBase = 1,
        ObservatoryTelescope = 0
    }
}