using Microsoft.Xna.Framework;
using RogueLegacy.Enums;

namespace RogueLegacy;

public struct AreaStruct
{
    public Color   Color;
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
