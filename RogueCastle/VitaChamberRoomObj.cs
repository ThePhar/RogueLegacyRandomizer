using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class VitaChamberRoomObj : BonusRoomObj
	{
		private GameObj m_fountain;
		private SpriteObj m_speechBubble;
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			base.GameObjList.Add(this.m_speechBubble);
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "fountain")
				{
					this.m_fountain = current;
					break;
				}
			}
			(this.m_fountain as SpriteObj).OutlineWidth = 2;
			this.m_speechBubble.X = this.m_fountain.X;
			base.Initialize();
		}
		public override void OnEnter()
		{
			if (base.RoomCompleted)
			{
				this.m_speechBubble.Visible = false;
				this.m_fountain.TextureColor = new Color(100, 100, 100);
			}
			else
			{
				this.m_fountain.TextureColor = Color.White;
			}
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			if (!base.RoomCompleted)
			{
				Rectangle bounds = this.m_fountain.Bounds;
				bounds.X -= 50;
				bounds.Width += 100;
				if (CollisionMath.Intersects(this.Player.Bounds, bounds) && this.Player.IsTouchingGround)
				{
					this.m_speechBubble.Y = this.m_fountain.Y - 150f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
					this.m_speechBubble.Visible = true;
				}
				else
				{
					this.m_speechBubble.Visible = false;
				}
				if (this.m_speechBubble.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
				{
					int num = (int)((float)this.Player.MaxHealth * 0.3f);
					int num2 = (int)(this.Player.MaxMana * 0.3f);
					this.Player.CurrentHealth += num;
					this.Player.CurrentMana += (float)num2;
					Console.WriteLine("Healed");
					SoundManager.PlaySound("Collect_Mana");
					this.Player.AttachedLevel.TextManager.DisplayNumberStringText(num, "hp recovered", Color.LawnGreen, new Vector2(this.Player.X, (float)(this.Player.Bounds.Top - 30)));
					this.Player.AttachedLevel.TextManager.DisplayNumberStringText(num2, "mp recovered", Color.CornflowerBlue, new Vector2(this.Player.X, (float)this.Player.Bounds.Top));
					base.RoomCompleted = true;
					this.m_fountain.TextureColor = new Color(100, 100, 100);
					this.m_speechBubble.Visible = false;
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_fountain = null;
				this.m_speechBubble = null;
				base.Dispose();
			}
		}
	}
}
