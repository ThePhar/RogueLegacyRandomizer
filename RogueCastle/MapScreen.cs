using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class MapScreen : Screen
	{
		private MapObj m_mapDisplay;
		private ObjContainer m_legend;
		private SpriteObj[] m_teleporterList;
		private SpriteObj m_titleText;
		private ObjContainer m_playerIcon;
		private int m_selectedTeleporter;
		private KeyIconTextObj m_continueText;
		private KeyIconTextObj m_recentreText;
		private KeyIconTextObj m_navigationText;
		private TextObj m_alzheimersQuestionMarks;
		private bool m_isTeleporter;
		public bool IsTeleporter
		{
			get
			{
				return m_isTeleporter;
			}
			set
			{
				m_mapDisplay.DrawTeleportersOnly = value;
				m_isTeleporter = value;
				if (value)
				{
					if (m_teleporterList != null)
					{
						Array.Clear(m_teleporterList, 0, m_teleporterList.Length);
					}
					m_teleporterList = m_mapDisplay.TeleporterList();
				}
			}
		}
		public MapScreen(ProceduralLevelScreen level)
		{
			m_mapDisplay = new MapObj(false, level);
			m_alzheimersQuestionMarks = new TextObj(Game.JunicodeLargeFont);
			m_alzheimersQuestionMarks.FontSize = 30f;
			m_alzheimersQuestionMarks.ForceDraw = true;
			m_alzheimersQuestionMarks.Text = "?????";
			m_alzheimersQuestionMarks.Align = Types.TextAlign.Centre;
			m_alzheimersQuestionMarks.Position = new Vector2(660f, 360f - m_alzheimersQuestionMarks.Height / 2f);
		}
		private void FindRoomTitlePos(List<RoomObj> roomList, GameTypes.LevelType levelType, out Vector2 pos)
		{
			float num = 3.40282347E+38f;
			float num2 = -3.40282347E+38f;
			float num3 = 3.40282347E+38f;
			float num4 = -3.40282347E+38f;
			foreach (RoomObj current in roomList)
			{
				if (current.Name != "Boss" && (current.LevelType == levelType || (current.LevelType == GameTypes.LevelType.CASTLE && (current.Name == "Start" || current.Name == "CastleEntrance"))))
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
			m_mapDisplay.SetPlayer(player);
		}
		public override void LoadContent()
		{
			m_mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), Camera);
			m_mapDisplay.CameraOffset = new Vector2(660f, 360f);
			m_legend = new ObjContainer();
			m_legend.ForceDraw = true;
			SpriteObj spriteObj = new SpriteObj("TraitsScreenPlate_Sprite");
			m_legend.AddChild(spriteObj);
			spriteObj.Scale = new Vector2(0.75f, 0.58f);
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Align = Types.TextAlign.Centre;
			textObj.Position = new Vector2(m_legend.Width / 2 * spriteObj.ScaleX, m_legend.Bounds.Top + 10);
			textObj.Text = "Legend";
			textObj.FontSize = 12f;
			textObj.DropShadow = new Vector2(2f, 2f);
			textObj.TextureColor = new Color(213, 213, 173);
			m_legend.AddChild(textObj);
			m_legend.AnimationDelay = 0.0333333351f;
			m_legend.Position = new Vector2(1320 - m_legend.Width - 20, 720 - m_legend.Height - 20);
			SpriteObj spriteObj2 = new SpriteObj("MapPlayerIcon_Sprite");
			spriteObj2.Position = new Vector2(30f, 60f);
			spriteObj2.PlayAnimation(true);
			m_legend.AddChild(spriteObj2);
			int num = 30;
			SpriteObj spriteObj3 = new SpriteObj("MapBossIcon_Sprite");
			spriteObj3.Position = new Vector2(spriteObj2.X, spriteObj2.Y + num);
			spriteObj3.PlayAnimation(true);
			m_legend.AddChild(spriteObj3);
			SpriteObj spriteObj4 = new SpriteObj("MapLockedChestIcon_Sprite");
			spriteObj4.Position = new Vector2(spriteObj2.X, spriteObj3.Y + num);
			spriteObj4.PlayAnimation(true);
			m_legend.AddChild(spriteObj4);
			SpriteObj spriteObj5 = new SpriteObj("MapFairyChestIcon_Sprite");
			spriteObj5.Position = new Vector2(spriteObj2.X, spriteObj4.Y + num);
			spriteObj5.PlayAnimation(true);
			m_legend.AddChild(spriteObj5);
			SpriteObj spriteObj6 = new SpriteObj("MapChestUnlocked_Sprite");
			spriteObj6.Position = new Vector2(spriteObj2.X, spriteObj5.Y + num);
			m_legend.AddChild(spriteObj6);
			SpriteObj spriteObj7 = new SpriteObj("MapTeleporterIcon_Sprite");
			spriteObj7.Position = new Vector2(spriteObj2.X, spriteObj6.Y + num);
			spriteObj7.PlayAnimation(true);
			m_legend.AddChild(spriteObj7);
			SpriteObj spriteObj8 = new SpriteObj("MapBonusIcon_Sprite");
			spriteObj8.Position = new Vector2(spriteObj2.X, spriteObj7.Y + num);
			spriteObj8.PlayAnimation(true);
			m_legend.AddChild(spriteObj8);
			TextObj textObj2 = new TextObj(Game.JunicodeFont);
			textObj2.Position = new Vector2(spriteObj2.X + 50f, 55f);
			textObj2.Text = "You are here \nBoss location \nUnopened chest \nFairy chest \nOpened chest \nTeleporter \nBonus Room";
			textObj2.FontSize = 10f;
			textObj2.DropShadow = new Vector2(2f, 2f);
			m_legend.AddChild(textObj2);
			spriteObj2.X += 4f;
			spriteObj2.Y += 4f;
			m_titleText = new SpriteObj("TeleporterTitleText_Sprite");
			m_titleText.ForceDraw = true;
			m_titleText.X = 660f;
			m_titleText.Y = 72f;
			m_playerIcon = new ObjContainer("PlayerWalking_Character");
			m_playerIcon.Scale = new Vector2(0.6f, 0.6f);
			m_playerIcon.AnimationDelay = 0.1f;
			m_playerIcon.PlayAnimation(true);
			m_playerIcon.ForceDraw = true;
			m_playerIcon.OutlineWidth = 2;
			m_playerIcon.GetChildAt(1).TextureColor = Color.Red;
			m_playerIcon.GetChildAt(7).TextureColor = Color.Red;
			m_playerIcon.GetChildAt(8).TextureColor = Color.Red;
			m_playerIcon.GetChildAt(16).Visible = false;
			m_playerIcon.GetChildAt(5).Visible = false;
			m_playerIcon.GetChildAt(13).Visible = false;
			m_playerIcon.GetChildAt(0).Visible = false;
			m_playerIcon.GetChildAt(15).Visible = false;
			m_playerIcon.GetChildAt(14).Visible = false;
			m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			m_continueText.Text = "to close map";
			m_continueText.FontSize = 12f;
			m_continueText.ForceDraw = true;
			m_continueText.Position = new Vector2(50f, 200 - m_continueText.Height - 40);
			m_recentreText = new KeyIconTextObj(Game.JunicodeFont);
			m_recentreText.Text = "to re-center on player";
			m_recentreText.FontSize = 12f;
			m_recentreText.Position = new Vector2(m_continueText.X, 200 - m_continueText.Height - 80);
			m_recentreText.ForceDraw = true;
			m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			m_navigationText.Text = "to move map";
			m_navigationText.FontSize = 12f;
			m_navigationText.Position = new Vector2(m_continueText.X, 200 - m_continueText.Height - 120);
			m_navigationText.ForceDraw = true;
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			m_mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), Camera);
			base.ReinitializeRTs();
		}
		public void AddRooms(List<RoomObj> roomList)
		{
			m_mapDisplay.AddAllRooms(roomList);
		}
		public void RefreshMapChestIcons(RoomObj room)
		{
			m_mapDisplay.RefreshChestIcons(room);
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("Map_On");
			m_mapDisplay.CentreAroundPlayer();
			if (!IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
			{
				m_mapDisplay.DrawNothing = true;
			}
			else
			{
				m_mapDisplay.DrawNothing = false;
			}
			m_continueText.Text = "[Input:" + 9 + "]  to close map";
			m_recentreText.Text = "[Input:" + 0 + "]  to center on player";
			if (!InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				m_navigationText.Text = "Use arrow keys to move map";
			}
			else
			{
				m_navigationText.Text = "[Button:LeftStick] to move map";
			}
			if (IsTeleporter && m_teleporterList.Length > 0)
			{
				SpriteObj spriteObj = m_teleporterList[m_selectedTeleporter];
				m_playerIcon.Position = new Vector2(spriteObj.X + 7f, spriteObj.Y - 20f);
				m_mapDisplay.CentreAroundTeleporter(m_selectedTeleporter, false);
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
				float num = 5f;
				if (Game.GlobalInput.Pressed(16) || Game.GlobalInput.Pressed(17))
				{
					MapObj expr_7F_cp_0 = m_mapDisplay;
					expr_7F_cp_0.CameraOffset.Y = expr_7F_cp_0.CameraOffset.Y + num;
				}
				else if (Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19))
				{
					MapObj expr_B5_cp_0 = m_mapDisplay;
					expr_B5_cp_0.CameraOffset.Y = expr_B5_cp_0.CameraOffset.Y - num;
				}
				if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
				{
					MapObj expr_E9_cp_0 = m_mapDisplay;
					expr_E9_cp_0.CameraOffset.X = expr_E9_cp_0.CameraOffset.X + num;
				}
				else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
				{
					MapObj expr_11F_cp_0 = m_mapDisplay;
					expr_11F_cp_0.CameraOffset.X = expr_11F_cp_0.CameraOffset.X - num;
				}
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					m_mapDisplay.CentreAroundPlayer();
				}
			}
			else
			{
				int selectedTeleporter = m_selectedTeleporter;
				if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
				{
					m_selectedTeleporter++;
					if (m_selectedTeleporter >= m_teleporterList.Length)
					{
						m_selectedTeleporter = 0;
					}
				}
				else if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
				{
					m_selectedTeleporter--;
					if (m_selectedTeleporter < 0 && m_teleporterList.Length > 0)
					{
						m_selectedTeleporter = m_teleporterList.Length - 1;
					}
					else if (m_selectedTeleporter < 0 && m_teleporterList.Length <= 0)
					{
						m_selectedTeleporter = 0;
					}
				}
				if (selectedTeleporter != m_selectedTeleporter)
				{
					m_mapDisplay.CentreAroundTeleporter(m_selectedTeleporter, true);
				}
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					m_mapDisplay.TeleportPlayer(m_selectedTeleporter);
					(ScreenManager as RCScreenManager).HideCurrentScreen();
				}
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gameTime)
		{
			Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			m_mapDisplay.DrawRenderTargets(Camera);
			Camera.End();
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			Camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Draw((ScreenManager as RCScreenManager).GetLevelScreen().RenderTarget, Vector2.Zero, Color.White * 0.3f);
			m_mapDisplay.Draw(Camera);
			if (IsTeleporter && m_teleporterList.Length > 0)
			{
				m_titleText.Draw(Camera);
				SpriteObj spriteObj = m_teleporterList[m_selectedTeleporter];
				m_playerIcon.Position = new Vector2(spriteObj.X + 14f, spriteObj.Y - 20f);
				m_playerIcon.Draw(Camera);
			}
			if (!IsTeleporter)
			{
				m_recentreText.Draw(Camera);
				m_navigationText.Draw(Camera);
			}
			if (!IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
			{
				m_alzheimersQuestionMarks.Draw(Camera);
			}
			m_continueText.Draw(Camera);
			m_legend.Draw(Camera);
			Camera.End();
			base.Draw(gameTime);
		}
		public void AddAllIcons(List<RoomObj> roomList)
		{
			m_mapDisplay.AddAllIcons(roomList);
		}
		public override void DisposeRTs()
		{
			m_mapDisplay.DisposeRTs();
			base.DisposeRTs();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Map Screen");
				if (m_mapDisplay != null)
				{
					m_mapDisplay.Dispose();
				}
				m_mapDisplay = null;
				if (m_legend != null)
				{
					m_legend.Dispose();
				}
				m_legend = null;
				if (m_playerIcon != null)
				{
					m_playerIcon.Dispose();
				}
				m_playerIcon = null;
				if (m_teleporterList != null)
				{
					Array.Clear(m_teleporterList, 0, m_teleporterList.Length);
				}
				m_teleporterList = null;
				if (m_titleText != null)
				{
					m_titleText.Dispose();
				}
				m_titleText = null;
				if (m_continueText != null)
				{
					m_continueText.Dispose();
				}
				m_continueText = null;
				if (m_recentreText != null)
				{
					m_recentreText.Dispose();
				}
				m_recentreText = null;
				if (m_navigationText != null)
				{
					m_navigationText.Dispose();
				}
				m_navigationText = null;
				m_alzheimersQuestionMarks.Dispose();
				m_alzheimersQuestionMarks = null;
				base.Dispose();
			}
		}
	}
}
