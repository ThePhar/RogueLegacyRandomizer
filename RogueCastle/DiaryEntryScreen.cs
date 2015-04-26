/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class DiaryEntryScreen : Screen
    {
        private List<ObjContainer> m_diaryList;
        private int m_entryIndex;
        private int m_entryRow;
        private float m_inputDelay;
        private ObjContainer m_selectedEntry;
        private SpriteObj m_titleText;
        private int m_unlockedEntries;

        public DiaryEntryScreen()
        {
            m_diaryList = new List<ObjContainer>();
            DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_titleText = new SpriteObj("DiaryEntryTitleText_Sprite");
            m_titleText.ForceDraw = true;
            m_titleText.X = 660f;
            m_titleText.Y = 72f;
            var num = 260;
            var num2 = 150;
            var num3 = num;
            var num4 = num2;
            var num5 = 100;
            var num6 = 200;
            var num7 = 5;
            var num8 = 0;
            for (var i = 0; i < 25; i++)
            {
                var objContainer = new ObjContainer("DialogBox_Character");
                objContainer.ForceDraw = true;
                objContainer.Scale = new Vector2(180f/objContainer.Width, 50f/objContainer.Height);
                objContainer.Position = new Vector2(num3, num4);
                var textObj = new TextObj(Game.JunicodeFont);
                textObj.Text = "Entry #" + (i + 1);
                textObj.OverrideParentScale = true;
                textObj.OutlineWidth = 2;
                textObj.FontSize = 12f;
                textObj.Y -= 64f;
                textObj.Align = Types.TextAlign.Centre;
                objContainer.AddChild(textObj);
                m_diaryList.Add(objContainer);
                num8++;
                num3 += num6;
                if (num8 >= num7)
                {
                    num8 = 0;
                    num3 = num;
                    num4 += num5;
                }
                if (i > 13)
                {
                    objContainer.Visible = false;
                }
            }
            base.LoadContent();
        }

        public override void OnEnter()
        {
            SoundManager.PlaySound("DialogOpen");
            m_inputDelay = 0.5f;
            m_entryRow = 1;
            m_entryIndex = 0;
            UpdateSelection();
            m_unlockedEntries = Game.PlayerStats.DiaryEntry;
            if (m_unlockedEntries >= 24)
            {
                GameUtil.UnlockAchievement("LOVE_OF_BOOKS");
            }
            for (var i = 0; i < m_diaryList.Count; i++)
            {
                if (i < m_unlockedEntries)
                {
                    m_diaryList[i].Visible = true;
                }
                else
                {
                    m_diaryList[i].Visible = false;
                }
            }
            BackBufferOpacity = 0f;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
            m_titleText.Opacity = 0f;
            Tween.To(m_titleText, 0.25f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            var num2 = 0f;
            foreach (var current in m_diaryList)
            {
                if (current.Visible)
                {
                    current.Opacity = 0f;
                    if (num >= 5)
                    {
                        num = 0;
                        num2 += 0.05f;
                    }
                    num++;
                    Tween.To(current, 0.25f, Tween.EaseNone, "delay", num2.ToString(), "Opacity", "1");
                    Tween.By(current, 0.25f, Quad.EaseOut, "delay", num2.ToString(), "Y", "50");
                }
            }
            base.OnEnter();
        }

        private void ExitTransition()
        {
            SoundManager.PlaySound("DialogMenuClose");
            var num = 0;
            var num2 = 0f;
            foreach (var current in m_diaryList)
            {
                if (current.Visible)
                {
                    current.Opacity = 1f;
                    if (num >= 5)
                    {
                        num = 0;
                        num2 += 0.05f;
                    }
                    num++;
                    Tween.To(current, 0.25f, Tween.EaseNone, "delay", num2.ToString(), "Opacity", "0");
                    Tween.By(current, 0.25f, Quad.EaseOut, "delay", num2.ToString(), "Y", "-50");
                }
            }
            m_titleText.Opacity = 1f;
            Tween.To(m_titleText, 0.25f, Tween.EaseNone, "Opacity", "0");
            m_inputDelay = 1f;
            Tween.To(this, 0.5f, Tween.EaseNone, "BackBufferOpacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void HandleInput()
        {
            if (m_inputDelay <= 0f)
            {
                if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                {
                    DisplayEntry();
                }
                else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                {
                    ExitTransition();
                }
                if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
                {
                    if (m_entryIndex > 0 && m_diaryList[m_entryIndex - 1].Visible)
                    {
                        SoundManager.PlaySound("frame_swap");
                        m_entryIndex--;
                    }
                }
                else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
                {
                    if (m_entryIndex < m_diaryList.Count - 1 && m_diaryList[m_entryIndex + 1].Visible)
                    {
                        m_entryIndex++;
                        SoundManager.PlaySound("frame_swap");
                    }
                }
                else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                {
                    if (m_entryRow > 1 && m_diaryList[m_entryIndex - 5].Visible)
                    {
                        m_entryRow--;
                        m_entryIndex -= 5;
                        SoundManager.PlaySound("frame_swap");
                    }
                }
                else if ((Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) && m_entryRow < 5 &&
                         m_diaryList[m_entryIndex + 5].Visible)
                {
                    m_entryRow++;
                    m_entryIndex += 5;
                    SoundManager.PlaySound("frame_swap");
                }
                if (m_entryRow > 5)
                {
                    m_entryRow = 5;
                }
                if (m_entryRow < 1)
                {
                    m_entryRow = 1;
                }
                if (m_entryIndex >= m_entryRow*5)
                {
                    m_entryIndex = m_entryRow*5 - 1;
                }
                if (m_entryIndex < m_entryRow*5 - 5)
                {
                    m_entryIndex = m_entryRow*5 - 5;
                }
                UpdateSelection();
                base.HandleInput();
            }
        }

        private void DisplayEntry()
        {
            var rCScreenManager = ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + m_entryIndex);
            rCScreenManager.DisplayScreen(13, true);
        }

        private void UpdateSelection()
        {
            if (m_selectedEntry != null)
            {
                m_selectedEntry.TextureColor = Color.White;
            }
            m_selectedEntry = m_diaryList[m_entryIndex];
            m_selectedEntry.TextureColor = Color.Yellow;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_inputDelay > 0f)
            {
                m_inputDelay -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_titleText.Draw(Camera);
            foreach (var current in m_diaryList)
            {
                current.Draw(Camera);
            }
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Diary Entry Screen");
                foreach (var current in m_diaryList)
                {
                    current.Dispose();
                }
                m_diaryList.Clear();
                m_diaryList = null;
                m_selectedEntry = null;
                m_titleText.Dispose();
                m_titleText = null;
                base.Dispose();
            }
        }
    }
}