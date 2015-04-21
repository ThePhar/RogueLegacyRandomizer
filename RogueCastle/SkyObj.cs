using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
namespace RogueCastle
{
	public class SkyObj : GameObj
	{
		private SpriteObj m_moon;
		private BackgroundObj m_differenceCloud;
		private BackgroundObj m_differenceCloud2;
		private BackgroundObj m_differenceCloud3;
		private Vector2 m_moonPos;
		private SpriteObj m_silhouette;
		private bool m_silhouetteFlying;
		private float m_silhouetteTimer;
		private ProceduralLevelScreen m_levelScreen;
		public float MorningOpacity
		{
			get;
			set;
		}
		public SkyObj(ProceduralLevelScreen levelScreen)
		{
			this.m_levelScreen = levelScreen;
		}
		public void LoadContent(Camera2D camera)
		{
			Vector2 one = new Vector2(2f, 2f);
			this.m_moon = new SpriteObj("ParallaxMoon_Sprite");
			this.m_moon.Position = new Vector2(900f, 200f);
			if (LevelEV.SAVE_FRAMES)
			{
				this.m_moon.Position /= 2f;
				one = Vector2.One;
			}
			this.m_moon.Scale = one;
			this.m_moon.ForceDraw = true;
			this.m_moonPos = this.m_moon.Position;
			this.m_differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud.Scale = one;
			this.m_differenceCloud.TextureColor = new Color(10, 10, 80);
			this.m_differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);
			this.m_differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud2.Scale = one;
			this.m_differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
			this.m_differenceCloud2.TextureColor = new Color(80, 80, 160);
			this.m_differenceCloud2.X -= 500f;
			this.m_differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);
			this.m_differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud3.Scale = one;
			this.m_differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
			this.m_differenceCloud3.TextureColor = Color.White;
			this.m_differenceCloud3.X -= 500f;
			this.m_differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
			this.m_silhouette = new SpriteObj("GardenBat_Sprite");
			this.m_silhouette.ForceDraw = true;
			this.m_silhouette.AnimationDelay = 0.05f;
			this.m_silhouette.Scale = one;
		}
		public void ReinitializeRT(Camera2D camera)
		{
			Vector2 one = new Vector2(2f, 2f);
			if (LevelEV.SAVE_FRAMES)
			{
				this.m_moon.Position /= 2f;
				one = Vector2.One;
			}
			if (this.m_differenceCloud != null)
			{
				this.m_differenceCloud.Dispose();
			}
			this.m_differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud.Scale = one;
			this.m_differenceCloud.TextureColor = new Color(10, 10, 80);
			this.m_differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);
			if (this.m_differenceCloud2 != null)
			{
				this.m_differenceCloud2.Dispose();
			}
			this.m_differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud2.Scale = one;
			this.m_differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
			this.m_differenceCloud2.TextureColor = new Color(80, 80, 160);
			this.m_differenceCloud2.X -= 500f;
			this.m_differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);
			if (this.m_differenceCloud3 != null)
			{
				this.m_differenceCloud3.Dispose();
			}
			this.m_differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			this.m_differenceCloud3.Scale = one;
			this.m_differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
			this.m_differenceCloud3.TextureColor = new Color(80, 80, 160);
			this.m_differenceCloud3.X -= 500f;
			this.m_differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
		}
		public void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!this.m_silhouetteFlying && this.m_silhouetteTimer > 0f)
			{
				this.m_silhouetteTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_silhouetteTimer <= 0f)
				{
					int num2 = CDGMath.RandomInt(1, 100);
					if (num2 > 95)
					{
						this.ShowSilhouette(true);
					}
					else if (num2 > 65)
					{
						this.ShowSilhouette(false);
					}
					else
					{
						this.m_silhouetteTimer = 5f;
					}
				}
			}
			if (!this.m_silhouetteFlying && this.m_silhouetteTimer <= 0f)
			{
				this.m_silhouetteTimer = 5f;
			}
			if (this.m_silhouette.SpriteName == "GardenPerson_Sprite")
			{
				this.m_silhouette.Rotation += 120f * num;
			}
		}
		private void ShowSilhouette(bool showSanta)
		{
			if (this.m_levelScreen != null)
			{
				this.m_silhouetteFlying = true;
				this.m_silhouette.Rotation = 0f;
				this.m_silhouette.Flip = SpriteEffects.None;
				bool flag = false;
				if (CDGMath.RandomInt(0, 1) > 0)
				{
					flag = true;
				}
				string[] array = new string[]
				{
					"GardenBat_Sprite",
					"GardenCrow_Sprite",
					"GardenBat_Sprite",
					"GardenCrow_Sprite",
					"GardenPerson_Sprite"
				};
				if (!showSanta)
				{
					this.m_silhouette.ChangeSprite(array[CDGMath.RandomInt(0, 4)]);
				}
				else
				{
					this.m_silhouette.ChangeSprite("GardenSanta_Sprite");
				}
				this.m_silhouette.PlayAnimation(true);
				Vector2 arg_A7_0 = Vector2.Zero;
				if (flag)
				{
					this.m_silhouette.X = (float)(-(float)this.m_silhouette.Width);
				}
				else
				{
					this.m_silhouette.Flip = SpriteEffects.FlipHorizontally;
					this.m_silhouette.X = (float)(this.m_levelScreen.CurrentRoom.Width + this.m_silhouette.Width);
				}
				this.m_silhouette.Y = CDGMath.RandomFloat(100f, 500f);
				int num = this.m_levelScreen.CurrentRoom.Bounds.Width + this.m_silhouette.Width * 2;
				if (!flag)
				{
					num = -num;
				}
				Tween.By(this.m_silhouette, CDGMath.RandomFloat(10f, 15f), new Easing(Tween.EaseNone), new string[]
				{
					"X",
					num.ToString(),
					"Y",
					CDGMath.RandomInt(-200, 200).ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "SilhouetteComplete", new object[0]);
			}
		}
		public void SilhouetteComplete()
		{
			this.m_silhouetteFlying = false;
		}
		public override void Draw(Camera2D camera)
		{
			this.m_moon.X = this.m_moonPos.X - camera.TopLeftCorner.X * 0.01f;
			this.m_moon.Y = this.m_moonPos.Y - camera.TopLeftCorner.Y * 0.01f;
			camera.GraphicsDevice.Clear(new Color(4, 29, 86));
			camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.SkyBlue * this.MorningOpacity);
			this.m_moon.Opacity = 1f - this.MorningOpacity;
			this.m_silhouette.Opacity = 1f - this.MorningOpacity;
			this.m_differenceCloud.Opacity = 1f - this.MorningOpacity;
			this.m_differenceCloud2.Opacity = 1f - this.MorningOpacity;
			this.m_differenceCloud3.Opacity = this.MorningOpacity;
			this.m_moon.Draw(camera);
			this.m_differenceCloud.Draw(camera);
			this.m_differenceCloud2.Draw(camera);
			this.m_differenceCloud3.Draw(camera);
			this.m_silhouette.Draw(camera);
			base.Draw(camera);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new SkyObj(this.m_levelScreen);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_differenceCloud.Dispose();
				this.m_differenceCloud = null;
				this.m_differenceCloud2.Dispose();
				this.m_differenceCloud2 = null;
				this.m_differenceCloud3.Dispose();
				this.m_differenceCloud3 = null;
				this.m_moon.Dispose();
				this.m_moon = null;
				this.m_silhouette.Dispose();
				this.m_silhouette = null;
				this.m_levelScreen = null;
				base.Dispose();
			}
		}
	}
}
