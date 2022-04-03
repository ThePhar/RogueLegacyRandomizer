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
        public bool       IsArchipelago         { get; set; } = false;
        public string     StartingName          { get; set; } = "Lee";
        public ClassType  StartingClass         { get; set; } = ClassType.Knight;
        public Gender     StartingGender        { get; set; } = Gender.Sir;
        public int        NewGamePlusLevel      { get; set; } = 0;
        public int        ChestsPerZone         { get; set; } = 20;
        public int        FairiesPerZone        { get; set; } = 5;
        public UniChests  UniversalChests       { get; set; } = UniChests.NoUniversal;
        public int        Diaries               { get; set; } = 25;
        public int        ArchitectFee          { get; set; } = 40;
        public bool       CharonFee             { get; set; } = true;
        public bool       RequirePurchasing     { get; set; } = true;
        public bool       ProgressiveBlueprints { get; set; } = true;
        public Multiplier GoldGainMultiplier    { get; set; } = Multiplier.Normal;
        public int        Children              { get; set; } = 3;
        public string[]   SirNames              { get; set; } = { };
        public string[]   LadyNames             { get; set; } = { };
        public bool       AllowDefaultNames     { get; set; } = true;
        public Multiplier CastleMultiplier      { get; set; } = Multiplier.Normal;
        public Multiplier EnemyLevelMultiplier  { get; set; } = Multiplier.Normal;
        public int        HealthPool            { get; set; } = 15;
        public int        ManaPool              { get; set; } = 15;
        public int        AttackPool            { get; set; } = 15;
        public int        MagicDamagePool       { get; set; } = 15;
        public int        ArmorPool             { get; set; } = 10;
        public int        EquipPool             { get; set; } = 10;
        public int        CriticalPool          { get; set; } = 5;
        public bool       FreeStartingDiary     { get; set; } = true;
        public bool       ArtificialGating      { get; set; } = true;
    }
}
