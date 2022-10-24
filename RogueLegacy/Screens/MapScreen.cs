// Rogue Legacy Randomizer - MapScreen.cs
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
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;

namespace RogueLegacy.Screens
{
    public class MapScreen : Screen
    {
        private TextObj        _alzheimersQuestionMarks;
        private KeyIconTextObj _continueText;
        private bool           _isTeleporter;
        private ObjContainer   _legend;
        private MapObj         _mapDisplay;
        private KeyIconTextObj _navigationText;
        private ObjContainer   _playerIcon;
        private KeyIconTextObj _recenterText;
        private int            _selectedTeleporter;
        private SpriteObj[]    _teleporterList;
        private SpriteObj      _titleText;

        public MapScreen(ProceduralLevelScreen level)
        {
            _mapDisplay = new MapObj(false, level);
            _alzheimersQuestionMarks = new TextObj(Game.JunicodeLargeFont);
            _alzheimersQuestionMarks.FontSize = 30f;
            _alzheimersQuestionMarks.ForceDraw = true;
            _alzheimersQuestionMarks.Text = "?????";
            _alzheimersQuestionMarks.Align = Types.TextAlign.Centre;
            _alzheimersQuestionMarks.Position = new Vector2(660f, 360f - _alzheimersQuestionMarks.Height / 2f);
        }

        public bool IsTeleporter
        {
            get => _isTeleporter;
            set
            {
                _mapDisplay.DrawTeleportersOnly = value;
                _isTeleporter = value;
                if (value)
                {
                    if (_teleporterList != null)
                    {
                        Array.Clear(_teleporterList, 0, _teleporterList.Length);
                    }

                    _teleporterList = _mapDisplay.TeleporterList();
                }
            }
        }

        private void FindRoomTitlePos(List<RoomObj> roomList, Zone zone, out Vector2 pos)
        {
            var num = 3.40282347E+38f;
            var num2 = -3.40282347E+38f;
            var num3 = 3.40282347E+38f;
            var num4 = -3.40282347E+38f;
            foreach (var current in roomList)
            {
                if (current.Name != "Boss" &&
                    (current.Zone == zone ||
                     current.Zone == Zone.Castle &&
                     (current.Name == "Start" || current.Name == "CastleEntrance")))
                {
                    if (current.X < num)
                    {
                        num = current.X;
                    }

                    if (current.X + current.Width > num2)
                    {
                        num2 = current.X + current.Width;
                    }

                    if (current.Y < num3)
                    {
                        num3 = current.Y;
                    }

                    if (current.Y + current.Height > num4)
                    {
                        num4 = current.Y + current.Height;
                    }
                }
            }

            pos = new Vector2((num2 + num) / 2f, (num4 + num3) / 2f);
            pos = new Vector2(pos.X / 1320f * 60f, pos.Y / 720f * 32f);
        }

        public void SetPlayer(PlayerObj player)
        {
            _mapDisplay.SetPlayer(player);
        }

