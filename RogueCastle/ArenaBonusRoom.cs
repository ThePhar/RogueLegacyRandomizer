using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class ArenaBonusRoom : BonusRoomObj
	{
		private ChestObj m_chest;
		private float m_chestStartingY;
		private bool m_chestRevealed;
		public override void Initialize()
		{
			foreach (GameObj current in base.GameObjList)
			{
				if (current is ChestObj)
				{
					this.m_chest = (current as ChestObj);
					break;
				}
			}
			this.m_chest.ChestType = 3;
			this.m_chestStartingY = this.m_chest.Y - 200f + (float)this.m_chest.Height + 6f;
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.UpdateEnemyNames();
			this.m_chest.Y = this.m_chestStartingY;
			this.m_chest.ChestType = 3;
			if (base.RoomCompleted)
			{
				this.m_chest.Opacity = 1f;
				this.m_chest.Y = this.m_chestStartingY + 200f;
				this.m_chest.IsEmpty = true;
				this.m_chest.ForceOpen();
				this.m_chestRevealed = true;
				using (List<EnemyObj>.Enumerator enumerator = base.EnemyList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EnemyObj current = enumerator.Current;
						if (!current.IsKilled)
						{
							current.KillSilently();
						}
					}
					goto IL_137;
				}
			}
			if (base.ActiveEnemies == 0)
			{
				this.m_chest.Opacity = 1f;
				this.m_chest.Y = this.m_chestStartingY + 200f;
				this.m_chest.IsEmpty = false;
				this.m_chest.IsLocked = false;
				this.m_chestRevealed = true;
			}
			else
			{
				this.m_chest.Opacity = 0f;
				this.m_chest.Y = this.m_chestStartingY;
				this.m_chest.IsLocked = true;
				this.m_chestRevealed = false;
			}
			IL_137:
			if (this.m_chest.PhysicsMngr == null)
			{
				this.Player.PhysicsMngr.AddObject(this.m_chest);
			}
			base.OnEnter();
		}
		private void UpdateEnemyNames()
		{
			bool flag = false;
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current is EnemyObj_EarthWizard)
				{
					if (!flag)
					{
						current.Name = "Barbatos";
						flag = true;
					}
					else
					{
						current.Name = "Amon";
					}
				}
				else if (current is EnemyObj_Skeleton)
				{
					if (!flag)
					{
						current.Name = "Berith";
						flag = true;
					}
					else
					{
						current.Name = "Halphas";
					}
				}
				else if (current is EnemyObj_Plant)
				{
					if (!flag)
					{
						current.Name = "Stolas";
						flag = true;
					}
					else
					{
						current.Name = "Focalor";
					}
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (!this.m_chest.IsOpen)
			{
				if (base.ActiveEnemies == 0 && this.m_chest.Opacity == 0f && !this.m_chestRevealed)
				{
					this.m_chestRevealed = true;
					this.DisplayChest();
				}
			}
			else if (!base.RoomCompleted)
			{
				base.RoomCompleted = true;
			}
			base.Update(gameTime);
		}
		public override void OnExit()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (Game.PlayerStats.EnemiesKilledList[15].W > 0f)
			{
				flag = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[22].W > 0f)
			{
				flag2 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[32].W > 0f)
			{
				flag3 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[12].W > 0f)
			{
				flag4 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[5].W > 0f)
			{
				flag5 = true;
			}
			if (flag && flag2 && flag3 && flag4 && flag5)
			{
				GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
			}
			base.OnExit();
		}
		private void DisplayChest()
		{
			this.m_chest.IsLocked = false;
			Tween.To(this.m_chest, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(this.m_chest, 2f, new Easing(Quad.EaseOut), new string[]
			{
				"Y",
				"200"
			});
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_chest = null;
				base.Dispose();
			}
		}
	}
}
