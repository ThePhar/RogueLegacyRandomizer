/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSystem;

namespace RogueCastle
{
    internal class VirtualScreen
    {
        public readonly int VirtualWidth;
        public readonly int VirtualHeight;
        public readonly float VirtualAspectRatio;
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D screen;
        private bool areaIsDirty = true;
        private Rectangle area;

        public RenderTarget2D RenderTarget
        {
            get { return screen; }
        }

        public VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            VirtualAspectRatio = virtualWidth/(float) virtualHeight;
            this.graphicsDevice = graphicsDevice;
            screen = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight);
        }

        public void ReinitializeRTs(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            if (!screen.IsDisposed)
            {
                screen.Dispose();
                screen = null;
            }
            screen = new RenderTarget2D(graphicsDevice, VirtualWidth, VirtualHeight);
        }

        public void PhysicalResolutionChanged()
        {
            areaIsDirty = true;
        }

        public void Update()
        {
            if (!areaIsDirty)
            {
                return;
            }
            areaIsDirty = false;
            int width = graphicsDevice.Viewport.Width;
            int height = graphicsDevice.Viewport.Height;
            float aspectRatio = graphicsDevice.Viewport.AspectRatio;
            if ((int) (aspectRatio*10f) == (int) (VirtualAspectRatio*10f))
            {
                area = new Rectangle(0, 0, width, height);
                return;
            }
            if (VirtualAspectRatio > aspectRatio)
            {
                float num = width/(float) VirtualWidth;
                float num2 = VirtualWidth*num;
                float num3 = VirtualHeight*num;
                int y = (int) ((height - num3)/2f);
                area = new Rectangle(0, y, (int) num2, (int) num3);
                return;
            }
            float num4 = height/(float) VirtualHeight;
            float num5 = VirtualWidth*num4;
            float num6 = VirtualHeight*num4;
            int x = (int) ((width - num5)/2f);
            area = new Rectangle(x, 0, (int) num5, (int) num6);
        }

        public void RecreateGraphics()
        {
            Console.WriteLine("GraphicsDevice Virtualization failed");
            GraphicsDevice graphicsDevice = (Game.ScreenManager.Game as Game).graphics.GraphicsDevice;
            Game.ScreenManager.ReinitializeCamera(graphicsDevice);
            SpriteLibrary.ClearLibrary();
            (Game.ScreenManager.Game as Game).LoadAllSpriteFonts();
            (Game.ScreenManager.Game as Game).LoadAllEffects();
            (Game.ScreenManager.Game as Game).LoadAllSpritesheets();
            if (!Game.GenericTexture.IsDisposed)
            {
                Game.GenericTexture.Dispose();
            }
            Game.GenericTexture = new Texture2D(graphicsDevice, 1, 1);
            Game.GenericTexture.SetData(new[]
            {
                Color.White
            });
            Game.ScreenManager.ReinitializeContent(null, null);
        }

        public void BeginCapture()
        {
            if (graphicsDevice.IsDisposed)
            {
                RecreateGraphics();
            }
            graphicsDevice.SetRenderTarget(screen);
        }

        public void EndCapture()
        {
            graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!(Game.ScreenManager.CurrentScreen is SkillScreen) &&
                !(Game.ScreenManager.CurrentScreen is LineageScreen) &&
                !(Game.ScreenManager.CurrentScreen is SkillUnlockScreen) && Game.ScreenManager.GetLevelScreen() != null &&
                (Game.PlayerStats.Traits.X == 20f || Game.PlayerStats.Traits.Y == 20f) &&
                Game.PlayerStats.SpecialItem != 8)
            {
                spriteBatch.Draw(screen, area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
                return;
            }
            spriteBatch.Draw(screen, area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}