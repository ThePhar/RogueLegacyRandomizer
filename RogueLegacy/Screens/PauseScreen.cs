// Rogue Legacy Randomizer - PauseScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;

namespace RogueLegacy.Screens
{
    public class PauseScreen : Screen
    {
        private List<PauseInfoObj> _infoObjList;
        private float              _inputDelay;
        private SpriteObj          _optionsIcon;
        private KeyIconTextObj     _optionsKey;
        private SpriteObj          _profileCard;
        private KeyIconTextObj     _profileCardKey;
        private SpriteObj          _titleText;

        public PauseScreen()
        {
            DrawIfCovered = true;
        }

        public override void LoadContent()
        {
            _titleText = new SpriteObj("GamePausedTitleText_Sprite");
            _titleText.X = 660f;
            _titleText.Y = 72f;
            _titleText.ForceDraw = true;
            _infoObjList = new List<PauseInfoObj>();
            _infoObjList.Add(new PauseInfoObj());
            _profileCard = new SpriteObj("TitleProfileCard_Sprite");
            _profileCard.OutlineWidth = 2;
            _profileCard.Scale = new Vector2(2f, 2f);
            _profileCard.Position = new Vector2(_profileCard.Width, 720 - _profileCard.Height);
            _profileCard.ForceDraw = true;
            _optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
            _optionsIcon.Scale = new Vector2(2f, 2f);
            _optionsIcon.OutlineWidth = _profileCard.OutlineWidth;
            _optionsIcon.Position = new Vector2(1320 - _optionsIcon.Width * 2 + 120, _profileCard.Y);
            _optionsIcon.ForceDraw = true;
            _profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
            _profileCardKey.Align = Types.TextAlign.Centre;
            _profileCardKey.FontSize = 12f;
            _profileCardKey.Text = "[Input:" + 7 + "]";
            _profileCardKey.Position = new Vector2(_profileCard.X,
                _profileCard.Bounds.Top - _profileCardKey.Height - 10);
            _profileCardKey.ForceDraw = true;
            _optionsKey = new KeyIconTextObj(Game.JunicodeFont);
            _optionsKey.Align = Types.TextAlign.Centre;
            _optionsKey.FontSize = 12f;
            _optionsKey.Text = "[Input:" + 4 + "]";
            _optionsKey.Position = new Vector2(_optionsIcon.X, _optionsIcon.Bounds.Top - _optionsKey.Height - 10);
            _optionsKey.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            UpdatePauseScreenIcons();
            _inputDelay = 0.5f;
            if (SoundManager.IsMusicPlaying)
            {
                SoundManager.PauseMusic();
            }

            SoundManager.PlaySound("Pause_Toggle");
            var levelScreen = (ScreenManager as RCScreenManager).GetLevelScreen();
            foreach (var current in _infoObjList)
            {
                current.Reset();
                current.Visible = false;
            }

            var player = (ScreenManager as RCScreenManager).Player;
            var pauseInfoObj = _infoObjList[0];
            pauseInfoObj.Visible = true;
            pauseInfoObj.AddItem("Class: ", ((ClassType) Game.PlayerStats.Class).Name(Game.PlayerStats.IsFemale));
            pauseInfoObj.AddItem("Strength: ", player.Damage.ToString());
            pauseInfoObj.AddItem("Magic: ", player.TotalMagicDamage.ToString());
            pauseInfoObj.AddItem("Armor: ", player.TotalArmor.ToString());
            pauseInfoObj.ResizePlate();
            pauseInfoObj.X = player.X - Camera.TopLeftCorner.X;
            pauseInfoObj.Y = player.Bounds.Bottom - Camera.TopLeftCorner.Y + pauseInfoObj.Height / 2f - 20f;
            if (!Game.PlayerStats.TutorialComplete)
            {
                pauseInfoObj.SetName("?????");
            }
            else
            {
                pauseInfoObj.SetName(Game.PlayerStats.PlayerName);
            }

            pauseInfoObj.SetNamePosition(new Vector2(pauseInfoObj.X, player.Bounds.Top - Camera.TopLeftCorner.Y - 40f));
            pauseInfoObj.Visible = player.Visible;
            var num = _infoObjList.Count - 1;
            for (var i = num;
                 i < levelScreen.CurrentRoom.EnemyList.Count + levelScreen.CurrentRoom.TempEnemyList.Count;
                 i++)
            {
                _infoObjList.Add(new PauseInfoObj
                {
                    Visible = false
                });
            }

            for (var j = 1; j < levelScreen.CurrentRoom.EnemyList.Count + 1; j++)
            {
                var enemyObj = levelScreen.CurrentRoom.EnemyList[j - 1];
                if (!enemyObj.NonKillable && !enemyObj.IsKilled && enemyObj.Visible)
                {
                    var pauseInfoObj2 = _infoObjList[j];
                    pauseInfoObj2.Visible = true;
                    if (!LevelENV.CreateRetailVersion)
                    {
                        pauseInfoObj2.AddItem("Level: ", enemyObj.Level.ToString());
                    }
                    else
                    {
                        pauseInfoObj2.AddItem("Level: ", ((int) (enemyObj.Level * 2.75f)).ToString());
                    }

                    pauseInfoObj2.AddItem("Strength: ", enemyObj.Damage.ToString());
                    pauseInfoObj2.AddItem("Health: ", enemyObj.CurrentHealth + "/" + enemyObj.MaxHealth);
                    pauseInfoObj2.ResizePlate();
                    pauseInfoObj2.X = enemyObj.X - Camera.TopLeftCorner.X;
                    pauseInfoObj2.Y = enemyObj.Bounds.Bottom - Camera.TopLeftCorner.Y + pauseInfoObj2.Height / 2f - 20f;
                    pauseInfoObj2.SetName(enemyObj.Name);
                    pauseInfoObj2.SetNamePosition(new Vector2(pauseInfoObj2.X,
                        enemyObj.Bounds.Top - Camera.TopLeftCorner.Y - 40f));
                }
            }

            var count = levelScreen.CurrentRoom.EnemyList.Count;
            for (var k = 0; k < levelScreen.CurrentRoom.TempEnemyList.Count; k++)
            {
                var enemyObj2 = levelScreen.CurrentRoom.TempEnemyList[k];
                if (!enemyObj2.NonKillable && !enemyObj2.IsKilled)
                {
                    var pauseInfoObj3 = _infoObjList[k + 1 + count];
                    pauseInfoObj3.Visible = true;
                    if (!LevelENV.CreateRetailVersion)
                    {
                        pauseInfoObj3.AddItem("Level: ", enemyObj2.Level.ToString());
                    }
                    else
                    {
                        pauseInfoObj3.AddItem("Level: ", ((int) (enemyObj2.Level * 2.75f)).ToString());
                    }

                    pauseInfoObj3.AddItem("Strength: ", enemyObj2.Damage.ToString());
                    pauseInfoObj3.AddItem("Health: ", enemyObj2.CurrentHealth + "/" + enemyObj2.MaxHealth);
                    pauseInfoObj3.ResizePlate();
                    pauseInfoObj3.X = enemyObj2.X - Camera.TopLeftCorner.X;
                    pauseInfoObj3.Y = enemyObj2.Bounds.Bottom - Camera.TopLeftCorner.Y + pauseInfoObj3.Height / 2f -
                                      20f;
                    pauseInfoObj3.SetName(enemyObj2.Name);
                    pauseInfoObj3.SetNamePosition(new Vector2(pauseInfoObj3.X,
                        enemyObj2.Bounds.Top - Camera.TopLeftCorner.Y - 40f));
                }
            }

            base.OnEnter();
        }

