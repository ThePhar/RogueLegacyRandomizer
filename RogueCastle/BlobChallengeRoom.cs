using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class BlobChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_Blob m_boss;
		private EnemyObj_Blob m_boss2;
		private Vector2 m_startingCamPos;
		public override bool BossKilled
		{
			get
			{
				return this.NumActiveBlobs == 0;
			}
		}
		public int NumActiveBlobs
		{
			get
			{
				int num = 0;
				foreach (EnemyObj current in base.EnemyList)
				{
					if (current.Type == 2 && !current.IsKilled)
					{
						num++;
					}
				}
				foreach (EnemyObj current2 in base.TempEnemyList)
				{
					if (current2.Type == 2 && !current2.IsKilled)
					{
						num++;
					}
				}
				return num;
			}
		}
		public BlobChallengeRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Blob);
			this.m_boss.SaveToFile = false;
			this.m_boss2 = (base.EnemyList[1] as EnemyObj_Blob);
			this.m_boss2.SaveToFile = false;
			base.Initialize();
		}
		private void SetRoomData()
		{
			this.m_boss.Name = "Astrodotus";
			this.m_boss.GetChildAt(0).TextureColor = Color.Green;
			this.m_boss.GetChildAt(2).TextureColor = Color.LightGreen;
			this.m_boss.GetChildAt(2).Opacity = 0.8f;
			(this.m_boss.GetChildAt(1) as SpriteObj).OutlineColour = Color.Red;
			this.m_boss.GetChildAt(1).TextureColor = Color.Red;
			this.m_boss2.GetChildAt(0).TextureColor = Color.Red;
			this.m_boss2.GetChildAt(2).TextureColor = Color.LightPink;
			this.m_boss2.GetChildAt(2).Opacity = 0.8f;
			(this.m_boss2.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
			this.m_boss2.GetChildAt(1).TextureColor = Color.DarkGray;
			this.m_boss.Level = 100;
			this.m_boss.MaxHealth = 100;
			this.m_boss.Damage = 370;
			this.m_boss.IsWeighted = false;
			this.m_boss.TurnSpeed = 0.015f;
			this.m_boss.Speed = 400f;
			this.m_boss.IsNeo = true;
			this.m_boss.ChangeNeoStats(0.8f, 1.06f, 6);
			this.m_boss.Scale = new Vector2(2f, 2f);
			this.m_boss2.Level = this.m_boss.Level;
			this.m_boss2.MaxHealth = this.m_boss.MaxHealth;
			this.m_boss2.Damage = this.m_boss.Damage;
			this.m_boss2.IsWeighted = this.m_boss.IsWeighted;
			this.m_boss2.TurnSpeed = 0.01f;
			this.m_boss2.Speed = 625f;
			this.m_boss2.IsNeo = this.m_boss.IsNeo;
			this.m_boss2.ChangeNeoStats(0.75f, 1.16f, 5);
			this.m_boss2.Scale = this.m_boss.Scale;
			Game.PlayerStats.PlayerName = "Lady Echidna";
			Game.PlayerStats.Class = 16;
			Game.PlayerStats.Spell = 15;
			Game.PlayerStats.IsFemale = true;
			Game.PlayerStats.BonusHealth = 90;
			Game.PlayerStats.BonusMana = 8;
			Game.PlayerStats.BonusStrength = 50;
			Game.PlayerStats.BonusMagic = 33;
			Game.PlayerStats.BonusDefense = 230;
			Game.PlayerStats.Traits = new Vector2(2f, 17f);
			this.Player.CanBeKnockedBack = false;
			Game.PlayerStats.GetEquippedArray[0] = 8;
			Game.PlayerStats.GetEquippedArray[1] = 8;
			Game.PlayerStats.GetEquippedArray[3] = 8;
			Game.PlayerStats.GetEquippedArray[2] = 8;
			Game.PlayerStats.GetEquippedRuneArray[1] = 7;
			Game.PlayerStats.GetEquippedRuneArray[2] = 7;
			this.Player.IsWeighted = false;
			if (this.m_boss != null)
			{
				this.m_boss.CurrentHealth = this.m_boss.MaxHealth;
			}
			if (this.m_boss2 != null)
			{
				this.m_boss2.CurrentHealth = this.m_boss2.MaxHealth;
			}
		}
		public override void OnEnter()
		{
			base.StorePlayerData();
			this.Player.Flip = SpriteEffects.FlipHorizontally;
			this.SetRoomData();
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.m_boss.AnimationDelay = 0.1f;
			this.m_boss.ChangeSprite("EnemyBlobBossAir_Character");
			this.m_boss.PlayAnimation(true);
			this.m_boss2.AnimationDelay = 0.1f;
			this.m_boss2.ChangeSprite("EnemyBlobBossAir_Character");
			this.m_boss2.PlayAnimation(true);
			this.Player.AttachedLevel.UpdateCamera();
			this.m_startingCamPos = this.Player.AttachedLevel.Camera.Position;
			this.Player.LockControls();
			this.Player.AttachedLevel.RunCinematicBorders(6f);
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				this.m_boss.Y.ToString(),
				"X",
				this.m_boss.X.ToString()
			});
			Tween.RunFunction(1.2f, this, "DisplayBossTitle", new object[]
			{
				Game.PlayerStats.PlayerName + " VS",
				this.m_boss.Name,
				"Intro2"
			});
			base.OnEnter();
			this.m_bossChest.ForcedItemType = 18;
		}
		public void Intro2()
		{
			object arg_96_0 = this.Player.AttachedLevel.Camera;
			float arg_96_1 = 1f;
			Easing arg_96_2 = new Easing(Quad.EaseInOut);
			string[] array = new string[8];
			array[0] = "delay";
			array[1] = "0.5";
			array[2] = "X";
			string[] arg_5D_0 = array;
			int arg_5D_1 = 3;
			int x = this.Bounds.Center.X;
			arg_5D_0[arg_5D_1] = x.ToString();
			array[4] = "Y";
			string[] arg_84_0 = array;
			int arg_84_1 = 5;
			int y = this.Bounds.Center.Y;
			arg_84_0[arg_84_1] = y.ToString();
			array[6] = "Zoom";
			array[7] = "0.5";
			Tween.To(arg_96_0, arg_96_1, arg_96_2, array);
			Tween.AddEndHandlerToLastTween(this, "EndCutscene", new object[0]);
		}
		public void EndCutscene()
		{
			this.m_boss.Rotation = 0f;
			this.Player.IsWeighted = true;
			SoundManager.PlayMusic("DungeonBoss", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.UnlockControls();
			this.m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			Rectangle bounds = this.Bounds;
			if (this.Player.Y > (float)bounds.Bottom)
			{
				this.Player.Y = (float)(bounds.Top + 20);
			}
			else if (this.Player.Y < (float)bounds.Top)
			{
				this.Player.Y = (float)(bounds.Bottom - 20);
			}
			if (this.Player.X > (float)bounds.Right)
			{
				this.Player.X = (float)(bounds.Left + 20);
			}
			else if (this.Player.X < (float)bounds.Left)
			{
				this.Player.X = (float)(bounds.Right - 20);
			}
			List<EnemyObj> list = this.Player.AttachedLevel.CurrentRoom.EnemyList;
			foreach (EnemyObj current in list)
			{
				if (current.Y > (float)(bounds.Bottom - 10))
				{
					current.Y = (float)(bounds.Top + 20);
				}
				else if (current.Y < (float)(bounds.Top + 10))
				{
					current.Y = (float)(bounds.Bottom - 20);
				}
				if (current.X > (float)(bounds.Right - 10))
				{
					current.X = (float)(bounds.Left + 20);
				}
				else if (current.X < (float)(bounds.Left + 10))
				{
					current.X = (float)(bounds.Right - 20);
				}
			}
			list = this.Player.AttachedLevel.CurrentRoom.TempEnemyList;
			foreach (EnemyObj current2 in list)
			{
				if (current2.Y > (float)(bounds.Bottom - 10))
				{
					current2.Y = (float)(bounds.Top + 20);
				}
				else if (current2.Y < (float)(bounds.Top + 10))
				{
					current2.Y = (float)(bounds.Bottom - 20);
				}
				if (current2.X > (float)(bounds.Right - 10))
				{
					current2.X = (float)(bounds.Left + 20);
				}
				else if (current2.X < (float)(bounds.Left + 10))
				{
					current2.X = (float)(bounds.Right - 20);
				}
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			Vector2 position = this.Player.Position;
			if (this.Player.X - (float)this.Player.Width / 2f < base.X)
			{
				this.Player.Position = new Vector2(this.Player.X + (float)this.Width, this.Player.Y);
				this.Player.Draw(camera);
			}
			else if (this.Player.X + (float)this.Player.Width / 2f > base.X + (float)this.Width)
			{
				this.Player.Position = new Vector2(this.Player.X - (float)this.Width, this.Player.Y);
				this.Player.Draw(camera);
			}
			if (this.Player.Y - (float)this.Player.Height / 2f < base.Y)
			{
				this.Player.Position = new Vector2(this.Player.X, this.Player.Y + (float)this.Height);
				this.Player.Draw(camera);
			}
			else if (this.Player.Y + (float)this.Player.Height / 2f > base.Y + (float)this.Height)
			{
				this.Player.Position = new Vector2(this.Player.X, this.Player.Y - (float)this.Height);
				this.Player.Draw(camera);
			}
			this.Player.Position = position;
			foreach (EnemyObj current in base.EnemyList)
			{
				Vector2 position2 = current.Position;
				Rectangle pureBounds = current.PureBounds;
				if (current.X - (float)current.Width / 2f < base.X)
				{
					current.Position = new Vector2(current.X + (float)this.Width, current.Y);
					current.Draw(camera);
				}
				else if (current.X + (float)current.Width / 2f > base.X + (float)this.Width)
				{
					current.Position = new Vector2(current.X - (float)this.Width, current.Y);
					current.Draw(camera);
				}
				if ((float)pureBounds.Top < base.Y)
				{
					current.Position = new Vector2(current.X, current.Y + (float)this.Height);
					current.Draw(camera);
				}
				else if ((float)pureBounds.Bottom > base.Y + (float)this.Height)
				{
					current.Position = new Vector2(current.X, current.Y - (float)this.Height);
					current.Draw(camera);
				}
				current.Position = position2;
			}
			foreach (EnemyObj current2 in base.TempEnemyList)
			{
				current2.ForceDraw = true;
				Vector2 position3 = current2.Position;
				Rectangle pureBounds2 = current2.PureBounds;
				if (current2.X - (float)current2.Width / 2f < base.X)
				{
					current2.Position = new Vector2(current2.X + (float)this.Width, current2.Y);
					current2.Draw(camera);
				}
				else if (current2.X + (float)current2.Width / 2f > base.X + (float)this.Width)
				{
					current2.Position = new Vector2(current2.X - (float)this.Width, current2.Y);
					current2.Draw(camera);
				}
				if ((float)pureBounds2.Top < base.Y)
				{
					current2.Position = new Vector2(current2.X, current2.Y + (float)this.Height);
					current2.Draw(camera);
				}
				else if ((float)pureBounds2.Bottom > base.Y + (float)this.Height)
				{
					current2.Position = new Vector2(current2.X, current2.Y - (float)this.Height);
					current2.Draw(camera);
				}
				current2.Position = position3;
			}
		}
		public override void OnExit()
		{
			if (!this.BossKilled)
			{
				foreach (EnemyObj current in base.EnemyList)
				{
					current.Reset();
				}
			}
			foreach (EnemyObj current2 in base.TempEnemyList)
			{
				current2.KillSilently();
				current2.Dispose();
			}
			base.TempEnemyList.Clear();
			this.Player.CanBeKnockedBack = true;
			base.OnExit();
		}
		protected override void SaveCompletionData()
		{
			Game.PlayerStats.ChallengeBlobBeaten = true;
			GameUtil.UnlockAchievement("FEAR_OF_SPACE");
		}
		protected override GameObj CreateCloneInstance()
		{
			return new BlobChallengeRoom();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_boss = null;
				this.m_boss2 = null;
				base.Dispose();
			}
		}
	}
}
