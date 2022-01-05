using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randomchaos2DGodRays
{
    public class PostProcessingManager
    {
        protected Game Game;
        public Vector2 HalfPixel;
        public RenderTarget2D newScene;
        protected List<BasePostProcessingEffect> postProcessingEffects = new List<BasePostProcessingEffect>();
        public Texture2D Scene;

        public PostProcessingManager(Game game, SpriteBatch spriteBatch)
        {
            Game = game;
            this.spriteBatch = spriteBatch;
        }

        public SpriteBatch spriteBatch { get; private set; }

        public void AddEffect(BasePostProcessingEffect ppEfect)
        {
            postProcessingEffects.Add(ppEfect);
        }

        public virtual void Draw(GameTime gameTime, Texture2D scene)
        {
            HalfPixel = -new Vector2(0.5f / Game.GraphicsDevice.Viewport.Width,
                0.5f / Game.GraphicsDevice.Viewport.Height);
            var count = postProcessingEffects.Count;
            Scene = scene;
            for (var i = 0; i < count; i++)
            {
                if (!postProcessingEffects[i].Enabled)
                {
                    continue;
                }

                postProcessingEffects[i].HalfPixel = HalfPixel;
                postProcessingEffects[i].orgScene = scene;
                postProcessingEffects[i].Draw(gameTime, Scene);
                Scene = postProcessingEffects[i].lastScene;
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(Scene,
                new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height),
                Color.White);
            spriteBatch.End();
        }

        protected void SaveTexture(Texture2D texture, string name)
        {
            var fileStream = new FileStream(name, FileMode.Create);
            texture.SaveAsJpeg(fileStream, texture.Width, texture.Height);
            fileStream.Close();
        }
    }
}