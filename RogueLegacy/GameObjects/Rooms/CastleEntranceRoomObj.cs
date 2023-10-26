//  RogueLegacyRandomizer - CastleEntranceRoomObj.cs
//  Last Modified 2023-10-26 1:59 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class CastleEntranceRoomObj : RoomObj
{
    private bool          _allFilesSaved;
    private DoorObj       _bossDoor;
    private bool          _bossDoorOpening;
    private ObjContainer  _bossDoorSprite;
    private PhysicsObj    _castleGate;
    private SpriteObj     _diary;
    private bool          _gateClosed;
    private KeyIconObj    _mapIcon;
    private TextObj       _mapText;
    private SpriteObj     _speechBubble;
    private TeleporterObj _teleporter;

    public CastleEntranceRoomObj()
    {
        _teleporter = new();
        _castleGate = new("CastleEntranceGate_Sprite")
        {
            IsWeighted = false,
            IsCollidable = true,
            CollisionTypeTag = 1,
            Layer = -1f,
            OutlineWidth = 2,
        };

        GameObjList.Add(_teleporter);
        GameObjList.Add(_castleGate);
    }

    public bool RoomCompleted { get; set; }

    public override void Initialize()
    {
        _speechBubble = new("ExclamationSquare_Sprite");
        _speechBubble.Flip = SpriteEffects.FlipHorizontally;
        _speechBubble.Scale = new(1.2f, 1.2f);
        GameObjList.Add(_speechBubble);
        _mapText = new KeyIconTextObj(Game.JunicodeFont);
        _mapText.Text = "view map any time";
        _mapText.Align = Types.TextAlign.Centre;
        _mapText.FontSize = 12f;
        _mapText.OutlineWidth = 2;
        var placeholder = new TextObj(Game.JunicodeFont);
        placeholder.Text = "this is where my shrine items would go... IF they were ready!";
        placeholder.Align = Types.TextAlign.Centre;
        placeholder.FontSize = 10f;
        placeholder.OutlineWidth = 2;
        placeholder.Name = "placeholder";
        GameObjList.Add(_mapText);
        GameObjList.Add(placeholder);
        _mapIcon = new();
        _mapIcon.Scale = new(0.5f, 0.5f);
        GameObjList.Add(_mapIcon);
        foreach (var current in GameObjList)
        {
            if (current.Name == "diary")
            {
                _diary = current as SpriteObj;
            }

            if (current.Name == "map")
            {
                (current as SpriteObj).OutlineWidth = 2;
                _mapText.Position = new(current.X, current.Bounds.Top - 50);
                _mapIcon.Position = new(_mapText.X, _mapText.Y - 20f);
            }

            if (current.Name == "placeholder")
            {
                placeholder.Position = new(_mapText.X + 1020f, _mapText.Y + 120);
            }
        }

        _diary.OutlineWidth = 2;
        _speechBubble.Position = new(_diary.X, _diary.Y - _speechBubble.Height - 20f);
        DoorObj doorObj = null;
        foreach (var current2 in GameObjList)
        {
            if (current2.Name == "LastDoor")
            {
                _bossDoorSprite = current2 as ObjContainer;
                break;
            }
        }

        foreach (var current3 in DoorList)
        {
            if (current3.DoorPosition == "Left")
            {
                doorObj = current3;
            }

            if (current3.IsBossDoor)
            {
                _bossDoor = current3;
                _bossDoor.Locked = true;
            }
        }

        for (var i = 1; i < _bossDoorSprite.NumChildren; i++) _bossDoorSprite.GetChildAt(i).Opacity = 0f;

        _bossDoorSprite.AnimationDelay = 0.1f;
        _castleGate.Position = new(doorObj.Bounds.Right - _castleGate.Width, doorObj.Y - _castleGate.Height);
        _teleporter.Position = new(3300, Y + 720f - 120f);
        base.Initialize();
    }

    public void RevealSymbol(Zone zone, bool tween)
    {
        var flag = false;
        int index;
        switch (zone)
        {
            case Zone.Castle:
                index = 1;
                if (Game.PlayerStats.ChallengeEyeballBeaten) flag = true;

                break;

            case Zone.Garden:
                index = 3;
                if (Game.PlayerStats.ChallengeSkullBeaten) flag = true;

                break;

            case Zone.Dungeon:
                index = 4;
                if (Game.PlayerStats.ChallengeBlobBeaten) flag = true;

                break;

            case Zone.Tower:
                index = 2;
                if (Game.PlayerStats.ChallengeFireballBeaten) flag = true;

                break;

            default:
                index = 5;
                if (Game.PlayerStats.ChallengeLastBossBeaten) flag = true;

                break;
        }

        _bossDoorSprite.GetChildAt(index).TextureColor = flag ? Color.Yellow : Color.White;

        if (tween)
        {
            _bossDoorSprite.GetChildAt(index).Opacity = 0f;
            Tween.To(_bossDoorSprite.GetChildAt(index), 0.5f, Quad.EaseInOut, "delay", "1.5", "Opacity", "1");
            return;
        }

        _bossDoorSprite.GetChildAt(index).Opacity = 1f;
    }

    public override void OnEnter()
    {
        _bossDoorOpening = false;
        if (Game.PlayerStats.ReadLastDiary && LinkedRoom.LinkedRoom != null) LinkedRoom = LinkedRoom.LinkedRoom;

        Game.PlayerStats.LoadStartingRoom = false;
        _speechBubble.Visible = Game.PlayerStats.DiaryEntry < 1;

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _mapIcon.SetButton(Game.GlobalInput.ButtonList[9]);
            _mapIcon.Scale = new(1f, 1f);
        }
        else
        {
            _mapIcon.SetKey(Game.GlobalInput.KeyList[9]);
            _mapIcon.Scale = new(0.5f, 0.5f);
        }

        if (!_allFilesSaved)
        {
            Player.Game.SaveManager.SaveAllFileTypes(false);
            _allFilesSaved = true;
        }

        if (Game.PlayerStats.EyeballBossBeaten) RevealSymbol(Zone.Castle, false);

        if (Game.PlayerStats.FairyBossBeaten) RevealSymbol(Zone.Garden, false);

        if (Game.PlayerStats.BlobBossBeaten) RevealSymbol(Zone.Dungeon, false);

        if (Game.PlayerStats.FireballBossBeaten) RevealSymbol(Zone.Tower, false);

        var hasFountainRequirement = RandomizerData.FountainPieceRequirement == 0 || Game.PlayerStats.FountainPieces >= RandomizerData.FountainPieceRequirement;
        var bossesKilled = !RandomizerData.RequireBosses ||
                           (Game.PlayerStats.EyeballBossBeaten &&
                            Game.PlayerStats.FairyBossBeaten &&
                            Game.PlayerStats.BlobBossBeaten &&
                            Game.PlayerStats.FireballBossBeaten);

        if (bossesKilled && hasFountainRequirement && !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
        {
            PlayBossDoorAnimation();
        }
        else if (Game.PlayerStats.FinalDoorOpened)
        {
            _bossDoor.Locked = false;
            _bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
            _bossDoorSprite.GoToFrame(_bossDoorSprite.TotalFrames);
        }

        if (!_gateClosed) CloseGate(true);

        if (bossesKilled && hasFountainRequirement && !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
        {
            Game.PlayerStats.FinalDoorOpened = true;
            Player.AttachedLevel.RunCinematicBorders(6f);
        }

        // Enable death links now that we are in the castle.
        var manager = Program.Game.ArchipelagoManager;
        manager.IsDeathLinkSafe = true;
        base.OnEnter();
    }

    public void PlayBossDoorAnimation()
    {
        Player.StopDash();
        _bossDoorOpening = true;
        _bossDoor.Locked = false;
        Player.AttachedLevel.UpdateCamera();
        RevealSymbol(Zone.None, true);
        Player.CurrentSpeed = 0f;
        Player.LockControls();
        Player.AttachedLevel.CameraLockedToPlayer = false;

        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", "3960");
        Tween.RunFunction(2.2f, this, "PlayBossDoorAnimation2", Player.AttachedLevel.Camera.X);
    }

    public void PlayBossDoorAnimation2(float storedX)
    {
        _bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
        _bossDoorSprite.PlayAnimation(false);
        SoundManager.PlaySound("LastDoor_Open");

        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "2", "X", storedX.ToString());
        Tween.RunFunction(3.1f, this, "BossDoorAnimationComplete");
    }

    public void BossDoorAnimationComplete()
    {
        _bossDoorOpening = false;
        Player.UnlockControls();
        Player.AttachedLevel.CameraLockedToPlayer = true;
    }

    public void ForceGateClosed()
    {
        _castleGate.Y += _castleGate.Height;
        _gateClosed = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (_bossDoorOpening && !Player.ControlsLocked)
        {
            Player.LockControls();
        }

        if (!SoundManager.IsMusicPlaying)
        {
            SoundManager.PlayMusic("CastleSong", true);
        }

        if (Player.X < _castleGate.Bounds.Right)
        {
            Player.X = _castleGate.Bounds.Right + 20;
            Player.AttachedLevel.UpdateCamera();
        }

        // Diary Logic
        var bounds = _diary.Bounds;
        bounds.X -= 50;
        bounds.Width += 100;
        _speechBubble.Y = _diary.Y - _speechBubble.Height - 20f - 30f + (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 2f;

        if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
        {
            if (_speechBubble.SpriteName == "ExclamationSquare_Sprite")
            {
                _speechBubble.ChangeSprite("UpArrowSquare_Sprite");
            }
        }
        else if (_speechBubble.SpriteName == "UpArrowSquare_Sprite")
        {
            _speechBubble.ChangeSprite("ExclamationSquare_Sprite");
        }

        var freeDiary = RandomizerData.FreeDiaryOnGeneration;

        if ((!Game.PlayerStats.ReadStartingDiary && freeDiary) || (Game.PlayerStats.DiaryEntry < 1 && !freeDiary) || CollisionMath.Intersects(Player.Bounds, bounds))
        {
            _speechBubble.Visible = true;
        }
        else if (Game.PlayerStats.ReadStartingDiary && freeDiary && !CollisionMath.Intersects(Player.Bounds, bounds))
        {
            _speechBubble.Visible = false;
        }
        else if (Game.PlayerStats.DiaryEntry >= 1 && !freeDiary)
        {
            _speechBubble.Visible = false;
        }

        if (Game.PlayerStats.DiaryEntry >= 24 || Game.PlayerStats.ReadStartingDiary)
        {
            _speechBubble.Visible = false;
        }

        if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround && InputTypeHelper.PressedUp)
        {
            if ((!Game.PlayerStats.ReadStartingDiary && Game.PlayerStats.DiaryEntry < 25 && freeDiary) || Game.PlayerStats.DiaryEntry < 1)
            {
                // We're going to set diaries based on lower ones completed.
                Game.PlayerStats.DiaryEntry = 0;

                for (var diary = 0; diary < LocationCode.MAX_DIARIES; diary++)
                {
                    var location = LocationCode.STARTING_DIARY + diary;

                    // Check if we already checked this location and try to get the next item in the sequence if so.
                    var manager = Program.Game.ArchipelagoManager;
                    if (manager.IsLocationChecked(location))
                    {
                        continue;
                    }

                    Game.PlayerStats.DiaryEntry = (byte) (diary + 1);
                    Game.PlayerStats.ReadStartingDiary = true;
                    Program.Game.CollectItemFromLocation(location);
                    Program.Game.SaveManager.SaveFiles(SaveType.PlayerData);
                    break;
                }
            }
        }

        base.Update(gameTime);
    }

    private void CloseGate(bool animate)
    {
        if (animate)
        {
            Player.Y = 381f;
            Player.X += 10f;
            Player.State = 1;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet.AddAction(new PlayAnimationLogicAction());
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 200));
            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
            Player.RunExternalLogicSet(logicSet);
            Tween.By(_castleGate, 1.5f, Quad.EaseOut, "Y", _castleGate.Height.ToString());
            Tween.AddEndHandlerToLastTween(Player, "UnlockControls");
            Player.AttachedLevel.RunCinematicBorders(1.5f);
        }
        else
        {
            _castleGate.Y += _castleGate.Height;
        }

        _gateClosed = true;
    }

    public override void Reset()
    {
        if (_gateClosed)
        {
            _castleGate.Y -= _castleGate.Height;
            _gateClosed = false;
        }

        base.Reset();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _castleGate = null;
        _teleporter = null;
        _bossDoor = null;
        _bossDoorSprite = null;
        _diary = null;
        _speechBubble = null;
        _mapText = null;
        _mapIcon = null;
        base.Dispose();
    }

    protected override GameObj CreateCloneInstance()
    {
        return new CastleEntranceRoomObj();
    }
}
