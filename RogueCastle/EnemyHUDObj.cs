using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyHUDObj : SpriteObj
	{
		private TextObj m_enemyNameText;
		private TextObj m_enemyLevelText;
		private SpriteObj m_enemyHPBar;
		private float m_enemyHPPercent;
		private int m_enemyHPBarLength;
		private int m_blinkNumber = 13;
		private float m_blinkDuration = 0.05f;
		private int m_blinkCounter;
		private float m_blinkDurationCounter;
		private float m_opacity = 1f;
		public EnemyHUDObj() : base("EnemyHUD_Sprite")
		{
			base.ForceDraw = true;
			this.m_enemyNameText = new TextObj(null);
			this.m_enemyNameText.Font = Game.JunicodeFont;
			this.m_enemyNameText.FontSize = 10f;
			this.m_enemyNameText.Align = Types.TextAlign.Right;
			this.m_enemyLevelText = new TextObj(null);
			this.m_enemyLevelText.Font = Game.EnemyLevelFont;
			this.m_enemyHPBar = new SpriteObj("EnemyHPBar_Sprite");
			this.m_enemyHPBar.ForceDraw = true;
			this.m_enemyHPBarLength = this.m_enemyHPBar.SpriteRect.Width;
		}
		public void UpdateEnemyInfo(string enemyName, int enemyLevel, float enemyHPPercent)
		{
			this.m_blinkDurationCounter = 0f;
			this.m_blinkCounter = 0;
			this.m_enemyHPBar.Opacity = 1f;
			this.m_enemyLevelText.Opacity = 1f;
			this.m_enemyNameText.Opacity = 1f;
			base.Opacity = 1f;
			if (enemyName == null)
			{
				enemyName = "Default Enemy";
			}
			if (enemyName.Length > 17)
			{
				enemyName = enemyName.Substring(0, 14) + "..";
			}
			this.m_enemyNameText.Text = enemyName;
			this.m_enemyLevelText.Text = ((int)((float)enemyLevel * 2.75f)).ToString();
			this.m_enemyHPPercent = enemyHPPercent;
			if (enemyHPPercent <= 0f)
			{
				this.m_blinkCounter = this.m_blinkNumber;
				this.m_blinkDurationCounter = this.m_blinkDuration;
				this.m_opacity = 0.5f;
				this.m_enemyHPBar.Opacity = 0.5f;
				this.m_enemyLevelText.Opacity = 0.5f;
				this.m_enemyNameText.Opacity = 0.5f;
				base.Opacity = 0.5f;
			}
		}
		public void Update(GameTime gameTime)
		{
			if (this.m_blinkDurationCounter > 0f)
			{
				this.m_blinkDurationCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (this.m_blinkCounter > 0 && this.m_blinkDurationCounter <= 0f)
			{
				if (this.m_opacity > 0f)
				{
					this.m_opacity = 0f;
				}
				else
				{
					this.m_opacity = 0.5f;
				}
				this.m_enemyHPBar.Opacity = this.m_opacity;
				this.m_enemyLevelText.Opacity = this.m_opacity;
				this.m_enemyNameText.Opacity = this.m_opacity;
				base.Opacity = this.m_opacity;
				this.m_blinkCounter--;
				this.m_blinkDurationCounter = this.m_blinkDuration;
			}
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			this.m_enemyHPBar.Position = new Vector2(base.X + 8f, base.Y + 17f);
			this.m_enemyHPBar.SpriteRect = new Rectangle(this.m_enemyHPBar.SpriteRect.X, this.m_enemyHPBar.SpriteRect.Y, (int)((float)this.m_enemyHPBarLength * this.m_enemyHPPercent), this.m_enemyHPBar.SpriteRect.Height);
			this.m_enemyHPBar.Draw(camera);
			this.m_enemyNameText.Position = new Vector2(base.X + (float)this.Width - 5f, base.Y - 10f);
			this.m_enemyNameText.Draw(camera);
			this.m_enemyLevelText.Position = new Vector2(base.X + 22f, base.Y - 8f);
			this.m_enemyLevelText.Draw(camera);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_enemyNameText.Dispose();
				this.m_enemyNameText = null;
				this.m_enemyLevelText.Dispose();
				this.m_enemyLevelText = null;
				this.m_enemyHPBar.Dispose();
				this.m_enemyHPBar = null;
				base.Dispose();
			}
		}
	}
}
