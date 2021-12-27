// 
// RogueLegacyArchipelago - SkillSystem.cs
// Last Modified 2021-12-25
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueCastle.Structs;

namespace RogueCastle
{
    public class SkillSystem
    {
        private const int Trait_ObservatoryTelescope = 0;
        private const int Trait_ObservatoryBase = 1;
        private const int Trait_RightHighTower = 2;
        private const int Trait_RightHighUpper = 3;
        private const int Trait_RightHighBase = 4;
        private const int Trait_RightExtension = 5;
        private const int Trait_RightBigRoof = 6;
        private const int Trait_RightBigUpper = 7;
        private const int Trait_RightWingRoof = 8;
        private const int Trait_RightBigBase = 9;
        private const int Trait_RightWingBase = 10;
        private const int Trait_RightWingWindow = 11;
        private const int Trait_LeftExtensionBase = 12;
        private const int Trait_LeftFarRoof = 13;
        private const int Trait_LeftFarBase = 14;
        private const int Trait_LeftBigRoof = 15;
        private const int Trait_LeftBigUpper2 = 16;
        private const int Trait_LeftBigWindows = 17;
        private const int Trait_LeftBigUpper = 18;
        private const int Trait_LeftBigBase = 19;
        private const int Trait_LeftWingRoof = 20;
        private const int Trait_LeftWingBase = 21;
        private const int Trait_LeftWingWindow = 22;
        private const int Trait_GroundRoad = 23;
        private const int Trait_MainRoof = 24;
        private const int Trait_MainBase = 25;
        private const int Trait_FrontWindowTop = 26;
        private const int Trait_FrontWindowBottom = 27;
        private const int Trait_LeftTree1 = 28;
        private const int Trait_LeftTree2 = 29;
        private const int Trait_RightTree1 = 30;
        private const int TOTAL = 31;
        private static readonly SkillType StartingTrait = SkillType.Smithy;
        private static SkillObj m_blankTrait;
        private static readonly SkillType[,] m_skillTypeArray;
        private static readonly Vector2[,] m_skillPositionArray;
        private static readonly int[,] m_manorPieceArray;
        private static SkillLinker[,] m_skillLinkerArray;

