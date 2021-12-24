// 
// RogueLegacyArchipelago - PlayerHUDObj.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.TypeDefinitions;

namespace RogueCastle
{
    public class PlayerHUDObj : SpriteObj
    {
        private readonly int m_maxBarLength = 360;
        private SpriteObj[] m_abilitiesSpriteArray;
        private SpriteObj m_coin;
        private TextObj m_goldText;
        private SpriteObj m_hpBar;
        private ObjContainer m_hpBarContainer;
        private TextObj m_hpText;
        private SpriteObj m_iconHolder1;
        private SpriteObj m_iconHolder2;
        private SpriteObj m_mpBar;
        private ObjContainer m_mpBarContainer;
        private TextObj m_mpText;
        private TextObj m_playerLevelText;
        private SpriteObj m_specialItemIcon;
        private TextObj m_spellCost;
        private SpriteObj m_spellIcon;

        public PlayerHUDObj() : base("PlayerHUDLvlText_Sprite")
        {
            ForceDraw = true;
            m_playerLevelText = new TextObj();
            m_playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
            m_playerLevelText.Font = Game.PlayerLevelFont;
            m_coin = new SpriteObj("PlayerUICoin_Sprite");
            m_coin.ForceDraw = true;
            m_goldText = new TextObj();
            m_goldText.Text = "0";
            m_goldText.Font = Game.GoldFont;
            m_goldText.FontSize = 25f;
            m_hpBar = new SpriteObj("HPBar_Sprite");
            m_hpBar.ForceDraw = true;
            m_mpBar = new SpriteObj("MPBar_Sprite");
            m_mpBar.ForceDraw = true;
            m_hpText = new TextObj(Game.JunicodeFont);
            m_hpText.FontSize = 7f;
            m_hpText.DropShadow = new Vector2(1f, 1f);
            m_hpText.ForceDraw = true;
            m_mpText = new TextObj(Game.JunicodeFont);
            m_mpText.FontSize = 7f;
            m_mpText.DropShadow = new Vector2(1f, 1f);
            m_mpText.ForceDraw = true;
            m_abilitiesSpriteArray = new SpriteObj[5];
            var position = new Vector2(130f, 690f);
            var num = 35;
            for (var i = 0; i < m_abilitiesSpriteArray.Length; i++)
            {
                m_abilitiesSpriteArray[i] = new SpriteObj("Blank_Sprite");
                m_abilitiesSpriteArray[i].ForceDraw = true;
                m_abilitiesSpriteArray[i].Position = position;
                m_abilitiesSpriteArray[i].Scale = new Vector2(0.5f, 0.5f);
                position.X += num;
            }
            m_hpBarContainer = new ObjContainer("PlayerHUDHPBar_Character");
            m_hpBarContainer.ForceDraw = true;
            m_mpBarContainer = new ObjContainer("PlayerHUDMPBar_Character");
            m_mpBarContainer.ForceDraw = true;
            m_specialItemIcon = new SpriteObj("Blank_Sprite");
            m_specialItemIcon.ForceDraw = true;
            m_specialItemIcon.OutlineWidth = 1;
            m_specialItemIcon.Scale = new Vector2(1.7f, 1.7f);
            m_specialItemIcon.Visible = false;
            m_spellIcon = new SpriteObj(SpellType.Icon(0));
            m_spellIcon.ForceDraw = true;
            m_spellIcon.OutlineWidth = 1;
            m_spellIcon.Visible = false;
            m_iconHolder1 = new SpriteObj("BlacksmithUI_IconBG_Sprite");
            m_iconHolder1.ForceDraw = true;
            m_iconHolder1.Opacity = 0.5f;
            m_iconHolder1.Scale = new Vector2(0.8f, 0.8f);
            m_iconHolder2 = (m_iconHolder1.Clone() as SpriteObj);
            m_spellCost = new TextObj(Game.JunicodeFont);
            m_spellCost.Align = Types.TextAlign.Centre;
            m_spellCost.ForceDraw = true;
            m_spellCost.OutlineWidth = 2;
            m_spellCost.FontSize = 8f;
            m_spellCost.Visible = false;
            UpdateSpecialItemIcon();
            UpdateSpellIcon();
        }

        public bool ShowBarsOnly { get; set; }

