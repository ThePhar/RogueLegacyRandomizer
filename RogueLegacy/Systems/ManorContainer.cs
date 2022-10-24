// Rogue Legacy Randomizer - ManorContainer.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using Archipelago.Definitions;

namespace RogueLegacy.Systems
{
    public static class ManorContainer
    {
        static ManorContainer()
        {
            // Define a lookup table between our internal identifiers and the ones used by the AP service.
            ArchipelagoLocationTable = new Dictionary<ManorPiece, int>
            {
                { ManorPiece.GroundRoad, (int) LocationDefinitions.ManorGroundRoad.Code },
                { ManorPiece.MainBase, (int) LocationDefinitions.ManorMainBase.Code },
                { ManorPiece.MainWindowBottom, (int) LocationDefinitions.ManorMainBottomWindow.Code },
                { ManorPiece.MainWindowTop, (int) LocationDefinitions.ManorMainTopWindow.Code },
                { ManorPiece.MainRoof, (int) LocationDefinitions.ManorMainRoof.Code },
                { ManorPiece.LeftWingBase, (int) LocationDefinitions.ManorLeftWingBase.Code },
                { ManorPiece.LeftWingWindow, (int) LocationDefinitions.ManorLeftWingWindow.Code },
                { ManorPiece.LeftWingRoof, (int) LocationDefinitions.ManorLeftWingRoof.Code },
                { ManorPiece.LeftBigBase, (int) LocationDefinitions.ManorLeftBigBase.Code },
                { ManorPiece.LeftBigUpper1, (int) LocationDefinitions.ManorLeftBigUpper1.Code },
                { ManorPiece.LeftBigUpper2, (int) LocationDefinitions.ManorLeftBigUpper2.Code },
                { ManorPiece.LeftBigWindows, (int) LocationDefinitions.ManorLeftBigWindows.Code },
                { ManorPiece.LeftBigRoof, (int) LocationDefinitions.ManorLeftBigRoof.Code },
                { ManorPiece.LeftFarBase, (int) LocationDefinitions.ManorLeftFarBase.Code },
                { ManorPiece.LeftFarRoof, (int) LocationDefinitions.ManorLeftFarRoof.Code },
                { ManorPiece.LeftExtension, (int) LocationDefinitions.ManorLeftExtension.Code },
                { ManorPiece.LeftTree1, (int) LocationDefinitions.ManorLeftTree1.Code },
                { ManorPiece.LeftTree2, (int) LocationDefinitions.ManorLeftTree2.Code },
                { ManorPiece.RightWingBase, (int) LocationDefinitions.ManorRightWingBase.Code },
                { ManorPiece.RightWingWindow, (int) LocationDefinitions.ManorRightWingWindow.Code },
                { ManorPiece.RightWingRoof, (int) LocationDefinitions.ManorRightWingRoof.Code },
                { ManorPiece.RightBigBase, (int) LocationDefinitions.ManorRightBigBase.Code },
                { ManorPiece.RightBigUpper, (int) LocationDefinitions.ManorRightBigUpper.Code },
                { ManorPiece.RightBigRoof, (int) LocationDefinitions.ManorRightBigRoof.Code },
                { ManorPiece.RightHighBase, (int) LocationDefinitions.ManorRightHighBase.Code },
                { ManorPiece.RightHighUpper, (int) LocationDefinitions.ManorRightHighUpper.Code },
                { ManorPiece.RightHighTower, (int) LocationDefinitions.ManorRightHighTower.Code },
                { ManorPiece.RightExtension, (int) LocationDefinitions.ManorRightExtension.Code },
                { ManorPiece.RightTree, (int) LocationDefinitions.ManorRightTree.Code },
                { ManorPiece.ObservatoryBase, (int) LocationDefinitions.ManorObservatoryBase.Code },
                { ManorPiece.ObservatoryTelescope, (int) LocationDefinitions.ManorObservatoryScope.Code }
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
}
