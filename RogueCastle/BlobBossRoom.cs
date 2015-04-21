using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class BlobBossRoom : BossRoomObj
	{
		private EnemyObj_Blob m_boss1;
		private List<ObjContainer> m_blobArray;
		private float m_desiredBossScale;
		private int m_numIntroBlobs = 10;
		public override bool BossKilled
		{
			get
			{
				return base.ActiveEnemies == 0;
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
		public override void Initialize()
		{
			this.m_boss1 = (base.EnemyList[0] as EnemyObj_Blob);
			this.m_boss1.PauseEnemy(true);
			this.m_boss1.DisableAllWeight = false;
			this.m_desiredBossScale = this.m_boss1.Scale.X;
			this.m_blobArray = new List<ObjContainer>();
			for (int i = 0; i < this.m_numIntroBlobs; i++)
			{
				ObjContainer objContainer = new ObjContainer("EnemyBlobBossAir_Character");
				objContainer.Position = this.m_boss1.Position;
				objContainer.Scale = new Vector2(0.4f, 0.4f);
				objContainer.GetChildAt(0).TextureColor = Color.White;
				objContainer.GetChildAt(2).TextureColor = Color.LightSkyBlue;
				objContainer.GetChildAt(2).Opacity = 0.8f;
				(objContainer.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
				objContainer.Y -= 1000f;
				this.m_blobArray.Add(objContainer);
				base.GameObjList.Add(objContainer);
			}
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.m_boss1.Name = "Herodotus";
			this.m_boss1.GetChildAt(0).TextureColor = Color.White;
			this.m_boss1.GetChildAt(2).TextureColor = Color.LightSkyBlue;
			this.m_boss1.GetChildAt(2).Opacity = 0.8f;
			(this.m_boss1.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
			this.m_boss1.GetChildAt(1).TextureColor = Color.Black;
			SoundManager.StopMusic(0.5f);
			this.Player.LockControls();
			this.Player.AttachedLevel.UpdateCamera();
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(this.Bounds.Left + 700).ToString(),
				"Y",
				this.m_boss1.Y.ToString()
			});
			Tween.By(this.m_blobArray[0], 1f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"0.5",
				"Y",
				"1150"
			});
			Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
			{
				this.m_blobArray[0]
			});
			Tween.By(this.m_blobArray[1], 1f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"1.5",
				"Y",
				"1150"
			});
			Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
			{
				this.m_blobArray[1]
			});
			Tween.RunFunction(1f, this, "DropBlobs", new object[0]);
			this.m_boss1.Scale = new Vector2(0.5f, 0.5f);
			this.Player.AttachedLevel.RunCinematicBorders(9f);
			base.OnEnter();
		}
		public void DropBlobs()
		{
			float num = 1f;
			for (int i = 2; i < this.m_blobArray.Count; i++)
			{
				Tween.By(this.m_blobArray[i], 1f, new Easing(Quad.EaseIn), new string[]
				{
					"delay",
					num.ToString(),
					"Y",
					"1150"
				});
				Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
				{
					this.m_blobArray[i]
				});
				num += 0.5f * (float)(this.m_blobArray.Count - i) / (float)this.m_blobArray.Count;
			}
			Tween.RunFunction(num + 1f, this.m_boss1, "PlayAnimation", new object[]
			{
				true
			});
			Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Blob_Idle_Loop"
			});
			Tween.RunFunction(num + 1f, this, "DisplayBossTitle", new object[]
			{
				"The Infinite",
				this.m_boss1.Name,
				"Intro2"
			});
			Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Blob_Spawn"
			});
		}
		public void GrowBlob(GameObj blob)
		{
			float num = (this.m_desiredBossScale - 0.5f) / (float)this.m_numIntroBlobs;
			blob.Visible = false;
			this.m_boss1.PlayAnimation(false);
			this.m_boss1.ScaleX += num;
			this.m_boss1.ScaleY += num;
			SoundManager.PlaySound(new string[]
			{
				"Boss_Blob_Spawn_01",
				"Boss_Blob_Spawn_02",
				"Boss_Blob_Spawn_03"
			});
		}
		public void Intro2()
		{
			this.m_boss1.PlayAnimation(true);
			Tween.To(this.Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"X",
				(this.Player.X + GlobalEV.Camera_XOffset).ToString(),
				"Y",
				((float)this.Bounds.Bottom - ((float)this.Player.AttachedLevel.Camera.Bounds.Bottom - this.Player.AttachedLevel.Camera.Y)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "BeginBattle", new object[0]);
		}
		public void BeginBattle()
		{
			SoundManager.PlayMusic("DungeonBoss", true, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_boss1.UnpauseEnemy(true);
			this.m_boss1.PlayAnimation(true);
		}
		public override void Update(GameTime gameTime)
		{
			Rectangle bounds = this.Bounds;
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current.Type == 2 && !current.IsKilled && (current.X > (float)(this.Bounds.Right - 20) || current.X < (float)(this.Bounds.Left + 20) || current.Y > (float)(this.Bounds.Bottom - 20) || current.Y < (float)(this.Bounds.Top + 20)))
				{
					current.Position = new Vector2((float)bounds.Center.X, (float)bounds.Center.Y);
				}
			}
			foreach (EnemyObj current2 in base.TempEnemyList)
			{
				if (current2.Type == 2 && !current2.IsKilled && (current2.X > (float)(this.Bounds.Right - 20) || current2.X < (float)(this.Bounds.Left + 20) || current2.Y > (float)(this.Bounds.Bottom - 20) || current2.Y < (float)(this.Bounds.Top + 20)))
				{
					current2.Position = new Vector2((float)bounds.Center.X, (float)bounds.Center.Y);
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_blobArray.Clear();
				this.m_blobArray = null;
				this.m_boss1 = null;
				base.Dispose();
			}
		}
	}
}