        public void UpdatePauseScreenIcons()
        {
            _profileCardKey.Text = "[Input:" + 7 + "]";
            _optionsKey.Text = "[Input:" + 4 + "]";
        }

        public override void OnExit()
        {
            if (SoundManager.IsMusicPaused)
            {
                SoundManager.ResumeMusic();
            }

            SoundManager.PlaySound("Resume_Toggle");
            foreach (var current in _infoObjList)
            {
                current.Visible = false;
            }

            base.OnExit();
        }

        public override void HandleInput()
        {
            if (_inputDelay <= 0f)
            {
                if (Game.GlobalInput.JustPressed(7) && Game.PlayerStats.TutorialComplete)
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(17, true);
                }

                if (Game.GlobalInput.JustPressed(4))
                {
                    var list = new List<object>();
                    list.Add(false);
                    (ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
                }

                if (Game.GlobalInput.JustPressed(8))
                {
                    (ScreenManager as RCScreenManager).GetLevelScreen().UnpauseScreen();
                    (ScreenManager as RCScreenManager).HideCurrentScreen();
                }

                base.HandleInput();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_inputDelay > 0f)
            {
                _inputDelay -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            _titleText.Draw(Camera);
            foreach (var current in _infoObjList)
            {
                current.Draw(Camera);
            }

            if (Game.PlayerStats.TutorialComplete)
            {
                _profileCardKey.Draw(Camera);
            }

            _optionsKey.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _optionsIcon.Draw(Camera);
            if (Game.PlayerStats.TutorialComplete)
            {
                _profileCard.Draw(Camera);
            }

            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Pause Screen");
            foreach (var current in _infoObjList)
            {
                current.Dispose();
            }

            _infoObjList.Clear();
            _infoObjList = null;
            _titleText.Dispose();
            _titleText = null;
            _profileCard.Dispose();
            _profileCard = null;
            _optionsIcon.Dispose();
            _optionsIcon = null;
            _profileCardKey.Dispose();
            _profileCardKey = null;
            _optionsKey.Dispose();
            _optionsKey = null;
            base.Dispose();
        }

        private class PauseInfoObj : ObjContainer
        {
            private int           m_arrayIndex;
            private TextObj       m_name;
            private ObjContainer  m_namePlate;
            private List<TextObj> m_textDataList;
            private List<TextObj> m_textList;

            public PauseInfoObj() : base("GameOverStatPlate_Character")
            {
                ForceDraw = true;
                m_textList = new List<TextObj>();
                m_textDataList = new List<TextObj>();
                m_namePlate = new ObjContainer("DialogBox_Character");
                m_namePlate.ForceDraw = true;
                m_name = new TextObj(Game.JunicodeFont);
                m_name.Align = Types.TextAlign.Centre;
                m_name.Text = "<noname>";
                m_name.FontSize = 8f;
                m_name.Y -= 45f;
                m_name.OverrideParentScale = true;
                m_name.DropShadow = new Vector2(2f, 2f);
                m_namePlate.AddChild(m_name);
            }

            public void SetName(string name)
            {
                m_name.Text = name;
                m_namePlate.Scale = Vector2.One;
                m_namePlate.Scale = new Vector2((m_name.Width + 70f) / m_namePlate.Width,
                    (m_name.Height + 20f) / m_namePlate.Height);
            }

            public void SetNamePosition(Vector2 pos)
            {
                m_namePlate.Position = pos;
            }

            public void AddItem(string title, string data)
            {
                TextObj textObj;
                if (m_textList.Count <= m_arrayIndex)
                {
                    textObj = new TextObj(Game.JunicodeFont);
                }
                else
                {
                    textObj = m_textList[m_arrayIndex];
                }

                textObj.FontSize = 8f;
                textObj.Text = title;
                textObj.Align = Types.TextAlign.Right;
                textObj.Y = _objectList[0].Bounds.Top + textObj.Height + m_arrayIndex * 20;
                textObj.DropShadow = new Vector2(2f, 2f);
                if (m_textList.Count <= m_arrayIndex)
                {
                    AddChild(textObj);
                    m_textList.Add(textObj);
                }

                TextObj textObj2;
                if (m_textDataList.Count <= m_arrayIndex)
                {
                    textObj2 = new TextObj(Game.JunicodeFont);
                }
                else
                {
                    textObj2 = m_textDataList[m_arrayIndex];
                }

                textObj2.FontSize = 8f;
                textObj2.Text = data;
                textObj2.Y = textObj.Y;
                textObj2.DropShadow = new Vector2(2f, 2f);
                if (m_textDataList.Count <= m_arrayIndex)
                {
                    AddChild(textObj2);
                    m_textDataList.Add(textObj2);
                }

                m_arrayIndex++;
            }

            public void ResizePlate()
            {
                _objectList[0].ScaleY = 1f;
                _objectList[0].ScaleY =
                    _objectList[1].Height * (_objectList.Count + 1) / 2 / (float) _objectList[0].Height;
                var num = 0;
                foreach (var current in m_textList)
                {
                    if (current.Width > num)
                    {
                        num = current.Width;
                    }
                }

                var num2 = 0;
                foreach (var current2 in m_textDataList)
                {
                    if (current2.Width > num2)
                    {
                        num2 = current2.Width;
                    }
                }

                _objectList[0].ScaleX = 1f;
                _objectList[0].ScaleX = (num + num2 + 50) / (float) _objectList[0].Width;
                var num3 = (int) (-(_objectList[0].Width / 2f) + num) + 25;
                var num4 = _objectList[0].Height / (m_textList.Count + 2);
                for (var i = 0; i < m_textList.Count; i++)
                {
                    m_textList[i].X = num3;
                    m_textList[i].Y = _objectList[0].Bounds.Top + num4 + num4 * i;
                    m_textDataList[i].X = num3;
                    m_textDataList[i].Y = m_textList[i].Y;
                }
            }

            public override void Draw(Camera2D camera)
            {
                if (Visible)
                {
                    m_namePlate.Draw(camera);
                    m_name.Draw(camera);
                }

                base.Draw(camera);
            }

            public void Reset()
            {
                foreach (var current in m_textList)
                {
                    current.Text = "";
                }

                foreach (var current2 in m_textDataList)
                {
                    current2.Text = "";
                }

                m_arrayIndex = 0;
            }

            public override void Dispose()
            {
                if (!IsDisposed)
                {
                    m_textList.Clear();
                    m_textList = null;
                    m_textDataList.Clear();
                    m_textDataList = null;
                    m_namePlate.Dispose();
                    m_namePlate = null;
                    m_name = null;
                    base.Dispose();
                }
            }
        }
    }
}