        static SkillSystem()
        {
            // Note: this type is marked as 'beforefieldinit'.
            var array = new SkillType[10, 10];
            array[0, 8] = SkillType.RandomizeChildren;
            array[1, 7] = SkillType.SuperSecret;
            array[1, 8] = SkillType.ManaCostDown;
            array[2, 7] = SkillType.InvulnerabilityTimeUp;
            array[2, 8] = SkillType.SpellSwordUp;
            array[3, 2] = SkillType.DownStrikeUp;
            array[3, 8] = SkillType.SpellswordUnlock;
            array[4, 2] = SkillType.NinjaUp;
            array[4, 3] = SkillType.ArmorUp;
            array[4, 8] = SkillType.PotionUp;
            array[5, 0] = SkillType.LichUp;
            array[5, 1] = SkillType.LichUnlock;
            array[5, 2] = SkillType.PricesDown;
            array[5, 4] = SkillType.AttackUp;
            array[5, 6] = SkillType.MagicDamageUp;
            array[5, 8] = SkillType.AssassinUp;
            array[6, 0] = SkillType.DeathDodge;
            array[6, 2] = SkillType.NinjaUnlock;
            array[6, 3] = SkillType.BarbarianUp;
            array[6, 4] = SkillType.Architect;
            array[6, 5] = SkillType.EquipUp;
            array[6, 6] = SkillType.Enchanter;
            array[6, 7] = SkillType.MageUp;
            array[6, 8] = SkillType.BankerUnlock;
            array[6, 9] = SkillType.BankerUp;
            array[7, 2] = SkillType.CritChanceUp;
            array[7, 5] = SkillType.KnightUp;
            array[7, 8] = SkillType.GoldGainUp;
            array[8, 2] = SkillType.CritDamageUp;
            array[8, 5] = SkillType.HealthUp;
            array[9, 5] = SkillType.Smithy;
            array[9, 6] = SkillType.ManaUp;
            m_skillTypeArray = array;
            var array2 = new Vector2[10, 10];

            array2[0, 0] = new Vector2(0f, 0f);
            array2[0, 1] = new Vector2(0f, 0f);
            array2[0, 2] = new Vector2(0f, 0f);
            array2[0, 3] = new Vector2(0f, 0f);
            array2[0, 4] = new Vector2(0f, 0f);
            array2[0, 5] = new Vector2(0f, 0f);
            array2[0, 6] = new Vector2(0f, 0f);
            array2[0, 7] = new Vector2(0f, 0f);
            array2[0, 8] = new Vector2(338f, 680f);
            array2[0, 9] = new Vector2(0f, 0f);

            array2[1, 0] = new Vector2(0f, 0f);
            array2[1, 1] = new Vector2(0f, 0f);
            array2[1, 2] = new Vector2(0f, 0f);
            array2[1, 3] = new Vector2(0f, 0f);
            array2[1, 4] = new Vector2(0f, 0f);
            array2[1, 5] = new Vector2(0f, 0f);
            array2[1, 6] = new Vector2(0f, 0f);
            array2[1, 7] = new Vector2(558f, 590f);
            array2[1, 8] = new Vector2(38f, 380f);
            array2[1, 9] = new Vector2(0f, 0f);

            array2[2, 0] = new Vector2(0f, 0f);
            array2[2, 1] = new Vector2(0f, 0f);
            array2[2, 2] = new Vector2(0f, 0f);
            array2[2, 3] = new Vector2(0f, 0f);
            array2[2, 4] = new Vector2(0f, 0f);
            array2[2, 5] = new Vector2(0f, 0f);
            array2[2, 6] = new Vector2(0f, 0f);
            array2[2, 7] = new Vector2(38f, 310f);
            array2[2, 8] = new Vector2(828f, 520f);
            array2[2, 9] = new Vector2(0f, 0f);
            array2[3, 0] = new Vector2(0f, 0f);

            array2[3, 1] = new Vector2(0f, 0f);
            array2[3, 2] = new Vector2(38f, 100f);
            array2[3, 3] = new Vector2(0f, 0f);
            array2[3, 4] = new Vector2(0f, 0f);
            array2[3, 5] = new Vector2(0f, 0f);
            array2[3, 6] = new Vector2(0f, 0f);
            array2[3, 7] = new Vector2(0f, 0f);
            array2[3, 8] = new Vector2(828f, 590f);
            array2[3, 9] = new Vector2(0f, 0f);

            array2[4, 0] = new Vector2(0f, 0f);
            array2[4, 1] = new Vector2(0f, 0f);
            array2[4, 2] = new Vector2(648f, 520f);
            array2[4, 3] = new Vector2(868f, 130f);
            array2[4, 4] = new Vector2(0f, 0f);
            array2[4, 5] = new Vector2(0f, 0f);
            array2[4, 6] = new Vector2(0f, 0f);
            array2[4, 7] = new Vector2(0f, 0f);
            array2[4, 8] = new Vector2(38f, 240f);
            array2[4, 9] = new Vector2(0f, 0f);

            array2[5, 0] = new Vector2(918f, 520f);
            array2[5, 1] = new Vector2(918f, 590f);
            array2[5, 2] = new Vector2(38f, 520f);
            array2[5, 3] = new Vector2(0f, 0f);
            array2[5, 4] = new Vector2(238f, 310f);
            array2[5, 5] = new Vector2(0f, 0f);
            array2[5, 6] = new Vector2(238f, 400f);
            array2[5, 7] = new Vector2(0f, 0f);
            array2[5, 8] = new Vector2(918f, 680f);
            array2[5, 9] = new Vector2(0f, 0f);

            array2[6, 0] = new Vector2(38f, 450f);
            array2[6, 1] = new Vector2(0f, 0f);
            array2[6, 2] = new Vector2(648f, 590f);
            array2[6, 3] = new Vector2(828f, 680f);
            array2[6, 4] = new Vector2(178f, 680f);
            array2[6, 5] = new Vector2(868f, 220f);
            array2[6, 6] = new Vector2(108f, 680f);
            array2[6, 7] = new Vector2(738f, 680f);
            array2[6, 8] = new Vector2(738f, 590f);
            array2[6, 9] = new Vector2(738f, 520f);

            array2[7, 0] = new Vector2(0f, 0f);
            array2[7, 1] = new Vector2(0f, 0f);
            array2[7, 2] = new Vector2(868f, 330f);
            array2[7, 3] = new Vector2(0f, 0f);
            array2[7, 4] = new Vector2(0f, 0f);
            array2[7, 5] = new Vector2(648f, 680f);
            array2[7, 6] = new Vector2(0f, 0f);
            array2[7, 7] = new Vector2(0f, 0f);
            array2[7, 8] = new Vector2(38f, 170f);
            array2[7, 9] = new Vector2(0f, 0f);

            array2[8, 0] = new Vector2(0f, 0f);
            array2[8, 1] = new Vector2(0f, 0f);
            array2[8, 2] = new Vector2(868f, 400f);
            array2[8, 3] = new Vector2(0f, 0f);
            array2[8, 4] = new Vector2(0f, 0f);
            array2[8, 5] = new Vector2(238f, 130f);
            array2[8, 6] = new Vector2(0f, 0f);
            array2[8, 7] = new Vector2(0f, 0f);
            array2[8, 8] = new Vector2(0f, 0f);
            array2[8, 9] = new Vector2(0f, 0f);

            array2[9, 0] = new Vector2(0f, 0f);
            array2[9, 1] = new Vector2(0f, 0f);
            array2[9, 2] = new Vector2(0f, 0f);
            array2[9, 3] = new Vector2(0f, 0f);
            array2[9, 4] = new Vector2(0f, 0f);
            array2[9, 5] = new Vector2(38f, 680f);
            array2[9, 6] = new Vector2(238f, 220f);
            array2[9, 7] = new Vector2(0f, 0f);
            array2[9, 8] = new Vector2(0f, 0f);
            array2[9, 9] = new Vector2(0f, 0f);
            m_skillPositionArray = array2;
            m_manorPieceArray = new[,]
            {
                {
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    0,
                    -1
                },

                {
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    2,
                    1,
                    -1
                },

                {
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    2,
                    3,
                    -1
                },

                {
                    -1,
                    -1,
                    15,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    4,
                    -1
                },

                {
                    -1,
                    -1,
                    16,
                    17,
                    -1,
                    -1,
                    -1,
                    -1,
                    6,
                    -1
                },

                {
                    13,
                    14,
                    18,
                    -1,
                    22,
                    -1,
                    11,
                    -1,
                    7,
                    -1
                },

                {
                    12,
                    -1,
                    19,
                    20,
                    21,
                    25,
                    10,
                    9,
                    8,
                    5
                },

                {
                    -1,
                    -1,
                    29,
                    -1,
                    -1,
                    27,
                    -1,
                    -1,
                    31,
                    -1
                },

                {
                    -1,
                    -1,
                    30,
                    -1,
                    -1,
                    28,
                    -1,
                    -1,
                    -1,
                    -1
                },

                {
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    26,
                    24,
                    -1,
                    -1,
                    -1
                }
            };
            IconsVisible = true;
        }

