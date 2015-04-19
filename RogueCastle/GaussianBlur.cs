using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class GaussianBlur
	{
		private Effect effect;
		private int radius;
		private float amount;
		private float sigma;
		private float[] kernel;
		private Vector2[] offsetsHoriz;
		private Vector2[] offsetsVert;
		private RenderTarget2D m_renderHolder;
		private RenderTarget2D m_renderHolder2;
		private bool m_invertMask;
		private EffectParameter m_offsetParameters;
		public int Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
				ComputeKernel();
			}
		}
		public float Amount
		{
			get
			{
				return amount;
			}
			set
			{
				amount = value;
				ComputeKernel();
			}
		}
		public float Sigma
		{
			get
			{
				return sigma;
			}
		}
		public float[] Kernel
		{
			get
			{
				return kernel;
			}
		}
		public Vector2[] TextureOffsetsX
		{
			get
			{
				return offsetsHoriz;
			}
		}
		public Vector2[] TextureOffsetsY
		{
			get
			{
				return offsetsVert;
			}
		}
		public bool InvertMask
		{
			get
			{
				return m_invertMask;
			}
			set
			{
				m_invertMask = value;
				effect.Parameters["invert"].SetValue(m_invertMask);
			}
		}
		public GaussianBlur()
		{
		}
		public GaussianBlur(Game game, int screenWidth, int screenHeight)
		{
			if (m_renderHolder != null && !m_renderHolder.IsDisposed)
			{
				m_renderHolder.Dispose();
			}
			if (m_renderHolder2 != null && !m_renderHolder2.IsDisposed)
			{
				m_renderHolder2.Dispose();
			}
			if (LevelEV.SAVE_FRAMES)
			{
				m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2);
				m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2);
			}
			else
			{
				m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
				m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
			}
			effect = game.Content.Load<Effect>("Shaders\\GaussianBlurMask");
			m_offsetParameters = effect.Parameters["offsets"];
		}
		public void ComputeKernel()
		{
			kernel = null;
			kernel = new float[radius * 2 + 1];
			sigma = radius / amount;
			float num = 2f * sigma * sigma;
			float num2 = (float)Math.Sqrt(num * 3.1415926535897931);
			float num3 = 0f;
			for (int i = -radius; i <= radius; i++)
			{
				float num4 = i * i;
				int num5 = i + radius;
				kernel[num5] = (float)Math.Exp(-(double)num4 / num) / num2;
				num3 += kernel[num5];
			}
			for (int j = 0; j < kernel.Length; j++)
			{
				kernel[j] /= num3;
			}
			effect.Parameters["weights"].SetValue(kernel);
		}
		public void ComputeOffsets()
		{
			offsetsHoriz = null;
			offsetsHoriz = new Vector2[radius * 2 + 1];
			offsetsVert = null;
			offsetsVert = new Vector2[radius * 2 + 1];
			float num = 1f / m_renderHolder.Width;
			float num2 = 1f / m_renderHolder.Height;
			for (int i = -radius; i <= radius; i++)
			{
				int num3 = i + radius;
				offsetsHoriz[num3] = new Vector2(i * num, 0f);
				offsetsVert[num3] = new Vector2(0f, i * num2);
			}
		}
		public void Draw(RenderTarget2D srcTexture, Camera2D Camera, RenderTarget2D mask = null)
		{
			if (effect == null)
			{
				throw new InvalidOperationException("GaussianBlur.fx effect not loaded.");
			}
			Camera.GraphicsDevice.SetRenderTarget(m_renderHolder);
			m_offsetParameters.SetValue(offsetsHoriz);
			if (mask != null)
			{
				Camera.GraphicsDevice.Textures[1] = mask;
				Camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			}
			Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, null, null, effect);
			if (LevelEV.SAVE_FRAMES)
			{
				Camera.Draw(srcTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 1f);
			}
			else
			{
				Camera.Draw(srcTexture, Vector2.Zero, Color.White);
			}
			Camera.End();
			if (LevelEV.SAVE_FRAMES)
			{
				Camera.GraphicsDevice.SetRenderTarget(m_renderHolder2);
				m_offsetParameters.SetValue(offsetsVert);
				if (mask != null)
				{
					Camera.GraphicsDevice.Textures[1] = mask;
				}
				Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
				Camera.Draw(m_renderHolder, Vector2.Zero, Color.White);
				Camera.End();
				Camera.GraphicsDevice.SetRenderTarget(srcTexture);
				Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null);
				Camera.Draw(m_renderHolder2, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 1f);
				Camera.End();
				return;
			}
			Camera.GraphicsDevice.SetRenderTarget(srcTexture);
			m_offsetParameters.SetValue(offsetsVert);
			if (mask != null)
			{
				Camera.GraphicsDevice.Textures[1] = mask;
			}
			Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
			Camera.Draw(m_renderHolder, Vector2.Zero, Color.White);
			Camera.End();
		}
	}
}
