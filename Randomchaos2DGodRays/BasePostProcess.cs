using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randomchaos2DGodRays
{
    public class BasePostProcess
    {
        public Texture2D BackBuffer;
        protected Effect effect;
        public bool Enabled = true;
        protected Game Game;
        public Vector2 HalfPixel;
        public RenderTarget2D newScene;
        public Texture2D orgBuffer;
        private ScreenQuad sq;
        public bool UsesVertexShader;

        public BasePostProcess(Game game)
        {
            Game = game;
        }

        protected SpriteBatch spriteBatch
        {
            get { return (SpriteBatch) Game.Services.GetService(typeof(SpriteBatch)); }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            if (sq == null)
            {
                sq = new ScreenQuad(Game);
                sq.Initialize();
            }

            effect.CurrentTechnique.Passes[0].Apply();
            sq.Draw();
        }
    }
}