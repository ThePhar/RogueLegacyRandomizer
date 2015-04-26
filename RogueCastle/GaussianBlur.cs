/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
    public class GaussianBlur
    {
        private readonly Effect effect;
        private readonly EffectParameter m_offsetParameters;
        private readonly RenderTarget2D m_renderHolder;
        private readonly RenderTarget2D m_renderHolder2;
        private float amount;
        private bool m_invertMask;
        private int radius;

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
                m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth/2, screenHeight/2);
                m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth/2, screenHeight/2);
            }
            else
            {
                m_renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
                m_renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight);
            }
            effect = game.Content.Load<Effect>("Shaders\\GaussianBlurMask");
            m_offsetParameters = effect.Parameters["offsets"];
        }

        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                ComputeKernel();
            }
        }

        public float Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                ComputeKernel();
            }
        }

        public float Sigma { get; private set; }
        public float[] Kernel { get; private set; }
        public Vector2[] TextureOffsetsX { get; private set; }
        public Vector2[] TextureOffsetsY { get; private set; }

        public bool InvertMask
        {
            get { return m_invertMask; }
            set
            {
                m_invertMask = value;
                effect.Parameters["invert"].SetValue(m_invertMask);
            }
        }

        public void ComputeKernel()
        {
            Kernel = null;
            Kernel = new float[radius*2 + 1];
            Sigma = radius/amount;
            var num = 2f*Sigma*Sigma;
            var num2 = (float) Math.Sqrt(num*3.1415926535897931);
            var num3 = 0f;
            for (var i = -radius; i <= radius; i++)
            {
                float num4 = i*i;
                var num5 = i + radius;
                Kernel[num5] = (float) Math.Exp(-(double) num4/num)/num2;
                num3 += Kernel[num5];
            }
            for (var j = 0; j < Kernel.Length; j++)
            {
                Kernel[j] /= num3;
            }
            effect.Parameters["weights"].SetValue(Kernel);
        }

        public void ComputeOffsets()
        {
            TextureOffsetsX = null;
            TextureOffsetsX = new Vector2[radius*2 + 1];
            TextureOffsetsY = null;
            TextureOffsetsY = new Vector2[radius*2 + 1];
            var num = 1f/m_renderHolder.Width;
            var num2 = 1f/m_renderHolder.Height;
            for (var i = -radius; i <= radius; i++)
            {
                var num3 = i + radius;
                TextureOffsetsX[num3] = new Vector2(i*num, 0f);
                TextureOffsetsY[num3] = new Vector2(0f, i*num2);
            }
        }

        public void Draw(RenderTarget2D srcTexture, Camera2D Camera, RenderTarget2D mask = null)
        {
            if (effect == null)
            {
                throw new InvalidOperationException("GaussianBlur.fx effect not loaded.");
            }
            Camera.GraphicsDevice.SetRenderTarget(m_renderHolder);
            m_offsetParameters.SetValue(TextureOffsetsX);
            if (mask != null)
            {
                Camera.GraphicsDevice.Textures[1] = mask;
                Camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            }
            Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, null, null, effect);
            if (LevelEV.SAVE_FRAMES)
            {
                Camera.Draw(srcTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f),
                    SpriteEffects.None, 1f);
            }
            else
            {
                Camera.Draw(srcTexture, Vector2.Zero, Color.White);
            }
            Camera.End();
            if (LevelEV.SAVE_FRAMES)
            {
                Camera.GraphicsDevice.SetRenderTarget(m_renderHolder2);
                m_offsetParameters.SetValue(TextureOffsetsY);
                if (mask != null)
                {
                    Camera.GraphicsDevice.Textures[1] = mask;
                }
                Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
                Camera.Draw(m_renderHolder, Vector2.Zero, Color.White);
                Camera.End();
                Camera.GraphicsDevice.SetRenderTarget(srcTexture);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null);
                Camera.Draw(m_renderHolder2, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f),
                    SpriteEffects.None, 1f);
                Camera.End();
                return;
            }
            Camera.GraphicsDevice.SetRenderTarget(srcTexture);
            m_offsetParameters.SetValue(TextureOffsetsY);
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