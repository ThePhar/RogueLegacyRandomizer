/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class ImpactEffectPool : IDisposableObj
	{
		private int m_poolSize;
		private DS2DPool<SpriteObj> m_resourcePool;
		private bool m_isPaused;
		private bool m_isDisposed;
		public bool IsDisposed
		{
			get
			{
				return m_isDisposed;
			}
		}
		public int ActiveTextObjs
		{
			get
			{
				return m_resourcePool.NumActiveObjs;
			}
		}
		public int TotalPoolSize
		{
			get
			{
				return m_resourcePool.TotalPoolSize;
			}
		}
		public int CurrentPoolSize
		{
			get
			{
				return TotalPoolSize - ActiveTextObjs;
			}
		}
		public ImpactEffectPool(int poolSize)
		{
			m_poolSize = poolSize;
			m_resourcePool = new DS2DPool<SpriteObj>();
		}
		public void Initialize()
		{
			for (int i = 0; i < m_poolSize; i++)
			{
				SpriteObj spriteObj = new SpriteObj("Blank_Sprite");
				spriteObj.AnimationDelay = 0.0333333351f;
				spriteObj.Visible = false;
				spriteObj.TextureColor = Color.White;
				m_resourcePool.AddToPool(spriteObj);
			}
		}
		public SpriteObj DisplayEffect(Vector2 position, string spriteName)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite(spriteName);
			spriteObj.TextureColor = Color.White;
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
			return spriteObj;
		}
		public void DisplayEnemyImpactEffect(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ImpactEnemy_Sprite");
			spriteObj.TextureColor = Color.White;
			spriteObj.Rotation = CDGMath.RandomInt(0, 360);
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
		}
		public void DisplayPlayerImpactEffect(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ImpactEnemy_Sprite");
			spriteObj.TextureColor = Color.Orange;
			spriteObj.Rotation = CDGMath.RandomInt(0, 360);
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
		}
		public void DisplayBlockImpactEffect(Vector2 position, Vector2 scale)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ImpactBlock_Sprite");
			spriteObj.TextureColor = Color.White;
			spriteObj.Rotation = CDGMath.RandomInt(0, 360);
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
			spriteObj.Scale = scale;
		}
		public void DisplayDeathEffect(Vector2 position)
		{
			for (int i = 0; i < 10; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionBlue_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = position;
				float num = CDGMath.RandomFloat(0.7f, 0.8f);
				int num2 = 50;
				spriteObj.Scale = new Vector2(num, num);
				spriteObj.Rotation = CDGMath.RandomInt(0, 90);
				spriteObj.PlayAnimation(true);
				float num3 = CDGMath.RandomFloat(0.5f, 1f);
				float num4 = CDGMath.RandomFloat(0f, 0.1f);
				Tween.To(spriteObj, num3 - 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"0"
				});
				Tween.To(spriteObj, num3, new Easing(Back.EaseIn), new string[]
				{
					"ScaleX",
					num4.ToString(),
					"ScaleY",
					num4.ToString()
				});
				Tween.By(spriteObj, num3, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					CDGMath.RandomInt(-num2, num2).ToString(),
					"Y",
					CDGMath.RandomInt(-num2, num2).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				Tween.By(spriteObj, num3 - 0.1f, new Easing(Quad.EaseOut), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(145, 190).ToString()
				});
			}
		}
		public void DisplaySpawnEffect(Vector2 position)
		{
			for (int i = 0; i < 10; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionOrange_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = position;
				float num = CDGMath.RandomFloat(0.7f, 0.8f);
				int num2 = 50;
				spriteObj.Scale = new Vector2(num, num);
				spriteObj.Rotation = CDGMath.RandomInt(0, 90);
				spriteObj.PlayAnimation(true);
				float num3 = CDGMath.RandomFloat(0.5f, 1f);
				float num4 = CDGMath.RandomFloat(0f, 0.1f);
				Tween.To(spriteObj, num3 - 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"0"
				});
				Tween.To(spriteObj, num3, new Easing(Back.EaseIn), new string[]
				{
					"ScaleX",
					num4.ToString(),
					"ScaleY",
					num4.ToString()
				});
				Tween.By(spriteObj, num3, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					CDGMath.RandomInt(-num2, num2).ToString(),
					"Y",
					CDGMath.RandomInt(-num2, num2).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				Tween.By(spriteObj, num3 - 0.1f, new Easing(Quad.EaseOut), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(145, 190).ToString()
				});
			}
		}
		public void DisplayChestSparkleEffect(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("LevelUpParticleFX_Sprite");
			spriteObj.Visible = true;
			float num = CDGMath.RandomFloat(0.2f, 0.5f);
			spriteObj.Scale = new Vector2(num, num);
			spriteObj.Opacity = 0f;
			spriteObj.Position = position;
			spriteObj.Rotation = CDGMath.RandomInt(0, 90);
			spriteObj.PlayAnimation(false);
			spriteObj.Position += new Vector2(CDGMath.RandomInt(-40, 40), CDGMath.RandomInt(-40, 40));
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayDoubleJumpEffect(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("DoubleJumpFX_Sprite");
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
		}
		public void DisplayDashEffect(Vector2 position, bool flip)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("DashFX_Sprite");
			if (flip)
			{
				spriteObj.Flip = SpriteEffects.FlipHorizontally;
			}
			spriteObj.Position = position;
			spriteObj.Visible = true;
			spriteObj.PlayAnimation(false);
		}
		public void DisplayTeleportEffect(Vector2 position)
		{
			float num = 0.1f;
			for (int i = 0; i < 5; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.Visible = true;
				spriteObj.ChangeSprite("TeleportRock" + (i + 1) + "_Sprite");
				spriteObj.PlayAnimation(true);
				spriteObj.Position = new Vector2(CDGMath.RandomFloat(position.X - 70f, position.X + 70f), position.Y + CDGMath.RandomInt(-50, -30));
				spriteObj.Opacity = 0f;
				float num2 = 1f;
				Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					num.ToString(),
					"Opacity",
					"1"
				});
				Tween.By(spriteObj, num2, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					num.ToString(),
					"Y",
					"-150"
				});
				spriteObj.Opacity = 1f;
				Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					(num2 + num - 0.5f).ToString(),
					"Opacity",
					"0"
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.Opacity = 0f;
				num += CDGMath.RandomFloat(0.1f, 0.3f);
			}
			SpriteObj spriteObj2 = m_resourcePool.CheckOut();
			spriteObj2.AnimationDelay = 0.05f;
			spriteObj2.Opacity = 0.8f;
			spriteObj2.Visible = true;
			spriteObj2.ChangeSprite("TeleporterBeam_Sprite");
			spriteObj2.Position = position;
			spriteObj2.ScaleY = 0f;
			spriteObj2.PlayAnimation(true);
			Tween.To(spriteObj2, 0.05f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleY",
				"1"
			});
			Tween.To(spriteObj2, 2f, new Easing(Linear.EaseNone), new string[0]);
			Tween.AddEndHandlerToLastTween(spriteObj2, "StopAnimation", new object[0]);
		}
		public void DisplayThrustDustEffect(GameObj obj, int numClouds, float duration)
		{
			float num = duration / numClouds;
			float num2 = 0f;
			for (int i = 0; i < numClouds; i++)
			{
				Tween.RunFunction(num2, this, "DisplayDustEffect", new object[]
				{
					obj
				});
				Tween.RunFunction(num2, this, "DisplayDustEffect", new object[]
				{
					obj
				});
				num2 += num;
			}
		}
		public void DisplayDustEffect(GameObj obj)
		{
			int num = CDGMath.RandomInt(-30, 30);
			int num2 = CDGMath.RandomInt(-30, 30);
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ExplosionBrown_Sprite");
			spriteObj.Opacity = 0f;
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(0, 270);
			spriteObj.Position = new Vector2(obj.X, obj.Bounds.Bottom);
			spriteObj.Scale = new Vector2(0.8f, 0.8f);
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
			{
				"Rotation",
				"180"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				num.ToString(),
				"Y",
				num2.ToString()
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
		}
		public void DisplayDustEffect(Vector2 pos)
		{
			int num = CDGMath.RandomInt(-30, 30);
			int num2 = CDGMath.RandomInt(-30, 30);
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ExplosionBrown_Sprite");
			spriteObj.Opacity = 0f;
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(0, 270);
			spriteObj.Position = pos;
			spriteObj.Scale = new Vector2(0.8f, 0.8f);
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
			{
				"Rotation",
				"180"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				num.ToString(),
				"Y",
				num2.ToString()
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
		}
		public void TurretFireEffect(Vector2 pos, Vector2 scale)
		{
			int num = CDGMath.RandomInt(-20, 20);
			int num2 = CDGMath.RandomInt(-20, 20);
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ExplosionBrown_Sprite");
			spriteObj.Opacity = 0f;
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(0, 270);
			spriteObj.Position = pos;
			spriteObj.Scale = scale;
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
			{
				"Rotation",
				CDGMath.RandomInt(-50, 50).ToString()
			});
			Tween.By(spriteObj, 0.7f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				num.ToString(),
				"Y",
				num2.ToString()
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.4f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
		}
		public void DisplayFartEffect(GameObj obj)
		{
			int num = CDGMath.RandomInt(-10, 10);
			if (obj.Flip == SpriteEffects.FlipHorizontally)
			{
				num = 20;
			}
			else
			{
				num = -20;
			}
			int num2 = CDGMath.RandomInt(-10, 10);
			num2 = 0;
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("ExplosionBrown_Sprite");
			spriteObj.Opacity = 0f;
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(0, 270);
			spriteObj.Position = new Vector2(obj.X, obj.Bounds.Bottom);
			if (obj.Flip == SpriteEffects.FlipHorizontally)
			{
				spriteObj.X += 30f;
			}
			else
			{
				spriteObj.X -= 30f;
			}
			spriteObj.Scale = new Vector2(0.8f, 0.8f);
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
			{
				"Rotation",
				"180"
			});
			Tween.By(spriteObj, 0.7f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				num.ToString(),
				"Y",
				num2.ToString()
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
		}
		public void DisplayExplosionEffect(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("EnemyDeathFX1_Sprite");
			spriteObj.Visible = true;
			spriteObj.Position = position;
			spriteObj.PlayAnimation(false);
			spriteObj.AnimationDelay = 0.0333333351f;
			spriteObj.Scale = new Vector2(2f, 2f);
		}
		public void StartInverseEmit(Vector2 pos)
		{
			float num = 0.4f;
			int num2 = 30;
			float num3 = num / num2;
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("Blank_Sprite");
				spriteObj.TextureColor = Color.Black;
				spriteObj.Visible = true;
				spriteObj.PlayAnimation(true);
				spriteObj.Scale = Vector2.Zero;
				spriteObj.X = pos.X + CDGMath.RandomInt(-100, 100);
				spriteObj.Y = pos.Y + CDGMath.RandomInt(-100, 100);
				Tween.To(spriteObj, num4, new Easing(Linear.EaseNone), new string[]
				{
					"ScaleX",
					"2",
					"ScaleY",
					"2"
				});
				Tween.To(spriteObj, num - num4, new Easing(Quad.EaseInOut), new string[]
				{
					"delay",
					num4.ToString(),
					"X",
					pos.X.ToString(),
					"Y",
					pos.Y.ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				num4 += num3;
			}
		}
		public void StartTranslocateEmit(Vector2 pos)
		{
			int num = 30;
			for (int i = 0; i < num; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("Blank_Sprite");
				spriteObj.TextureColor = Color.White;
				spriteObj.Visible = true;
				spriteObj.PlayAnimation(true);
				spriteObj.Scale = new Vector2(2f, 2f);
				spriteObj.Position = pos;
				Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"ScaleX",
					"0",
					"ScaleY",
					"0"
				});
				Tween.By(spriteObj, 1f, new Easing(Quad.EaseIn), new string[]
				{
					"X",
					CDGMath.RandomInt(-250, 250).ToString()
				});
				Tween.By(spriteObj, 1f, new Easing(Quad.EaseIn), new string[]
				{
					"Y",
					CDGMath.RandomInt(500, 800).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			}
		}
		public void BlackSmokeEffect(GameObj obj)
		{
			for (int i = 0; i < 2; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("BlackSmoke_Sprite");
				spriteObj.Visible = true;
				spriteObj.Y = obj.Y;
				spriteObj.Y += CDGMath.RandomInt(-40, 40);
				spriteObj.X = obj.Bounds.Left;
				spriteObj.Opacity = 0f;
				spriteObj.PlayAnimation(true);
				spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
				if (CDGMath.RandomPlusMinus() < 0)
				{
					spriteObj.Flip = SpriteEffects.FlipHorizontally;
				}
				if (CDGMath.RandomPlusMinus() < 0)
				{
					spriteObj.Flip = SpriteEffects.FlipVertically;
				}
				int num = -1;
				if (obj.Flip == SpriteEffects.FlipHorizontally)
				{
					num = 1;
					spriteObj.X = obj.Bounds.Right;
				}
				Tween.To(spriteObj, 0.4f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj, 1.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(CDGMath.RandomInt(50, 100) * num).ToString(),
					"Y",
					CDGMath.RandomInt(-100, 100).ToString(),
					"Rotation",
					CDGMath.RandomInt(-45, 45).ToString()
				});
				spriteObj.Opacity = 1f;
				Tween.To(spriteObj, 1f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
				Tween.RunFunction(2f, spriteObj, "StopAnimation", new object[0]);
			}
		}
		public void BlackSmokeEffect(Vector2 pos, Vector2 scale)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("BlackSmoke_Sprite");
			spriteObj.Visible = true;
			spriteObj.Y = pos.Y;
			spriteObj.Y += CDGMath.RandomInt(-40, 40);
			spriteObj.X = pos.X;
			spriteObj.X += CDGMath.RandomInt(-40, 40);
			spriteObj.Scale = scale;
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
			Tween.To(spriteObj, 0.4f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 1.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				CDGMath.RandomInt(50, 100).ToString(),
				"Y",
				CDGMath.RandomInt(-100, 100).ToString(),
				"Rotation",
				CDGMath.RandomInt(-45, 45).ToString()
			});
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
			Tween.RunFunction(2f, spriteObj, "StopAnimation", new object[0]);
		}
		public void CrowDestructionEffect(Vector2 pos)
		{
			for (int i = 0; i < 2; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("BlackSmoke_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = pos;
				spriteObj.Opacity = 0f;
				spriteObj.PlayAnimation(true);
				spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
				Tween.To(spriteObj, 0.4f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj, 1.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					CDGMath.RandomInt(-50, 50).ToString(),
					"Y",
					CDGMath.RandomInt(-50, 50).ToString(),
					"Rotation",
					CDGMath.RandomInt(-45, 45).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.Opacity = 1f;
				Tween.To(spriteObj, 1f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
			}
			for (int j = 0; j < 4; j++)
			{
				SpriteObj spriteObj2 = m_resourcePool.CheckOut();
				spriteObj2.ChangeSprite("CrowFeather_Sprite");
				spriteObj2.Visible = true;
				spriteObj2.Scale = new Vector2(2f, 2f);
				spriteObj2.Position = pos;
				spriteObj2.X += CDGMath.RandomInt(-20, 20);
				spriteObj2.Y += CDGMath.RandomInt(-20, 20);
				spriteObj2.Opacity = 0f;
				spriteObj2.PlayAnimation(true);
				spriteObj2.Rotation = CDGMath.RandomInt(-30, 30);
				Tween.To(spriteObj2, 0.1f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj2, 1.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					CDGMath.RandomInt(-50, 50).ToString(),
					"Y",
					CDGMath.RandomInt(-50, 50).ToString(),
					"Rotation",
					CDGMath.RandomInt(-180, 180).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj2, "StopAnimation", new object[0]);
				spriteObj2.Opacity = 1f;
				Tween.To(spriteObj2, 1f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"Opacity",
					"0"
				});
				spriteObj2.Opacity = 0f;
			}
		}
		public void CrowSmokeEffect(Vector2 pos)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("BlackSmoke_Sprite");
			spriteObj.Visible = true;
			spriteObj.Y = pos.Y;
			spriteObj.Y += CDGMath.RandomInt(-40, 40);
			spriteObj.X = pos.X;
			spriteObj.X += CDGMath.RandomInt(-40, 40);
			spriteObj.Scale = new Vector2(0.7f, 0.7f);
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				CDGMath.RandomInt(50, 100).ToString(),
				"Y",
				CDGMath.RandomInt(-100, 100).ToString(),
				"Rotation",
				CDGMath.RandomInt(-45, 45).ToString()
			});
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
			Tween.RunFunction(1.05f, spriteObj, "StopAnimation", new object[0]);
		}
		public void WoodChipEffect(Vector2 pos)
		{
			for (int i = 0; i < 5; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("WoodChip_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = pos;
				spriteObj.PlayAnimation(true);
				spriteObj.Scale = new Vector2(2f, 2f);
				int num = CDGMath.RandomInt(-360, 360);
				spriteObj.Rotation = num;
				Vector2 vector = new Vector2(CDGMath.RandomInt(-60, 60), CDGMath.RandomInt(-60, 60));
				Tween.By(spriteObj, 0.3f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					vector.X.ToString(),
					"Y",
					vector.Y.ToString()
				});
				Tween.By(spriteObj, 0.3f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(-360, 360).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.1",
					"Opacity",
					"0"
				});
			}
		}
		public void SpellCastEffect(Vector2 pos, float angle, bool megaSpell)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("SpellPortal_Sprite");
			if (megaSpell)
			{
				spriteObj.TextureColor = Color.Red;
			}
			spriteObj.Visible = true;
			spriteObj.Position = pos;
			spriteObj.PlayAnimation(false);
			spriteObj.Scale = new Vector2(2f, 2f);
			spriteObj.Rotation = angle;
			spriteObj.OutlineWidth = 2;
		}
		public void LastBossSpellCastEffect(GameObj obj, float angle, bool megaSpell)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("SpellPortal_Sprite");
			if (megaSpell)
			{
				spriteObj.TextureColor = Color.Red;
			}
			spriteObj.Visible = true;
			spriteObj.Position = obj.Position;
			spriteObj.PlayAnimation(false);
			spriteObj.Scale = new Vector2(2f, 2f);
			if (obj.Flip == SpriteEffects.None)
			{
				spriteObj.Rotation = -angle;
			}
			else
			{
				spriteObj.Rotation = angle;
			}
			spriteObj.OutlineWidth = 2;
		}
		public void LoadingGateSmokeEffect(int numEntities)
		{
			float num = 1320f / numEntities;
			for (int i = 0; i < numEntities; i++)
			{
				int num2 = CDGMath.RandomInt(-50, 50);
				int num3 = CDGMath.RandomInt(-50, 0);
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionBrown_Sprite");
				spriteObj.Visible = true;
				spriteObj.ForceDraw = true;
				spriteObj.Position = new Vector2(num * i, 720f);
				spriteObj.PlayAnimation(true);
				spriteObj.Opacity = 0f;
				spriteObj.Scale = new Vector2(1.5f, 1.5f);
				Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"Opacity",
					"0.8"
				});
				Tween.By(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomFloat(-180f, 180f).ToString()
				});
				Tween.By(spriteObj, 0.7f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					num2.ToString(),
					"Y",
					num3.ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.Opacity = 0.8f;
				Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
			}
		}
		public void MegaTeleport(Vector2 pos, Vector2 scale)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("MegaTeleport_Sprite");
			spriteObj.TextureColor = Color.LightSkyBlue;
			spriteObj.Scale = scale;
			spriteObj.Visible = true;
			spriteObj.Position = pos;
			spriteObj.PlayAnimation(false);
			spriteObj.AnimationDelay = 0.0166666675f;
			Tween.By(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.15",
				"Y",
				"-720"
			});
		}
		public void MegaTeleportReverse(Vector2 pos, Vector2 scale)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("MegaTeleportReverse_Sprite");
			spriteObj.TextureColor = Color.LightSkyBlue;
			spriteObj.Scale = scale;
			spriteObj.Visible = true;
			spriteObj.Position = pos;
			spriteObj.Y -= 720f;
			spriteObj.PlayAnimation(1, 1, true);
			spriteObj.AnimationDelay = 0.0166666675f;
			Tween.By(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				"720"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "PlayAnimation", new object[]
			{
				false
			});
		}
		public void DestroyFireballBoss(Vector2 pos)
		{
			float num = 0f;
			float num2 = 24f;
			for (int i = 0; i < 15; i++)
			{
				float num3 = CDGMath.RandomFloat(0.5f, 1.1f);
				int num4 = CDGMath.RandomInt(50, 200);
				float num5 = CDGMath.RandomFloat(2f, 5f);
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("SpellDamageShield_Sprite");
				spriteObj.Visible = true;
				spriteObj.Scale = new Vector2(num5, num5);
				spriteObj.Position = pos;
				spriteObj.PlayAnimation(true);
				Vector2 vector = CDGMath.AngleToVector(num);
				Tween.By(spriteObj, 1.5f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					(vector.X * num4).ToString(),
					"Y",
					(vector.Y * num4).ToString()
				});
				Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					num3.ToString(),
					"Opacity",
					"0"
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				num += num2;
			}
		}
		public void SkillTreeDustEffect(Vector2 pos, bool horizontal, float length)
		{
			int num = (int)(length / 20f);
			float scaleFactor = length / num;
			Vector2 zero = Vector2.Zero;
			if (horizontal)
			{
				zero = new Vector2(1f, 0f);
			}
			else
			{
				zero = new Vector2(0f, -1f);
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = CDGMath.RandomInt(-10, 10);
				int num3 = CDGMath.RandomInt(-20, 0);
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionBrown_Sprite");
				spriteObj.Opacity = 0f;
				spriteObj.Visible = true;
				spriteObj.Scale = new Vector2(0.5f, 0.5f);
				spriteObj.Rotation = CDGMath.RandomInt(0, 270);
				spriteObj.Position = pos + zero * scaleFactor * (i + 1);
				spriteObj.PlayAnimation(true);
				Tween.To(spriteObj, 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj, 1.5f, new Easing(Linear.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomFloat(-30f, 30f).ToString()
				});
				Tween.By(spriteObj, 1.5f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					num2.ToString(),
					"Y",
					num3.ToString()
				});
				float num4 = CDGMath.RandomFloat(0.5f, 0.8f);
				Tween.To(spriteObj, 1.5f, new Easing(Quad.EaseOut), new string[]
				{
					"ScaleX",
					num4.ToString(),
					"ScaleY",
					num4.ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.Opacity = 1f;
				Tween.To(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.6",
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
			}
		}
		public void SkillTreeDustDuration(Vector2 pos, bool horizontal, float length, float duration)
		{
			float num = 0.25f;
			int num2 = (int)(duration / num);
			for (int i = 0; i < num2; i++)
			{
				Tween.RunFunction(num * i, this, "SkillTreeDustEffect", new object[]
				{
					pos,
					horizontal,
					length
				});
			}
		}
		public void CarnivalGoldEffect(Vector2 startPos, Vector2 endPos, int numCoins)
		{
			float num = 0.32f;
			for (int i = 0; i < numCoins; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("Coin_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = startPos;
				spriteObj.PlayAnimation(true);
				int num2 = CDGMath.RandomInt(-30, 30);
				int num3 = CDGMath.RandomInt(-30, 30);
				Tween.By(spriteObj, 0.3f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					num2.ToString(),
					"Y",
					num3.ToString()
				});
				spriteObj.X += num2;
				spriteObj.Y += num3;
				Tween.To(spriteObj, 0.5f, new Easing(Quad.EaseIn), new string[]
				{
					"delay",
					num.ToString(),
					"X",
					endPos.X.ToString(),
					"Y",
					endPos.Y.ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.X -= num2;
				spriteObj.Y -= num3;
				num += 0.05f;
			}
		}
		public void AssassinCastEffect(Vector2 pos)
		{
			int num = 10;
			float num2 = 0f;
			float num3 = 360 / num;
			for (int i = 0; i < num; i++)
			{
				Vector2 vector = CDGMath.AngleToVector(num2);
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionBrown_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = pos;
				spriteObj.TextureColor = new Color(20, 20, 20);
				spriteObj.Opacity = 0f;
				spriteObj.PlayAnimation(true);
				float num4 = CDGMath.RandomFloat(0.5f, 1.5f);
				spriteObj.Scale = new Vector2(num4, num4);
				spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
				vector.X += CDGMath.RandomInt(-5, 5);
				vector.Y += CDGMath.RandomInt(-5, 5);
				Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0.5"
				});
				Tween.By(spriteObj, 1f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					(vector.X * CDGMath.RandomInt(20, 25)).ToString(),
					"Y",
					(vector.Y * CDGMath.RandomInt(20, 25)).ToString(),
					"Rotation",
					CDGMath.RandomInt(-180, 180).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				spriteObj.Opacity = 0.5f;
				Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
				num2 += num3;
			}
		}
		public void NinjaDisappearEffect(GameObj obj)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("Log_Sprite");
			spriteObj.AnimationDelay = 0.05f;
			spriteObj.Position = obj.Position;
			spriteObj.Visible = true;
			spriteObj.Scale = obj.Scale / 2f;
			spriteObj.PlayAnimation(true);
			Tween.By(spriteObj, 0.3f, new Easing(Quad.EaseIn), new string[]
			{
				"delay",
				"0.2",
				"Y",
				"50"
			});
			Tween.To(spriteObj, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
			SpriteObj spriteObj2 = m_resourcePool.CheckOut();
			spriteObj2.ChangeSprite("NinjaSmoke_Sprite");
			spriteObj2.AnimationDelay = 0.05f;
			spriteObj2.Position = obj.Position;
			spriteObj2.Visible = true;
			spriteObj2.Scale = obj.Scale * 2f;
			spriteObj2.PlayAnimation(false);
		}
		public void NinjaAppearEffect(GameObj obj)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("NinjaSmoke_Sprite");
			spriteObj.AnimationDelay = 0.05f;
			spriteObj.Position = obj.Position;
			spriteObj.Visible = true;
			spriteObj.Scale = obj.Scale * 2f;
			spriteObj.PlayAnimation(false);
		}
		public void DisplayCriticalText(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("CriticalText_Sprite");
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(-20, 20);
			spriteObj.Position = CDGMath.GetCirclePosition(spriteObj.Rotation - 90f, 20f, position);
			spriteObj.Scale = Vector2.Zero;
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Back.EaseOutLarge), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayFusRoDahText(Vector2 position)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("FusRoDahText_Sprite");
			spriteObj.Visible = true;
			spriteObj.Rotation = CDGMath.RandomInt(-20, 20);
			spriteObj.Position = CDGMath.GetCirclePosition(spriteObj.Rotation - 90f, 40f, position);
			spriteObj.Scale = Vector2.Zero;
			spriteObj.PlayAnimation(true);
			Tween.To(spriteObj, 0.2f, new Easing(Back.EaseOutLarge), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayTanookiEffect(GameObj obj)
		{
			for (int i = 0; i < 10; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("ExplosionBrown_Sprite");
				spriteObj.Visible = true;
				spriteObj.Position = obj.Position;
				spriteObj.X += CDGMath.RandomInt(-30, 30);
				spriteObj.Y += CDGMath.RandomInt(-30, 30);
				float num = CDGMath.RandomFloat(0.7f, 0.8f);
				int num2 = 50;
				spriteObj.Scale = new Vector2(num, num);
				spriteObj.Rotation = CDGMath.RandomInt(0, 90);
				spriteObj.PlayAnimation(true);
				float num3 = CDGMath.RandomFloat(0.5f, 1f);
				float num4 = CDGMath.RandomFloat(0f, 0.1f);
				Tween.To(spriteObj, num3 - 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"0"
				});
				Tween.To(spriteObj, num3, new Easing(Back.EaseIn), new string[]
				{
					"ScaleX",
					num4.ToString(),
					"ScaleY",
					num4.ToString()
				});
				Tween.By(spriteObj, num3, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					CDGMath.RandomInt(-num2, num2).ToString(),
					"Y",
					CDGMath.RandomInt(-num2, num2).ToString()
				});
				Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
				Tween.By(spriteObj, num3 - 0.1f, new Easing(Quad.EaseOut), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(145, 190).ToString()
				});
			}
		}
		public void DisplayMusicNote(Vector2 pos)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("NoteWhite_Sprite");
			spriteObj.Visible = true;
			spriteObj.Position = pos;
			spriteObj.Scale = new Vector2(2f, 2f);
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			if (CDGMath.RandomPlusMinus() < 0)
			{
				spriteObj.Flip = SpriteEffects.FlipHorizontally;
			}
			Tween.By(spriteObj, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"Y",
				"-50"
			});
			Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.8",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
			Tween.RunFunction(1f, spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayQuestionMark(Vector2 pos)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("QuestionMark_Sprite");
			spriteObj.Visible = true;
			spriteObj.Position = pos;
			float num = CDGMath.RandomFloat(0.8f, 2f);
			spriteObj.Scale = new Vector2(num, num);
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			Tween.By(spriteObj, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"Y",
				CDGMath.RandomInt(-70, -50).ToString()
			});
			Tween.By(spriteObj, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				CDGMath.RandomInt(-20, 20).ToString()
			});
			Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.8",
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
			Tween.RunFunction(1f, spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayMassiveSmoke(Vector2 topLeft)
		{
			Vector2 pos = topLeft;
			for (int i = 0; i < 20; i++)
			{
				IntroSmokeEffect(pos);
				pos.Y += 40f;
			}
		}
		public void IntroSmokeEffect(Vector2 pos)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("BlackSmoke_Sprite");
			spriteObj.Visible = true;
			spriteObj.Y = pos.Y;
			spriteObj.Y += CDGMath.RandomInt(-40, 40);
			spriteObj.X = pos.X;
			spriteObj.X += CDGMath.RandomInt(-40, 40);
			spriteObj.Scale = new Vector2(2.5f, 2.5f);
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
			float num = CDGMath.RandomFloat(0f, 0.2f);
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				num.ToString(),
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				num.ToString(),
				"X",
				CDGMath.RandomInt(50, 100).ToString(),
				"Y",
				CDGMath.RandomInt(-100, 100).ToString(),
				"Rotation",
				CDGMath.RandomInt(-45, 45).ToString()
			});
			spriteObj.Opacity = 1f;
			Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				(num + 0.5f).ToString(),
				"Opacity",
				"0"
			});
			spriteObj.Opacity = 0f;
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayIceParticleEffect(GameObj sprite)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("WizardIceParticle_Sprite");
			spriteObj.Visible = true;
			spriteObj.Scale = Vector2.Zero;
			spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right), CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
			spriteObj.Opacity = 0f;
			Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(spriteObj, 0.9f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"2.5",
				"ScaleY",
				"2.5"
			});
			Tween.By(spriteObj, 0.9f, new Easing(Tween.EaseNone), new string[]
			{
				"Rotation",
				(CDGMath.RandomInt(90, 270) * CDGMath.RandomPlusMinus()).ToString()
			});
			spriteObj.Opacity = 1f;
			float num = CDGMath.RandomFloat(0.4f, 0.7f);
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				num.ToString(),
				"Opacity",
				"0"
			});
			Tween.By(spriteObj, 0.2f + num, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				CDGMath.RandomInt(-20, 20).ToString(),
				"Y",
				CDGMath.RandomInt(-20, 20).ToString()
			});
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			Tween.RunFunction(1f, spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayFireParticleEffect(GameObj sprite)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("WizardFireParticle_Sprite");
			spriteObj.Visible = true;
			spriteObj.Scale = Vector2.Zero;
			spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right), CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
			spriteObj.Opacity = 0f;
			Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(spriteObj, 0.9f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"4",
				"ScaleY",
				"4"
			});
			spriteObj.Opacity = 1f;
			float num = CDGMath.RandomFloat(0.4f, 0.7f);
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				num.ToString(),
				"Opacity",
				"0"
			});
			Tween.By(spriteObj, 0.2f + num, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				CDGMath.RandomInt(-20, -5).ToString()
			});
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			Tween.RunFunction(1f, spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayEarthParticleEffect(GameObj sprite)
		{
			int num = CDGMath.RandomInt(1, 4);
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("Blossom" + num + "_Sprite");
			spriteObj.Visible = true;
			spriteObj.Scale = new Vector2(0.2f, 0.2f);
			spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right), CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
			spriteObj.Opacity = 0f;
			Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(spriteObj, 0.9f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"0.7",
				"ScaleY",
				"0.7"
			});
			Tween.By(spriteObj, 0.9f, new Easing(Tween.EaseNone), new string[]
			{
				"Rotation",
				(CDGMath.RandomInt(10, 45) * CDGMath.RandomPlusMinus()).ToString()
			});
			spriteObj.Opacity = 1f;
			float num2 = CDGMath.RandomFloat(0.4f, 0.7f);
			Tween.To(spriteObj, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				num2.ToString(),
				"Opacity",
				"0"
			});
			Tween.By(spriteObj, 0.2f + num2, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				CDGMath.RandomInt(5, 20).ToString()
			});
			spriteObj.Opacity = 0f;
			spriteObj.PlayAnimation(true);
			Tween.RunFunction(1f, spriteObj, "StopAnimation", new object[0]);
		}
		public void DisplayFountainShatterSmoke(GameObj sprite)
		{
			int num = 15;
			float num2 = sprite.Width / (float)num;
			float num3 = sprite.Bounds.Left;
			for (int i = 0; i < num; i++)
			{
				SpriteObj spriteObj = m_resourcePool.CheckOut();
				spriteObj.ChangeSprite("FountainShatterSmoke_Sprite");
				spriteObj.Visible = true;
				spriteObj.PlayAnimation(true);
				spriteObj.Opacity = 0f;
				spriteObj.Scale = Vector2.Zero;
				spriteObj.Position = new Vector2(num3, sprite.Y);
				float num4 = CDGMath.RandomFloat(2f, 4f);
				Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj, 4f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(-40, 40).ToString()
				});
				Tween.By(spriteObj, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					CDGMath.RandomInt(-20, 20).ToString(),
					"Y",
					CDGMath.RandomInt(-50, -30).ToString()
				});
				Tween.To(spriteObj, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"ScaleX",
					num4.ToString(),
					"ScaleY",
					num4.ToString()
				});
				spriteObj.Opacity = 1f;
				Tween.To(spriteObj, 2f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					CDGMath.RandomFloat(1f, 2f).ToString(),
					"Opacity",
					"0"
				});
				spriteObj.Opacity = 0f;
				Tween.RunFunction(4.5f, spriteObj, "StopAnimation", new object[0]);
				num3 += num2;
			}
			num /= 2;
			num3 = sprite.Bounds.Left + 50;
			num2 = (sprite.Width - 50) / (float)num;
			for (int j = 0; j < num; j++)
			{
				SpriteObj spriteObj2 = m_resourcePool.CheckOut();
				spriteObj2.ChangeSprite("FountainShatterSmoke_Sprite");
				spriteObj2.Visible = true;
				spriteObj2.PlayAnimation(true);
				spriteObj2.Scale = Vector2.Zero;
				spriteObj2.Opacity = 0f;
				spriteObj2.Position = new Vector2(num3, sprite.Y - 100f);
				float num5 = CDGMath.RandomFloat(2f, 4f);
				Tween.To(spriteObj2, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj2, 4f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(-180, 180).ToString()
				});
				Tween.By(spriteObj2, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					CDGMath.RandomInt(-20, 20).ToString(),
					"Y",
					CDGMath.RandomInt(-50, -30).ToString()
				});
				Tween.To(spriteObj2, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"ScaleX",
					num5.ToString(),
					"ScaleY",
					num5.ToString()
				});
				spriteObj2.Opacity = 1f;
				Tween.To(spriteObj2, 2f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					CDGMath.RandomFloat(1f, 2f).ToString(),
					"Opacity",
					"0"
				});
				spriteObj2.Opacity = 0f;
				Tween.RunFunction(4.5f, spriteObj2, "StopAnimation", new object[0]);
				num3 += num2;
			}
			num /= 2;
			num3 = sprite.Bounds.Left + 100;
			num2 = (sprite.Width - 100) / (float)num;
			for (int k = 0; k < num; k++)
			{
				SpriteObj spriteObj3 = m_resourcePool.CheckOut();
				spriteObj3.ChangeSprite("FountainShatterSmoke_Sprite");
				spriteObj3.Visible = true;
				spriteObj3.PlayAnimation(true);
				spriteObj3.Scale = Vector2.Zero;
				spriteObj3.Opacity = 0f;
				spriteObj3.Position = new Vector2(num3, sprite.Y - 200f);
				float num6 = CDGMath.RandomFloat(2f, 4f);
				Tween.To(spriteObj3, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(spriteObj3, 4f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					CDGMath.RandomInt(-180, 180).ToString()
				});
				Tween.By(spriteObj3, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					CDGMath.RandomInt(-20, 20).ToString(),
					"Y",
					CDGMath.RandomInt(-50, -30).ToString()
				});
				Tween.To(spriteObj3, 3f, new Easing(Tween.EaseNone), new string[]
				{
					"ScaleX",
					num6.ToString(),
					"ScaleY",
					num6.ToString()
				});
				spriteObj3.Opacity = 1f;
				Tween.To(spriteObj3, 2f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					CDGMath.RandomFloat(1f, 2f).ToString(),
					"Opacity",
					"0"
				});
				spriteObj3.Opacity = 0f;
				Tween.RunFunction(4.5f, spriteObj3, "StopAnimation", new object[0]);
				num3 += num2;
			}
		}
		public void DoorSparkleEffect(Rectangle rect)
		{
			SpriteObj spriteObj = m_resourcePool.CheckOut();
			spriteObj.ChangeSprite("LevelUpParticleFX_Sprite");
			spriteObj.Visible = true;
			float num = CDGMath.RandomFloat(0.3f, 0.5f);
			spriteObj.Scale = new Vector2(num, num);
			spriteObj.Opacity = 0f;
			spriteObj.Position = new Vector2(CDGMath.RandomInt(rect.X, rect.X + rect.Width), CDGMath.RandomInt(rect.Y, rect.Y + rect.Height));
			spriteObj.Rotation = CDGMath.RandomInt(0, 90);
			spriteObj.PlayAnimation(false);
			Tween.To(spriteObj, 0.4f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(spriteObj, 0.6f, new Easing(Linear.EaseNone), new string[]
			{
				"Rotation",
				CDGMath.RandomInt(-45, 45).ToString(),
				"Y",
				"-50"
			});
			Tween.To(spriteObj, 0.7f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation", new object[0]);
		}
		public void DestroyEffect(SpriteObj obj)
		{
			obj.OutlineWidth = 0;
			obj.Visible = false;
			obj.Rotation = 0f;
			obj.TextureColor = Color.White;
			obj.Opacity = 1f;
			m_resourcePool.CheckIn(obj);
			obj.Flip = SpriteEffects.None;
			obj.Scale = new Vector2(1f, 1f);
			obj.AnimationDelay = 0.0333333351f;
		}
		public void PauseAllAnimations()
		{
			m_isPaused = true;
			foreach (SpriteObj current in m_resourcePool.ActiveObjsList)
			{
				current.PauseAnimation();
			}
		}
		public void ResumeAllAnimations()
		{
			m_isPaused = false;
			foreach (SpriteObj current in m_resourcePool.ActiveObjsList)
			{
				current.ResumeAnimation();
			}
		}
		public void DestroyAllEffects()
		{
			foreach (SpriteObj current in m_resourcePool.ActiveObjsList)
			{
				current.StopAnimation();
			}
		}
		public void Draw(Camera2D camera)
		{
			for (int i = 0; i < m_resourcePool.ActiveObjsList.Count; i++)
			{
				if (!m_resourcePool.ActiveObjsList[i].IsAnimating && !m_isPaused)
				{
					DestroyEffect(m_resourcePool.ActiveObjsList[i]);
					i--;
				}
				else
				{
					m_resourcePool.ActiveObjsList[i].Draw(camera);
				}
			}
		}
		public void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Impact Effect Pool");
				m_isDisposed = true;
				m_resourcePool.Dispose();
				m_resourcePool = null;
			}
		}
	}
}
