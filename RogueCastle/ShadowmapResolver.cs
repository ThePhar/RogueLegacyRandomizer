using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class ShadowmapResolver
	{
		private GraphicsDevice graphicsDevice;
		private int reductionChainCount;
		private int baseSize;
		private int depthBufferSize;
		private Effect resolveShadowsEffect;
		private Effect reductionEffect;
		public static Effect blender;
		private RenderTarget2D distortRT;
		private RenderTarget2D shadowMap;
		private RenderTarget2D shadowsRT;
		private RenderTarget2D processedShadowsRT;
		private QuadRenderComponent quadRender;
		private RenderTarget2D distancesRT;
		private RenderTarget2D[] reductionRT;
		public ShadowmapResolver(GraphicsDevice graphicsDevice, QuadRenderComponent quadRender, ShadowmapSize maxShadowmapSize, ShadowmapSize maxDepthBufferSize)
		{
			this.graphicsDevice = graphicsDevice;
			this.quadRender = quadRender;
			this.reductionChainCount = (int)maxShadowmapSize;
			this.baseSize = 2 << this.reductionChainCount;
			this.depthBufferSize = 2 << (int)maxDepthBufferSize;
		}
		public void LoadContent(ContentManager content)
		{
			this.reductionEffect = content.Load<Effect>("Shaders\\reductionEffect");
			this.resolveShadowsEffect = content.Load<Effect>("Shaders\\resolveShadowsEffect");
			ShadowmapResolver.blender = content.Load<Effect>("Shaders\\2xMultiBlend");
			SurfaceFormat preferredFormat = SurfaceFormat.Color;
			this.distortRT = new RenderTarget2D(this.graphicsDevice, this.baseSize, this.baseSize, false, preferredFormat, DepthFormat.None);
			this.distancesRT = new RenderTarget2D(this.graphicsDevice, this.baseSize, this.baseSize, false, preferredFormat, DepthFormat.None);
			this.shadowMap = new RenderTarget2D(this.graphicsDevice, 2, this.baseSize, false, preferredFormat, DepthFormat.None);
			this.reductionRT = new RenderTarget2D[this.reductionChainCount];
			for (int i = 0; i < this.reductionChainCount; i++)
			{
				this.reductionRT[i] = new RenderTarget2D(this.graphicsDevice, 2 << i, this.baseSize, false, preferredFormat, DepthFormat.None);
			}
			this.shadowsRT = new RenderTarget2D(this.graphicsDevice, this.baseSize, this.baseSize);
			this.processedShadowsRT = new RenderTarget2D(this.graphicsDevice, this.baseSize, this.baseSize);
		}
		public void ResolveShadows(Texture2D shadowCastersTexture, RenderTarget2D result, Vector2 lightPosition)
		{
			this.graphicsDevice.BlendState = BlendState.Opaque;
			this.ExecuteTechnique(shadowCastersTexture, this.distancesRT, "ComputeDistances");
			this.ExecuteTechnique(this.distancesRT, this.distortRT, "Distort");
			this.ApplyHorizontalReduction(this.distortRT, this.shadowMap);
			this.ExecuteTechnique(null, this.shadowsRT, "DrawShadows", this.shadowMap);
			this.ExecuteTechnique(this.shadowsRT, this.processedShadowsRT, "BlurHorizontally");
			this.ExecuteTechnique(this.processedShadowsRT, result, "BlurVerticallyAndAttenuate");
		}
		private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName)
		{
			this.ExecuteTechnique(source, destination, techniqueName, null);
		}
		private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName, Texture2D shadowMap)
		{
			Vector2 value = new Vector2((float)this.baseSize, (float)this.baseSize);
			this.graphicsDevice.SetRenderTarget(destination);
			this.graphicsDevice.Clear(Color.White);
			this.resolveShadowsEffect.Parameters["renderTargetSize"].SetValue(value);
			if (source != null)
			{
				this.resolveShadowsEffect.Parameters["InputTexture"].SetValue(source);
			}
			if (shadowMap != null)
			{
				this.resolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);
			}
			this.resolveShadowsEffect.CurrentTechnique = this.resolveShadowsEffect.Techniques[techniqueName];
			foreach (EffectPass current in this.resolveShadowsEffect.CurrentTechnique.Passes)
			{
				current.Apply();
				this.quadRender.Render(Vector2.One * -1f, Vector2.One);
			}
			this.graphicsDevice.SetRenderTarget(null);
		}
		private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
		{
			int i = this.reductionChainCount - 1;
			RenderTarget2D renderTarget2D = source;
			RenderTarget2D renderTarget2D2 = this.reductionRT[i];
			this.reductionEffect.CurrentTechnique = this.reductionEffect.Techniques["HorizontalReduction"];
			while (i >= 0)
			{
				renderTarget2D2 = this.reductionRT[i];
				this.graphicsDevice.SetRenderTarget(renderTarget2D2);
				this.graphicsDevice.Clear(Color.White);
				this.reductionEffect.Parameters["SourceTexture"].SetValue(renderTarget2D);
				Vector2 value = new Vector2(1f / (float)renderTarget2D.Width, 1f / (float)renderTarget2D.Height);
				this.reductionEffect.Parameters["TextureDimensions"].SetValue(value);
				foreach (EffectPass current in this.reductionEffect.CurrentTechnique.Passes)
				{
					current.Apply();
					this.quadRender.Render(Vector2.One * -1f, new Vector2(1f, 1f));
				}
				this.graphicsDevice.SetRenderTarget(null);
				renderTarget2D = renderTarget2D2;
				i--;
			}
			this.graphicsDevice.SetRenderTarget(destination);
			this.reductionEffect.CurrentTechnique = this.reductionEffect.Techniques["Copy"];
			this.reductionEffect.Parameters["SourceTexture"].SetValue(renderTarget2D2);
			foreach (EffectPass current2 in this.reductionEffect.CurrentTechnique.Passes)
			{
				current2.Apply();
				this.quadRender.Render(Vector2.One * -1f, new Vector2(1f, 1f));
			}
			this.reductionEffect.Parameters["SourceTexture"].SetValue(this.reductionRT[this.reductionChainCount - 1]);
			this.graphicsDevice.SetRenderTarget(null);
		}
	}
}