        public static List<SkillObj> SkillArray { get; private set; }
        public static bool IconsVisible { get; private set; }

        public static void Initialize()
        {
            m_blankTrait = new SkillObj("Icon_Sword_Sprite");
            if (m_skillTypeArray.Length != m_skillPositionArray.Length)
            {
                throw new Exception(
                    "Cannot create Trait System. The type array is not the same length as the position array.");
            }
            SkillArray = new List<SkillObj>();
            for (var i = 2; i < 34; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((SkillType) i);
                skillObj.Position = GetSkillPosition(skillObj);
                SkillArray.Add(skillObj);
            }
            GetSkill(StartingTrait).Visible = true;
            m_skillLinkerArray = new SkillLinker[10, 10];
            for (var j = 0; j < 10; j++)
            {
                for (var k = 0; k < 10; k++)
                {
                    m_skillLinkerArray[j, k] = SkillBuilder.GetSkillLinker(j, k);
                }
            }
        }

        public static void LevelUpTrait(SkillObj trait, bool giveGoldBonus)
        {
            Game.PlayerStats.CurrentLevel++;
            trait.CurrentLevel++;
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
            foreach (var current in SkillArray)
            {
                UpdateTraitSprite(current);
            }
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
            trait.ChangeSprite(text);
        }

        public static List<SkillObj> GetAllConnectingTraits(SkillObj trait)
        {
            var typeArrayColumns = GetTypeArrayColumns();
            var typeArrayRows = GetTypeArrayRows();
            var traitTypeIndex = GetTraitTypeIndex(trait);
            var array = new SkillObj[4];
            if (traitTypeIndex.X + 1f < typeArrayColumns)
            {
                array[0] = GetSkill((int) traitTypeIndex.X + 1, (int) traitTypeIndex.Y);
            }
            if (traitTypeIndex.X - 1f >= 0f)
            {
                array[1] = GetSkill((int) traitTypeIndex.X - 1, (int) traitTypeIndex.Y);
            }
            if (traitTypeIndex.Y - 1f >= 0f)
            {
                array[2] = GetSkill((int) traitTypeIndex.X, (int) traitTypeIndex.Y - 1);
            }
            if (traitTypeIndex.Y + 1f < typeArrayRows)
            {
                array[3] = GetSkill((int) traitTypeIndex.X, (int) traitTypeIndex.Y + 1);
            }
            var list = new List<SkillObj>();
            var array2 = array;
            for (var i = 0; i < array2.Length; i++)
            {
                var skillObj = array2[i];
                if (skillObj != null)
                {
                    list.Add(skillObj);
                }
            }
            return list;
        }

