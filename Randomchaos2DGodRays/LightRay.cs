using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Randomchaos2DGodRays
{
	public class LightRay : BasePostProcess
	{
		public Vector2 lighScreenSourcePos;
		public float Density = 0.5f;
		public float Decay = 0.95f;
		public float Weight = 1f;
		public float Exposure = 0.15f;
		public LightRay(Game game, Vector2 sourcePos, float density, float decay, float weight, float exposure) : base(game)
		{
			this.lighScreenSourcePos = sourcePos;
			this.Density = density;
			this.Decay = decay;
			this.Weight = weight;
			this.Exposure = exposure;
			this.UsesVertexShader = true;
		}
		public override void Draw(GameTime gameTime)
		{
			if (this.effect == null)
			{
				this.effect = this.Game.Content.Load<Effect>("Shaders/LightRays");
			}
			this.effect.CurrentTechnique = this.effect.Techniques["LightRayFX"];
			this.effect.Parameters["halfPixel"].SetValue(this.HalfPixel);
			this.effect.Parameters["Density"].SetValue(this.Density);
			this.effect.Parameters["Decay"].SetValue(this.Decay);
			this.effect.Parameters["Weight"].SetValue(this.Weight);
			this.effect.Parameters["Exposure"].SetValue(this.Exposure);
			this.effect.Parameters["lightScreenPosition"].SetValue(this.lighScreenSourcePos);
			base.Draw(gameTime);
		}
	}
}
