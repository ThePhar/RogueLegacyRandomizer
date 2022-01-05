using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randomchaos2DGodRays
{
    public class LightSourceMask : BasePostProcess
    {
        public Vector2 lighScreenSourcePos;
        public float lightSize = 1500f;
        public string lightSourceasset;
        public Texture lishsourceTexture;

        public LightSourceMask(Game game, Vector2 sourcePos, string lightSourceasset, float lightSize) : base(game)
        {
            UsesVertexShader = true;
            lighScreenSourcePos = sourcePos;
            this.lightSourceasset = lightSourceasset;
            this.lightSize = lightSize;
        }

        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/LightSourceMask");
                lishsourceTexture = Game.Content.Load<Texture2D>(lightSourceasset);
            }

            effect.Parameters["screenRes"].SetValue(new Vector2(16f, 9f));
            effect.Parameters["halfPixel"].SetValue(HalfPixel);
            effect.CurrentTechnique = effect.Techniques["LightSourceMask"];
            effect.Parameters["flare"].SetValue(lishsourceTexture);
            effect.Parameters["SunSize"].SetValue(lightSize);
            effect.Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);
            base.Draw(gameTime);
        }
    }
}