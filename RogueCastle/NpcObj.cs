using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class NpcObj : ObjContainer
	{
		private SpriteObj m_talkIcon;
		public bool CanTalk
		{
			get;
			set;
		}
		public bool IsTouching
		{
			get
			{
				return this.m_talkIcon.Visible;
			}
		}
		public NpcObj(string spriteName) : base(spriteName)
		{
			this.CanTalk = true;
			this.m_talkIcon = new SpriteObj("ExclamationBubble_Sprite");
			this.m_talkIcon.Scale = new Vector2(2f, 2f);
			this.m_talkIcon.Visible = false;
			this.m_talkIcon.OutlineWidth = 2;
			base.OutlineWidth = 2;
		}
		public void Update(GameTime gameTime, PlayerObj player)
		{
			bool flag = false;
			if (this.Flip == SpriteEffects.None && player.X > base.X)
			{
				flag = true;
			}
			if (this.Flip != SpriteEffects.None && player.X < base.X)
			{
				flag = true;
			}
			if (player != null && CollisionMath.Intersects(player.TerrainBounds, new Rectangle(this.Bounds.X - 50, this.Bounds.Y, this.Bounds.Width + 100, this.Bounds.Height)) && flag && player.Flip != this.Flip && this.CanTalk)
			{
				this.m_talkIcon.Visible = true;
			}
			else
			{
				this.m_talkIcon.Visible = false;
			}
			if (this.Flip == SpriteEffects.None)
			{
				this.m_talkIcon.Position = new Vector2((float)this.Bounds.Left - this.m_talkIcon.AnchorX, (float)this.Bounds.Top - this.m_talkIcon.AnchorY + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f);
				return;
			}
			this.m_talkIcon.Position = new Vector2((float)this.Bounds.Right + this.m_talkIcon.AnchorX, (float)this.Bounds.Top - this.m_talkIcon.AnchorY + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f);
		}
		public override void Draw(Camera2D camera)
		{
			if (this.Flip == SpriteEffects.None)
			{
				this.m_talkIcon.Flip = SpriteEffects.FlipHorizontally;
			}
			else
			{
				this.m_talkIcon.Flip = SpriteEffects.None;
			}
			base.Draw(camera);
			this.m_talkIcon.Draw(camera);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new NpcObj(base.SpriteName);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_talkIcon.Dispose();
				this.m_talkIcon = null;
				base.Dispose();
			}
		}
	}
}