        public void SetPosition(Vector2 position)
        {
            SpriteObj spriteObj;
            SpriteObj spriteObj2;
            ObjContainer objContainer;
            ObjContainer objContainer2;
            if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f)
            {
                spriteObj = m_hpBar;
                spriteObj2 = m_mpBar;
                objContainer = m_hpBarContainer;
                objContainer2 = m_mpBarContainer;
            }
            else
            {
                spriteObj = m_mpBar;
                spriteObj2 = m_hpBar;
                objContainer = m_mpBarContainer;
                objContainer2 = m_hpBarContainer;
            }
            Position = position;
            spriteObj.Position = new Vector2(X + 7f, Y + 60f);
            spriteObj2.Position = new Vector2(X + 8f, Y + 29f);
            m_playerLevelText.Position = new Vector2(X + 30f, Y - 20f);
            if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f)
            {
                m_mpText.Position = new Vector2(X + 5f, Y + 19f);
                m_mpText.X += 8f;
                m_hpText.Position = m_mpText.Position;
                m_hpText.Y += 28f;
            }
            else
            {
                m_hpText.Position = new Vector2(X + 5f, Y + 19f);
                m_hpText.X += 8f;
                m_hpText.Y += 5f;
                m_mpText.Position = m_hpText.Position;
                m_mpText.Y += 30f;
            }
            objContainer2.Position = new Vector2(X, Y + 17f);
            if (spriteObj2 == m_hpBar)
            {
                spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 7f);
            }
            else
            {
                spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 6f);
            }
            objContainer.Position = new Vector2(X, objContainer2.Bounds.Bottom);
            if (spriteObj == m_mpBar)
            {
                spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 6f);
            }
            else
            {
                spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 7f);
            }
            m_coin.Position = new Vector2(X, objContainer.Bounds.Bottom + 2);
            m_goldText.Position = new Vector2(m_coin.X + 28f, m_coin.Y - 2f);
            m_iconHolder1.Position = new Vector2(m_coin.X + 25f, m_coin.Y + 60f);
            m_iconHolder2.Position = new Vector2(m_iconHolder1.X + 55f, m_iconHolder1.Y);
            m_spellIcon.Position = m_iconHolder1.Position;
            m_specialItemIcon.Position = m_iconHolder2.Position;
            m_spellCost.Position = new Vector2(m_spellIcon.X, m_spellIcon.Bounds.Bottom + 10);
        }

        public void Update(PlayerObj player)
        {
            var num = Game.PlayerStats.CurrentLevel;
            if (num < 0)
            {
                num = 0;
            }
            m_playerLevelText.Text = num.ToString();
            var num2 = Game.PlayerStats.Gold;
            if (num2 < 0)
            {
                num2 = 0;
            }
            m_goldText.Text = num2.ToString();
            m_hpText.Text = player.CurrentHealth + "/" + player.MaxHealth;
            m_mpText.Text = player.CurrentMana + "/" + player.MaxMana;
            UpdatePlayerHP(player);
            UpdatePlayerMP(player);
        }

        private void UpdatePlayerHP(PlayerObj player)
        {
            var num = player.MaxHealth - player.BaseHealth;
            var num2 = player.CurrentHealth/(float) player.MaxHealth;
            var num3 = (int) (88f + num/5f);
            if (num3 > m_maxBarLength)
            {
                num3 = m_maxBarLength;
            }
            var scaleX = (num3 - 28 - 28)/32f;
            m_hpBarContainer.GetChildAt(1).ScaleX = scaleX;
            m_hpBarContainer.GetChildAt(2).X = m_hpBarContainer.GetChildAt(1).Bounds.Right;
            m_hpBarContainer.CalculateBounds();
            m_hpBar.ScaleX = 1f;
            m_hpBar.ScaleX = (m_hpBarContainer.Width - 8)/(float) m_hpBar.Width*num2;
        }

        private void UpdatePlayerMP(PlayerObj player)
        {
            var num = (int) (player.MaxMana - player.BaseMana);
            var num2 = player.CurrentMana/player.MaxMana;
            var num3 = (int) (88f + num/5f);
            if (num3 > m_maxBarLength)
            {
                num3 = m_maxBarLength;
            }
            var scaleX = (num3 - 28 - 28)/32f;
            m_mpBarContainer.GetChildAt(1).ScaleX = scaleX;
            m_mpBarContainer.GetChildAt(2).X = m_mpBarContainer.GetChildAt(1).Bounds.Right;
            m_mpBarContainer.CalculateBounds();
            m_mpBar.ScaleX = 1f;
            m_mpBar.ScaleX = (m_mpBarContainer.Width - 8)/(float) m_mpBar.Width*num2;
        }

        public void UpdatePlayerLevel()
        {
            m_playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
        }

        public void UpdateAbilityIcons()
        {
            var abilitiesSpriteArray = m_abilitiesSpriteArray;
            for (var i = 0; i < abilitiesSpriteArray.Length; i++)
            {
                var spriteObj = abilitiesSpriteArray[i];
                spriteObj.ChangeSprite("Blank_Sprite");
            }
            var num = 0;
            var getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
            for (var j = 0; j < getEquippedRuneArray.Length; j++)
            {
                var b = getEquippedRuneArray[j];
                if (b != -1)
                {
                    m_abilitiesSpriteArray[num].ChangeSprite(EquipmentAbilityType.Icon(b));
                    num++;
                }
            }
        }

        public void UpdateSpecialItemIcon()
        {
            m_specialItemIcon.Visible = false;
            m_iconHolder2.Opacity = 0.5f;
            if (Game.PlayerStats.SpecialItem != 0)
            {
                m_specialItemIcon.Visible = true;
                m_specialItemIcon.ChangeSprite(SpecialItemType.SpriteName(Game.PlayerStats.SpecialItem));
                m_iconHolder2.Opacity = 1f;
            }
        }

        public void UpdateSpellIcon()
        {
            m_spellIcon.Visible = false;
            m_iconHolder1.Opacity = 0.5f;
            m_spellCost.Visible = false;
            if (Game.PlayerStats.Spell != 0)
            {
                m_spellIcon.ChangeSprite(SpellType.Icon(Game.PlayerStats.Spell));
                m_spellIcon.Visible = true;
                m_iconHolder1.Opacity = 1f;
                m_spellCost.Text =
                    (int)
                        (SpellEV.GetManaCost(Game.PlayerStats.Spell)*
                         (1f - SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount)) + " mp";
                m_spellCost.Visible = true;
            }
        }

        public override void Draw(Camera2D camera)
        {
            if (Visible)
            {
                if (!ShowBarsOnly)
                {
                    base.Draw(camera);
                    m_coin.Draw(camera);
                    m_playerLevelText.Draw(camera);
                    m_goldText.Draw(camera);
                    camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                    var abilitiesSpriteArray = m_abilitiesSpriteArray;
                    for (var i = 0; i < abilitiesSpriteArray.Length; i++)
                    {
                        var spriteObj = abilitiesSpriteArray[i];
                        spriteObj.Draw(camera);
                    }
                    m_iconHolder1.Draw(camera);
                    m_iconHolder2.Draw(camera);
                    m_spellIcon.Draw(camera);
                    m_specialItemIcon.Draw(camera);
                    camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    m_spellCost.Draw(camera);
                }
                m_mpBar.Draw(camera);
                m_mpText.Draw(camera);
                if (Game.PlayerStats.Traits.X != 30f && Game.PlayerStats.Traits.Y != 30f)
                {
                    m_hpBar.Draw(camera);
                    m_hpText.Draw(camera);
                }
                m_mpBarContainer.Draw(camera);
                m_hpBarContainer.Draw(camera);
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                var abilitiesSpriteArray = m_abilitiesSpriteArray;
                for (var i = 0; i < abilitiesSpriteArray.Length; i++)
                {
                    var spriteObj = abilitiesSpriteArray[i];
                    spriteObj.Dispose();
                }
                Array.Clear(m_abilitiesSpriteArray, 0, m_abilitiesSpriteArray.Length);
                m_abilitiesSpriteArray = null;
                m_coin.Dispose();
                m_coin = null;
                m_mpBar.Dispose();
                m_mpBar = null;
                m_hpBar.Dispose();
                m_hpBar = null;
                m_playerLevelText.Dispose();
                m_playerLevelText = null;
                m_goldText.Dispose();
                m_goldText = null;
                m_hpText.Dispose();
                m_hpText = null;
                m_mpText.Dispose();
                m_mpText = null;
                m_hpBarContainer.Dispose();
                m_hpBarContainer = null;
                m_mpBarContainer.Dispose();
                m_mpBarContainer = null;
                m_specialItemIcon.Dispose();
                m_specialItemIcon = null;
                m_spellIcon.Dispose();
                m_spellIcon = null;
                m_spellCost.Dispose();
                m_spellCost = null;
                m_iconHolder1.Dispose();
                m_iconHolder1 = null;
                m_iconHolder2.Dispose();
                m_iconHolder2 = null;
                base.Dispose();
            }
        }
    }
}
