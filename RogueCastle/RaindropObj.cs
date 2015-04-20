/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;

namespace RogueCastle
{
	public class RaindropObj : SpriteObj
	{
		private float m_speedY;
		private float m_speedX;
		private Vector2 m_startingPos;
		private bool m_splashing;
		private bool m_isSnowflake;
		private bool m_isParticle;
		public bool IsCollidable
		{
			get;
			set;
		}
		public Vector2 MaxYSpeed
		{
			get;
			set;
		}
		public Vector2 MaxXSpeed
		{
			get;
			set;
		}
		public RaindropObj(Vector2 startingPos) : base("Raindrop_Sprite")
		{
			ChangeToRainDrop();
			m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
			m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
			IsCollidable = true;
			m_startingPos = startingPos;
			Position = m_startingPos;
			AnimationDelay = 0.0333333351f;
			Scale = new Vector2(2f, 2f);
		}
		public void ChangeToSnowflake()
		{
			ChangeSprite("Snowflake_Sprite");
			m_isSnowflake = true;
			m_isParticle = false;
			Rotation = 0f;
			MaxYSpeed = new Vector2(200f, 400f);
			MaxXSpeed = new Vector2(-200f, 0f);
			Position = m_startingPos;
			m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
			m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
		}
		public void ChangeToRainDrop()
		{
			m_isSnowflake = false;
			m_isParticle = false;
			MaxYSpeed = new Vector2(800f, 1200f);
			MaxXSpeed = new Vector2(-200f, -200f);
			Rotation = 5f;
			m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
			m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
		}
		public void ChangeToParticle()
		{
			m_isSnowflake = false;
			m_isParticle = true;
			MaxYSpeed = new Vector2(0f, 0f);
			MaxXSpeed = new Vector2(500f, 1500f);
			Rotation = -90f;
			m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
			m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
			float num = CDGMath.RandomFloat(2f, 8f);
			Scale = new Vector2(num, num);
		}
		public void Update(List<TerrainObj> collisionList, GameTime gameTime)
		{
			if (!m_splashing)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				Y += m_speedY * num;
				X += m_speedX * num;
				if (IsCollidable)
				{
					Rectangle bounds = Bounds;
					foreach (TerrainObj current in collisionList)
					{
						TerrainObj terrainObj = current;
						Rectangle bounds2 = terrainObj.Bounds;
						if (terrainObj.Visible && terrainObj.CollidesTop && terrainObj.Y > 120f && CollisionMath.Intersects(bounds, bounds2))
						{
							if (terrainObj.Rotation == 0f)
							{
								if (!m_isSnowflake)
								{
									Y = bounds2.Top - 10;
								}
								RunSplashAnimation();
								break;
							}
							if (terrainObj.Rotation != 0f && CollisionMath.RotatedRectIntersects(bounds, 0f, Vector2.Zero, new Rectangle((int)terrainObj.X, (int)terrainObj.Y, terrainObj.Width, terrainObj.Height), terrainObj.Rotation, Vector2.Zero))
							{
								if (!m_isSnowflake)
								{
									Y -= 12f;
								}
								RunSplashAnimation();
								break;
							}
							break;
						}
					}
				}
				if (Y > 720f)
				{
					RunSplashAnimation();
				}
			}
			if (!IsAnimating && m_splashing && !m_isSnowflake)
			{
				KillDrop();
			}
		}
		public void UpdateNoCollision(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Y += m_speedY * num;
			X += m_speedX * num;
			if (X > m_startingPos.X + 4000f || X < m_startingPos.X - 4000f)
			{
				KillDrop();
			}
			if (Y > m_startingPos.Y + 4000f || Y < m_startingPos.Y - 4000f)
			{
				KillDrop();
			}
		}
		private void RunSplashAnimation()
		{
			m_splashing = true;
			Rotation = 0f;
			if (!m_isSnowflake)
			{
				PlayAnimation(2, TotalFrames, false);
				return;
			}
			Tween.To(this, 0.25f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(this, "KillDrop", new object[0]);
		}
		public void KillDrop()
		{
			m_splashing = false;
			GoToFrame(1);
			Rotation = 5f;
			X = m_startingPos.X;
			Y = CDGMath.RandomInt(-100, 0);
			if (m_isParticle)
			{
				Y = m_startingPos.Y;
				Rotation = -90f;
			}
			m_speedY = CDGMath.RandomFloat(MaxYSpeed.X, MaxYSpeed.Y);
			m_speedX = CDGMath.RandomFloat(MaxXSpeed.X, MaxXSpeed.Y);
			Opacity = 1f;
		}
	}
}
