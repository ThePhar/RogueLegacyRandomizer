using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Randomchaos2DGodRays
{
	public class LightSourceMask : BasePostProcess
	{
		public Texture lishsourceTexture;
		public Vector2 lighScreenSourcePos;
		public string lightSourceasset;
		public float lightSize = 1500f;
		public LightSourceMask(Game game, Vector2 sourcePos, string lightSourceasset, float lightSize) : base(game)
		{
			this.UsesVertexShader = true;
			this.lighScreenSourcePos = sourcePos;
			this.lightSourceasset = lightSourceasset;
			this.lightSize = lightSize;
		}
		public override void Draw(GameTime gameTime)
		{
			if (this.effect == null)
			{
				this.effect = this.Game.Content.Load<Effect>("Shaders/LightSourceMask");
				this.lishsourceTexture = this.Game.Content.Load<Texture2D>(this.lightSourceasset);
			}
			this.effect.Parameters["screenRes"].SetValue(new Vector2(16f, 9f));
			this.effect.Parameters["halfPixel"].SetValue(this.HalfPixel);
			this.effect.CurrentTechnique = this.effect.Techniques["LightSourceMask"];
			this.effect.Parameters["flare"].SetValue(this.lishsourceTexture);
			this.effect.Parameters["SunSize"].SetValue(this.lightSize);
			this.effect.Parameters["lightScreenPosition"].SetValue(this.lighScreenSourcePos);
			base.Draw(gameTime);
		}
	}
}
