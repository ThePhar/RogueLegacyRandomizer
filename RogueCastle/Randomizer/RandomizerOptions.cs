//
//  Rogue Legacy Randomizer - RandomizerOptions.cs
//  Last Modified 2022-04-03
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using RogueCastle.Enums;

namespace RogueCastle.Randomizer
{
    public record RandomizerOptions
    {
        public bool       IsArchipelago         { get; private set; } = true;
        public string     StartingName          { get; private set; } = "Lee";
        public ClassType  StartingClass         { get; private set; } = ClassType.Knight;
        public Gender     StartingGender        { get; private set; } = Gender.Sir;
        public int        NewGamePlusLevel      { get; private set; } = 0;
        public int        ChestsPerZone         { get; private set; } = 20;
        public int        FairiesPerZone        { get; private set; } = 5;
        public UniChests  UniversalChests       { get; private set; } = UniChests.NoUniversal;
        public int        Diaries               { get; private set; } = 25;
        public int        ArchitectFee          { get; private set; } = 40;
        public bool       CharonFee             { get; private set; } = true;
        public bool       RequirePurchasing     { get; private set; } = true;
        public bool       ProgressiveBlueprints { get; private set; } = true;
        public Multiplier GoldGainMultiplier    { get; private set; } = Multiplier.Normal;
        public int        Children              { get; private set; } = 3;
        public string[]   SirNames              { get; private set; } = { };
        public string[]   LadyNames             { get; private set; } = { };
        public bool       AllowDefaultNames     { get; private set; } = true;
        public Multiplier CastleMultiplier      { get; private set; } = Multiplier.Normal;
        public Multiplier EnemyLevelMultiplier  { get; private set; } = Multiplier.Normal;
        public int        HealthPool            { get; private set; } = 15;
        public int        ManaPool              { get; private set; } = 15;
        public int        AttackPool            { get; private set; } = 15;
        public int        MagicDamagePool       { get; private set; } = 15;
        public int        ArmorPool             { get; private set; } = 10;
        public int        EquipPool             { get; private set; } = 10;
        public int        CriticalPool          { get; private set; } = 5;
        public bool       FreeStartingDiary     { get; private set; } = true;
        public bool       ArtificialGating      { get; private set; } = true;
    }
}
