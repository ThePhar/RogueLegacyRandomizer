// Rogue Legacy Randomizer - PlayerHUDObj.cs
// Last Modified 2022-12-01
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;

namespace RogueLegacy.GameObjects.HUD;

public class PlayerHUDObj : SpriteObj
{
    private readonly int          _maxBarLength = 360;
    private          SpriteObj[]  _abilitiesSpriteArray;
    private          SpriteObj    _coin;
    private          TextObj      _goldText;
    private          SpriteObj    _hpBar;
    private          ObjContainer _hpBarContainer;
    private          TextObj      _hpText;
    private          SpriteObj    _iconHolder1;
    private          SpriteObj    _iconHolder2;
    private          SpriteObj    _mpBar;
    private          ObjContainer _mpBarContainer;
    private          TextObj      _mpText;
    private          TextObj      _playerLevelText;
    private          SpriteObj    _specialItemIcon;
    private          TextObj      _spellCost;
    private          SpriteObj    _spellIcon;

    public PlayerHUDObj() : base("PlayerHUDLvlText_Sprite")
    {
        ForceDraw = true;
        _playerLevelText = new TextObj();
        _playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
        _playerLevelText.Font = Game.PlayerLevelFont;
        _coin = new SpriteObj("PlayerUICoin_Sprite");
        _coin.ForceDraw = true;
        _goldText = new TextObj();
        _goldText.Text = "0";
        _goldText.Font = Game.GoldFont;
        _goldText.FontSize = 25f;
        _hpBar = new SpriteObj("HPBar_Sprite");
        _hpBar.ForceDraw = true;
        _mpBar = new SpriteObj("MPBar_Sprite");
        _mpBar.ForceDraw = true;
        _hpText = new TextObj(Game.JunicodeFont);
        _hpText.FontSize = 7f;
        _hpText.DropShadow = new Vector2(1f, 1f);
        _hpText.ForceDraw = true;
        _mpText = new TextObj(Game.JunicodeFont);
        _mpText.FontSize = 7f;
        _mpText.DropShadow = new Vector2(1f, 1f);
        _mpText.ForceDraw = true;
        _abilitiesSpriteArray = new SpriteObj[5];
        var position = new Vector2(130f, 690f);
        var num = 35;
        for (var i = 0; i < _abilitiesSpriteArray.Length; i++)
        {
            _abilitiesSpriteArray[i] = new SpriteObj("Blank_Sprite");
            _abilitiesSpriteArray[i].ForceDraw = true;
            _abilitiesSpriteArray[i].Position = position;
            _abilitiesSpriteArray[i].Scale = new Vector2(0.5f, 0.5f);
            position.X += num;
        }

        _hpBarContainer = new ObjContainer("PlayerHUDHPBar_Character");
        _hpBarContainer.ForceDraw = true;
        _mpBarContainer = new ObjContainer("PlayerHUDMPBar_Character");
        _mpBarContainer.ForceDraw = true;
        _specialItemIcon = new SpriteObj("Blank_Sprite");
        _specialItemIcon.ForceDraw = true;
        _specialItemIcon.OutlineWidth = 1;
        _specialItemIcon.Scale = new Vector2(1.7f, 1.7f);
        _specialItemIcon.Visible = false;
        _spellIcon = new SpriteObj(((SpellType) 0).Icon());
        _spellIcon.ForceDraw = true;
        _spellIcon.OutlineWidth = 1;
        _spellIcon.Visible = false;
        _iconHolder1 = new SpriteObj("BlacksmithUI_IconBG_Sprite");
        _iconHolder1.ForceDraw = true;
        _iconHolder1.Opacity = 0.5f;
        _iconHolder1.Scale = new Vector2(0.8f, 0.8f);
        _iconHolder2 = _iconHolder1.Clone() as SpriteObj;
        _spellCost = new TextObj(Game.JunicodeFont);
        _spellCost.Align = Types.TextAlign.Centre;
        _spellCost.ForceDraw = true;
        _spellCost.OutlineWidth = 2;
        _spellCost.FontSize = 8f;
        _spellCost.Visible = false;
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
            spriteObj = _hpBar;
            spriteObj2 = _mpBar;
            objContainer = _hpBarContainer;
            objContainer2 = _mpBarContainer;
        }
        else
        {
            spriteObj = _mpBar;
            spriteObj2 = _hpBar;
            objContainer = _mpBarContainer;
            objContainer2 = _hpBarContainer;
        }

