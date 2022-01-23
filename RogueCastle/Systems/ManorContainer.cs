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
using Archipelago.Definitions;

namespace RogueCastle.Systems
{
    public static class ManorContainer
    {
        static ManorContainer()
        {
            // Define a lookup table between our internal identifiers and the ones used by the AP service.
            ArchipelagoLocationTable = new Dictionary<ManorPiece, int>
            {
                { ManorPiece.GroundRoad, LocationDefinitions.ManorGroundRoad.Code },
                { ManorPiece.MainBase, LocationDefinitions.ManorMainBase.Code },
                { ManorPiece.MainWindowBottom, LocationDefinitions.ManorMainBottomWindow.Code },
                { ManorPiece.MainWindowTop, LocationDefinitions.ManorMainTopWindow.Code },
                { ManorPiece.MainRoof, LocationDefinitions.ManorMainRoof.Code },
                { ManorPiece.LeftWingBase, LocationDefinitions.ManorLeftWingBase.Code },
                { ManorPiece.LeftWingWindow, LocationDefinitions.ManorLeftWingWindow.Code },
                { ManorPiece.LeftWingRoof, LocationDefinitions.ManorLeftWingRoof.Code },
                { ManorPiece.LeftBigBase, LocationDefinitions.ManorLeftBigBase.Code },
                { ManorPiece.LeftBigUpper1, LocationDefinitions.ManorLeftBigUpper1.Code },
                { ManorPiece.LeftBigUpper2, LocationDefinitions.ManorLeftBigUpper2.Code },
                { ManorPiece.LeftBigWindows, LocationDefinitions.ManorLeftBigWindows.Code },
                { ManorPiece.LeftBigRoof, LocationDefinitions.ManorLeftBigRoof.Code },
                { ManorPiece.LeftFarBase, LocationDefinitions.ManorLeftFarBase.Code },
                { ManorPiece.LeftFarRoof, LocationDefinitions.ManorLeftFarRoof.Code },
                { ManorPiece.LeftExtension, LocationDefinitions.ManorLeftExtension.Code },
                { ManorPiece.LeftTree1, LocationDefinitions.ManorLeftTree1.Code },
                { ManorPiece.LeftTree2, LocationDefinitions.ManorLeftTree2.Code },
                { ManorPiece.RightWingBase, LocationDefinitions.ManorRightWingBase.Code },
                { ManorPiece.RightWingWindow, LocationDefinitions.ManorRightWingWindow.Code },
                { ManorPiece.RightWingRoof, LocationDefinitions.ManorRightWingRoof.Code },
                { ManorPiece.RightBigBase, LocationDefinitions.ManorRightBigBase.Code },
                { ManorPiece.RightBigUpper, LocationDefinitions.ManorRightBigUpper.Code },
                { ManorPiece.RightBigRoof, LocationDefinitions.ManorRightBigRoof.Code },
                { ManorPiece.RightHighBase, LocationDefinitions.ManorRightHighBase.Code },
                { ManorPiece.RightHighUpper, LocationDefinitions.ManorRightHighUpper.Code },
                { ManorPiece.RightHighTower, LocationDefinitions.ManorRightHighTower.Code },
                { ManorPiece.RightExtension, LocationDefinitions.ManorRightExtension.Code },
                { ManorPiece.RightTree, LocationDefinitions.ManorRightTree.Code },
                { ManorPiece.ObservatoryBase, LocationDefinitions.ManorObservatoryBase.Code },
                { ManorPiece.ObservatoryTelescope, LocationDefinitions.ManorObservatoryScope.Code },
            };
        }

        public static Dictionary<ManorPiece, int> ArchipelagoLocationTable { get; private set; }
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
