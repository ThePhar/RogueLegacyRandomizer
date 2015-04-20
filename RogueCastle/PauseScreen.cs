/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
				m_namePlate.Scale = new Vector2((m_name.Width + 70f) / m_namePlate.Width, (m_name.Height + 20f) / m_namePlate.Height);
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
				_objectList[0].ScaleY = _objectList[1].Height * (_objectList.Count + 1) / 2 / (float)_objectList[0].Height;
				int num = 0;
				foreach (TextObj current in m_textList)
				{
					if (current.Width > num)
					{
						num = current.Width;
					}
				}
				int num2 = 0;
				foreach (TextObj current2 in m_textDataList)
				{
					if (current2.Width > num2)
					{
						num2 = current2.Width;
					}
				}
				_objectList[0].ScaleX = 1f;
				_objectList[0].ScaleX = (num + num2 + 50) / (float)_objectList[0].Width;
				int num3 = (int)(-(_objectList[0].Width / 2f) + num) + 25;
				int num4 = _objectList[0].Height / (m_textList.Count + 2);
				for (int i = 0; i < m_textList.Count; i++)
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
				foreach (TextObj current in m_textList)
				{
					current.Text = "";
				}
				foreach (TextObj current2 in m_textDataList)
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
		private SpriteObj m_titleText;
		private List<PauseInfoObj> m_infoObjList;
		private SpriteObj m_profileCard;
		private SpriteObj m_optionsIcon;
		private KeyIconTextObj m_profileCardKey;
		private KeyIconTextObj m_optionsKey;
		private float m_inputDelay;
		public PauseScreen()
		{
			DrawIfCovered = true;
		}
		public override void LoadContent()
		{
			m_titleText = new SpriteObj("GamePausedTitleText_Sprite");
			m_titleText.X = 660f;
			m_titleText.Y = 72f;
			m_titleText.ForceDraw = true;
			m_infoObjList = new List<PauseInfoObj>();
			m_infoObjList.Add(new PauseInfoObj());
			m_profileCard = new SpriteObj("TitleProfileCard_Sprite");
			m_profileCard.OutlineWidth = 2;
			m_profileCard.Scale = new Vector2(2f, 2f);
			m_profileCard.Position = new Vector2(m_profileCard.Width, 720 - m_profileCard.Height);
			m_profileCard.ForceDraw = true;
			m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
			m_optionsIcon.Scale = new Vector2(2f, 2f);
			m_optionsIcon.OutlineWidth = m_profileCard.OutlineWidth;
			m_optionsIcon.Position = new Vector2(1320 - m_optionsIcon.Width * 2 + 120, m_profileCard.Y);
			m_optionsIcon.ForceDraw = true;
			m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
			m_profileCardKey.Align = Types.TextAlign.Centre;
			m_profileCardKey.FontSize = 12f;
			m_profileCardKey.Text = "[Input:" + 7 + "]";
			m_profileCardKey.Position = new Vector2(m_profileCard.X, m_profileCard.Bounds.Top - m_profileCardKey.Height - 10);
			m_profileCardKey.ForceDraw = true;
			m_optionsKey = new KeyIconTextObj(Game.JunicodeFont);
			m_optionsKey.Align = Types.TextAlign.Centre;
			m_optionsKey.FontSize = 12f;
			m_optionsKey.Text = "[Input:" + 4 + "]";
			m_optionsKey.Position = new Vector2(m_optionsIcon.X, m_optionsIcon.Bounds.Top - m_optionsKey.Height - 10);
			m_optionsKey.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			UpdatePauseScreenIcons();
			m_inputDelay = 0.5f;
			if (SoundManager.IsMusicPlaying)
			{
				SoundManager.PauseMusic();
			}
			SoundManager.PlaySound("Pause_Toggle");
			ProceduralLevelScreen levelScreen = (ScreenManager as RCScreenManager).GetLevelScreen();
			foreach (PauseInfoObj current in m_infoObjList)
			{
				current.Reset();
				current.Visible = false;
			}
			PlayerObj player = (ScreenManager as RCScreenManager).Player;
			PauseInfoObj pauseInfoObj = m_infoObjList[0];
			pauseInfoObj.Visible = true;
			pauseInfoObj.AddItem("Class: ", ClassType.ToString(Game.PlayerStats.Class, Game.PlayerStats.IsFemale));
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
			int num = m_infoObjList.Count - 1;
			for (int i = num; i < levelScreen.CurrentRoom.EnemyList.Count + levelScreen.CurrentRoom.TempEnemyList.Count; i++)
			{
				m_infoObjList.Add(new PauseInfoObj
				{
					Visible = false
				});
			}
			for (int j = 1; j < levelScreen.CurrentRoom.EnemyList.Count + 1; j++)
			{
				EnemyObj enemyObj = levelScreen.CurrentRoom.EnemyList[j - 1];
				if (!enemyObj.NonKillable && !enemyObj.IsKilled && enemyObj.Visible)
				{
					PauseInfoObj pauseInfoObj2 = m_infoObjList[j];
					pauseInfoObj2.Visible = true;
					if (!LevelEV.CREATE_RETAIL_VERSION)
					{
						pauseInfoObj2.AddItem("Level: ", enemyObj.Level.ToString());
					}
					else
					{
						pauseInfoObj2.AddItem("Level: ", ((int)(enemyObj.Level * 2.75f)).ToString());
					}
					pauseInfoObj2.AddItem("Strength: ", enemyObj.Damage.ToString());
					pauseInfoObj2.AddItem("Health: ", enemyObj.CurrentHealth + "/" + enemyObj.MaxHealth);
					pauseInfoObj2.ResizePlate();
					pauseInfoObj2.X = enemyObj.X - Camera.TopLeftCorner.X;
					pauseInfoObj2.Y = enemyObj.Bounds.Bottom - Camera.TopLeftCorner.Y + pauseInfoObj2.Height / 2f - 20f;
					pauseInfoObj2.SetName(enemyObj.Name);
					pauseInfoObj2.SetNamePosition(new Vector2(pauseInfoObj2.X, enemyObj.Bounds.Top - Camera.TopLeftCorner.Y - 40f));
				}
			}
			int count = levelScreen.CurrentRoom.EnemyList.Count;
			for (int k = 0; k < levelScreen.CurrentRoom.TempEnemyList.Count; k++)
			{
				EnemyObj enemyObj2 = levelScreen.CurrentRoom.TempEnemyList[k];
				if (!enemyObj2.NonKillable && !enemyObj2.IsKilled)
				{
					PauseInfoObj pauseInfoObj3 = m_infoObjList[k + 1 + count];
					pauseInfoObj3.Visible = true;
					if (!LevelEV.CREATE_RETAIL_VERSION)
					{
						pauseInfoObj3.AddItem("Level: ", enemyObj2.Level.ToString());
					}
					else
					{
						pauseInfoObj3.AddItem("Level: ", ((int)(enemyObj2.Level * 2.75f)).ToString());
					}
					pauseInfoObj3.AddItem("Strength: ", enemyObj2.Damage.ToString());
					pauseInfoObj3.AddItem("Health: ", enemyObj2.CurrentHealth + "/" + enemyObj2.MaxHealth);
					pauseInfoObj3.ResizePlate();
					pauseInfoObj3.X = enemyObj2.X - Camera.TopLeftCorner.X;
					pauseInfoObj3.Y = enemyObj2.Bounds.Bottom - Camera.TopLeftCorner.Y + pauseInfoObj3.Height / 2f - 20f;
					pauseInfoObj3.SetName(enemyObj2.Name);
					pauseInfoObj3.SetNamePosition(new Vector2(pauseInfoObj3.X, enemyObj2.Bounds.Top - Camera.TopLeftCorner.Y - 40f));
				}
			}
			base.OnEnter();
		}
		public void UpdatePauseScreenIcons()
		{
			m_profileCardKey.Text = "[Input:" + 7 + "]";
			m_optionsKey.Text = "[Input:" + 4 + "]";
		}
		public override void OnExit()
		{
			if (SoundManager.IsMusicPaused)
			{
				SoundManager.ResumeMusic();
			}
			SoundManager.PlaySound("Resume_Toggle");
			foreach (PauseInfoObj current in m_infoObjList)
			{
				current.Visible = false;
			}
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (m_inputDelay <= 0f)
			{
				if (Game.GlobalInput.JustPressed(7) && Game.PlayerStats.TutorialComplete)
				{
					(ScreenManager as RCScreenManager).DisplayScreen(17, true, null);
				}
				if (Game.GlobalInput.JustPressed(4))
				{
					List<object> list = new List<object>();
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
			if (m_inputDelay > 0f)
			{
				m_inputDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			m_titleText.Draw(Camera);
			foreach (PauseInfoObj current in m_infoObjList)
			{
				current.Draw(Camera);
			}
			if (Game.PlayerStats.TutorialComplete)
			{
				m_profileCardKey.Draw(Camera);
			}
			m_optionsKey.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			m_optionsIcon.Draw(Camera);
			if (Game.PlayerStats.TutorialComplete)
			{
				m_profileCard.Draw(Camera);
			}
			Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Pause Screen");
				foreach (PauseInfoObj current in m_infoObjList)
				{
					current.Dispose();
				}
				m_infoObjList.Clear();
				m_infoObjList = null;
				m_titleText.Dispose();
				m_titleText = null;
				m_profileCard.Dispose();
				m_profileCard = null;
				m_optionsIcon.Dispose();
				m_optionsIcon = null;
				m_profileCardKey.Dispose();
				m_profileCardKey = null;
				m_optionsKey.Dispose();
				m_optionsKey = null;
				base.Dispose();
			}
		}
	}
}
