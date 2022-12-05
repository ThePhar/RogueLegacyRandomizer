// Rogue Legacy Randomizer - ArenaBonusRoom.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Linq;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class ArenaBonusRoom : BonusRoomObj
{
    private ChestObj _chest;
    private bool     _chestRevealed;
    private float    _chestStartingY;

    public override void Initialize()
    {
        _chest = (ChestObj) GameObjList.First(obj => obj is ChestObj);
        _chest.ChestType = ChestType.Gold;
        _chestStartingY = _chest.Y - 200f + _chest.Height + 6f;
        base.Initialize();
    }

    public override void OnEnter()
    {
        UpdateEnemyNames();
        _chest.Y = _chestStartingY;
        _chest.ChestType = ChestType.Gold;
        if (RoomCompleted)
        {
            _chest.Opacity = 1f;
            _chest.Y = _chestStartingY + 200f;
            _chest.IsEmpty = true;
            _chest.ForceOpen();
            _chestRevealed = true;
            using var enumerator = EnemyList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current is { IsKilled: false })
                {
                    current.KillSilently();
                }
            }
        }
        else
        {
            if (ActiveEnemies == 0)
            {
                _chest.Opacity = 1f;
                _chest.Y = _chestStartingY + 200f;
                _chest.IsEmpty = false;
                _chest.IsLocked = false;
                _chestRevealed = true;
            }
            else
            {
                _chest.Opacity = 0f;
                _chest.Y = _chestStartingY;
                _chest.IsLocked = true;
                _chestRevealed = false;
            }
        }

        if (_chest.PhysicsMngr == null)
        {
            Player.PhysicsMngr.AddObject(_chest);
        }

        base.OnEnter();
    }

    private void UpdateEnemyNames()
    {
        var partnerNamed = false;
        foreach (var current in EnemyList)
        {
            switch (current)
            {
                case EnemyObj_EarthWizard when !partnerNamed:
                    current.Name = "Barbatos";
                    partnerNamed = true;
                    break;
                case EnemyObj_EarthWizard:
                    current.Name = "Amon";
                    break;

                case EnemyObj_Skeleton when !partnerNamed:
                    current.Name = "Berith";
                    partnerNamed = true;
                    break;
                case EnemyObj_Skeleton:
                    current.Name = "Halphas";
                    break;

                case EnemyObj_Plant when !partnerNamed:
                    current.Name = "Stolas";
                    partnerNamed = true;
                    break;
                case EnemyObj_Plant:
                    current.Name = "Focalor";
                    break;
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!_chest.IsOpen)
        {
            if (ActiveEnemies == 0 && _chest.Opacity == 0f && !_chestRevealed)
            {
                _chestRevealed = true;
                DisplayChest();
            }
        }
        else if (!RoomCompleted)
        {
            RoomCompleted = true;
        }

        base.Update(gameTime);
    }

    private void DisplayChest()
    {
        _chest.IsLocked = false;
        Tween.To(_chest, 2f, Tween.EaseNone, "Opacity", "1");
        Tween.By(_chest, 2f, Quad.EaseOut, "Y", "200");
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _chest = null;
        base.Dispose();
    }
}
