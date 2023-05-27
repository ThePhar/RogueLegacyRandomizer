// RogueLegacyRandomizer - AreaStruct.cs
// Last Modified 2023-05-27 1:53 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using Microsoft.Xna.Framework;
using RogueLegacy.Enums;

namespace RogueLegacy;

public struct AreaStruct
{
    public Color Color;
    public Color   MapColor;
    public Zone    Zone;
    public Vector2 BonusRooms;
    public Vector2 EnemyLevel;
    public Vector2 SecretRooms;
    public Vector2 TotalRooms;
    public bool    BossInArea;
    public bool    IsFinalArea;
    public bool    LinkToCastleOnly;
    public byte    BossType;
    public int     BossLevel;
    public int     EnemyLevelScale;
}