using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class GameOverBossScreen : Screen
	{
		private EnemyObj_LastBoss m_lastBoss;
		private ObjContainer m_dialoguePlate;
		private KeyIconTextObj m_continueText;
		private SpriteObj m_playerGhost;
		private SpriteObj m_king;
		private SpriteObj m_spotlight;
		private LineageObj m_playerFrame;
		private FrameSoundObj m_bossFallSound;
		private FrameSoundObj m_bossKneesSound;
		private Vector2 m_initialCameraPos;
		private bool m_lockControls;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public GameOverBossScreen()
		{
			base.DrawIfCovered = true;
		}
		public override void PassInData(List<object> objList)
		{
			this.m_lastBoss = (objList[0] as EnemyObj_LastBoss);
			this.m_bossKneesSound = new FrameSoundObj(this.m_lastBoss, 3, new string[]
			{
				"FinalBoss_St2_Deathsceen_Knees"
			});
			this.m_bossFallSound = new FrameSoundObj(this.m_lastBoss, 13, new string[]
			{
				"FinalBoss_St2_Deathsceen_Fall"
			});
			base.PassInData(objList);
		}
		public override void LoadContent()
		{
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.FontSize = 14f;
			this.m_continueText.Align = Types.TextAlign.Right;
			this.m_continueText.Opacity = 0f;
			this.m_continueText.Position = new Vector2(1270f, 30f);
			this.m_continueText.ForceDraw = true;
			Vector2 dropShadow = new Vector2(2f, 2f);
			Color textureColor = new Color(255, 254, 128);
			this.m_dialoguePlate = new ObjContainer("DialogBox_Character");
			this.m_dialoguePlate.Position = new Vector2(660f, 610f);
			this.m_dialoguePlate.ForceDraw = true;
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Align = Types.TextAlign.Centre;
			textObj.Text = "Your valor shown in battle shall never be forgotten.";
			textObj.FontSize = 18f;
			textObj.DropShadow = dropShadow;
			textObj.Position = new Vector2(0f, (float)(-(float)this.m_dialoguePlate.Height / 2 + 25));
			this.m_dialoguePlate.AddChild(textObj);
			KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj.FontSize = 12f;
			keyIconTextObj.Align = Types.TextAlign.Centre;
			keyIconTextObj.Text = "\"Arrrrggghhhh\"";
			keyIconTextObj.DropShadow = dropShadow;
			keyIconTextObj.Y = 10f;
			keyIconTextObj.TextureColor = textureColor;
			this.m_dialoguePlate.AddChild(keyIconTextObj);
			TextObj textObj2 = new TextObj(Game.JunicodeFont);
			textObj2.FontSize = 8f;
			textObj2.Text = "-Player X's parting words";
			textObj2.Y = keyIconTextObj.Y;
			textObj2.Y += 40f;
			textObj2.X += 20f;
			textObj2.DropShadow = dropShadow;
			this.m_dialoguePlate.AddChild(textObj2);
			this.m_playerGhost = new SpriteObj("PlayerGhost_Sprite");
			this.m_playerGhost.AnimationDelay = 0.1f;
			this.m_spotlight = new SpriteObj("GameOverSpotlight_Sprite");
			this.m_spotlight.Rotation = 90f;
			this.m_spotlight.ForceDraw = true;
			this.m_spotlight.Position = new Vector2(660f, (float)(40 + this.m_spotlight.Height));
			this.m_playerFrame = new LineageObj(null, true);
			this.m_playerFrame.DisablePlaque = true;
			this.m_king = new SpriteObj("King_Sprite");
			this.m_king.OutlineWidth = 2;
			this.m_king.AnimationDelay = 0.1f;
			this.m_king.PlayAnimation(true);
			this.m_king.Scale = new Vector2(2f, 2f);
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.m_initialCameraPos = base.Camera.Position;
			this.SetObjectKilledPlayerText();
			this.m_playerFrame.Opacity = 0f;
			this.m_playerFrame.Position = this.m_lastBoss.Position;
			this.m_playerFrame.SetTraits(Vector2.Zero);
			this.m_playerFrame.IsFemale = false;
			this.m_playerFrame.Class = 0;
			this.m_playerFrame.Y -= 120f;
			this.m_playerFrame.SetPortrait(8, 1, 1);
			this.m_playerFrame.UpdateData();
			Tween.To(this.m_playerFrame, 1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"4",
				"Opacity",
				"1"
			});
			SoundManager.StopMusic(0.5f);
			this.m_lockControls = false;
			SoundManager.PlaySound("Player_Death_FadeToBlack");
			this.m_continueText.Text = "Press [Input:" + 0 + "] to move on";
			this.m_lastBoss.Visible = true;
			this.m_lastBoss.Opacity = 1f;
			this.m_continueText.Opacity = 0f;
			this.m_dialoguePlate.Opacity = 0f;
			this.m_playerGhost.Opacity = 0f;
			this.m_spotlight.Opacity = 0f;
			this.m_playerGhost.Position = new Vector2(this.m_lastBoss.X - (float)(this.m_playerGhost.Width / 2), (float)(this.m_lastBoss.Bounds.Top - 20));
			Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Ghost"
			});
			Tween.To(this.m_playerGhost, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"3",
				"Opacity",
				"0.4"
			});
			Tween.By(this.m_playerGhost, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"3",
				"Y",
				"-150"
			});
			this.m_playerGhost.Opacity = 0.4f;
			Tween.To(this.m_playerGhost, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"4",
				"Opacity",
				"0"
			});
			this.m_playerGhost.Opacity = 0f;
			this.m_playerGhost.PlayAnimation(true);
			Tween.To(this, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"1"
			});
			Tween.To(this.m_spotlight, 0.1f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"1",
				"Opacity",
				"1"
			});
			Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_Spotlight"
			});
			Tween.RunFunction(2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"FinalBoss_St1_DeathGrunt"
			});
			Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", new object[]
			{
				"GameOverBossStinger",
				false,
				0.5f
			});
			Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_lastBoss.AbsX.ToString(),
				"Y",
				(this.m_lastBoss.Bounds.Bottom - 10).ToString()
			});
			Tween.RunFunction(2f, this.m_lastBoss, "PlayAnimation", new object[]
			{
				false
			});
			(this.m_dialoguePlate.GetChildAt(2) as TextObj).Text = "The sun... I had forgotten how it feels...";
			(this.m_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + this.m_lastBoss.Name + "'s Parting Words";
			Tween.To(this.m_dialoguePlate, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"2",
				"Opacity",
				"1"
			});
			Tween.RunFunction(4f, this, "DropStats", new object[0]);
			Tween.To(this.m_continueText, 0.4f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"4",
				"Opacity",
				"1"
			});
			base.OnEnter();
		}
		public void DropStats()
		{
			Vector2 arg_05_0 = Vector2.Zero;
			float num = 0f;
			Vector2 topLeftCorner = base.Camera.TopLeftCorner;
			topLeftCorner.X += 200f;
			topLeftCorner.Y += 450f;
			this.m_king.Position = topLeftCorner;
			this.m_king.Visible = true;
			this.m_king.StopAnimation();
			this.m_king.Scale /= 2f;
			this.m_king.Opacity = 0f;
			num += 0.05f;
			Tween.To(this.m_king, 0f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				num.ToString(),
				"Opacity",
				"1"
			});
		}
		private void SetObjectKilledPlayerText()
		{
			TextObj textObj = this.m_dialoguePlate.GetChildAt(1) as TextObj;
			textObj.Text = this.m_lastBoss.Name + " has been slain by " + Game.PlayerStats.PlayerName;
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)) && this.m_continueText.Opacity == 1f)
			{
				this.m_lockControls = true;
				this.ExitTransition();
			}
			base.HandleInput();
		}
		private void ExitTransition()
		{
			Tween.StopAll(false);
			SoundManager.StopMusic(1f);
			Tween.To(this, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"BackBufferOpacity",
				"0"
			});
			Tween.To(this.m_lastBoss, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_dialoguePlate, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_continueText, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_playerGhost, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_king, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_spotlight, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_playerFrame, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(base.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_initialCameraPos.X.ToString(),
				"Y",
				this.m_initialCameraPos.Y.ToString()
			});
			Tween.RunFunction(0.5f, base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void OnExit()
		{
			this.BackBufferOpacity = 0f;
			Game.ScreenManager.Player.UnlockControls();
			Game.ScreenManager.Player.AttachedLevel.CameraLockedToPlayer = true;
			(Game.ScreenManager.GetLevelScreen().CurrentRoom as LastBossRoom).BossCleanup();
			base.OnExit();
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_lastBoss.SpriteName == "EnemyLastBossDeath_Character")
			{
				this.m_bossKneesSound.Update();
				this.m_bossFallSound.Update();
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
			base.Camera.Draw(Game.GenericTexture, new Rectangle((int)base.Camera.TopLeftCorner.X - 10, (int)base.Camera.TopLeftCorner.Y - 10, 1420, 820), Color.Black * this.BackBufferOpacity);
			this.m_king.Draw(base.Camera);
			this.m_playerFrame.Draw(base.Camera);
			this.m_lastBoss.Draw(base.Camera);
			if (this.m_playerGhost.Opacity > 0f)
			{
				this.m_playerGhost.X += (float)Math.Sin((double)(Game.TotalGameTime * 5f));
			}
			this.m_playerGhost.Draw(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
			this.m_spotlight.Draw(base.Camera);
			this.m_dialoguePlate.Draw(base.Camera);
			this.m_continueText.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Game Over Boss Screen");
				this.m_lastBoss = null;
				this.m_dialoguePlate.Dispose();
				this.m_dialoguePlate = null;
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_playerGhost.Dispose();
				this.m_playerGhost = null;
				this.m_spotlight.Dispose();
				this.m_spotlight = null;
				this.m_bossFallSound.Dispose();
				this.m_bossFallSound = null;
				this.m_bossKneesSound.Dispose();
				this.m_bossKneesSound = null;
				this.m_playerFrame.Dispose();
				this.m_playerFrame = null;
				this.m_king.Dispose();
				this.m_king = null;
				base.Dispose();
			}
		}
	}
}
