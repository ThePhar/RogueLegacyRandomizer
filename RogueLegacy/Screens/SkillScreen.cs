// Rogue Legacy Randomizer - SkillScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using RogueLegacy.Systems;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class SkillScreen : Screen
    {
        private readonly int              _shakeAmount = 2;
        private readonly float            _shakeDelay  = 0.01f;
        private          SpriteObj        _bg;
        private          bool             _cameraTweening;
        private          SpriteObj        _cloud1;
        private          SpriteObj        _cloud2;
        private          SpriteObj        _cloud3;
        private          SpriteObj        _cloud4;
        private          SpriteObj        _cloud5;
        private          SpriteObj        _coinIcon;
        private          KeyIconTextObj   _confirmText;
        private          KeyIconTextObj   _continueText;
        private          SpriteObj        _descriptionDivider;
        private          ObjContainer     _dialoguePlate;
        private          bool             _fadingIn;
        private          bool             _horizontalShake;
        private          ImpactEffectPool _impactEffectPool;
        private          KeyIconTextObj   _inputDescription;
        private          bool             _lockControls;
        private          ObjContainer     _manor;
        private          KeyIconTextObj   _navigationText;
        private          TextObj          _playerMoney;
        private          float            _screenShakeMagnitude;
        private          Vector2          _selectedTraitIndex;
        private          SpriteObj        _selectionIcon;
        private          float            _shakeDuration;
        private          GameObj          _shakeObj;
        private          bool             _shakeScreen;
        private          float            _shakeTimer;
        private          bool             _shookLeft;
        private          TextObj          _skillCost;
        private          SpriteObj        _skillCostBG;
        private          TextObj          _skillCurrent;
        private          TextObj          _skillDescription;
        private          SpriteObj        _skillIcon;
        private          TextObj          _skillLevel;
        private          TextObj          _skillTitle;
        private          TextObj          _skillUpgrade;
        private          SpriteObj        _titleText;
        private          KeyIconTextObj   _toggleIconsText;
        private          bool             _verticalShake;

        public SkillScreen()
        {
            _selectedTraitIndex = new Vector2(5f, 9f);
            _impactEffectPool = new ImpactEffectPool(1000);
            DrawIfCovered = true;
        }

        public override void LoadContent()
        {
            _impactEffectPool.Initialize();
            _manor = new ObjContainer("TraitsCastle_Character");
            _manor.Scale = new Vector2(2f, 2f);
            _manor.ForceDraw = true;
            for (var i = 0; i < _manor.NumChildren; i++)
            {
                _manor.GetChildAt(i).Visible = false;
                _manor.GetChildAt(i).Opacity = 0f;
            }

            _dialoguePlate = new ObjContainer("TraitsScreenPlate_Container");
            _dialoguePlate.ForceDraw = true;
            _dialoguePlate.Position = new Vector2(1320 - _dialoguePlate.Width / 2, 360f);
            _skillIcon = new SpriteObj("Icon_Health_Up_Sprite");
            _skillIcon.Position = new Vector2(-110f, -200f);
            _dialoguePlate.AddChild(_skillIcon);
            _skillTitle = new TextObj(Game.JunicodeFont);
            _skillTitle.Text = "Skill name";
            _skillTitle.DropShadow = new Vector2(2f, 2f);
            _skillTitle.TextureColor = new Color(236, 197, 132);
            _skillTitle.Position = new Vector2(_skillIcon.Bounds.Right + 15, _skillIcon.Y);
            _skillTitle.FontSize = 12f;
            _dialoguePlate.AddChild(_skillTitle);
            _skillDescription = new TextObj(Game.JunicodeFont);
            _skillDescription.Text = "Description text goes here.  Let's see how well the word wrap function works.";
            _skillDescription.Position = new Vector2(_dialoguePlate.GetChildAt(1).X - 30f,
                _dialoguePlate.GetChildAt(1).Bounds.Bottom + 20);
            _skillDescription.FontSize = 10f;
            _skillDescription.DropShadow = new Vector2(2f, 2f);
            _skillDescription.TextureColor = new Color(228, 218, 208);
            _skillDescription.WordWrap(_dialoguePlate.Width - 50);
            _dialoguePlate.AddChild(_skillDescription);
            _inputDescription = new KeyIconTextObj(Game.JunicodeFont);
            _inputDescription.Text = "Input descriptions go here..";
            _inputDescription.Position = new Vector2(_skillIcon.X - 30f, _skillDescription.Bounds.Bottom + 20);
            _inputDescription.FontSize = 10f;
            _inputDescription.DropShadow = new Vector2(2f, 2f);
            _inputDescription.TextureColor = new Color(228, 218, 208);
            _inputDescription.WordWrap(_dialoguePlate.Width - 50);
            _dialoguePlate.AddChild(_inputDescription);
            _descriptionDivider = new SpriteObj("Blank_Sprite");
            _descriptionDivider.ScaleX = 250f / _descriptionDivider.Width;
            _descriptionDivider.ScaleY = 0.25f;
            _descriptionDivider.ForceDraw = true;
            _descriptionDivider.DropShadow = new Vector2(2f, 2f);
            _skillCurrent = new TextObj(Game.JunicodeFont);
            _skillCurrent.Position = new Vector2(_inputDescription.X, _inputDescription.Bounds.Bottom + 10);
            _skillCurrent.FontSize = 10f;
            _skillCurrent.DropShadow = new Vector2(2f, 2f);
            _skillCurrent.TextureColor = new Color(228, 218, 208);
            _skillCurrent.WordWrap(_dialoguePlate.Width - 50);
            _dialoguePlate.AddChild(_skillCurrent);
            _skillUpgrade = _skillCurrent.Clone() as TextObj;
            _skillUpgrade.Y += 15f;
            _dialoguePlate.AddChild(_skillUpgrade);
            _skillLevel = _skillUpgrade.Clone() as TextObj;
            _skillLevel.Y += 15f;
            _dialoguePlate.AddChild(_skillLevel);
            _skillCost = new TextObj(Game.JunicodeFont);
            _skillCost.X = _skillIcon.X;
            _skillCost.Y = 182f;
            _skillCost.FontSize = 10f;
            _skillCost.DropShadow = new Vector2(2f, 2f);
            _skillCost.TextureColor = Color.Yellow;
            _dialoguePlate.AddChild(_skillCost);
            _skillCostBG = new SpriteObj("SkillTreeGoldIcon_Sprite");
            _skillCostBG.Position = new Vector2(-180f, 180f);
            _dialoguePlate.AddChild(_skillCostBG);
            _dialoguePlate.ForceDraw = true;
            _bg = new SpriteObj("TraitsBG_Sprite");
            _bg.Scale = new Vector2(1320f / _bg.Width, 1320f / _bg.Width);
            _bg.ForceDraw = true;
            _cloud1 = new SpriteObj("TraitsCloud1_Sprite")
            {
                ForceDraw = true
            };
            _cloud2 = new SpriteObj("TraitsCloud2_Sprite")
            {
                ForceDraw = true
            };
            _cloud3 = new SpriteObj("TraitsCloud3_Sprite")
            {
                ForceDraw = true
            };
            _cloud4 = new SpriteObj("TraitsCloud4_Sprite")
            {
                ForceDraw = true
            };
            _cloud5 = new SpriteObj("TraitsCloud5_Sprite")
            {
                ForceDraw = true
            };
            var opacity = 1f;
            _cloud1.Opacity = opacity;
            _cloud2.Opacity = opacity;
            _cloud3.Opacity = opacity;
            _cloud4.Opacity = opacity;
            _cloud5.Opacity = opacity;
            _cloud1.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            _cloud2.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            _cloud3.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            _cloud4.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            _cloud5.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            _selectionIcon = new SpriteObj("IconHalo_Sprite");
            _selectionIcon.ForceDraw = true;
            _selectionIcon.AnimationDelay = 0.1f;
            _selectionIcon.PlayAnimation();
            _selectionIcon.Scale = new Vector2(1.1f, 1.1f);
            _titleText = new SpriteObj("ManorTitleText_Sprite");
            _titleText.X = _titleText.Width / 2f + 20f;
            _titleText.Y = 64.8f;
            _titleText.ForceDraw = true;
            _continueText = new KeyIconTextObj(Game.JunicodeFont);
            _continueText.ForceDraw = true;
            _continueText.FontSize = 12f;
            _continueText.DropShadow = new Vector2(2f, 2f);
            _continueText.Position = new Vector2(1300f, 630f);
            _continueText.Align = Types.TextAlign.Right;
            _toggleIconsText = new KeyIconTextObj(Game.JunicodeFont);
            _toggleIconsText.ForceDraw = true;
            _toggleIconsText.FontSize = 12f;
            _toggleIconsText.DropShadow = new Vector2(2f, 2f);
            _toggleIconsText.Position = new Vector2(_continueText.X, _continueText.Y + 40f);
            _toggleIconsText.Align = Types.TextAlign.Right;
            _confirmText = new KeyIconTextObj(Game.JunicodeFont);
            _confirmText.Align = Types.TextAlign.Right;
            _confirmText.FontSize = 12f;
            _confirmText.DropShadow = new Vector2(2f, 2f);
            _confirmText.Position = new Vector2(1300f, 10f);
            _confirmText.ForceDraw = true;
            _navigationText = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Align = Types.TextAlign.Right;
            _navigationText.FontSize = 12f;
            _navigationText.DropShadow = new Vector2(2f, 2f);
            _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40f);
            _navigationText.ForceDraw = true;
            _coinIcon = new SpriteObj("CoinIcon_Sprite");
            _coinIcon.Position = new Vector2(1100f, 585f);
            _coinIcon.Scale = new Vector2(0.9f, 0.9f);
            _coinIcon.ForceDraw = true;
            _playerMoney = new TextObj(Game.GoldFont);
            _playerMoney.Align = Types.TextAlign.Left;
            _playerMoney.Text = "1000";
            _playerMoney.FontSize = 30f;
            _playerMoney.Position = new Vector2(_coinIcon.X + 35f, _coinIcon.Y);
            _playerMoney.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            _lockControls = false;
            _manor.GetChildAt(23).Visible = true;
            _manor.GetChildAt(23).Opacity = 1f;
            Camera.Position = new Vector2(660f, 360f);
            var skillArray = SkillSystem.GetSkillArray();
            for (var i = 0; i < skillArray.Length; i++)
            {
                if (skillArray[i].CurrentLevel > 0)
                {
                    SetVisible(skillArray[i], false);
                }

                if (skillArray[i].Trait >= SkillType.ManorGroundRoad)
                {
                    var networkItem = Program.Game.ArchipelagoManager.LocationCache[ManorContainer.ArchipelagoLocationTable[skillArray[i].ManorPiece]];
                    skillArray[i].Description = string.Format(
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nThis upgrade contains {0} for {1}.",
                        Program.Game.ArchipelagoManager.GetItemName(networkItem.Item),
                        Program.Game.ArchipelagoManager.GetPlayerName(networkItem.Player));

                    skillArray[i].Description =
                        skillArray[i].Description.Replace("GENDER", Game.PlayerStats.IsFemale ? "motherless" : "fatherless");

                    skillArray[i].Description = AddSpacesToString(Enum.GetName(typeof(ManorPiece), SkillSystem.GetManorPiece(skillArray[i]))) +
                                                "\n\n" + skillArray[i].Description;
                }
            }

            if (!SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("SkillTreeSong", true, 1f);
            }

            var skill = SkillSystem.GetSkill((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y);
            _selectionIcon.Position = SkillSystem.GetSkillPosition(skill);
            UpdateDescriptionPlate(skill);
            _dialoguePlate.Visible = true;
            _confirmText.Text = "[Input:" + 0 + "] to purchase/upgrade skill";
            _toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
            _continueText.Text = "[Input:" + 2 + "] to exit the manor";
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _navigationText.Text = "[Button:LeftStick] to navigate skills";
            }
            else
            {
                _navigationText.Text = "Arrow keys to navigate skills";
            }

            SkillSystem.UpdateAllTraitSprites();
            base.OnEnter();
        }

        public override void OnExit()
        {
            Game.ScreenManager.Player.AttachedLevel.UpdatePlayerSpellIcon();
            SoundManager.StopMusic(0.5f);
            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.UpgradeData, SaveType.PlayerData);
            base.OnExit();
        }

        public void SetVisible(SkillObj trait, bool fadeIn)
        {
            var manorPiece = SkillSystem.GetManorPiece(trait);
            if (fadeIn)
            {
                var location = ManorContainer.ArchipelagoLocationTable[(ManorPiece) manorPiece];
                SetManorPieceVisible(new Tuple<int, int>(manorPiece, location), trait);
                Program.Game.ArchipelagoManager.CheckLocations(ManorContainer.ArchipelagoLocationTable[(ManorPiece) manorPiece]);
                return;
            }

            var childAt = _manor.GetChildAt(manorPiece);
            childAt.Opacity = 1f;
            childAt.Visible = true;
            foreach (var current in SkillSystem.GetAllConnectingTraits(trait))
            {
                if (!current.Visible)
                {
                    current.Visible = true;
                    current.Opacity = 1f;
                }
            }

            if (_manor.GetChildAt(7).Visible && _manor.GetChildAt(16).Visible)
            {
                (_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
            }

            if (_manor.GetChildAt(6).Visible && _manor.GetChildAt(16).Visible)
            {
                (_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
            }

            if (_manor.GetChildAt(2).Visible)
            {
                var spriteObj = _manor.GetChildAt(32) as SpriteObj;
                spriteObj.Visible = true;
                spriteObj.Opacity = 1f;
                spriteObj.PlayAnimation();
                spriteObj.OverrideParentAnimationDelay = true;
                spriteObj.AnimationDelay = 0.0333333351f;
                spriteObj.Visible = true;
            }
        }

        public void SetManorPieceVisible(Tuple<int, int> manorPiece, SkillObj skillObj)
        {
            var childAt = _manor.GetChildAt(manorPiece.Item1);
            var num = 0f;
            if (!childAt.Visible)
            {
                _lockControls = true;
                childAt.Visible = true;
                var pos = new Vector2(childAt.AbsPosition.X, childAt.AbsBounds.Bottom);
                switch (manorPiece.Item1)
                {
                    case 0:
                    case 11:
                    case 17:
                    case 22:
                    case 24:
                    case 27:
                    case 28:
                        num = 0.5f;
                        childAt.Opacity = 0f;
                        Tween.To(childAt, num, Tween.EaseNone, "Opacity", "1");
                        goto IL_A26;

                    case 1:
                    case 5:
                        childAt.Opacity = 1f;
                        num = 1f;
                        childAt.X -= childAt.Width * 2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "X", (childAt.Width * 2).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, false, childAt.Height * 2, num);
                        goto IL_A26;

                    case 2:
                    {
                        childAt.Opacity = 1f;
                        num = 1.5f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2, num);
                        var spriteObj = _manor.GetChildAt(32) as SpriteObj;
                        spriteObj.PlayAnimation();
                        spriteObj.OverrideParentAnimationDelay = true;
                        spriteObj.AnimationDelay = 0.0333333351f;
                        spriteObj.Visible = true;
                        spriteObj.Opacity = 0f;
                        Tween.To(spriteObj, 0.5f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "1");
                        goto IL_A26;
                    }

                    case 3:
                    case 6:
                    case 9:
                    case 13:
                    case 15:
                    case 20:
                    case 25:
                        childAt.Opacity = 1f;
                        num = 1f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2, num);
                        goto IL_A26;

                    case 4:
                        pos.Y -= 50f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2 * 0.25f, num);
                        goto IL_A26;

                    case 7:
                        pos.X = childAt.AbsBounds.Right - childAt.Width * 2 * 0.25f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2 * 0.25f, num);
                        goto IL_A26;

                    case 8:
                        pos.X = childAt.AbsBounds.Right - childAt.Width * 2 * 0.25f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2 * 0.25f, num);
                        goto IL_A26;

                    case 10:
                    case 21:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2, num);
                        goto IL_A26;

                    case 12:
                    case 14:
                        childAt.Opacity = 1f;
                        num = 1f;
                        childAt.X += childAt.Width * 2;
                        pos.X = childAt.AbsPosition.X - 60f;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "X", (-(childAt.Width * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, false, childAt.Height * 2, num);
                        goto IL_A26;

                    case 16:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2 * 0.5f, num);
                        goto IL_A26;

                    case 18:
                    case 19:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height * 2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height * 2)).ToString());
                        _impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width * 2 * 0.2f, num);
                        goto IL_A26;

                    case 23:
                        goto IL_A26;

                    case 29:
                    case 30:
                    case 31:
                        Tween.RunFunction(0.25f, typeof(SoundManager), "PlaySound", "skill_tree_reveal_bounce");
                        childAt.Opacity = 1f;
                        childAt.Scale = Vector2.Zero;
                        num = 1f;
                        Tween.To(childAt, num, Bounce.EaseOut, "ScaleX", "1", "ScaleY", "1");
                        goto IL_A26;
                }

                num = 0.7f;
                var vector = new Vector2(childAt.AbsPosition.X, childAt.AbsBounds.Bottom);
                childAt.Opacity = 1f;
                childAt.Y -= 720f;
                Tween.By(childAt, num, Quad.EaseIn, "Y", "720");
                Tween.AddEndHandlerToLastTween(_impactEffectPool, "SkillTreeDustEffect", vector, true,
                    childAt.Width * 2);
                Tween.RunFunction(num, this, "ShakeScreen", 5, true, true);
                Tween.RunFunction(num + 0.2f, this, "StopScreenShake");
            }

            IL_A26:
            Tween.RunFunction(num, this, "SetSkillIconVisible", skillObj, manorPiece);
            if (_manor.GetChildAt(7).Visible && _manor.GetChildAt(16).Visible)
            {
                (_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
            }

            if (_manor.GetChildAt(6).Visible && _manor.GetChildAt(16).Visible)
            {
                (_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
            }
        }

        public void SetSkillIconVisible(SkillObj skill, Tuple<int, int> manorPiece)
        {
            var num = 0f;
            foreach (var current in SkillSystem.GetAllConnectingTraits(skill))
            {
                if (!current.Visible)
                {
                    current.Visible = true;
                    current.Opacity = 0f;
                    Tween.To(current, 0.2f, Linear.EaseNone, "Opacity", "1");
                    num += 0.2f;
                }
            }

            Tween.RunFunction(num, this, "UnlockControls");
            Tween.RunFunction(num, this, "CheckForSkillUnlock", skill, true, manorPiece);
        }

        public void CheckForSkillUnlock(SkillObj skill, bool displayScreen, Tuple<int, int> manorPiece = null)
        {
            byte b = 0;
            switch (skill.Trait)
            {
                case SkillType.Smithy:
                    b = 1;
                    break;

                case SkillType.Enchanter:
                    b = 2;
                    break;

                case SkillType.Architect:
                    b = 3;
                    break;

                case SkillType.LichUnlock:
                    b = 7;
                    break;

                case SkillType.BankerUnlock:
                    b = 5;
                    break;

                case SkillType.SpellswordUnlock:
                    b = 6;
                    break;

                case SkillType.NinjaUnlock:
                    b = 4;
                    break;

                case SkillType.KnightUp:
                    b = 8;
                    if (Game.PlayerStats.Class == 0)
                    {
                        Game.PlayerStats.Class = 8;
                    }

                    break;

                case SkillType.MageUp:
                    b = 9;
                    if (Game.PlayerStats.Class == 1)
                    {
                        Game.PlayerStats.Class = 9;
                    }

                    break;

                case SkillType.AssassinUp:
                    b = 12;
                    if (Game.PlayerStats.Class == 3)
                    {
                        Game.PlayerStats.Class = 11;
                    }

                    break;

                case SkillType.BankerUp:
                    b = 13;
                    if (Game.PlayerStats.Class == 5)
                    {
                        Game.PlayerStats.Class = 13;
                    }

                    break;

                case SkillType.BarbarianUp:
                    b = 10;
                    if (Game.PlayerStats.Class == 2)
                    {
                        Game.PlayerStats.Class = 10;
                    }

                    break;

                case SkillType.LichUp:
                    b = 15;
                    if (Game.PlayerStats.Class == 7)
                    {
                        Game.PlayerStats.Class = 15;
                    }

                    break;

                case SkillType.NinjaUp:
                    b = 11;
                    if (Game.PlayerStats.Class == 4)
                    {
                        Game.PlayerStats.Class = 12;
                    }

                    break;

                case SkillType.SpellSwordUp:
                    b = 14;
                    if (Game.PlayerStats.Class == 6)
                    {
                        Game.PlayerStats.Class = 14;
                    }

                    break;

                case SkillType.SuperSecret:
                    b = 16;
                    break;

                default:
                    b = (byte) SkillUnlock.NetworkItem;
                    break;
            }

            if (b == (byte) SkillUnlock.NetworkItem && displayScreen)
            {
                var list = new List<object>
                {
                    SkillUnlock.NetworkItem,
                    manorPiece.Item2
                };

                (ScreenManager as RCScreenManager).DisplayScreen((int) ScreenType.SkillUnlock, true, list);
            }
            else if (b != 0 && displayScreen)
            {
                var list = new List<object> { b };
                (ScreenManager as RCScreenManager).DisplayScreen((int) ScreenType.SkillUnlock, true, list);
            }
        }

        public void UnlockControls()
        {
            _lockControls = false;
        }

        public void StartShake(GameObj obj, float shakeDuration)
        {
            _shakeDuration = shakeDuration;
            _shakeObj = obj;
            _shakeTimer = _shakeDelay;
            _shookLeft = false;
        }

        public void EndShake()
        {
            if (_shookLeft)
            {
                _shakeObj.X += _shakeAmount;
            }

            _shakeObj = null;
            _shakeTimer = 0f;
        }

        public void FadingComplete()
        {
            _fadingIn = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_cameraTweening && Camera.Y != 360f)
            {
                _cameraTweening = true;
                Tween.To(Camera, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
                Tween.AddEndHandlerToLastTween(this, "EndCameraTween");
            }

            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_cloud1.Bounds.Right < -100)
            {
                _cloud1.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }

            if (_cloud2.Bounds.Right < -100)
            {
                _cloud2.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }

            if (_cloud3.Bounds.Right < -100)
            {
                _cloud3.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }

            if (_cloud4.Bounds.Right < -100)
            {
                _cloud4.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }

            if (_cloud5.Bounds.Right < -100)
            {
                _cloud5.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }

            _cloud1.X -= 20f * num;
            _cloud2.X -= 16f * num;
            _cloud3.X -= 15f * num;
            _cloud4.X -= 5f * num;
            _cloud5.X -= 10f * num;
            if (_shakeDuration > 0f)
            {
                _shakeDuration -= num;
                if (_shakeTimer > 0f && _shakeObj != null)
                {
                    _shakeTimer -= num;
                    if (_shakeTimer <= 0f)
                    {
                        _shakeTimer = _shakeDelay;
                        if (_shookLeft)
                        {
                            _shookLeft = false;
                            _shakeObj.X += _shakeAmount;
                        }
                        else
                        {
                            _shakeObj.X -= _shakeAmount;
                            _shookLeft = true;
                        }
                    }
                }
            }

            if (_shakeScreen)
            {
                UpdateShake();
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (!_cameraTweening && !_lockControls)
            {
                var flag = false;
                if (Game.GlobalInput.JustPressed(9))
                {
                    if (SkillSystem.IconsVisible)
                    {
                        _toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons on";
                        _confirmText.Visible = false;
                        _continueText.Visible = false;
                        _navigationText.Visible = false;
                        SkillSystem.HideAllIcons();
                        _selectionIcon.Opacity = 0f;
                        _dialoguePlate.Opacity = 0f;
                        _descriptionDivider.Opacity = 0f;
                        _coinIcon.Opacity = 0f;
                        _playerMoney.Opacity = 0f;
                        _titleText.Opacity = 0f;
                    }
                    else
                    {
                        _toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
                        _confirmText.Visible = true;
                        _continueText.Visible = true;
                        _navigationText.Visible = true;
                        SkillSystem.ShowAllIcons();
                        _selectionIcon.Opacity = 1f;
                        _dialoguePlate.Opacity = 1f;
                        _descriptionDivider.Opacity = 1f;
                        _coinIcon.Opacity = 1f;
                        _playerMoney.Opacity = 1f;
                        _titleText.Opacity = 1f;
                    }

                    flag = true;
                }

                if (SkillSystem.IconsVisible)
                {
                    var selectedTraitIndex = _selectedTraitIndex;
                    var vector = new Vector2(-1f, -1f);
                    if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y)
                                .TopLink;
                        var skill = SkillSystem.GetSkill(SkillType.SuperSecret);
                        if (!_cameraTweening && skill.Visible && vector == new Vector2(7f, 1f))
                        {
                            _cameraTweening = true;
                            Tween.To(Camera, 0.5f, Quad.EaseOut, "Y", 60f.ToString());
                            Tween.AddEndHandlerToLastTween(this, "EndCameraTween");
                        }
                    }
                    else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y)
                                .BottomLink;
                    }

                    if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y)
                                .LeftLink;
                    }
                    else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y)
                                .RightLink;
                    }

                    if (vector.X != -1f && vector.Y != -1f)
                    {
                        var skill2 = SkillSystem.GetSkill((int) vector.X, (int) vector.Y);
                        if (skill2.Trait != SkillType.Null && skill2.Visible)
                        {
                            _selectedTraitIndex = vector;
                        }
                    }

                    if (selectedTraitIndex != _selectedTraitIndex)
                    {
                        var skill3 = SkillSystem.GetSkill((int) _selectedTraitIndex.X,
                            (int) _selectedTraitIndex.Y);
                        _selectionIcon.Position = SkillSystem.GetSkillPosition(skill3);
                        UpdateDescriptionPlate(skill3);
                        SoundManager.PlaySound("ShopMenuMove");
                        skill3.Scale = new Vector2(1.1f, 1.1f);
                        Tween.To(skill3, 0.1f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
                        _dialoguePlate.Visible = true;
                    }

                    var skill4 = SkillSystem.GetSkill((int) _selectedTraitIndex.X, (int) _selectedTraitIndex.Y);
                    if ((Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) &&
                        Game.PlayerStats.Gold >= skill4.TotalCost && skill4.CurrentLevel < skill4.MaxLevel)
                    {
                        SoundManager.PlaySound("TraitUpgrade");
                        if (!_fadingIn)
                        {
                            Game.PlayerStats.Gold -= skill4.TotalCost;
                            SetVisible(skill4, true);
                            SkillSystem.LevelUpTrait(skill4, true, false);
                            if (skill4.CurrentLevel >= skill4.MaxLevel)
                            {
                                SoundManager.PlaySound("TraitMaxxed");
                            }

                            UpdateDescriptionPlate(skill4);
                        }
                    }
                    else if ((Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) &&
                             Game.PlayerStats.Gold < skill4.TotalCost)
                    {
                        SoundManager.PlaySound("TraitPurchaseFail");
                    }

                    if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) && !flag)
                    {
                        _lockControls = true;
                        var rCScreenManager = ScreenManager as RCScreenManager;
                        var levelScreen = rCScreenManager.GetLevelScreen();
                        levelScreen.Reset();
                        if (levelScreen.CurrentRoom is StartingRoomObj)
                        {
                            rCScreenManager.StartWipeTransition();
                            Tween.RunFunction(0.2f, rCScreenManager, "HideCurrentScreen");
                            Tween.RunFunction(0.2f, levelScreen.CurrentRoom, "OnEnter");
                        }
                        else
                        {
                            (ScreenManager as RCScreenManager).DisplayScreen(15, true);
                        }
                    }
                }

                base.HandleInput();
            }
        }

        public void EndCameraTween()
        {
            _cameraTweening = false;
        }

        public void UpdateDescriptionPlate(SkillObj skill)
        {
            // Set initial parameters.
            var text = skill.IconName;
            Console.WriteLine(text);
            text = text.Replace("Locked", "");
            text = text.Replace("Max", "");
            _skillIcon.ChangeSprite(text);
            _skillTitle.Text = skill.Name;
            _skillDescription.Text = skill.Description;
            _skillDescription.WordWrap(280);
            _inputDescription.Text = skill.InputDescription;
            _inputDescription.WordWrap(280);
            _inputDescription.Y = _skillDescription.Bounds.Bottom + 10;

            // Update stats and modifier texts.
            var stat = skill.Trait.SkillStat();
            if (stat > -1f)
            {
                if (stat < 1f)
                {
                    stat *= 100f;
                    stat = (int) Math.Round(stat, MidpointRounding.AwayFromZero);
                }

                if (stat == 0f)
                {
                    stat = skill.ModifierAmount;
                    if (skill.Trait == SkillType.CritChanceUp)
                    {
                        stat *= 100f;
                        stat = (int) Math.Round(stat, MidpointRounding.AwayFromZero);
                    }
                }

                _skillCurrent.Text = "Current: " + stat + skill.UnitOfMeasurement;
                if (skill.CurrentLevel < skill.MaxLevel)
                {
                    var mod = skill.PerLevelModifier;
                    if (mod < 1f && skill.Trait != SkillType.InvulnerabilityTimeUp)
                    {
                        mod *= 100f;
                        if (skill.Trait != SkillType.DeathDodge)
                        {
                            mod = (int) Math.Round(mod, MidpointRounding.AwayFromZero);
                        }
                    }

                    _skillUpgrade.Text = "Upgrade: +" + mod + skill.UnitOfMeasurement;
                }
                else
                {
                    _skillUpgrade.Text = "Upgrade: --";
                }

                _skillLevel.Text = string.Concat("Level: ", skill.CurrentLevel, "/", skill.MaxLevel);

                var upText = "unlock";
                if (skill.CurrentLevel > 0)
                {
                    upText = "upgrade";
                }

                if (skill.CanPurchase)
                {
                    _skillCost.TextureColor = Color.Yellow;
                    _skillCost.Text = string.Format("{0} gold to {1}.", skill.TotalCost, upText);
                }
                else
                {
                    _skillCost.TextureColor = Color.Red;
                    _skillCost.Text = string.Format("Cannot purchase {0}.", upText);
                }

                if (_inputDescription.Text != " " && _inputDescription.Text != "")
                {
                    _skillCurrent.Y = _inputDescription.Bounds.Bottom + 40;
                }
                else
                {
                    _skillCurrent.Y = _skillDescription.Bounds.Bottom + 40;
                }

                _skillUpgrade.Y = _skillCurrent.Y + 30f;
                _skillLevel.Y = _skillUpgrade.Y + 30f;
                _descriptionDivider.Visible = true;
            }
            else
            {
                _skillCurrent.Text = "";
                _skillUpgrade.Text = "";
                _skillLevel.Text = "";
                _descriptionDivider.Visible = false;
                var upText = "unlock";
                if (skill.CurrentLevel > 0)
                {
                    upText = "upgrade";
                }

                if (skill.CanPurchase)
                {
                    _skillCost.TextureColor = Color.Yellow;
                    _skillCost.Text = string.Format("{0} gold to {1}.", skill.TotalCost, upText);
                }
                else
                {
                    _skillCost.TextureColor = Color.Red;
                    _skillCost.Text = string.Format("Cannot purchase {0}.", upText);
                }
            }

            // Update visibility.
            _descriptionDivider.Position = new Vector2(_skillCurrent.AbsX, _skillCurrent.AbsY - 20f);
            if (skill.CurrentLevel >= skill.MaxLevel)
            {
                _skillCost.Visible = false;
                _skillCostBG.Visible = false;
            }
            else
            {
                _skillCost.Visible = true;
                _skillCostBG.Visible = true;
            }

            // Money money money.
            _playerMoney.Text = Game.PlayerStats.Gold.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            _cloud1.Y = _cloud2.Y = _cloud3.Y = _cloud4.Y = _cloud5.Y = Camera.TopLeftCorner.Y * 0.2f;
            _bg.Y = Camera.TopLeftCorner.Y * 0.2f;
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null,
                Camera.GetTransformation());
            _bg.Draw(Camera);
            _cloud1.Draw(Camera);
            _cloud2.Draw(Camera);
            _cloud3.Draw(Camera);
            _cloud4.Draw(Camera);
            _cloud5.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _manor.Draw(Camera);
            _impactEffectPool.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _selectionIcon.Draw(Camera);
            var skillArray = SkillSystem.GetSkillArray();
            for (var i = 0; i < skillArray.Length; i++)
            {
                var skillObj = skillArray[i];
                if (skillObj.Trait != SkillType.Filler && skillObj.Trait != SkillType.Null && skillObj.Visible)
                {
                    skillObj.Draw(Camera);
                }
            }

            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            _dialoguePlate.Draw(Camera);
            _continueText.Draw(Camera);
            _toggleIconsText.Draw(Camera);
            _confirmText.Draw(Camera);
            _navigationText.Draw(Camera);
            _playerMoney.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _descriptionDivider.Draw(Camera);
            _coinIcon.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Skill Screen");
                _titleText.Dispose();
                _titleText = null;
                _bg.Dispose();
                _bg = null;
                _cloud1.Dispose();
                _cloud1 = null;
                _cloud2.Dispose();
                _cloud2 = null;
                _cloud3.Dispose();
                _cloud3 = null;
                _cloud4.Dispose();
                _cloud4 = null;
                _cloud5.Dispose();
                _cloud5 = null;
                _continueText.Dispose();
                _continueText = null;
                _toggleIconsText.Dispose();
                _toggleIconsText = null;
                _confirmText.Dispose();
                _confirmText = null;
                _navigationText.Dispose();
                _navigationText = null;
                _dialoguePlate.Dispose();
                _dialoguePlate = null;
                _selectionIcon.Dispose();
                _selectionIcon = null;
                _impactEffectPool.Dispose();
                _impactEffectPool = null;
                _manor.Dispose();
                _manor = null;
                _shakeObj = null;
                _playerMoney.Dispose();
                _playerMoney = null;
                _coinIcon.Dispose();
                _coinIcon = null;
                _skillCurrent = null;
                _skillCost = null;
                _skillCostBG = null;
                _skillDescription = null;
                _inputDescription = null;
                _skillUpgrade = null;
                _skillLevel = null;
                _skillIcon = null;
                _skillTitle = null;
                _descriptionDivider.Dispose();
                _descriptionDivider = null;
                base.Dispose();
            }
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
        {
            SoundManager.PlaySound("TowerLand");
            _screenShakeMagnitude = magnitude;
            _horizontalShake = horizontalShake;
            _verticalShake = verticalShake;
            _shakeScreen = true;
        }

        public void UpdateShake()
        {
            if (_horizontalShake)
            {
                _bg.X = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
                _manor.X = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
            }

            if (_verticalShake)
            {
                _bg.Y = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
                _manor.Y = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
            }
        }

        public void StopScreenShake()
        {
            _shakeScreen = false;
            _bg.X = 0f;
            _bg.Y = 0f;
            _manor.X = 0f;
            _manor.Y = 0f;
        }

        private static string AddSpacesToString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ' || char.IsNumber(text[i]) && text[i - 1] != ' ')
                {
                    newText.Append(' ');
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}
