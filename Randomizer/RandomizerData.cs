// RogueLegacyRandomizer - RandomizerData.cs
// Last Modified 2023-07-30 8:18 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueLegacy;
using RogueLegacy.Enums;

namespace Randomizer;

public class RandomizerData
{
    private readonly Dictionary<string, object> _settings;

    public RandomizerData(IDictionary<string, object> settings, string seed, int slot = 1)
    {
        _settings = new Dictionary<string, object>(settings);
        Seed = seed;
        Slot = slot;

        // Convert properties appropriately if we're using the old version.
        _settings["world_version"] = GetValueOrNull("world_version") ?? 1;
        if ((long) _settings["world_version"] == 1)
        {
            // Starting Gender
            _settings["starting_gender"] = Convert.ToInt32(_settings["starting_gender"]) == 0 ? Gender.Sir : Gender.Lady;

            // Starting Class
            _settings["starting_class"] = Convert.ToByte(_settings["starting_class"]);

            // New Game Plus
            _settings["new_game_plus"] = Convert.ToInt32(_settings["new_game_plus"]);

            // Vendor Purchasing
            _settings["require_vendor_purchasing"] = Convert.ToBoolean(_settings["require_purchasing"]);

            // Skill Purchasing
            _settings["require_skill_purchasing"] = false;

            // Shuffle Blacksmith & Enchantress
            _settings["shuffle_blacksmith"] = false;
            _settings["shuffle_enchantress"] = false;

            // Gold Multiplier
            _settings["gold_gain_multiplier"] = Convert.ToInt32(_settings["gold_gain_multiplier"]) switch
            {
                0 => 1f,    // Normal
                1 => 0.25f, // Quarter
                2 => 0.5f,  // Half
                3 => 2f,    // Double
                4 => 4f,    // Quadruple
                _ => throw new ArgumentException($"Unexpected Gold Multiplier!")
            };

            // Spending Restrictions
            _settings["spending_restrictions"] = false;

            // Number of Children
            _settings["number_of_children"] = Convert.ToInt32(_settings["number_of_children"]);

            // Free Diary
            _settings["free_diary_per_generation"] = Convert.ToBoolean(_settings["free_diary_on_generation"]);

            // Bosses
            _settings["challenge_khidr"] = Convert.ToBoolean(_settings["khidr"]);
            _settings["challenge_alexander"] = Convert.ToBoolean(_settings["alexander"]);
            _settings["challenge_leon"] = Convert.ToBoolean(_settings["leon"]);
            _settings["challenge_herodotus"] = Convert.ToBoolean(_settings["herodotus"]);

            // Castle Size
            _settings["castle_size"] = PossibleCastleSizes[0];

            // Fountain Piece Requirement
            _settings["fountain_piece_requirement"] = 0;

            // Require Bosses
            _settings["require_bosses"] = true;

            // DeathLink
            _settings["death_link"] = Convert.ToInt32(_settings["death_link"]) == 0 ? DeathLinkMode.Disabled : DeathLinkMode.Enabled;
        }
        else // World Version 2
        {
            // Starting Gender
            _settings["starting_gender"] = Convert.ToString(_settings["starting_gender"]) == "sir" ? Gender.Sir : Gender.Lady;

            // Starting Class
            _settings["starting_class"] = Convert.ToString(_settings["starting_class"]) switch
            {
                "knight"     => (byte) 0,
                "mage"       => (byte) 1,
                "barbarian"  => (byte) 2,
                "knave"      => (byte) 3,
                "shinobi"    => (byte) 4,
                "miner"      => (byte) 5,
                "spellthief" => (byte) 6,
                "lich"       => (byte) 7,
                "dragon"     => (byte) 16,
                "traitor"    => (byte) 17,
                _ => throw new ArgumentException($"Unexpected Starting Class!"),
            };

            // New Game Plus
            _settings["new_game_plus"] = Convert.ToBoolean(_settings["new_game_plus"]) ? 1 : 0;

            // Vendor Purchasing
            _settings["require_vendor_purchasing"] = Convert.ToBoolean(_settings["require_vendor_purchasing"]);

            // Skill Purchasing
            _settings["require_skill_purchasing"] = Convert.ToBoolean(_settings["require_skill_purchasing"]);

            // Shuffle Blacksmith & Enchantress
            _settings["shuffle_blacksmith"] = Convert.ToBoolean(_settings["shuffle_blacksmith"]);
            _settings["shuffle_enchantress"] = Convert.ToBoolean(_settings["shuffle_enchantress"]);

            // Gold Multiplier
            _settings["gold_gain_multiplier"] = Convert.ToString(_settings["gold_gain_multiplier"]) switch
            {
                "normal"    => 1f,
                "quarter"   => 0.25f,
                "half"      => 0.5f,
                "double"    => 2f,
                "quadruple" => 4f,
                _ => throw new ArgumentException($"Unexpected Gold Multiplier!")
            };

            // Spending Restrictions
            _settings["spending_restrictions"] = Convert.ToBoolean(_settings["spending_restrictions"]);

            // Number of Children
            _settings["number_of_children"] = Convert.ToString(_settings["number_of_children"]) switch
            {
                "variable" => 0,
                "one"      => 1,
                "two"      => 2,
                "three"    => 3,
                "four"     => 4,
                "five"     => 5,
                _          => throw new ArgumentException($"Unexpected Amount of Children!"),
            };

            // Free Diary
            _settings["free_diary_per_generation"] = Convert.ToBoolean(_settings["free_diary_per_generation"]);

            // Bosses
            _settings["challenge_khidr"] = Convert.ToBoolean(_settings["challenge_khidr"]);
            _settings["challenge_alexander"] = Convert.ToBoolean(_settings["challenge_alexander"]);
            _settings["challenge_leon"] = Convert.ToBoolean(_settings["challenge_leon"]);
            _settings["challenge_herodotus"] = Convert.ToBoolean(_settings["challenge_herodotus"]);

            // Castle Size
            _settings["castle_size"] = Convert.ToString(_settings["castle_size"]) switch
            {
                "standard"   => PossibleCastleSizes[0],
                "large"      => PossibleCastleSizes[1],
                "very_large" => PossibleCastleSizes[2],
                "labyrinth"  => PossibleCastleSizes[3],
                _            => throw new ArgumentException($"Unexpected Castle Size!"),
            };

            // Fountain Piece Requirement
            _settings["fountain_piece_requirement"] = Convert.ToInt32(_settings["fountain_piece_requirement"]);

            // Require Bosses
            _settings["require_bosses"] = Convert.ToBoolean(_settings["require_bosses"]);

            // DeathLink
            _settings["death_link"] = Convert.ToString(_settings["death_link"]) switch
            {
                "disabled"        => DeathLinkMode.Disabled,
                "enabled"         => DeathLinkMode.Enabled,
                "forced_disabled" => DeathLinkMode.ForcedDisabled,
                "forced_enabled"  => DeathLinkMode.ForcedEnabled,
                _                 => throw new ArgumentException($"Unexpected DeathLink Mode!"),
            };
        }

        // All settings that are universal between world version 1 and 2.
        _settings["chests_per_zone"] = Convert.ToInt32(_settings["chests_per_zone"]);
        _settings["fairy_chests_per_zone"] = Convert.ToInt32(_settings["fairy_chests_per_zone"]);
        _settings["universal_chests"] = Convert.ToBoolean(_settings["universal_chests"]);
        _settings["universal_fairy_chests"] = Convert.ToBoolean(_settings["universal_fairy_chests"]);
        _settings["architect_fee"] = Convert.ToInt32(_settings["architect_fee"]);
        _settings["disable_charon"] = Convert.ToBoolean(_settings["disable_charon"]);
    }

