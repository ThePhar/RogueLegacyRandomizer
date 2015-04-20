/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			m_levelScreen = levelScreen;
		}
		public void LoadContent(Camera2D camera)
		{
			Vector2 one = new Vector2(2f, 2f);
			m_moon = new SpriteObj("ParallaxMoon_Sprite");
			m_moon.Position = new Vector2(900f, 200f);
			if (LevelEV.SAVE_FRAMES)
			{
				m_moon.Position /= 2f;
				one = Vector2.One;
			}
			m_moon.Scale = one;
			m_moon.ForceDraw = true;
			m_moonPos = m_moon.Position;
			m_differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud.Scale = one;
			m_differenceCloud.TextureColor = new Color(10, 10, 80);
			m_differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);
			m_differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud2.Scale = one;
			m_differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
			m_differenceCloud2.TextureColor = new Color(80, 80, 160);
			m_differenceCloud2.X -= 500f;
			m_differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);
			m_differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud3.Scale = one;
			m_differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
			m_differenceCloud3.TextureColor = Color.White;
			m_differenceCloud3.X -= 500f;
			m_differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
			m_silhouette = new SpriteObj("GardenBat_Sprite");
			m_silhouette.ForceDraw = true;
			m_silhouette.AnimationDelay = 0.05f;
			m_silhouette.Scale = one;
		}
		public void ReinitializeRT(Camera2D camera)
		{
			Vector2 one = new Vector2(2f, 2f);
			if (LevelEV.SAVE_FRAMES)
			{
				m_moon.Position /= 2f;
				one = Vector2.One;
			}
			if (m_differenceCloud != null)
			{
				m_differenceCloud.Dispose();
			}
			m_differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud.Scale = one;
			m_differenceCloud.TextureColor = new Color(10, 10, 80);
			m_differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);
			if (m_differenceCloud2 != null)
			{
				m_differenceCloud2.Dispose();
			}
			m_differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud2.Scale = one;
			m_differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
			m_differenceCloud2.TextureColor = new Color(80, 80, 160);
			m_differenceCloud2.X -= 500f;
			m_differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);
			if (m_differenceCloud3 != null)
			{
				m_differenceCloud3.Dispose();
			}
			m_differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			m_differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
			m_differenceCloud3.Scale = one;
			m_differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
			m_differenceCloud3.TextureColor = new Color(80, 80, 160);
			m_differenceCloud3.X -= 500f;
			m_differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
		}
		public void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!m_silhouetteFlying && m_silhouetteTimer > 0f)
			{
				m_silhouetteTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_silhouetteTimer <= 0f)
				{
					int num2 = CDGMath.RandomInt(1, 100);
					if (num2 > 95)
					{
						ShowSilhouette(true);
					}
					else if (num2 > 65)
					{
						ShowSilhouette(false);
					}
					else
					{
						m_silhouetteTimer = 5f;
					}
				}
			}
			if (!m_silhouetteFlying && m_silhouetteTimer <= 0f)
			{
				m_silhouetteTimer = 5f;
			}
			if (m_silhouette.SpriteName == "GardenPerson_Sprite")
			{
				m_silhouette.Rotation += 120f * num;
			}
		}
		private void ShowSilhouette(bool showSanta)
		{
			if (m_levelScreen != null)
			{
				m_silhouetteFlying = true;
				m_silhouette.Rotation = 0f;
				m_silhouette.Flip = SpriteEffects.None;
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
					m_silhouette.ChangeSprite(array[CDGMath.RandomInt(0, 4)]);
				}
				else
				{
					m_silhouette.ChangeSprite("GardenSanta_Sprite");
				}
				m_silhouette.PlayAnimation(true);
				Vector2 arg_A7_0 = Vector2.Zero;
				if (flag)
				{
					m_silhouette.X = -(float)m_silhouette.Width;
				}
				else
				{
					m_silhouette.Flip = SpriteEffects.FlipHorizontally;
					m_silhouette.X = m_levelScreen.CurrentRoom.Width + m_silhouette.Width;
				}
				m_silhouette.Y = CDGMath.RandomFloat(100f, 500f);
				int num = m_levelScreen.CurrentRoom.Bounds.Width + m_silhouette.Width * 2;
				if (!flag)
				{
					num = -num;
				}
				Tween.By(m_silhouette, CDGMath.RandomFloat(10f, 15f), new Easing(Tween.EaseNone), new string[]
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
			m_silhouetteFlying = false;
		}
		public override void Draw(Camera2D camera)
		{
			m_moon.X = m_moonPos.X - camera.TopLeftCorner.X * 0.01f;
			m_moon.Y = m_moonPos.Y - camera.TopLeftCorner.Y * 0.01f;
			camera.GraphicsDevice.Clear(new Color(4, 29, 86));
			camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.SkyBlue * MorningOpacity);
			m_moon.Opacity = 1f - MorningOpacity;
			m_silhouette.Opacity = 1f - MorningOpacity;
			m_differenceCloud.Opacity = 1f - MorningOpacity;
			m_differenceCloud2.Opacity = 1f - MorningOpacity;
			m_differenceCloud3.Opacity = MorningOpacity;
			m_moon.Draw(camera);
			m_differenceCloud.Draw(camera);
			m_differenceCloud2.Draw(camera);
			m_differenceCloud3.Draw(camera);
			m_silhouette.Draw(camera);
			base.Draw(camera);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new SkyObj(m_levelScreen);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_differenceCloud.Dispose();
				m_differenceCloud = null;
				m_differenceCloud2.Dispose();
				m_differenceCloud2 = null;
				m_differenceCloud3.Dispose();
				m_differenceCloud3 = null;
				m_moon.Dispose();
				m_moon = null;
				m_silhouette.Dispose();
				m_silhouette = null;
				m_levelScreen = null;
				base.Dispose();
			}
		}
	}
}
