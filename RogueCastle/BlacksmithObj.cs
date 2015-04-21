using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class BlacksmithObj : ObjContainer
	{
		private const int PART_ASSET1 = 0;
		private const int PART_ASSET2 = 1;
		private const int PART_BODY = 2;
		private const int PART_HEAD = 3;
		private const int PART_HEADTRIM = 4;
		private const int PART_ARM = 5;
		private SpriteObj m_hammerSprite;
		private SpriteObj m_headSprite;
		private float m_hammerAnimCounter;
		public BlacksmithObj() : base("Blacksmith_Character")
		{
			this.m_hammerSprite = (this._objectList[5] as SpriteObj);
			this.m_headSprite = (this._objectList[3] as SpriteObj);
			base.AnimationDelay = 0.1f;
		}
		public void Update(GameTime gameTime)
		{
			if (this.m_hammerAnimCounter <= 0f && !this.m_hammerSprite.IsAnimating)
			{
				this.m_hammerSprite.PlayAnimation(false);
				this.m_hammerAnimCounter = CDGMath.RandomFloat(0.5f, 3f);
				return;
			}
			this.m_hammerAnimCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_hammerSprite = null;
				this.m_headSprite = null;
				base.Dispose();
			}
		}
	}
}
