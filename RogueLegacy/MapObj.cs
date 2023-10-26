//  RogueLegacyRandomizer - MapObj.cs
//  Last Modified 2023-10-26 11:58 AM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class MapObj : GameObj
{
    public           Vector2               CameraOffset;
    private          Rectangle             _alphaMaskRect;
    private          RenderTarget2D        _alphaMaskRt;
    private          List<SpriteObj>       _doorSpriteList;
    private          List<Vector2>         _doorSpritePosList;
    private          List<SpriteObj>       _iconSpriteList;
    private          List<Vector2>         _iconSpritePosList;
    private          ProceduralLevelScreen _level;
    private          RenderTarget2D        _mapScreenRt;
    private          PlayerObj             _player;
    private          SpriteObj             _playerSprite;
    private          List<SpriteObj>       _roomSpriteList;
    private          List<Vector2>         _roomSpritePosList;
    private readonly Vector2               _spriteScale;
    private          List<SpriteObj>       _teleporterList;
    private          List<Vector2>         _teleporterPosList;
    private          TweenObject           _xOffsetTween;
    private          TweenObject           _yOffsetTween;

    public MapObj(bool followPlayer, ProceduralLevelScreen level)
    {
        _level = level;
        FollowPlayer = followPlayer;
        Opacity = 0.3f;
        _roomSpriteList = new();
        _doorSpriteList = new();
        _iconSpriteList = new();
        _roomSpritePosList = new();
        _doorSpritePosList = new();
        _iconSpritePosList = new();
        CameraOffset = new(20f, 560f);
        _playerSprite = new("MapPlayerIcon_Sprite");
        _playerSprite.AnimationDelay = 0.0333333351f;
        _playerSprite.ForceDraw = true;
        _playerSprite.PlayAnimation();
        _spriteScale = new(22f, 22.5f);
        AddedRoomsList = new();
        _teleporterList = new();
        _teleporterPosList = new();
    }

    public bool          DrawTeleportersOnly { get; set; }
    public bool          DrawNothing         { get; set; }
    public bool          FollowPlayer        { get; set; }
    public List<RoomObj> AddedRoomsList      { get; private set; }

    public float CameraOffsetX
    {
        get { return CameraOffset.X; }
        set { CameraOffset.X = value; }
    }

    public float CameraOffsetY
    {
        get { return CameraOffset.Y; }
        set { CameraOffset.Y = value; }
    }

    public void InitializeAlphaMap(Rectangle mapSize, Camera2D camera)
    {
        _alphaMaskRect = mapSize;
        _mapScreenRt = new(camera.GraphicsDevice, 1320, 720);
        _alphaMaskRt = new(camera.GraphicsDevice, 1320, 720);
        CameraOffset = new(mapSize.X, mapSize.Y);
        var spriteObj = new SpriteObj("MapMask_Sprite");
        spriteObj.ForceDraw = true;
        spriteObj.Position = new(mapSize.X, mapSize.Y);
        spriteObj.Scale = new(mapSize.Width / (float) spriteObj.Width,
            mapSize.Height / (float) spriteObj.Height);
        camera.GraphicsDevice.SetRenderTarget(_alphaMaskRt);
        camera.GraphicsDevice.Clear(Color.White);
        camera.Begin();
        spriteObj.Draw(camera);
        camera.End();
        camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
    }

    public void InitializeAlphaMap(RenderTarget2D mapScreenRT, RenderTarget2D alphaMaskRT, Rectangle mapSize)
    {
        _mapScreenRt = mapScreenRT;
        _alphaMaskRt = alphaMaskRT;
        _alphaMaskRect = mapSize;
        CameraOffset = new(mapSize.X, mapSize.Y);
    }

    public void DisposeRTs()
    {
        _mapScreenRt.Dispose();
        _mapScreenRt = null;
        _alphaMaskRt.Dispose();
        _alphaMaskRt = null;
    }

    public void SetPlayer(PlayerObj player)
    {
        _player = player;
    }

    public void AddRoom(RoomObj room)
    {
        if (!AddedRoomsList.Contains(room) && room.Width / 1320 < 5)
        {
            var spriteObj =
                new SpriteObj(string.Concat("MapRoom", room.Width / 1320, "x", room.Height / 720, "_Sprite"));
            spriteObj.Position = new(room.X / _spriteScale.X, room.Y / _spriteScale.Y);
            spriteObj.Scale = new((spriteObj.Width - 3f) / spriteObj.Width,
                (spriteObj.Height - 3f) / spriteObj.Height);
            spriteObj.ForceDraw = true;
            spriteObj.TextureColor = room.TextureColor;
            _roomSpriteList.Add(spriteObj);
            _roomSpritePosList.Add(spriteObj.Position);
            foreach (var current in room.DoorList)
                if (!(room.Name == "CastleEntrance") || !(current.DoorPosition == "Left"))
                {
                    var flag = false;
                    var spriteObj2 = new SpriteObj("MapDoor_Sprite");
                    spriteObj2.ForceDraw = true;
                    string doorPosition;
                    if ((doorPosition = current.DoorPosition) != null)
                    {
                        if (!(doorPosition == "Left"))
                        {
                            if (!(doorPosition == "Right"))
                            {
                                if (!(doorPosition == "Bottom"))
                                {
                                    if (doorPosition == "Top")
                                    {
                                        spriteObj2.Rotation = -90f;
                                        spriteObj2.Position = new(current.X / _spriteScale.X,
                                            current.Y / _spriteScale.Y + 2f);
                                        flag = true;
                                    }
                                }
                                else
                                {
                                    spriteObj2.Rotation = -90f;
                                    spriteObj2.Position = new(current.X / _spriteScale.X,
                                        (current.Y + current.Height) / _spriteScale.Y + 2f);
                                    flag = true;
                                }
                            }
                            else
                            {
                                spriteObj2.Position = new(room.Bounds.Right / _spriteScale.X - 5f,
                                    current.Y / _spriteScale.Y - 2f);
                                flag = true;
                            }
                        }
                        else
                        {
                            spriteObj2.Position =
                                new(room.Bounds.Left / _spriteScale.X - spriteObj2.Width + 2f,
                                    current.Y / _spriteScale.Y - 2f);
                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        _doorSpritePosList.Add(spriteObj2.Position);
                        _doorSpriteList.Add(spriteObj2);
                    }
                }

            if (room.Name != "Bonus" && Game.PlayerStats.Class != 13)
            {
                foreach (var current2 in room.GameObjList)
                {
                    var chestObj = current2 as ChestObj;
                    if (chestObj != null)
                    {
                        SpriteObj spriteObj3;
                        if (chestObj.IsOpen)
                        {
                            spriteObj3 = new("MapChestUnlocked_Sprite");
                        }
                        else if (chestObj is FairyChestObj)
                        {
                            spriteObj3 = new("MapFairyChestIcon_Sprite");
                            if ((chestObj as FairyChestObj).ConditionType == 10)
                            {
                                spriteObj3.Opacity = 0.2f;
                            }
                        }
                        else
                        {
                            spriteObj3 = new("MapLockedChestIcon_Sprite");
                        }

                        _iconSpriteList.Add(spriteObj3);
                        spriteObj3.AnimationDelay = 0.0333333351f;
                        spriteObj3.PlayAnimation();
                        spriteObj3.ForceDraw = true;
                        spriteObj3.Position = new(current2.X / _spriteScale.X - 8f,
                            current2.Y / _spriteScale.Y - 12f);
                        if (room.IsReversed)
                        {
                            spriteObj3.X -= current2.Width / _spriteScale.X;
                        }

                        _iconSpritePosList.Add(spriteObj3.Position);
                    }
                }
            }

            if (room.Name == "EntranceBoss")
            {
                var spriteObj4 = new SpriteObj("MapBossIcon_Sprite");
                spriteObj4.AnimationDelay = 0.0333333351f;
                spriteObj4.ForceDraw = true;
                spriteObj4.PlayAnimation();
                spriteObj4.Position = new(
                    (room.X + room.Width / 2f) / _spriteScale.X - spriteObj4.Width / 2 - 1f,
                    (room.Y + room.Height / 2f) / _spriteScale.Y - spriteObj4.Height / 2 - 2f);
                _iconSpriteList.Add(spriteObj4);
                _iconSpritePosList.Add(spriteObj4.Position);
                _teleporterList.Add(spriteObj4);
                _teleporterPosList.Add(spriteObj4.Position);
            }
            else if (room.Name == "Linker")
            {
                var spriteObj5 = new SpriteObj("MapTeleporterIcon_Sprite");
                spriteObj5.AnimationDelay = 0.0333333351f;
                spriteObj5.ForceDraw = true;
                spriteObj5.PlayAnimation();
                spriteObj5.Position = new(
                    (room.X + room.Width / 2f) / _spriteScale.X - spriteObj5.Width / 2 - 1f,
                    (room.Y + room.Height / 2f) / _spriteScale.Y - spriteObj5.Height / 2 - 2f);
                _iconSpriteList.Add(spriteObj5);
                _iconSpritePosList.Add(spriteObj5.Position);
                _teleporterList.Add(spriteObj5);
                _teleporterPosList.Add(spriteObj5.Position);
            }
            else if (room.Name == "CastleEntrance")
            {
                var spriteObj6 = new SpriteObj("MapTeleporterIcon_Sprite");
                spriteObj6.AnimationDelay = 0.0333333351f;
                spriteObj6.ForceDraw = true;
                spriteObj6.PlayAnimation();
                spriteObj6.Position = new(
                    (room.X + room.Width / 2f) / _spriteScale.X - spriteObj6.Width / 2 - 1f,
                    (room.Y + room.Height / 2f) / _spriteScale.Y - spriteObj6.Height / 2 - 2f);
                _iconSpriteList.Add(spriteObj6);
                _iconSpritePosList.Add(spriteObj6.Position);
                _teleporterList.Add(spriteObj6);
                _teleporterPosList.Add(spriteObj6.Position);
            }

            if (Game.PlayerStats.Class != 13 && room.Name == "Bonus")
            {
                var spriteObj7 = new SpriteObj("MapBonusIcon_Sprite");
                spriteObj7.PlayAnimation();
                spriteObj7.AnimationDelay = 0.0333333351f;
                spriteObj7.ForceDraw = true;
                spriteObj7.Position = new(
                    (room.X + room.Width / 2f) / _spriteScale.X - spriteObj7.Width / 2 - 1f,
                    (room.Y + room.Height / 2f) / _spriteScale.Y - spriteObj7.Height / 2 - 2f);
                _iconSpriteList.Add(spriteObj7);
                _iconSpritePosList.Add(spriteObj7.Position);
            }

            AddedRoomsList.Add(room);
        }
    }

    public void AddAllRooms(List<RoomObj> roomList)
    {
        foreach (var current in roomList) AddRoom(current);
    }

    public void AddAllIcons(List<RoomObj> roomList)
    {
        foreach (var current in roomList)
            if (!AddedRoomsList.Contains(current))
            {
                if (current.Name != "Bonus")
                {
                    using (var enumerator2 = current.GameObjList.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var current2 = enumerator2.Current;
                            var chestObj = current2 as ChestObj;
                            if (chestObj != null)
                            {
                                SpriteObj spriteObj;
                                if (chestObj.IsOpen)
                                {
                                    spriteObj = new("MapChestUnlocked_Sprite");
                                }
                                else if (chestObj is FairyChestObj)
                                {
                                    spriteObj = new("MapFairyChestIcon_Sprite");
                                    if ((chestObj as FairyChestObj).ConditionType == 10)
                                    {
                                        spriteObj.Opacity = 0.2f;
                                    }
                                }
                                else
                                {
                                    spriteObj = new("MapLockedChestIcon_Sprite");
                                }

                                _iconSpriteList.Add(spriteObj);
                                spriteObj.AnimationDelay = 0.0333333351f;
                                spriteObj.PlayAnimation();
                                spriteObj.ForceDraw = true;
                                spriteObj.Position = new(current2.X / _spriteScale.X - 8f,
                                    current2.Y / _spriteScale.Y - 12f);
                                if (current.IsReversed)
                                {
                                    spriteObj.X -= current2.Width / _spriteScale.X;
                                }

                                _iconSpritePosList.Add(spriteObj.Position);
                            }
                        }

                        continue;
                    }
                }

                if (current.Name == "Bonus")
                {
                    var spriteObj2 = new SpriteObj("MapBonusIcon_Sprite");
                    spriteObj2.PlayAnimation();
                    spriteObj2.AnimationDelay = 0.0333333351f;
                    spriteObj2.ForceDraw = true;
                    spriteObj2.Position =
                        new((current.X + current.Width / 2f) / _spriteScale.X - spriteObj2.Width / 2 - 1f,
                            (current.Y + current.Height / 2f) / _spriteScale.Y - spriteObj2.Height / 2 - 2f);
                    _iconSpriteList.Add(spriteObj2);
                    _iconSpritePosList.Add(spriteObj2.Position);
                }
            }
    }

    public void AddLinkerRoom(Zone zone, List<RoomObj> roomList)
    {
        foreach (var current in roomList)
            if (current.Name == "Linker" && current.Zone == zone)
            {
                AddRoom(current);
            }
    }

    public void RefreshChestIcons(RoomObj room)
    {
        foreach (var current in room.GameObjList)
        {
            var chestObj = current as ChestObj;
            if (chestObj != null && chestObj.IsOpen)
            {
                var pt = new Vector2(chestObj.X / _spriteScale.X - 8f, chestObj.Y / _spriteScale.Y - 12f);
                for (var i = 0; i < _iconSpritePosList.Count; i++)
                    if (CDGMath.DistanceBetweenPts(pt, _iconSpritePosList[i]) < 15f)
                    {
                        _iconSpriteList[i].ChangeSprite("MapChestUnlocked_Sprite");
                        _iconSpriteList[i].Opacity = 1f;
                        break;
                    }
            }
        }
    }

    public void CentreAroundPos(Vector2 pos, bool tween = false)
    {
        if (!tween)
        {
            CameraOffset.X = _alphaMaskRect.X + _alphaMaskRect.Width / 2f - pos.X / 1320f * 60f;
            CameraOffset.Y = _alphaMaskRect.Y + _alphaMaskRect.Height / 2f - pos.Y / 720f * 32f;
            return;
        }

        if (_xOffsetTween != null && _xOffsetTween.TweenedObject == this)
        {
            _xOffsetTween.StopTween(false);
        }

        if (_yOffsetTween != null && _yOffsetTween.TweenedObject == this)
        {
            _yOffsetTween.StopTween(false);
        }

        _xOffsetTween = Tween.To(this, 0.3f, Quad.EaseOut, "CameraOffsetX",
            (_alphaMaskRect.X + _alphaMaskRect.Width / 2f - pos.X / 1320f * 60f).ToString());
        _yOffsetTween = Tween.To(this, 0.3f, Quad.EaseOut, "CameraOffsetY",
            (_alphaMaskRect.Y + _alphaMaskRect.Height / 2f - pos.Y / 720f * 32f).ToString());
    }

    public void CentreAroundObj(GameObj obj)
    {
        CentreAroundPos(obj.Position);
    }

    public void CentreAroundPlayer()
    {
        CentreAroundObj(_player);
    }

    public void TeleportPlayer(int index)
    {
        if (_teleporterList.Count > 0)
        {
            var position = _teleporterPosList[index];
            position.X += 10f;
            position.Y += 10f;
            position.X *= _spriteScale.X;
            position.Y *= _spriteScale.Y;
            position.X += 30f;
            if (index == 0)
            {
                position.X = 3300;
                position.Y += 230f;
            }
            else
            {
                position.Y += 290f;
            }

            _player.TeleportPlayer(position);
        }
    }

    public void CentreAroundTeleporter(int index, bool tween = false)
    {
        var pos = _teleporterPosList[index];
        pos.X *= _spriteScale.X;
        pos.Y *= _spriteScale.Y;
        CentreAroundPos(pos, tween);
    }

    public void DrawRenderTargets(Camera2D camera)
    {
        if (FollowPlayer)
        {
            CameraOffset.X = (int) (_alphaMaskRect.X + _alphaMaskRect.Width / 2f - _player.X / 1320f * 60f);
            CameraOffset.Y = _alphaMaskRect.Y + _alphaMaskRect.Height / 2f - (int) _player.Y / 720f * 32f;
        }

        camera.GraphicsDevice.SetRenderTarget(_mapScreenRt);
        camera.GraphicsDevice.Clear(Color.Transparent);
        for (var i = 0; i < _roomSpriteList.Count; i++)
        {
            _roomSpriteList[i].Position = CameraOffset + _roomSpritePosList[i];
            _roomSpriteList[i].Draw(camera);
        }

        for (var j = 0; j < _doorSpriteList.Count; j++)
        {
            _doorSpriteList[j].Position = CameraOffset + _doorSpritePosList[j];
            _doorSpriteList[j].Draw(camera);
        }

        if (!DrawTeleportersOnly)
        {
            for (var k = 0; k < _iconSpriteList.Count; k++)
            {
                _iconSpriteList[k].Position = CameraOffset + _iconSpritePosList[k];
                _iconSpriteList[k].Draw(camera);
            }
        }
        else
        {
            for (var l = 0; l < _teleporterList.Count; l++)
            {
                _teleporterList[l].Position = CameraOffset + _teleporterPosList[l];
                _teleporterList[l].Draw(camera);
            }
        }

        if (Game.PlayerStats.Traits.X == 28f || Game.PlayerStats.Traits.Y == 28f)
        {
            _playerSprite.TextureColor = Color.Red;
            foreach (var current in AddedRoomsList)
            foreach (var current2 in current.EnemyList)
                if (!current2.IsKilled && !current2.IsDemented && current2.SaveToFile && current2.Type != 21 &&
                    current2.Type != 27 && current2.Type != 17)
                {
                    _playerSprite.Position =
                        new Vector2(current2.X / _spriteScale.X - 9f, current2.Y / _spriteScale.Y - 10f) +
                        CameraOffset;
                    _playerSprite.Draw(camera);
                }
        }

        _playerSprite.TextureColor = Color.White;
        _playerSprite.Position =
            new Vector2(_level.Player.X / _spriteScale.X - 9f, _level.Player.Y / _spriteScale.Y - 10f) +
            CameraOffset;
        _playerSprite.Draw(camera);
    }

    public override void Draw(Camera2D camera)
    {
        if (Visible)
        {
            camera.End();
            camera.GraphicsDevice.Textures[1] = _alphaMaskRt;
            camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                Game.MaskEffect);
            if (!DrawNothing)
            {
                camera.Draw(_mapScreenRt, Vector2.Zero, Color.White);
            }

            camera.End();
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                null);
            if (DrawNothing)
            {
                _playerSprite.Draw(camera);
            }
        }
    }

    public void ClearRoomsAdded()
    {
        AddedRoomsList.Clear();
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            _player = null;
            _level = null;
            if (_alphaMaskRt != null && !_alphaMaskRt.IsDisposed)
            {
                _alphaMaskRt.Dispose();
            }

            _alphaMaskRt = null;
            if (_mapScreenRt != null && !_mapScreenRt.IsDisposed)
            {
                _mapScreenRt.Dispose();
            }

            _mapScreenRt = null;
            foreach (var current in _roomSpriteList) current.Dispose();
            _roomSpriteList.Clear();
            _roomSpriteList = null;
            foreach (var current2 in _doorSpriteList) current2.Dispose();
            _doorSpriteList.Clear();
            _doorSpriteList = null;
            foreach (var current3 in _iconSpriteList) current3.Dispose();
            _iconSpriteList.Clear();
            _iconSpriteList = null;
            AddedRoomsList.Clear();
            AddedRoomsList = null;
            _roomSpritePosList.Clear();
            _roomSpritePosList = null;
            _doorSpritePosList.Clear();
            _doorSpritePosList = null;
            _iconSpritePosList.Clear();
            _iconSpritePosList = null;
            _playerSprite.Dispose();
            _playerSprite = null;
            foreach (var current4 in _teleporterList) current4.Dispose();
            _teleporterList.Clear();
            _teleporterList = null;
            _teleporterPosList.Clear();
            _teleporterPosList = null;
            _xOffsetTween = null;
            _yOffsetTween = null;
            base.Dispose();
        }
    }

    protected override GameObj CreateCloneInstance()
    {
        return new MapObj(FollowPlayer, _level);
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);
        var mapObj = obj as MapObj;
        mapObj.DrawTeleportersOnly = DrawTeleportersOnly;
        mapObj.CameraOffsetX = CameraOffsetX;
        mapObj.CameraOffsetY = CameraOffsetY;
        mapObj.InitializeAlphaMap(_mapScreenRt, _alphaMaskRt, _alphaMaskRect);
        mapObj.SetPlayer(_player);
        mapObj.AddAllRooms(AddedRoomsList);
    }

    public SpriteObj[] TeleporterList()
    {
        return _teleporterList.ToArray();
    }
}
