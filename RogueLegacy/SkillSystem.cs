//  RogueLegacyRandomizer - SkillSystem.cs
//  Last Modified 2023-10-25 8:34 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

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
        public Vector2      Position   { get; init; } = new(-80, -80);
        public ManorPiece   ManorPiece { get; init; } = ManorPiece.None;
        public ManorPiece[] Connected  { get; init; } = Array.Empty<ManorPiece>();
    }

    public static class SkillSystem
    {
        private const SkillType STARTING_TRAIT = SkillType.ManorMainBase;

        private static SkillObj     _blankTrait;
        private static Vector2[,]   _skillPositionArray;
        private static int[,]       _manorPieceArray;
        private static SkillType[,] _skillTypeArray;
        private static SkillData[,] _skills;

        public static List<SkillObj> SkillArray   { get; private set; }
        public static bool           IconsVisible { get; private set; }

        static SkillSystem()
        {
            PreInitialize();
        }

        public static void PreInitialize()
        {
            _skills = new SkillData[13, 10];
            _skills[0, 0] = new() { SkillType = SkillType.Smithy, Position = new(45 + (0 * 75), 675 - (0 * 75)) };
            _skills[1, 0] = new() { SkillType = SkillType.Enchanter, Position = new(45 + (1 * 75), 675 - (0 * 75)) };
            _skills[3, 0] = new() { SkillType = SkillType.Architect, Position = new(45 + (3 * 75), 675 - (0 * 75)) };

            _skills[0, 1] = new() { SkillType = SkillType.HealthUp, Position = new(45 + (0 * 75), 675 - (1 * 75)) };
            _skills[1, 1] = new() { SkillType = SkillType.ManaUp, Position = new(45 + (1 * 75), 675 - (1 * 75)) };
            _skills[2, 1] = new() { SkillType = SkillType.AttackUp, Position = new(45 + (2 * 75), 675 - (1 * 75)) };
            _skills[3, 1] = new() { SkillType = SkillType.MagicDamageUp, Position = new(45 + (3 * 75), 675 - (1 * 75)) };

            _skills[0, 2] = new() { SkillType = SkillType.ArmorUp, Position = new(45 + (0 * 75), 675 - (2 * 75)) };
            _skills[1, 2] = new() { SkillType = SkillType.EquipUp, Position = new(45 + (1 * 75), 675 - (2 * 75)) };
            _skills[2, 2] = new() { SkillType = SkillType.CritChanceUp, Position = new(45 + (2 * 75), 675 - (2 * 75)) };
            _skills[3, 2] = new() { SkillType = SkillType.CritDamageUp, Position = new(45 + (3 * 75), 675 - (2 * 75)) };

            _skills[0, 3] = new() { SkillType = SkillType.DownStrikeUp, Position = new(45 + (0 * 75), 675 - (3 * 75)) };
            _skills[1, 3] = new() { SkillType = SkillType.GoldGainUp, Position = new(45 + (1 * 75), 675 - (3 * 75)) };
            _skills[2, 3] = new() { SkillType = SkillType.PotionUp, Position = new(45 + (2 * 75), 675 - (3 * 75)) };
            _skills[3, 3] = new() { SkillType = SkillType.InvulnTimeUp, Position = new(45 + (3 * 75), 675 - (3 * 75)) };

            _skills[0, 4] = new() { SkillType = SkillType.ManaCostDown, Position = new(45 + (0 * 75), 675 - (4 * 75)) };
            _skills[1, 4] = new() { SkillType = SkillType.DeathDodge, Position = new(45 + (1 * 75), 675 - (4 * 75)) };
            _skills[2, 4] = new() { SkillType = SkillType.PricesDown, Position = new(45 + (2 * 75), 675 - (4 * 75)) };
            _skills[3, 4] = new() { SkillType = SkillType.RandomChildren, Position = new(45 + (3 * 75), 675 - (4 * 75)) };

            _skills[0, 9] = new() { SkillType = SkillType.KnightUnlock, Position = new(45 + (0 * 75), 45 + (0 * 75)) };
            _skills[1, 9] = new() { SkillType = SkillType.KnightUp, Position = new(45 + (1 * 75), 45 + (0 * 75)) };
            _skills[2, 9] = new() { SkillType = SkillType.NinjaUnlock, Position = new(45 + (2 * 75), 45 + (0 * 75)) };
            _skills[3, 9] = new() { SkillType = SkillType.NinjaUp, Position = new(45 + (3 * 75), 45 + (0 * 75)) };

            _skills[0, 8] = new() { SkillType = SkillType.MageUnlock, Position = new(45 + (0 * 75), 45 + (1 * 75)) };
            _skills[1, 8] = new() { SkillType = SkillType.MageUp, Position = new(45 + (1 * 75), 45 + (1 * 75)) };
            _skills[2, 8] = new() { SkillType = SkillType.BankerUnlock, Position = new(45 + (2 * 75), 45 + (1 * 75)) };
            _skills[3, 8] = new() { SkillType = SkillType.BankerUp, Position = new(45 + (3 * 75), 45 + (1 * 75)) };

            _skills[0, 7] = new() { SkillType = SkillType.BarbarianUnlock, Position = new(45 + (0 * 75), 45 + (2 * 75)) };
            _skills[1, 7] = new() { SkillType = SkillType.BarbarianUp, Position = new(45 + (1 * 75), 45 + (2 * 75)) };
            _skills[2, 7] = new() { SkillType = SkillType.LichUnlock, Position = new(45 + (2 * 75), 45 + (2 * 75)) };
            _skills[3, 7] = new() { SkillType = SkillType.LichUp, Position = new(45 + (3 * 75), 45 + (2 * 75)) };
            _skills[4, 7] = new() { SkillType = SkillType.SuperSecret, Position = new(45 + (4 * 75), 45 + (2 * 75)) };

            _skills[0, 6] = new() { SkillType = SkillType.AssassinUnlock, Position = new(45 + (0 * 75), 45 + (3 * 75)) };
            _skills[1, 6] = new() { SkillType = SkillType.AssassinUp, Position = new(45 + (1 * 75), 45 + (3 * 75)) };
            _skills[2, 6] = new() { SkillType = SkillType.SpellswordUnlock, Position = new(45 + (2 * 75), 45 + (3 * 75)) };
            _skills[3, 6] = new() { SkillType = SkillType.SpellSwordUp, Position = new(45 + (3 * 75), 45 + (3 * 75)) };
            _skills[4, 6] = new() { SkillType = SkillType.Traitorous, Position = new(45 + (4 * 75), 45 + (3 * 75)) };

            // Manor
            _skills[6, 0] = new()
            {
                SkillType = SkillType.ManorLeftTree1,
                Position = new(905 - (6 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.LeftTree1,
                Connected = new[]
                {
                    ManorPiece.LeftTree2,
                },
            };
            _skills[7, 0] = new()
            {
                SkillType = SkillType.ManorLeftTree2,
                Position = new(905 - (5 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.LeftTree2,
            };
            _skills[8, 0] = new()
            {
                SkillType = SkillType.ManorGroundRoad,
                Position = new(905 - (4 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.GroundRoad,
            };
            _skills[10, 0] = new()
            {
                SkillType = SkillType.ManorRightTree,
                Position = new(905 - (2 * 75), 675 - (0 * 75)),
                ManorPiece = ManorPiece.RightTree,
            };

            _skills[5, 1] = new()
            {
                SkillType = SkillType.ManorLeftFarBase,
                Position = new(905 - (7 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftFarBase,
                Connected = new[]
                {
                    ManorPiece.LeftFarRoof,
                },
            };
            _skills[6, 1] = new()
            {
                SkillType = SkillType.ManorLeftBigBase,
                Position = new(905 - (6 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftBigBase,
                Connected = new[]
                {
                    ManorPiece.LeftBigUpper1,
                    ManorPiece.LeftFarBase,
                    ManorPiece.LeftTree1,
                },
            };
            _skills[7, 1] = new()
            {
                SkillType = SkillType.ManorLeftWingBase,
                Position = new(905 - (5 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.LeftWingBase,
                Connected = new[]
                {
                    ManorPiece.LeftWingWindow,
                    ManorPiece.LeftWingRoof,
                    ManorPiece.LeftBigBase,
                },
            };
            _skills[8, 1] = new()
            {
                SkillType = SkillType.ManorMainBase,
                Position = new(905 - (4 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.MainBase,
                Connected = new[]
                {
                    ManorPiece.LeftWingBase,
                    ManorPiece.MainWindowBottom,
                    ManorPiece.MainWindowTop,
                    ManorPiece.MainRoof,
                    ManorPiece.RightWingBase,
                    ManorPiece.GroundRoad,
                },
            };
            _skills[9, 1] = new()
            {
                SkillType = SkillType.ManorRightWingBase,
                Position = new(905 - (3 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightWingBase,
                Connected = new[]
                {
                    ManorPiece.RightWingWindow,
                    ManorPiece.RightWingRoof,
                    ManorPiece.RightBigBase,
                },
            };
            _skills[10, 1] = new()
            {
                SkillType = SkillType.ManorRightBigBase,
                Position = new(905 - (2 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightBigBase,
                Connected = new[]
                {
                    ManorPiece.RightTree,
                    ManorPiece.RightBigUpper,
                    ManorPiece.RightExtension,
                },
            };
            _skills[11, 1] = new()
            {
                SkillType = SkillType.ManorRightExtension,
                Position = new(905 - (1 * 75), 675 - (1 * 75)),
                ManorPiece = ManorPiece.RightExtension,
            };

            _skills[5, 2] = new()
            {
                SkillType = SkillType.ManorLeftExtension,
                Position = new(905 - (7 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftExtension,
            };
            _skills[6, 2] = new()
            {
                SkillType = SkillType.ManorLeftBigUpper1,
                Position = new(905 - (6 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftBigUpper1,
                Connected = new[]
                {
                    ManorPiece.LeftBigUpper2,
                },
            };
            _skills[7, 2] = new()
            {
                SkillType = SkillType.ManorLeftWingWindow,
                Position = new(905 - (5 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.LeftWingWindow,
            };
            _skills[8, 2] = new()
            {
                SkillType = SkillType.ManorMainWindowBottom,
                Position = new(905 - (4 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.MainWindowBottom,
            };
            _skills[9, 2] = new()
            {
                SkillType = SkillType.ManorRightWingWindow,
                Position = new(905 - (3 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightWingWindow,
            };
            _skills[10, 2] = new()
            {
                SkillType = SkillType.ManorRightBigUpper,
                Position = new(905 - (2 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightBigUpper,
                Connected = new[]
                {
                    ManorPiece.RightBigRoof,
                    ManorPiece.RightHighBase,
                },
            };
            _skills[11, 2] = new()
            {
                SkillType = SkillType.ManorRightHighBase,
                Position = new(905 - (1 * 75), 675 - (2 * 75)),
                ManorPiece = ManorPiece.RightHighBase,
                Connected = new[]
                {
                    ManorPiece.RightHighUpper,
                },
            };

            _skills[5, 3] = new()
            {
                SkillType = SkillType.ManorLeftFarRoof,
                Position = new(905 - (7 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftFarRoof,
                Connected = new[]
                {
                    ManorPiece.LeftExtension,
                },
            };
            _skills[6, 3] = new()
            {
                SkillType = SkillType.ManorLeftBigUpper2,
                Position = new(905 - (6 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftBigUpper2,
                Connected = new[]
                {
                    ManorPiece.LeftBigRoof,
                    ManorPiece.LeftBigWindows,
                },
            };
            _skills[7, 3] = new()
            {
                SkillType = SkillType.ManorLeftWingRoof,
                Position = new(905 - (5 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.LeftWingRoof,
            };
            _skills[8, 3] = new()
            {
                SkillType = SkillType.ManorMainWindowTop,
                Position = new(905 - (4 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.MainWindowTop,
            };
            _skills[9, 3] = new()
            {
                SkillType = SkillType.ManorRightWingRoof,
                Position = new(905 - (3 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightWingRoof,
            };
            _skills[10, 3] = new()
            {
                SkillType = SkillType.ManorRightBigRoof,
                Position = new(905 - (2 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightBigRoof,
            };
            _skills[11, 3] = new()
            {
                SkillType = SkillType.ManorRightHighUpper,
                Position = new(905 - (1 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.RightHighUpper,
                Connected = new[]
                {
                    ManorPiece.RightHighTower,
                    ManorPiece.ObservatoryBase,
                },
            };
            _skills[12, 3] = new()
            {
                SkillType = SkillType.ManorObservatoryBase,
                Position = new(905 - (0 * 75), 675 - (3 * 75)),
                ManorPiece = ManorPiece.ObservatoryBase,
                Connected = new[]
                {
                    ManorPiece.ObservatoryTelescope,
                },
            };

            _skills[6, 4] = new()
            {
                SkillType = SkillType.ManorLeftBigWindows,
                Position = new(905 - (6 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.LeftBigWindows,
            };
            _skills[8, 4] = new()
            {
                SkillType = SkillType.ManorMainRoof,
                Position = new(905 - (4 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.MainRoof,
            };
            _skills[11, 4] = new()
            {
                SkillType = SkillType.ManorRightHighTower,
                Position = new(905 - (1 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.RightHighTower,
            };
            _skills[12, 4] = new()
            {
                SkillType = SkillType.ManorObservatoryTelescope,
                Position = new(905 - (0 * 75), 675 - (4 * 75)),
                ManorPiece = ManorPiece.ObservatoryTelescope,
            };

            _skills[6, 5] = new()
            {
                SkillType = SkillType.ManorLeftBigRoof,
                Position = new(905 - (6 * 75), 675 - (5 * 75)),
                ManorPiece = ManorPiece.LeftBigRoof,
            };

            _skillTypeArray = new SkillType[13, 10];
            _skillPositionArray = new Vector2[13, 10];
            _manorPieceArray = new int[13, 10];
            for (var i = 0; i < 13; i++)
            for (var j = 0; j < 10; j++)
            {
                if (_skills[i, j] == null)
                {
                    _skillTypeArray[i, j] = SkillType.Null;
                    _skillPositionArray[i, j] = new(-80, -80);
                    _manorPieceArray[i, j] = (int) ManorPiece.None;
                }
                else
                {
                    _skillTypeArray[i, j] = _skills[i, j].SkillType;
                    _skillPositionArray[i, j] = _skills[i, j].Position;
                    _manorPieceArray[i, j] = (int) _skills[i, j].ManorPiece;
                }
            }

            IconsVisible = true;
        }

        public static void Initialize()
        {
            _blankTrait = new("Icon_Sword_Sprite");
            if (_skillTypeArray.Length != _skillPositionArray.Length)
            {
                throw new("Cannot create Trait System. The type array is not the same length as the position array.");
            }

            SkillArray = new();
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
            var data = _skills[(int) index.X, (int) index.Y];

            for (var i = 0; i < 13; i++)
            for (var j = 0; j < 10; j++)
            {
                if (_skills[i, j] == null)
                {
                    continue;
                }

                if (data.Connected.Contains(_skills[i, j].ManorPiece))
                {
                    list.Add(GetSkill(_skills[i, j].SkillType));
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
            return GetSkill(_skillTypeArray[indexX, indexY]);
        }

        public static bool IsSkillScreenSkill(SkillType skillType)
        {
            for (var i = 0; i < _skillTypeArray.GetLength(0); i++)
            for (var j = 0; j < _skillTypeArray.GetLength(1); j++)
            {
                if (_skillTypeArray[i, j] == skillType)
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

            for (var i = 0; i < _skillTypeArray.GetLength(0); i++)
            for (var j = 0; j < _skillTypeArray.GetLength(1); j++)
            {
                if (_skillTypeArray[i, j] == traitType)
                {
                    result = new(i, j);
                }
            }

            return result;
        }

        public static Vector2 GetSkillPosition(SkillObj skill)
        {
            var traitTypeIndex = GetTraitTypeIndex(skill);
            if (traitTypeIndex.X == -1f || traitTypeIndex.Y == -1f)
            {
                return new(-80, -80);
            }

            return _skillPositionArray[(int) traitTypeIndex.X, (int) traitTypeIndex.Y];
        }

        public static int GetTypeArrayRows()
        {
            return _skillTypeArray.GetLength(0);
        }

        public static int GetTypeArrayColumns()
        {
            return _skillTypeArray.GetLength(1);
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
