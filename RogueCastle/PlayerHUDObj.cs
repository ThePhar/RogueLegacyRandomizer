using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class PlayerHUDObj : SpriteObj
	{
		private int m_maxBarLength = 360;
		private TextObj m_playerLevelText;
		private SpriteObj m_coin;
		private TextObj m_goldText;
		private SpriteObj m_hpBar;
		private TextObj m_hpText;
		private SpriteObj m_mpBar;
		private TextObj m_mpText;
		private SpriteObj[] m_abilitiesSpriteArray;
		private ObjContainer m_hpBarContainer;
		private ObjContainer m_mpBarContainer;
		private SpriteObj m_specialItemIcon;
		private SpriteObj m_spellIcon;
		private TextObj m_spellCost;
		private SpriteObj m_iconHolder1;
		private SpriteObj m_iconHolder2;
		public bool ShowBarsOnly
		{
			get;
			set;
		}
		public PlayerHUDObj() : base("PlayerHUDLvlText_Sprite")
		{
			base.ForceDraw = true;
			this.m_playerLevelText = new TextObj(null);
			this.m_playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
			this.m_playerLevelText.Font = Game.PlayerLevelFont;
			this.m_coin = new SpriteObj("PlayerUICoin_Sprite");
			this.m_coin.ForceDraw = true;
			this.m_goldText = new TextObj(null);
			this.m_goldText.Text = "0";
			this.m_goldText.Font = Game.GoldFont;
			this.m_goldText.FontSize = 25f;
			this.m_hpBar = new SpriteObj("HPBar_Sprite");
			this.m_hpBar.ForceDraw = true;
			this.m_mpBar = new SpriteObj("MPBar_Sprite");
			this.m_mpBar.ForceDraw = true;
			this.m_hpText = new TextObj(Game.JunicodeFont);
			this.m_hpText.FontSize = 7f;
			this.m_hpText.DropShadow = new Vector2(1f, 1f);
			this.m_hpText.ForceDraw = true;
			this.m_mpText = new TextObj(Game.JunicodeFont);
			this.m_mpText.FontSize = 7f;
			this.m_mpText.DropShadow = new Vector2(1f, 1f);
			this.m_mpText.ForceDraw = true;
			this.m_abilitiesSpriteArray = new SpriteObj[5];
			Vector2 position = new Vector2(130f, 690f);
			int num = 35;
			for (int i = 0; i < this.m_abilitiesSpriteArray.Length; i++)
			{
				this.m_abilitiesSpriteArray[i] = new SpriteObj("Blank_Sprite");
				this.m_abilitiesSpriteArray[i].ForceDraw = true;
				this.m_abilitiesSpriteArray[i].Position = position;
				this.m_abilitiesSpriteArray[i].Scale = new Vector2(0.5f, 0.5f);
				position.X += (float)num;
			}
			this.m_hpBarContainer = new ObjContainer("PlayerHUDHPBar_Character");
			this.m_hpBarContainer.ForceDraw = true;
			this.m_mpBarContainer = new ObjContainer("PlayerHUDMPBar_Character");
			this.m_mpBarContainer.ForceDraw = true;
			this.m_specialItemIcon = new SpriteObj("Blank_Sprite");
			this.m_specialItemIcon.ForceDraw = true;
			this.m_specialItemIcon.OutlineWidth = 1;
			this.m_specialItemIcon.Scale = new Vector2(1.7f, 1.7f);
			this.m_specialItemIcon.Visible = false;
			this.m_spellIcon = new SpriteObj(SpellType.Icon(0));
			this.m_spellIcon.ForceDraw = true;
			this.m_spellIcon.OutlineWidth = 1;
			this.m_spellIcon.Visible = false;
			this.m_iconHolder1 = new SpriteObj("BlacksmithUI_IconBG_Sprite");
			this.m_iconHolder1.ForceDraw = true;
			this.m_iconHolder1.Opacity = 0.5f;
			this.m_iconHolder1.Scale = new Vector2(0.8f, 0.8f);
			this.m_iconHolder2 = (this.m_iconHolder1.Clone() as SpriteObj);
			this.m_spellCost = new TextObj(Game.JunicodeFont);
			this.m_spellCost.Align = Types.TextAlign.Centre;
			this.m_spellCost.ForceDraw = true;
			this.m_spellCost.OutlineWidth = 2;
			this.m_spellCost.FontSize = 8f;
			this.m_spellCost.Visible = false;
			this.UpdateSpecialItemIcon();
			this.UpdateSpellIcon();
		}
		public void SetPosition(Vector2 position)
		{
			SpriteObj spriteObj;
			SpriteObj spriteObj2;
			ObjContainer objContainer;
			ObjContainer objContainer2;
			if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f)
			{
				spriteObj = this.m_hpBar;
				spriteObj2 = this.m_mpBar;
				objContainer = this.m_hpBarContainer;
				objContainer2 = this.m_mpBarContainer;
			}
			else
			{
				spriteObj = this.m_mpBar;
				spriteObj2 = this.m_hpBar;
				objContainer = this.m_mpBarContainer;
				objContainer2 = this.m_hpBarContainer;
			}
			base.Position = position;
			spriteObj.Position = new Vector2(base.X + 7f, base.Y + 60f);
			spriteObj2.Position = new Vector2(base.X + 8f, base.Y + 29f);
			this.m_playerLevelText.Position = new Vector2(base.X + 30f, base.Y - 20f);
			if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f)
			{
				this.m_mpText.Position = new Vector2(base.X + 5f, base.Y + 19f);
				this.m_mpText.X += 8f;
				this.m_hpText.Position = this.m_mpText.Position;
				this.m_hpText.Y += 28f;
			}
			else
			{
				this.m_hpText.Position = new Vector2(base.X + 5f, base.Y + 19f);
				this.m_hpText.X += 8f;
				this.m_hpText.Y += 5f;
				this.m_mpText.Position = this.m_hpText.Position;
				this.m_mpText.Y += 30f;
			}
			objContainer2.Position = new Vector2(base.X, base.Y + 17f);
			if (spriteObj2 == this.m_hpBar)
			{
				spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 7f);
			}
			else
			{
				spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 6f);
			}
			objContainer.Position = new Vector2(base.X, (float)objContainer2.Bounds.Bottom);
			if (spriteObj == this.m_mpBar)
			{
				spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 6f);
			}
			else
			{
				spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 7f);
			}
			this.m_coin.Position = new Vector2(base.X, (float)(objContainer.Bounds.Bottom + 2));
			this.m_goldText.Position = new Vector2(this.m_coin.X + 28f, this.m_coin.Y - 2f);
			this.m_iconHolder1.Position = new Vector2(this.m_coin.X + 25f, this.m_coin.Y + 60f);
			this.m_iconHolder2.Position = new Vector2(this.m_iconHolder1.X + 55f, this.m_iconHolder1.Y);
			this.m_spellIcon.Position = this.m_iconHolder1.Position;
			this.m_specialItemIcon.Position = this.m_iconHolder2.Position;
			this.m_spellCost.Position = new Vector2(this.m_spellIcon.X, (float)(this.m_spellIcon.Bounds.Bottom + 10));
		}
		public void Update(PlayerObj player)
		{
			int num = Game.PlayerStats.CurrentLevel;
			if (num < 0)
			{
				num = 0;
			}
			this.m_playerLevelText.Text = num.ToString();
			int num2 = Game.PlayerStats.Gold;
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.m_goldText.Text = num2.ToString();
			this.m_hpText.Text = player.CurrentHealth + "/" + player.MaxHealth;
			this.m_mpText.Text = player.CurrentMana + "/" + player.MaxMana;
			this.UpdatePlayerHP(player);
			this.UpdatePlayerMP(player);
		}
		private void UpdatePlayerHP(PlayerObj player)
		{
			int num = player.MaxHealth - player.BaseHealth;
			float num2 = (float)player.CurrentHealth / (float)player.MaxHealth;
			int num3 = (int)(88f + (float)num / 5f);
			if (num3 > this.m_maxBarLength)
			{
				num3 = this.m_maxBarLength;
			}
			float scaleX = (float)(num3 - 28 - 28) / 32f;
			this.m_hpBarContainer.GetChildAt(1).ScaleX = scaleX;
			this.m_hpBarContainer.GetChildAt(2).X = (float)this.m_hpBarContainer.GetChildAt(1).Bounds.Right;
			this.m_hpBarContainer.CalculateBounds();
			this.m_hpBar.ScaleX = 1f;
			this.m_hpBar.ScaleX = (float)(this.m_hpBarContainer.Width - 8) / (float)this.m_hpBar.Width * num2;
		}
		private void UpdatePlayerMP(PlayerObj player)
		{
			int num = (int)(player.MaxMana - player.BaseMana);
			float num2 = player.CurrentMana / player.MaxMana;
			int num3 = (int)(88f + (float)num / 5f);
			if (num3 > this.m_maxBarLength)
			{
				num3 = this.m_maxBarLength;
			}
			float scaleX = (float)(num3 - 28 - 28) / 32f;
			this.m_mpBarContainer.GetChildAt(1).ScaleX = scaleX;
			this.m_mpBarContainer.GetChildAt(2).X = (float)this.m_mpBarContainer.GetChildAt(1).Bounds.Right;
			this.m_mpBarContainer.CalculateBounds();
			this.m_mpBar.ScaleX = 1f;
			this.m_mpBar.ScaleX = (float)(this.m_mpBarContainer.Width - 8) / (float)this.m_mpBar.Width * num2;
		}
		public void UpdatePlayerLevel()
		{
			this.m_playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
		}
		public void UpdateAbilityIcons()
		{
			SpriteObj[] abilitiesSpriteArray = this.m_abilitiesSpriteArray;
			for (int i = 0; i < abilitiesSpriteArray.Length; i++)
			{
				SpriteObj spriteObj = abilitiesSpriteArray[i];
				spriteObj.ChangeSprite("Blank_Sprite");
			}
			int num = 0;
			sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
			for (int j = 0; j < getEquippedRuneArray.Length; j++)
			{
				sbyte b = getEquippedRuneArray[j];
				if (b != -1)
				{
					this.m_abilitiesSpriteArray[num].ChangeSprite(EquipmentAbilityType.Icon((int)b));
					num++;
				}
			}
		}
		public void UpdateSpecialItemIcon()
		{
			this.m_specialItemIcon.Visible = false;
			this.m_iconHolder2.Opacity = 0.5f;
			if (Game.PlayerStats.SpecialItem != 0)
			{
				this.m_specialItemIcon.Visible = true;
				this.m_specialItemIcon.ChangeSprite(SpecialItemType.SpriteName(Game.PlayerStats.SpecialItem));
				this.m_iconHolder2.Opacity = 1f;
			}
		}
		public void UpdateSpellIcon()
		{
			this.m_spellIcon.Visible = false;
			this.m_iconHolder1.Opacity = 0.5f;
			this.m_spellCost.Visible = false;
			if (Game.PlayerStats.Spell != 0)
			{
				this.m_spellIcon.ChangeSprite(SpellType.Icon(Game.PlayerStats.Spell));
				this.m_spellIcon.Visible = true;
				this.m_iconHolder1.Opacity = 1f;
				this.m_spellCost.Text = (int)((float)SpellEV.GetManaCost(Game.PlayerStats.Spell) * (1f - SkillSystem.GetSkill(SkillType.Mana_Cost_Down).ModifierAmount)) + " mp";
				this.m_spellCost.Visible = true;
			}
		}
		public override void Draw(Camera2D camera)
		{
			if (base.Visible)
			{
				if (!this.ShowBarsOnly)
				{
					base.Draw(camera);
					this.m_coin.Draw(camera);
					this.m_playerLevelText.Draw(camera);
					this.m_goldText.Draw(camera);
					camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
					SpriteObj[] abilitiesSpriteArray = this.m_abilitiesSpriteArray;
					for (int i = 0; i < abilitiesSpriteArray.Length; i++)
					{
						SpriteObj spriteObj = abilitiesSpriteArray[i];
						spriteObj.Draw(camera);
					}
					this.m_iconHolder1.Draw(camera);
					this.m_iconHolder2.Draw(camera);
					this.m_spellIcon.Draw(camera);
					this.m_specialItemIcon.Draw(camera);
					camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
					this.m_spellCost.Draw(camera);
				}
				this.m_mpBar.Draw(camera);
				this.m_mpText.Draw(camera);
				if (Game.PlayerStats.Traits.X != 30f && Game.PlayerStats.Traits.Y != 30f)
				{
					this.m_hpBar.Draw(camera);
					this.m_hpText.Draw(camera);
				}
				this.m_mpBarContainer.Draw(camera);
				this.m_hpBarContainer.Draw(camera);
			}
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				SpriteObj[] abilitiesSpriteArray = this.m_abilitiesSpriteArray;
				for (int i = 0; i < abilitiesSpriteArray.Length; i++)
				{
					SpriteObj spriteObj = abilitiesSpriteArray[i];
					spriteObj.Dispose();
				}
				Array.Clear(this.m_abilitiesSpriteArray, 0, this.m_abilitiesSpriteArray.Length);
				this.m_abilitiesSpriteArray = null;
				this.m_coin.Dispose();
				this.m_coin = null;
				this.m_mpBar.Dispose();
				this.m_mpBar = null;
				this.m_hpBar.Dispose();
				this.m_hpBar = null;
				this.m_playerLevelText.Dispose();
				this.m_playerLevelText = null;
				this.m_goldText.Dispose();
				this.m_goldText = null;
				this.m_hpText.Dispose();
				this.m_hpText = null;
				this.m_mpText.Dispose();
				this.m_mpText = null;
				this.m_hpBarContainer.Dispose();
				this.m_hpBarContainer = null;
				this.m_mpBarContainer.Dispose();
				this.m_mpBarContainer = null;
				this.m_specialItemIcon.Dispose();
				this.m_specialItemIcon = null;
				this.m_spellIcon.Dispose();
				this.m_spellIcon = null;
				this.m_spellCost.Dispose();
				this.m_spellCost = null;
				this.m_iconHolder1.Dispose();
				this.m_iconHolder1 = null;
				this.m_iconHolder2.Dispose();
				this.m_iconHolder2 = null;
				base.Dispose();
			}
		}
	}
}
