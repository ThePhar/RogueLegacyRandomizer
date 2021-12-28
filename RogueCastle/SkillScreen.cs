// 
// RogueLegacyArchipelago - SkillScreen.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class SkillScreen : Screen
    {
        private readonly int m_shakeAmount = 2;
        private readonly float m_shakeDelay = 0.01f;

        private bool m_cameraTweening;
        private bool m_fadingIn;
        private bool m_horizontalShake;
        private bool m_lockControls;
        private bool m_shakeScreen;
        private bool m_shookLeft;
        private bool m_verticalShake;
        private float m_screenShakeMagnitude;
        private float m_shakeDuration;
        private float m_shakeTimer;
        private GameObj m_shakeObj;
        private ImpactEffectPool m_impactEffectPool;
        private KeyIconTextObj m_confirmText;
        private KeyIconTextObj m_continueText;
        private KeyIconTextObj m_inputDescription;
        private KeyIconTextObj m_navigationText;
        private KeyIconTextObj m_toggleIconsText;
        private SpriteObj m_bg;
        private SpriteObj m_cloud1;
        private SpriteObj m_cloud2;
        private SpriteObj m_cloud3;
        private SpriteObj m_cloud4;
        private SpriteObj m_cloud5;
        private SpriteObj m_coinIcon;
        private SpriteObj m_descriptionDivider;
        private SpriteObj m_selectionIcon;
        private SpriteObj m_skillCostBG;
        private SpriteObj m_skillIcon;
        private SpriteObj m_titleText;
        private ObjContainer m_dialoguePlate;
        private ObjContainer m_manor;
        private TextObj m_playerMoney;
        private TextObj m_skillCost;
        private TextObj m_skillCurrent;
        private TextObj m_skillDescription;
        private TextObj m_skillLevel;
        private TextObj m_skillTitle;
        private TextObj m_skillUpgrade;
        private Vector2 m_selectedTraitIndex;

        public SkillScreen()
        {
            m_selectedTraitIndex = new Vector2(5f, 9f);
            m_impactEffectPool = new ImpactEffectPool(1000);
            DrawIfCovered = true;
        }

        public override void LoadContent()
        {
            m_impactEffectPool.Initialize();
            m_manor = new ObjContainer("TraitsCastle_Character");
            m_manor.Scale = new Vector2(2f, 2f);
            m_manor.ForceDraw = true;
            for (var i = 0; i < m_manor.NumChildren; i++)
            {
                m_manor.GetChildAt(i).Visible = false;
                m_manor.GetChildAt(i).Opacity = 0f;
            }
            m_dialoguePlate = new ObjContainer("TraitsScreenPlate_Container");
            m_dialoguePlate.ForceDraw = true;
            m_dialoguePlate.Position = new Vector2(1320 - m_dialoguePlate.Width/2, 360f);
            m_skillIcon = new SpriteObj("Icon_Health_Up_Sprite");
            m_skillIcon.Position = new Vector2(-110f, -200f);
            m_dialoguePlate.AddChild(m_skillIcon);
            m_skillTitle = new TextObj(Game.JunicodeFont);
            m_skillTitle.Text = "Skill name";
            m_skillTitle.DropShadow = new Vector2(2f, 2f);
            m_skillTitle.TextureColor = new Color(236, 197, 132);
            m_skillTitle.Position = new Vector2(m_skillIcon.Bounds.Right + 15, m_skillIcon.Y);
            m_skillTitle.FontSize = 12f;
            m_dialoguePlate.AddChild(m_skillTitle);
            m_skillDescription = new TextObj(Game.JunicodeFont);
            m_skillDescription.Text = "Description text goes here.  Let's see how well the word wrap function works.";
            m_skillDescription.Position = new Vector2(m_dialoguePlate.GetChildAt(1).X - 30f,
                m_dialoguePlate.GetChildAt(1).Bounds.Bottom + 20);
            m_skillDescription.FontSize = 10f;
            m_skillDescription.DropShadow = new Vector2(2f, 2f);
            m_skillDescription.TextureColor = new Color(228, 218, 208);
            m_skillDescription.WordWrap(m_dialoguePlate.Width - 50);
            m_dialoguePlate.AddChild(m_skillDescription);
            m_inputDescription = new KeyIconTextObj(Game.JunicodeFont);
            m_inputDescription.Text = "Input descriptions go here..";
            m_inputDescription.Position = new Vector2(m_skillIcon.X - 30f, m_skillDescription.Bounds.Bottom + 20);
            m_inputDescription.FontSize = 10f;
            m_inputDescription.DropShadow = new Vector2(2f, 2f);
            m_inputDescription.TextureColor = new Color(228, 218, 208);
            m_inputDescription.WordWrap(m_dialoguePlate.Width - 50);
            m_dialoguePlate.AddChild(m_inputDescription);
            m_descriptionDivider = new SpriteObj("Blank_Sprite");
            m_descriptionDivider.ScaleX = 250f/m_descriptionDivider.Width;
            m_descriptionDivider.ScaleY = 0.25f;
            m_descriptionDivider.ForceDraw = true;
            m_descriptionDivider.DropShadow = new Vector2(2f, 2f);
            m_skillCurrent = new TextObj(Game.JunicodeFont);
            m_skillCurrent.Position = new Vector2(m_inputDescription.X, m_inputDescription.Bounds.Bottom + 10);
            m_skillCurrent.FontSize = 10f;
            m_skillCurrent.DropShadow = new Vector2(2f, 2f);
            m_skillCurrent.TextureColor = new Color(228, 218, 208);
            m_skillCurrent.WordWrap(m_dialoguePlate.Width - 50);
            m_dialoguePlate.AddChild(m_skillCurrent);
            m_skillUpgrade = (m_skillCurrent.Clone() as TextObj);
            m_skillUpgrade.Y += 15f;
            m_dialoguePlate.AddChild(m_skillUpgrade);
            m_skillLevel = (m_skillUpgrade.Clone() as TextObj);
            m_skillLevel.Y += 15f;
            m_dialoguePlate.AddChild(m_skillLevel);
            m_skillCost = new TextObj(Game.JunicodeFont);
            m_skillCost.X = m_skillIcon.X;
            m_skillCost.Y = 182f;
            m_skillCost.FontSize = 10f;
            m_skillCost.DropShadow = new Vector2(2f, 2f);
            m_skillCost.TextureColor = Color.Yellow;
            m_dialoguePlate.AddChild(m_skillCost);
            m_skillCostBG = new SpriteObj("SkillTreeGoldIcon_Sprite");
            m_skillCostBG.Position = new Vector2(-180f, 180f);
            m_dialoguePlate.AddChild(m_skillCostBG);
            m_dialoguePlate.ForceDraw = true;
            m_bg = new SpriteObj("TraitsBG_Sprite");
            m_bg.Scale = new Vector2(1320f/m_bg.Width, 1320f/m_bg.Width);
            m_bg.ForceDraw = true;
            m_cloud1 = new SpriteObj("TraitsCloud1_Sprite")
            {
                ForceDraw = true
            };
            m_cloud2 = new SpriteObj("TraitsCloud2_Sprite")
            {
                ForceDraw = true
            };
            m_cloud3 = new SpriteObj("TraitsCloud3_Sprite")
            {
                ForceDraw = true
            };
            m_cloud4 = new SpriteObj("TraitsCloud4_Sprite")
            {
                ForceDraw = true
            };
            m_cloud5 = new SpriteObj("TraitsCloud5_Sprite")
            {
                ForceDraw = true
            };
            var opacity = 1f;
            m_cloud1.Opacity = opacity;
            m_cloud2.Opacity = opacity;
            m_cloud3.Opacity = opacity;
            m_cloud4.Opacity = opacity;
            m_cloud5.Opacity = opacity;
            m_cloud1.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            m_cloud2.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            m_cloud3.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            m_cloud4.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            m_cloud5.Position = new Vector2(CDGMath.RandomInt(0, 1520), CDGMath.RandomInt(0, 360));
            m_selectionIcon = new SpriteObj("IconHalo_Sprite");
            m_selectionIcon.ForceDraw = true;
            m_selectionIcon.AnimationDelay = 0.1f;
            m_selectionIcon.PlayAnimation();
            m_selectionIcon.Scale = new Vector2(1.1f, 1.1f);
            m_titleText = new SpriteObj("ManorTitleText_Sprite");
            m_titleText.X = m_titleText.Width/2f + 20f;
            m_titleText.Y = 64.8f;
            m_titleText.ForceDraw = true;
            m_continueText = new KeyIconTextObj(Game.JunicodeFont);
            m_continueText.ForceDraw = true;
            m_continueText.FontSize = 12f;
            m_continueText.DropShadow = new Vector2(2f, 2f);
            m_continueText.Position = new Vector2(1300f, 630f);
            m_continueText.Align = Types.TextAlign.Right;
            m_toggleIconsText = new KeyIconTextObj(Game.JunicodeFont);
            m_toggleIconsText.ForceDraw = true;
            m_toggleIconsText.FontSize = 12f;
            m_toggleIconsText.DropShadow = new Vector2(2f, 2f);
            m_toggleIconsText.Position = new Vector2(m_continueText.X, m_continueText.Y + 40f);
            m_toggleIconsText.Align = Types.TextAlign.Right;
            m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
            m_confirmText.Align = Types.TextAlign.Right;
            m_confirmText.FontSize = 12f;
            m_confirmText.DropShadow = new Vector2(2f, 2f);
            m_confirmText.Position = new Vector2(1300f, 10f);
            m_confirmText.ForceDraw = true;
            m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
            m_navigationText.Align = Types.TextAlign.Right;
            m_navigationText.FontSize = 12f;
            m_navigationText.DropShadow = new Vector2(2f, 2f);
            m_navigationText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f);
            m_navigationText.ForceDraw = true;
            m_coinIcon = new SpriteObj("CoinIcon_Sprite");
            m_coinIcon.Position = new Vector2(1100f, 585f);
            m_coinIcon.Scale = new Vector2(0.9f, 0.9f);
            m_coinIcon.ForceDraw = true;
            m_playerMoney = new TextObj(Game.GoldFont);
            m_playerMoney.Align = Types.TextAlign.Left;
            m_playerMoney.Text = "1000";
            m_playerMoney.FontSize = 30f;
            m_playerMoney.Position = new Vector2(m_coinIcon.X + 35f, m_coinIcon.Y);
            m_playerMoney.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            var flag = true;

            // Update manor text.
            var manorSkill = SkillSystem.GetSkill(SkillType.ManorUpgrade);
            manorSkill.Description = "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
            manorSkill.Description =
                manorSkill.Description.Replace("GENDER", Game.PlayerStats.IsFemale ? "motherless" : "fatherless");

            m_lockControls = false;
            m_manor.GetChildAt(23).Visible = true;
            m_manor.GetChildAt(23).Opacity = 1f;
            Camera.Position = new Vector2(660f, 360f);
            var skillArray = SkillSystem.GetSkillArray();

            foreach (var s in skillArray)
            {
                SetVisible(s, false);
            }

            if (!SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("SkillTreeSong", true, 1f);
            }
            var skill = SkillSystem.GetSkill((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y);

            Console.WriteLine(m_selectedTraitIndex);
            Console.WriteLine("IS SKILL NULL?: " + (skill == null));

            m_selectionIcon.Position = SkillSystem.GetSkillPosition(skill);
            UpdateDescriptionPlate(skill);
            m_dialoguePlate.Visible = true;
            m_confirmText.Text = "[Input:" + 0 + "] to purchase/upgrade skill";
            m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
            m_continueText.Text = "[Input:" + 2 + "] to exit the manor";
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                m_navigationText.Text = "[Button:LeftStick] to navigate skills";
            }
            else
            {
                m_navigationText.Text = "Arrow keys to navigate skills";
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

        public void SetVisible(SkillObj skill, bool fadeIn)
        {
            if (skill.TraitType == SkillType.ManorUpgrade && fadeIn)
            {
                var level = skill.CurrentLevel;
                Console.WriteLine("USING PIECE {0}", SkillSystem.ManorPiecesOrder[level]);
                SetManorPieceVisible(SkillSystem.ManorPiecesOrder[level], skill);
                Program.Game.ArchipelagoManager.CheckLocations(SkillSystem.ManorPiecesOrder[level].Item2);

                return;
            }

            for (var i = 0; i < SkillSystem.GetSkill(SkillType.ManorUpgrade).CurrentLevel; i++)
            {
                var childAt = m_manor.GetChildAt(SkillSystem.ManorPiecesOrder[i].Item1);
                childAt.Opacity = 1f;
                childAt.Visible = true;
            }

            foreach (var current in SkillSystem.GetAllConnectingTraits(skill))
            {
                if (!current.Visible)
                {
                    current.Visible = true;
                    current.Opacity = 1f;
                }
            }

            if (m_manor.GetChildAt(7).Visible && m_manor.GetChildAt(16).Visible)
            {
                (m_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
            }

            if (m_manor.GetChildAt(6).Visible && m_manor.GetChildAt(16).Visible)
            {
                (m_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
            }

            if (m_manor.GetChildAt(2).Visible)
            {
                var spriteObj = m_manor.GetChildAt(32) as SpriteObj;
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

            var manorIndex = manorPiece.Item1;
            var childAt = m_manor.GetChildAt(manorIndex);
            var num = 0f;
            if (!childAt.Visible)
            {
                m_lockControls = true;
                childAt.Visible = true;
                var pos = new Vector2(childAt.AbsPosition.X, childAt.AbsBounds.Bottom);
                switch (manorIndex)
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
                        childAt.X -= childAt.Width*2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "X", (childAt.Width*2).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, false, childAt.Height*2, num);
                        goto IL_A26;
                    case 2:
                    {
                        childAt.Opacity = 1f;
                        num = 1.5f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2, num);
                        var spriteObj = m_manor.GetChildAt(32) as SpriteObj;
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
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2, num);
                        goto IL_A26;
                    case 4:
                        pos.Y -= 50f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2*0.25f, num);
                        goto IL_A26;
                    case 7:
                        pos.X = childAt.AbsBounds.Right - childAt.Width*2*0.25f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2*0.25f, num);
                        goto IL_A26;
                    case 8:
                        pos.X = childAt.AbsBounds.Right - childAt.Width*2*0.25f;
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2*0.25f, num);
                        goto IL_A26;
                    case 10:
                    case 21:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2, num);
                        goto IL_A26;
                    case 12:
                    case 14:
                        childAt.Opacity = 1f;
                        num = 1f;
                        childAt.X += childAt.Width*2;
                        pos.X = childAt.AbsPosition.X - 60f;
                        SoundManager.PlaySound("skill_tree_reveal_short_01", "skill_tree_reveal_short_02");
                        Tween.By(childAt, num, Quad.EaseOut, "X", (-(childAt.Width*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, false, childAt.Height*2, num);
                        goto IL_A26;
                    case 16:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2*0.5f, num);
                        goto IL_A26;
                    case 18:
                    case 19:
                        childAt.Opacity = 1f;
                        num = 3f;
                        childAt.Y += childAt.Height*2;
                        SoundManager.PlaySound("skill_tree_reveal_01", "skill_tree_reveal_02");
                        Tween.By(childAt, num, Quad.EaseOut, "Y", (-(childAt.Height*2)).ToString());
                        m_impactEffectPool.SkillTreeDustDuration(pos, true, childAt.Width*2*0.2f, num);
                        goto IL_A26;
                    case 23:
                        goto IL_A26;
                    case 29:
                    case 30:
                    case 31:
                        Tween.RunFunction(0.25f, typeof (SoundManager), "PlaySound", "skill_tree_reveal_bounce");
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
                Tween.AddEndHandlerToLastTween(m_impactEffectPool, "SkillTreeDustEffect", vector, true, childAt.Width*2);
                Tween.RunFunction(num, this, "ShakeScreen", 5, true, true);
                Tween.RunFunction(num + 0.2f, this, "StopScreenShake");
            }

            IL_A26:
            Tween.RunFunction(num, this, "SetSkillIconVisible", skillObj, manorPiece);
            if (m_manor.GetChildAt(7).Visible && m_manor.GetChildAt(16).Visible)
            {
                (m_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
            }
            if (m_manor.GetChildAt(6).Visible && m_manor.GetChildAt(16).Visible)
            {
                (m_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
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
            switch (skill.TraitType)
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
                case SkillType.ManorUpgrade:
                    b = SkillUnlockType.NetworkItem;
                    break;
            }

            if (b == SkillUnlockType.NetworkItem && displayScreen)
            {
                var list = new List<object>
                {
                    SkillUnlockType.NetworkItem,
                    manorPiece.Item2,
                };

                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.SkillUnlock, true, list);
            }
            else if (b != 0 && displayScreen)
            {
                var list = new List<object> { b };
                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.SkillUnlock, true, list);
            }
        }

        public void UnlockControls()
        {
            m_lockControls = false;
        }

        public void StartShake(GameObj obj, float shakeDuration)
        {
            m_shakeDuration = shakeDuration;
            m_shakeObj = obj;
            m_shakeTimer = m_shakeDelay;
            m_shookLeft = false;
        }

        public void EndShake()
        {
            if (m_shookLeft)
            {
                m_shakeObj.X += m_shakeAmount;
            }
            m_shakeObj = null;
            m_shakeTimer = 0f;
        }

        public void FadingComplete()
        {
            m_fadingIn = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_cameraTweening && (m_selectedTraitIndex != new Vector2(7f, 1f) && m_selectedTraitIndex != new Vector2(8f, 9f)) && Camera.Y != 360f)
            {
                m_cameraTweening = true;
                Tween.To(Camera, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
                Tween.AddEndHandlerToLastTween(this, "EndCameraTween");
            }
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (m_cloud1.Bounds.Right < -100)
            {
                m_cloud1.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }
            if (m_cloud2.Bounds.Right < -100)
            {
                m_cloud2.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }
            if (m_cloud3.Bounds.Right < -100)
            {
                m_cloud3.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }
            if (m_cloud4.Bounds.Right < -100)
            {
                m_cloud4.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }
            if (m_cloud5.Bounds.Right < -100)
            {
                m_cloud5.Position = new Vector2(CDGMath.RandomInt(1420, 1520), CDGMath.RandomInt(0, 360));
            }
            m_cloud1.X -= 20f*num;
            m_cloud2.X -= 16f*num;
            m_cloud3.X -= 15f*num;
            m_cloud4.X -= 5f*num;
            m_cloud5.X -= 10f*num;
            if (m_shakeDuration > 0f)
            {
                m_shakeDuration -= num;
                if (m_shakeTimer > 0f && m_shakeObj != null)
                {
                    m_shakeTimer -= num;
                    if (m_shakeTimer <= 0f)
                    {
                        m_shakeTimer = m_shakeDelay;
                        if (m_shookLeft)
                        {
                            m_shookLeft = false;
                            m_shakeObj.X += m_shakeAmount;
                        }
                        else
                        {
                            m_shakeObj.X -= m_shakeAmount;
                            m_shookLeft = true;
                        }
                    }
                }
            }
            if (m_shakeScreen)
            {
                UpdateShake();
            }
            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (!m_cameraTweening && !m_lockControls)
            {
                var flag = false;
                if (Game.GlobalInput.JustPressed(9))
                {
                    if (SkillSystem.IconsVisible)
                    {
                        m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons on";
                        m_confirmText.Visible = false;
                        m_continueText.Visible = false;
                        m_navigationText.Visible = false;
                        SkillSystem.HideAllIcons();
                        m_selectionIcon.Opacity = 0f;
                        m_dialoguePlate.Opacity = 0f;
                        m_descriptionDivider.Opacity = 0f;
                        m_coinIcon.Opacity = 0f;
                        m_playerMoney.Opacity = 0f;
                        m_titleText.Opacity = 0f;
                    }
                    else
                    {
                        m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
                        m_confirmText.Visible = true;
                        m_continueText.Visible = true;
                        m_navigationText.Visible = true;
                        SkillSystem.ShowAllIcons();
                        m_selectionIcon.Opacity = 1f;
                        m_dialoguePlate.Opacity = 1f;
                        m_descriptionDivider.Opacity = 1f;
                        m_coinIcon.Opacity = 1f;
                        m_playerMoney.Opacity = 1f;
                        m_titleText.Opacity = 1f;
                    }
                    flag = true;
                }
                if (SkillSystem.IconsVisible)
                {
                    var selectedTraitIndex = m_selectedTraitIndex;
                    var vector = new Vector2(-1f, -1f);
                    // Move Up and Down
                    if (Game.GlobalInput.JustPressed(InputMapType.PlayerUp1) || Game.GlobalInput.JustPressed(InputMapType.PlayerUp2))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y).TopLink;

                        var dragonSkill = SkillSystem.GetSkill(SkillType.SuperSecret);
                        if (!m_cameraTweening && dragonSkill.Visible && (vector == new Vector2(7f, 1f) || vector == new Vector2(8f, 9f)))
                        {
                            m_cameraTweening = true;
                            Tween.To(Camera, 0.5f, Quad.EaseOut, "Y", 60f.ToString());
                            Tween.AddEndHandlerToLastTween(this, "EndCameraTween");
                        }
                    }
                    else if (Game.GlobalInput.JustPressed(InputMapType.PlayerDown1) || Game.GlobalInput.JustPressed(InputMapType.PlayerDown2))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y)
                                .BottomLink;
                    }

                    // Move Left and Right
                    if (Game.GlobalInput.JustPressed(InputMapType.PlayerLeft1) || Game.GlobalInput.JustPressed(InputMapType.PlayerLeft2))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y)
                                .LeftLink;
                    }
                    else if (Game.GlobalInput.JustPressed(InputMapType.PlayerRight1) || Game.GlobalInput.JustPressed(InputMapType.PlayerRight2))
                    {
                        vector =
                            SkillSystem.GetSkillLink((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y)
                                .RightLink;
                    }

                    // Move selection.
                    if (vector.X != -1f && vector.Y != -1f)
                    {
                        var skill2 = SkillSystem.GetSkill((int) vector.X, (int) vector.Y);
                        if (skill2.TraitType != SkillType.Null && skill2.Visible)
                        {
                            m_selectedTraitIndex = vector;
                        }
                    }

                    // Update current selected trait.
                    if (selectedTraitIndex != m_selectedTraitIndex)
                    {
                        var selectedSkill = SkillSystem.GetSkill((int) m_selectedTraitIndex.X,
                            (int) m_selectedTraitIndex.Y);
                        m_selectionIcon.Position = SkillSystem.GetSkillPosition(selectedSkill);
                        UpdateDescriptionPlate(selectedSkill);
                        SoundManager.PlaySound("ShopMenuMove");
                        selectedSkill.Scale = new Vector2(1.1f, 1.1f);
                        Tween.To(selectedSkill, 0.1f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
                        m_dialoguePlate.Visible = true;
                    }

                    // Purchase Skill Feature
                    var skill = SkillSystem.GetSkill((int) m_selectedTraitIndex.X, (int) m_selectedTraitIndex.Y);
                    if (
                        (Game.GlobalInput.JustPressed(InputMapType.MenuConfirm1) || Game.GlobalInput.JustPressed(InputMapType.MenuConfirm2))
                        && Game.PlayerStats.Gold >= skill.TotalCost
                        && skill.CurrentLevel < skill.MaxLevel
                        && skill.CanPurchase)
                    {
                        SoundManager.PlaySound("TraitUpgrade");
                        if (!m_fadingIn)
                        {
                            Game.PlayerStats.Gold -= skill.TotalCost;
                            SetVisible(skill, true);

                            SkillSystem.LevelUpTrait(skill, true);

                            if (skill.CurrentLevel >= skill.MaxLevel)
                            {
                                SoundManager.PlaySound("TraitMaxxed");
                            }
                            UpdateDescriptionPlate(skill);
                        }
                    }
                    else if ((Game.GlobalInput.JustPressed(InputMapType.MenuConfirm1) || Game.GlobalInput.JustPressed(InputMapType.MenuConfirm2)))
                    {
                        SoundManager.PlaySound("TraitPurchaseFail");
                    }

                    // Toggle Hiding Skill Screen
                    if (Game.GlobalInput.JustPressed(2) || (Game.GlobalInput.JustPressed(3) && !flag))
                    {
                        m_lockControls = true;
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
            m_cameraTweening = false;
        }

        public void UpdateDescriptionPlate(SkillObj skill)
        {
            // Set initial parameters.
            var text = skill.IconName;
            Console.WriteLine(text);
            text = text.Replace("Locked", "");
            text = text.Replace("Max", "");
            m_skillIcon.ChangeSprite(text);
            m_skillTitle.Text = skill.Name;
            m_skillDescription.Text = skill.Description;
            m_skillDescription.WordWrap(280);
            m_inputDescription.Text = skill.InputDescription;
            m_inputDescription.WordWrap(280);
            m_inputDescription.Y = m_skillDescription.Bounds.Bottom + 10;

            // Update stats and modifier texts.
            var stat = SkillStatType.GetSkillStat(skill.TraitType);
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
                    if (skill.TraitType == SkillType.CritChanceUp)
                    {
                        stat *= 100f;
                        stat = (int) Math.Round(stat, MidpointRounding.AwayFromZero);
                    }
                }
                m_skillCurrent.Text = "Current: " + stat + skill.UnitOfMeasurement;
                if (skill.CurrentLevel < skill.MaxLevel)
                {
                    var mod = skill.PerLevelModifier;
                    if (mod < 1f && skill.TraitType != SkillType.InvulnerabilityTimeUp)
                    {
                        mod *= 100f;
                        if (skill.TraitType != SkillType.DeathDodge)
                        {
                            mod = (int) Math.Round(mod, MidpointRounding.AwayFromZero);
                        }
                    }
                    m_skillUpgrade.Text = "Upgrade: +" + mod + skill.UnitOfMeasurement;
                }
                else
                {
                    m_skillUpgrade.Text = "Upgrade: --";
                }
                m_skillLevel.Text = string.Concat("Level: ", skill.CurrentLevel, "/", skill.MaxLevel);

                var upText = "unlock";
                if (skill.CurrentLevel > 0)
                    upText = "upgrade";

                if (skill.CanPurchase)
                {
                    m_skillCost.TextureColor = Color.Yellow;
                    m_skillCost.Text = string.Format("{0} gold to {1}.", skill.TotalCost, upText);
                }
                else
                {
                    m_skillCost.TextureColor = Color.Red;
                    m_skillCost.Text = string.Format("Cannot purchase {0}.", upText);
                }

                if (m_inputDescription.Text != " " && m_inputDescription.Text != "")
                {
                    m_skillCurrent.Y = m_inputDescription.Bounds.Bottom + 40;
                }
                else
                {
                    m_skillCurrent.Y = m_skillDescription.Bounds.Bottom + 40;
                }
                m_skillUpgrade.Y = m_skillCurrent.Y + 30f;
                m_skillLevel.Y = m_skillUpgrade.Y + 30f;
                m_descriptionDivider.Visible = true;
            }
            else
            {
                m_skillCurrent.Text = "";
                m_skillUpgrade.Text = "";
                m_skillLevel.Text = "";
                m_descriptionDivider.Visible = false;
                var upText = "unlock";
                if (skill.CurrentLevel > 0)
                {
                    upText = "upgrade";
                }

                if (skill.CanPurchase)
                {
                    m_skillCost.TextureColor = Color.Yellow;
                    m_skillCost.Text = string.Format("{0} gold to {1}.", skill.TotalCost, upText);
                }
                else
                {
                    m_skillCost.TextureColor = Color.Red;
                    m_skillCost.Text = string.Format("Cannot purchase {0}.", upText);
                }
            }

            // Update visibility.
            m_descriptionDivider.Position = new Vector2(m_skillCurrent.AbsX, m_skillCurrent.AbsY - 20f);
            if (skill.CurrentLevel >= skill.MaxLevel)
            {
                m_skillCost.Visible = false;
                m_skillCostBG.Visible = false;
            }
            else
            {
                m_skillCost.Visible = true;
                m_skillCostBG.Visible = true;
            }

            // Money money money.
            m_playerMoney.Text = Game.PlayerStats.Gold.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            m_cloud1.Y = (m_cloud2.Y = (m_cloud3.Y = (m_cloud4.Y = (m_cloud5.Y = Camera.TopLeftCorner.Y*0.2f))));
            m_bg.Y = Camera.TopLeftCorner.Y*0.2f;
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null,
                Camera.GetTransformation());
            m_bg.Draw(Camera);
            m_cloud1.Draw(Camera);
            m_cloud2.Draw(Camera);
            m_cloud3.Draw(Camera);
            m_cloud4.Draw(Camera);
            m_cloud5.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            m_manor.Draw(Camera);
            m_impactEffectPool.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_selectionIcon.Draw(Camera);
            var skillArray = SkillSystem.GetSkillArray();
            for (var i = 0; i < skillArray.Length; i++)
            {
                var skillObj = skillArray[i];
                if (skillObj.TraitType != SkillType.Filler && skillObj.TraitType != SkillType.Null && skillObj.Visible)
                {
                    skillObj.Draw(Camera);
                }
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            m_dialoguePlate.Draw(Camera);
            m_continueText.Draw(Camera);
            m_toggleIconsText.Draw(Camera);
            m_confirmText.Draw(Camera);
            m_navigationText.Draw(Camera);
            m_playerMoney.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            m_descriptionDivider.Draw(Camera);
            m_coinIcon.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Skill Screen");
                m_titleText.Dispose();
                m_titleText = null;
                m_bg.Dispose();
                m_bg = null;
                m_cloud1.Dispose();
                m_cloud1 = null;
                m_cloud2.Dispose();
                m_cloud2 = null;
                m_cloud3.Dispose();
                m_cloud3 = null;
                m_cloud4.Dispose();
                m_cloud4 = null;
                m_cloud5.Dispose();
                m_cloud5 = null;
                m_continueText.Dispose();
                m_continueText = null;
                m_toggleIconsText.Dispose();
                m_toggleIconsText = null;
                m_confirmText.Dispose();
                m_confirmText = null;
                m_navigationText.Dispose();
                m_navigationText = null;
                m_dialoguePlate.Dispose();
                m_dialoguePlate = null;
                m_selectionIcon.Dispose();
                m_selectionIcon = null;
                m_impactEffectPool.Dispose();
                m_impactEffectPool = null;
                m_manor.Dispose();
                m_manor = null;
                m_shakeObj = null;
                m_playerMoney.Dispose();
                m_playerMoney = null;
                m_coinIcon.Dispose();
                m_coinIcon = null;
                m_skillCurrent = null;
                m_skillCost = null;
                m_skillCostBG = null;
                m_skillDescription = null;
                m_inputDescription = null;
                m_skillUpgrade = null;
                m_skillLevel = null;
                m_skillIcon = null;
                m_skillTitle = null;
                m_descriptionDivider.Dispose();
                m_descriptionDivider = null;
                base.Dispose();
            }
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
        {
            SoundManager.PlaySound("TowerLand");
            m_screenShakeMagnitude = magnitude;
            m_horizontalShake = horizontalShake;
            m_verticalShake = verticalShake;
            m_shakeScreen = true;
        }

        public void UpdateShake()
        {
            if (m_horizontalShake)
            {
                m_bg.X = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
                m_manor.X = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
            }
            if (m_verticalShake)
            {
                m_bg.Y = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
                m_manor.Y = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
            }
        }

        public void StopScreenShake()
        {
            m_shakeScreen = false;
            m_bg.X = 0f;
            m_bg.Y = 0f;
            m_manor.X = 0f;
            m_manor.Y = 0f;
        }
    }
}
