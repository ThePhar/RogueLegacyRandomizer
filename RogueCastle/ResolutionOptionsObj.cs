using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class ResolutionOptionsObj : OptionsObj
	{
		private List<Vector2> m_displayModeList;
		private TextObj m_toggleText;
		private Vector2 m_selectedResolution;
		private int m_selectedResIndex;
		private float m_resetCounter;
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (value)
				{
					this.m_toggleText.TextureColor = Color.Yellow;
					return;
				}
				this.m_toggleText.TextureColor = Color.White;
				this.m_toggleText.Text = this.m_selectedResolution.X + "x" + this.m_selectedResolution.Y;
			}
		}
		public ResolutionOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Resolution")
		{
			this.m_toggleText = (this.m_nameText.Clone() as TextObj);
			this.m_toggleText.X = (float)this.m_optionsTextOffset;
			this.m_toggleText.Text = "null";
			this.AddChild(this.m_toggleText);
		}
		public override void Initialize()
		{
			this.m_resetCounter = 0f;
			this.m_selectedResolution = new Vector2((float)this.m_parentScreen.ScreenManager.Game.GraphicsDevice.Viewport.Width, (float)this.m_parentScreen.ScreenManager.Game.GraphicsDevice.Viewport.Height);
			if (this.m_displayModeList != null)
			{
				this.m_displayModeList.Clear();
			}
			this.m_displayModeList = (this.m_parentScreen.ScreenManager.Game as Game).GetSupportedResolutions();
			this.m_toggleText.Text = this.m_selectedResolution.X + "x" + this.m_selectedResolution.Y;
			this.m_selectedResIndex = 0;
			for (int i = 0; i < this.m_displayModeList.Count; i++)
			{
				if (this.m_selectedResolution == this.m_displayModeList[i])
				{
					this.m_selectedResIndex = i;
					return;
				}
			}
		}
		public override void HandleInput()
		{
			int selectedResIndex = this.m_selectedResIndex;
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
			{
				this.m_selectedResIndex--;
				SoundManager.PlaySound("frame_swap");
			}
			else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				this.m_selectedResIndex++;
				SoundManager.PlaySound("frame_swap");
			}
			if (this.m_selectedResIndex < 0)
			{
				this.m_selectedResIndex = 0;
			}
			if (this.m_selectedResIndex > this.m_displayModeList.Count - 1)
			{
				this.m_selectedResIndex = this.m_displayModeList.Count - 1;
			}
			if (this.m_selectedResIndex != selectedResIndex)
			{
				this.m_toggleText.Text = this.m_displayModeList[this.m_selectedResIndex].X + "x" + this.m_displayModeList[this.m_selectedResIndex].Y;
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				SoundManager.PlaySound("Option_Menu_Select");
				Vector2 vector = this.m_displayModeList[this.m_selectedResIndex];
				if (this.m_selectedResolution != vector)
				{
					(this.m_parentScreen.ScreenManager.Game as Game).graphics.PreferredBackBufferWidth = (int)vector.X;
					(this.m_parentScreen.ScreenManager.Game as Game).graphics.PreferredBackBufferHeight = (int)vector.Y;
					(this.m_parentScreen.ScreenManager.Game as Game).graphics.ApplyChanges();
					(this.m_parentScreen.ScreenManager as RCScreenManager).ForceResolutionChangeCheck();
					if ((this.m_parentScreen.ScreenManager.Game as Game).graphics.IsFullScreen)
					{
						RCScreenManager rCScreenManager = this.m_parentScreen.ScreenManager as RCScreenManager;
						rCScreenManager.DialogueScreen.SetDialogue("Resolution Changed");
						rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
						rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "SaveResolution", new object[]
						{
							vector
						});
						rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelResolution", new object[0]);
						rCScreenManager.DisplayScreen(13, false, null);
						this.m_resetCounter = 10f;
					}
					else
					{
						this.m_selectedResolution = vector;
						this.SaveResolution(vector);
					}
				}
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				this.IsActive = false;
			}
			base.HandleInput();
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_resetCounter > 0f)
			{
				this.m_resetCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_resetCounter <= 0f)
				{
					RCScreenManager rCScreenManager = this.m_parentScreen.ScreenManager as RCScreenManager;
					rCScreenManager.HideCurrentScreen();
					this.CancelResolution();
				}
			}
			base.Update(gameTime);
		}
		public void SaveResolution(Vector2 resolution)
		{
			Game.GameConfig.ScreenWidth = (int)resolution.X;
			Game.GameConfig.ScreenHeight = (int)resolution.Y;
			this.m_resetCounter = 0f;
			this.m_selectedResolution = resolution;
			this.IsActive = false;
		}
		public void CancelResolution()
		{
			this.m_resetCounter = 0f;
			(this.m_parentScreen.ScreenManager.Game as Game).graphics.PreferredBackBufferWidth = (int)this.m_selectedResolution.X;
			(this.m_parentScreen.ScreenManager.Game as Game).graphics.PreferredBackBufferHeight = (int)this.m_selectedResolution.Y;
			(this.m_parentScreen.ScreenManager.Game as Game).graphics.ApplyChanges();
			(this.m_parentScreen.ScreenManager as RCScreenManager).ForceResolutionChangeCheck();
			this.m_toggleText.Text = this.m_selectedResolution.X + "x" + this.m_selectedResolution.Y;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_toggleText = null;
				this.m_displayModeList.Clear();
				this.m_displayModeList = null;
				base.Dispose();
			}
		}
	}
}