        Position = position;
        spriteObj.Position = new Vector2(X + 7f, Y + 60f);
        spriteObj2.Position = new Vector2(X + 8f, Y + 29f);
        _playerLevelText.Position = new Vector2(X + 30f, Y - 20f);
        if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f)
        {
            _mpText.Position = new Vector2(X + 5f, Y + 19f);
            _mpText.X += 8f;
            _hpText.Position = _mpText.Position;
            _hpText.Y += 28f;
        }
        else
        {
            _hpText.Position = new Vector2(X + 5f, Y + 19f);
            _hpText.X += 8f;
            _hpText.Y += 5f;
            _mpText.Position = _hpText.Position;
            _mpText.Y += 30f;
        }

        objContainer2.Position = new Vector2(X, Y + 17f);
        if (spriteObj2 == _hpBar)
        {
            spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 7f);
        }
        else
        {
            spriteObj2.Position = new Vector2(objContainer2.X + 2f, objContainer2.Y + 6f);
        }

        objContainer.Position = new Vector2(X, objContainer2.Bounds.Bottom);
        if (spriteObj == _mpBar)
        {
            spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 6f);
        }
        else
        {
            spriteObj.Position = new Vector2(objContainer.X + 2f, objContainer.Y + 7f);
        }

        _coin.Position = new Vector2(X, objContainer.Bounds.Bottom + 2);
        _goldText.Position = new Vector2(_coin.X + 28f, _coin.Y - 2f);
        _iconHolder1.Position = new Vector2(_coin.X + 25f, _coin.Y + 60f);
        _iconHolder2.Position = new Vector2(_iconHolder1.X + 55f, _iconHolder1.Y);
        _spellIcon.Position = _iconHolder1.Position;
        _specialItemIcon.Position = _iconHolder2.Position;
        _spellCost.Position = new Vector2(_spellIcon.X, _spellIcon.Bounds.Bottom + 10);
    }

    public void Update(PlayerObj player)
    {
        var num = Game.PlayerStats.CurrentLevel;
        if (num < 0)
        {
            num = 0;
        }

        _playerLevelText.Text = num.ToString();
        var num2 = Game.PlayerStats.Gold;
        if (num2 < 0)
        {
            num2 = 0;
        }

        _goldText.Text = num2.ToString();

        _hpText.Text = player.CurrentHealth + "/" + player.MaxHealth;
        _mpText.Text = player.CurrentMana + "/" + player.MaxMana;
        UpdatePlayerHP(player);
        UpdatePlayerMP(player);
    }

    private void UpdatePlayerHP(PlayerObj player)
    {
        var num = player.MaxHealth - player.BaseHealth;
        var num2 = player.CurrentHealth / (float) player.MaxHealth;
        var num3 = (int) (88f + num / 5f);
        if (num3 > _maxBarLength)
        {
            num3 = _maxBarLength;
        }

        var scaleX = (num3 - 28 - 28) / 32f;
        _hpBarContainer.GetChildAt(1).ScaleX = scaleX;
        _hpBarContainer.GetChildAt(2).X = _hpBarContainer.GetChildAt(1).Bounds.Right;
        _hpBarContainer.CalculateBounds();
        _hpBar.ScaleX = 1f;
        _hpBar.ScaleX = (_hpBarContainer.Width - 8) / (float) _hpBar.Width * num2;
    }

    private void UpdatePlayerMP(PlayerObj player)
    {
        var num = (int) (player.MaxMana - player.BaseMana);
        var num2 = player.CurrentMana / player.MaxMana;
        var num3 = (int) (88f + num / 5f);
        if (num3 > _maxBarLength)
        {
            num3 = _maxBarLength;
        }

        var scaleX = (num3 - 28 - 28) / 32f;
        _mpBarContainer.GetChildAt(1).ScaleX = scaleX;
        _mpBarContainer.GetChildAt(2).X = _mpBarContainer.GetChildAt(1).Bounds.Right;
        _mpBarContainer.CalculateBounds();
        _mpBar.ScaleX = 1f;
        _mpBar.ScaleX = (_mpBarContainer.Width - 8) / (float) _mpBar.Width * num2;
    }

    public void UpdatePlayerLevel()
    {
        _playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
    }

    public void UpdateAbilityIcons()
    {
        var abilitiesSpriteArray = _abilitiesSpriteArray;
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
                _abilitiesSpriteArray[num].ChangeSprite(((EquipmentAbility) b).Icon());
                num++;
            }
        }
    }

    public void UpdateSpecialItemIcon()
    {
        _specialItemIcon.Visible = false;
        _iconHolder2.Opacity = 0.5f;
        if (Game.PlayerStats.SpecialItem != 0)
        {
            _specialItemIcon.Visible = true;
            _specialItemIcon.ChangeSprite(((SpecialItemType) Game.PlayerStats.SpecialItem).SpriteName());
            _iconHolder2.Opacity = 1f;
        }
    }

    public void UpdateSpellIcon()
    {
        _spellIcon.Visible = false;
        _iconHolder1.Opacity = 0.5f;
        _spellCost.Visible = false;
        if (Game.PlayerStats.Spell != 0)
        {
            _spellIcon.ChangeSprite(((SpellType) Game.PlayerStats.Spell).Icon());
            _spellIcon.Visible = true;
            _iconHolder1.Opacity = 1f;
            _spellCost.Text =
                (int)
                (SpellEV.GetManaCost(Game.PlayerStats.Spell) *
                 (1f - SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount)) + " mp";
            _spellCost.Visible = true;
        }
    }

    public override void Draw(Camera2D camera)
    {
        if (Visible)
        {
            if (!ShowBarsOnly)
            {
                base.Draw(camera);
                _coin.Draw(camera);
                _playerLevelText.Draw(camera);
                _goldText.Draw(camera);
                camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                var abilitiesSpriteArray = _abilitiesSpriteArray;
                for (var i = 0; i < abilitiesSpriteArray.Length; i++)
                {
                    var spriteObj = abilitiesSpriteArray[i];
                    spriteObj.Draw(camera);
                }

                _iconHolder1.Draw(camera);
                _iconHolder2.Draw(camera);
                _spellIcon.Draw(camera);
                _specialItemIcon.Draw(camera);
                camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                _spellCost.Draw(camera);
            }

            _mpBar.Draw(camera);
            _mpText.Draw(camera);
            if (Game.PlayerStats.Traits.X != 30f && Game.PlayerStats.Traits.Y != 30f)
            {
                _hpBar.Draw(camera);
                _hpText.Draw(camera);
            }

            _mpBarContainer.Draw(camera);
            _hpBarContainer.Draw(camera);
        }
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        var abilitiesSpriteArray = _abilitiesSpriteArray;
        for (var i = 0; i < abilitiesSpriteArray.Length; i++)
        {
            var spriteObj = abilitiesSpriteArray[i];
            spriteObj.Dispose();
        }

        Array.Clear(_abilitiesSpriteArray, 0, _abilitiesSpriteArray.Length);
        _abilitiesSpriteArray = null;
        _coin.Dispose();
        _coin = null;
        _mpBar.Dispose();
        _mpBar = null;
        _hpBar.Dispose();
        _hpBar = null;
        _playerLevelText.Dispose();
        _playerLevelText = null;
        _goldText.Dispose();
        _goldText = null;
        _hpText.Dispose();
        _hpText = null;
        _mpText.Dispose();
        _mpText = null;
        _hpBarContainer.Dispose();
        _hpBarContainer = null;
        _mpBarContainer.Dispose();
        _mpBarContainer = null;
        _specialItemIcon.Dispose();
        _specialItemIcon = null;
        _spellIcon.Dispose();
        _spellIcon = null;
        _spellCost.Dispose();
        _spellCost = null;
        _iconHolder1.Dispose();
        _iconHolder1 = null;
        _iconHolder2.Dispose();
        _iconHolder2 = null;
        base.Dispose();
    }
}