    private object GetValueOrNull(string key)
    {
        var exists = _settings.TryGetValue(key, out var value);
        return exists ? value : null;
    }

    public string                        Seed             { get; }
    public int                           Slot             { get; }

    public int           WorldVersion             => (int)           _settings["world_version"];
    public Gender        StartingGender           => (Gender)        _settings["starting_gender"];
    public byte          StartingClass            => (byte)          _settings["starting_class"];
    public int           NewGamePlus              => (int)           _settings["new_game_plus"];
    public int           ChestsPerZone            => (int)           _settings["chests_per_zone"];
    public int           FairyChestsPerZone       => (int)           _settings["fairy_chests_per_zone"];
    public bool          UniversalChests          => (bool)          _settings["universal_chests"];
    public bool          UniversalFairyChests     => (bool)          _settings["universal_fairy_chests"];
    public int           ArchitectFee             => (int)           _settings["architect_fee"];
    public bool          DisableCharon            => (bool)          _settings["disable_charon"];
    public bool          RequireVendorPurchasing  => (bool)          _settings["require_vendor_purchasing"];
    public bool          RequireSkillPurchasing   => (bool)          _settings["require_skill_purchasing"];
    public bool          ShuffleBlacksmith        => (bool)          _settings["shuffle_blacksmith"];
    public bool          ShuffleEnchantress       => (bool)          _settings["shuffle_enchantress"];
    public float         GoldGainMultiplier       => (float)         _settings["gold_gain_multiplier"];
    public bool          SpendingRestrictions     => (bool)          _settings["spending_restrictions"];
    public int           NumberOfChildren         => (int)           _settings["number_of_children"];
    public bool          FreeDiaryOnGeneration    => (bool)          _settings["free_diary_per_generation"];
    public bool          ChallengeKhidr           => (bool)          _settings["challenge_khidr"];
    public bool          ChallengeAlexander       => (bool)          _settings["challenge_alexander"];
    public bool          ChallengeLeon            => (bool)          _settings["challenge_leon"];
    public bool          ChallengeHerodotus       => (bool)          _settings["challenge_herodotus"];
    public AreaStruct[]  AreaStructs              => (AreaStruct[])  _settings["castle_size"];
    public int           FountainPieceRequirement => (int)           _settings["fountain_piece_requirement"];
    public bool          RequireBosses            => (bool)          _settings["require_bosses"];
    public DeathLinkMode DeathLinkMode            => (DeathLinkMode) _settings["death_link"];
    public bool          CanToggleDeathLink       => DeathLinkMode is DeathLinkMode.Disabled or DeathLinkMode.Enabled;

