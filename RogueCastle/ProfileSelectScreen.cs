using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class ProfileSelectScreen : Screen
	{
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_cancelText;
		private KeyIconTextObj m_navigationText;
		private KeyIconTextObj m_deleteProfileText;
		private SpriteObj m_title;
		private ObjContainer m_slot1Container;
		private ObjContainer m_slot2Container;
		private ObjContainer m_slot3Container;
		private int m_selectedIndex;
		private bool m_lockControls;
		private List<ObjContainer> m_slotArray;
		private ObjContainer m_selectedSlot;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public ProfileSelectScreen()
		{
			this.m_slotArray = new List<ObjContainer>();
			base.DrawIfCovered = true;
		}
		public override void LoadContent()
		{
			this.m_title = new SpriteObj("ProfileSelectTitle_Sprite");
			this.m_title.ForceDraw = true;
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Align = Types.TextAlign.Centre;
			textObj.Text = "- START NEW LEGACY - ";
			textObj.TextureColor = Color.White;
			textObj.OutlineWidth = 2;
			textObj.FontSize = 10f;
			textObj.Position = new Vector2(0f, -((float)textObj.Height / 2f));
			this.m_slot1Container = new ObjContainer("ProfileSlotBG_Container");
			TextObj obj = textObj.Clone() as TextObj;
			this.m_slot1Container.AddChild(obj);
			SpriteObj spriteObj = new SpriteObj("ProfileSlot1Text_Sprite");
			spriteObj.Position = new Vector2(-130f, -35f);
			this.m_slot1Container.AddChild(spriteObj);
			TextObj textObj2 = textObj.Clone() as TextObj;
			this.m_slot1Container.AddChild(textObj2);
			textObj2.Position = new Vector2(120f, 15f);
			TextObj textObj3 = textObj.Clone() as TextObj;
			textObj3.Position = new Vector2(-120f, 15f);
			this.m_slot1Container.AddChild(textObj3);
			this.m_slot1Container.ForceDraw = true;
			this.m_slot2Container = new ObjContainer("ProfileSlotBG_Container");
			TextObj obj2 = textObj.Clone() as TextObj;
			this.m_slot2Container.AddChild(obj2);
			SpriteObj spriteObj2 = new SpriteObj("ProfileSlot2Text_Sprite");
			spriteObj2.Position = new Vector2(-130f, -35f);
			this.m_slot2Container.AddChild(spriteObj2);
			TextObj textObj4 = textObj.Clone() as TextObj;
			this.m_slot2Container.AddChild(textObj4);
			textObj4.Position = new Vector2(120f, 15f);
			TextObj textObj5 = textObj.Clone() as TextObj;
			textObj5.Position = new Vector2(-120f, 15f);
			this.m_slot2Container.AddChild(textObj5);
			this.m_slot2Container.ForceDraw = true;
			this.m_slot3Container = new ObjContainer("ProfileSlotBG_Container");
			TextObj obj3 = textObj.Clone() as TextObj;
			this.m_slot3Container.AddChild(obj3);
			SpriteObj spriteObj3 = new SpriteObj("ProfileSlot3Text_Sprite");
			spriteObj3.Position = new Vector2(-130f, -35f);
			this.m_slot3Container.AddChild(spriteObj3);
			TextObj textObj6 = textObj.Clone() as TextObj;
			this.m_slot3Container.AddChild(textObj6);
			textObj6.Position = new Vector2(120f, 15f);
			TextObj textObj7 = textObj.Clone() as TextObj;
			textObj7.Position = new Vector2(-120f, 15f);
			this.m_slot3Container.AddChild(textObj7);
			this.m_slot3Container.ForceDraw = true;
			this.m_slotArray.Add(this.m_slot1Container);
			this.m_slotArray.Add(this.m_slot2Container);
			this.m_slotArray.Add(this.m_slot3Container);
			this.m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_confirmText.Text = "to select profile";
			this.m_confirmText.DropShadow = new Vector2(2f, 2f);
			this.m_confirmText.FontSize = 12f;
			this.m_confirmText.Align = Types.TextAlign.Right;
			this.m_confirmText.Position = new Vector2(1290f, 570f);
			this.m_confirmText.ForceDraw = true;
			this.m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_cancelText.Text = "to exit screen";
			this.m_cancelText.Align = Types.TextAlign.Right;
			this.m_cancelText.DropShadow = new Vector2(2f, 2f);
			this.m_cancelText.FontSize = 12f;
			this.m_cancelText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 40f);
			this.m_cancelText.ForceDraw = true;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Text = "to navigate profiles";
			this.m_navigationText.Align = Types.TextAlign.Right;
			this.m_navigationText.DropShadow = new Vector2(2f, 2f);
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 80f);
			this.m_navigationText.ForceDraw = true;
			this.m_deleteProfileText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_deleteProfileText.Text = "to delete profile";
			this.m_deleteProfileText.Align = Types.TextAlign.Left;
			this.m_deleteProfileText.DropShadow = new Vector2(2f, 2f);
			this.m_deleteProfileText.FontSize = 12f;
			this.m_deleteProfileText.Position = new Vector2(20f, this.m_confirmText.Y + 80f);
			this.m_deleteProfileText.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("DialogOpen");
			this.m_lockControls = true;
			this.m_selectedIndex = (int)(Game.GameConfig.ProfileSlot - 1);
			this.m_selectedSlot = this.m_slotArray[this.m_selectedIndex];
			this.m_selectedSlot.TextureColor = Color.Yellow;
			this.CheckSaveHeaders(this.m_slot1Container, 1);
			this.CheckSaveHeaders(this.m_slot2Container, 2);
			this.CheckSaveHeaders(this.m_slot3Container, 3);
			this.m_deleteProfileText.Visible = true;
			if (this.m_slotArray[this.m_selectedIndex].ID == 0)
			{
				this.m_deleteProfileText.Visible = false;
			}
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.9"
			});
			this.m_title.Position = new Vector2(660f, 100f);
			this.m_slot1Container.Position = new Vector2(660f, 300f);
			this.m_slot2Container.Position = new Vector2(660f, 420f);
			this.m_slot3Container.Position = new Vector2(660f, 540f);
			this.TweenInText(this.m_title, 0f);
			this.TweenInText(this.m_slot1Container, 0.05f);
			this.TweenInText(this.m_slot2Container, 0.1f);
			this.TweenInText(this.m_slot3Container, 0.15f);
			Tween.RunFunction(0.5f, this, "UnlockControls", new object[0]);
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
				this.m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
				this.m_navigationText.Text = "[Button:LeftStick] to navigate profiles";
			}
			else
			{
				this.m_confirmText.ForcedScale = new Vector2(1f, 1f);
				this.m_cancelText.ForcedScale = new Vector2(1f, 1f);
				this.m_navigationText.Text = "Arrow keys to navigate profiles";
			}
			this.m_confirmText.Text = "[Input:" + 0 + "] to select profiles";
			this.m_cancelText.Text = "[Input:" + 2 + "] to exit profiles";
			this.m_deleteProfileText.Text = "[Input:" + 26 + "] to delete profile";
			this.m_confirmText.Opacity = 0f;
			this.m_cancelText.Opacity = 0f;
			this.m_navigationText.Opacity = 0f;
			this.m_deleteProfileText.Opacity = 0f;
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
			Tween.To(this.m_deleteProfileText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			base.OnEnter();
		}
		private void CheckSaveHeaders(ObjContainer container, byte profile)
		{
			TextObj textObj = container.GetChildAt(1) as TextObj;
			TextObj textObj2 = container.GetChildAt(3) as TextObj;
			TextObj textObj3 = container.GetChildAt(4) as TextObj;
			textObj2.Text = "";
			textObj3.Text = "";
			string text = null;
			byte classType = 0;
			int num = 0;
			bool flag = false;
			int num2 = 0;
			try
			{
				(base.ScreenManager.Game as Game).SaveManager.GetSaveHeader(profile, out classType, out text, out num, out flag, out num2);
				if (text == null)
				{
					textObj.Text = "- START NEW LEGACY -";
					container.ID = 0;
				}
				else
				{
					bool isFemale = text.Contains("Lady");
					if (!flag)
					{
						textObj.Text = text + " the " + ClassType.ToString(classType, isFemale);
					}
					else
					{
						textObj.Text = text + " the Deceased";
					}
					textObj2.Text = "Lvl. " + num;
					if (num2 > 0)
					{
						textObj3.Text = "NG+ " + num2;
					}
					container.ID = 1;
				}
			}
			catch
			{
				textObj.Text = "- START NEW LEGACY -";
				container.ID = 0;
			}
		}
		public void UnlockControls()
		{
			this.m_lockControls = false;
		}
		private void TweenInText(GameObj obj, float delay)
		{
			obj.Opacity = 0f;
			obj.Y -= 50f;
			Tween.To(obj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				delay.ToString(),
				"Opacity",
				"1"
			});
			Tween.By(obj, 0.5f, new Easing(Quad.EaseOut), new string[]
			{
				"delay",
				delay.ToString(),
				"Y",
				"50"
			});
		}
		private void ExitTransition()
		{
			SoundManager.PlaySound("DialogMenuClose");
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
			Tween.To(this.m_deleteProfileText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			this.m_lockControls = true;
			this.TweenOutText(this.m_title, 0f);
			this.TweenOutText(this.m_slot1Container, 0.05f);
			this.TweenOutText(this.m_slot2Container, 0.1f);
			this.TweenOutText(this.m_slot3Container, 0.15f);
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"BackBufferOpacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		private void TweenOutText(GameObj obj, float delay)
		{
			Tween.To(obj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				delay.ToString(),
				"Opacity",
				"0"
			});
			Tween.By(obj, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				delay.ToString(),
				"Y",
				"-50"
			});
		}
		public override void OnExit()
		{
			this.m_slot1Container.TextureColor = Color.White;
			this.m_slot2Container.TextureColor = Color.White;
			this.m_slot3Container.TextureColor = Color.White;
			this.m_lockControls = false;
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls)
			{
				ObjContainer selectedSlot = this.m_selectedSlot;
				if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
				{
					this.m_selectedIndex++;
					if (this.m_selectedIndex >= this.m_slotArray.Count)
					{
						this.m_selectedIndex = 0;
					}
					this.m_selectedSlot = this.m_slotArray[this.m_selectedIndex];
					SoundManager.PlaySound("frame_swap");
					this.m_deleteProfileText.Visible = true;
					if (this.m_selectedSlot.ID == 0)
					{
						this.m_deleteProfileText.Visible = false;
					}
				}
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					this.m_selectedIndex--;
					if (this.m_selectedIndex < 0)
					{
						this.m_selectedIndex = this.m_slotArray.Count - 1;
					}
					this.m_selectedSlot = this.m_slotArray[this.m_selectedIndex];
					SoundManager.PlaySound("frame_swap");
					this.m_deleteProfileText.Visible = true;
					if (this.m_selectedSlot.ID == 0)
					{
						this.m_deleteProfileText.Visible = false;
					}
				}
				if (this.m_selectedSlot != selectedSlot)
				{
					selectedSlot.TextureColor = Color.White;
					this.m_selectedSlot.TextureColor = Color.Yellow;
				}
				if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
				{
					this.ExitTransition();
				}
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					SoundManager.PlaySound("Map_On");
					Game.GameConfig.ProfileSlot = (byte)(this.m_selectedIndex + 1);
					Game game = base.ScreenManager.Game as Game;
					game.SaveConfig();
					if (game.SaveManager.FileExists(SaveType.PlayerData))
					{
						(base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
					}
					else
					{
						SkillSystem.ResetAllTraits();
						Game.PlayerStats.Dispose();
						Game.PlayerStats = new PlayerStats();
						(base.ScreenManager as RCScreenManager).Player.Reset();
						Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
						Game.ScreenManager.Player.CurrentMana = (float)Game.PlayerStats.CurrentMana;
						(base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
					}
				}
				if (Game.GlobalInput.JustPressed(26) && this.m_deleteProfileText.Visible)
				{
					SoundManager.PlaySound("Map_On");
					this.DeleteSaveAsk();
				}
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin();
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_title.Draw(base.Camera);
			this.m_slot1Container.Draw(base.Camera);
			this.m_slot2Container.Draw(base.Camera);
			this.m_slot3Container.Draw(base.Camera);
			this.m_confirmText.Draw(base.Camera);
			this.m_cancelText.Draw(base.Camera);
			this.m_navigationText.Draw(base.Camera);
			this.m_deleteProfileText.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public void DeleteSaveAsk()
		{
			RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("Delete Save");
			rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain", new object[0]);
			rCScreenManager.DisplayScreen(13, false, null);
		}
		public void DeleteSaveAskAgain()
		{
			RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("Delete Save2");
			rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave", new object[0]);
			rCScreenManager.DisplayScreen(13, false, null);
		}
		public void DeleteSave()
		{
			bool flag = false;
			byte profileSlot = Game.GameConfig.ProfileSlot;
			if ((int)Game.GameConfig.ProfileSlot == this.m_selectedIndex + 1)
			{
				flag = true;
			}
			Game.GameConfig.ProfileSlot = (byte)(this.m_selectedIndex + 1);
			(base.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
			(base.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);
			Game.GameConfig.ProfileSlot = profileSlot;
			if (flag)
			{
				Game.PlayerStats.Dispose();
				SkillSystem.ResetAllTraits();
				Game.PlayerStats = new PlayerStats();
				(base.ScreenManager as RCScreenManager).Player.Reset();
				SoundManager.StopMusic(1f);
				(base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
				return;
			}
			this.m_deleteProfileText.Visible = false;
			this.CheckSaveHeaders(this.m_slotArray[this.m_selectedIndex], (byte)(this.m_selectedIndex + 1));
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_title.Dispose();
				this.m_title = null;
				this.m_slot1Container.Dispose();
				this.m_slot1Container = null;
				this.m_slot2Container.Dispose();
				this.m_slot2Container = null;
				this.m_slot3Container.Dispose();
				this.m_slot3Container = null;
				this.m_slotArray.Clear();
				this.m_slotArray = null;
				this.m_selectedSlot = null;
				this.m_confirmText.Dispose();
				this.m_confirmText = null;
				this.m_cancelText.Dispose();
				this.m_cancelText = null;
				this.m_navigationText.Dispose();
				this.m_navigationText = null;
				this.m_deleteProfileText.Dispose();
				this.m_deleteProfileText = null;
				base.Dispose();
			}
		}
	}
}
