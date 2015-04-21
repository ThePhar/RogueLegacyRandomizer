using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Xml;
namespace RogueCastle
{
	public class HazardObj : PhysicsObj, IDealsDamageObj
	{
		private Texture2D m_texture;
		public override int Width
		{
			get
			{
				return this._width;
			}
		}
		public override int Height
		{
			get
			{
				return this._height;
			}
		}
		public override Rectangle TerrainBounds
		{
			get
			{
				foreach (CollisionBox current in base.CollisionBoxes)
				{
					if (current.Type == 0)
					{
						return current.AbsRect;
					}
				}
				return this.Bounds;
			}
		}
		public int Damage
		{
			get
			{
				PlayerObj player = Game.ScreenManager.Player;
				int num = (int)Math.Round((double)((float)(player.BaseHealth + player.GetEquipmentHealth() + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount), MidpointRounding.AwayFromZero);
				int num2 = (int)((float)num * 0.2f);
				if (num2 < 1)
				{
					num2 = 1;
				}
				return num2;
			}
		}
		public HazardObj(int width, int height) : base("Spikes_Sprite", null)
		{
			base.IsWeighted = false;
			base.IsCollidable = true;
			base.CollisionTypeTag = 10;
			base.DisableHitboxUpdating = true;
		}
		public void InitializeTextures(Camera2D camera)
		{
			Vector2 vector = new Vector2(60f / (float)this._width, 60f / (float)this._height);
			this._width = (int)((float)this._width * vector.X);
			this._height = (int)((float)this._height * vector.Y);
			this.m_texture = base.ConvertToTexture(camera, true, null);
			this._width = (int)Math.Ceiling((double)((float)this._width / vector.X));
			this._height = (int)Math.Ceiling((double)((float)this._height / vector.Y));
			this.Scale = new Vector2((float)this._width / ((float)this._width / 60f * 64f), 1f);
		}
		public void SetWidth(int width)
		{
			this._width = width;
			foreach (CollisionBox current in base.CollisionBoxes)
			{
				if (current.Type == 0)
				{
					current.Width = this._width - 25;
				}
				else
				{
					current.Width = this._width;
				}
			}
		}
		public void SetHeight(int height)
		{
			this._height = height;
		}
		public override void Draw(Camera2D camera)
		{
			camera.Draw(this.m_texture, base.Position, new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / 60f * 64f), this.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new HazardObj(this._width, this._height);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			HazardObj hazardObj = obj as HazardObj;
			hazardObj.SetWidth(this.Width);
			hazardObj.SetHeight(this.Height);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_texture.Dispose();
				this.m_texture = null;
				base.Dispose();
			}
		}
		public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
		{
			base.PopulateFromXMLReader(reader, ci);
			this.SetWidth(this._width);
			this.SetHeight(this._height);
		}
	}
}
