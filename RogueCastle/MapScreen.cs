using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
				return this.m_isTeleporter;
			}
			set
			{
				this.m_mapDisplay.DrawTeleportersOnly = value;
				this.m_isTeleporter = value;
				if (value)
				{
					if (this.m_teleporterList != null)
					{
						Array.Clear(this.m_teleporterList, 0, this.m_teleporterList.Length);
					}
					this.m_teleporterList = this.m_mapDisplay.TeleporterList();
				}
			}
		}
		public MapScreen(ProceduralLevelScreen level)
		{
			this.m_mapDisplay = new MapObj(false, level);
			this.m_alzheimersQuestionMarks = new TextObj(Game.JunicodeLargeFont);
			this.m_alzheimersQuestionMarks.FontSize = 30f;
			this.m_alzheimersQuestionMarks.ForceDraw = true;
			this.m_alzheimersQuestionMarks.Text = "?????";
			this.m_alzheimersQuestionMarks.Align = Types.TextAlign.Centre;
			this.m_alzheimersQuestionMarks.Position = new Vector2(660f, 360f - (float)this.m_alzheimersQuestionMarks.Height / 2f);
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
					if (current.X + (float)current.Width > num2)
					{
						num2 = current.X + (float)current.Width;
					}
					if (current.Y < num3)
					{
						num3 = current.Y;
					}
					if (current.Y + (float)current.Height > num4)
					{
						num4 = current.Y + (float)current.Height;
					}
				}
			}
			pos = new Vector2((num2 + num) / 2f, (num4 + num3) / 2f);
			pos = new Vector2(pos.X / 1320f * 60f, pos.Y / 720f * 32f);
		}
		public void SetPlayer(PlayerObj player)
		{
			this.m_mapDisplay.SetPlayer(player);
		}
		public override void LoadContent()
		{
			this.m_mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), base.Camera);
			this.m_mapDisplay.CameraOffset = new Vector2(660f, 360f);
			this.m_legend = new ObjContainer();
			this.m_legend.ForceDraw = true;
			SpriteObj spriteObj = new SpriteObj("TraitsScreenPlate_Sprite");
			this.m_legend.AddChild(spriteObj);
			spriteObj.Scale = new Vector2(0.75f, 0.58f);
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Align = Types.TextAlign.Centre;
			textObj.Position = new Vector2((float)(this.m_legend.Width / 2) * spriteObj.ScaleX, (float)(this.m_legend.Bounds.Top + 10));
			textObj.Text = "Legend";
			textObj.FontSize = 12f;
			textObj.DropShadow = new Vector2(2f, 2f);
			textObj.TextureColor = new Color(213, 213, 173);
			this.m_legend.AddChild(textObj);
			this.m_legend.AnimationDelay = 0.0333333351f;
			this.m_legend.Position = new Vector2((float)(1320 - this.m_legend.Width - 20), (float)(720 - this.m_legend.Height - 20));
			SpriteObj spriteObj2 = new SpriteObj("MapPlayerIcon_Sprite");
			spriteObj2.Position = new Vector2(30f, 60f);
			spriteObj2.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj2);
			int num = 30;
			SpriteObj spriteObj3 = new SpriteObj("MapBossIcon_Sprite");
			spriteObj3.Position = new Vector2(spriteObj2.X, spriteObj2.Y + (float)num);
			spriteObj3.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj3);
			SpriteObj spriteObj4 = new SpriteObj("MapLockedChestIcon_Sprite");
			spriteObj4.Position = new Vector2(spriteObj2.X, spriteObj3.Y + (float)num);
			spriteObj4.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj4);
			SpriteObj spriteObj5 = new SpriteObj("MapFairyChestIcon_Sprite");
			spriteObj5.Position = new Vector2(spriteObj2.X, spriteObj4.Y + (float)num);
			spriteObj5.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj5);
			SpriteObj spriteObj6 = new SpriteObj("MapChestUnlocked_Sprite");
			spriteObj6.Position = new Vector2(spriteObj2.X, spriteObj5.Y + (float)num);
			this.m_legend.AddChild(spriteObj6);
			SpriteObj spriteObj7 = new SpriteObj("MapTeleporterIcon_Sprite");
			spriteObj7.Position = new Vector2(spriteObj2.X, spriteObj6.Y + (float)num);
			spriteObj7.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj7);
			SpriteObj spriteObj8 = new SpriteObj("MapBonusIcon_Sprite");
			spriteObj8.Position = new Vector2(spriteObj2.X, spriteObj7.Y + (float)num);
			spriteObj8.PlayAnimation(true);
			this.m_legend.AddChild(spriteObj8);
			TextObj textObj2 = new TextObj(Game.JunicodeFont);
			textObj2.Position = new Vector2(spriteObj2.X + 50f, 55f);
			textObj2.Text = "You are here \nBoss location \nUnopened chest \nFairy chest \nOpened chest \nTeleporter \nBonus Room";
			textObj2.FontSize = 10f;
			textObj2.DropShadow = new Vector2(2f, 2f);
			this.m_legend.AddChild(textObj2);
			spriteObj2.X += 4f;
			spriteObj2.Y += 4f;
			this.m_titleText = new SpriteObj("TeleporterTitleText_Sprite");
			this.m_titleText.ForceDraw = true;
			this.m_titleText.X = 660f;
			this.m_titleText.Y = 72f;
			this.m_playerIcon = new ObjContainer("PlayerWalking_Character");
			this.m_playerIcon.Scale = new Vector2(0.6f, 0.6f);
			this.m_playerIcon.AnimationDelay = 0.1f;
			this.m_playerIcon.PlayAnimation(true);
			this.m_playerIcon.ForceDraw = true;
			this.m_playerIcon.OutlineWidth = 2;
			this.m_playerIcon.GetChildAt(1).TextureColor = Color.Red;
			this.m_playerIcon.GetChildAt(7).TextureColor = Color.Red;
			this.m_playerIcon.GetChildAt(8).TextureColor = Color.Red;
			this.m_playerIcon.GetChildAt(16).Visible = false;
			this.m_playerIcon.GetChildAt(5).Visible = false;
			this.m_playerIcon.GetChildAt(13).Visible = false;
			this.m_playerIcon.GetChildAt(0).Visible = false;
			this.m_playerIcon.GetChildAt(15).Visible = false;
			this.m_playerIcon.GetChildAt(14).Visible = false;
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.Text = "to close map";
			this.m_continueText.FontSize = 12f;
			this.m_continueText.ForceDraw = true;
			this.m_continueText.Position = new Vector2(50f, (float)(200 - this.m_continueText.Height - 40));
			this.m_recentreText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_recentreText.Text = "to re-center on player";
			this.m_recentreText.FontSize = 12f;
			this.m_recentreText.Position = new Vector2(this.m_continueText.X, (float)(200 - this.m_continueText.Height - 80));
			this.m_recentreText.ForceDraw = true;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Text = "to move map";
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.Position = new Vector2(this.m_continueText.X, (float)(200 - this.m_continueText.Height - 120));
			this.m_navigationText.ForceDraw = true;
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			this.m_mapDisplay.InitializeAlphaMap(new Rectangle(50, 50, 1220, 620), base.Camera);
			base.ReinitializeRTs();
		}
		public void AddRooms(List<RoomObj> roomList)
		{
			this.m_mapDisplay.AddAllRooms(roomList);
		}
		public void RefreshMapChestIcons(RoomObj room)
		{
			this.m_mapDisplay.RefreshChestIcons(room);
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("Map_On");
			this.m_mapDisplay.CentreAroundPlayer();
			if (!this.IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
			{
				this.m_mapDisplay.DrawNothing = true;
			}
			else
			{
				this.m_mapDisplay.DrawNothing = false;
			}
			this.m_continueText.Text = "[Input:" + 9 + "]  to close map";
			this.m_recentreText.Text = "[Input:" + 0 + "]  to center on player";
			if (!InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_navigationText.Text = "Use arrow keys to move map";
			}
			else
			{
				this.m_navigationText.Text = "[Button:LeftStick] to move map";
			}
			if (this.IsTeleporter && this.m_teleporterList.Length > 0)
			{
				SpriteObj spriteObj = this.m_teleporterList[this.m_selectedTeleporter];
				this.m_playerIcon.Position = new Vector2(spriteObj.X + 7f, spriteObj.Y - 20f);
				this.m_mapDisplay.CentreAroundTeleporter(this.m_selectedTeleporter, false);
			}
			base.OnEnter();
		}
		public override void OnExit()
		{
			SoundManager.PlaySound("Map_Off");
			this.IsTeleporter = false;
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(9) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				Game.ScreenManager.Player.UnlockControls();
				(base.ScreenManager as RCScreenManager).HideCurrentScreen();
			}
			if (!this.IsTeleporter)
			{
				float num = 5f;
				if (Game.GlobalInput.Pressed(16) || Game.GlobalInput.Pressed(17))
				{
					MapObj expr_7F_cp_0 = this.m_mapDisplay;
					expr_7F_cp_0.CameraOffset.Y = expr_7F_cp_0.CameraOffset.Y + num;
				}
				else if (Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19))
				{
					MapObj expr_B5_cp_0 = this.m_mapDisplay;
					expr_B5_cp_0.CameraOffset.Y = expr_B5_cp_0.CameraOffset.Y - num;
				}
				if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
				{
					MapObj expr_E9_cp_0 = this.m_mapDisplay;
					expr_E9_cp_0.CameraOffset.X = expr_E9_cp_0.CameraOffset.X + num;
				}
				else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
				{
					MapObj expr_11F_cp_0 = this.m_mapDisplay;
					expr_11F_cp_0.CameraOffset.X = expr_11F_cp_0.CameraOffset.X - num;
				}
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					this.m_mapDisplay.CentreAroundPlayer();
				}
			}
			else
			{
				int selectedTeleporter = this.m_selectedTeleporter;
				if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
				{
					this.m_selectedTeleporter++;
					if (this.m_selectedTeleporter >= this.m_teleporterList.Length)
					{
						this.m_selectedTeleporter = 0;
					}
				}
				else if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
				{
					this.m_selectedTeleporter--;
					if (this.m_selectedTeleporter < 0 && this.m_teleporterList.Length > 0)
					{
						this.m_selectedTeleporter = this.m_teleporterList.Length - 1;
					}
					else if (this.m_selectedTeleporter < 0 && this.m_teleporterList.Length <= 0)
					{
						this.m_selectedTeleporter = 0;
					}
				}
				if (selectedTeleporter != this.m_selectedTeleporter)
				{
					this.m_mapDisplay.CentreAroundTeleporter(this.m_selectedTeleporter, true);
				}
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					this.m_mapDisplay.TeleportPlayer(this.m_selectedTeleporter);
					(base.ScreenManager as RCScreenManager).HideCurrentScreen();
				}
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.m_mapDisplay.DrawRenderTargets(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			base.Camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Draw((base.ScreenManager as RCScreenManager).GetLevelScreen().RenderTarget, Vector2.Zero, Color.White * 0.3f);
			this.m_mapDisplay.Draw(base.Camera);
			if (this.IsTeleporter && this.m_teleporterList.Length > 0)
			{
				this.m_titleText.Draw(base.Camera);
				SpriteObj spriteObj = this.m_teleporterList[this.m_selectedTeleporter];
				this.m_playerIcon.Position = new Vector2(spriteObj.X + 14f, spriteObj.Y - 20f);
				this.m_playerIcon.Draw(base.Camera);
			}
			if (!this.IsTeleporter)
			{
				this.m_recentreText.Draw(base.Camera);
				this.m_navigationText.Draw(base.Camera);
			}
			if (!this.IsTeleporter && (Game.PlayerStats.Traits.X == 11f || Game.PlayerStats.Traits.Y == 11f))
			{
				this.m_alzheimersQuestionMarks.Draw(base.Camera);
			}
			this.m_continueText.Draw(base.Camera);
			this.m_legend.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public void AddAllIcons(List<RoomObj> roomList)
		{
			this.m_mapDisplay.AddAllIcons(roomList);
		}
		public override void DisposeRTs()
		{
			this.m_mapDisplay.DisposeRTs();
			base.DisposeRTs();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Map Screen");
				if (this.m_mapDisplay != null)
				{
					this.m_mapDisplay.Dispose();
				}
				this.m_mapDisplay = null;
				if (this.m_legend != null)
				{
					this.m_legend.Dispose();
				}
				this.m_legend = null;
				if (this.m_playerIcon != null)
				{
					this.m_playerIcon.Dispose();
				}
				this.m_playerIcon = null;
				if (this.m_teleporterList != null)
				{
					Array.Clear(this.m_teleporterList, 0, this.m_teleporterList.Length);
				}
				this.m_teleporterList = null;
				if (this.m_titleText != null)
				{
					this.m_titleText.Dispose();
				}
				this.m_titleText = null;
				if (this.m_continueText != null)
				{
					this.m_continueText.Dispose();
				}
				this.m_continueText = null;
				if (this.m_recentreText != null)
				{
					this.m_recentreText.Dispose();
				}
				this.m_recentreText = null;
				if (this.m_navigationText != null)
				{
					this.m_navigationText.Dispose();
				}
				this.m_navigationText = null;
				this.m_alzheimersQuestionMarks.Dispose();
				this.m_alzheimersQuestionMarks = null;
				base.Dispose();
			}
		}
	}
}
