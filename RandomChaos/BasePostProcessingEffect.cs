// 
//  Rogue Legacy Randomizer - BasePostProcessingEffect.cs
//  Last Modified 2022-04-08
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RandomChaos
{
    public class BasePostProcessingEffect
    {
        public    bool                  Enabled = true;
        public    Vector2               HalfPixel;
        public    Texture2D             LastScene;
        public    Texture2D             OrgScene;
        protected Game                  Game;
        protected List<BasePostProcess> PostProcesses = new();

        public BasePostProcessingEffect(Game game)
        {
            Game = game;
        }

        public void AddPostProcess(BasePostProcess postProcess)
        {
            PostProcesses.Add(postProcess);
        }

        public void Draw(GameTime gameTime, Texture2D scene)
        {
            if (!Enabled)
            {
                return;
            }

            OrgScene = scene;
            var count = PostProcesses.Count;
            LastScene = null;
            for (var i = 0; i < count; i++)
            {
                if (!PostProcesses[i].Enabled)
                {
                    continue;
                }

                PostProcesses[i].HalfPixel = HalfPixel;
                PostProcesses[i].OrgBuffer = OrgScene;
                PostProcesses[i].NewScene ??= new RenderTarget2D(Game.GraphicsDevice, Game.GraphicsDevice.Viewport.Width / 2,
                    Game.GraphicsDevice.Viewport.Height / 2, false, SurfaceFormat.Color, DepthFormat.None);

                Game.GraphicsDevice.SetRenderTarget(PostProcesses[i].NewScene);
                LastScene ??= OrgScene;

                PostProcesses[i].BackBuffer = LastScene;
                Game.GraphicsDevice.Textures[0] = PostProcesses[i].BackBuffer;
                PostProcesses[i].Draw(gameTime);
                Game.GraphicsDevice.SetRenderTarget(null);
                LastScene = PostProcesses[i].NewScene;
            }

            LastScene ??= scene;
        }
    }
}
