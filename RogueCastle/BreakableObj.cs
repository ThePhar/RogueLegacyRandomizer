using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class BreakableObj : PhysicsObjContainer
	{
		private bool m_internalIsWeighted;
		public bool Broken
		{
			get;
			internal set;
		}
		public bool DropItem
		{
			get;
			set;
		}
		public bool HitBySpellsOnly
		{
			get;
			set;
		}
		public BreakableObj(string spriteName) : base(spriteName, null)
		{
			base.DisableCollisionBoxRotations = true;
			this.Broken = false;
			base.OutlineWidth = 2;
			base.SameTypesCollide = true;
			base.CollisionTypeTag = 5;
			base.CollidesLeft = false;
			base.CollidesRight = false;
			base.CollidesBottom = false;
			foreach (GameObj current in this._objectList)
			{
				current.Visible = false;
			}
			this._objectList[0].Visible = true;
			this.DropItem = true;
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (playerObj != null && otherBox.Type == 1 && !this.HitBySpellsOnly && !this.Broken)
			{
				this.Break();
			}
			ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
			if (projectileObj != null && (projectileObj.CollisionTypeTag == 2 || projectileObj.CollisionTypeTag == 10) && otherBox.Type == 1)
			{
				if (!this.Broken)
				{
					this.Break();
				}
				if (projectileObj.DestroysWithTerrain && base.SpriteName == "Target1_Character")
				{
					projectileObj.RunDestroyAnimation(false);
				}
			}
			if ((otherBox.AbsRect.Y > thisBox.AbsRect.Y || otherBox.AbsRotation != 0f) && (otherBox.Parent is TerrainObj || otherBox.AbsParent is BreakableObj))
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		public void Break()
		{
			PlayerObj player = Game.ScreenManager.Player;
			foreach (GameObj current in this._objectList)
			{
				current.Visible = true;
			}
			base.GoToFrame(2);
			this.Broken = true;
			this.m_internalIsWeighted = base.IsWeighted;
			base.IsWeighted = false;
			base.IsCollidable = false;
			if (this.DropItem)
			{
				bool flag = false;
				if (base.Name == "Health")
				{
					player.AttachedLevel.ItemDropManager.DropItem(base.Position, 2, 0.1f);
					flag = true;
				}
				else if (base.Name == "Mana")
				{
					player.AttachedLevel.ItemDropManager.DropItem(base.Position, 3, 0.1f);
					flag = true;
				}
				if (flag)
				{
					for (int i = 0; i < base.NumChildren; i++)
					{
						Tween.By(base.GetChildAt(i), 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"X",
							CDGMath.RandomInt(-50, 50).ToString(),
							"Y",
							"50",
							"Rotation",
							CDGMath.RandomInt(-360, 360).ToString()
						});
						Tween.To(base.GetChildAt(i), 0.1f, new Easing(Linear.EaseNone), new string[]
						{
							"delay",
							"0.2",
							"Opacity",
							"0"
						});
					}
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
					{
						"EnemyHit1",
						"EnemyHit2",
						"EnemyHit3",
						"EnemyHit4",
						"EnemyHit5",
						"EnemyHit6"
					});
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
					{
						"Break1",
						"Break2",
						"Break3"
					});
					if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f)
					{
						player.CurrentMana += 1f;
						player.AttachedLevel.TextManager.DisplayNumberStringText(1, "mp", Color.RoyalBlue, new Vector2(player.X, (float)(player.Bounds.Top - 30)));
					}
					return;
				}
				int num = CDGMath.RandomInt(1, 100);
				int num2 = 0;
				int j = 0;
				while (j < GameEV.BREAKABLE_ITEMDROP_CHANCE.Length)
				{
					num2 += GameEV.BREAKABLE_ITEMDROP_CHANCE[j];
					if (num <= num2)
					{
						if (j == 0)
						{
							if (Game.PlayerStats.Traits.X != 24f && Game.PlayerStats.Traits.Y != 24f)
							{
								player.AttachedLevel.ItemDropManager.DropItem(base.Position, 2, 0.1f);
								break;
							}
							EnemyObj_Chicken enemyObj_Chicken = new EnemyObj_Chicken(null, null, null, GameTypes.EnemyDifficulty.BASIC);
							enemyObj_Chicken.AccelerationY = -500f;
							enemyObj_Chicken.Position = base.Position;
							enemyObj_Chicken.Y -= 50f;
							enemyObj_Chicken.SaveToFile = false;
							player.AttachedLevel.AddEnemyToCurrentRoom(enemyObj_Chicken);
							enemyObj_Chicken.IsCollidable = false;
							Tween.RunFunction(0.2f, enemyObj_Chicken, "MakeCollideable", new object[0]);
							SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
							{
								"Chicken_Cluck_01",
								"Chicken_Cluck_02",
								"Chicken_Cluck_03"
							});
							break;
						}
						else
						{
							if (j == 1)
							{
								player.AttachedLevel.ItemDropManager.DropItem(base.Position, 3, 0.1f);
								break;
							}
							if (j == 2)
							{
								player.AttachedLevel.ItemDropManager.DropItem(base.Position, 1, 10f);
								break;
							}
							if (j == 3)
							{
								player.AttachedLevel.ItemDropManager.DropItem(base.Position, 10, 100f);
								break;
							}
							break;
						}
					}
					else
					{
						j++;
					}
				}
			}
			for (int k = 0; k < base.NumChildren; k++)
			{
				Tween.By(base.GetChildAt(k), 0.3f, new Easing(Linear.EaseNone), new string[]
				{
					"X",
					CDGMath.RandomInt(-50, 50).ToString(),
					"Y",
					"50",
					"Rotation",
					CDGMath.RandomInt(-360, 360).ToString()
				});
				Tween.To(base.GetChildAt(k), 0.1f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"0"
				});
			}
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
			{
				"EnemyHit1",
				"EnemyHit2",
				"EnemyHit3",
				"EnemyHit4",
				"EnemyHit5",
				"EnemyHit6"
			});
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
			{
				"Break1",
				"Break2",
				"Break3"
			});
			if (Game.PlayerStats.Traits.X == 15f || Game.PlayerStats.Traits.Y == 15f)
			{
				player.CurrentMana += 1f;
				player.AttachedLevel.TextManager.DisplayNumberStringText(1, "mp", Color.RoyalBlue, new Vector2(player.X, (float)(player.Bounds.Top - 30)));
			}
		}
		public void ForceBreak()
		{
			foreach (GameObj current in this._objectList)
			{
				current.Visible = true;
				current.Opacity = 0f;
			}
			base.GoToFrame(2);
			this.Broken = true;
			this.m_internalIsWeighted = base.IsWeighted;
			base.IsWeighted = false;
			base.IsCollidable = false;
		}
		public void Reset()
		{
			base.GoToFrame(1);
			this.Broken = false;
			base.IsWeighted = this.m_internalIsWeighted;
			base.IsCollidable = true;
			this.ChangeSprite(this._spriteName);
			for (int i = 0; i < base.NumChildren; i++)
			{
				base.GetChildAt(i).Opacity = 1f;
				base.GetChildAt(i).Rotation = 0f;
			}
			foreach (GameObj current in this._objectList)
			{
				current.Visible = false;
			}
			this._objectList[0].Visible = true;
		}
		public void UpdateTerrainBox()
		{
			foreach (CollisionBox current in base.CollisionBoxes)
			{
				if (current.Type == 0)
				{
					this.m_terrainBounds = current.AbsRect;
					break;
				}
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new BreakableObj(this._spriteName);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			BreakableObj breakableObj = obj as BreakableObj;
			breakableObj.HitBySpellsOnly = this.HitBySpellsOnly;
			breakableObj.DropItem = this.DropItem;
		}
	}
}
