// 
//  RogueLegacyArchipelago - SkillSystem.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RogueCastle.Structs;
using RogueCastle.Systems;

namespace RogueCastle
{
    public class SkillSystem
    {
        private const SkillType StartingTrait = SkillType.ManorMainBase;
        private static SkillObj _blankTrait;
        private static readonly Dictionary<SkillType, SkillMeta> SkillMeta;

        static SkillSystem()
        {
            // // Note: this type is marked as 'beforefieldinit'.
            // var array = new SkillType[10, 10];
            // array[0, 8] = SkillType.RandomizeChildren;
            // array[1, 7] = SkillType.SuperSecret;
            // array[1, 8] = SkillType.ManaCostDown;
            // array[2, 7] = SkillType.InvulnerabilityTimeUp;
            // array[2, 8] = SkillType.SpellSwordUp;
            // array[3, 2] = SkillType.DownStrikeUp;
            // array[3, 8] = SkillType.SpellswordUnlock;
            // array[4, 2] = SkillType.NinjaUp;
            // array[4, 3] = SkillType.ArmorUp;
            // array[4, 8] = SkillType.PotionUp;
            // array[5, 0] = SkillType.LichUp;
            // array[5, 1] = SkillType.LichUnlock;
            // array[5, 2] = SkillType.PricesDown;
            // array[5, 4] = SkillType.AttackUp;
            // array[5, 6] = SkillType.MagicDamageUp;
            // array[5, 8] = SkillType.AssassinUp;
            // array[6, 0] = SkillType.DeathDodge;
            // array[6, 2] = SkillType.NinjaUnlock;
            // array[6, 3] = SkillType.BarbarianUp;
            // array[6, 4] = SkillType.Architect;
            // array[6, 5] = SkillType.EquipUp;
            // array[6, 6] = SkillType.Enchanter;
            // array[6, 7] = SkillType.MageUp;
            // array[6, 8] = SkillType.BankerUnlock;
            // array[6, 9] = SkillType.BankerUp;
            // array[7, 2] = SkillType.CritChanceUp;
            // array[7, 5] = SkillType.KnightUp;
            // array[7, 8] = SkillType.GoldGainUp;
            // array[8, 2] = SkillType.CritDamageUp;
            // array[8, 5] = SkillType.HealthUp;
            // array[9, 5] = SkillType.Smithy;
            // array[9, 6] = SkillType.ManaUp;
            // array[9, 7] = SkillType.ManorUpgrade;
            // array[9, 8] = SkillType.Traitorous;
            // m_skillTypeArray = array;
            //
            // var array2 = new Vector2[10, 10];
            // array2[0, 0] = new Vector2(0f, 0f);
            // array2[0, 1] = new Vector2(0f, 0f);
            // array2[0, 2] = new Vector2(0f, 0f);
            // array2[0, 3] = new Vector2(0f, 0f);
            // array2[0, 4] = new Vector2(0f, 0f);
            // array2[0, 5] = new Vector2(0f, 0f);
            // array2[0, 6] = new Vector2(0f, 0f);
            // array2[0, 7] = new Vector2(0f, 0f);
            // array2[0, 8] = new Vector2(860f, 125f);
            // array2[0, 9] = new Vector2(0f, 0f);
            // array2[1, 0] = new Vector2(0f, 0f);
            // array2[1, 1] = new Vector2(0f, 0f);
            // array2[1, 2] = new Vector2(0f, 0f);
            // array2[1, 3] = new Vector2(0f, 0f);
            // array2[1, 4] = new Vector2(0f, 0f);
            // array2[1, 5] = new Vector2(0f, 0f);
            // array2[1, 6] = new Vector2(0f, 0f);
            // array2[1, 7] = new Vector2(655f, -100f);
            // array2[1, 8] = new Vector2(735f, 95f);
            // array2[1, 9] = new Vector2(0f, 0f);
            // array2[2, 0] = new Vector2(0f, 0f);
            // array2[2, 1] = new Vector2(0f, 0f);
            // array2[2, 2] = new Vector2(0f, 0f);
            // array2[2, 3] = new Vector2(0f, 0f);
            // array2[2, 4] = new Vector2(0f, 0f);
            // array2[2, 5] = new Vector2(0f, 0f);
            // array2[2, 6] = new Vector2(0f, 0f);
            // array2[2, 7] = new Vector2(655f, 50f);
            // array2[2, 8] = new Vector2(655f, 125f);
            // array2[2, 9] = new Vector2(0f, 0f);
            // array2[3, 0] = new Vector2(0f, 0f);
            // array2[3, 1] = new Vector2(0f, 0f);
            // array2[3, 2] = new Vector2(365f, 150f);
            // array2[3, 3] = new Vector2(0f, 0f);
            // array2[3, 4] = new Vector2(0f, 0f);
            // array2[3, 5] = new Vector2(0f, 0f);
            // array2[3, 6] = new Vector2(0f, 0f);
            // array2[3, 7] = new Vector2(0f, 0f);
            // array2[3, 8] = new Vector2(655f, 200f);
            // array2[3, 9] = new Vector2(0f, 0f);
            // array2[4, 0] = new Vector2(0f, 0f);
            // array2[4, 1] = new Vector2(0f, 0f);
            // array2[4, 2] = new Vector2(185f, 250f);
            // array2[4, 3] = new Vector2(365f, 250f);
            // array2[4, 4] = new Vector2(0f, 0f);
            // array2[4, 5] = new Vector2(0f, 0f);
            // array2[4, 6] = new Vector2(0f, 0f);
            // array2[4, 7] = new Vector2(0f, 0f);
            // array2[4, 8] = new Vector2(735f, 200f);
            // array2[4, 9] = new Vector2(0f, 0f);
            // array2[5, 0] = new Vector2(110f, 360f);
            // array2[5, 1] = new Vector2(110f, 460f);
            // array2[5, 2] = new Vector2(185f, 360f);
            // array2[5, 3] = new Vector2(0f, 0f);
            // array2[5, 4] = new Vector2(275f, 555f);
            // array2[5, 5] = new Vector2(0f, 0f);
            // array2[5, 6] = new Vector2(735f, 555f);
            // array2[5, 7] = new Vector2(0f, 0f);
            // array2[5, 8] = new Vector2(735f, 280f);
            // array2[5, 9] = new Vector2(0f, 0f);
            // array2[6, 0] = new Vector2(40f, 410f);
            // array2[6, 1] = new Vector2(0f, 0f);
            // array2[6, 2] = new Vector2(185f, 555f);
            // array2[6, 3] = new Vector2(275f, 360f);
            // array2[6, 4] = new Vector2(275f, 460f);
            // array2[6, 5] = new Vector2(505f, 315f);
            // array2[6, 6] = new Vector2(735f, 460f);
            // array2[6, 7] = new Vector2(735f, 360f);
            // array2[6, 8] = new Vector2(860f, 460f);
            // array2[6, 9] = new Vector2(938f, 415f);
            // array2[7, 0] = new Vector2(0f, 0f);
            // array2[7, 1] = new Vector2(0f, 0f);
            // array2[7, 2] = new Vector2(185f, 680f);
            // array2[7, 3] = new Vector2(0f, 0f);
            // array2[7, 4] = new Vector2(0f, 0f);
            // array2[7, 5] = new Vector2(505f, 410f);
            // array2[7, 6] = new Vector2(0f, 0f);
            // array2[7, 7] = new Vector2(0f, 0f);
            // array2[7, 8] = new Vector2(860f, 680f);
            // array2[7, 9] = new Vector2(0f, 0f);
            // array2[8, 0] = new Vector2(0f, 0f);
            // array2[8, 1] = new Vector2(0f, 0f);
            // array2[8, 2] = new Vector2(275f, 680f);
            // array2[8, 3] = new Vector2(0f, 0f);
            // array2[8, 4] = new Vector2(0f, 0f);
            // array2[8, 5] = new Vector2(505f, 490f);
            // array2[8, 6] = new Vector2(0f, 0f);
            // array2[8, 7] = new Vector2(0f, 0f);
            // array2[8, 8] = new Vector2(0f, 0f);
            // array2[8, 9] = new Vector2(0f, 0f);
            // array2[9, 0] = new Vector2(0f, 0f);
            // array2[9, 1] = new Vector2(0f, 0f);
            // array2[9, 2] = new Vector2(0f, 0f);
            // array2[9, 3] = new Vector2(0f, 0f);
            // array2[9, 4] = new Vector2(0f, 0f);
            // array2[9, 5] = new Vector2(505f, 590f);
            // array2[9, 6] = new Vector2(505f, 680f);
            // array2[9, 7] = new Vector2(605f, 680f);
            // array2[9, 8] = new Vector2(655f, -180f);
            // array2[9, 9] = new Vector2(0f, 0f);
            SkillMeta = new Dictionary<SkillType, SkillMeta>
            {
                // Manor Ground Road
                {
                    SkillType.ManorGroundRoad, new SkillMeta
                    {
                        Position = new Vector2(40f, 610f),
                        ManorPiece = ManorPiece.GroundRoad,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightWingWindow,
                            RightLink = SkillType.ManorMainWindowTop,
                            BottomLink = SkillType.ManorMainBase,
                        }
                    }
                },
                // Manor Main Base
                {
                    SkillType.ManorMainBase, new SkillMeta
                    {
                        Position = new Vector2(40f, 680f),
                        ManorPiece = ManorPiece.MainBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorGroundRoad,
                            RightLink = SkillType.ManorMainWindowBottom
                        }
                    }
                },
                // Manor Main Window Bottom
                {
                    SkillType.ManorMainWindowBottom, new SkillMeta
                    {
                        Position = new Vector2(110f, 680f),
                        ManorPiece = ManorPiece.MainWindowBottom,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorMainWindowTop,
                            RightLink = SkillType.Enchanter,
                            LeftLink = SkillType.ManorMainBase
                        }
                    }
                },
                // Manor Main Window Top
                {
                    SkillType.ManorMainWindowTop, new SkillMeta
                    {
                        Position = new Vector2(110f, 610f),
                        ManorPiece = ManorPiece.MainWindowTop,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorMainRoof,
                            RightLink = SkillType.Architect,
                            BottomLink = SkillType.ManorMainWindowBottom,
                            LeftLink = SkillType.ManorGroundRoad
                        }
                    }
                },
                // Manor Main Roof
                {
                    SkillType.ManorMainRoof, new SkillMeta
                    {
                        Position = new Vector2(110f, 540f),
                        ManorPiece = ManorPiece.MainRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightWingBase,
                            RightLink = SkillType.ManorLeftWingBase,
                            BottomLink = SkillType.ManorMainWindowTop
                        }
                    }
                },
                // Manor Left Wing Base
                {
                    SkillType.ManorLeftWingBase, new SkillMeta
                    {
                        Position = new Vector2(180f, 540f),
                        ManorPiece = ManorPiece.LeftWingBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftWingWindow,
                            RightLink = SkillType.Smithy,
                            LeftLink = SkillType.ManorMainRoof
                        }
                    }
                },
                // Manor Left Wing Window
                {
                    SkillType.ManorLeftWingWindow, new SkillMeta
                    {
                        Position = new Vector2(180f, 470f),
                        ManorPiece = ManorPiece.LeftWingWindow,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightExtension,
                            RightLink = SkillType.ManorLeftWingRoof,
                            BottomLink = SkillType.ManorLeftWingBase,
                            LeftLink = SkillType.ManorRightWingBase
                        }
                    }
                },
                // Manor Left Wing Roof
                {
                    SkillType.ManorLeftWingRoof, new SkillMeta
                    {
                        Position = new Vector2(250f, 470f),
                        ManorPiece = ManorPiece.LeftWingRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftBigBase,
                            RightLink = SkillType.HealthUp,
                            LeftLink = SkillType.ManorLeftWingWindow
                        }
                    }
                },
                // Manor Left Big Base
                {
                    SkillType.ManorLeftBigBase, new SkillMeta
                    {
                        Position = new Vector2(250f, 400f),
                        ManorPiece = ManorPiece.LeftBigBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftTree1,
                            RightLink = SkillType.ManorLeftBigUpper1,
                            BottomLink = SkillType.ManorLeftWingRoof,
                            LeftLink = SkillType.ManorRightWingRoof
                        }
                    }
                },
                // Manor Left Big Upper 1
                {
                    SkillType.ManorLeftBigUpper1, new SkillMeta
                    {
                        Position = new Vector2(320f, 400f),
                        ManorPiece = ManorPiece.LeftBigUpper1,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftBigUpper2,
                            LeftLink = SkillType.ManorLeftBigBase
                        }
                    }
                },
                // Manor Left Big Upper 2
                {
                    SkillType.ManorLeftBigUpper2, new SkillMeta
                    {
                        Position = new Vector2(320f, 330f),
                        ManorPiece = ManorPiece.LeftBigUpper2,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftBigRoof,
                            RightLink = SkillType.ManorLeftFarBase,
                            BottomLink = SkillType.ManorLeftBigUpper1,
                            LeftLink = SkillType.ManorLeftTree1
                        }
                    }
                },
                // Manor Left Big Windows
                {
                    SkillType.ManorLeftBigWindows, new SkillMeta
                    {
                        Position = new Vector2(320f, 190f),
                        ManorPiece = ManorPiece.LeftBigWindows,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.ManorLeftExtension,
                            BottomLink = SkillType.ManorLeftBigRoof,
                            LeftLink = SkillType.ManorRightBigRoof
                        }
                    }
                },
                // Manor Left Big Roof
                {
                    SkillType.ManorLeftBigRoof, new SkillMeta
                    {
                        Position = new Vector2(320f, 260f),
                        ManorPiece = ManorPiece.LeftBigRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftBigWindows,
                            RightLink = SkillType.ManorLeftFarRoof,
                            BottomLink = SkillType.ManorLeftBigUpper2,
                            LeftLink = SkillType.ManorLeftTree2
                        }
                    }
                },
                // Manor Left Far Base
                {
                    SkillType.ManorLeftFarBase, new SkillMeta
                    {
                        Position = new Vector2(390f, 330f),
                        ManorPiece = ManorPiece.LeftFarBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftFarRoof,
                            RightLink = SkillType.SuperSecret,
                            LeftLink = SkillType.ManorLeftBigUpper2
                        }
                    }
                },
                // Manor Far Roof
                {
                    SkillType.ManorLeftFarRoof, new SkillMeta
                    {
                        Position = new Vector2(390f, 260f),
                        ManorPiece = ManorPiece.LeftFarRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftExtension,
                            RightLink = SkillType.AssassinUp,
                            BottomLink = SkillType.ManorLeftFarBase,
                            LeftLink = SkillType.ManorLeftBigRoof
                        }
                    }
                },
                // Manor Left Extension
                {
                    SkillType.ManorLeftExtension, new SkillMeta
                    {
                        Position = new Vector2(390f, 190f),
                        ManorPiece = ManorPiece.LeftExtension,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.BarbarianUp,
                            BottomLink = SkillType.ManorLeftFarRoof,
                            LeftLink = SkillType.ManorLeftBigWindows
                        }
                    }
                },
                // Manor Left Tree 1
                {
                    SkillType.ManorLeftTree1, new SkillMeta
                    {
                        Position = new Vector2(250f, 330f),
                        ManorPiece = ManorPiece.LeftTree1,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorLeftTree2,
                            RightLink = SkillType.ManorLeftBigUpper2,
                            BottomLink = SkillType.ManorLeftBigBase,
                            LeftLink = SkillType.ManorRightBigBase
                        }
                    }
                },
                // Manor Left Tree 2
                {
                    SkillType.ManorLeftTree2, new SkillMeta
                    {
                        Position = new Vector2(250f, 260f),
                        ManorPiece = ManorPiece.LeftTree2,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.ManorLeftBigRoof,
                            BottomLink = SkillType.ManorLeftTree1,
                            LeftLink = SkillType.ManorRightExtension
                        }
                    }
                },
                // Manor Right Wing Base
                {
                    SkillType.ManorRightWingBase, new SkillMeta
                    {
                        Position = new Vector2(110f, 470f),
                        ManorPiece = ManorPiece.RightWingBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightWingRoof,
                            RightLink = SkillType.ManorLeftWingWindow,
                            BottomLink = SkillType.ManorMainRoof
                        }
                    }
                },
                // Manor Right Wing Window
                {
                    SkillType.ManorRightWingWindow, new SkillMeta
                    {
                        Position = new Vector2(40f, 400f),
                        ManorPiece = ManorPiece.RightWingWindow,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightTree,
                            RightLink = SkillType.ManorRightWingRoof,
                            BottomLink = SkillType.ManorGroundRoad
                        }
                    }
                },
                // Manor Right Wing Roof
                {
                    SkillType.ManorRightWingRoof, new SkillMeta
                    {
                        Position = new Vector2(110f, 400f),
                        ManorPiece = ManorPiece.RightWingRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightBigBase,
                            RightLink = SkillType.ManorLeftBigBase,
                            BottomLink = SkillType.ManorRightWingBase,
                            LeftLink = SkillType.ManorRightWingWindow
                        }
                    }
                },
                // Manor Right Big Base
                {
                    SkillType.ManorRightBigBase, new SkillMeta
                    {
                        Position = new Vector2(110f, 330f),
                        ManorPiece = ManorPiece.RightBigBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightBigUpper,
                            RightLink = SkillType.ManorLeftTree1,
                            BottomLink = SkillType.ManorRightWingRoof
                        }
                    }
                },
                // Manor Right Big Upper
                {
                    SkillType.ManorRightBigUpper, new SkillMeta
                    {
                        Position = new Vector2(110f, 260f),
                        ManorPiece = ManorPiece.RightBigUpper,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightBigRoof,
                            RightLink = SkillType.ManorRightExtension,
                            BottomLink = SkillType.ManorRightBigBase,
                            LeftLink = SkillType.ManorRightTree
                        }
                    }
                },
                // Manor Right Big Roof
                {
                    SkillType.ManorRightBigRoof, new SkillMeta
                    {
                        Position = new Vector2(110f, 190f),
                        ManorPiece = ManorPiece.RightBigRoof,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorObservatoryBase,
                            RightLink = SkillType.ManorLeftBigWindows,
                            BottomLink = SkillType.ManorRightBigUpper,
                            LeftLink = SkillType.ManorRightHighBase
                        }
                    }
                },
                // Manor Right High Base
                {
                    SkillType.ManorRightHighBase, new SkillMeta
                    {
                        Position = new Vector2(40f, 190f),
                        ManorPiece = ManorPiece.RightHighBase,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightHighUpper,
                            RightLink = SkillType.ManorRightBigRoof,
                            BottomLink = SkillType.ManorRightTree
                        }
                    }
                },
                // Manor Right High Upper
                {
                    SkillType.ManorRightHighUpper, new SkillMeta
                    {
                        Position = new Vector2(40f, 120f),
                        ManorPiece = ManorPiece.RightHighUpper,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightHighTower,
                            RightLink = SkillType.ManorObservatoryBase,
                            BottomLink = SkillType.ManorRightHighBase
                        }
                    }
                },
                // Manor Right High Tower
                {
                    SkillType.ManorRightHighTower, new SkillMeta
                    {
                        Position = new Vector2(40f, 50f),
                        ManorPiece = ManorPiece.RightHighTower,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.KnightUp,
                            BottomLink = SkillType.ManorRightHighUpper
                        }
                    }
                },
                // Manor Right Extension
                {
                    SkillType.ManorRightExtension, new SkillMeta
                    {
                        Position = new Vector2(180f, 260f),
                        ManorPiece = ManorPiece.RightExtension,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorObservatoryTelescope,
                            RightLink = SkillType.ManorLeftTree2,
                            BottomLink = SkillType.ManorLeftWingWindow,
                            LeftLink = SkillType.ManorRightBigUpper
                        }
                    }
                },
                // Manor Right Tree
                {
                    SkillType.ManorRightTree, new SkillMeta
                    {
                        Position = new Vector2(40f, 260f),
                        ManorPiece = ManorPiece.RightTree,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManorRightHighBase,
                            RightLink = SkillType.ManorRightBigUpper,
                            BottomLink = SkillType.ManorRightWingWindow
                        }
                    }
                },
                // Manor Observatory Base
                {
                    SkillType.ManorObservatoryBase, new SkillMeta
                    {
                        Position = new Vector2(110f, 120f),
                        ManorPiece = ManorPiece.ObservatoryBase,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.ManorObservatoryTelescope,
                            BottomLink = SkillType.ManorRightBigRoof,
                            LeftLink = SkillType.ManorRightHighUpper
                        }
                    }
                },
                // Manor Observatory Telescope
                {
                    SkillType.ManorObservatoryTelescope, new SkillMeta
                    {
                        Position = new Vector2(180f, 120f),
                        ManorPiece = ManorPiece.ObservatoryTelescope,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.MageUp,
                            BottomLink = SkillType.ManorRightExtension,
                            LeftLink = SkillType.ManorObservatoryBase
                        }
                    }
                },
                // Smithy
                {
                    SkillType.Smithy, new SkillMeta
                    {
                        Position = new Vector2(580f, 530f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            BottomLink = SkillType.Architect,
                            RightLink = SkillType.ArmorUp,
                            LeftLink = SkillType.ManorLeftWingBase
                        }
                    }
                },
                // Architect
                {
                    SkillType.Architect, new SkillMeta
                    {
                        Position = new Vector2(580f, 600f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.Smithy,
                            RightLink = SkillType.DownStrikeUp,
                            BottomLink = SkillType.Enchanter,
                            LeftLink = SkillType.ManorMainWindowTop
                        }
                    }
                },
                // Enchanter
                {
                    SkillType.Enchanter, new SkillMeta
                    {
                        Position = new Vector2(580f, 670f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.Architect,
                            RightLink = SkillType.ManaCostDown,
                            LeftLink = SkillType.ManorMainWindowBottom
                        }
                    }
                },
                // Paladin
                {
                    SkillType.KnightUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 40f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.NinjaUnlock,
                            BottomLink = SkillType.MageUp,
                            LeftLink = SkillType.ManorRightHighTower
                        }
                    }
                },
                // Archmage
                {
                    SkillType.MageUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 110f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.KnightUp,
                            RightLink = SkillType.BankerUnlock,
                            BottomLink = SkillType.BarbarianUp,
                            LeftLink = SkillType.ManorObservatoryTelescope
                        }
                    }
                },
                // Barbarian King
                {
                    SkillType.BarbarianUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 180f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.MageUp,
                            RightLink = SkillType.LichUnlock,
                            BottomLink = SkillType.AssassinUp,
                            LeftLink = SkillType.ManorLeftExtension
                        }
                    }
                },
                // Assassin
                {
                    SkillType.AssassinUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 250f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.BarbarianUp,
                            RightLink = SkillType.SpellswordUnlock,
                            BottomLink = SkillType.ManaUp,
                            LeftLink = SkillType.ManorLeftFarRoof
                        }
                    }
                },
                // Shinobi Unlock
                {
                    SkillType.NinjaUnlock, new SkillMeta
                    {
                        Position = new Vector2(790f, 40f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.NinjaUp,
                            BottomLink = SkillType.BankerUnlock,
                            LeftLink = SkillType.KnightUp
                        }
                    }
                },
                // Hokage
                {
                    SkillType.NinjaUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 40f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            BottomLink = SkillType.BankerUp,
                            LeftLink = SkillType.NinjaUnlock
                        }
                    }
                },
                // Miner Unlock
                {
                    SkillType.BankerUnlock, new SkillMeta
                    {
                        Position = new Vector2(790f, 110f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.NinjaUnlock,
                            RightLink = SkillType.BankerUp,
                            BottomLink = SkillType.LichUnlock,
                            LeftLink = SkillType.MageUp
                        }
                    }
                },
                // Spelunker
                {
                    SkillType.BankerUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 110f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.NinjaUp,
                            BottomLink = SkillType.LichUp,
                            LeftLink = SkillType.BankerUnlock
                        }
                    }
                },
                // Lich Unlock
                {
                    SkillType.LichUnlock, new SkillMeta
                    {
                        Position = new Vector2(790f, 180f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.BankerUnlock,
                            RightLink = SkillType.LichUp,
                            BottomLink = SkillType.SpellswordUnlock,
                            LeftLink = SkillType.BarbarianUp
                        }
                    }
                },
                // Lich King
                {
                    SkillType.LichUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 180f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.BankerUp,
                            BottomLink = SkillType.SpellSwordUp,
                            LeftLink = SkillType.LichUnlock
                        }
                    }
                },
                // Spellthief Unlock
                {
                    SkillType.SpellswordUnlock, new SkillMeta
                    {
                        Position = new Vector2(790f, 250f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.LichUnlock,
                            RightLink = SkillType.SpellSwordUp,
                            BottomLink = SkillType.SuperSecret,
                            LeftLink = SkillType.AssassinUp
                        }
                    }
                },
                // Spellsword
                {
                    SkillType.SpellSwordUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 250f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.LichUp,
                            BottomLink = SkillType.Traitorous,
                            LeftLink = SkillType.SpellswordUnlock
                        }
                    }
                },
                // Dragon
                {
                    SkillType.SuperSecret, new SkillMeta
                    {
                        Position = new Vector2(790f, 320f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.SpellswordUnlock,
                            RightLink = SkillType.Traitorous,
                            BottomLink = SkillType.AttackUp,
                            LeftLink = SkillType.ManorLeftFarBase
                        }
                    }
                },
                // Traitor
                {
                    SkillType.Traitorous, new SkillMeta
                    {
                        Position = new Vector2(860f, 320f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.SpellSwordUp,
                            BottomLink = SkillType.MagicDamageUp,
                            LeftLink = SkillType.SuperSecret
                        }
                    }
                },
                // Health Up
                {
                    SkillType.HealthUp, new SkillMeta
                    {
                        Position = new Vector2(650f, 460f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            RightLink = SkillType.ManaUp,
                            BottomLink = SkillType.ArmorUp,
                            LeftLink = SkillType.ManorLeftWingRoof
                        }
                    }
                },
                // Mana Up
                {
                    SkillType.ManaUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 460f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.AssassinUp,
                            RightLink = SkillType.AttackUp,
                            BottomLink = SkillType.EquipUp,
                            LeftLink = SkillType.HealthUp
                        }
                    }
                },
                // Attack Up
                {
                    SkillType.AttackUp, new SkillMeta
                    {
                        Position = new Vector2(790f, 460f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.SuperSecret,
                            RightLink = SkillType.MagicDamageUp,
                            BottomLink = SkillType.CritChanceUp,
                            LeftLink = SkillType.ManaUp
                        }
                    }
                },
                // Magic Damage Up
                {
                    SkillType.MagicDamageUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 460f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.Traitorous,
                            BottomLink = SkillType.CritDamageUp,
                            LeftLink = SkillType.AttackUp
                        }
                    }
                },
                // Armor Up
                {
                    SkillType.ArmorUp, new SkillMeta
                    {
                        Position = new Vector2(650f, 530f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.HealthUp,
                            RightLink = SkillType.EquipUp,
                            BottomLink = SkillType.DownStrikeUp,
                            LeftLink = SkillType.Smithy
                        }
                    }
                },
                // Equip Up
                {
                    SkillType.EquipUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 530f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ManaUp,
                            RightLink = SkillType.CritChanceUp,
                            BottomLink = SkillType.GoldGainUp,
                            LeftLink = SkillType.ArmorUp
                        }
                    }
                },
                // Crit Chance
                {
                    SkillType.CritChanceUp, new SkillMeta
                    {
                        Position = new Vector2(790f, 530f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.AttackUp,
                            RightLink = SkillType.CritDamageUp,
                            BottomLink = SkillType.PotionUp,
                            LeftLink = SkillType.EquipUp
                        }
                    }
                },
                // Crit Damage
                {
                    SkillType.CritDamageUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 530f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.MagicDamageUp,
                            BottomLink = SkillType.InvulnerabilityTimeUp,
                            LeftLink = SkillType.CritChanceUp
                        }
                    }
                },
                // Down Strike
                {
                    SkillType.DownStrikeUp, new SkillMeta
                    {
                        Position = new Vector2(650f, 600f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.ArmorUp,
                            RightLink = SkillType.GoldGainUp,
                            BottomLink = SkillType.ManaCostDown,
                            LeftLink = SkillType.Architect
                        }
                    }
                },
                // Gold Gain Up
                {
                    SkillType.GoldGainUp, new SkillMeta
                    {
                        Position = new Vector2(720f, 600f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.EquipUp,
                            RightLink = SkillType.PotionUp,
                            BottomLink = SkillType.DeathDodge,
                            LeftLink = SkillType.DownStrikeUp
                        }
                    }
                },
                // Potion Up
                {
                    SkillType.PotionUp, new SkillMeta
                    {
                        Position = new Vector2(790f, 600f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.CritChanceUp,
                            RightLink = SkillType.InvulnerabilityTimeUp,
                            BottomLink = SkillType.PricesDown,
                            LeftLink = SkillType.GoldGainUp
                        }
                    }
                },
                // Invuln Up
                {
                    SkillType.InvulnerabilityTimeUp, new SkillMeta
                    {
                        Position = new Vector2(860f, 600f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.CritDamageUp,
                            BottomLink = SkillType.RandomizeChildren,
                            LeftLink = SkillType.PotionUp
                        }
                    }
                },
                // Mana Cost Down
                {
                    SkillType.ManaCostDown, new SkillMeta
                    {
                        Position = new Vector2(650f, 670f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.DownStrikeUp,
                            RightLink = SkillType.DeathDodge,
                            LeftLink = SkillType.Enchanter
                        }
                    }
                },
                // Death Defy
                {
                    SkillType.DeathDodge, new SkillMeta
                    {
                        Position = new Vector2(720f, 670f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.GoldGainUp,
                            RightLink = SkillType.PricesDown,
                            LeftLink = SkillType.ManaCostDown
                        }
                    }
                },
                // Haggle
                {
                    SkillType.PricesDown, new SkillMeta
                    {
                        Position = new Vector2(790f, 670f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.PotionUp,
                            RightLink = SkillType.RandomizeChildren,
                            LeftLink = SkillType.DeathDodge
                        }
                    }
                },
                // Randomize Children
                {
                    SkillType.RandomizeChildren, new SkillMeta
                    {
                        Position = new Vector2(860f, 670f), ManorPiece = ManorPiece.None,
                        SkillLink = new SkillLink
                        {
                            TopLink = SkillType.InvulnerabilityTimeUp,
                            LeftLink = SkillType.PricesDown
                        }
                    }
                }
            };

            IconsVisible = true;
        }

        public static List<SkillObj> SkillArray { get; private set; }
        public static bool IconsVisible { get; private set; }

        public static void Initialize()
        {
            _blankTrait = new SkillObj("Icon_Sword_Sprite");

            SkillArray = new List<SkillObj>();
            foreach (var meta in SkillMeta)
            {
                if (meta.Value.ManorPiece != ManorPiece.None)
                {
                    var manorObj = SkillBuilder.BuildSkill(SkillType.Manor);
                    manorObj.Position = GetSkillPosition(manorObj);
                    manorObj.ManorPiece = SkillMeta[meta.Key].ManorPiece;
                    SkillArray.Add(manorObj);
                    continue;
                }

                var skillObj = SkillBuilder.BuildSkill(meta.Key);
                skillObj.Position = GetSkillPosition(skillObj);
                SkillArray.Add(skillObj);
            }
        }

        public static void LevelUpTrait(SkillObj trait, bool giveGoldBonus, bool level = true)
        {
            trait.CurrentLevel++;

            if (level)
            {
                Game.PlayerStats.CurrentLevel++;
            }

            UpdateTraitSprite(trait);
            if (trait.TraitType == SkillType.GoldFlatBonus && giveGoldBonus)
            {
                Game.PlayerStats.Gold += (int) trait.ModifierAmount;
            }
        }

        public static void ResetAllTraits()
        {
            foreach (var current in SkillArray)
            {
                current.CurrentLevel = 0;
                current.Visible = false;
            }

            GetSkill(StartingTrait).Visible = true;
            Game.PlayerStats.CurrentLevel = 0;
        }

        public static void UpdateAllTraitSprites()
        {
            foreach (var current in SkillArray) UpdateTraitSprite(current);
        }

        public static void UpdateTraitSprite(SkillObj trait)
        {
            var text = trait.IconName;
            if (trait.CurrentLevel > 0 && trait.CurrentLevel < trait.MaxLevel)
            {
                text = text.Replace("Locked", "");
            }
            else if (trait.CurrentLevel > 0 && trait.CurrentLevel >= trait.MaxLevel)
            {
                text = text.Replace("Locked", "Max");
            }

            Console.WriteLine(trait.Name);
            trait.ChangeSprite(text);
        }

        public static SkillObj GetSkill(SkillType skillType)
        {
            Console.WriteLine("GET SKILL: " + Enum.GetName(typeof(SkillType), skillType));
            try
            {
                var meta = SkillMeta.First(kp => kp.Key == skillType).Value;

                if (meta.ManorPiece != ManorPiece.None && SkillMeta.ContainsKey(skillType))
                {
                    return SkillArray.First(skill => skill.ManorPiece == meta.ManorPiece);
                }
            }
            catch { }

            foreach (var current in SkillArray.Where(current => current.TraitType == skillType))
                return current;

            return _blankTrait;
        }

        public static Vector2 GetSkillPosition(SkillObj skill)
        {
            if (skill.TraitType == SkillType.Manor)
            {
                return SkillMeta.First(kp => kp.Value.ManorPiece == skill.ManorPiece).Value.Position;
            }

            return SkillMeta[skill.TraitType].Position;
        }

        public static SkillObj[] GetSkillArray()
        {
            return SkillArray.ToArray();
        }

        public static ManorPiece GetManorPiece(SkillObj trait)
        {
            if (trait.TraitType == SkillType.Manor)
            {
                return SkillMeta.First(kp => kp.Value.ManorPiece == trait.ManorPiece).Value.ManorPiece;
            }

            return ManorPiece.None;
        }

        public static SkillMeta GetSkillMeta(SkillType skillType)
        {
            return SkillMeta[skillType];
        }

        public static void HideAllIcons()
        {
            foreach (var current in SkillArray) current.Opacity = 0f;
            IconsVisible = false;
        }

        public static void ShowAllIcons()
        {
            foreach (var current in SkillArray) current.Opacity = 1f;
            IconsVisible = true;
        }
    }
}
