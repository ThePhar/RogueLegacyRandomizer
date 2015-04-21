using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public abstract class ChallengeBossRoomObj : RoomObj
	{
		protected bool m_cutsceneRunning;
		protected ChestObj m_bossChest;
		private float m_sparkleTimer;
		private bool m_teleportingOut;
		private float m_roomFloor;
		private TextObj m_bossTitle1;
		private TextObj m_bossTitle2;
		private SpriteObj m_bossDivider;
		private PlayerStats m_storedPlayerStats;
		private int m_storedHealth;
		private float m_storedMana;
		private Vector2 m_storedScale;
		private List<RaindropObj> m_rainFG;
		public abstract bool BossKilled
		{
			get;
		}
		public int StoredHP
		{
			get
			{
				return this.m_storedHealth;
			}
		}
		public float StoredMP
		{
			get
			{
				return this.m_storedMana;
			}
		}
		public ChallengeBossRoomObj()
		{
			this.m_storedPlayerStats = new PlayerStats();
		}
		public override void Initialize()
		{
			this.m_bossTitle1 = new TextObj(Game.JunicodeFont);
			this.m_bossTitle1.Text = "The Forsaken";
			this.m_bossTitle1.OutlineWidth = 2;
			this.m_bossTitle1.FontSize = 18f;
			this.m_bossTitle2 = new TextObj(Game.JunicodeLargeFont);
			this.m_bossTitle2.Text = "Alexander";
			this.m_bossTitle2.OutlineWidth = 2;
			this.m_bossTitle2.FontSize = 40f;
			this.m_bossDivider = new SpriteObj("Blank_Sprite");
			this.m_bossDivider.OutlineWidth = 2;
			foreach (DoorObj current in base.DoorList)
			{
				this.m_roomFloor = (float)current.Bounds.Bottom;
			}
			this.m_bossChest = new ChestObj(null);
			this.m_bossChest.Position = new Vector2((float)this.Bounds.Center.X - (float)this.m_bossChest.Width / 2f, (float)this.Bounds.Center.Y);
			base.GameObjList.Add(this.m_bossChest);
			this.m_rainFG = new List<RaindropObj>();
			for (int i = 0; i < 50; i++)
			{
				RaindropObj raindropObj = new RaindropObj(new Vector2(CDGMath.RandomFloat(base.X - (float)this.Width, base.X), CDGMath.RandomFloat(base.Y, base.Y + (float)this.Height)));
				this.m_rainFG.Add(raindropObj);
				raindropObj.ChangeToParticle();
			}
			base.Initialize();
		}
		public void StorePlayerData()
		{
			this.m_storedPlayerStats = Game.PlayerStats;
			Game.PlayerStats = new PlayerStats();
			Game.PlayerStats.TutorialComplete = true;
			this.m_storedScale = this.Player.Scale;
			this.Player.Scale = new Vector2(2f, 2f);
			SkillSystem.ResetAllTraits();
			this.Player.OverrideInternalScale(this.Player.Scale);
			this.m_storedHealth = this.Player.CurrentHealth;
			this.m_storedMana = this.Player.CurrentMana;
		}
		public void LoadPlayerData()
		{
			Game.PlayerStats = this.m_storedPlayerStats;
		}
		public override void OnEnter()
		{
			this.Player.CurrentHealth = this.Player.MaxHealth;
			this.Player.CurrentMana = this.Player.MaxMana;
			this.Player.ChangeSprite("PlayerIdle_Character");
			this.Player.AttachedLevel.UpdatePlayerHUD();
			this.Player.AttachedLevel.UpdatePlayerHUDAbilities();
			this.Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.UpdateEquipmentColours();
			this.Player.AttachedLevel.RefreshPlayerHUDPos();
			Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
			this.m_bossChest.ChestType = 5;
			this.m_bossChest.Visible = false;
			this.m_bossChest.IsLocked = true;
			this.m_bossChest.X = this.Player.X;
			if (this.m_bossChest.PhysicsMngr == null)
			{
				this.Player.PhysicsMngr.AddObject(this.m_bossChest);
			}
			this.m_teleportingOut = false;
			this.m_bossTitle1.Opacity = 0f;
			this.m_bossTitle2.Opacity = 0f;
			this.m_bossDivider.ScaleX = 0f;
			this.m_bossDivider.Opacity = 0f;
			if (LevelEV.WEAKEN_BOSSES)
			{
				foreach (EnemyObj current in base.EnemyList)
				{
					current.CurrentHealth = 1;
				}
			}
			base.OnEnter();
		}
		public override void OnExit()
		{
			this.LoadPlayerData();
			(Game.ScreenManager.Game as Game).SaveManager.LoadFiles(this.Player.AttachedLevel, new SaveType[]
			{
				SaveType.UpgradeData
			});
			this.Player.AttachedLevel.UpdatePlayerHUD();
			this.Player.AttachedLevel.UpdatePlayerHUDAbilities();
			this.Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.UpdateEquipmentColours();
			this.Player.AttachedLevel.RefreshPlayerHUDPos();
			this.Player.CurrentHealth = this.m_storedHealth;
			this.Player.CurrentMana = this.m_storedMana;
			if (this.BossKilled)
			{
				this.SaveCompletionData();
			}
			Game.PlayerStats.NewBossBeaten = true;
			if (base.LinkedRoom != null)
			{
				this.Player.AttachedLevel.CloseBossDoor(base.LinkedRoom, base.LevelType);
			}
			(Game.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
			base.OnExit();
		}
		public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler)
		{
			SoundManager.PlaySound("Boss_Title");
			this.m_bossTitle1.Text = bossTitle1;
			this.m_bossTitle2.Text = bossTitle2;
			Camera2D camera = this.Player.AttachedLevel.Camera;
			if (this.Player.AttachedLevel.CurrentRoom is LastBossRoom)
			{
				this.m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 100f);
			}
			else
			{
				this.m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 50f);
			}
			this.m_bossTitle2.X = this.m_bossTitle1.X - 0f;
			this.m_bossTitle2.Y = this.m_bossTitle1.Y + 50f;
			this.m_bossDivider.Position = this.m_bossTitle1.Position;
			this.m_bossDivider.Y += (float)(this.m_bossTitle1.Height - 5);
			this.m_bossTitle1.X -= 1000f;
			this.m_bossTitle2.X += 1500f;
			Tween.To(this.m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.To(this.m_bossDivider, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0",
				"ScaleX",
				((float)(this.m_bossTitle2.Width / 5)).ToString()
			});
			Tween.To(this.m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.To(this.m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.By(this.m_bossTitle1, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				"1000"
			});
			Tween.By(this.m_bossTitle2, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				"-1500"
			});
			this.m_bossTitle1.X += 1000f;
			this.m_bossTitle2.X -= 1500f;
			Tween.By(this.m_bossTitle1, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"20"
			});
			Tween.By(this.m_bossTitle2, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"-20"
			});
			this.m_bossTitle1.X -= 1000f;
			this.m_bossTitle2.X += 1500f;
			Tween.AddEndHandlerToLastTween(this, endHandler, new object[0]);
			Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Title_Exit"
			});
			this.m_bossTitle1.X += 1020f;
			this.m_bossTitle2.X -= 1520f;
			this.m_bossTitle1.Opacity = 1f;
			this.m_bossTitle2.Opacity = 1f;
			Tween.To(this.m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"3",
				"Opacity",
				"0"
			});
			Tween.To(this.m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"3",
				"Opacity",
				"0"
			});
			Tween.By(this.m_bossTitle1, 0.6f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"3",
				"X",
				"1500"
			});
			Tween.By(this.m_bossTitle2, 0.6f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"3",
				"X",
				"-1000"
			});
			this.m_bossTitle1.Opacity = 0f;
			this.m_bossTitle2.Opacity = 0f;
			this.m_bossDivider.Opacity = 1f;
			Tween.To(this.m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"2.8",
				"Opacity",
				"0"
			});
			this.m_bossDivider.Opacity = 0f;
		}
		public override void Update(GameTime gameTime)
		{
			foreach (RaindropObj current in this.m_rainFG)
			{
				current.UpdateNoCollision(gameTime);
			}
			if (!this.m_cutsceneRunning)
			{
				base.Update(gameTime);
			}
			if (this.BossKilled && !this.m_bossChest.Visible)
			{
				this.BossCleanup();
				this.m_bossChest.Visible = true;
				this.m_bossChest.Opacity = 0f;
				SoundManager.PlayMusic("TitleScreenSong", true, 1f);
				Tween.To(this.m_bossChest, 4f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.To(this.m_bossChest, 4f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					this.m_roomFloor.ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "UnlockChest", new object[0]);
				this.m_sparkleTimer = 0.5f;
			}
			if (this.m_bossChest.Visible && !this.m_bossChest.IsOpen && this.BossKilled)
			{
				if (this.m_sparkleTimer > 0f)
				{
					this.m_sparkleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (this.m_sparkleTimer <= 0f)
					{
						this.m_sparkleTimer = 0.5f;
						Tween.RunFunction(0f, this.Player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect", new object[]
						{
							new Vector2(this.m_bossChest.X, this.m_bossChest.Y - (float)(this.m_bossChest.Height / 2))
						});
						return;
					}
				}
			}
			else if (this.m_bossChest.Visible && this.m_bossChest.IsOpen && this.BossKilled && !this.m_teleportingOut)
			{
				this.m_teleportingOut = true;
				if (LevelEV.RUN_DEMO_VERSION)
				{
					(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(29, true, null);
					return;
				}
				this.TeleportPlayer();
			}
		}
		protected abstract void SaveCompletionData();
		public virtual void BossCleanup()
		{
			this.Player.StopAllSpells();
		}
		public void TeleportPlayer()
		{
			this.Player.CurrentSpeed = 0f;
			Vector2 position = this.Player.Position;
			Vector2 scale = this.Player.Scale;
			LogicSet logicSet = new LogicSet(this.Player);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "LockControls", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlaySoundLogicAction(new string[]
			{
				"Teleport_Disappear"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[]
			{
				new Vector2(this.Player.X, (float)this.Player.Bounds.Bottom),
				this.Player.Scale
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData", new object[0]), Types.Sequence.Serial);
			if (base.LinkedRoom != null)
			{
				this.Player.Scale = this.m_storedScale;
				this.Player.OverrideInternalScale(this.m_storedScale);
				this.Player.UpdateCollisionBoxes();
				this.Player.Position = new Vector2((float)base.LinkedRoom.Bounds.Center.X, (float)(base.LinkedRoom.Bounds.Bottom - 60) - ((float)this.Player.TerrainBounds.Bottom - this.Player.Y));
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "ScaleY", this.m_storedScale.Y), Types.Sequence.Serial);
				logicSet.AddAction(new TeleportLogicAction(null, this.Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[]
				{
					new Vector2(this.Player.X, (float)(base.LinkedRoom.Bounds.Bottom - 60)),
					this.m_storedScale
				}), Types.Sequence.Serial);
				logicSet.AddAction(new PlaySoundLogicAction(new string[]
				{
					"Teleport_Reappear"
				}), Types.Sequence.Serial);
			}
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "ForceInvincible", false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "UnlockControls", new object[0]), Types.Sequence.Serial);
			this.Player.RunExternalLogicSet(logicSet);
			this.Player.Position = position;
			this.Player.Scale = scale;
			this.Player.UpdateCollisionBoxes();
		}
		public void TeleportScaleOut()
		{
			Tween.To(this.Player, 0.05f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"0"
			});
		}
		public void TeleportScaleIn()
		{
			Tween.To(this.Player, 0.05f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.15",
				"ScaleX",
				this.m_storedScale.X.ToString()
			});
		}
		public void KickPlayerOut()
		{
			this.Player.AttachedLevel.PauseScreen();
			SoundManager.StopMusic(0f);
			this.Player.LockControls();
			this.Player.AttachedLevel.RunWhiteSlashEffect();
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
			this.Player.IsWeighted = false;
			this.Player.IsCollidable = false;
			this.Player.CurrentSpeed = 0f;
			this.Player.AccelerationX = 0f;
			this.Player.AccelerationY = 0f;
			if (this is BlobChallengeRoom)
			{
				Tween.To(this.Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"Zoom",
					"1",
					"X",
					this.Player.X.ToString(),
					"Y",
					this.Player.Y.ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "LockCamera", new object[0]);
			}
			Tween.RunFunction(1f, this, "KickPlayerOut2", new object[0]);
		}
		public void KickPlayerOut2()
		{
			this.Player.AttachedLevel.UnpauseScreen();
			this.Player.CurrentSpeed = 0f;
			Vector2 position = this.Player.Position;
			Vector2 scale = this.Player.Scale;
			LogicSet logicSet = new LogicSet(this.Player);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "LockControls", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1.3f, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlaySoundLogicAction(new string[]
			{
				"Teleport_Disappear"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[]
			{
				new Vector2(this.Player.X, (float)this.Player.Bounds.Bottom),
				this.Player.Scale
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData", new object[0]), Types.Sequence.Serial);
			if (base.LinkedRoom != null)
			{
				this.Player.Scale = this.m_storedScale;
				this.Player.OverrideInternalScale(this.m_storedScale);
				this.Player.UpdateCollisionBoxes();
				this.Player.Position = new Vector2((float)base.LinkedRoom.Bounds.Center.X, (float)(base.LinkedRoom.Bounds.Bottom - 60) - ((float)this.Player.TerrainBounds.Bottom - this.Player.Y));
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "ScaleY", this.m_storedScale.Y), Types.Sequence.Serial);
				logicSet.AddAction(new TeleportLogicAction(null, this.Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[]
				{
					new Vector2(this.Player.X, (float)(base.LinkedRoom.Bounds.Bottom - 60)),
					this.m_storedScale
				}), Types.Sequence.Serial);
				logicSet.AddAction(new PlaySoundLogicAction(new string[]
				{
					"Teleport_Reappear"
				}), Types.Sequence.Serial);
			}
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsWeighted", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsCollidable", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "ForceInvincible", false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "UnlockControls", new object[0]), Types.Sequence.Serial);
			this.Player.RunExternalLogicSet(logicSet);
			this.Player.Position = position;
			this.Player.Scale = scale;
			this.Player.UpdateCollisionBoxes();
		}
		public void LockCamera()
		{
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
		}
		public void UnlockChest()
		{
			this.m_bossChest.IsLocked = false;
		}
		public override void Draw(Camera2D camera)
		{
			foreach (RaindropObj current in this.m_rainFG)
			{
				current.Draw(camera);
			}
			base.Draw(camera);
			this.m_bossDivider.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			this.m_bossTitle1.Draw(camera);
			this.m_bossTitle2.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_bossChest = null;
				this.m_bossDivider.Dispose();
				this.m_bossDivider = null;
				this.m_bossTitle1.Dispose();
				this.m_bossTitle1 = null;
				this.m_bossTitle2.Dispose();
				this.m_bossTitle2 = null;
				foreach (RaindropObj current in this.m_rainFG)
				{
					current.Dispose();
				}
				this.m_rainFG.Clear();
				this.m_rainFG = null;
				base.Dispose();
			}
		}
	}
}
