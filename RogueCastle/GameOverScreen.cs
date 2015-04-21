using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class GameOverScreen : Screen
	{
		private PlayerObj m_player;
		private ObjContainer m_dialoguePlate;
		private KeyIconTextObj m_continueText;
		private SpriteObj m_playerGhost;
		private SpriteObj m_spotlight;
		private List<EnemyObj> m_enemyList;
		private List<Vector2> m_enemyStoredPositions;
		private int m_coinsCollected;
		private int m_bagsCollected;
		private int m_diamondsCollected;
		private FrameSoundObj m_playerFallSound;
		private FrameSoundObj m_playerSwordSpinSound;
		private FrameSoundObj m_playerSwordFallSound;
		private GameObj m_objKilledPlayer;
		private LineageObj m_playerFrame;
		private bool m_lockControls;
		private bool m_droppingStats;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public GameOverScreen()
		{
			this.m_enemyStoredPositions = new List<Vector2>();
		}
		public override void PassInData(List<object> objList)
		{
			if (objList != null)
			{
				this.m_player = (objList[0] as PlayerObj);
				if (this.m_playerFallSound == null)
				{
					this.m_playerFallSound = new FrameSoundObj(this.m_player, 14, new string[]
					{
						"Player_Death_BodyFall"
					});
					this.m_playerSwordSpinSound = new FrameSoundObj(this.m_player, 2, new string[]
					{
						"Player_Death_SwordTwirl"
					});
					this.m_playerSwordFallSound = new FrameSoundObj(this.m_player, 9, new string[]
					{
						"Player_Death_SwordLand"
					});
				}
				this.m_enemyList = (objList[1] as List<EnemyObj>);
				this.m_coinsCollected = (int)objList[2];
				this.m_bagsCollected = (int)objList[3];
				this.m_diamondsCollected = (int)objList[4];
				if (objList[5] != null)
				{
					this.m_objKilledPlayer = (objList[5] as GameObj);
				}
				this.SetObjectKilledPlayerText();
				this.m_enemyStoredPositions.Clear();
				base.PassInData(objList);
			}
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
			textObj.FontSize = 17f;
			textObj.DropShadow = dropShadow;
			textObj.Position = new Vector2(0f, (float)(-(float)this.m_dialoguePlate.Height / 2 + 25));
			this.m_dialoguePlate.AddChild(textObj);
			KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj.FontSize = 12f;
			keyIconTextObj.Align = Types.TextAlign.Centre;
			keyIconTextObj.Text = "\"Arrrrggghhhh\"";
			keyIconTextObj.DropShadow = dropShadow;
			keyIconTextObj.Y = 0f;
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
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.m_playerFrame.Opacity = 0f;
			this.m_playerFrame.Position = this.m_player.Position;
			this.m_playerFrame.SetTraits(Game.PlayerStats.Traits);
			this.m_playerFrame.IsFemale = Game.PlayerStats.IsFemale;
			this.m_playerFrame.Class = Game.PlayerStats.Class;
			this.m_playerFrame.Y -= 120f;
			this.m_playerFrame.SetPortrait(Game.PlayerStats.HeadPiece, Game.PlayerStats.ShoulderPiece, Game.PlayerStats.ChestPiece);
			this.m_playerFrame.UpdateData();
			Tween.To(this.m_playerFrame, 1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"4",
				"Opacity",
				"1"
			});
			FamilyTreeNode item = new FamilyTreeNode
			{
				Name = Game.PlayerStats.PlayerName,
				Age = Game.PlayerStats.Age,
				ChildAge = Game.PlayerStats.ChildAge,
				Class = Game.PlayerStats.Class,
				HeadPiece = Game.PlayerStats.HeadPiece,
				ChestPiece = Game.PlayerStats.ChestPiece,
				ShoulderPiece = Game.PlayerStats.ShoulderPiece,
				NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
				BeatenABoss = Game.PlayerStats.NewBossBeaten,
				Traits = Game.PlayerStats.Traits,
				IsFemale = Game.PlayerStats.IsFemale
			};
			Vector2 traits = Game.PlayerStats.Traits;
			Game.PlayerStats.FamilyTreeArray.Add(item);
			if (Game.PlayerStats.CurrentBranches != null)
			{
				Game.PlayerStats.CurrentBranches.Clear();
			}
			Game.PlayerStats.IsDead = true;
			Game.PlayerStats.Traits = Vector2.Zero;
			Game.PlayerStats.NewBossBeaten = false;
			Game.PlayerStats.RerolledChildren = false;
			Game.PlayerStats.HasArchitectFee = false;
			Game.PlayerStats.NumEnemiesBeaten = 0;
			Game.PlayerStats.LichHealth = 0;
			Game.PlayerStats.LichMana = 0;
			Game.PlayerStats.LichHealthMod = 1f;
			Game.PlayerStats.TimesDead++;
			Game.PlayerStats.LoadStartingRoom = true;
			Game.PlayerStats.EnemiesKilledInRun.Clear();
			if (Game.PlayerStats.SpecialItem != 1 && Game.PlayerStats.SpecialItem != 9 && Game.PlayerStats.SpecialItem != 10 && Game.PlayerStats.SpecialItem != 11 && Game.PlayerStats.SpecialItem != 12 && Game.PlayerStats.SpecialItem != 13)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.Lineage,
				SaveType.MapData
			});
			(base.ScreenManager.Game as Game).SaveManager.SaveAllFileTypes(true);
			Game.PlayerStats.Traits = traits;
			if (Game.PlayerStats.TimesDead >= 20)
			{
				GameUtil.UnlockAchievement("FEAR_OF_LIFE");
			}
			SoundManager.StopMusic(0.5f);
			this.m_droppingStats = false;
			this.m_lockControls = false;
			SoundManager.PlaySound("Player_Death_FadeToBlack");
			this.m_continueText.Text = "Press [Input:" + 0 + "] to move on";
			this.m_player.Visible = true;
			this.m_player.Opacity = 1f;
			this.m_continueText.Opacity = 0f;
			this.m_dialoguePlate.Opacity = 0f;
			this.m_playerGhost.Opacity = 0f;
			this.m_spotlight.Opacity = 0f;
			this.m_playerGhost.Position = new Vector2(this.m_player.X - (float)(this.m_playerGhost.Width / 2), (float)(this.m_player.Bounds.Top - 20));
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
			Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", new object[]
			{
				"GameOverStinger",
				false,
				0.5f
			});
			Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_player.AbsX.ToString(),
				"Y",
				(this.m_player.Bounds.Bottom - 10).ToString(),
				"Zoom",
				"1"
			});
			Tween.RunFunction(2f, this.m_player, "RunDeathAnimation1", new object[0]);
			if (Game.PlayerStats.Traits.X == 13f || Game.PlayerStats.Traits.Y == 13f)
			{
				(this.m_dialoguePlate.GetChildAt(2) as TextObj).Text = "#)!(%*#@!%^";
				(this.m_dialoguePlate.GetChildAt(2) as TextObj).RandomizeSentence(true);
			}
			else
			{
				(this.m_dialoguePlate.GetChildAt(2) as TextObj).Text = GameEV.GAME_HINTS[CDGMath.RandomInt(0, GameEV.GAME_HINTS.Length - 1)];
			}
			(this.m_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + Game.PlayerStats.PlayerName + "'s Parting Words";
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
		public override void OnExit()
		{
			Tween.StopAll(false);
			if (this.m_enemyList != null)
			{
				this.m_enemyList.Clear();
				this.m_enemyList = null;
			}
			Game.PlayerStats.Traits = Vector2.Zero;
			this.BackBufferOpacity = 0f;
			base.OnExit();
		}
		public void DropStats()
		{
			this.m_droppingStats = true;
			Vector2 arg_0C_0 = Vector2.Zero;
			float num = 0f;
			Vector2 topLeftCorner = base.Camera.TopLeftCorner;
			topLeftCorner.X += 200f;
			topLeftCorner.Y += 450f;
			if (this.m_enemyList != null)
			{
				foreach (EnemyObj current in this.m_enemyList)
				{
					this.m_enemyStoredPositions.Add(current.Position);
					current.Position = topLeftCorner;
					current.ChangeSprite(current.ResetSpriteName);
					if (current.SpriteName == "EnemyZombieRise_Character")
					{
						current.ChangeSprite("EnemyZombieWalk_Character");
					}
					current.Visible = true;
					current.Flip = SpriteEffects.FlipHorizontally;
					Tween.StopAllContaining(current, false);
					current.Scale = current.InternalScale;
					current.Scale /= 2f;
					current.Opacity = 0f;
					num += 0.05f;
					EnemyObj_Eyeball enemyObj_Eyeball = current as EnemyObj_Eyeball;
					if (enemyObj_Eyeball != null && enemyObj_Eyeball.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
					{
						enemyObj_Eyeball.ChangeToBossPupil();
					}
					Tween.To(current, 0f, new Easing(Tween.EaseNone), new string[]
					{
						"delay",
						num.ToString(),
						"Opacity",
						"1"
					});
					Tween.RunFunction(num, this, "PlayEnemySound", new object[0]);
					topLeftCorner.X += 25f;
					if (current.X + (float)current.Width > base.Camera.TopLeftCorner.X + 200f + 950f)
					{
						topLeftCorner.Y += 30f;
						topLeftCorner.X = base.Camera.TopLeftCorner.X + 200f;
					}
				}
			}
		}
		public void PlayEnemySound()
		{
			SoundManager.PlaySound("Enemy_Kill_Plant");
		}
		private void SetObjectKilledPlayerText()
		{
			TextObj textObj = this.m_dialoguePlate.GetChildAt(1) as TextObj;
			if (this.m_objKilledPlayer != null)
			{
				EnemyObj enemyObj = this.m_objKilledPlayer as EnemyObj;
				ProjectileObj projectileObj = this.m_objKilledPlayer as ProjectileObj;
				if (enemyObj != null)
				{
					if (enemyObj.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemyObj is EnemyObj_LastBoss)
					{
						textObj.Text = Game.PlayerStats.PlayerName + " has been slain by " + enemyObj.Name;
					}
					else
					{
						textObj.Text = Game.PlayerStats.PlayerName + " has been slain by a " + enemyObj.Name;
					}
				}
				else if (projectileObj != null)
				{
					enemyObj = (projectileObj.Source as EnemyObj);
					if (enemyObj != null)
					{
						if (enemyObj.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemyObj is EnemyObj_LastBoss)
						{
							textObj.Text = Game.PlayerStats.PlayerName + " has been slain by " + enemyObj.Name;
						}
						else
						{
							textObj.Text = Game.PlayerStats.PlayerName + " has been slain by a " + enemyObj.Name;
						}
					}
					else
					{
						textObj.Text = Game.PlayerStats.PlayerName + " was done in by a projectile";
					}
				}
				HazardObj hazardObj = this.m_objKilledPlayer as HazardObj;
				if (hazardObj != null)
				{
					textObj.Text = Game.PlayerStats.PlayerName + " slipped and was impaled by spikes";
					return;
				}
			}
			else
			{
				textObj.Text = Game.PlayerStats.PlayerName + " has been slain";
			}
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls && this.m_droppingStats && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				if (this.m_enemyList.Count > 0 && this.m_enemyList[this.m_enemyList.Count - 1].Opacity != 1f)
				{
					foreach (EnemyObj current in this.m_enemyList)
					{
						Tween.StopAllContaining(current, false);
						current.Opacity = 1f;
					}
					Tween.StopAllContaining(this, false);
					this.PlayEnemySound();
				}
				else
				{
					(base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
					this.m_lockControls = true;
				}
			}
			base.HandleInput();
		}
		public override void Update(GameTime gameTime)
		{
			if (InputManager.JustPressed(Keys.Space, new PlayerIndex?(PlayerIndex.One)))
			{
				(this.m_dialoguePlate.GetChildAt(2) as TextObj).Text = GameEV.GAME_HINTS[CDGMath.RandomInt(0, GameEV.GAME_HINTS.Length - 1)];
			}
			if (this.m_player.SpriteName == "PlayerDeath_Character")
			{
				this.m_playerFallSound.Update();
				this.m_playerSwordFallSound.Update();
				this.m_playerSwordSpinSound.Update();
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
			base.Camera.Draw(Game.GenericTexture, new Rectangle((int)base.Camera.TopLeftCorner.X - 10, (int)base.Camera.TopLeftCorner.Y - 10, 1420, 820), Color.Black * this.BackBufferOpacity);
			foreach (EnemyObj current in this.m_enemyList)
			{
				current.Draw(base.Camera);
			}
			this.m_playerFrame.Draw(base.Camera);
			this.m_player.Draw(base.Camera);
			if (this.m_playerGhost.Opacity > 0f)
			{
				this.m_playerGhost.X += (float)Math.Sin((double)(Game.TotalGameTime * 5f)) * 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
				Console.WriteLine("Disposing Game Over Screen");
				this.m_player = null;
				this.m_dialoguePlate.Dispose();
				this.m_dialoguePlate = null;
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_playerGhost.Dispose();
				this.m_playerGhost = null;
				this.m_spotlight.Dispose();
				this.m_spotlight = null;
				this.m_playerFallSound.Dispose();
				this.m_playerFallSound = null;
				this.m_playerSwordFallSound.Dispose();
				this.m_playerSwordFallSound = null;
				this.m_playerSwordSpinSound.Dispose();
				this.m_playerSwordSpinSound = null;
				this.m_objKilledPlayer = null;
				if (this.m_enemyList != null)
				{
					this.m_enemyList.Clear();
				}
				this.m_enemyList = null;
				if (this.m_enemyStoredPositions != null)
				{
					this.m_enemyStoredPositions.Clear();
				}
				this.m_enemyStoredPositions = null;
				this.m_playerFrame.Dispose();
				this.m_playerFrame = null;
				base.Dispose();
			}
		}
	}
}
