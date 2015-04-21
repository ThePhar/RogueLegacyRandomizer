using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class OptionsScreen : Screen
	{
		private OptionsObj m_selectedOption;
		private int m_selectedOptionIndex;
		private bool m_changingControls;
		private List<OptionsObj> m_optionsArray;
		private ObjContainer m_bgSprite;
		private bool m_transitioning;
		private OptionsObj m_backToMenuObj;
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_cancelText;
		private KeyIconTextObj m_navigationText;
		private SpriteObj m_optionsTitle;
		private SpriteObj m_changeControlsTitle;
		private SpriteObj m_optionsBar;
		private TextObj m_quickDropText;
		private OptionsObj m_quickDropObj;
		private OptionsObj m_reduceQualityObj;
		private OptionsObj m_enableSteamCloudObj;
		private bool m_titleScreenOptions;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public OptionsScreen()
		{
			this.m_optionsArray = new List<OptionsObj>();
			base.UpdateIfCovered = true;
			base.DrawIfCovered = true;
			this.m_titleScreenOptions = true;
		}
		public override void LoadContent()
		{
			this.m_bgSprite = new ObjContainer("SkillUnlockPlate_Character");
			this.m_bgSprite.ForceDraw = true;
			this.m_optionsTitle = new SpriteObj("OptionsScreenTitle_Sprite");
			this.m_bgSprite.AddChild(this.m_optionsTitle);
			this.m_optionsTitle.Position = new Vector2(0f, (float)(-(float)this.m_bgSprite.Width) / 2f + 60f);
			this.m_changeControlsTitle = new SpriteObj("OptionsScreenChangeControls_Sprite");
			this.m_bgSprite.AddChild(this.m_changeControlsTitle);
			this.m_changeControlsTitle.Position = new Vector2(1320f, this.m_optionsTitle.Y);
			this.m_optionsArray.Add(new ResolutionOptionsObj(this));
			this.m_optionsArray.Add(new FullScreenOptionsObj(this));
			this.m_reduceQualityObj = new ReduceQualityOptionsObj(this);
			this.m_optionsArray.Add(this.m_reduceQualityObj);
			this.m_optionsArray.Add(new MusicVolOptionsObj(this));
			this.m_optionsArray.Add(new SFXVolOptionsObj(this));
			this.m_quickDropObj = new QuickDropOptionsObj(this);
			this.m_optionsArray.Add(this.m_quickDropObj);
			this.m_optionsArray.Add(new DeadZoneOptionsObj(this));
			this.m_optionsArray.Add(new ToggleDirectInputOptionsObj(this));
			this.m_optionsArray.Add(new ChangeControlsOptionsObj(this));
			this.m_optionsArray.Add(new ExitProgramOptionsObj(this));
			this.m_backToMenuObj = new BackToMenuOptionsObj(this);
			this.m_backToMenuObj.X = 420f;
			for (int i = 0; i < this.m_optionsArray.Count; i++)
			{
				this.m_optionsArray[i].X = 420f;
				this.m_optionsArray[i].Y = (float)(160 + i * 30);
			}
			this.m_optionsBar = new SpriteObj("OptionsBar_Sprite");
			this.m_optionsBar.ForceDraw = true;
			this.m_optionsBar.Position = new Vector2(this.m_optionsArray[0].X - 20f, this.m_optionsArray[0].Y);
			this.m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_confirmText.Text = "to select option";
			this.m_confirmText.DropShadow = new Vector2(2f, 2f);
			this.m_confirmText.FontSize = 12f;
			this.m_confirmText.Align = Types.TextAlign.Right;
			this.m_confirmText.Position = new Vector2(1290f, 570f);
			this.m_confirmText.ForceDraw = true;
			this.m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_cancelText.Text = "to exit options";
			this.m_cancelText.Align = Types.TextAlign.Right;
			this.m_cancelText.DropShadow = new Vector2(2f, 2f);
			this.m_cancelText.FontSize = 12f;
			this.m_cancelText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 40f);
			this.m_cancelText.ForceDraw = true;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Text = "to navigate options";
			this.m_navigationText.Align = Types.TextAlign.Right;
			this.m_navigationText.DropShadow = new Vector2(2f, 2f);
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 80f);
			this.m_navigationText.ForceDraw = true;
			this.m_quickDropText = new TextObj(Game.JunicodeFont);
			this.m_quickDropText.FontSize = 8f;
			this.m_quickDropText.Text = "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
			this.m_quickDropText.Position = new Vector2(420f, 530f);
			this.m_quickDropText.ForceDraw = true;
			this.m_quickDropText.DropShadow = new Vector2(2f, 2f);
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			this.m_titleScreenOptions = (bool)objList[0];
			base.PassInData(objList);
		}
		public override void OnEnter()
		{
			this.m_quickDropText.Visible = false;
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
				this.m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
				this.m_navigationText.Text = "[Button:LeftStick] to navigate options";
			}
			else
			{
				this.m_confirmText.ForcedScale = new Vector2(1f, 1f);
				this.m_cancelText.ForcedScale = new Vector2(1f, 1f);
				this.m_navigationText.Text = "Arrow keys to navigate options";
			}
			this.m_confirmText.Text = "[Input:" + 0 + "] to select option";
			this.m_cancelText.Text = "[Input:" + 2 + "] to exit options";
			this.m_confirmText.Opacity = 0f;
			this.m_cancelText.Opacity = 0f;
			this.m_navigationText.Opacity = 0f;
			Tween.To(this.m_confirmText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_cancelText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_navigationText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"DialogueMenuOpen"
			});
			if (!this.m_optionsArray.Contains(this.m_backToMenuObj))
			{
				this.m_optionsArray.Insert(this.m_optionsArray.Count - 1, this.m_backToMenuObj);
			}
			if (this.m_titleScreenOptions)
			{
				this.m_optionsArray.RemoveAt(this.m_optionsArray.Count - 2);
			}
			this.m_transitioning = true;
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.8"
			});
			this.m_selectedOptionIndex = 0;
			this.m_selectedOption = this.m_optionsArray[this.m_selectedOptionIndex];
			this.m_selectedOption.IsActive = false;
			this.m_bgSprite.Position = new Vector2(660f, 0f);
			this.m_bgSprite.Opacity = 0f;
			Tween.To(this.m_bgSprite, 0.5f, new Easing(Quad.EaseOut), new string[]
			{
				"Y",
				360f.ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "EndTransition", new object[0]);
			Tween.To(this.m_bgSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			int num = 0;
			foreach (OptionsObj current in this.m_optionsArray)
			{
				current.Y = (float)(160 + num * 30) - 360f;
				current.Opacity = 0f;
				Tween.By(current, 0.5f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					360f.ToString()
				});
				Tween.To(current, 0.2f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				current.Initialize();
				num++;
			}
			this.m_optionsBar.Opacity = 0f;
			Tween.To(this.m_optionsBar, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			base.OnEnter();
		}
		public void EndTransition()
		{
			this.m_transitioning = false;
		}
		private void ExitTransition()
		{
			SoundManager.PlaySound("DialogMenuClose");
			this.m_transitioning = true;
			Tween.To(this.m_confirmText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_cancelText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_navigationText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0"
			});
			Tween.To(this.m_optionsBar, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			this.m_bgSprite.Position = new Vector2(660f, 360f);
			this.m_bgSprite.Opacity = 1f;
			Tween.To(this.m_bgSprite, 0.5f, new Easing(Quad.EaseOut), new string[]
			{
				"Y",
				"0"
			});
			Tween.To(this.m_bgSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			int num = 0;
			foreach (OptionsObj current in this.m_optionsArray)
			{
				current.Y = (float)(160 + num * 30);
				current.Opacity = 1f;
				Tween.By(current, 0.5f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					-360f.ToString()
				});
				Tween.To(current, 0.2f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				num++;
			}
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void OnExit()
		{
			this.m_selectedOption.IsActive = false;
			this.m_selectedOption.IsSelected = false;
			this.m_selectedOption = null;
			(base.ScreenManager.Game as Game).SaveConfig();
			(base.ScreenManager as RCScreenManager).UpdatePauseScreenIcons();
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (!this.m_transitioning)
			{
				if (this.m_selectedOption.IsActive)
				{
					this.m_selectedOption.HandleInput();
				}
				else
				{
					if (!this.m_selectedOption.IsActive)
					{
						int selectedOptionIndex = this.m_selectedOptionIndex;
						if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
						{
							if (this.m_selectedOptionIndex > 0)
							{
								SoundManager.PlaySound("frame_swap");
							}
							this.m_selectedOptionIndex--;
						}
						else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
						{
							if (this.m_selectedOptionIndex < this.m_optionsArray.Count - 1)
							{
								SoundManager.PlaySound("frame_swap");
							}
							this.m_selectedOptionIndex++;
						}
						if (this.m_selectedOptionIndex < 0)
						{
							this.m_selectedOptionIndex = this.m_optionsArray.Count - 1;
						}
						if (this.m_selectedOptionIndex > this.m_optionsArray.Count - 1)
						{
							this.m_selectedOptionIndex = 0;
						}
						if (selectedOptionIndex != this.m_selectedOptionIndex)
						{
							if (this.m_selectedOption != null)
							{
								this.m_selectedOption.IsSelected = false;
							}
							this.m_selectedOption = this.m_optionsArray[this.m_selectedOptionIndex];
							this.m_selectedOption.IsSelected = true;
						}
					}
					if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
					{
						SoundManager.PlaySound("Option_Menu_Select");
						this.m_selectedOption.IsActive = true;
					}
					if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) || Game.GlobalInput.JustPressed(4))
					{
						this.ExitTransition();
					}
				}
				if (this.m_selectedOption == this.m_quickDropObj)
				{
					this.m_quickDropText.Visible = true;
					this.m_quickDropText.Text = "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
				}
				else if (this.m_selectedOption == this.m_reduceQualityObj)
				{
					this.m_quickDropText.Visible = true;
					this.m_quickDropText.Text = "*The game must be restarted for this change to come into effect.";
				}
				else if (this.m_selectedOption == this.m_enableSteamCloudObj)
				{
					this.m_quickDropText.Visible = true;
					this.m_quickDropText.Text = "*Cloud support must be enabled on the Steam platform as well for\nthis feature to work.";
				}
				else
				{
					this.m_quickDropText.Visible = false;
				}
			}
			else
			{
				this.m_quickDropText.Visible = false;
			}
			base.HandleInput();
		}
		public override void Update(GameTime gameTime)
		{
			foreach (OptionsObj current in this.m_optionsArray)
			{
				current.Update(gameTime);
			}
			this.m_optionsBar.Position = new Vector2(this.m_selectedOption.X - 15f, this.m_selectedOption.Y);
			base.Update(gameTime);
		}
		public void ToggleControlsConfig()
		{
			if (!this.m_changingControls)
			{
				foreach (OptionsObj current in this.m_optionsArray)
				{
					Tween.By(current, 0.3f, new Easing(Quad.EaseInOut), new string[]
					{
						"X",
						"-1320"
					});
				}
				Tween.By(this.m_optionsTitle, 0.3f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					"-1320"
				});
				Tween.By(this.m_changeControlsTitle, 0.3f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					"-1320"
				});
				this.m_changingControls = true;
				return;
			}
			foreach (OptionsObj current2 in this.m_optionsArray)
			{
				Tween.By(current2, 0.3f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					"1320"
				});
			}
			Tween.By(this.m_optionsTitle, 0.3f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"1320"
			});
			Tween.By(this.m_changeControlsTitle, 0.3f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"1320"
			});
			this.m_changingControls = false;
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin();
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_bgSprite.Draw(base.Camera);
			foreach (OptionsObj current in this.m_optionsArray)
			{
				current.Draw(base.Camera);
			}
			this.m_quickDropText.Draw(base.Camera);
			this.m_confirmText.Draw(base.Camera);
			this.m_cancelText.Draw(base.Camera);
			this.m_navigationText.Draw(base.Camera);
			this.m_optionsBar.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Options Screen");
				foreach (OptionsObj current in this.m_optionsArray)
				{
					current.Dispose();
				}
				this.m_optionsArray.Clear();
				this.m_optionsArray = null;
				this.m_bgSprite.Dispose();
				this.m_bgSprite = null;
				this.m_optionsTitle = null;
				this.m_changeControlsTitle = null;
				this.m_backToMenuObj = null;
				this.m_confirmText.Dispose();
				this.m_confirmText = null;
				this.m_cancelText.Dispose();
				this.m_cancelText = null;
				this.m_navigationText.Dispose();
				this.m_navigationText = null;
				this.m_optionsBar.Dispose();
				this.m_optionsBar = null;
				this.m_selectedOption = null;
				this.m_quickDropText.Dispose();
				this.m_quickDropText = null;
				this.m_quickDropObj = null;
				this.m_enableSteamCloudObj = null;
				this.m_reduceQualityObj = null;
				base.Dispose();
			}
		}
	}
}
