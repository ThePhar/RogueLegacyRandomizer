using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			m_bossTitle1 = new TextObj(Game.JunicodeFont);
			m_bossTitle1.Text = "The Forsaken";
			m_bossTitle1.OutlineWidth = 2;
			m_bossTitle1.FontSize = 18f;
			m_bossTitle2 = new TextObj(Game.JunicodeLargeFont);
			m_bossTitle2.Text = "Alexander";
			m_bossTitle2.OutlineWidth = 2;
			m_bossTitle2.FontSize = 40f;
			m_bossDivider = new SpriteObj("Blank_Sprite");
			m_bossDivider.OutlineWidth = 2;
			foreach (DoorObj current in DoorList)
			{
				m_roomFloor = current.Bounds.Bottom;
			}
			m_bossChest = new ChestObj(null);
			m_bossChest.Position = new Vector2(Bounds.Center.X - m_bossChest.Width / 2f, Bounds.Center.Y);
			GameObjList.Add(m_bossChest);
			base.Initialize();
		}
		public override void OnEnter()
		{
			Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
			m_bossChest.ChestType = 5;
			m_bossChest.Visible = false;
			m_bossChest.IsLocked = true;
			if (m_bossChest.PhysicsMngr == null)
			{
				Player.PhysicsMngr.AddObject(m_bossChest);
			}
			m_teleportingOut = false;
			m_bossTitle1.Opacity = 0f;
			m_bossTitle2.Opacity = 0f;
			m_bossDivider.ScaleX = 0f;
			m_bossDivider.Opacity = 0f;
			base.OnEnter();
		}
		public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler)
		{
			SoundManager.PlaySound("Boss_Title");
			m_bossTitle1.Text = bossTitle1;
			m_bossTitle2.Text = bossTitle2;
			Camera2D camera = Player.AttachedLevel.Camera;
			if (Player.AttachedLevel.CurrentRoom is LastBossRoom)
			{
				m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 100f);
			}
			else
			{
				m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 50f);
			}
			m_bossTitle2.X = m_bossTitle1.X - 0f;
			m_bossTitle2.Y = m_bossTitle1.Y + 50f;
			m_bossDivider.Position = m_bossTitle1.Position;
			m_bossDivider.Y += m_bossTitle1.Height - 5;
			m_bossTitle1.X -= 1000f;
			m_bossTitle2.X += 1500f;
			Tween.To(m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.To(m_bossDivider, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0",
				"ScaleX",
				((float)(m_bossTitle2.Width / 5)).ToString()
			});
			Tween.To(m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.To(m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"1"
			});
			Tween.By(m_bossTitle1, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				"1000"
			});
			Tween.By(m_bossTitle2, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				"-1500"
			});
			m_bossTitle1.X += 1000f;
			m_bossTitle2.X -= 1500f;
			Tween.By(m_bossTitle1, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"20"
			});
			Tween.By(m_bossTitle2, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"-20"
			});
			m_bossTitle1.X -= 1000f;
			m_bossTitle2.X += 1500f;
			Tween.AddEndHandlerToLastTween(this, endHandler, new object[0]);
			Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Title_Exit"
			});
			m_bossTitle1.X += 1020f;
			m_bossTitle2.X -= 1520f;
			m_bossTitle1.Opacity = 1f;
			m_bossTitle2.Opacity = 1f;
			Tween.To(m_bossTitle1, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"3",
				"Opacity",
				"0"
			});
			Tween.To(m_bossTitle2, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"3",
				"Opacity",
				"0"
			});
			Tween.By(m_bossTitle1, 0.6f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"3",
				"X",
				"1500"
			});
			Tween.By(m_bossTitle2, 0.6f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"3",
				"X",
				"-1000"
			});
			m_bossTitle1.Opacity = 0f;
			m_bossTitle2.Opacity = 0f;
			m_bossDivider.Opacity = 1f;
			Tween.To(m_bossDivider, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"2.8",
				"Opacity",
				"0"
			});
			m_bossDivider.Opacity = 0f;
		}
		public override void Update(GameTime gameTime)
		{
			if (!m_cutsceneRunning)
			{
				base.Update(gameTime);
			}
			if (BossKilled && !m_bossChest.Visible)
			{
				BossCleanup();
				m_bossChest.Visible = true;
				m_bossChest.Opacity = 0f;
				SoundManager.PlayMusic("TitleScreenSong", true, 1f);
				Tween.To(m_bossChest, 4f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.To(m_bossChest, 4f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					m_roomFloor.ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "UnlockChest", new object[0]);
				m_sparkleTimer = 0.5f;
			}
			if (m_bossChest.Visible && !m_bossChest.IsOpen && BossKilled)
			{
				if (m_sparkleTimer > 0f)
				{
					m_sparkleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (m_sparkleTimer <= 0f)
					{
						m_sparkleTimer = 0.5f;
						Tween.RunFunction(0f, Player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect", new object[]
						{
							new Vector2(m_bossChest.X, m_bossChest.Y - m_bossChest.Height / 2)
						});
						return;
					}
				}
			}
			else if (m_bossChest.Visible && m_bossChest.IsOpen && BossKilled && !m_teleportingOut)
			{
				m_teleportingOut = true;
				if (LevelEV.RUN_DEMO_VERSION)
				{
					(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(29, true, null);
					return;
				}
				TeleportPlayer();
			}
		}
		public virtual void BossCleanup()
		{
			Player.StopAllSpells();
			Game.PlayerStats.NewBossBeaten = true;
			if (LinkedRoom != null)
			{
				Player.AttachedLevel.CloseBossDoor(LinkedRoom, LevelType);
			}
		}
		public void TeleportPlayer()
		{
			Player.CurrentSpeed = 0f;
			Vector2 position = Player.Position;
			Vector2 scale = Player.Scale;
			Tween.To(Player, 0.05f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"1.2",
				"ScaleX",
				"0"
			});
			Player.ScaleX = 0f;
			Tween.To(Player, 0.05f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"7",
				"ScaleX",
				scale.X.ToString()
			});
			Player.ScaleX = scale.X;
			LogicSet logicSet = new LogicSet(Player);
			logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlaySoundLogicAction(new string[]
			{
				"Teleport_Disappear"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[]
			{
				new Vector2(Player.X, Player.Bounds.Bottom),
				Player.Scale
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			if (LinkedRoom != null)
			{
				Player.Position = new Vector2(Player.AttachedLevel.RoomList[1].Bounds.Center.X, Player.AttachedLevel.RoomList[1].Bounds.Center.Y);
				Player.UpdateCollisionBoxes();
				logicSet.AddAction(new TeleportLogicAction(null, Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.RoomList[1], "RevealSymbol", new object[]
				{
					LevelType,
					true
				}), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(3.5f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
				Player.Position = new Vector2(LinkedRoom.Bounds.Center.X, LinkedRoom.Bounds.Bottom - 60 - (Player.Bounds.Bottom - Player.Y));
				Player.UpdateCollisionBoxes();
				logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false), Types.Sequence.Serial);
				logicSet.AddAction(new TeleportLogicAction(null, Player.Position), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
				logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[]
				{
					new Vector2(Player.X, LinkedRoom.Bounds.Bottom - 60),
					scale
				}), Types.Sequence.Serial);
				logicSet.AddAction(new PlaySoundLogicAction(new string[]
				{
					"Teleport_Reappear"
				}), Types.Sequence.Serial);
			}
			logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls", new object[0]), Types.Sequence.Serial);
			Player.RunExternalLogicSet(logicSet);
			Player.Position = position;
			Player.UpdateCollisionBoxes();
		}
		public void UnlockChest()
		{
			m_bossChest.IsLocked = false;
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			m_bossDivider.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			m_bossTitle1.Draw(camera);
			m_bossTitle2.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_bossChest = null;
				m_bossDivider.Dispose();
				m_bossDivider = null;
				m_bossTitle1.Dispose();
				m_bossTitle1 = null;
				m_bossTitle2.Dispose();
				m_bossTitle2 = null;
				base.Dispose();
			}
		}
	}
}
