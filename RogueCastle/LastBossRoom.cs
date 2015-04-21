using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class LastBossRoom : BossRoomObj
	{
		private EnemyObj_LastBoss m_boss;
		private bool m_shake;
		private bool m_shookLeft;
		private float m_shakeTimer = 0.03f;
		private float m_shakeDuration = 0.03f;
		private ObjContainer m_fountain;
		private DoorObj m_bossDoor;
		private SpriteObj m_bossDoorSprite;
		private int m_bossCoins = 40;
		private int m_bossMoneyBags = 16;
		private int m_bossDiamonds = 12;
		private float m_playerX;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled && this.m_boss.IsSecondForm;
			}
		}
		public LastBossRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_LastBoss);
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "fountain")
				{
					this.m_fountain = (current as ObjContainer);
				}
				if (current.Name == "stainglass")
				{
					current.Opacity = 0.5f;
				}
				if (current.Name == "door")
				{
					this.m_bossDoorSprite = (current as SpriteObj);
				}
			}
			foreach (DoorObj current2 in base.DoorList)
			{
				if (current2.Name == "FinalBossDoor")
				{
					this.m_bossDoor = current2;
					this.m_bossDoor.Locked = true;
					break;
				}
			}
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.Player.AttachedLevel.RemoveCompassDoor();
			this.m_boss.Level += 8;
			this.m_boss.CurrentHealth = this.m_boss.MaxHealth;
			this.BackBufferOpacity = 0f;
			SoundManager.StopMusic(0.5f);
			this.StartCutscene();
			base.OnEnter();
		}
		public void StartCutscene()
		{
			this.m_cutsceneRunning = true;
			this.Player.LockControls();
			this.Player.AccelerationY = 0f;
			this.Player.AttachedLevel.RunCinematicBorders(8f);
			this.Player.Flip = SpriteEffects.None;
			this.Player.State = 1;
			LogicSet logicSet = new LogicSet(this.Player);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsWeighted", false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsCollidable", false), Types.Sequence.Serial);
			logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1.5f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsWeighted", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsCollidable", true), Types.Sequence.Serial);
			this.Player.RunExternalLogicSet(logicSet);
			Tween.RunFunction(1.6f, this, "Cutscene2", new object[0]);
		}
		public void Cutscene2()
		{
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			Tween.By(this.Player.AttachedLevel.Camera, 1.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"300"
			});
			Tween.AddEndHandlerToLastTween(this, "Cutscene3", new object[0]);
		}
		public void Cutscene3()
		{
			Tween.RunFunction(0.5f, this, "Cutscene4", new object[0]);
			RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
			if (Game.PlayerStats.Class == 17)
			{
				rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk01_Special");
				GameUtil.UnlockAchievement("LOVE_OF_LAUGHING_AT_OTHERS");
			}
			else
			{
				rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk01");
			}
			rCScreenManager.DisplayScreen(13, true, null);
		}
		public void Cutscene4()
		{
			Tween.RunFunction(0.5f, this, "DisplayBossTitle", new object[]
			{
				"The Traitor",
				this.m_boss.Name,
				"Cutscene5"
			});
		}
		public void Cutscene5()
		{
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.Player.X.ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "BeginBattle", new object[0]);
		}
		public void BeginBattle()
		{
			SoundManager.PlayMusic("TitleScreenSong", true, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_cutsceneRunning = false;
		}
		public void RunFountainCutscene()
		{
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			Camera2D camera = this.Player.AttachedLevel.Camera;
			this.m_playerX = camera.X;
			SoundManager.PlaySound("Cutsc_CameraMove");
			Tween.To(camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(this.m_fountain.Bounds.Center.X - 400).ToString()
			});
			Tween.RunFunction(2f, this, "RunFountainCutscene2", new object[0]);
		}
		public void RunFountainCutscene2()
		{
			this.StartShake();
			SoundManager.PlaySound("Cutsc_StatueCrumble");
			this.m_fountain.ChangeSprite("FountainOfYouthShatter_Character");
			this.m_fountain.PlayAnimation(false);
			this.Player.AttachedLevel.ImpactEffectPool.DisplayFountainShatterSmoke(this.m_fountain);
			Tween.RunFunction(2f, this, "DisplaySecondBoss", new object[0]);
			Tween.RunFunction(2f, this, "RunFountainCutscene3", new object[0]);
		}
		public void DisplaySecondBoss()
		{
			this.m_boss.SecondFormComplete();
			this.m_boss.UpdateCollisionBoxes();
			this.m_boss.Position = new Vector2(this.m_fountain.X, this.m_fountain.Y - ((float)this.m_boss.Bounds.Bottom - this.m_boss.Y));
		}
		public void RunFountainCutscene3()
		{
			SoundManager.PlaySound("FinalBoss_St2_BlockLaugh");
			SoundManager.PlayMusic("LastBossSong", true, 1f);
			this.m_fountain.Visible = false;
			this.StopShake();
			Tween.RunFunction(2f, this, "DisplayBossTitle", new object[]
			{
				"Johannes",
				"The Fountain",
				"RunFountainCutscene4"
			});
		}
		public void StartShake()
		{
			this.m_shake = true;
		}
		public void StopShake()
		{
			this.m_shake = false;
		}
		public void DisableFountain()
		{
			this.m_fountain.Visible = false;
		}
		public void RunFountainCutscene4()
		{
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_playerX.ToString()
			});
			Tween.AddEndHandlerToLastTween(this.m_boss, "SecondFormActive", new object[0]);
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_shake && this.m_shakeTimer > 0f)
			{
				this.m_shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_shakeTimer <= 0f)
				{
					Camera2D camera = this.Player.AttachedLevel.Camera;
					this.m_shakeTimer = this.m_shakeDuration;
					if (this.m_shookLeft)
					{
						this.m_shookLeft = false;
						camera.X += 5f;
					}
					else
					{
						camera.X -= 5f;
						this.m_shookLeft = true;
					}
				}
			}
			if (!this.m_cutsceneRunning)
			{
				foreach (EnemyObj current in base.EnemyList)
				{
					if (!current.IsKilled)
					{
						current.Update(gameTime);
					}
				}
				foreach (EnemyObj current2 in base.TempEnemyList)
				{
					if (!current2.IsKilled)
					{
						current2.Update(gameTime);
					}
				}
			}
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			camera.Draw(Game.GenericTexture, new Rectangle((int)base.X, (int)base.Y, this.Width, this.Height), Color.White * this.BackBufferOpacity);
		}
		public void ChangeWindowOpacity()
		{
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "stainglass")
				{
					Tween.To(current, 2f, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"0.2"
					});
				}
			}
		}
		public override void BossCleanup()
		{
			this.Player.StopAllSpells();
			Game.PlayerStats.NewBossBeaten = true;
			this.m_bossDoorSprite.ChangeSprite("CastleDoorOpen_Sprite");
			this.m_bossDoor.Locked = false;
			SoundManager.PlaySound("FinalBoss_St2_WeatherChange_b");
			this.DropGold();
			this.AddEnemyKilled();
		}
		private void AddEnemyKilled()
		{
			Game.PlayerStats.NumEnemiesBeaten++;
			Vector4 value = Game.PlayerStats.EnemiesKilledList[(int)this.m_boss.Type];
			value.X += 1f;
			value.Y += 1f;
			Game.PlayerStats.EnemiesKilledList[(int)this.m_boss.Type] = value;
		}
		private void DropGold()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.m_bossCoins; i++)
			{
				list.Add(0);
			}
			for (int j = 0; j < this.m_bossMoneyBags; j++)
			{
				list.Add(1);
			}
			for (int k = 0; k < this.m_bossDiamonds; k++)
			{
				list.Add(2);
			}
			CDGMath.Shuffle<int>(list);
			float num = 0f;
			SoundManager.PlaySound("Boss_Flash");
			for (int l = 0; l < list.Count; l++)
			{
				Vector2 position = this.m_boss.Position;
				if (list[l] == 0)
				{
					Tween.RunFunction((float)l * num, this.Player.AttachedLevel.ItemDropManager, "DropItemWide", new object[]
					{
						position,
						1,
						10
					});
				}
				else if (list[l] == 1)
				{
					Tween.RunFunction((float)l * num, this.Player.AttachedLevel.ItemDropManager, "DropItemWide", new object[]
					{
						position,
						10,
						100
					});
				}
				else
				{
					Tween.RunFunction((float)l * num, this.Player.AttachedLevel.ItemDropManager, "DropItemWide", new object[]
					{
						position,
						11,
						500
					});
				}
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new LastBossRoom();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_fountain = null;
				this.m_boss = null;
				base.Dispose();
			}
		}
	}
}
