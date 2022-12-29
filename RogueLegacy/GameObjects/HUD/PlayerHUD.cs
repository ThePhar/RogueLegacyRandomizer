using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer.Definitions;
using RogueLegacy.Enums;

namespace RogueLegacy.GameObjects.HUD;

public class PlayerHUD : SpriteObj
{
    private const int MaxBarLength = 360;

    private          SpriteObj[]      _abilitiesSpriteArray;
    private          SpriteObj        _coin;
    private          TextObj          _goldText;
    private readonly SpriteObj        _fountainPiece;
    private readonly TextObj          _fountainPieceText;
    private          SpriteObj        _hpBar;
    private          ObjContainer     _hpBarContainer;
    private          TextObj          _hpText;
    private          SpriteObj        _iconHolder1;
    private          SpriteObj        _iconHolder2;
    private          SpriteObj        _mpBar;
    private          ObjContainer     _mpBarContainer;
    private          TextObj          _mpText;
    private          TextObj          _playerLevelText;
    private          SpriteObj        _specialItemIcon;
    private          TextObj          _spellCost;
    private          SpriteObj        _spellIcon;
    private          ReceivedItemsHUD _receivedItemsHUD;

    public PlayerHUD() : base("PlayerHUDLvlText_Sprite")
    {
        ForceDraw = true;

        _coin = new SpriteObj("PlayerUICoin_Sprite") { ForceDraw = true };
        _fountainPiece = new SpriteObj("TeleportRock4_Sprite")
        {
            ForceDraw = true,
            OutlineColour = Color.Black,
            OutlineWidth = 2,
            Anchor = new Vector2(0, 0)
        };

        _hpBar = new SpriteObj("HPBar_Sprite") { ForceDraw = true };
        _mpBar = new SpriteObj("MPBar_Sprite") { ForceDraw = true };
        _hpBarContainer = new ObjContainer("PlayerHUDHPBar_Character") { ForceDraw = true };
        _mpBarContainer = new ObjContainer("PlayerHUDMPBar_Character") { ForceDraw = true };
        _receivedItemsHUD = new ReceivedItemsHUD();
        _playerLevelText = new TextObj
        {
            Text = Game.PlayerStats.CurrentLevel.ToString(),
            Font = Game.PlayerLevelFont
        };
        _goldText = new TextObj
        {
            Text = "0",
            Font = Game.GoldFont,
            FontSize = 25f
        };
        _fountainPieceText = new TextObj
        {
            Text = "0",
            Font = Game.BitFont,
            FontSize = 17f,
            OutlineColour = Color.Black,
            OutlineWidth = 2,
            TextureColor = Color.MediumPurple
        };
        _hpText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 7f,
            DropShadow = new Vector2(1f, 1f),
            ForceDraw = true
        };
        _mpText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 7f,
            DropShadow = new Vector2(1f, 1f),
            ForceDraw = true
        };
        _specialItemIcon = new SpriteObj("Blank_Sprite")
        {
            ForceDraw = true,
            OutlineWidth = 1,
            Scale = new Vector2(1.7f, 1.7f),
            Visible = false
        };
        _spellIcon = new SpriteObj(((SpellType) 0).Icon())
        {
            ForceDraw = true,
            OutlineWidth = 1,
            Visible = false
        };
        _iconHolder1 = new SpriteObj("BlacksmithUI_IconBG_Sprite")
        {
            ForceDraw = true,
            Opacity = 0.5f,
            Scale = new Vector2(0.8f, 0.8f)
        };
        _iconHolder2 = _iconHolder1.Clone() as SpriteObj;
        _spellCost = new TextObj(Game.JunicodeFont)
        {
            Align = Types.TextAlign.Centre,
            ForceDraw = true,
            OutlineWidth = 2,
            FontSize = 8f,
            Visible = false
        };

        _abilitiesSpriteArray = new SpriteObj[5];
        var position = new Vector2(130f, 690f);
        for (var i = 0; i < _abilitiesSpriteArray.Length; i++)
        {
            _abilitiesSpriteArray[i] = new SpriteObj("Blank_Sprite")
            {
                ForceDraw = true,
                Position = position,
                Scale = new Vector2(0.5f, 0.5f)
            };

            position.X += 35;
        }

        UpdateSpecialItemIcon();
        UpdateSpellIcon();
    }

    public bool ShowBarsOnly { get; set; }

    public void SetPosition(Vector2 position)
    {
        SpriteObj topBar;
        SpriteObj bottomBar;
        ObjContainer topContainer;
        ObjContainer bottomContainer;

        // Reverse the bars if they have "Dextrocardia".
        if (TraitHelper.HasTrait(Trait.Dextrocardia))
        {
            topBar = _hpBar;
            bottomBar = _mpBar;
            topContainer = _hpBarContainer;
            bottomContainer = _mpBarContainer;
        }
        else
        {
            topBar = _mpBar;
            bottomBar = _hpBar;
            topContainer = _mpBarContainer;
            bottomContainer = _hpBarContainer;
        }

        // Set positions for HP and MP text and initial spots for top and bottom bars.
        Position = position;
        topBar.Position = new Vector2(X + 7f, Y + 60f);
        bottomBar.Position = new Vector2(X + 8f, Y + 29f);
        _playerLevelText.Position = new Vector2(X + 30f, Y - 20f);
        if (TraitHelper.HasTrait(Trait.Dextrocardia))
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

        // Finalize bar and container positions for HP and MP.
        bottomContainer.Position = new Vector2(X, Y + 17f);
        bottomBar.Position = bottomBar == _hpBar
            ? new Vector2(bottomContainer.X + 2f, bottomContainer.Y + 7f)
            : new Vector2(bottomContainer.X + 2f, bottomContainer.Y + 6f);
        topContainer.Position = new Vector2(X, bottomContainer.Bounds.Bottom);
        topBar.Position = topBar == _mpBar
            ? new Vector2(topContainer.X + 2f, topContainer.Y + 6f)
            : new Vector2(topContainer.X + 2f, topContainer.Y + 7f);

        // Set the rest of the UI positions.
        _coin.Position = new Vector2(X, topContainer.Bounds.Bottom + 2);
        _goldText.Position = new Vector2(_coin.X + 28f, _coin.Y - 2f);
        _iconHolder1.Position = new Vector2(_coin.X + 25f, _coin.Y + 60f);
        _iconHolder2.Position = new Vector2(_iconHolder1.X + 55f, _iconHolder1.Y);
        _spellIcon.Position = _iconHolder1.Position;
        _specialItemIcon.Position = _iconHolder2.Position;
        _spellCost.Position = new Vector2(_spellIcon.X, _spellIcon.Bounds.Bottom + 10);
        _receivedItemsHUD.Position = new Vector2(X, topContainer.Bounds.Bottom + 8);
    }

    public void Update(PlayerObj player)
    {
        var level = Game.PlayerStats.CurrentLevel;
        if (level < 0)
            level = 0;

        var gold = Game.PlayerStats.Gold;
        if (gold < 0)
            gold = 0;

        _playerLevelText.Text = level.ToString();
        _goldText.Text = gold.ToString();

        _fountainPieceText.Text =
            $"{Game.PlayerStats.FountainPieces}/{Program.Game.ArchipelagoManager.RandomizerData.FountainHuntRequirement}";
        _hpText.Text = player.CurrentHealth + "/" + player.MaxHealth;
        _mpText.Text = player.CurrentMana + "/" + player.MaxMana;
        UpdatePlayerHP(player);
        UpdatePlayerMP(player);

        _fountainPiece.Position = new Vector2(X, _spellCost.Bounds.Bottom + 12f);
        _fountainPieceText.Position = new Vector2(_fountainPiece.X + 24f, _fountainPiece.Y);

        _receivedItemsHUD.Update();
    }

    private void UpdatePlayerHP(PlayerObj player)
    {
        var currentHealth = player.MaxHealth - player.BaseHealth;
        var healthPercentage = player.CurrentHealth / (float) player.MaxHealth;
        var hpBarWidth = (int) (88f + currentHealth / 5f);
        if (hpBarWidth > MaxBarLength)
        {
            hpBarWidth = MaxBarLength;
        }

        var scaleX = (hpBarWidth - 28 - 28) / 32f;
        _hpBarContainer.GetChildAt(1).ScaleX = scaleX;
        _hpBarContainer.GetChildAt(2).X = _hpBarContainer.GetChildAt(1).Bounds.Right;
        _hpBarContainer.CalculateBounds();
        _hpBar.ScaleX = 1f;
        _hpBar.ScaleX = (_hpBarContainer.Width - 8) / (float) _hpBar.Width * healthPercentage;
    }

    private void UpdatePlayerMP(PlayerObj player)
    {
        var currentMana = (int) (player.MaxMana - player.BaseMana);
        var manaPercentage = player.CurrentMana / player.MaxMana;
        var mpBarWidth = (int) (88f + currentMana / 5f);
        if (mpBarWidth > MaxBarLength)
        {
            mpBarWidth = MaxBarLength;
        }

        var scaleX = (mpBarWidth - 28 - 28) / 32f;
        _mpBarContainer.GetChildAt(1).ScaleX = scaleX;
        _mpBarContainer.GetChildAt(2).X = _mpBarContainer.GetChildAt(1).Bounds.Right;
        _mpBarContainer.CalculateBounds();
        _mpBar.ScaleX = 1f;
        _mpBar.ScaleX = (_mpBarContainer.Width - 8) / (float) _mpBar.Width * manaPercentage;
    }

    public void UpdatePlayerLevel()
    {
        _playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
    }

    public void UpdateAbilityIcons()
    {
        var abilitiesSpriteArray = _abilitiesSpriteArray;
        foreach (var spriteObj in abilitiesSpriteArray)
        {
            spriteObj.ChangeSprite("Blank_Sprite");
        }

        var runeIndex = 0;
        var getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
        foreach (var rune in getEquippedRuneArray)
        {
            if (rune == -1)
                continue;

            _abilitiesSpriteArray[runeIndex].ChangeSprite(((EquipmentAbility) rune).Icon());
            runeIndex++;
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
        if (Game.PlayerStats.Spell != (byte) SpellType.None)
        {
            _spellIcon.ChangeSprite(((SpellType) Game.PlayerStats.Spell).Icon());
            _spellIcon.Visible = true;
            _iconHolder1.Opacity = 1f;

            var mpCost =
                SpellEV.GetManaCost(Game.PlayerStats.Spell) *
                (1f - SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount);
            _spellCost.Text = (int) mpCost + " mp";
            _spellCost.Visible = true;
        }
    }

    public void AddReceivedItem(ItemCode.ItemType type, long item, string receivedFrom,
        Tuple<float, float, float, float> stats)
    {
        _receivedItemsHUD.Elements.Add(new ReceivedItemElement(_receivedItemsHUD, type, item, receivedFrom, stats));
    }

    public override void Draw(Camera2D camera)
    {
        if (!Visible)
            return;

        if (!ShowBarsOnly)
        {
            base.Draw(camera);
            if (Program.Game.ArchipelagoManager.RandomizerData.FountainHuntMode)
            {
                _fountainPiece.Draw(camera);
                _fountainPieceText.Draw(camera);
            }

            _coin.Draw(camera);
            _playerLevelText.Draw(camera);
            _goldText.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _iconHolder1.Draw(camera);
            _iconHolder2.Draw(camera);
            _spellIcon.Draw(camera);
            _specialItemIcon.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _spellCost.Draw(camera);

            var abilitiesSpriteArray = _abilitiesSpriteArray;
            foreach (var spriteObj in abilitiesSpriteArray)
                spriteObj.Draw(camera);
        }

        _mpBar.Draw(camera);
        _mpText.Draw(camera);
        if (!TraitHelper.HasTrait(Trait.CIP))
        {
            _hpBar.Draw(camera);
            _hpText.Draw(camera);
        }

        _mpBarContainer.Draw(camera);
        _hpBarContainer.Draw(camera);
        _receivedItemsHUD.Draw(camera);
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;

        var abilitiesSpriteArray = _abilitiesSpriteArray;
        foreach (var spriteObj in abilitiesSpriteArray)
            spriteObj.Dispose();

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
        _receivedItemsHUD.Dispose();
        _receivedItemsHUD = null;
        base.Dispose();
    }
}
