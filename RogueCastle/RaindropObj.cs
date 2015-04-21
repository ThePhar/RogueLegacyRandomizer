using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
			this.ChangeToRainDrop();
			this.m_speedY = CDGMath.RandomFloat(this.MaxYSpeed.X, this.MaxYSpeed.Y);
			this.m_speedX = CDGMath.RandomFloat(this.MaxXSpeed.X, this.MaxXSpeed.Y);
			this.IsCollidable = true;
			this.m_startingPos = startingPos;
			base.Position = this.m_startingPos;
			base.AnimationDelay = 0.0333333351f;
			this.Scale = new Vector2(2f, 2f);
		}
		public void ChangeToSnowflake()
		{
			this.ChangeSprite("Snowflake_Sprite");
			this.m_isSnowflake = true;
			this.m_isParticle = false;
			base.Rotation = 0f;
			this.MaxYSpeed = new Vector2(200f, 400f);
			this.MaxXSpeed = new Vector2(-200f, 0f);
			base.Position = this.m_startingPos;
			this.m_speedY = CDGMath.RandomFloat(this.MaxYSpeed.X, this.MaxYSpeed.Y);
			this.m_speedX = CDGMath.RandomFloat(this.MaxXSpeed.X, this.MaxXSpeed.Y);
		}
		public void ChangeToRainDrop()
		{
			this.m_isSnowflake = false;
			this.m_isParticle = false;
			this.MaxYSpeed = new Vector2(800f, 1200f);
			this.MaxXSpeed = new Vector2(-200f, -200f);
			base.Rotation = 5f;
			this.m_speedY = CDGMath.RandomFloat(this.MaxYSpeed.X, this.MaxYSpeed.Y);
			this.m_speedX = CDGMath.RandomFloat(this.MaxXSpeed.X, this.MaxXSpeed.Y);
		}
		public void ChangeToParticle()
		{
			this.m_isSnowflake = false;
			this.m_isParticle = true;
			this.MaxYSpeed = new Vector2(0f, 0f);
			this.MaxXSpeed = new Vector2(500f, 1500f);
			base.Rotation = -90f;
			this.m_speedY = CDGMath.RandomFloat(this.MaxYSpeed.X, this.MaxYSpeed.Y);
			this.m_speedX = CDGMath.RandomFloat(this.MaxXSpeed.X, this.MaxXSpeed.Y);
			float num = CDGMath.RandomFloat(2f, 8f);
			this.Scale = new Vector2(num, num);
		}
		public void Update(List<TerrainObj> collisionList, GameTime gameTime)
		{
			if (!this.m_splashing)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				base.Y += this.m_speedY * num;
				base.X += this.m_speedX * num;
				if (this.IsCollidable)
				{
					Rectangle bounds = this.Bounds;
					foreach (TerrainObj current in collisionList)
					{
						TerrainObj terrainObj = current;
						Rectangle bounds2 = terrainObj.Bounds;
						if (terrainObj.Visible && terrainObj.CollidesTop && terrainObj.Y > 120f && CollisionMath.Intersects(bounds, bounds2))
						{
							if (terrainObj.Rotation == 0f)
							{
								if (!this.m_isSnowflake)
								{
									base.Y = (float)(bounds2.Top - 10);
								}
								this.RunSplashAnimation();
								break;
							}
							if (terrainObj.Rotation != 0f && CollisionMath.RotatedRectIntersects(bounds, 0f, Vector2.Zero, new Rectangle((int)terrainObj.X, (int)terrainObj.Y, terrainObj.Width, terrainObj.Height), terrainObj.Rotation, Vector2.Zero))
							{
								if (!this.m_isSnowflake)
								{
									base.Y -= 12f;
								}
								this.RunSplashAnimation();
								break;
							}
							break;
						}
					}
				}
				if (base.Y > 720f)
				{
					this.RunSplashAnimation();
				}
			}
			if (!base.IsAnimating && this.m_splashing && !this.m_isSnowflake)
			{
				this.KillDrop();
			}
		}
		public void UpdateNoCollision(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			base.Y += this.m_speedY * num;
			base.X += this.m_speedX * num;
			if (base.X > this.m_startingPos.X + 4000f || base.X < this.m_startingPos.X - 4000f)
			{
				this.KillDrop();
			}
			if (base.Y > this.m_startingPos.Y + 4000f || base.Y < this.m_startingPos.Y - 4000f)
			{
				this.KillDrop();
			}
		}
		private void RunSplashAnimation()
		{
			this.m_splashing = true;
			base.Rotation = 0f;
			if (!this.m_isSnowflake)
			{
				base.PlayAnimation(2, base.TotalFrames, false);
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
			this.m_splashing = false;
			base.GoToFrame(1);
			base.Rotation = 5f;
			base.X = this.m_startingPos.X;
			base.Y = (float)CDGMath.RandomInt(-100, 0);
			if (this.m_isParticle)
			{
				base.Y = this.m_startingPos.Y;
				base.Rotation = -90f;
			}
			this.m_speedY = CDGMath.RandomFloat(this.MaxYSpeed.X, this.MaxYSpeed.Y);
			this.m_speedX = CDGMath.RandomFloat(this.MaxXSpeed.X, this.MaxXSpeed.Y);
			base.Opacity = 1f;
		}
	}
}
