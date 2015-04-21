using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class DiaryEntryScreen : Screen
	{
		private SpriteObj m_titleText;
		private List<ObjContainer> m_diaryList;
		private int m_entryIndex;
		private int m_entryRow;
		private ObjContainer m_selectedEntry;
		private int m_unlockedEntries;
		private float m_inputDelay;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public DiaryEntryScreen()
		{
			this.m_diaryList = new List<ObjContainer>();
			base.DrawIfCovered = true;
		}
		public override void LoadContent()
		{
			this.m_titleText = new SpriteObj("DiaryEntryTitleText_Sprite");
			this.m_titleText.ForceDraw = true;
			this.m_titleText.X = 660f;
			this.m_titleText.Y = 72f;
			int num = 260;
			int num2 = 150;
			int num3 = num;
			int num4 = num2;
			int num5 = 100;
			int num6 = 200;
			int num7 = 5;
			int num8 = 0;
			for (int i = 0; i < 25; i++)
			{
				ObjContainer objContainer = new ObjContainer("DialogBox_Character");
				objContainer.ForceDraw = true;
				objContainer.Scale = new Vector2(180f / (float)objContainer.Width, 50f / (float)objContainer.Height);
				objContainer.Position = new Vector2((float)num3, (float)num4);
				TextObj textObj = new TextObj(Game.JunicodeFont);
				textObj.Text = "Entry #" + (i + 1);
				textObj.OverrideParentScale = true;
				textObj.OutlineWidth = 2;
				textObj.FontSize = 12f;
				textObj.Y -= 64f;
				textObj.Align = Types.TextAlign.Centre;
				objContainer.AddChild(textObj);
				this.m_diaryList.Add(objContainer);
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
			this.m_inputDelay = 0.5f;
			this.m_entryRow = 1;
			this.m_entryIndex = 0;
			this.UpdateSelection();
			this.m_unlockedEntries = (int)Game.PlayerStats.DiaryEntry;
			if (this.m_unlockedEntries >= 24)
			{
				GameUtil.UnlockAchievement("LOVE_OF_BOOKS");
			}
			for (int i = 0; i < this.m_diaryList.Count; i++)
			{
				if (i < this.m_unlockedEntries)
				{
					this.m_diaryList[i].Visible = true;
				}
				else
				{
					this.m_diaryList[i].Visible = false;
				}
			}
			this.BackBufferOpacity = 0f;
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.7"
			});
			this.m_titleText.Opacity = 0f;
			Tween.To(this.m_titleText, 0.25f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			int num = 0;
			float num2 = 0f;
			foreach (ObjContainer current in this.m_diaryList)
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
					Tween.To(current, 0.25f, new Easing(Tween.EaseNone), new string[]
					{
						"delay",
						num2.ToString(),
						"Opacity",
						"1"
					});
					Tween.By(current, 0.25f, new Easing(Quad.EaseOut), new string[]
					{
						"delay",
						num2.ToString(),
						"Y",
						"50"
					});
				}
			}
			base.OnEnter();
		}
		private void ExitTransition()
		{
			SoundManager.PlaySound("DialogMenuClose");
			int num = 0;
			float num2 = 0f;
			foreach (ObjContainer current in this.m_diaryList)
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
					Tween.To(current, 0.25f, new Easing(Tween.EaseNone), new string[]
					{
						"delay",
						num2.ToString(),
						"Opacity",
						"0"
					});
					Tween.By(current, 0.25f, new Easing(Quad.EaseOut), new string[]
					{
						"delay",
						num2.ToString(),
						"Y",
						"-50"
					});
				}
			}
			this.m_titleText.Opacity = 1f;
			Tween.To(this.m_titleText, 0.25f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			this.m_inputDelay = 1f;
			Tween.To(this, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void HandleInput()
		{
			if (this.m_inputDelay <= 0f)
			{
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					this.DisplayEntry();
				}
				else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
				{
					this.ExitTransition();
				}
				if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
				{
					if (this.m_entryIndex > 0 && this.m_diaryList[this.m_entryIndex - 1].Visible)
					{
						SoundManager.PlaySound("frame_swap");
						this.m_entryIndex--;
					}
				}
				else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
				{
					if (this.m_entryIndex < this.m_diaryList.Count - 1 && this.m_diaryList[this.m_entryIndex + 1].Visible)
					{
						this.m_entryIndex++;
						SoundManager.PlaySound("frame_swap");
					}
				}
				else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					if (this.m_entryRow > 1 && this.m_diaryList[this.m_entryIndex - 5].Visible)
					{
						this.m_entryRow--;
						this.m_entryIndex -= 5;
						SoundManager.PlaySound("frame_swap");
					}
				}
				else if ((Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) && this.m_entryRow < 5 && this.m_diaryList[this.m_entryIndex + 5].Visible)
				{
					this.m_entryRow++;
					this.m_entryIndex += 5;
					SoundManager.PlaySound("frame_swap");
				}
				if (this.m_entryRow > 5)
				{
					this.m_entryRow = 5;
				}
				if (this.m_entryRow < 1)
				{
					this.m_entryRow = 1;
				}
				if (this.m_entryIndex >= this.m_entryRow * 5)
				{
					this.m_entryIndex = this.m_entryRow * 5 - 1;
				}
				if (this.m_entryIndex < this.m_entryRow * 5 - 5)
				{
					this.m_entryIndex = this.m_entryRow * 5 - 5;
				}
				this.UpdateSelection();
				base.HandleInput();
			}
		}
		private void DisplayEntry()
		{
			RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + this.m_entryIndex);
			rCScreenManager.DisplayScreen(13, true, null);
		}
		private void UpdateSelection()
		{
			if (this.m_selectedEntry != null)
			{
				this.m_selectedEntry.TextureColor = Color.White;
			}
			this.m_selectedEntry = this.m_diaryList[this.m_entryIndex];
			this.m_selectedEntry.TextureColor = Color.Yellow;
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_inputDelay > 0f)
			{
				this.m_inputDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_titleText.Draw(base.Camera);
			foreach (ObjContainer current in this.m_diaryList)
			{
				current.Draw(base.Camera);
			}
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Diary Entry Screen");
				foreach (ObjContainer current in this.m_diaryList)
				{
					current.Dispose();
				}
				this.m_diaryList.Clear();
				this.m_diaryList = null;
				this.m_selectedEntry = null;
				this.m_titleText.Dispose();
				this.m_titleText = null;
				base.Dispose();
			}
		}
	}
}
