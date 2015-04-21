using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class PauseScreen : Screen
	{
		private class PauseInfoObj : ObjContainer
		{
			private List<TextObj> m_textList;
			private List<TextObj> m_textDataList;
			private int m_arrayIndex;
			private ObjContainer m_namePlate;
			private TextObj m_name;
			public PauseInfoObj() : base("GameOverStatPlate_Character")
			{
				base.ForceDraw = true;
				this.m_textList = new List<TextObj>();
				this.m_textDataList = new List<TextObj>();
				this.m_namePlate = new ObjContainer("DialogBox_Character");
				this.m_namePlate.ForceDraw = true;
				this.m_name = new TextObj(Game.JunicodeFont);
				this.m_name.Align = Types.TextAlign.Centre;
				this.m_name.Text = "<noname>";
				this.m_name.FontSize = 8f;
				this.m_name.Y -= 45f;
				this.m_name.OverrideParentScale = true;
				this.m_name.DropShadow = new Vector2(2f, 2f);
				this.m_namePlate.AddChild(this.m_name);
			}
			public void SetName(string name)
			{
				this.m_name.Text = name;
				this.m_namePlate.Scale = Vector2.One;
				this.m_namePlate.Scale = new Vector2(((float)this.m_name.Width + 70f) / (float)this.m_namePlate.Width, ((float)this.m_name.Height + 20f) / (float)this.m_namePlate.Height);
			}
			public void SetNamePosition(Vector2 pos)
			{
				this.m_namePlate.Position = pos;
			}
			public void AddItem(string title, string data)
			{
				TextObj textObj;
				if (this.m_textList.Count <= this.m_arrayIndex)
				{
					textObj = new TextObj(Game.JunicodeFont);
				}
				else
				{
					textObj = this.m_textList[this.m_arrayIndex];
				}
				textObj.FontSize = 8f;
				textObj.Text = title;
				textObj.Align = Types.TextAlign.Right;
				textObj.Y = (float)(this._objectList[0].Bounds.Top + textObj.Height + this.m_arrayIndex * 20);
				textObj.DropShadow = new Vector2(2f, 2f);
				if (this.m_textList.Count <= this.m_arrayIndex)
				{
					this.AddChild(textObj);
					this.m_textList.Add(textObj);
				}
				TextObj textObj2;
				if (this.m_textDataList.Count <= this.m_arrayIndex)
				{
					textObj2 = new TextObj(Game.JunicodeFont);
				}
				else
				{
					textObj2 = this.m_textDataList[this.m_arrayIndex];
				}
				textObj2.FontSize = 8f;
				textObj2.Text = data;
				textObj2.Y = textObj.Y;
				textObj2.DropShadow = new Vector2(2f, 2f);
				if (this.m_textDataList.Count <= this.m_arrayIndex)
				{
					this.AddChild(textObj2);
					this.m_textDataList.Add(textObj2);
				}
				this.m_arrayIndex++;
			}
			public void ResizePlate()
			{
				this._objectList[0].ScaleY = 1f;
				this._objectList[0].ScaleY = (float)(this._objectList[1].Height * (this._objectList.Count + 1) / 2) / (float)this._objectList[0].Height;
				int num = 0;
				foreach (TextObj current in this.m_textList)
				{
					if (current.Width > num)
					{
						num = current.Width;
					}
				}
				int num2 = 0;
				foreach (TextObj current2 in this.m_textDataList)
				{
					if (current2.Width > num2)
					{
						num2 = current2.Width;
					}
				}
				this._objectList[0].ScaleX = 1f;
				this._objectList[0].ScaleX = (float)(num + num2 + 50) / (float)this._objectList[0].Width;
				int num3 = (int)(-((float)this._objectList[0].Width / 2f) + (float)num) + 25;
				int num4 = this._objectList[0].Height / (this.m_textList.Count + 2);
				for (int i = 0; i < this.m_textList.Count; i++)
				{
					this.m_textList[i].X = (float)num3;
					this.m_textList[i].Y = (float)(this._objectList[0].Bounds.Top + num4 + num4 * i);
					this.m_textDataList[i].X = (float)num3;
					this.m_textDataList[i].Y = this.m_textList[i].Y;
				}
			}
			public override void Draw(Camera2D camera)
			{
				if (base.Visible)
				{
					this.m_namePlate.Draw(camera);
					this.m_name.Draw(camera);
				}
				base.Draw(camera);
			}
			public void Reset()
			{
				foreach (TextObj current in this.m_textList)
				{
					current.Text = "";
				}
				foreach (TextObj current2 in this.m_textDataList)
				{
					current2.Text = "";
				}
				this.m_arrayIndex = 0;
			}
			public override void Dispose()
			{
				if (!base.IsDisposed)
				{
					this.m_textList.Clear();
					this.m_textList = null;
					this.m_textDataList.Clear();
					this.m_textDataList = null;
					this.m_namePlate.Dispose();
					this.m_namePlate = null;
					this.m_name = null;
					base.Dispose();
				}
			}
		}
		private SpriteObj m_titleText;
		private List<PauseScreen.PauseInfoObj> m_infoObjList;
		private SpriteObj m_profileCard;
		private SpriteObj m_optionsIcon;
		private KeyIconTextObj m_profileCardKey;
		private KeyIconTextObj m_optionsKey;
		private float m_inputDelay;
		public PauseScreen()
		{
			base.DrawIfCovered = true;
		}
		public override void LoadContent()
		{
			this.m_titleText = new SpriteObj("GamePausedTitleText_Sprite");
			this.m_titleText.X = 660f;
			this.m_titleText.Y = 72f;
			this.m_titleText.ForceDraw = true;
			this.m_infoObjList = new List<PauseScreen.PauseInfoObj>();
			this.m_infoObjList.Add(new PauseScreen.PauseInfoObj());
			this.m_profileCard = new SpriteObj("TitleProfileCard_Sprite");
			this.m_profileCard.OutlineWidth = 2;
			this.m_profileCard.Scale = new Vector2(2f, 2f);
			this.m_profileCard.Position = new Vector2((float)this.m_profileCard.Width, (float)(720 - this.m_profileCard.Height));
			this.m_profileCard.ForceDraw = true;
			this.m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
			this.m_optionsIcon.Scale = new Vector2(2f, 2f);
			this.m_optionsIcon.OutlineWidth = this.m_profileCard.OutlineWidth;
			this.m_optionsIcon.Position = new Vector2((float)(1320 - this.m_optionsIcon.Width * 2 + 120), this.m_profileCard.Y);
			this.m_optionsIcon.ForceDraw = true;
			this.m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_profileCardKey.Align = Types.TextAlign.Centre;
			this.m_profileCardKey.FontSize = 12f;
			this.m_profileCardKey.Text = "[Input:" + 7 + "]";
			this.m_profileCardKey.Position = new Vector2(this.m_profileCard.X, (float)(this.m_profileCard.Bounds.Top - this.m_profileCardKey.Height - 10));
			this.m_profileCardKey.ForceDraw = true;
			this.m_optionsKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_optionsKey.Align = Types.TextAlign.Centre;
			this.m_optionsKey.FontSize = 12f;
			this.m_optionsKey.Text = "[Input:" + 4 + "]";
			this.m_optionsKey.Position = new Vector2(this.m_optionsIcon.X, (float)(this.m_optionsIcon.Bounds.Top - this.m_optionsKey.Height - 10));
			this.m_optionsKey.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.UpdatePauseScreenIcons();
			this.m_inputDelay = 0.5f;
			if (SoundManager.IsMusicPlaying)
			{
				SoundManager.PauseMusic();
			}
			SoundManager.PlaySound("Pause_Toggle");
			ProceduralLevelScreen levelScreen = (base.ScreenManager as RCScreenManager).GetLevelScreen();
			foreach (PauseScreen.PauseInfoObj current in this.m_infoObjList)
			{
				current.Reset();
				current.Visible = false;
			}
			PlayerObj player = (base.ScreenManager as RCScreenManager).Player;
			PauseScreen.PauseInfoObj pauseInfoObj = this.m_infoObjList[0];
			pauseInfoObj.Visible = true;
			pauseInfoObj.AddItem("Class: ", ClassType.ToString(Game.PlayerStats.Class, Game.PlayerStats.IsFemale));
			pauseInfoObj.AddItem("Strength: ", player.Damage.ToString());
			pauseInfoObj.AddItem("Magic: ", player.TotalMagicDamage.ToString());
			pauseInfoObj.AddItem("Armor: ", player.TotalArmor.ToString());
			pauseInfoObj.ResizePlate();
			pauseInfoObj.X = player.X - base.Camera.TopLeftCorner.X;
			pauseInfoObj.Y = (float)player.Bounds.Bottom - base.Camera.TopLeftCorner.Y + (float)pauseInfoObj.Height / 2f - 20f;
			if (!Game.PlayerStats.TutorialComplete)
			{
				pauseInfoObj.SetName("?????");
			}
			else
			{
				pauseInfoObj.SetName(Game.PlayerStats.PlayerName);
			}
			pauseInfoObj.SetNamePosition(new Vector2(pauseInfoObj.X, (float)player.Bounds.Top - base.Camera.TopLeftCorner.Y - 40f));
			pauseInfoObj.Visible = player.Visible;
			int num = this.m_infoObjList.Count - 1;
			for (int i = num; i < levelScreen.CurrentRoom.EnemyList.Count + levelScreen.CurrentRoom.TempEnemyList.Count; i++)
			{
				this.m_infoObjList.Add(new PauseScreen.PauseInfoObj
				{
					Visible = false
				});
			}
			for (int j = 1; j < levelScreen.CurrentRoom.EnemyList.Count + 1; j++)
			{
				EnemyObj enemyObj = levelScreen.CurrentRoom.EnemyList[j - 1];
				if (!enemyObj.NonKillable && !enemyObj.IsKilled && enemyObj.Visible)
				{
					PauseScreen.PauseInfoObj pauseInfoObj2 = this.m_infoObjList[j];
					pauseInfoObj2.Visible = true;
					if (!LevelEV.CREATE_RETAIL_VERSION)
					{
						pauseInfoObj2.AddItem("Level: ", enemyObj.Level.ToString());
					}
					else
					{
						pauseInfoObj2.AddItem("Level: ", ((int)((float)enemyObj.Level * 2.75f)).ToString());
					}
					pauseInfoObj2.AddItem("Strength: ", enemyObj.Damage.ToString());
					pauseInfoObj2.AddItem("Health: ", enemyObj.CurrentHealth + "/" + enemyObj.MaxHealth);
					pauseInfoObj2.ResizePlate();
					pauseInfoObj2.X = enemyObj.X - base.Camera.TopLeftCorner.X;
					pauseInfoObj2.Y = (float)enemyObj.Bounds.Bottom - base.Camera.TopLeftCorner.Y + (float)pauseInfoObj2.Height / 2f - 20f;
					pauseInfoObj2.SetName(enemyObj.Name);
					pauseInfoObj2.SetNamePosition(new Vector2(pauseInfoObj2.X, (float)enemyObj.Bounds.Top - base.Camera.TopLeftCorner.Y - 40f));
				}
			}
			int count = levelScreen.CurrentRoom.EnemyList.Count;
			for (int k = 0; k < levelScreen.CurrentRoom.TempEnemyList.Count; k++)
			{
				EnemyObj enemyObj2 = levelScreen.CurrentRoom.TempEnemyList[k];
				if (!enemyObj2.NonKillable && !enemyObj2.IsKilled)
				{
					PauseScreen.PauseInfoObj pauseInfoObj3 = this.m_infoObjList[k + 1 + count];
					pauseInfoObj3.Visible = true;
					if (!LevelEV.CREATE_RETAIL_VERSION)
					{
						pauseInfoObj3.AddItem("Level: ", enemyObj2.Level.ToString());
					}
					else
					{
						pauseInfoObj3.AddItem("Level: ", ((int)((float)enemyObj2.Level * 2.75f)).ToString());
					}
					pauseInfoObj3.AddItem("Strength: ", enemyObj2.Damage.ToString());
					pauseInfoObj3.AddItem("Health: ", enemyObj2.CurrentHealth + "/" + enemyObj2.MaxHealth);
					pauseInfoObj3.ResizePlate();
					pauseInfoObj3.X = enemyObj2.X - base.Camera.TopLeftCorner.X;
					pauseInfoObj3.Y = (float)enemyObj2.Bounds.Bottom - base.Camera.TopLeftCorner.Y + (float)pauseInfoObj3.Height / 2f - 20f;
					pauseInfoObj3.SetName(enemyObj2.Name);
					pauseInfoObj3.SetNamePosition(new Vector2(pauseInfoObj3.X, (float)enemyObj2.Bounds.Top - base.Camera.TopLeftCorner.Y - 40f));
				}
			}
			base.OnEnter();
		}
		public void UpdatePauseScreenIcons()
		{
			this.m_profileCardKey.Text = "[Input:" + 7 + "]";
			this.m_optionsKey.Text = "[Input:" + 4 + "]";
		}
		public override void OnExit()
		{
			if (SoundManager.IsMusicPaused)
			{
				SoundManager.ResumeMusic();
			}
			SoundManager.PlaySound("Resume_Toggle");
			foreach (PauseScreen.PauseInfoObj current in this.m_infoObjList)
			{
				current.Visible = false;
			}
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (this.m_inputDelay <= 0f)
			{
				if (Game.GlobalInput.JustPressed(7) && Game.PlayerStats.TutorialComplete)
				{
					(base.ScreenManager as RCScreenManager).DisplayScreen(17, true, null);
				}
				if (Game.GlobalInput.JustPressed(4))
				{
					List<object> list = new List<object>();
					list.Add(false);
					(base.ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
				}
				if (Game.GlobalInput.JustPressed(8))
				{
					(base.ScreenManager as RCScreenManager).GetLevelScreen().UnpauseScreen();
					(base.ScreenManager as RCScreenManager).HideCurrentScreen();
				}
				base.HandleInput();
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_inputDelay > 0f)
			{
				this.m_inputDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			this.m_titleText.Draw(base.Camera);
			foreach (PauseScreen.PauseInfoObj current in this.m_infoObjList)
			{
				current.Draw(base.Camera);
			}
			if (Game.PlayerStats.TutorialComplete)
			{
				this.m_profileCardKey.Draw(base.Camera);
			}
			this.m_optionsKey.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_optionsIcon.Draw(base.Camera);
			if (Game.PlayerStats.TutorialComplete)
			{
				this.m_profileCard.Draw(base.Camera);
			}
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Pause Screen");
				foreach (PauseScreen.PauseInfoObj current in this.m_infoObjList)
				{
					current.Dispose();
				}
				this.m_infoObjList.Clear();
				this.m_infoObjList = null;
				this.m_titleText.Dispose();
				this.m_titleText = null;
				this.m_profileCard.Dispose();
				this.m_profileCard = null;
				this.m_optionsIcon.Dispose();
				this.m_optionsIcon = null;
				this.m_profileCardKey.Dispose();
				this.m_profileCardKey = null;
				this.m_optionsKey.Dispose();
				this.m_optionsKey = null;
				base.Dispose();
			}
		}
	}
}
