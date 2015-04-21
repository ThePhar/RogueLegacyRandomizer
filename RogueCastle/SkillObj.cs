using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class SkillObj : SpriteObj
	{
		private TextObj LevelText;
		private SpriteObj m_coinIcon;
		private int m_currentLevel;
		public string Description
		{
			get;
			set;
		}
		public string InputDescription
		{
			get;
			set;
		}
		public float PerLevelModifier
		{
			get;
			set;
		}
		public int BaseCost
		{
			get;
			set;
		}
		public int Appreciation
		{
			get;
			set;
		}
		public int MaxLevel
		{
			get;
			set;
		}
		public SkillType TraitType
		{
			get;
			set;
		}
		public string IconName
		{
			get;
			set;
		}
		public string UnitOfMeasurement
		{
			get;
			set;
		}
		public byte StatType
		{
			get;
			set;
		}
		public bool DisplayStat
		{
			get;
			set;
		}
		public int CurrentLevel
		{
			get
			{
				return this.m_currentLevel;
			}
			set
			{
				if (value > this.MaxLevel)
				{
					this.m_currentLevel = this.MaxLevel;
					return;
				}
				this.m_currentLevel = value;
			}
		}
		public int TotalCost
		{
			get
			{
				return this.BaseCost + this.CurrentLevel * this.Appreciation + 10 * Game.PlayerStats.CurrentLevel;
			}
		}
		public float ModifierAmount
		{
			get
			{
				return (float)this.CurrentLevel * this.PerLevelModifier;
			}
		}
		public SkillObj(string spriteName) : base(spriteName)
		{
			this.StatType = 0;
			this.DisplayStat = false;
			base.Visible = false;
			base.ForceDraw = true;
			this.LevelText = new TextObj(Game.JunicodeFont);
			this.LevelText.FontSize = 10f;
			this.LevelText.Align = Types.TextAlign.Centre;
			this.LevelText.OutlineWidth = 2;
			this.InputDescription = "";
			base.OutlineWidth = 2;
			this.m_coinIcon = new SpriteObj("UpgradeIcon_Sprite");
		}
		public override void Draw(Camera2D camera)
		{
			if (base.Opacity > 0f)
			{
				float opacity = base.Opacity;
				base.TextureColor = Color.Black;
				base.Opacity = 0.5f;
				base.X += 8f;
				base.Y += 8f;
				base.Draw(camera);
				base.X -= 8f;
				base.Y -= 8f;
				base.TextureColor = Color.White;
				base.Opacity = opacity;
			}
			base.Draw(camera);
			this.LevelText.Position = new Vector2(base.X, (float)(this.Bounds.Bottom - this.LevelText.Height / 2));
			this.LevelText.Text = this.CurrentLevel + "/" + this.MaxLevel;
			this.LevelText.Opacity = base.Opacity;
			if (this.CurrentLevel >= this.MaxLevel)
			{
				this.LevelText.TextureColor = Color.Yellow;
				this.LevelText.Text = "Max";
			}
			else
			{
				this.LevelText.TextureColor = Color.White;
			}
			this.LevelText.Draw(camera);
			if (Game.PlayerStats.Gold >= this.TotalCost && this.CurrentLevel < this.MaxLevel)
			{
				this.m_coinIcon.Opacity = base.Opacity;
				this.m_coinIcon.Position = new Vector2(base.X + 18f, base.Y - 40f);
				this.m_coinIcon.Draw(camera);
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new SkillObj(this._spriteName);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			SkillObj skillObj = obj as SkillObj;
			skillObj.Description = this.Description;
			skillObj.PerLevelModifier = this.PerLevelModifier;
			skillObj.BaseCost = this.BaseCost;
			skillObj.Appreciation = this.Appreciation;
			skillObj.MaxLevel = this.MaxLevel;
			skillObj.CurrentLevel = this.CurrentLevel;
			skillObj.TraitType = this.TraitType;
			skillObj.InputDescription = this.InputDescription;
			skillObj.UnitOfMeasurement = this.UnitOfMeasurement;
			skillObj.StatType = this.StatType;
			skillObj.DisplayStat = this.DisplayStat;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.LevelText.Dispose();
				this.LevelText = null;
				this.m_coinIcon.Dispose();
				this.m_coinIcon = null;
				base.Dispose();
			}
		}
	}
}