        public static SkillObj GetSkill(SkillType skillType)
        {
            foreach (var current in SkillArray)
            {
                if (current.TraitType == skillType)
                {
                    return current;
                }
            }
            return m_blankTrait;
        }

        public static SkillObj GetSkill(int indexX, int indexY)
        {
            return GetSkill(m_skillTypeArray[indexY, indexX]);
        }

        public static Vector2 GetTraitTypeIndex(SkillObj trait)
        {
            var result = new Vector2(-1f, -1f);
            var traitType = trait.TraitType;
            for (var i = 0; i < m_skillTypeArray.GetLength(1); i++)
            {
                for (var j = 0; j < m_skillTypeArray.GetLength(0); j++)
                {
                    if (m_skillTypeArray[j, i] == traitType)
                    {
                        result = new Vector2(i, j);
                    }
                }
            }
            return result;
        }

        public static Vector2 GetSkillPosition(SkillObj skill)
        {
            var traitTypeIndex = GetTraitTypeIndex(skill);
            return m_skillPositionArray[(int) traitTypeIndex.Y, (int) traitTypeIndex.X];
        }

        public static int GetTypeArrayRows()
        {
            return m_skillTypeArray.GetLength(0);
        }

        public static int GetTypeArrayColumns()
        {
            return m_skillTypeArray.GetLength(1);
        }

        public static SkillObj[] GetSkillArray()
        {
            return SkillArray.ToArray();
        }

        public static int GetManorPiece(SkillObj trait)
        {
            var traitTypeIndex = GetTraitTypeIndex(trait);
            return m_manorPieceArray[(int) traitTypeIndex.Y, (int) traitTypeIndex.X];
        }

        public static SkillLinker GetSkillLink(int x, int y)
        {
            return m_skillLinkerArray[x, y];
        }

        public static void HideAllIcons()
        {
            foreach (var current in SkillArray)
            {
                current.Opacity = 0f;
            }
            IconsVisible = false;
        }

        public static void ShowAllIcons()
        {
            foreach (var current in SkillArray)
            {
                current.Opacity = 1f;
            }
            IconsVisible = true;
        }
    }
}
