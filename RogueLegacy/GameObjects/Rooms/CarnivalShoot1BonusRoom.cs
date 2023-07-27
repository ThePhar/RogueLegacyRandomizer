// RogueLegacyRandomizer - CarnivalShoot1BonusRoom.cs
// Last Modified 2023-07-27 12:05 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class CarnivalShoot1BonusRoom : BonusRoomObj
{
    private const int   NUM_TARGETS      = 8;
    private const int   NUM_TRIES        = 12;
    private const float TARGET_SPEED_MOD = 100f;

    private List<ObjContainer> _balloonList;
    private BreakableObj       _currentTarget;
    private int                _currentTargetIndex;
    private ObjContainer       _daggerIcons;
    private int                _daggersThrown;
    private NpcObj             _elf;
    private bool               _isPlayingGame;
    private GameObj            _line;
    private ChestObj           _rewardChest;
    private bool               _spokeToNPC;
    private float              _storedPlayerMana;
    private byte               _storedPlayerSpell;
    private ObjContainer       _targetIcons;
    private List<BreakableObj> _targetList;
    private bool               _targetMovingUp;
    private float              _targetSpeed = 200f;

    public CarnivalShoot1BonusRoom()
    {
        _elf = new NpcObj("Clown_Character")
        {
            Scale = new Vector2(2f, 2f)
        };
        _balloonList = new List<ObjContainer>();
    }

    private int ActiveTargets => _targetList.Select(t => !t.Broken).Count();

    public override void LoadContent(GraphicsDevice graphics)
    {
        _daggerIcons = new ObjContainer();
        var xOffset = 0;
        var yOffset = 10;
        for (var i = 0; i < NUM_TRIES; i++)
        {
            var spriteObj = new SpriteObj("SpellDagger_Sprite")
            {
                Scale = new Vector2(2f, 2f),
                X = xOffset + 10,
                Y = yOffset
            };

            xOffset += spriteObj.Width;
            if (i == NUM_TRIES / 2 - 1)
            {
                xOffset = 0;
                yOffset += 20;
            }

            _daggerIcons.AddChild(spriteObj);
        }

        _daggerIcons.OutlineWidth = 2;
        _targetIcons = new ObjContainer();
        for (var j = 0; j < NUM_TARGETS; j++)
        {
            var spriteObj2 = new SpriteObj("Target2Piece1_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            };

            spriteObj2.X += j * (spriteObj2.Width + 10);
            _targetIcons.AddChild(spriteObj2);
        }

        _targetIcons.OutlineWidth = 2;
        GameObjList.Add(_targetIcons);
        GameObjList.Add(_daggerIcons);
        base.LoadContent(graphics);
    }

    public override void Initialize()
    {
        Color[] array =
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Orange,
            Color.Purple,
            Color.Pink,
            Color.MediumTurquoise,
            Color.CornflowerBlue
        };

        foreach (var obj in GameObjList)
        {
            if (obj is WaypointObj) _elf.X = obj.X;

            switch (obj.Name)
            {
                case "Line":
                    _line = obj;
                    break;

                case "Balloon":
                    _balloonList.Add(obj as ObjContainer);
                    ((ObjContainer) obj).GetChildAt(1).TextureColor = array[CDGMath.RandomInt(0, array.Length - 1)];
                    break;
            }
        }

        var chestY = 0f;
        foreach (var terrain in TerrainObjList.Where(terrain => terrain.Name == "Floor"))
        {
            _elf.Y = terrain.Y - (_elf.Bounds.Bottom - _elf.Y);
            chestY = terrain.Y;
            break;
        }

        if (!IsReversed) _elf.Flip = SpriteEffects.FlipHorizontally;

        GameObjList.Add(_elf);
        _elf.Y -= 2f;
        _targetList = new List<BreakableObj>();
        for (var i = 0; i < NUM_TARGETS; i++)
        {
            var breakableObj = new BreakableObj("Target1_Character")
            {
                Scale = new Vector2(2f, 2f),
                Visible = false,
                DropItem = false,
                HitBySpellsOnly = true,
                Position = _line.Position
            };

            breakableObj.Flip = !IsReversed ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _targetList.Add(breakableObj);
            GameObjList.Add(breakableObj);
        }

        _rewardChest = new ChestObj(null)
        {
            ChestType = ChestType.Gold
        };

        if (!IsReversed)
        {
            _rewardChest.Flip = SpriteEffects.FlipHorizontally;
            _rewardChest.Position = new Vector2(_elf.X + 100f, chestY - _rewardChest.Height - 8f);
        }
        else
        {
            _rewardChest.Position = new Vector2(_elf.X - 150f, chestY - _rewardChest.Height - 8f);
        }

        _rewardChest.Visible = false;
        GameObjList.Add(_rewardChest);
        base.Initialize();
    }

    public override void OnEnter()
    {
        _rewardChest.ChestType = ChestType.Gold;
        if (_rewardChest.PhysicsMngr == null) Player.PhysicsMngr.AddObject(_rewardChest);

        _spokeToNPC = false;
        Player.AttachedLevel.CameraLockedToPlayer = false;
        if (!IsReversed)
            Player.AttachedLevel.Camera.Position = new Vector2(Bounds.Left + Player.AttachedLevel.Camera.Width / 2,
                Bounds.Top + Player.AttachedLevel.Camera.Height / 2);
        else
            Player.AttachedLevel.Camera.Position = new Vector2(Bounds.Right - Player.AttachedLevel.Camera.Width / 2,
                Bounds.Top + Player.AttachedLevel.Camera.Height / 2);

        _currentTargetIndex = 0;
        _daggersThrown = 0;
        _storedPlayerMana = Player.CurrentMana;
        _storedPlayerSpell = Game.PlayerStats.Spell;
        InitializeTargetSystem();
        if (!IsReversed)
        {
            _targetIcons.Position = new Vector2(Bounds.Right - 100 - _targetIcons.Width, Bounds.Bottom - 40);
            _daggerIcons.Position = _targetIcons.Position;
            _daggerIcons.X -= 400 + _daggerIcons.Width;
        }
        else
        {
            _targetIcons.Position = new Vector2(Bounds.Left + 150, Bounds.Bottom - 40);
            _daggerIcons.Position = _targetIcons.Position;
            _daggerIcons.X += _targetIcons.Width + 400;
        }

        _daggerIcons.Y -= 30f;
        ReflipPosters();
        base.OnEnter();
    }

    private void ReflipPosters()
    {
        foreach (var obj in GameObjList)
            if (obj is SpriteObj
                {
                    Flip: SpriteEffects.FlipHorizontally,
                    SpriteName: "CarnivalPoster1_Sprite" or "CarnivalPoster2_Sprite" or "CarnivalPoster3_Sprite"
                    or "CarnivalTent_Sprite"
                } spriteObj)
                spriteObj.Flip = SpriteEffects.None;
    }

    public void BeginGame()
    {
        Player.AttachedLevel.ProjectileManager.DestroyAllProjectiles(true);
        Player.StopAllSpells();
        _isPlayingGame = true;
        _spokeToNPC = true;
        Player.AttachedLevel.CameraLockedToPlayer = false;
        Player.AttachedLevel.Camera.Y = Bounds.Center.Y;

        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X",
            !IsReversed
                ? (_line.X + 500f).ToString(CultureInfo.InvariantCulture)
                : (_line.X - 500f).ToString(CultureInfo.InvariantCulture));

        EquipPlayer();
        ActivateTarget();
    }

    public void EndGame()
    {
        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X",
            !IsReversed
                ? (X + Player.AttachedLevel.Camera.Width / 2f).ToString(CultureInfo.InvariantCulture)
                : (Bounds.Right - Player.AttachedLevel.Camera.Width / 2f).ToString(CultureInfo.InvariantCulture));

        Tween.AddEndHandlerToLastTween(this, "CheckPlayerReward");
        _isPlayingGame = false;
        Game.PlayerStats.Spell = (byte) SpellType.None;
        Player.AttachedLevel.UpdatePlayerSpellIcon();
        RoomCompleted = true;
    }

    public void CheckPlayerReward()
    {
        var rCScreenManager = (RCScreenManager) Player.AttachedLevel.ScreenManager;

        if (ActiveTargets <= 0)
        {
            rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Reward");
            rCScreenManager.DisplayScreen((int) ScreenType.Dialogue, true);
            RevealChest();

            Program.Game.CollectItemFromLocation(LocationCode.CARNIVAL_GAME_REWARD);
            return;
        }

        rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Fail");
        rCScreenManager.DisplayScreen((int) ScreenType.Dialogue, true);
    }

    public void RevealChest()
    {
        Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(_rewardChest.Position);
        _rewardChest.Visible = true;
    }

    private void InitializeTargetSystem()
    {
        foreach (var current in _targetList)
        {
            current.Reset();
            current.Visible = false;
            if (!IsReversed)
            {
                current.Position = new Vector2(Bounds.Right, Bounds.Center.Y);
                current.Flip = SpriteEffects.FlipHorizontally;
            }
            else
            {
                current.Position = new Vector2(Bounds.Left, Bounds.Center.Y);
                current.Flip = SpriteEffects.None;
            }
        }
    }

    private void EquipPlayer()
    {
        Game.PlayerStats.Spell = 1;
        Player.AttachedLevel.UpdatePlayerSpellIcon();
        Player.CurrentMana = Player.MaxMana;
    }

    public void UnequipPlayer()
    {
        Game.PlayerStats.Spell = _storedPlayerSpell;
        Player.AttachedLevel.UpdatePlayerSpellIcon();
        Player.CurrentMana = _storedPlayerMana;
    }

    public override void OnExit()
    {
        UnequipPlayer();
        Player.AttachedLevel.CameraLockedToPlayer = true;
        base.OnExit();
    }

    private void HandleInput()
    {
        if (_isPlayingGame &&
            (Game.GlobalInput.JustPressed(24) ||
             (Game.GlobalInput.JustPressed(12) && Game.PlayerStats.Class == 16)) &&
            Player.SpellCastDelay <= 0f)
        {
            _daggersThrown++;
            Player.CurrentMana = Player.MaxMana;
            if (_daggersThrown <= NUM_TRIES) _daggerIcons.GetChildAt(NUM_TRIES - _daggersThrown).Visible = false;

            if (_daggersThrown > NUM_TRIES) Game.PlayerStats.Spell = 0;
        }
    }

    public override void Update(GameTime gameTime)
    {
        _elf.Update(gameTime, Player);
        if (!IsReversed)
        {
            if (Player.X >= _line.X - 150f) Player.X = (int) _line.X - 150;
        }
        else if (Player.X < _line.X + 150f)
        {
            Player.X = _line.X + 150f;
        }

        if (!IsReversed)
        {
            if (_isPlayingGame && Player.X < Player.AttachedLevel.Camera.Bounds.Left)
                Player.X = Player.AttachedLevel.Camera.Bounds.Left;

            if (Player.X > Bounds.Right - 1320) Player.X = Bounds.Right - 1320;
        }
        else
        {
            if (_isPlayingGame && Player.X > Player.AttachedLevel.Camera.Bounds.Right)
                Player.X = Player.AttachedLevel.Camera.Bounds.Right;

            if (Player.X < Bounds.Left + 1320) Player.X = Bounds.Left + 1320;
        }

        if (_currentTarget != null && !_currentTarget.Broken)
        {
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_targetMovingUp && _currentTarget.Bounds.Top > Bounds.Top + 80)
            {
                _currentTarget.Y -= num * _targetSpeed;
            }
            else if (_targetMovingUp)
            {
                _currentTarget.Y += num * _targetSpeed;
                _targetMovingUp = false;
            }

            if (!_targetMovingUp && _currentTarget.Bounds.Bottom < Bounds.Bottom - 140)
            {
                _currentTarget.Y += num * _targetSpeed;
            }
            else if (!_targetMovingUp)
            {
                _currentTarget.Y -= num * _targetSpeed;
                _targetMovingUp = true;
            }
        }

        if (_isPlayingGame &&
            ((_daggersThrown >= NUM_TRIES && Player.AttachedLevel.ProjectileManager.ActiveProjectiles < 1 &&
              ActiveTargets > 0) || ActiveTargets <= 0))
            EndGame();

        if (_currentTarget != null && _currentTarget.Broken && ActiveTargets >= 0)
        {
            _currentTargetIndex++;
            ActivateTarget();
        }

        if (_elf.IsTouching && !RoomCompleted && !_spokeToNPC)
        {
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Start");
                rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "BeginGame");
                rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                    "Canceling Selection");
                (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
            }
        }
        else if (_elf.IsTouching && RoomCompleted &&
                 (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
        {
            var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
            rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-End");
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
        }

        if (_isPlayingGame)
            _elf.CanTalk = false;
        else
            _elf.CanTalk = true;

        var totalGameTime = Game.TotalGameTimeSeconds;
        var num2 = 2f;
        foreach (var current in _balloonList)
        {
            current.Rotation = (float) Math.Sin(totalGameTime * num2) * num2;
            num2 += 0.2f;
        }

        HandleInput();
        base.Update(gameTime);
    }

    public void ActivateTarget()
    {
        if (NUM_TARGETS - _currentTargetIndex < _targetIcons.NumChildren)
        {
            _targetIcons.GetChildAt(NUM_TARGETS - _currentTargetIndex).Visible = false;
            GiveGold();
        }

        if (_currentTargetIndex >= NUM_TARGETS)
        {
            _currentTarget = null;
            return;
        }

        if (_currentTarget != null) _targetSpeed += TARGET_SPEED_MOD;

        _currentTarget = _targetList[_currentTargetIndex];
        _currentTarget.Visible = true;
        if (!IsReversed)
        {
            Tween.By(_currentTarget, 2f, Quad.EaseOut, "X", (-400 + CDGMath.RandomInt(-200, 200)).ToString());
            return;
        }

        Tween.By(_currentTarget, 2f, Quad.EaseOut, "X", (400 + CDGMath.RandomInt(-200, 200)).ToString());
    }

    public void GiveGold()
    {
        var num = NUM_TARGETS - ActiveTargets;
        if (ActiveTargets > 0)
            Player.AttachedLevel.ImpactEffectPool.CarnivalGoldEffect(
                _currentTarget.Position,
                new Vector2(
                    Player.AttachedLevel.Camera.TopLeftCorner.X + 50f,
                    Player.AttachedLevel.Camera.TopLeftCorner.Y + 135f
                ),
                num);

        var gold = (int) (num * 10 * ArchipelagoManager.RandomizerData.GoldGainMultiplier);
        Player.AttachedLevel.TextManager.DisplayNumberStringText(gold, " gold", Color.Yellow, _currentTarget.Position);
        Game.PlayerStats.Gold += gold;
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        _targetList.Clear();
        _targetList = null;
        _line = null;
        _currentTarget = null;
        _elf = null;
        _daggerIcons = null;
        _targetIcons = null;
        _balloonList.Clear();
        _balloonList = null;
        _rewardChest = null;
        base.Dispose();
    }
}