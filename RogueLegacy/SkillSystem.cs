// RogueLegacyRandomizer - SkillSystem.cs
// Last Modified 2023-08-03 3:18 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Systems;

namespace RogueLegacy
{
    public class SkillData
    {
        public SkillType    SkillType  { get; init; } = SkillType.Null;
        public Vector2      Position   { get; init; } = new Vector2(-80, -80);
        public ManorPiece   ManorPiece { get; init; } = ManorPiece.None;
        public ManorPiece[] Connected  { get; init; } = Array.Empty<ManorPiece>();
    }

    public class SkillSystem
    {
        private const SkillType STARTING_TRAIT = SkillType.ManorMainBase;

        private static          SkillObj       _blankTrait;
        private static readonly Vector2[,]     _skillPositionArray;
        private static readonly int[,]         _manorPieceArray;

        public static readonly SkillType[,] SkillTypeArray;
        public static readonly SkillData[,] Skills;

        static SkillSystem()
        {
            Skills = new SkillData[13, 10];
            Skills[ 0, 0] = new SkillData { SkillType = SkillType.Smithy, Position           = new Vector2(45 + (0 * 75), 675 - (0 * 75)) };
            Skills[ 1, 0] = new SkillData { SkillType = SkillType.Enchanter, Position        = new Vector2(45 + (1 * 75), 675 - (0 * 75)) };
            Skills[ 3, 0] = new SkillData { SkillType = SkillType.Architect, Position        = new Vector2(45 + (3 * 75), 675 - (0 * 75)) };

            Skills[ 0, 1] = new SkillData { SkillType = SkillType.HealthUp, Position         = new Vector2(45 + (0 * 75), 675 - (1 * 75)) };
            Skills[ 1, 1] = new SkillData { SkillType = SkillType.ManaUp, Position           = new Vector2(45 + (1 * 75), 675 - (1 * 75)) };
            Skills[ 2, 1] = new SkillData { SkillType = SkillType.AttackUp, Position         = new Vector2(45 + (2 * 75), 675 - (1 * 75)) };
            Skills[ 3, 1] = new SkillData { SkillType = SkillType.MagicDamageUp, Position    = new Vector2(45 + (3 * 75), 675 - (1 * 75)) };

            Skills[ 0, 2] = new SkillData { SkillType = SkillType.ArmorUp, Position          = new Vector2(45 + (0 * 75), 675 - (2 * 75)) };
            Skills[ 1, 2] = new SkillData { SkillType = SkillType.EquipUp, Position          = new Vector2(45 + (1 * 75), 675 - (2 * 75)) };
            Skills[ 2, 2] = new SkillData { SkillType = SkillType.CritChanceUp, Position     = new Vector2(45 + (2 * 75), 675 - (2 * 75)) };
            Skills[ 3, 2] = new SkillData { SkillType = SkillType.CritDamageUp, Position     = new Vector2(45 + (3 * 75), 675 - (2 * 75)) };

            Skills[ 0, 3] = new SkillData { SkillType = SkillType.DownStrikeUp, Position     = new Vector2(45 + (0 * 75), 675 - (3 * 75)) };
            Skills[ 1, 3] = new SkillData { SkillType = SkillType.GoldGainUp, Position       = new Vector2(45 + (1 * 75), 675 - (3 * 75)) };
            Skills[ 2, 3] = new SkillData { SkillType = SkillType.PotionUp, Position         = new Vector2(45 + (2 * 75), 675 - (3 * 75)) };
            Skills[ 3, 3] = new SkillData { SkillType = SkillType.InvulnTimeUp, Position     = new Vector2(45 + (3 * 75), 675 - (3 * 75)) };

            Skills[ 0, 4] = new SkillData { SkillType = SkillType.ManaCostDown, Position     = new Vector2(45 + (0 * 75), 675 - (4 * 75)) };
            Skills[ 1, 4] = new SkillData { SkillType = SkillType.DeathDodge, Position       = new Vector2(45 + (1 * 75), 675 - (4 * 75)) };
            Skills[ 2, 4] = new SkillData { SkillType = SkillType.PricesDown, Position       = new Vector2(45 + (2 * 75), 675 - (4 * 75)) };
            Skills[ 3, 4] = new SkillData { SkillType = SkillType.RandomChildren, Position   = new Vector2(45 + (3 * 75), 675 - (4 * 75)) };

            Skills[ 0, 9] = new SkillData { SkillType = SkillType.KnightUnlock, Position     = new Vector2(45 + (0 * 75), 45  + (0 * 75)) };
            Skills[ 1, 9] = new SkillData { SkillType = SkillType.KnightUp, Position         = new Vector2(45 + (1 * 75), 45  + (0 * 75)) };
            Skills[ 2, 9] = new SkillData { SkillType = SkillType.NinjaUnlock, Position      = new Vector2(45 + (2 * 75), 45  + (0 * 75)) };
            Skills[ 3, 9] = new SkillData { SkillType = SkillType.NinjaUp, Position          = new Vector2(45 + (3 * 75), 45  + (0 * 75)) };

            Skills[ 0, 8] = new SkillData { SkillType = SkillType.MageUnlock, Position       = new Vector2(45 + (0 * 75), 45  + (1 * 75)) };
            Skills[ 1, 8] = new SkillData { SkillType = SkillType.MageUp, Position           = new Vector2(45 + (1 * 75), 45  + (1 * 75)) };
            Skills[ 2, 8] = new SkillData { SkillType = SkillType.BankerUnlock, Position     = new Vector2(45 + (2 * 75), 45  + (1 * 75)) };
            Skills[ 3, 8] = new SkillData { SkillType = SkillType.BankerUp, Position         = new Vector2(45 + (3 * 75), 45  + (1 * 75)) };

            Skills[ 0, 7] = new SkillData { SkillType = SkillType.BarbarianUnlock, Position  = new Vector2(45 + (0 * 75), 45  + (2 * 75)) };
            Skills[ 1, 7] = new SkillData { SkillType = SkillType.BarbarianUp, Position      = new Vector2(45 + (1 * 75), 45  + (2 * 75)) };
            Skills[ 2, 7] = new SkillData { SkillType = SkillType.LichUnlock, Position       = new Vector2(45 + (2 * 75), 45  + (2 * 75)) };
            Skills[ 3, 7] = new SkillData { SkillType = SkillType.LichUp, Position           = new Vector2(45 + (3 * 75), 45  + (2 * 75)) };
            Skills[ 4, 7] = new SkillData { SkillType = SkillType.SuperSecret, Position      = new Vector2(45 + (4 * 75), 45  + (2 * 75)) };

            Skills[ 0, 6] = new SkillData { SkillType = SkillType.AssassinUnlock, Position   = new Vector2(45 + (0 * 75), 45  + (3 * 75)) };
            Skills[ 1, 6] = new SkillData { SkillType = SkillType.AssassinUp, Position       = new Vector2(45 + (1 * 75), 45  + (3 * 75)) };
            Skills[ 2, 6] = new SkillData { SkillType = SkillType.SpellswordUnlock, Position = new Vector2(45 + (2 * 75), 45  + (3 * 75)) };
            Skills[ 3, 6] = new SkillData { SkillType = SkillType.SpellSwordUp, Position     = new Vector2(45 + (3 * 75), 45  + (3 * 75)) };
            Skills[ 4, 6] = new SkillData { SkillType = SkillType.Traitorous, Position       = new Vector2(45 + (4 * 75), 45  + (3 * 75)) };

            // Manor
            Skills[6, 0] = new SkillData
            {
                SkillType = SkillType.ManorLeftTree1,
                Position = new Vector2(905 - (6 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.LeftTree1,
                Connected = new[]
                {
                    ManorPiece.LeftTree2,
                }
            };
            Skills[7, 0] = new SkillData
            {
                SkillType = SkillType.ManorLeftTree2,
                Position = new Vector2(905 - (5 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.LeftTree2
            };
            Skills[8, 0] = new SkillData
            {
                SkillType = SkillType.ManorGroundRoad,
                Position = new Vector2(905 - (4 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.GroundRoad
            };
            Skills[10, 0] = new SkillData
            {
                SkillType = SkillType.ManorRightTree,
                Position = new Vector2(905 - (2 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.RightTree
            };

            Skills[5, 1] = new SkillData
            {
                SkillType = SkillType.ManorLeftFarBase,
                Position = new Vector2(905 - (7 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftFarBase,
                Connected = new []
                {
                    ManorPiece.LeftFarRoof,
                }
            };
            Skills[6, 1] = new SkillData
            {
                SkillType = SkillType.ManorLeftBigBase,
                Position = new Vector2(905 - (6 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftBigBase,
                Connected = new []
                {
                    ManorPiece.LeftBigUpper1,
                    ManorPiece.LeftFarBase,
                    ManorPiece.LeftTree1,
                }
            };
            Skills[7, 1] = new SkillData
            {
                SkillType = SkillType.ManorLeftWingBase,
                Position = new Vector2(905 - (5 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftWingBase,
                Connected = new []
                {
                    ManorPiece.LeftWingWindow,
                    ManorPiece.LeftWingRoof,
                    ManorPiece.LeftBigBase,
                }
            };
            Skills[8, 1] = new SkillData
            {
                SkillType = SkillType.ManorMainBase,
                Position = new Vector2(905 - (4 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.MainBase,
                Connected = new[]
                {
                    ManorPiece.LeftWingBase,
                    ManorPiece.MainWindowBottom,
                    ManorPiece.MainWindowTop,
                    ManorPiece.MainRoof,
                    ManorPiece.RightWingBase,
                    ManorPiece.GroundRoad,
                }
            };
            Skills[9, 1] = new SkillData
            {
                SkillType = SkillType.ManorRightWingBase,
                Position = new Vector2(905 - (3 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightWingBase,
                Connected = new []
                {
                    ManorPiece.RightWingWindow,
                    ManorPiece.RightWingRoof,
                    ManorPiece.RightBigBase,
                }
            };
            Skills[10, 1] = new SkillData
            {
                SkillType = SkillType.ManorRightBigBase,
                Position = new Vector2(905 - (2 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightBigBase,
                Connected = new []
                {
                    ManorPiece.RightTree,
                    ManorPiece.RightBigUpper,
                    ManorPiece.RightExtension,
                }
            };
            Skills[11, 1] = new SkillData
            {
                SkillType = SkillType.ManorRightExtension,
                Position = new Vector2(905 - (1 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightExtension
            };

            Skills[5, 2] = new SkillData
            {
                SkillType = SkillType.ManorLeftExtension,
                Position = new Vector2(905 - (7 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftExtension
            };
            Skills[6, 2] = new SkillData
            {
                SkillType = SkillType.ManorLeftBigUpper1,
                Position = new Vector2(905 - (6 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftBigUpper1,
                Connected = new []
                {
                    ManorPiece.LeftBigUpper2,
                }
            };
            Skills[7, 2] = new SkillData
            {
                SkillType = SkillType.ManorLeftWingWindow,
                Position = new Vector2(905 - (5 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftWingWindow
            };
            Skills[8, 2] = new SkillData
            {
                SkillType = SkillType.ManorMainWindowBottom,
                Position = new Vector2(905 - (4 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.MainWindowBottom
            };
            Skills[9, 2] = new SkillData
            {
                SkillType = SkillType.ManorRightWingWindow,
                Position = new Vector2(905 - (3 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightWingWindow
            };
            Skills[10, 2] = new SkillData
            {
                SkillType = SkillType.ManorRightBigUpper,
                Position = new Vector2(905 - (2 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightBigUpper,
                Connected = new []
                {
                    ManorPiece.RightBigRoof,
                    ManorPiece.RightHighBase,
                }
            };
            Skills[11, 2] = new SkillData
            {
                SkillType = SkillType.ManorRightHighBase,
                Position = new Vector2(905 - (1 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightHighBase,
                Connected = new []
                {
                    ManorPiece.RightHighUpper,
                }
            };

            Skills[5, 3] = new SkillData
            {
                SkillType = SkillType.ManorLeftFarRoof,
                Position = new Vector2(905 - (7 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftFarRoof,
                Connected = new []
                {
                    ManorPiece.LeftExtension,
                }
            };
            Skills[6, 3] = new SkillData
            {
                SkillType = SkillType.ManorLeftBigUpper2,
                Position = new Vector2(905 - (6 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftBigUpper2,
                Connected = new []
                {
                    ManorPiece.LeftBigRoof,
                    ManorPiece.LeftBigWindows,
                }
            };
            Skills[7, 3] = new SkillData
            {
                SkillType = SkillType.ManorLeftWingRoof,
                Position = new Vector2(905 - (5 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftWingRoof
            };
            Skills[8, 3] = new SkillData
            {
                SkillType = SkillType.ManorMainWindowTop,
                Position = new Vector2(905 - (4 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.MainWindowTop
            };
            Skills[9, 3] = new SkillData
            {
                SkillType = SkillType.ManorRightWingRoof,
                Position = new Vector2(905 - (3 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightWingRoof
            };
            Skills[10, 3] = new SkillData
            {
                SkillType = SkillType.ManorRightBigRoof,
                Position = new Vector2(905 - (2 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightBigRoof
            };
            Skills[11, 3] = new SkillData
            {
                SkillType = SkillType.ManorRightHighUpper,
                Position = new Vector2(905 - (1 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightHighUpper,
                Connected = new []
                {
                    ManorPiece.RightHighTower,
                    ManorPiece.ObservatoryBase,
                }
            };
            Skills[12, 3] = new SkillData
            {
                SkillType = SkillType.ManorObservatoryBase,
                Position = new Vector2(905 - (0 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.ObservatoryBase,
                Connected = new []
                {
                    ManorPiece.ObservatoryTelescope
                }
            };

            Skills[6, 4] = new SkillData
            {
                SkillType = SkillType.ManorLeftBigWindows,
                Position = new Vector2(905 - (6 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.LeftBigWindows
            };
            Skills[8, 4] = new SkillData
            {
                SkillType = SkillType.ManorMainRoof,
                Position = new Vector2(905 - (4 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.MainRoof
            };
            Skills[11, 4] = new SkillData
            {
                SkillType = SkillType.ManorRightHighTower,
                Position = new Vector2(905 - (1 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.RightHighTower
            };
            Skills[12, 4] = new SkillData
            {
                SkillType = SkillType.ManorObservatoryTelescope,
                Position = new Vector2(905 - (0 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.ObservatoryTelescope
            };

            Skills[6, 5] = new SkillData
            {
                SkillType = SkillType.ManorLeftBigRoof,
                Position = new Vector2(905 - (6 * 75), 675 - (5 * 75)),
                ManorPiece = ManorPiece.LeftBigRoof
            };

            SkillTypeArray = new SkillType[13, 10];
            _skillPositionArray = new Vector2[13, 10];
            _manorPieceArray = new int[13, 10];
            for (var i = 0; i < 13; i++)
            for (var j = 0; j < 10; j++)
            {
                if (Skills[i, j] == null)
                {
                    SkillTypeArray[i, j] = SkillType.Null;
                    _skillPositionArray[i, j] = new Vector2(-80, -80);
                    _manorPieceArray[i, j] = (int) ManorPiece.None;
                }
                else
                {
                    SkillTypeArray[i, j] = Skills[i, j].SkillType;
                    _skillPositionArray[i, j] = Skills[i, j].Position;
                    _manorPieceArray[i, j] = (int) Skills[i, j].ManorPiece;
                }
            }

            IconsVisible = true;
        }

        public static List<SkillObj> SkillArray { get; private set; }
        public static bool           IconsVisible    { get; private set; }

        public static void Initialize()
        {
            _blankTrait = new SkillObj("Icon_Sword_Sprite");
            if (SkillTypeArray.Length != _skillPositionArray.Length)
            {
                throw new Exception(
                    "Cannot create Trait System. The type array is not the same length as the position array.");
            }

            SkillArray = new List<SkillObj>();
            for (var i = (int) SkillType.ManorGroundRoad; i <= (int) SkillType.ManorObservatoryTelescope; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((SkillType) i);
                skillObj.Position = GetSkillPosition(skillObj);
                SkillArray.Add(skillObj);
            }

            for (var i = (int) SkillType.HealthUp; i <= (int) SkillType.Traitorous; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((SkillType) i);
                skillObj.Position = GetSkillPosition(skillObj);
                SkillArray.Add(skillObj);
            }

            for (var i = (int) SkillType.BlacksmithSword; i <= (int) SkillType.EnchantressCape; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((SkillType) i);
                SkillArray.Add(skillObj);
            }

            GetSkill(STARTING_TRAIT).Visible = true;
        }

        public static void LevelUpTrait(SkillObj trait, bool giveGoldBonus, bool level = true)
        {
            if (trait.CurrentLevel >= trait.MaxLevel)
            {
                return;
            }

            trait.CurrentLevel++;
            UpdateTraitSprite(trait);
            if (trait.Trait == SkillType.GoldFlatBonus && giveGoldBonus)
            {
                Game.PlayerStats.Gold += (int) trait.ModifierAmount;
            }

            if (level)
            {
                Game.PlayerStats.CurrentLevel++;
            }
        }

        public static void ResetAllTraits()
        {
            foreach (var current in SkillArray)
            {
                current.CurrentLevel = 0;
                current.Visible = false;
            }

            GetSkill(STARTING_TRAIT).Visible = true;
            Game.PlayerStats.CurrentLevel = 0;
        }

        public static void UpdateAllTraitSprites()
        {
            foreach (var current in SkillArray) UpdateTraitSprite(current);
        }

        public static void UpdateTraitSprite(SkillObj trait)
        {
            var text = trait.IconName;
            if (trait.CurrentLevel > 0 && trait.CurrentLevel >= trait.MaxMaxLevel)
            {
                text = text.Replace("Locked", "Max");
            }
            else if (trait.CurrentLevel > 0 && trait.CurrentLevel <= trait.MaxLevel)
            {
                text = text.Replace("Locked", "");
            }

            trait.ChangeSprite(text);
        }

        public static List<SkillObj> GetAllConnectingTraits(SkillObj trait)
        {
            var list = new List<SkillObj>();
            var index = GetTraitTypeIndex(trait);
            var data = Skills[(int) index.X, (int) index.Y];

            for (var i = 0; i < 13; i++)
            for (var j = 0; j < 10; j++)
            {
                if (Skills[i, j] == null)
                {
                    continue;
                }

                if (data.Connected.Contains(Skills[i, j].ManorPiece))
                {
                    list.Add(GetSkill(Skills[i, j].SkillType));
                }
            }

            return list;
        }

        public static SkillObj GetSkill(SkillType skillType)
        {
            foreach (var current in SkillArray.Where(current => current.Trait == skillType))
            {
                return current;
            }

            return _blankTrait;
        }

        public static SkillObj GetSkill(int indexX, int indexY)
        {
            return GetSkill(SkillTypeArray[indexX, indexY]);
        }

        public static bool IsSkillScreenSkill(SkillType skillType)
        {
            for (var i = 0; i < SkillTypeArray.GetLength(0); i++)
            for (var j = 0; j < SkillTypeArray.GetLength(1); j++)
            {
                if (SkillTypeArray[i, j] == skillType)
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector2 GetTraitTypeIndex(SkillObj trait)
        {
            var result = new Vector2(-1f, -1f);
            var traitType = trait.Trait;

            for (var i = 0; i < SkillTypeArray.GetLength(0); i++)
            for (var j = 0; j < SkillTypeArray.GetLength(1); j++)
            {
                if (SkillTypeArray[i, j] == traitType)
                {
                    result = new Vector2(i, j);
                }
            }

            return result;
        }

        public static Vector2 GetSkillPosition(SkillObj skill)
        {
            var traitTypeIndex = GetTraitTypeIndex(skill);
            if (traitTypeIndex.X == -1f || traitTypeIndex.Y == -1f)
            {
                return new Vector2(-80, -80);
            }

            return _skillPositionArray[(int) traitTypeIndex.X, (int) traitTypeIndex.Y];
        }

        public static int GetTypeArrayRows()
        {
            return SkillTypeArray.GetLength(0);
        }

        public static int GetTypeArrayColumns()
        {
            return SkillTypeArray.GetLength(1);
        }

        public static SkillObj[] GetSkillArray()
        {
            return SkillArray.ToArray();
        }

        public static int GetManorPiece(SkillObj trait)
        {
            var traitTypeIndex = GetTraitTypeIndex(trait);
            if (traitTypeIndex.X == -1f || traitTypeIndex.Y == -1f)
            {
                return -1;
            }

            return _manorPieceArray[(int) traitTypeIndex.X, (int) traitTypeIndex.Y];
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