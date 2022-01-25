// 
//  Rogue Legacy Randomizer - DiaryEntryScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens
{
    public class DiaryEntryScreen : Screen
    {
        private List<ObjContainer> _diaryList;
        private int                _entryIndex;
        private int                _entryRow;
        private float              _inputDelay;
        private ObjContainer       _selectedEntry;
        private SpriteObj          _titleText;
        private int                _unlockedEntries;

        public DiaryEntryScreen()
        {
            _diaryList = new List<ObjContainer>();
            DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _titleText = new SpriteObj("DiaryEntryTitleText_Sprite");
            _titleText.ForceDraw = true;
            _titleText.X = 660f;
            _titleText.Y = 72f;
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
                objContainer.Scale = new Vector2(180f / objContainer.Width, 50f / objContainer.Height);
                objContainer.Position = new Vector2(num3, num4);
                var textObj = new TextObj(Game.JunicodeFont);
                textObj.Text = "Entry #" + (i + 1);
                textObj.OverrideParentScale = true;
                textObj.OutlineWidth = 2;
                textObj.FontSize = 12f;
                textObj.Y -= 64f;
                textObj.Align = Types.TextAlign.Centre;
                objContainer.AddChild(textObj);
                _diaryList.Add(objContainer);
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
            _inputDelay = 0.5f;
            _entryRow = 1;
            _entryIndex = 0;
            UpdateSelection();
            _unlockedEntries = Game.PlayerStats.DiaryEntry;
            if (_unlockedEntries >= 24)
            {
                GameUtil.UnlockAchievement("LOVE_OF_BOOKS");
            }

            for (var i = 0; i < _diaryList.Count; i++)
            {
                if (i < _unlockedEntries)
                {
                    _diaryList[i].Visible = true;
                }
                else
                {
                    _diaryList[i].Visible = false;
                }
            }

            BackBufferOpacity = 0f;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
            _titleText.Opacity = 0f;
            Tween.To(_titleText, 0.25f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            var num2 = 0f;
            foreach (var current in _diaryList)
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
            foreach (var current in _diaryList)
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

            _titleText.Opacity = 1f;
            Tween.To(_titleText, 0.25f, Tween.EaseNone, "Opacity", "0");
            _inputDelay = 1f;
            Tween.To(this, 0.5f, Tween.EaseNone, "BackBufferOpacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void HandleInput()
        {
            if (_inputDelay <= 0f)
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
                    if (_entryIndex > 0 && _diaryList[_entryIndex - 1].Visible)
                    {
                        SoundManager.PlaySound("frame_swap");
                        _entryIndex--;
                    }
                }
                else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
                {
                    if (_entryIndex < _diaryList.Count - 1 && _diaryList[_entryIndex + 1].Visible)
                    {
                        _entryIndex++;
                        SoundManager.PlaySound("frame_swap");
                    }
                }
                else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                {
                    if (_entryRow > 1 && _diaryList[_entryIndex - 5].Visible)
                    {
                        _entryRow--;
                        _entryIndex -= 5;
                        SoundManager.PlaySound("frame_swap");
                    }
                }
                else if ((Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) && _entryRow < 5 &&
                         _diaryList[_entryIndex + 5].Visible)
                {
                    _entryRow++;
                    _entryIndex += 5;
                    SoundManager.PlaySound("frame_swap");
                }

                if (_entryRow > 5)
                {
                    _entryRow = 5;
                }

                if (_entryRow < 1)
                {
                    _entryRow = 1;
                }

                if (_entryIndex >= _entryRow * 5)
                {
                    _entryIndex = _entryRow * 5 - 1;
                }

                if (_entryIndex < _entryRow * 5 - 5)
                {
                    _entryIndex = _entryRow * 5 - 5;
                }

                UpdateSelection();
                base.HandleInput();
            }
        }

        private void DisplayEntry()
        {
            var rCScreenManager = ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + _entryIndex);
            rCScreenManager.DisplayScreen(13, true);
        }

        private void UpdateSelection()
        {
            if (_selectedEntry != null)
            {
                _selectedEntry.TextureColor = Color.White;
            }

            _selectedEntry = _diaryList[_entryIndex];
            _selectedEntry.TextureColor = Color.Yellow;
        }

        public override void Update(GameTime gameTime)
        {
            if (_inputDelay > 0f)
            {
                _inputDelay -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _titleText.Draw(Camera);
            foreach (var current in _diaryList)
            {
                current.Draw(Camera);
            }

            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Diary Entry Screen");
            foreach (var current in _diaryList)
            {
                current.Dispose();
            }

            _diaryList.Clear();
            _diaryList = null;
            _selectedEntry = null;
            _titleText.Dispose();
            _titleText = null;
            base.Dispose();
        }
    }
}
