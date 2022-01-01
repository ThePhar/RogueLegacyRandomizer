// 
// RogueLegacyArchipelago - LineageObj.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class LineageObj : ObjContainer
    {
        private readonly int m_textYPos = 140;
        public byte Age = 30;
        public bool BeatenABoss;
        public byte ChildAge = 4;
        public byte Class;
        public bool FlipPortrait;
        public bool IsFemale;
        private TextObj m_ageText;
        private TextObj m_classTextObj;
        private bool m_frameDropping;
        private SpriteObj m_frameSprite;
        private bool m_isDead;
        private Color m_lichColour1 = new Color(255, 255, 255, 255);
        private Color m_lichColour2 = new Color(198, 198, 198, 255);
        private SpriteObj m_plaqueSprite;
        private TextObj m_playerName;
        private ObjContainer m_playerSprite;
        private Color m_skinColour1 = new Color(231, 175, 131, 255);
        private Color m_skinColour2 = new Color(199, 109, 112, 255);
        private SpriteObj m_spellIcon;
        private SpriteObj m_spellIconHolder;
        private TextObj m_trait1Title;
        private TextObj m_trait2Title;
        public int NumEnemiesKilled;
        public byte Spell;

        public LineageObj(LineageScreen screen, bool createEmpty = false)
        {
            Name = "";
            m_frameSprite = new SpriteObj("LineageScreenFrame_Sprite");
            m_frameSprite.Scale = new Vector2(2.8f, 2.8f);
            m_frameSprite.DropShadow = new Vector2(4f, 6f);
            m_plaqueSprite = new SpriteObj("LineageScreenPlaque1Long_Sprite");
            m_plaqueSprite.Scale = new Vector2(1.8f, 2f);
            m_playerSprite = new ObjContainer("PlayerIdle_Character");
            m_playerSprite.AnimationDelay = 0.1f;
            m_playerSprite.Scale = new Vector2(2f, 2f);
            m_playerSprite.OutlineWidth = 2;
            m_playerSprite.GetChildAt(10).Visible = false;
            m_playerSprite.GetChildAt(11).Visible = false;
            m_playerSprite.GetChildAt(1).TextureColor = Color.Red;
            m_playerSprite.GetChildAt(7).TextureColor = Color.Red;
            m_playerSprite.GetChildAt(14).Visible = false;
            m_playerSprite.GetChildAt(16).Visible = false;
            var textureColor = new Color(251, 156, 172);
            m_playerSprite.GetChildAt(13).TextureColor = textureColor;
            m_playerName = new TextObj(Game.JunicodeFont);
            m_playerName.FontSize = 10f;
            m_playerName.Text = "Sir Skunky IV";
            m_playerName.Align = Types.TextAlign.Centre;
            m_playerName.OutlineColour = new Color(181, 142, 39);
            m_playerName.OutlineWidth = 2;
            m_playerName.Y = m_textYPos;
            m_playerName.LimitCorners = true;
            AddChild(m_playerName);
            m_classTextObj = new TextObj(Game.JunicodeFont);
            m_classTextObj.FontSize = 8f;
            m_classTextObj.Align = Types.TextAlign.Centre;
            m_classTextObj.OutlineColour = new Color(181, 142, 39);
            m_classTextObj.OutlineWidth = 2;
            m_classTextObj.Text = "the Knight";
            m_classTextObj.Y = m_playerName.Y + m_playerName.Height - 8f;
            m_classTextObj.LimitCorners = true;
            AddChild(m_classTextObj);
            m_trait1Title = new TextObj(Game.JunicodeFont);
            m_trait1Title.FontSize = 8f;
            m_trait1Title.Align = Types.TextAlign.Centre;
            m_trait1Title.OutlineColour = new Color(181, 142, 39);
            m_trait1Title.OutlineWidth = 2;
            m_trait1Title.Y = m_classTextObj.Y + m_classTextObj.Height + 5f;
            m_trait1Title.Text = "";
            m_trait1Title.LimitCorners = true;
            AddChild(m_trait1Title);
            m_trait2Title = m_trait1Title.Clone() as TextObj;
            m_trait2Title.Y += 20f;
            m_trait2Title.Text = "";
            m_trait2Title.LimitCorners = true;
            AddChild(m_trait2Title);
            m_ageText = m_trait1Title.Clone() as TextObj;
            m_ageText.Text = "xxx - xxx";
            m_ageText.Visible = false;
            m_ageText.LimitCorners = true;
            AddChild(m_ageText);
            m_spellIcon = new SpriteObj("Blank_Sprite");
            m_spellIcon.OutlineWidth = 1;
            m_spellIconHolder = new SpriteObj("BlacksmithUI_IconBG_Sprite");
            if (!createEmpty)
            {
                IsFemale = false;
                if (CDGMath.RandomInt(0, 1) > 0)
                {
                    IsFemale = true;
                }

                if (IsFemale)
                {
                    CreateFemaleName(screen);
                }
                else
                {
                    CreateMaleName(screen);
                }

                Traits = TraitType.CreateRandomTraits();
                Class = ClassType.GetRandomClass();
                m_classTextObj.Text = "the " + ClassType.ToString(Class, IsFemale);
                while (Class == 7 || Class == 15)
                {
                    if (Traits.X != 12f && Traits.Y != 12f)
                    {
                        break;
                    }

                    Traits = TraitType.CreateRandomTraits();
                }

                while ((Class == 1 || Class == 9 || Class == 16) && (Traits.X == 31f || Traits.Y == 31f))
                    Traits = TraitType.CreateRandomTraits();
                var spellList = ClassType.GetSpellList(Class);
                do
                {
                    Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
                } while ((Spell == 11 || Spell == 4 || Spell == 6) && (Traits.X == 31f || Traits.Y == 31f));

                Array.Clear(spellList, 0, spellList.Length);
                Age = (byte) CDGMath.RandomInt(18, 30);
                ChildAge = (byte) CDGMath.RandomInt(2, 5);
                UpdateData();
            }
        }

        public string PlayerName
        {
            get { return m_playerName.Text; }
            set { m_playerName.Text = value; }
        }

        public byte ChestPiece { get; set; }
        public byte HeadPiece { get; set; }
        public byte ShoulderPiece { get; set; }
        public bool DisablePlaque { get; set; }
        public Vector2 Traits { get; internal set; }

        public bool IsDead
        {
            get { return m_isDead; }
            set
            {
                m_isDead = value;
                if (value)
                {
                    m_trait1Title.Visible = false;
                    m_trait2Title.Visible = false;
                    m_ageText.Visible = true;
                    return;
                }

                m_trait1Title.Visible = true;
                m_trait2Title.Visible = true;
                m_ageText.Visible = false;
            }
        }

        public override Rectangle Bounds
        {
            get { return m_playerSprite.Bounds; }
        }

        public override Rectangle AbsBounds
        {
            get { return m_playerSprite.Bounds; }
        }

        private void CreateMaleName(LineageScreen screen)
        {
            var text = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
            if (screen != null)
            {
                var num = 0;
                while (screen.CurrentBranchNameCopyFound(text))
                {
                    text = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
                    num++;
                    if (num > 20)
                    {
                        break;
                    }
                }
            }

            if (text != null)
            {
                if (text.Length > 10)
                {
                    text = text.Substring(0, 9) + ".";
                }

                var num2 = 0;
                var text2 = "";
                if (screen != null)
                {
                    num2 = screen.NameCopies(text);
                }

                if (num2 > 0)
                {
                    text2 = CDGMath.ToRoman(num2 + 1);
                }

                m_playerName.Text = "Sir " + text;
                if (text2 != "")
                {
                    var expr_BD = m_playerName;
                    expr_BD.Text = expr_BD.Text + " " + text2;
                }
            }
            else
            {
                m_playerName.Text = "Sir Hero";
            }
        }

        private void CreateFemaleName(LineageScreen screen)
        {
            var text = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
            if (screen != null)
            {
                var num = 0;
                while (screen.CurrentBranchNameCopyFound(text))
                {
                    text = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
                    num++;
                    if (num > 20)
                    {
                        break;
                    }
                }
            }

            if (text != null)
            {
                if (text.Length > 10)
                {
                    text = text.Substring(0, 9) + ".";
                }

                var num2 = 0;
                var text2 = "";
                if (screen != null)
                {
                    num2 = screen.NameCopies(text);
                }

                if (num2 > 0)
                {
                    text2 = CDGMath.ToRoman(num2 + 1);
                }

                m_playerName.Text = "Lady " + text;
                if (text2 != "")
                {
                    var expr_BD = m_playerName;
                    expr_BD.Text = expr_BD.Text + " " + text2;
                }
            }
            else
            {
                m_playerName.Text = "Lady Heroine";
            }
        }

        public void RandomizePortrait()
        {
            var num = CDGMath.RandomInt(1, 5);
            var num2 = CDGMath.RandomInt(1, 5);
            var num3 = CDGMath.RandomInt(1, 5);
            if (Class == 17)
            {
                num = 7;
            }
            else if (Class == 16)
            {
                num = 6;
            }

            SetPortrait((byte) num, (byte) num2, (byte) num3);
        }

        public void SetPortrait(byte headPiece, byte shoulderPiece, byte chestPiece)
        {
            HeadPiece = headPiece;
            ShoulderPiece = shoulderPiece;
            ChestPiece = chestPiece;
            var text = (m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
            var startIndex = text.IndexOf("_") - 1;
            text = text.Remove(startIndex, 1);
            text = text.Replace("_", HeadPiece + "_");
            m_playerSprite.GetChildAt(12).ChangeSprite(text);
            var text2 = (m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
            startIndex = text2.IndexOf("_") - 1;
            text2 = text2.Remove(startIndex, 1);
            text2 = text2.Replace("_", ChestPiece + "_");
            m_playerSprite.GetChildAt(4).ChangeSprite(text2);
            var text3 = (m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
            startIndex = text3.IndexOf("_") - 1;
            text3 = text3.Remove(startIndex, 1);
            text3 = text3.Replace("_", ShoulderPiece + "_");
            m_playerSprite.GetChildAt(9).ChangeSprite(text3);
            var text4 = (m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
            startIndex = text4.IndexOf("_") - 1;
            text4 = text4.Remove(startIndex, 1);
            text4 = text4.Replace("_", ShoulderPiece + "_");
            m_playerSprite.GetChildAt(3).ChangeSprite(text4);
        }

        public void UpdateAge(int currentEra)
        {
            var num = currentEra - ChildAge;
            var num2 = currentEra + Age;
            m_ageText.Text = num + " - " + num2;
        }

        public void UpdateData()
        {
            SetTraits(Traits);
            if (Traits.X == 8f || Traits.Y == 8f)
            {
                m_playerSprite.GetChildAt(7).Visible = false;
            }

            if (Traits.X == 20f || Traits.Y == 20f)
            {
                FlipPortrait = true;
            }

            m_classTextObj.Text = "the " + ClassType.ToString(Class, IsFemale);
            m_spellIcon.ChangeSprite(SpellType.Icon(Spell));
            if (Class == 0 || Class == 8)
            {
                m_playerSprite.GetChildAt(15).Visible = true;
                m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleShield_Sprite");
            }
            else if (Class == 5 || Class == 13)
            {
                m_playerSprite.GetChildAt(15).Visible = true;
                m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleLamp_Sprite");
            }
            else if (Class == 1 || Class == 9)
            {
                m_playerSprite.GetChildAt(15).Visible = true;
                m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleBeard_Sprite");
            }
            else if (Class == 4 || Class == 12)
            {
                m_playerSprite.GetChildAt(15).Visible = true;
                m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleHeadband_Sprite");
            }
            else if (Class == 2 || Class == 10)
            {
                m_playerSprite.GetChildAt(15).Visible = true;
                m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleHorns_Sprite");
            }
            else
            {
                m_playerSprite.GetChildAt(15).Visible = false;
            }

            m_playerSprite.GetChildAt(0).Visible = false;
            if (Class == 16)
            {
                m_playerSprite.GetChildAt(0).Visible = true;
                m_playerSprite.GetChildAt(12).ChangeSprite("PlayerIdleHead" + 6 + "_Sprite");
            }

            if (Class == 17)
            {
                m_playerSprite.GetChildAt(12).ChangeSprite("PlayerIdleHead" + 7 + "_Sprite");
            }

            if (!IsFemale)
            {
                m_playerSprite.GetChildAt(5).Visible = false;
                m_playerSprite.GetChildAt(13).Visible = false;
            }
            else
            {
                m_playerSprite.GetChildAt(5).Visible = true;
                m_playerSprite.GetChildAt(13).Visible = true;
            }

            if (Traits.X == 6f || Traits.Y == 6f)
            {
                m_playerSprite.Scale = new Vector2(3f, 3f);
            }

            if (Traits.X == 7f || Traits.Y == 7f)
            {
                m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
            }

            if (Traits.X == 10f || Traits.Y == 10f)
            {
                m_playerSprite.ScaleX *= 0.825f;
                m_playerSprite.ScaleY *= 1.25f;
            }

            if (Traits.X == 9f || Traits.Y == 9f)
            {
                m_playerSprite.ScaleX *= 1.25f;
                m_playerSprite.ScaleY *= 1.175f;
            }

            if (Class == 6 || Class == 14)
            {
                m_playerSprite.OutlineColour = Color.White;
                return;
            }

            m_playerSprite.OutlineColour = Color.Black;
        }

        public void DropFrame()
        {
            m_frameDropping = true;
            Tween.By(m_frameSprite, 0.7f, Back.EaseOut, "X", (-(float) m_frameSprite.Width / 2f - 2f).ToString(), "Y",
                "30", "Rotation", "45");
            Tween.By(m_playerSprite, 0.7f, Back.EaseOut, "X", (-(float) m_frameSprite.Width / 2f - 2f).ToString(), "Y",
                "30", "Rotation", "45");
            Tween.RunFunction(1.5f, this, "DropFrame2");
        }

        public void DropFrame2()
        {
            SoundManager.PlaySound("Cutsc_Picture_Fall");
            Tween.By(m_frameSprite, 0.5f, Quad.EaseIn, "Y", "1000");
            Tween.By(m_playerSprite, 0.5f, Quad.EaseIn, "Y", "1000");
        }

        public override void Draw(Camera2D camera)
        {
            if (FlipPortrait)
            {
                m_playerSprite.Rotation = 180f;
            }

            if (!m_frameDropping)
            {
                m_frameSprite.Position = Position;
                m_frameSprite.Y -= 12f;
                m_frameSprite.X += 5f;
            }

            m_frameSprite.Opacity = Opacity;
            m_frameSprite.Draw(camera);
            if (!IsDead && Spell != 0)
            {
                m_spellIconHolder.Position = new Vector2(m_frameSprite.X, m_frameSprite.Bounds.Bottom - 20);
                m_spellIcon.Position = m_spellIconHolder.Position;
                m_spellIconHolder.Draw(camera);
                m_spellIcon.Draw(camera);
            }

            m_playerSprite.OutlineColour = OutlineColour;
            m_playerSprite.OutlineWidth = OutlineWidth;
            if (!m_frameDropping)
            {
                m_playerSprite.Position = Position;
                m_playerSprite.X += 10f;
                if (FlipPortrait)
                {
                    m_playerSprite.X -= 10f;
                    m_playerSprite.Y -= 30f;
                }
            }

            m_playerSprite.Opacity = Opacity;
            m_playerSprite.Draw(camera);
            if (CollisionMath.Intersects(Bounds, camera.Bounds))
            {
                if (Class == 7 || Class == 15)
                {
                    Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
                    Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
                    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_lichColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_lichColour2.ToVector4());
                    camera.End();
                    camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                        Game.ColourSwapShader, camera.GetTransformation());
                    m_playerSprite.GetChildAt(12).Draw(camera);
                    camera.End();
                    camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                        null, camera.GetTransformation());
                    if (IsFemale)
                    {
                        m_playerSprite.GetChildAt(13).Draw(camera);
                    }

                    m_playerSprite.GetChildAt(15).Draw(camera);
                }
                else if (Class == 3 || Class == 11)
                {
                    Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
                    Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
                    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
                    camera.End();
                    camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                        Game.ColourSwapShader, camera.GetTransformation());
                    m_playerSprite.GetChildAt(12).Draw(camera);
                    camera.End();
                    camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                        null, camera.GetTransformation());
                    if (IsFemale)
                    {
                        m_playerSprite.GetChildAt(13).Draw(camera);
                    }

                    m_playerSprite.GetChildAt(15).Draw(camera);
                }
            }

            if (!DisablePlaque)
            {
                if (!m_frameDropping)
                {
                    m_plaqueSprite.Position = Position;
                    m_plaqueSprite.X += 5f;
                    m_plaqueSprite.Y = m_frameSprite.Y + m_frameSprite.Height - 30f;
                }

                m_plaqueSprite.Draw(camera);
                camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                base.Draw(camera);
                camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            }

            if (m_frameDropping)
            {
                m_frameSprite.Draw(camera);
                m_playerSprite.Draw(camera);
            }
        }

        public void SetTraits(Vector2 traits)
        {
            Traits = traits;
            var text = "";
            if (Traits.X != 0f)
            {
                text += TraitType.ToString((byte) Traits.X);
            }
            else
            {
                m_trait1Title.Text = "";
            }

            if (Traits.Y != 0f)
            {
                text = text + ", " + TraitType.ToString((byte) Traits.Y);
            }

            m_trait1Title.Text = text;
        }

        public void ClearTraits()
        {
            Traits = Vector2.Zero;
            m_trait1Title.Text = "No Traits";
            m_trait2Title.Text = "";
        }

        public void OutlineLineageObj(Color color, int width)
        {
            m_plaqueSprite.OutlineColour = color;
            m_plaqueSprite.OutlineWidth = width;
            m_frameSprite.OutlineColour = color;
            m_frameSprite.OutlineWidth = width;
        }

        public void UpdateClassRank()
        {
            var text = "the ";
            if (BeatenABoss)
            {
                text += "Legendary ";
            }
            else if (NumEnemiesKilled < 5)
            {
                text += "Useless ";
            }
            else if (NumEnemiesKilled >= 5 && NumEnemiesKilled < 10)
            {
                text += "Feeble ";
            }
            else if (NumEnemiesKilled >= 10 && NumEnemiesKilled < 15)
            {
                text += "Determined ";
            }
            else if (NumEnemiesKilled >= 15 && NumEnemiesKilled < 20)
            {
                text += "Stout ";
            }
            else if (NumEnemiesKilled >= 20 && NumEnemiesKilled < 25)
            {
                text += "Gallant ";
            }
            else if (NumEnemiesKilled >= 25 && NumEnemiesKilled < 30)
            {
                text += "Valiant ";
            }
            else if (NumEnemiesKilled >= 30 && NumEnemiesKilled < 35)
            {
                text += "Heroic ";
            }
            else
            {
                text += "Divine ";
            }

            text += ClassType.ToString(Class, IsFemale);
            m_classTextObj.Text = text;
        }

        protected override GameObj CreateCloneInstance()
        {
            return new LineageObj(null);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_playerSprite.Dispose();
                m_playerSprite = null;
                m_trait1Title = null;
                m_trait2Title = null;
                m_ageText = null;
                m_playerName = null;
                m_classTextObj = null;
                m_frameSprite.Dispose();
                m_frameSprite = null;
                m_plaqueSprite.Dispose();
                m_plaqueSprite = null;
                m_spellIcon.Dispose();
                m_spellIcon = null;
                m_spellIconHolder.Dispose();
                m_spellIconHolder = null;
                base.Dispose();
            }
        }
    }
}