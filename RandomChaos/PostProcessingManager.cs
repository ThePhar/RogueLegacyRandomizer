// 
//  Rogue Legacy Randomizer - PostProcessingManager.cs
//  Last Modified 2022-04-08
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RandomChaos
{
    public class PostProcessingManager
    {
        public    Vector2                        HalfPixel;
        public    RenderTarget2D                 NewScene;
        public    Texture2D                      Scene;
        protected Game                           Game;
        protected List<BasePostProcessingEffect> PostProcessingEffects = new();

        public PostProcessingManager(Game game, SpriteBatch spriteBatch)
        {
            Game = game;
            SpriteBatch = spriteBatch;
        }

        public SpriteBatch SpriteBatch { get; }

        public void AddEffect(BasePostProcessingEffect effect)
        {
            PostProcessingEffects.Add(effect);
        }

        public virtual void Draw(GameTime gameTime, Texture2D scene)
        {
            HalfPixel = -new Vector2(0.5f / Game.GraphicsDevice.Viewport.Width, 0.5f / Game.GraphicsDevice.Viewport.Height);
            var count = PostProcessingEffects.Count;
            Scene = scene;
            for (var i = 0; i < count; i++)
            {
                if (!PostProcessingEffects[i].Enabled)
                {
                    continue;
                }

                PostProcessingEffects[i].HalfPixel = HalfPixel;
                PostProcessingEffects[i].OrgScene = scene;
                PostProcessingEffects[i].Draw(gameTime, Scene);
                Scene = PostProcessingEffects[i].LastScene;
            }

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            SpriteBatch.Draw(Scene, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
            SpriteBatch.End();
        }

        protected void SaveTexture(Texture2D texture, string name)
        {
            var fileStream = new FileStream(name, FileMode.Create);
            texture.SaveAsJpeg(fileStream, texture.Width, texture.Height);
            fileStream.Close();
        }
    }
}
