using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public abstract class BossRoomObj : RoomObj
	{
		protected bool m_cutsceneRunning;
		private ChestObj m_bossChest;
		private float m_sparkleTimer;
		private bool m_teleportingOut;
		private float m_roomFloor;
		private TextObj m_bossTitle1;
		private TextObj m_bossTitle2;
		private SpriteObj m_bossDivider;
		public abstract bool BossKilled
		{
			get;
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
			base.Initialize();
		}
		public override void OnEnter()
		{
			Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
			this.m_bossChest.ChestType = 5;
			this.m_bossChest.Visible = false;
			this.m_bossChest.IsLocked = true;
			if (this.m_bossChest.PhysicsMngr == null)
			{
				this.Player.PhysicsMngr.AddObject(this.m_bossChest);
			}
			this.m_teleportingOut = false;
			this.m_bossTitle1.Opacity = 0f;
			this.m_bossTitle2.Opacity = 0f;
			this.m_bossDivider.ScaleX = 0f;
			this.m_bossDivider.Opacity = 0f;
			base.OnEnter();
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
		public virtual void BossCleanup()
		{
			this.Player.StopAllSpells();
			Game.PlayerStats.NewBossBeaten = true;
			if (base.LinkedRoom != null)
			{
				this.Player.AttachedLevel.CloseBossDoor(base.LinkedRoom, base.LevelType);
			}
		}
		public void TeleportPlayer()
		{
			this.Player.CurrentSpeed = 0f;
			Vector2 position = this.Player.Position;
			Vector2 scale = this.Player.Scale;
			Tween.To(this.Player, 0.05f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"1.2",
				"ScaleX",
				"0"
			});
			this.Player.ScaleX = 0f;
			Tween.To(this.Player, 0.05f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"7",
				"ScaleX",
				scale.X.ToString()
			});
			this.Player.ScaleX = scale.X;
			LogicSet logicSet = new LogicSet(this.Player);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "LockControls", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlaySoundLogicAction(new string[]
			{
				"Teleport_Disappear"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[]
			{
				new Vector2(this.Player.X, (float)this.Player.Bounds.Bottom),
				this.Player.Scale
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			if (base.LinkedRoom != null)
			{
				this.Player.Position = new Vector2((float)this.Player.AttachedLevel.RoomList[1].Bounds.Center.X, (float)this.Player.AttachedLevel.RoomList[1].Bounds.Center.Y);
				this.Player.UpdateCollisionBoxes();
				logicSet.AddAction(new TeleportLogicAction(null, this.Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.RoomList[1], "RevealSymbol", new object[]
				{
					base.LevelType,
					true
				}), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(3.5f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
				this.Player.Position = new Vector2((float)base.LinkedRoom.Bounds.Center.X, (float)(base.LinkedRoom.Bounds.Bottom - 60) - ((float)this.Player.Bounds.Bottom - this.Player.Y));
				this.Player.UpdateCollisionBoxes();
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
				logicSet.AddAction(new TeleportLogicAction(null, this.Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[]
				{
					new Vector2(this.Player.X, (float)(base.LinkedRoom.Bounds.Bottom - 60)),
					scale
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
			this.Player.UpdateCollisionBoxes();
		}
		public void UnlockChest()
		{
			this.m_bossChest.IsLocked = false;
		}
		public override void Draw(Camera2D camera)
		{
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
				base.Dispose();
			}
		}
	}
}
