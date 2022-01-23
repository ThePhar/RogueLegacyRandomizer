//
//  Rogue Legacy Randomizer - SkillSystem.cs
//  Last Modified 2022-01-23
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
using RogueCastle.Enums;
using RogueCastle.Systems;

namespace RogueCastle
{
    public class SkillSystem
    {
        private const Skill STARTING_TRAIT = Skill.ManorMainBase;

        private static          SkillObj       _blankTrait;
        private static readonly Skill[,]       _skillTypeArray;
        private static readonly Vector2[,]     _skillPositionArray;
        private static readonly int[,]         _manorPieceArray;
        private static          SkillLinker[,] _skillLinkerArray;

        static SkillSystem()
        {
            var array = new Skill[10, 10];
            array[0, 8] = Skill.ManorObservatoryTelescope;
            array[1, 7] = Skill.SuperSecret;
            array[1, 8] = Skill.ManorObservatoryBase;
            array[2, 7] = Skill.ManorRightHighTower;
            array[2, 8] = Skill.ManorRightHighUpper;
            array[3, 2] = Skill.ManorLeftBigRoof;
            array[3, 8] = Skill.ManorRightHighBase;
            array[4, 2] = Skill.ManorLeftBigUpper2;
            array[4, 3] = Skill.ManorLeftBigWindows;
            array[4, 8] = Skill.ManorRightBigRoof;
            array[5, 0] = Skill.ManorLeftFarRoof;
            array[5, 1] = Skill.ManorLeftFarBase;
            array[5, 2] = Skill.ManorLeftBigUpper1;
            array[5, 4] = Skill.ManorLeftWingWindow;
            array[5, 6] = Skill.ManorRightWingWindow;
            array[5, 8] = Skill.ManorRightBigUpper;
            array[6, 0] = Skill.ManorLeftExtension;
            array[6, 2] = Skill.ManorLeftBigBase;
            array[6, 3] = Skill.ManorLeftWingRoof;
            array[6, 4] = Skill.ManorLeftWingBase;
            array[6, 5] = Skill.ManorMainRoof;
            array[6, 6] = Skill.ManorRightWingBase;
            array[6, 7] = Skill.ManorRightWingRoof;
            array[6, 8] = Skill.ManorRightBigBase;
            array[6, 9] = Skill.ManorRightExtension;
            array[7, 2] = Skill.ManorLeftTree1;
            array[7, 5] = Skill.ManorMainWindowTop;
            array[7, 8] = Skill.ManorRightTree;
            array[8, 2] = Skill.ManorLeftTree2;
            array[8, 5] = Skill.ManorMainWindowBottom;
            array[9, 5] = Skill.ManorMainBase;
            array[9, 6] = Skill.ManorGroundRoad;
            _skillTypeArray = array;

            var array2 = new Vector2[10, 10];
            array2[0, 0] = new Vector2(0f, 0f);
            array2[0, 1] = new Vector2(0f, 0f);
            array2[0, 2] = new Vector2(0f, 0f);
            array2[0, 3] = new Vector2(0f, 0f);
            array2[0, 4] = new Vector2(0f, 0f);
            array2[0, 5] = new Vector2(0f, 0f);
            array2[0, 6] = new Vector2(0f, 0f);
            array2[0, 7] = new Vector2(0f, 0f);
            array2[0, 8] = new Vector2(860f, 125f);
            array2[0, 9] = new Vector2(0f, 0f);
            array2[1, 0] = new Vector2(0f, 0f);
            array2[1, 1] = new Vector2(0f, 0f);
            array2[1, 2] = new Vector2(0f, 0f);
            array2[1, 3] = new Vector2(0f, 0f);
            array2[1, 4] = new Vector2(0f, 0f);
            array2[1, 5] = new Vector2(0f, 0f);
            array2[1, 6] = new Vector2(0f, 0f);
            array2[1, 7] = new Vector2(655f, -100f);
            array2[1, 8] = new Vector2(735f, 95f);
            array2[1, 9] = new Vector2(0f, 0f);
            array2[2, 0] = new Vector2(0f, 0f);
            array2[2, 1] = new Vector2(0f, 0f);
            array2[2, 2] = new Vector2(0f, 0f);
            array2[2, 3] = new Vector2(0f, 0f);
            array2[2, 4] = new Vector2(0f, 0f);
            array2[2, 5] = new Vector2(0f, 0f);
            array2[2, 6] = new Vector2(0f, 0f);
            array2[2, 7] = new Vector2(655f, 50f);
            array2[2, 8] = new Vector2(655f, 125f);
            array2[2, 9] = new Vector2(0f, 0f);
            array2[3, 0] = new Vector2(0f, 0f);
            array2[3, 1] = new Vector2(0f, 0f);
            array2[3, 2] = new Vector2(365f, 150f);
            array2[3, 3] = new Vector2(0f, 0f);
            array2[3, 4] = new Vector2(0f, 0f);
            array2[3, 5] = new Vector2(0f, 0f);
            array2[3, 6] = new Vector2(0f, 0f);
            array2[3, 7] = new Vector2(0f, 0f);
            array2[3, 8] = new Vector2(655f, 200f);
            array2[3, 9] = new Vector2(0f, 0f);
            array2[4, 0] = new Vector2(0f, 0f);
            array2[4, 1] = new Vector2(0f, 0f);
            array2[4, 2] = new Vector2(185f, 250f);
            array2[4, 3] = new Vector2(365f, 250f);
            array2[4, 4] = new Vector2(0f, 0f);
            array2[4, 5] = new Vector2(0f, 0f);
            array2[4, 6] = new Vector2(0f, 0f);
            array2[4, 7] = new Vector2(0f, 0f);
            array2[4, 8] = new Vector2(735f, 200f);
            array2[4, 9] = new Vector2(0f, 0f);
            array2[5, 0] = new Vector2(110f, 360f);
            array2[5, 1] = new Vector2(110f, 460f);
            array2[5, 2] = new Vector2(185f, 360f);
            array2[5, 3] = new Vector2(0f, 0f);
            array2[5, 4] = new Vector2(275f, 555f);
            array2[5, 5] = new Vector2(0f, 0f);
            array2[5, 6] = new Vector2(735f, 555f);
            array2[5, 7] = new Vector2(0f, 0f);
            array2[5, 8] = new Vector2(735f, 280f);
            array2[5, 9] = new Vector2(0f, 0f);
            array2[6, 0] = new Vector2(40f, 410f);
            array2[6, 1] = new Vector2(0f, 0f);
            array2[6, 2] = new Vector2(185f, 555f);
            array2[6, 3] = new Vector2(275f, 360f);
            array2[6, 4] = new Vector2(275f, 460f);
            array2[6, 5] = new Vector2(505f, 315f);
            array2[6, 6] = new Vector2(735f, 460f);
            array2[6, 7] = new Vector2(735f, 360f);
            array2[6, 8] = new Vector2(860f, 460f);
            array2[6, 9] = new Vector2(938f, 415f);
            array2[7, 0] = new Vector2(0f, 0f);
            array2[7, 1] = new Vector2(0f, 0f);
            array2[7, 2] = new Vector2(185f, 680f);
            array2[7, 3] = new Vector2(0f, 0f);
            array2[7, 4] = new Vector2(0f, 0f);
            array2[7, 5] = new Vector2(505f, 410f);
            array2[7, 6] = new Vector2(0f, 0f);
            array2[7, 7] = new Vector2(0f, 0f);
            array2[7, 8] = new Vector2(860f, 680f);
            array2[7, 9] = new Vector2(0f, 0f);
            array2[8, 0] = new Vector2(0f, 0f);
            array2[8, 1] = new Vector2(0f, 0f);
            array2[8, 2] = new Vector2(275f, 680f);
            array2[8, 3] = new Vector2(0f, 0f);
            array2[8, 4] = new Vector2(0f, 0f);
            array2[8, 5] = new Vector2(505f, 490f);
            array2[8, 6] = new Vector2(0f, 0f);
            array2[8, 7] = new Vector2(0f, 0f);
            array2[8, 8] = new Vector2(0f, 0f);
            array2[8, 9] = new Vector2(0f, 0f);
            array2[9, 0] = new Vector2(0f, 0f);
            array2[9, 1] = new Vector2(0f, 0f);
            array2[9, 2] = new Vector2(0f, 0f);
            array2[9, 3] = new Vector2(0f, 0f);
            array2[9, 4] = new Vector2(0f, 0f);
            array2[9, 5] = new Vector2(505f, 590f);
            array2[9, 6] = new Vector2(505f, 680f);
            array2[9, 7] = new Vector2(0f, 0f);
            array2[9, 8] = new Vector2(0f, 0f);
            array2[9, 9] = new Vector2(0f, 0f);

            _skillPositionArray = array2;
            _manorPieceArray = new[,]
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

        public static List<SkillObj> SkillManorArray { get; private set; }
        public static List<SkillObj> SkillStatArray  { get; private set; }
        public static bool           IconsVisible    { get; private set; }

        public static void Initialize()
        {
            _blankTrait = new SkillObj("Icon_Sword_Sprite");
            if (_skillTypeArray.Length != _skillPositionArray.Length)
            {
                throw new Exception(
                    "Cannot create Trait System. The type array is not the same length as the position array.");
            }

            SkillManorArray = new List<SkillObj>();
            for (var i = (int) Skill.ManorGroundRoad; i <= (int) Skill.ManorObservatoryTelescope; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((Skill) i);
                skillObj.Position = GetSkillPosition(skillObj);
                skillObj.ManorPiece = (ManorPiece) GetManorPiece(skillObj);
                SkillManorArray.Add(skillObj);
            }

            SkillStatArray = new List<SkillObj>();
            for (var i = (int) Skill.HealthUp; i <= (int) Skill.Traitorous; i++)
            {
                var skillObj = SkillBuilder.BuildSkill((Skill) i);
                SkillStatArray.Add(skillObj);
            }

            GetSkill(STARTING_TRAIT).Visible = true;
            _skillLinkerArray = new SkillLinker[10, 10];
            for (var j = 0; j < 10; j++)
            for (var k = 0; k < 10; k++)
            {
                _skillLinkerArray[j, k] = SkillBuilder.GetSkillLinker(j, k);
            }
        }

        public static void LevelUpTrait(SkillObj trait, bool giveGoldBonus, bool level = true)
        {
            trait.CurrentLevel++;
            UpdateTraitSprite(trait);
            if (trait.Trait == Skill.GoldFlatBonus && giveGoldBonus)
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
            foreach (var current in SkillManorArray)
            {
                current.CurrentLevel = 0;
                current.Visible = false;
            }

            foreach (var current in SkillStatArray) current.CurrentLevel = 0;

            GetSkill(STARTING_TRAIT).Visible = true;
            Game.PlayerStats.CurrentLevel = 0;
        }

        public static void UpdateAllTraitSprites()
        {
            foreach (var current in SkillManorArray) UpdateTraitSprite(current);
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

        public static SkillObj GetSkill(Skill skill)
        {
            foreach (var current in SkillManorArray.Where(current => current.Trait == skill))
            {
                return current;
            }

            foreach (var current in SkillStatArray.Where(current => current.Trait == skill))
            {
                return current;
            }

            return _blankTrait;
        }

        public static SkillObj GetSkill(int indexX, int indexY)
        {
            return GetSkill(_skillTypeArray[indexY, indexX]);
        }

        public static Vector2 GetTraitTypeIndex(SkillObj trait)
        {
            var result = new Vector2(-1f, -1f);
            var traitType = trait.Trait;

            for (var i = 0; i < _skillTypeArray.GetLength(1); i++)
            for (var j = 0; j < _skillTypeArray.GetLength(0); j++)
            {
                if (_skillTypeArray[j, i] == traitType)
                {
                    result = new Vector2(i, j);
                }
            }

            return result;
        }

        public static Vector2 GetSkillPosition(SkillObj skill)
        {
            var traitTypeIndex = GetTraitTypeIndex(skill);
            return _skillPositionArray[(int) traitTypeIndex.Y, (int) traitTypeIndex.X];
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
            return SkillManorArray.ToArray();
        }

        public static SkillObj[] GetStatArray()
        {
            return SkillStatArray.ToArray();
        }

        public static int GetManorPiece(SkillObj trait)
        {
            var traitTypeIndex = GetTraitTypeIndex(trait);
            return _manorPieceArray[(int) traitTypeIndex.Y, (int) traitTypeIndex.X];
        }

        public static SkillLinker GetSkillLink(int x, int y)
        {
            return _skillLinkerArray[x, y];
        }

        public static void HideAllIcons()
        {
            foreach (var current in SkillManorArray) current.Opacity = 0f;
            IconsVisible = false;
        }

        public static void ShowAllIcons()
        {
            foreach (var current in SkillManorArray) current.Opacity = 1f;
            IconsVisible = true;
        }
    }
}
