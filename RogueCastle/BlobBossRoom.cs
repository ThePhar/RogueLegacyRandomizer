/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
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
				return ActiveEnemies == 0;
			}
		}
		public int NumActiveBlobs
		{
			get
			{
				int num = 0;
				foreach (EnemyObj current in EnemyList)
				{
					if (current.Type == 2 && !current.IsKilled)
					{
						num++;
					}
				}
				foreach (EnemyObj current2 in TempEnemyList)
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
			m_boss1 = (EnemyList[0] as EnemyObj_Blob);
			m_boss1.PauseEnemy(true);
			m_boss1.DisableAllWeight = false;
			m_desiredBossScale = m_boss1.Scale.X;
			m_blobArray = new List<ObjContainer>();
			for (int i = 0; i < m_numIntroBlobs; i++)
			{
				ObjContainer objContainer = new ObjContainer("EnemyBlobBossAir_Character");
				objContainer.Position = m_boss1.Position;
				objContainer.Scale = new Vector2(0.4f, 0.4f);
				objContainer.GetChildAt(0).TextureColor = Color.White;
				objContainer.GetChildAt(2).TextureColor = Color.LightSkyBlue;
				objContainer.GetChildAt(2).Opacity = 0.8f;
				(objContainer.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
				objContainer.Y -= 1000f;
				m_blobArray.Add(objContainer);
				GameObjList.Add(objContainer);
			}
			base.Initialize();
		}
		public override void OnEnter()
		{
			m_boss1.Name = "Herodotus";
			m_boss1.GetChildAt(0).TextureColor = Color.White;
			m_boss1.GetChildAt(2).TextureColor = Color.LightSkyBlue;
			m_boss1.GetChildAt(2).Opacity = 0.8f;
			(m_boss1.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
			m_boss1.GetChildAt(1).TextureColor = Color.Black;
			SoundManager.StopMusic(0.5f);
			Player.LockControls();
			Player.AttachedLevel.UpdateCamera();
			Player.AttachedLevel.CameraLockedToPlayer = false;
			Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(Bounds.Left + 700).ToString(),
				"Y",
				m_boss1.Y.ToString()
			});
			Tween.By(m_blobArray[0], 1f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"0.5",
				"Y",
				"1150"
			});
			Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
			{
				m_blobArray[0]
			});
			Tween.By(m_blobArray[1], 1f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"1.5",
				"Y",
				"1150"
			});
			Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
			{
				m_blobArray[1]
			});
			Tween.RunFunction(1f, this, "DropBlobs", new object[0]);
			m_boss1.Scale = new Vector2(0.5f, 0.5f);
			Player.AttachedLevel.RunCinematicBorders(9f);
			base.OnEnter();
		}
		public void DropBlobs()
		{
			float num = 1f;
			for (int i = 2; i < m_blobArray.Count; i++)
			{
				Tween.By(m_blobArray[i], 1f, new Easing(Quad.EaseIn), new string[]
				{
					"delay",
					num.ToString(),
					"Y",
					"1150"
				});
				Tween.AddEndHandlerToLastTween(this, "GrowBlob", new object[]
				{
					m_blobArray[i]
				});
				num += 0.5f * (m_blobArray.Count - i) / m_blobArray.Count;
			}
			Tween.RunFunction(num + 1f, m_boss1, "PlayAnimation", new object[]
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
				m_boss1.Name,
				"Intro2"
			});
			Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Blob_Spawn"
			});
		}
		public void GrowBlob(GameObj blob)
		{
			float num = (m_desiredBossScale - 0.5f) / m_numIntroBlobs;
			blob.Visible = false;
			m_boss1.PlayAnimation(false);
			m_boss1.ScaleX += num;
			m_boss1.ScaleY += num;
			SoundManager.PlaySound(new string[]
			{
				"Boss_Blob_Spawn_01",
				"Boss_Blob_Spawn_02",
				"Boss_Blob_Spawn_03"
			});
		}
		public void Intro2()
		{
			m_boss1.PlayAnimation(true);
			Tween.To(Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"X",
				(Player.X + GlobalEV.Camera_XOffset).ToString(),
				"Y",
				(Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "BeginBattle", new object[0]);
		}
		public void BeginBattle()
		{
			SoundManager.PlayMusic("DungeonBoss", true, 1f);
			Player.AttachedLevel.CameraLockedToPlayer = true;
			Player.UnlockControls();
			m_boss1.UnpauseEnemy(true);
			m_boss1.PlayAnimation(true);
		}
		public override void Update(GameTime gameTime)
		{
			Rectangle bounds = Bounds;
			foreach (EnemyObj current in EnemyList)
			{
				if (current.Type == 2 && !current.IsKilled && (current.X > Bounds.Right - 20 || current.X < Bounds.Left + 20 || current.Y > Bounds.Bottom - 20 || current.Y < Bounds.Top + 20))
				{
					current.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
				}
			}
			foreach (EnemyObj current2 in TempEnemyList)
			{
				if (current2.Type == 2 && !current2.IsKilled && (current2.X > Bounds.Right - 20 || current2.X < Bounds.Left + 20 || current2.Y > Bounds.Bottom - 20 || current2.Y < Bounds.Top + 20))
				{
					current2.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_blobArray.Clear();
				m_blobArray = null;
				m_boss1 = null;
				base.Dispose();
			}
		}
	}
}