        public override void LoadContent()
        {
            _mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), Camera);
            _mapDisplay.CameraOffset = new Vector2(660f, 360f);
            _legend = new ObjContainer();
            _legend.ForceDraw = true;
            var spriteObj = new SpriteObj("TraitsScreenPlate_Sprite");
            _legend.AddChild(spriteObj);
            spriteObj.Scale = new Vector2(0.75f, 0.58f);
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Position = new Vector2(_legend.Width / 2 * spriteObj.ScaleX, _legend.Bounds.Top + 10);
            textObj.Text = "Legend";
            textObj.FontSize = 12f;
            textObj.DropShadow = new Vector2(2f, 2f);
            textObj.TextureColor = new Color(213, 213, 173);
            _legend.AddChild(textObj);
            _legend.AnimationDelay = 0.0333333351f;
            _legend.Position = new Vector2(1320 - _legend.Width - 20, 720 - _legend.Height - 20);
            var spriteObj2 = new SpriteObj("MapPlayerIcon_Sprite");
            spriteObj2.Position = new Vector2(30f, 60f);
            spriteObj2.PlayAnimation();
            _legend.AddChild(spriteObj2);
            var num = 30;
            var spriteObj3 = new SpriteObj("MapBossIcon_Sprite");
            spriteObj3.Position = new Vector2(spriteObj2.X, spriteObj2.Y + num);
            spriteObj3.PlayAnimation();
            _legend.AddChild(spriteObj3);
            var spriteObj4 = new SpriteObj("MapLockedChestIcon_Sprite");
            spriteObj4.Position = new Vector2(spriteObj2.X, spriteObj3.Y + num);
            spriteObj4.PlayAnimation();
            _legend.AddChild(spriteObj4);
            var spriteObj5 = new SpriteObj("MapFairyChestIcon_Sprite");
            spriteObj5.Position = new Vector2(spriteObj2.X, spriteObj4.Y + num);
            spriteObj5.PlayAnimation();
            _legend.AddChild(spriteObj5);
            var spriteObj6 = new SpriteObj("MapChestUnlocked_Sprite");
            spriteObj6.Position = new Vector2(spriteObj2.X, spriteObj5.Y + num);
            _legend.AddChild(spriteObj6);
            var spriteObj7 = new SpriteObj("MapTeleporterIcon_Sprite");
            spriteObj7.Position = new Vector2(spriteObj2.X, spriteObj6.Y + num);
            spriteObj7.PlayAnimation();
            _legend.AddChild(spriteObj7);
            var spriteObj8 = new SpriteObj("MapBonusIcon_Sprite");
            spriteObj8.Position = new Vector2(spriteObj2.X, spriteObj7.Y + num);
            spriteObj8.PlayAnimation();
            _legend.AddChild(spriteObj8);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.Position = new Vector2(spriteObj2.X + 50f, 55f);
            textObj2.Text =
                "You are here \nBoss location \nUnopened chest \nFairy chest \nOpened chest \nTeleporter \nBonus Room";
            textObj2.FontSize = 10f;
            textObj2.DropShadow = new Vector2(2f, 2f);
            _legend.AddChild(textObj2);
            spriteObj2.X += 4f;
            spriteObj2.Y += 4f;
            _titleText = new SpriteObj("TeleporterTitleText_Sprite");
            _titleText.ForceDraw = true;
            _titleText.X = 660f;
            _titleText.Y = 72f;
            _playerIcon = new ObjContainer("PlayerWalking_Character");
            _playerIcon.Scale = new Vector2(0.6f, 0.6f);
            _playerIcon.AnimationDelay = 0.1f;
            _playerIcon.PlayAnimation();
            _playerIcon.ForceDraw = true;
            _playerIcon.OutlineWidth = 2;
            _playerIcon.GetChildAt(1).TextureColor = Color.Red;
            _playerIcon.GetChildAt(7).TextureColor = Color.Red;
            _playerIcon.GetChildAt(8).TextureColor = Color.Red;
            _playerIcon.GetChildAt(16).Visible = false;
            _playerIcon.GetChildAt(5).Visible = false;
            _playerIcon.GetChildAt(13).Visible = false;
            _playerIcon.GetChildAt(0).Visible = false;
            _playerIcon.GetChildAt(15).Visible = false;
            _playerIcon.GetChildAt(14).Visible = false;
            _continueText = new KeyIconTextObj(Game.JunicodeFont);
            _continueText.Text = "to close map";
            _continueText.FontSize = 12f;
            _continueText.ForceDraw = true;
            _continueText.Position = new Vector2(50f, 200 - _continueText.Height - 40);
            _recenterText = new KeyIconTextObj(Game.JunicodeFont);
            _recenterText.Text = "to re-center on player";
            _recenterText.FontSize = 12f;
            _recenterText.Position = new Vector2(_continueText.X, 200 - _continueText.Height - 80);
            _recenterText.ForceDraw = true;
            _navigationText = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Text = "to move map";
            _navigationText.FontSize = 12f;
            _navigationText.Position = new Vector2(_continueText.X, 200 - _continueText.Height - 120);
            _navigationText.ForceDraw = true;
            base.LoadContent();
        }

        public override void ReinitializeRTs()
        {
            _mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), Camera);
            base.ReinitializeRTs();
        }

        public void AddRooms(List<RoomObj> roomList)
        {
            _mapDisplay.AddAllRooms(roomList);
        }

        public void RefreshMapChestIcons(RoomObj room)
        {
            _mapDisplay.RefreshChestIcons(room);
        }

        public override void OnEnter()
        {
            SoundManager.PlaySound("Map_On");
            _mapDisplay.CentreAroundPlayer();
            if (!IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
            {
                _mapDisplay.DrawNothing = true;
            }
            else
            {
                _mapDisplay.DrawNothing = false;
            }

            _continueText.Text = "[Input:" + 9 + "]  to close map";
            _recenterText.Text = "[Input:" + 0 + "]  to center on player";
            if (!InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _navigationText.Text = "Use arrow keys to move map";
            }
            else
            {
                _navigationText.Text = "[Button:LeftStick] to move map";
            }

            if (IsTeleporter && _teleporterList.Length > 0)
            {
                var spriteObj = _teleporterList[_selectedTeleporter];
                _playerIcon.Position = new Vector2(spriteObj.X + 7f, spriteObj.Y - 20f);
                _mapDisplay.CentreAroundTeleporter(_selectedTeleporter);
            }

            base.OnEnter();
        }

        public override void OnExit()
        {
            SoundManager.PlaySound("Map_Off");
            IsTeleporter = false;
            base.OnExit();
        }

        public override void HandleInput()
        {
            if (Game.GlobalInput.JustPressed(9) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                Game.ScreenManager.Player.UnlockControls();
                (ScreenManager as RCScreenManager).HideCurrentScreen();
            }

            if (!IsTeleporter)
            {
                var num = 5f;
                if (Game.GlobalInput.Pressed(16) || Game.GlobalInput.Pressed(17))
                {
                    var expr_7F_cp_0 = _mapDisplay;
                    expr_7F_cp_0.CameraOffset.Y = expr_7F_cp_0.CameraOffset.Y + num;
                }
                else if (Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19))
                {
                    var expr_B5_cp_0 = _mapDisplay;
                    expr_B5_cp_0.CameraOffset.Y = expr_B5_cp_0.CameraOffset.Y - num;
                }

                if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
                {
                    var expr_E9_cp_0 = _mapDisplay;
                    expr_E9_cp_0.CameraOffset.X = expr_E9_cp_0.CameraOffset.X + num;
                }
                else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
                {
                    var expr_11F_cp_0 = _mapDisplay;
                    expr_11F_cp_0.CameraOffset.X = expr_11F_cp_0.CameraOffset.X - num;
                }

                if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                {
                    _mapDisplay.CentreAroundPlayer();
                }
            }
            else
            {
                var selectedTeleporter = _selectedTeleporter;
                if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
                {
                    _selectedTeleporter++;
                    if (_selectedTeleporter >= _teleporterList.Length)
                    {
                        _selectedTeleporter = 0;
                    }
                }
                else if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
                {
                    _selectedTeleporter--;
                    if (_selectedTeleporter < 0 && _teleporterList.Length > 0)
                    {
                        _selectedTeleporter = _teleporterList.Length - 1;
                    }
                    else if (_selectedTeleporter < 0 && _teleporterList.Length <= 0)
                    {
                        _selectedTeleporter = 0;
                    }
                }

                if (selectedTeleporter != _selectedTeleporter)
                {
                    _mapDisplay.CentreAroundTeleporter(_selectedTeleporter, true);
                }

                if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                {
                    _mapDisplay.TeleportPlayer(_selectedTeleporter);
                    (ScreenManager as RCScreenManager).HideCurrentScreen();
                }
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            _mapDisplay.DrawRenderTargets(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Draw((ScreenManager as RCScreenManager).GetLevelScreen().RenderTarget, Vector2.Zero,
                Color.White * 0.3f);
            _mapDisplay.Draw(Camera);
            if (IsTeleporter && _teleporterList.Length > 0)
            {
                _titleText.Draw(Camera);
                var spriteObj = _teleporterList[_selectedTeleporter];
                _playerIcon.Position = new Vector2(spriteObj.X + 14f, spriteObj.Y - 20f);
                _playerIcon.Draw(Camera);
            }

            if (!IsTeleporter)
            {
                _recenterText.Draw(Camera);
                _navigationText.Draw(Camera);
            }

            if (!IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
            {
                _alzheimersQuestionMarks.Draw(Camera);
            }

            _continueText.Draw(Camera);
            _legend.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public void AddAllIcons(List<RoomObj> roomList)
        {
            _mapDisplay.AddAllIcons(roomList);
        }

        public override void DisposeRTs()
        {
            _mapDisplay.DisposeRTs();
            base.DisposeRTs();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Map Screen");
            if (_mapDisplay != null)
            {
                _mapDisplay.Dispose();
            }

            _mapDisplay = null;
            if (_legend != null)
            {
                _legend.Dispose();
            }

            _legend = null;
            if (_playerIcon != null)
            {
                _playerIcon.Dispose();
            }

            _playerIcon = null;
            if (_teleporterList != null)
            {
                Array.Clear(_teleporterList, 0, _teleporterList.Length);
            }

            _teleporterList = null;
            if (_titleText != null)
            {
                _titleText.Dispose();
            }

            _titleText = null;
            if (_continueText != null)
            {
                _continueText.Dispose();
            }

            _continueText = null;
            if (_recenterText != null)
            {
                _recenterText.Dispose();
            }

            _recenterText = null;
            if (_navigationText != null)
            {
                _navigationText.Dispose();
            }

            _navigationText = null;
            _alzheimersQuestionMarks.Dispose();
            _alzheimersQuestionMarks = null;
            base.Dispose();
        }
    }
}
