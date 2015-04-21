using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class HoverObj : SpriteObj
	{
		private Vector2 m_startPos;
		public float HoverSpeed = 1f;
		public float Amplitude = 1f;
		public HoverObj(string spriteName) : base(spriteName)
		{
		}
		public void SetStartingPos(Vector2 pos)
		{
			this.m_startPos = pos;
		}
		public override void Draw(Camera2D camera)
		{
			base.Y = this.m_startPos.Y + (float)Math.Sin((double)(Game.TotalGameTime * this.HoverSpeed)) * this.Amplitude;
			base.Draw(camera);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new HoverObj(base.SpriteName);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			HoverObj hoverObj = obj as HoverObj;
			hoverObj.HoverSpeed = this.HoverSpeed;
			hoverObj.Amplitude = this.Amplitude;
			hoverObj.SetStartingPos(this.m_startPos);
		}
	}
}
