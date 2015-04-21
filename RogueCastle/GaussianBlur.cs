using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
				return this.radius;
			}
			set
			{
				this.radius = value;
				this.ComputeKernel();
			}
		}
		public float Amount
		{
			get
			{
				return this.amount;
			}
			set
			{
				this.amount = value;
				this.ComputeKernel();
			}
		}
		public float Sigma
		{
			get
			{
				return this.sigma;
			}
		}
		public float[] Kernel
		{
			get
			{
				return this.kernel;
			}
		}
		public Vector2[] TextureOffsetsX
		{
			get
			{
				return this.offsetsHoriz;
			}
		}
		public Vector2[] TextureOffsetsY
		{
			get
			{
				return this.offsetsVert;
			}
		}
		public bool InvertMask
		{
			get
			{
				return this.m_invertMask;
			}
			set
			{
				this.m_invertMask = value;
				this.effect.Parameters["invert"].SetValue(this.m_invertMask);
			}
		}
		public GaussianBlur()
		{
		}
		public GaussianBlur(Game game, int screenWidth, int screenHeight)
		{
			if (this.m_renderHolder != null && !this.m_renderHolder.IsDisposed)
			{
				this.m_renderHolder.Dispose();
			}
			if (this.m_renderHolder2 != null && !this.m_renderHolder2.IsDisposed)
			{
				this.m_renderHolder2.Dispose();
			}
			if (LevelEV.SAVE_FRAMES)
			{
				this.m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2);
				this.m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2);
			}
			else
			{
				this.m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
				this.m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
			}
			this.effect = game.Content.Load<Effect>("Shaders\\GaussianBlurMask");
			this.m_offsetParameters = this.effect.Parameters["offsets"];
		}
		public void ComputeKernel()
		{
			this.kernel = null;
			this.kernel = new float[this.radius * 2 + 1];
			this.sigma = (float)this.radius / this.amount;
			float num = 2f * this.sigma * this.sigma;
			float num2 = (float)Math.Sqrt((double)num * 3.1415926535897931);
			float num3 = 0f;
			for (int i = -this.radius; i <= this.radius; i++)
			{
				float num4 = (float)(i * i);
				int num5 = i + this.radius;
				this.kernel[num5] = (float)Math.Exp((double)(-(double)num4 / num)) / num2;
				num3 += this.kernel[num5];
			}
			for (int j = 0; j < this.kernel.Length; j++)
			{
				this.kernel[j] /= num3;
			}
			this.effect.Parameters["weights"].SetValue(this.kernel);
		}
		public void ComputeOffsets()
		{
			this.offsetsHoriz = null;
			this.offsetsHoriz = new Vector2[this.radius * 2 + 1];
			this.offsetsVert = null;
			this.offsetsVert = new Vector2[this.radius * 2 + 1];
			float num = 1f / (float)this.m_renderHolder.Width;
			float num2 = 1f / (float)this.m_renderHolder.Height;
			for (int i = -this.radius; i <= this.radius; i++)
			{
				int num3 = i + this.radius;
				this.offsetsHoriz[num3] = new Vector2((float)i * num, 0f);
				this.offsetsVert[num3] = new Vector2(0f, (float)i * num2);
			}
		}
		public void Draw(RenderTarget2D srcTexture, Camera2D Camera, RenderTarget2D mask = null)
		{
			if (this.effect == null)
			{
				throw new InvalidOperationException("GaussianBlur.fx effect not loaded.");
			}
			Camera.GraphicsDevice.SetRenderTarget(this.m_renderHolder);
			this.m_offsetParameters.SetValue(this.offsetsHoriz);
			if (mask != null)
			{
				Camera.GraphicsDevice.Textures[1] = mask;
				Camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			}
			Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, null, null, this.effect);
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
				Camera.GraphicsDevice.SetRenderTarget(this.m_renderHolder2);
				this.m_offsetParameters.SetValue(this.offsetsVert);
				if (mask != null)
				{
					Camera.GraphicsDevice.Textures[1] = mask;
				}
				Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, this.effect);
				Camera.Draw(this.m_renderHolder, Vector2.Zero, Color.White);
				Camera.End();
				Camera.GraphicsDevice.SetRenderTarget(srcTexture);
				Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null);
				Camera.Draw(this.m_renderHolder2, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 1f);
				Camera.End();
				return;
			}
			Camera.GraphicsDevice.SetRenderTarget(srcTexture);
			this.m_offsetParameters.SetValue(this.offsetsVert);
			if (mask != null)
			{
				Camera.GraphicsDevice.Textures[1] = mask;
			}
			Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, this.effect);
			Camera.Draw(this.m_renderHolder, Vector2.Zero, Color.White);
			Camera.End();
		}
	}
}