    private static AreaStruct[][] PossibleCastleSizes => new[]
    {
        // Standard
        new[]
        {
            new AreaStruct
            {
                Zone = Zone.Castle,
                TotalRooms = new Vector2(30f, 35f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(4f, 5f),
                Color = Color.White
            },
            new AreaStruct
            {
                Zone = Zone.Garden,
                TotalRooms = new Vector2(28f, 30f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(4f, 5f),
                Color = Color.Green
            },
            new AreaStruct
            {
                Zone = Zone.Tower,
                TotalRooms = new Vector2(28f, 30f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(4f, 5f),
                Color = Color.DarkBlue
            },
            new AreaStruct
            {
                Zone = Zone.Dungeon,
                TotalRooms = new Vector2(28f, 30f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(4f, 5f),
                Color = Color.Red
            }
        },

        // Large
        new[]
        {
            new AreaStruct
            {
                Zone = Zone.Castle,
                TotalRooms = new Vector2(30f * 2, 35f * 2),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 2, 3f * 2),
                BonusRooms = new Vector2(4f * 2, 5f * 2),
                Color = Color.White
            },
            new AreaStruct
            {
                Zone = Zone.Garden,
                TotalRooms = new Vector2(28f * 2, 30f * 2),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 2, 3f * 2),
                BonusRooms = new Vector2(4f * 2, 5f * 2),
                Color = Color.Green
            },
            new AreaStruct
            {
                Zone = Zone.Tower,
                TotalRooms = new Vector2(28f * 2, 30f * 2),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 2, 3f * 2),
                BonusRooms = new Vector2(4f * 2, 5f * 2),
                Color = Color.DarkBlue
            },
            new AreaStruct
            {
                Zone = Zone.Dungeon,
                TotalRooms = new Vector2(28f * 2, 30f * 2),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 2, 3f * 2),
                BonusRooms = new Vector2(4f * 2, 5f * 2),
                Color = Color.Red
            }
        },

        // Very Large
        new[]
        {
            new AreaStruct
            {
                Zone = Zone.Castle,
                TotalRooms = new Vector2(30f * 3, 35f * 3),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 3, 3f * 3),
                BonusRooms = new Vector2(4f * 3, 5f * 3),
                Color = Color.White
            },
            new AreaStruct
            {
                Zone = Zone.Garden,
                TotalRooms = new Vector2(28f * 3, 30f * 3),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 3, 3f * 3),
                BonusRooms = new Vector2(4f * 3, 5f * 3),
                Color = Color.Green
            },
            new AreaStruct
            {
                Zone = Zone.Tower,
                TotalRooms = new Vector2(28f * 3, 30f * 3),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 3, 3f * 3),
                BonusRooms = new Vector2(4f * 3, 5f * 3),
                Color = Color.DarkBlue
            },
            new AreaStruct
            {
                Zone = Zone.Dungeon,
                TotalRooms = new Vector2(28f * 3, 30f * 3),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 3, 3f * 3),
                BonusRooms = new Vector2(4f * 3, 5f * 3),
                Color = Color.Red
            }
        },

        new[]
        {
            new AreaStruct
            {
                Zone = Zone.Castle,
                TotalRooms = new Vector2(30f * 4, 35f * 4),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 4, 3f * 4),
                BonusRooms = new Vector2(4f * 4, 5f * 4),
                Color = Color.White
            },
            new AreaStruct
            {
                Zone = Zone.Garden,
                TotalRooms = new Vector2(28f * 4, 30f * 4),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 4, 3f * 4),
                BonusRooms = new Vector2(4f * 4, 5f * 4),
                Color = Color.Green
            },
            new AreaStruct
            {
                Zone = Zone.Tower,
                TotalRooms = new Vector2(28f * 4, 30f * 4),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 4, 3f * 4),
                BonusRooms = new Vector2(4f * 4, 5f * 4),
                Color = Color.DarkBlue
            },
            new AreaStruct
            {
                Zone = Zone.Dungeon,
                TotalRooms = new Vector2(28f * 4, 30f * 4),
                BossInArea = true,
                SecretRooms = new Vector2(1f * 4, 3f * 4),
                BonusRooms = new Vector2(4f * 4, 5f * 4),
                Color = Color.Red
            }
        }
    };
}