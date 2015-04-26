/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
    public class ShadowmapResolver
    {
        public static Effect blender;
        private readonly int baseSize;
        private readonly GraphicsDevice graphicsDevice;
        private readonly QuadRenderComponent quadRender;
        private readonly int reductionChainCount;
        private int depthBufferSize;
        private RenderTarget2D distancesRT;
        private RenderTarget2D distortRT;
        private RenderTarget2D processedShadowsRT;
        private Effect reductionEffect;
        private RenderTarget2D[] reductionRT;
        private Effect resolveShadowsEffect;
        private RenderTarget2D shadowMap;
        private RenderTarget2D shadowsRT;

        public ShadowmapResolver(GraphicsDevice graphicsDevice, QuadRenderComponent quadRender,
            ShadowmapSize maxShadowmapSize, ShadowmapSize maxDepthBufferSize)
        {
            this.graphicsDevice = graphicsDevice;
            this.quadRender = quadRender;
            reductionChainCount = (int) maxShadowmapSize;
            baseSize = 2 << reductionChainCount;
            depthBufferSize = 2 << (int) maxDepthBufferSize;
        }

        public void LoadContent(ContentManager content)
        {
            reductionEffect = content.Load<Effect>("Shaders\\reductionEffect");
            resolveShadowsEffect = content.Load<Effect>("Shaders\\resolveShadowsEffect");
            blender = content.Load<Effect>("Shaders\\2xMultiBlend");
            var preferredFormat = SurfaceFormat.Color;
            distortRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize, false, preferredFormat, DepthFormat.None);
            distancesRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize, false, preferredFormat,
                DepthFormat.None);
            shadowMap = new RenderTarget2D(graphicsDevice, 2, baseSize, false, preferredFormat, DepthFormat.None);
            reductionRT = new RenderTarget2D[reductionChainCount];
            for (var i = 0; i < reductionChainCount; i++)
            {
                reductionRT[i] = new RenderTarget2D(graphicsDevice, 2 << i, baseSize, false, preferredFormat,
                    DepthFormat.None);
            }
            shadowsRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
            processedShadowsRT = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
        }

        public void ResolveShadows(Texture2D shadowCastersTexture, RenderTarget2D result, Vector2 lightPosition)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            ExecuteTechnique(shadowCastersTexture, distancesRT, "ComputeDistances");
            ExecuteTechnique(distancesRT, distortRT, "Distort");
            ApplyHorizontalReduction(distortRT, shadowMap);
            ExecuteTechnique(null, shadowsRT, "DrawShadows", shadowMap);
            ExecuteTechnique(shadowsRT, processedShadowsRT, "BlurHorizontally");
            ExecuteTechnique(processedShadowsRT, result, "BlurVerticallyAndAttenuate");
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName)
        {
            ExecuteTechnique(source, destination, techniqueName, null);
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName,
            Texture2D shadowMap)
        {
            var value = new Vector2(baseSize, baseSize);
            graphicsDevice.SetRenderTarget(destination);
            graphicsDevice.Clear(Color.White);
            resolveShadowsEffect.Parameters["renderTargetSize"].SetValue(value);
            if (source != null)
            {
                resolveShadowsEffect.Parameters["InputTexture"].SetValue(source);
            }
            if (shadowMap != null)
            {
                resolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);
            }
            resolveShadowsEffect.CurrentTechnique = resolveShadowsEffect.Techniques[techniqueName];
            foreach (var current in resolveShadowsEffect.CurrentTechnique.Passes)
            {
                current.Apply();
                quadRender.Render(Vector2.One*-1f, Vector2.One);
            }
            graphicsDevice.SetRenderTarget(null);
        }

        private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
        {
            var i = reductionChainCount - 1;
            var renderTarget2D = source;
            var renderTarget2D2 = reductionRT[i];
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["HorizontalReduction"];
            while (i >= 0)
            {
                renderTarget2D2 = reductionRT[i];
                graphicsDevice.SetRenderTarget(renderTarget2D2);
                graphicsDevice.Clear(Color.White);
                reductionEffect.Parameters["SourceTexture"].SetValue(renderTarget2D);
                var value = new Vector2(1f/renderTarget2D.Width, 1f/renderTarget2D.Height);
                reductionEffect.Parameters["TextureDimensions"].SetValue(value);
                foreach (var current in reductionEffect.CurrentTechnique.Passes)
                {
                    current.Apply();
                    quadRender.Render(Vector2.One*-1f, new Vector2(1f, 1f));
                }
                graphicsDevice.SetRenderTarget(null);
                renderTarget2D = renderTarget2D2;
                i--;
            }
            graphicsDevice.SetRenderTarget(destination);
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["Copy"];
            reductionEffect.Parameters["SourceTexture"].SetValue(renderTarget2D2);
            foreach (var current2 in reductionEffect.CurrentTechnique.Passes)
            {
                current2.Apply();
                quadRender.Render(Vector2.One*-1f, new Vector2(1f, 1f));
            }
            reductionEffect.Parameters["SourceTexture"].SetValue(reductionRT[reductionChainCount - 1]);
            graphicsDevice.SetRenderTarget(null);
        }
    }
}