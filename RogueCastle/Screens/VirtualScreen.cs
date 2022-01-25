// 
//  Rogue Legacy Randomizer - VirtualScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSystem;

namespace RogueCastle.Screens
{
    internal class VirtualScreen
    {
        public readonly float          VirtualAspectRatio;
        public readonly int            VirtualHeight;
        public readonly int            VirtualWidth;
        private         Rectangle      _area;
        private         bool           _areaIsDirty = true;
        private         GraphicsDevice _graphicsDevice;

        public VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            VirtualAspectRatio = virtualWidth / (float) virtualHeight;
            _graphicsDevice = graphicsDevice;
            RenderTarget = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight);
        }

        public RenderTarget2D RenderTarget { get; private set; }

        public void ReinitializeRTs(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            if (!RenderTarget.IsDisposed)
            {
                RenderTarget.Dispose();
                RenderTarget = null;
            }

            RenderTarget = new RenderTarget2D(graphicsDevice, VirtualWidth, VirtualHeight);
        }

        public void PhysicalResolutionChanged()
        {
            _areaIsDirty = true;
        }

        public void Update()
        {
            if (!_areaIsDirty)
            {
                return;
            }

            _areaIsDirty = false;
            var width = _graphicsDevice.Viewport.Width;
            var height = _graphicsDevice.Viewport.Height;
            var aspectRatio = _graphicsDevice.Viewport.AspectRatio;
            if ((int) (aspectRatio * 10f) == (int) (VirtualAspectRatio * 10f))
            {
                _area = new Rectangle(0, 0, width, height);
                return;
            }

            if (VirtualAspectRatio > aspectRatio)
            {
                var num = width / (float) VirtualWidth;
                var num2 = VirtualWidth * num;
                var num3 = VirtualHeight * num;
                var y = (int) ((height - num3) / 2f);
                _area = new Rectangle(0, y, (int) num2, (int) num3);
                return;
            }

            var num4 = height / (float) VirtualHeight;
            var num5 = VirtualWidth * num4;
            var num6 = VirtualHeight * num4;
            var x = (int) ((width - num5) / 2f);
            _area = new Rectangle(x, 0, (int) num5, (int) num6);
        }

        public void RecreateGraphics()
        {
            Console.WriteLine("GraphicsDevice Virtualization failed");
            var graphicsDevice = (Game.ScreenManager.Game as Game).GraphicsDeviceManager.GraphicsDevice;
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
            if (_graphicsDevice.IsDisposed)
            {
                RecreateGraphics();
            }

            _graphicsDevice.SetRenderTarget(RenderTarget);
        }

        public void EndCapture()
        {
            _graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!(Game.ScreenManager.CurrentScreen is SkillScreen) &&
                !(Game.ScreenManager.CurrentScreen is LineageScreen) &&
                !(Game.ScreenManager.CurrentScreen is SkillUnlockScreen) &&
                Game.ScreenManager.GetLevelScreen() != null &&
                (Game.PlayerStats.Traits.X == 20f || Game.PlayerStats.Traits.Y == 20f) &&
                Game.PlayerStats.SpecialItem != 8)
            {
                spriteBatch.Draw(RenderTarget, _area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically,
                    0f);
                return;
            }

            spriteBatch.Draw(RenderTarget, _area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
