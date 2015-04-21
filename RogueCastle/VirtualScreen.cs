using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteSystem;
using System;
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
			get
			{
				return this.screen;
			}
		}
		public VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice)
		{
			this.VirtualWidth = virtualWidth;
			this.VirtualHeight = virtualHeight;
			this.VirtualAspectRatio = (float)virtualWidth / (float)virtualHeight;
			this.graphicsDevice = graphicsDevice;
			this.screen = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight);
		}
		public void ReinitializeRTs(GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			if (!this.screen.IsDisposed)
			{
				this.screen.Dispose();
				this.screen = null;
			}
			this.screen = new RenderTarget2D(graphicsDevice, this.VirtualWidth, this.VirtualHeight);
		}
		public void PhysicalResolutionChanged()
		{
			this.areaIsDirty = true;
		}
		public void Update()
		{
			if (!this.areaIsDirty)
			{
				return;
			}
			this.areaIsDirty = false;
			int width = this.graphicsDevice.Viewport.Width;
			int height = this.graphicsDevice.Viewport.Height;
			float aspectRatio = this.graphicsDevice.Viewport.AspectRatio;
			if ((int)(aspectRatio * 10f) == (int)(this.VirtualAspectRatio * 10f))
			{
				this.area = new Rectangle(0, 0, width, height);
				return;
			}
			if (this.VirtualAspectRatio > aspectRatio)
			{
				float num = (float)width / (float)this.VirtualWidth;
				float num2 = (float)this.VirtualWidth * num;
				float num3 = (float)this.VirtualHeight * num;
				int y = (int)(((float)height - num3) / 2f);
				this.area = new Rectangle(0, y, (int)num2, (int)num3);
				return;
			}
			float num4 = (float)height / (float)this.VirtualHeight;
			float num5 = (float)this.VirtualWidth * num4;
			float num6 = (float)this.VirtualHeight * num4;
			int x = (int)(((float)width - num5) / 2f);
			this.area = new Rectangle(x, 0, (int)num5, (int)num6);
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
			Game.GenericTexture.SetData<Color>(new Color[]
			{
				Color.White
			});
			Game.ScreenManager.ReinitializeContent(null, null);
		}
		public void BeginCapture()
		{
			if (this.graphicsDevice.IsDisposed)
			{
				this.RecreateGraphics();
			}
			this.graphicsDevice.SetRenderTarget(this.screen);
		}
		public void EndCapture()
		{
			this.graphicsDevice.SetRenderTarget(null);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!(Game.ScreenManager.CurrentScreen is SkillScreen) && !(Game.ScreenManager.CurrentScreen is LineageScreen) && !(Game.ScreenManager.CurrentScreen is SkillUnlockScreen) && Game.ScreenManager.GetLevelScreen() != null && (Game.PlayerStats.Traits.X == 20f || Game.PlayerStats.Traits.Y == 20f) && Game.PlayerStats.SpecialItem != 8)
			{
				spriteBatch.Draw(this.screen, this.area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
				return;
			}
			spriteBatch.Draw(this.screen, this.area, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}
