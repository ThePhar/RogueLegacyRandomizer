//  RogueLegacyRandomizer - SaveGameManager.cs
//  Last Modified 2023-10-26 2:32 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Randomizer;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy;

public class SaveGameManager
{
    public const byte SAVE_VERSION = 2;

    private const    string EXT               = "rcrdat";
    private readonly string _fileNameLineage  = $"{LevelENV.GameName.Replace(" ", "")}_Lineage.{EXT}";
    private readonly string _fileNameMap      = $"{LevelENV.GameName.Replace(" ", "")}_Map.{EXT}";
    private readonly string _fileNameMapData  = $"{LevelENV.GameName.Replace(" ", "")}_MapDat.{EXT}";
    private readonly string _fileNamePlayer   = $"{LevelENV.GameName.Replace(" ", "")}_Player.{EXT}";
    private readonly string _fileNameUpgrades = $"{LevelENV.GameName.Replace(" ", "")}_BP.{EXT}";

    private bool             _autosaveLoaded;
    private int              _saveFailCounter;
    private StorageContainer _storageContainer;

    public void Initialize()
    {
        if (_storageContainer != null)
        {
            _storageContainer.Dispose();
            _storageContainer = null;
        }

        PerformDirectoryCheck();
    }

    public void CreateSaveDirectory()
    {
        GetStorageContainer();
        if (!_storageContainer.DirectoryExists($"Profile_{Game.ProfileName}"))
        {
            _storageContainer.CreateDirectory($"Profile_{Game.ProfileName}");
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void SaveFiles(params SaveType[] saveList)
    {
        if (LevelENV.DisableSaving)
        {
            RandUtil.Console("Rogue Legacy", "A save was requested, but saving is disabled.");
            return;
        }

        GetStorageContainer();
        try
        {
            foreach (var saveType in saveList)
            {
                SaveData(saveType, false);
            }

            _saveFailCounter = 0;
        }
        catch
        {
            if (_saveFailCounter > 2)
            {
                var screenManager = Game.ScreenManager;
                screenManager.DialogueScreen.SetDialogue("Save File Error Antivirus");
                Tween.RunFunction(0.25f, screenManager, "DisplayScreen", 13, true, typeof(List<object>));
                _saveFailCounter = 0;
            }
            else
            {
                _saveFailCounter++;
            }
        }
        finally
        {
            if (_storageContainer is { IsDisposed: false })
            {
                _storageContainer.Dispose();
            }

            _storageContainer = null;
        }
    }

    public void SaveBackupFiles(params SaveType[] saveList)
    {
        if (LevelENV.DisableSaving)
        {
            RandUtil.Console("Rogue Legacy", "A backup save was requested, but saving is disabled.");
            return;
        }

        GetStorageContainer();
        foreach (var saveType in saveList)
        {
            SaveData(saveType, true);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void SaveAllFileTypes(bool saveBackup)
    {
        if (!saveBackup)
        {
            SaveFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
            return;
        }

        SaveBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
    }

    public void LoadFiles(ProceduralLevelScreen level, params SaveType[] loadList)
    {
        if (LevelENV.EnableBackupSaving)
        {
            GetStorageContainer();
            var saveType = SaveType.None;
            try
            {
                try
                {
                    if (!LevelENV.DisableSaving)
                    {
                        foreach (var saveTypeToLoad in loadList)
                        {
                            saveType = saveTypeToLoad;
                            LoadData(saveTypeToLoad, level);
                        }
                    }
                }
                catch
                {
                    if (saveType is SaveType.Map or SaveType.MapData or SaveType.None)
                    {
                        throw new();
                    }

                    if (!_autosaveLoaded)
                    {
                        var screenManager = Game.ScreenManager;
                        screenManager.DialogueScreen.SetDialogue("Save File Error");
                        screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
                        screenManager.DisplayScreen(13, false);
                        Game.PlayerStats.HeadPiece = 0;
                    }
                    else
                    {
                        _autosaveLoaded = false;
                        var screenManager2 = Game.ScreenManager;
                        screenManager2.DialogueScreen.SetDialogue("Save File Error 2");
                        screenManager2.DialogueScreen.SetConfirmEndHandler(this, "StartNewGame");
                        screenManager2.DisplayScreen(13, false);
                        Game.PlayerStats.HeadPiece = 0;
                    }
                }

                return;
            }
            finally
            {
                if (_storageContainer is { IsDisposed: false })
                {
                    _storageContainer.Dispose();
                }
            }
        }

        if (LevelENV.DisableSaving)
        {
            return;
        }

        GetStorageContainer();
        foreach (var saveType in loadList)
        {
            LoadData(saveType, level);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void ForceBackup()
    {
        if (_storageContainer is { IsDisposed: false })
        {
            _storageContainer.Dispose();
        }

        var screenManager = Game.ScreenManager;
        screenManager.DialogueScreen.SetDialogue("Save File Error");
        screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
        screenManager.DisplayScreen(13, false);
    }

    public void LoadAutosave()
    {
        RandUtil.Console("Rogue Legacy","Save file corrupted.");
        SkillSystem.ResetAllTraits();
        Game.PlayerStats.Dispose();
        Game.PlayerStats = new();
        Game.ScreenManager.Player.Reset();
        LoadBackups();
        Game.ScreenManager.DisplayScreen(3, true);
    }

    public void StartNewGame()
    {
        ClearAllFileTypes(false);
        ClearAllFileTypes(true);
        SkillSystem.ResetAllTraits();
        Game.PlayerStats.Dispose();
        Game.PlayerStats = new();
        Game.ScreenManager.Player.Reset();
        Game.ScreenManager.DisplayScreen(23, true);
    }

    public void ResetAutosave()
    {
        _autosaveLoaded = false;
    }

    public void LoadAllFileTypes(ProceduralLevelScreen level)
    {
        LoadFiles(level, SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
    }

    public void ClearAllFileTypes(bool deleteBackups)
    {
        if (!deleteBackups)
        {
            ClearFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
            return;
        }

        ClearBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
    }

    private void DeleteData(SaveType deleteType)
    {
        switch (deleteType)
        {
            case SaveType.PlayerData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNamePlayer}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/{_fileNamePlayer}");
                }

                break;

            case SaveType.UpgradeData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameUpgrades}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/{_fileNameUpgrades}");
                }

                break;

            case SaveType.Map:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMap}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/{_fileNameMap}");
                }

                break;

            case SaveType.MapData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMapData}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/{_fileNameMapData}");
                }

                break;

            case SaveType.Lineage:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameLineage}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/{_fileNameLineage}");
                }

                break;
        }

        RandUtil.Console("Rogue Legacy", $"Save file type {deleteType} deleted.");
    }

    private void DeleteBackupData(SaveType deleteType)
    {
        switch (deleteType)
        {
            case SaveType.PlayerData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNamePlayer}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNamePlayer}");
                }

                break;

            case SaveType.UpgradeData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameUpgrades}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameUpgrades}");
                }

                break;

            case SaveType.Map:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMap}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMap}");
                }

                break;

            case SaveType.MapData:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMapData}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMapData}");
                }

                break;

            case SaveType.Lineage:
                if (_storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameLineage}"))
                {
                    _storageContainer.DeleteFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameLineage}");
                }

                break;
        }

        RandUtil.Console("Rogue Legacy", $"Backup save file type {deleteType} deleted.");
    }

    private void LoadBackups()
    {
        RandUtil.Console("Rogue Legacy", "Replacing save file with back up saves.");
        GetStorageContainer();
        if (
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNamePlayer}") &&
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNamePlayer}"))
        {
            var autoSave = _storageContainer.OpenFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNamePlayer}", FileMode.Open);
            var newSave = _storageContainer.CreateFile($"Profile_{Game.ProfileName}/{_fileNamePlayer}");
            autoSave.CopyTo(newSave);
            autoSave.Close();
            newSave.Close();
        }

        if (
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameUpgrades}") &&
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameUpgrades}"))
        {
            var autoSave = _storageContainer.OpenFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameUpgrades}", FileMode.Open);
            var newSave = _storageContainer.CreateFile($"Profile_{Game.ProfileName}/{_fileNameUpgrades}");
            autoSave.CopyTo(newSave);
            autoSave.Close();
            newSave.Close();
        }

        if (
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMap}") &&
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMap}"))
        {
            var autoSave = _storageContainer.OpenFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMap}", FileMode.Open);
            var newSave = _storageContainer.CreateFile($"Profile_{Game.ProfileName}/{_fileNameMap}");
            autoSave.CopyTo(newSave);
            autoSave.Close();
            newSave.Close();
        }

        if (
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMapData}") &&
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMapData}"))
        {
            var autoSave = _storageContainer.OpenFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameMapData}", FileMode.Open);
            var save = _storageContainer.CreateFile($"Profile_{Game.ProfileName}/{_fileNameMapData}");
            autoSave.CopyTo(save);
            autoSave.Close();
            save.Close();
        }

        if (
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/AutoSave_{_fileNameLineage}") &&
            _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameLineage}"))
        {
            var autoSave = _storageContainer.OpenFile($"Profile_{Game.ProfileName}/AutoSave_{_fileNameLineage}", FileMode.Open);
            var save = _storageContainer.CreateFile($"Profile_{Game.ProfileName}/{_fileNameLineage}");
            autoSave.CopyTo(save);
            autoSave.Close();
            save.Close();
        }

        _autosaveLoaded = true;
        _storageContainer.Dispose();
        _storageContainer = null;
    }

    private void SaveData(SaveType saveType, bool saveBackup)
    {
        switch (saveType)
        {
            case SaveType.PlayerData:
                SavePlayerData(saveBackup);
                break;

            case SaveType.UpgradeData:
                SaveUpgradeData(saveBackup);
                break;

            case SaveType.Map:
                SaveMap(saveBackup);
                break;

            case SaveType.MapData:
                SaveMapData(saveBackup);
                break;

            case SaveType.Lineage:
                SaveLineageData(saveBackup);
                break;
        }

        RandUtil.Console("Rogue Legacy", $"Data type {saveType} saved!");
    }

    private void SavePlayerData(bool saveBackup)
    {
        var savefile = _fileNamePlayer;
        if (saveBackup)
        {
            savefile = savefile.Insert(0, "AutoSave_");
        }

        savefile = savefile.Insert(0, $"Profile_{Game.ProfileName}/");
        using var stream = _storageContainer.CreateFile(savefile);
        using var writer = new BinaryWriter(stream);

        // Set some variables for preparation for saving.
        Game.PlayerStats.CurrentHealth = Game.ScreenManager.Player.CurrentHealth;
        Game.PlayerStats.CurrentMana = (int) Game.ScreenManager.Player.CurrentMana;

        // Save version number;
        writer.Write(SAVE_VERSION);

        // PlayerStats
        writer.Write(Game.ProfileName);
        writer.Write(Game.PlayerStats.Gold);
        writer.Write(Game.PlayerStats.CurrentHealth);
        writer.Write(Game.PlayerStats.CurrentMana);
        writer.Write(Game.PlayerStats.Age);
        writer.Write(Game.PlayerStats.ChildAge);
        writer.Write(Game.PlayerStats.Spell);
        writer.Write(Game.PlayerStats.Class);
        writer.Write(Game.PlayerStats.SpecialItem);
        writer.Write((byte) Game.PlayerStats.Traits.X);
        writer.Write((byte) Game.PlayerStats.Traits.Y);
        writer.Write(Game.PlayerStats.PlayerName);
        writer.Write(Game.PlayerStats.HeadPiece);
        writer.Write(Game.PlayerStats.ShoulderPiece);
        writer.Write(Game.PlayerStats.ChestPiece);
        writer.Write(Game.PlayerStats.DiaryEntry);
        writer.Write(Game.PlayerStats.BonusHealth);
        writer.Write(Game.PlayerStats.BonusStrength);
        writer.Write(Game.PlayerStats.BonusMana);
        writer.Write(Game.PlayerStats.BonusDefense);
        writer.Write(Game.PlayerStats.BonusWeight);
        writer.Write(Game.PlayerStats.BonusMagic);
        writer.Write(Game.PlayerStats.LichHealth);
        writer.Write(Game.PlayerStats.LichMana);
        writer.Write(Game.PlayerStats.LichHealthMod);
        writer.Write(Game.PlayerStats.NewBossBeaten);
        writer.Write(Game.PlayerStats.EyeballBossBeaten);
        writer.Write(Game.PlayerStats.FairyBossBeaten);
        writer.Write(Game.PlayerStats.FireballBossBeaten);
        writer.Write(Game.PlayerStats.BlobBossBeaten);
        writer.Write(Game.PlayerStats.LastbossBeaten);
        writer.Write(Game.PlayerStats.TimesCastleBeaten);
        writer.Write(Game.PlayerStats.NumEnemiesBeaten);
        writer.Write(Game.PlayerStats.TutorialComplete);
        writer.Write(Game.PlayerStats.CharacterFound);
        writer.Write(Game.PlayerStats.LoadStartingRoom);
        writer.Write(Game.PlayerStats.LockCastle);
        writer.Write(Game.PlayerStats.SpokeToBlacksmith);
        writer.Write(Game.PlayerStats.SpokeToEnchantress);
        writer.Write(Game.PlayerStats.SpokeToArchitect);
        writer.Write(Game.PlayerStats.SpokeToTollCollector);
        writer.Write(Game.PlayerStats.IsDead);
        writer.Write(Game.PlayerStats.FinalDoorOpened);
        writer.Write(Game.PlayerStats.RerolledChildren);
        writer.Write(Game.PlayerStats.IsFemale);
        writer.Write(Game.PlayerStats.TimesDead);
        writer.Write(Game.PlayerStats.HasArchitectFee);
        writer.Write(Game.PlayerStats.ReadLastDiary);
        writer.Write(Game.PlayerStats.ReadStartingDiary);
        writer.Write(Game.PlayerStats.SpokenToLastBoss);
        writer.Write(Game.PlayerStats.HardcoreMode);
        writer.Write(Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength);
        writer.Write((byte) Game.PlayerStats.WizardSpellList.X);
        writer.Write((byte) Game.PlayerStats.WizardSpellList.Y);
        writer.Write((byte) Game.PlayerStats.WizardSpellList.Z);

        // Received Items from Archipelago
        writer.Write(Program.Game.NextChildItemQueue.Count);
        foreach (var item in Program.Game.NextChildItemQueue)
        {
            writer.Write(item.Item);
            writer.Write(item.Player);
            writer.Write(item.Location);
            writer.Write((int) item.Flags);
        }

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", "Saving Player Stats");
            RandUtil.Console("Rogue Legacy", $"Profile Name: {Game.ProfileName}");
            RandUtil.Console("Rogue Legacy", $"Gold: {Game.PlayerStats.Gold}");
            RandUtil.Console("Rogue Legacy", $"Current Health: {Game.PlayerStats.CurrentHealth}");
            RandUtil.Console("Rogue Legacy", $"Current Mana: {Game.PlayerStats.CurrentMana}");
            RandUtil.Console("Rogue Legacy", $"Age: {Game.PlayerStats.Age}");
            RandUtil.Console("Rogue Legacy", $"Child Age: {Game.PlayerStats.ChildAge}");
            RandUtil.Console("Rogue Legacy", $"Spell: {Game.PlayerStats.Spell}");
            RandUtil.Console("Rogue Legacy", $"Class: {Game.PlayerStats.Class}");
            RandUtil.Console("Rogue Legacy", $"Special Item: {Game.PlayerStats.SpecialItem}");
            RandUtil.Console("Rogue Legacy", $"Traits: {Game.PlayerStats.Traits.X}, {Game.PlayerStats.Traits.Y}");
            RandUtil.Console("Rogue Legacy", $"Name: {Game.PlayerStats.PlayerName}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Head Piece: {Game.PlayerStats.HeadPiece}");
            RandUtil.Console("Rogue Legacy", $"Shoulder Piece: {Game.PlayerStats.ShoulderPiece}");
            RandUtil.Console("Rogue Legacy", $"Chest Piece: {Game.PlayerStats.ChestPiece}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Diary Entry: {Game.PlayerStats.DiaryEntry}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Bonus Health: {Game.PlayerStats.BonusHealth}");
            RandUtil.Console("Rogue Legacy", $"Bonus Strength: {Game.PlayerStats.BonusStrength}");
            RandUtil.Console("Rogue Legacy", $"Bonus Mana: {Game.PlayerStats.BonusMana}");
            RandUtil.Console("Rogue Legacy", $"Bonus Armor: {Game.PlayerStats.BonusDefense}");
            RandUtil.Console("Rogue Legacy", $"Bonus Weight: {Game.PlayerStats.BonusWeight}");
            RandUtil.Console("Rogue Legacy", $"Bonus Magic: {Game.PlayerStats.BonusMagic}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Lich Health: {Game.PlayerStats.LichHealth}");
            RandUtil.Console("Rogue Legacy", $"Lich Mana: {Game.PlayerStats.LichMana}");
            RandUtil.Console("Rogue Legacy", $"Lich Health Mod: {Game.PlayerStats.LichHealthMod}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"New Boss Beaten: {Game.PlayerStats.NewBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Eyeball Boss Beaten: {Game.PlayerStats.EyeballBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fairy Boss Beaten: {Game.PlayerStats.FairyBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fireball Boss Beaten: {Game.PlayerStats.FireballBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Blob Boss Beaten: {Game.PlayerStats.BlobBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Beaten: {Game.PlayerStats.LastbossBeaten}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Times Castle Beaten: {Game.PlayerStats.TimesCastleBeaten}");
            RandUtil.Console("Rogue Legacy", $"Number of Enemies Beaten: {Game.PlayerStats.NumEnemiesBeaten}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Tutorial Complete: {Game.PlayerStats.TutorialComplete}");
            RandUtil.Console("Rogue Legacy", $"Character Found: {Game.PlayerStats.CharacterFound}");
            RandUtil.Console("Rogue Legacy", $"Load Starting Room: {Game.PlayerStats.LoadStartingRoom}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Spoke to Blacksmith: {Game.PlayerStats.SpokeToBlacksmith}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Enchantress: {Game.PlayerStats.SpokeToEnchantress}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Architect: {Game.PlayerStats.SpokeToArchitect}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Toll Collector: {Game.PlayerStats.SpokeToTollCollector}");
            RandUtil.Console("Rogue Legacy", $"Player Is Dead: {Game.PlayerStats.IsDead}");
            RandUtil.Console("Rogue Legacy", $"Final Door Opened: {Game.PlayerStats.FinalDoorOpened}");
            RandUtil.Console("Rogue Legacy", $"Rerolled Children: {Game.PlayerStats.RerolledChildren}");
            RandUtil.Console("Rogue Legacy", $"Is Female: {Game.PlayerStats.IsFemale}");
            RandUtil.Console("Rogue Legacy", $"Times Dead: {Game.PlayerStats.TimesDead}");
            RandUtil.Console("Rogue Legacy", $"Has Architect Fee: {Game.PlayerStats.HasArchitectFee}");
            RandUtil.Console("Rogue Legacy", $"Player read last diary: {Game.PlayerStats.ReadLastDiary}");
            RandUtil.Console("Rogue Legacy", $"Player read starting room diary this life: {Game.PlayerStats.ReadStartingDiary}");
            RandUtil.Console("Rogue Legacy", $"Player has spoken to last boss: {Game.PlayerStats.SpokenToLastBoss}");
            RandUtil.Console("Rogue Legacy", $"Is Hardcore mode: {Game.PlayerStats.HardcoreMode}");
            RandUtil.Console("Rogue Legacy", $"Total Hours Played {Game.PlayerStats.TotalHoursPlayed}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 1: {Game.PlayerStats.WizardSpellList.X}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 2: {Game.PlayerStats.WizardSpellList.Y}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 3: {Game.PlayerStats.WizardSpellList.Z}");
        }

        // Enemy Data
        var enemiesKilledList = Game.PlayerStats.EnemiesKilledList;
        foreach (var enemy in enemiesKilledList)
        {
            writer.Write((byte) enemy.X);
            writer.Write((byte) enemy.Y);
            writer.Write((byte) enemy.Z);
            writer.Write((byte) enemy.W);
        }

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", "Saving Enemy List Data");
            for (var type = 0; type < enemiesKilledList.Count; type++)
            {
                var enemy = enemiesKilledList[type];
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Basic, Killed: {enemy.X}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Advanced, Killed: {enemy.Y}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Expert, Killed: {enemy.Z}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Mini-Boss, Killed: {enemy.W}");
            }

            RandUtil.Console("Rogue Legacy", "Saving num enemies killed");
            RandUtil.Console("Rogue Legacy", $"Number of enemies killed in run: {Game.PlayerStats.EnemiesKilledInRun.Count}");
        }

        writer.Write(Game.PlayerStats.EnemiesKilledInRun.Count);
        foreach (var enemy in Game.PlayerStats.EnemiesKilledInRun)
        {
            writer.Write((int) enemy.X);
            writer.Write((int) enemy.Y);

            if (LevelENV.ShowSaveLoadDebugText)
            {
                RandUtil.Console("Rogue Legacy", $"Enemy Room Index: {enemy.X}");
                RandUtil.Console("Rogue Legacy", $"Enemy Index in EnemyList: {enemy.Y}");
            }
        }

        // Challenge Bosses Completion
        writer.Write(Game.PlayerStats.ChallengeEyeballUnlocked);
        writer.Write(Game.PlayerStats.ChallengeSkullUnlocked);
        writer.Write(Game.PlayerStats.ChallengeFireballUnlocked);
        writer.Write(Game.PlayerStats.ChallengeBlobUnlocked);
        writer.Write(Game.PlayerStats.ChallengeLastBossUnlocked);
        writer.Write(Game.PlayerStats.ChallengeEyeballBeaten);
        writer.Write(Game.PlayerStats.ChallengeSkullBeaten);
        writer.Write(Game.PlayerStats.ChallengeFireballBeaten);
        writer.Write(Game.PlayerStats.ChallengeBlobBeaten);
        writer.Write(Game.PlayerStats.ChallengeLastBossBeaten);

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", $"Eyeball Challenge Unlocked: {Game.PlayerStats.ChallengeEyeballUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Skull Challenge Unlocked: {Game.PlayerStats.ChallengeSkullUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Fireball Challenge Unlocked: {Game.PlayerStats.ChallengeFireballUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Blob Challenge Unlocked: {Game.PlayerStats.ChallengeBlobUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Challenge Unlocked: {Game.PlayerStats.ChallengeLastBossUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Eyeball Challenge Beaten: {Game.PlayerStats.ChallengeEyeballBeaten}");
            RandUtil.Console("Rogue Legacy", $"Skull Challenge Beaten: {Game.PlayerStats.ChallengeSkullBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fireball Challenge Beaten: {Game.PlayerStats.ChallengeFireballBeaten}");
            RandUtil.Console("Rogue Legacy", $"Blob Challenge Beaten: {Game.PlayerStats.ChallengeBlobBeaten}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Challenge Beaten: {Game.PlayerStats.ChallengeLastBossBeaten}");
        }

        if (saveBackup && stream is FileStream fileStream)
        {
            fileStream.Flush(true);
        }

        RandUtil.Console("Rogue Legacy", "Player saving complete.");
        writer.Close();
        stream.Close();
    }

    private void SaveUpgradeData(bool saveBackup)
    {
        var text = _fileNameUpgrades;
        if (saveBackup)
        {
            text = text.Insert(0, "AutoSave_");
        }

        text = text.Insert(0, $"Profile_{Game.ProfileName}/");
        using (var stream = _storageContainer.CreateFile(text))
        {
            using (var binaryWriter = new BinaryWriter(stream))
            {
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nSaving Equipment States");
                }

                var getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Standard Blueprints");
                }

                foreach (var current in getBlueprintArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b = array[i];
                        binaryWriter.Write(b);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write($" {b}");
                        }
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                var getRuneArray = Game.PlayerStats.GetRuneArray;
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nRune Blueprints");
                }

                foreach (var current2 in getRuneArray)
                {
                    var array2 = current2;
                    for (var j = 0; j < array2.Length; j++)
                    {
                        var b2 = array2[j];
                        binaryWriter.Write(b2);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write($" {b2}");
                        }
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                var getEquippedArray = Game.PlayerStats.GetEquippedArray;
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nEquipped Standard Item");
                }

                var array3 = getEquippedArray;
                for (var k = 0; k < array3.Length; k++)
                {
                    var b3 = array3[k];
                    binaryWriter.Write(b3);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {b3}");
                    }
                }

                var getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nEquipped Abilities");
                }

                var array4 = getEquippedRuneArray;
                for (var l = 0; l < array4.Length; l++)
                {
                    var b4 = array4[l];
                    binaryWriter.Write(b4);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {b4}");
                    }
                }

                var skillArray = SkillSystem.GetSkillArray();
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nskills");
                }

                var array5 = skillArray;
                foreach (var skillObj in array5)
                {
                    binaryWriter.Write(skillObj.MaxLevel);
                    binaryWriter.Write(skillObj.CurrentLevel);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {skillObj.CurrentLevel}");
                    }
                }

                // Store the count as well.
                binaryWriter.Write(Game.ItemHandler.ReceivedItems.Count);
                foreach (var pair in Game.ItemHandler.ReceivedItems)
                {
                    var index = pair.Key;
                    var item = pair.Value;
                    binaryWriter.Write(index);
                    binaryWriter.Write(item.Item);
                    binaryWriter.Write(item.Location);
                    binaryWriter.Write(item.Player);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {item.Item}:{item.Location}:{item.Player}");
                    }
                }

                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                binaryWriter.Close();
            }

            stream.Close();
        }
    }

    private void SaveMap(bool saveBackup)
    {
        var text = _fileNameMap;
        if (saveBackup)
        {
            text = text.Insert(0, "AutoSave_");
        }

        text = text.Insert(0, $"Profile_{Game.ProfileName}/");
        using (var stream = _storageContainer.CreateFile(text))
        {
            using (var binaryWriter = new BinaryWriter(stream))
            {
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nSaving Map");
                }

                var num = 0;
                var levelScreen = Game.ScreenManager.GetLevelScreen();
                if (levelScreen != null)
                {
                    if (LevelENV.RunDemoVersion)
                    {
                        binaryWriter.Write(levelScreen.RoomList.Count - 4);
                    }
                    else
                    {
                        binaryWriter.Write(levelScreen.RoomList.Count - 12);
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine($"Map size: {(levelScreen.RoomList.Count - 12)}");
                    }

                    var list = new List<byte>();
                    var list2 = new List<byte>();
                    foreach (var current in levelScreen.RoomList)
                        if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" &&
                            current.Name != "Compass" && current.Name != "ChallengeBoss")
                        {
                            binaryWriter.Write(current.PoolIndex);
                            binaryWriter.Write((byte) current.Zone);
                            binaryWriter.Write((int) current.X);
                            binaryWriter.Write((int) current.Y);
                            binaryWriter.Write((byte) current.LinkerRoomZone);
                            binaryWriter.Write(current.TextureColor.R);
                            binaryWriter.Write(current.TextureColor.G);
                            binaryWriter.Write(current.TextureColor.B);
                            if (LevelENV.ShowSaveLoadDebugText)
                            {
                                Console.Write($"I:{current.PoolIndex} T:{(int) current.Zone}, ");
                            }

                            num++;
                            if (num > 5)
                            {
                                num = 0;
                                if (LevelENV.ShowSaveLoadDebugText)
                                {
                                    Console.Write("\n");
                                }
                            }

                            foreach (var current2 in current.EnemyList)
                                if (current2.IsProcedural)
                                {
                                    list.Add(current2.Type);
                                    list2.Add((byte) current2.Difficulty);
                                }
                        }

                    var count = list.Count;
                    binaryWriter.Write(count);
                    foreach (var current3 in list) binaryWriter.Write(current3);
                    using (var enumerator4 = list2.GetEnumerator())
                    {
                        while (enumerator4.MoveNext())
                        {
                            var current4 = enumerator4.Current;
                            binaryWriter.Write(current4);
                        }

                        goto IL_34E;
                    }
                }


                Console.WriteLine(
                    "WARNING: Attempting to save LEVEL screen but it was null. Make sure it exists in the screen manager before saving it.");
                IL_34E:
                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                binaryWriter.Close();
            }

            stream.Close();
        }
    }

    private void SaveMapData(bool saveBackup)
    {
        var text = _fileNameMapData;
        if (saveBackup)
        {
            text = text.Insert(0, "AutoSave_");
        }

        text = text.Insert(0, $"Profile_{Game.ProfileName}/");
        using (var stream = _storageContainer.CreateFile(text))
        {
            using (var binaryWriter = new BinaryWriter(stream))
            {
                var levelScreen = Game.ScreenManager.GetLevelScreen();
                if (levelScreen != null)
                {
                    var mapRoomsAdded = levelScreen.MapRoomsAdded;
                    var list = new List<bool>();
                    var list2 = new List<bool>();
                    var list3 = new List<int>();
                    var list4 = new List<bool>();
                    var list5 = new List<byte>();
                    var list6 = new List<bool>();
                    var list7 = new List<bool>();
                    var list8 = new List<bool>();
                    foreach (var current in levelScreen.RoomList)
                    {
                        if (mapRoomsAdded.Contains(current))
                        {
                            list.Add(true);
                        }
                        else
                        {
                            list.Add(false);
                        }

                        var bonusRoomObj = current as BonusRoomObj;
                        if (bonusRoomObj != null)
                        {
                            if (bonusRoomObj.RoomCompleted)
                            {
                                list2.Add(true);
                            }
                            else
                            {
                                list2.Add(false);
                            }

                            list3.Add(bonusRoomObj.ID);
                        }

                        if (current.Name != "Boss" && current.Name != "ChallengeBoss")
                        {
                            foreach (var current2 in current.EnemyList)
                                if (current2.IsKilled)
                                {
                                    list7.Add(true);
                                }
                                else
                                {
                                    list7.Add(false);
                                }
                        }

                        if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" &&
                            current.Name != "ChallengeBoss")
                        {
                            foreach (var current3 in current.GameObjList)
                            {
                                var breakableObj = current3 as BreakableObj;
                                if (breakableObj != null)
                                {
                                    if (breakableObj.Broken)
                                    {
                                        list8.Add(true);
                                    }
                                    else
                                    {
                                        list8.Add(false);
                                    }
                                }

                                var chestObj = current3 as ChestObj;
                                if (chestObj != null)
                                {
                                    list5.Add((byte) chestObj.ChestType);
                                    if (chestObj.IsOpen)
                                    {
                                        list4.Add(true);
                                    }
                                    else
                                    {
                                        list4.Add(false);
                                    }

                                    var fairyChestObj = chestObj as FairyChestObj;
                                    if (fairyChestObj != null)
                                    {
                                        if (fairyChestObj.State == 2)
                                        {
                                            list6.Add(true);
                                        }
                                        else
                                        {
                                            list6.Add(false);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    binaryWriter.Write(list.Count);
                    foreach (var current4 in list) binaryWriter.Write(current4);
                    binaryWriter.Write(list2.Count);
                    foreach (var current5 in list2) binaryWriter.Write(current5);
                    foreach (var current6 in list3) binaryWriter.Write(current6);
                    binaryWriter.Write(list5.Count);
                    foreach (var current7 in list5) binaryWriter.Write(current7);
                    binaryWriter.Write(list4.Count);
                    foreach (var current8 in list4) binaryWriter.Write(current8);
                    binaryWriter.Write(list6.Count);
                    foreach (var current9 in list6) binaryWriter.Write(current9);
                    binaryWriter.Write(list7.Count);
                    foreach (var current10 in list7) binaryWriter.Write(current10);
                    binaryWriter.Write(list8.Count);
                    using (var enumerator11 = list8.GetEnumerator())
                    {
                        while (enumerator11.MoveNext())
                        {
                            var current11 = enumerator11.Current;
                            binaryWriter.Write(current11);
                        }

                        goto IL_4C3;
                    }
                }

                Console.WriteLine(
                    "WARNING: Attempting to save level screen MAP data but level was null. Make sure it exists in the screen manager before saving it.");
                IL_4C3:
                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                binaryWriter.Close();
            }

            stream.Close();
        }
    }

    private void SaveLineageData(bool saveBackup)
    {
        var text = _fileNameLineage;
        if (saveBackup)
        {
            text = text.Insert(0, "AutoSave_");
        }

        text = text.Insert(0, $"Profile_{Game.ProfileName}/");
        using (var stream = _storageContainer.CreateFile(text))
        {
            using (var binaryWriter = new BinaryWriter(stream))
            {
                Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN SAVING /////");
                var currentBranches = Game.PlayerStats.CurrentBranches;
                var num = 0;
                if (currentBranches != null)
                {
                    num = currentBranches.Count;
                    binaryWriter.Write(num);
                    for (var i = 0; i < num; i++)
                    {
                        binaryWriter.Write(currentBranches[i].Name);
                        binaryWriter.Write(currentBranches[i].Spell);
                        binaryWriter.Write(currentBranches[i].Class);
                        binaryWriter.Write(currentBranches[i].HeadPiece);
                        binaryWriter.Write(currentBranches[i].ChestPiece);
                        binaryWriter.Write(currentBranches[i].ShoulderPiece);
                        binaryWriter.Write(currentBranches[i].Age);
                        binaryWriter.Write(currentBranches[i].ChildAge);
                        binaryWriter.Write((byte) currentBranches[i].Traits.X);
                        binaryWriter.Write((byte) currentBranches[i].Traits.Y);
                        binaryWriter.Write(currentBranches[i].IsFemale);
                    }
                }
                else
                {
                    binaryWriter.Write(num);
                }

                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving Current Branch Lineage Data");
                    for (var j = 0; j < num; j++)
                    {
                        Console.WriteLine($"Player Name: {currentBranches[j].Name}");
                        Console.WriteLine($"Spell: {currentBranches[j].Name}");
                        Console.WriteLine($"Class: {currentBranches[j].Name}");
                        Console.WriteLine($"Head Piece: {currentBranches[j].HeadPiece}");
                        Console.WriteLine($"Chest Piece: {currentBranches[j].ChestPiece}");
                        Console.WriteLine($"Shoulder Piece: {currentBranches[j].ShoulderPiece}");
                        Console.WriteLine($"Player Age: {currentBranches[j].Age}");
                        Console.WriteLine($"Player Child Age: {currentBranches[j].ChildAge}");
                        Console.WriteLine($"Traits: {currentBranches[j].Traits.X}, {currentBranches[j].Traits.Y}");
                        Console.WriteLine($"Is Female: {currentBranches[j].IsFemale}");
                    }
                }

                var familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                var num2 = 0;
                if (familyTreeArray != null)
                {
                    num2 = familyTreeArray.Count;
                    binaryWriter.Write(num2);
                    for (var k = 0; k < num2; k++)
                    {
                        binaryWriter.Write(familyTreeArray[k].Name);
                        binaryWriter.Write(familyTreeArray[k].Age);
                        binaryWriter.Write(familyTreeArray[k].Class);
                        binaryWriter.Write(familyTreeArray[k].HeadPiece);
                        binaryWriter.Write(familyTreeArray[k].ChestPiece);
                        binaryWriter.Write(familyTreeArray[k].ShoulderPiece);
                        binaryWriter.Write(familyTreeArray[k].NumEnemiesBeaten);
                        binaryWriter.Write(familyTreeArray[k].BeatenABoss);
                        binaryWriter.Write((byte) familyTreeArray[k].Traits.X);
                        binaryWriter.Write((byte) familyTreeArray[k].Traits.Y);
                        binaryWriter.Write(familyTreeArray[k].IsFemale);
                    }
                }
                else
                {
                    binaryWriter.Write(num2);
                }

                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving Family Tree Data");
                    Console.WriteLine($"Number of Branches: {num2}");
                    for (var l = 0; l < num2; l++)
                    {
                        Console.WriteLine("/// Saving branch");
                        Console.WriteLine($"Name: {familyTreeArray[l].Name}");
                        Console.WriteLine($"Age: {familyTreeArray[l].Age}");
                        Console.WriteLine($"Class: {familyTreeArray[l].Class}");
                        Console.WriteLine($"Head Piece: {familyTreeArray[l].HeadPiece}");
                        Console.WriteLine($"Chest Piece: {familyTreeArray[l].ChestPiece}");
                        Console.WriteLine($"Shoulder Piece: {familyTreeArray[l].ShoulderPiece}");
                        Console.WriteLine($"Number of Enemies Beaten: {familyTreeArray[l].NumEnemiesBeaten}");
                        Console.WriteLine($"Beaten a Boss: {familyTreeArray[l].BeatenABoss}");
                        Console.WriteLine($"Traits: {familyTreeArray[l].Traits.X}, {familyTreeArray[l].Traits.Y}");
                        Console.WriteLine($"Is Female: {familyTreeArray[l].IsFemale}");
                    }
                }

                Console.WriteLine("///// PLAYER LINEAGE DATA - SAVE COMPLETE /////");
                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                binaryWriter.Close();
            }

            stream.Close();
        }
    }

    private void LoadData(SaveType loadType, ProceduralLevelScreen level)
    {
        if (!FileExists(loadType))
        {
            RandUtil.Console("Rogue Legacy", $"Could not load data of type {loadType} because data did not exist.");
            return;
        }

        switch (loadType)
        {
            case SaveType.PlayerData:
                LoadPlayerData();
                break;

            case SaveType.UpgradeData:
                LoadUpgradeData();
                break;

            case SaveType.Map:
                RandUtil.Console("Rogue Legacy", "Cannot load Map directly from LoadData. Call LoadMap() instead.");
                break;

            case SaveType.MapData:
                if (level != null)
                {
                    LoadMapData(level);
                }
                else
                {
                    RandUtil.Console("Rogue Legacy", "Could not load Map data. Level was null.");
                }

                break;

            case SaveType.Lineage:
                LoadLineageData();
                break;
        }

        RandUtil.Console("Rogue Legacy", $"Data of type {loadType} Loaded.");
    }

    private void LoadPlayerData()
    {
        var savefile = $"Profile_{Game.ProfileName}/{_fileNamePlayer}";
        using var stream = _storageContainer.OpenFile(savefile, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new BinaryReader(stream);

        // Verify version number.
        var saveVersion = reader.ReadByte();
        if (saveVersion != SAVE_VERSION)
        {
            throw new FileLoadException($"{saveVersion}");
        }

        Game.ProfileName = reader.ReadString();
        Game.PlayerStats.Gold = reader.ReadInt32();
        Game.PlayerStats.CurrentHealth = reader.ReadInt32();
        Game.PlayerStats.CurrentMana = reader.ReadInt32();
        Game.PlayerStats.Age = reader.ReadByte();
        Game.PlayerStats.ChildAge = reader.ReadByte();
        Game.PlayerStats.Spell = reader.ReadByte();
        Game.PlayerStats.Class = reader.ReadByte();
        Game.PlayerStats.SpecialItem = reader.ReadByte();
        Game.PlayerStats.Traits = new(reader.ReadByte(), reader.ReadByte());
        Game.PlayerStats.PlayerName = reader.ReadString();
        Game.PlayerStats.HeadPiece = reader.ReadByte();
        Game.PlayerStats.ShoulderPiece = reader.ReadByte();
        Game.PlayerStats.ChestPiece = reader.ReadByte();
        Game.PlayerStats.DiaryEntry = reader.ReadByte();
        Game.PlayerStats.BonusHealth = reader.ReadInt32();
        Game.PlayerStats.BonusStrength = reader.ReadInt32();
        Game.PlayerStats.BonusMana = reader.ReadInt32();
        Game.PlayerStats.BonusDefense = reader.ReadInt32();
        Game.PlayerStats.BonusWeight = reader.ReadInt32();
        Game.PlayerStats.BonusMagic = reader.ReadInt32();
        Game.PlayerStats.LichHealth = reader.ReadInt32();
        Game.PlayerStats.LichMana = reader.ReadInt32();
        Game.PlayerStats.LichHealthMod = reader.ReadSingle();
        Game.PlayerStats.NewBossBeaten = reader.ReadBoolean();
        Game.PlayerStats.EyeballBossBeaten = reader.ReadBoolean();
        Game.PlayerStats.FairyBossBeaten = reader.ReadBoolean();
        Game.PlayerStats.FireballBossBeaten = reader.ReadBoolean();
        Game.PlayerStats.BlobBossBeaten = reader.ReadBoolean();
        Game.PlayerStats.LastbossBeaten = reader.ReadBoolean();
        Game.PlayerStats.TimesCastleBeaten = reader.ReadInt32();
        Game.PlayerStats.NumEnemiesBeaten = reader.ReadInt32();
        Game.PlayerStats.TutorialComplete = reader.ReadBoolean();
        Game.PlayerStats.CharacterFound = reader.ReadBoolean();
        Game.PlayerStats.LoadStartingRoom = reader.ReadBoolean();
        Game.PlayerStats.LockCastle = reader.ReadBoolean();
        Game.PlayerStats.SpokeToBlacksmith = reader.ReadBoolean();
        Game.PlayerStats.SpokeToEnchantress = reader.ReadBoolean();
        Game.PlayerStats.SpokeToArchitect = reader.ReadBoolean();
        Game.PlayerStats.SpokeToTollCollector = reader.ReadBoolean();
        Game.PlayerStats.IsDead = reader.ReadBoolean();
        Game.PlayerStats.FinalDoorOpened = reader.ReadBoolean();
        Game.PlayerStats.RerolledChildren = reader.ReadBoolean();
        Game.PlayerStats.IsFemale = reader.ReadBoolean();
        Game.PlayerStats.TimesDead = reader.ReadInt32();
        Game.PlayerStats.HasArchitectFee = reader.ReadBoolean();
        Game.PlayerStats.ReadLastDiary = reader.ReadBoolean();
        Game.PlayerStats.ReadStartingDiary = reader.ReadBoolean();
        Game.PlayerStats.SpokenToLastBoss = reader.ReadBoolean();
        Game.PlayerStats.HardcoreMode = reader.ReadBoolean();
        Game.PlayerStats.TotalHoursPlayed = reader.ReadSingle();

        // Load mage spells.
        var spell1 = reader.ReadByte();
        var spell2 = reader.ReadByte();
        var spell3 = reader.ReadByte();
        Game.PlayerStats.WizardSpellList = new(spell1, spell2, spell3);

        // Received Items from Archipelago.
        Program.Game.NextChildItemQueue = new();
        var queuedCount = reader.ReadInt32();
        for (var i = 0; i < queuedCount; i++)
        {
            var item = new NetworkItem
            {
                Item = reader.ReadInt64(),
                Player = reader.ReadInt32(),
                Location = reader.ReadInt64(),
                Flags = (ItemFlags) reader.ReadInt32(),
            };

            Program.Game.NextChildItemQueue.Enqueue(item);
        }

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", "Loading Player Stats");
            RandUtil.Console("Rogue Legacy", $"Profile Name: {Game.ProfileName}");
            RandUtil.Console("Rogue Legacy", $"Gold: {Game.PlayerStats.Gold}");
            RandUtil.Console("Rogue Legacy", $"Current Health: {Game.PlayerStats.CurrentHealth}");
            RandUtil.Console("Rogue Legacy", $"Current Mana: {Game.PlayerStats.CurrentMana}");
            RandUtil.Console("Rogue Legacy", $"Age: {Game.PlayerStats.Age}");
            RandUtil.Console("Rogue Legacy", $"Child Age: {Game.PlayerStats.ChildAge}");
            RandUtil.Console("Rogue Legacy", $"Spell: {Game.PlayerStats.Spell}");
            RandUtil.Console("Rogue Legacy", $"Class: {Game.PlayerStats.Class}");
            RandUtil.Console("Rogue Legacy", $"Special Item: {Game.PlayerStats.SpecialItem}");
            RandUtil.Console("Rogue Legacy", $"Traits: {Game.PlayerStats.Traits.X}, {Game.PlayerStats.Traits.Y}");
            RandUtil.Console("Rogue Legacy", $"Name: {Game.PlayerStats.PlayerName}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Head Piece: {Game.PlayerStats.HeadPiece}");
            RandUtil.Console("Rogue Legacy", $"Shoulder Piece: {Game.PlayerStats.ShoulderPiece}");
            RandUtil.Console("Rogue Legacy", $"Chest Piece: {Game.PlayerStats.ChestPiece}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Diary Entry: {Game.PlayerStats.DiaryEntry}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Bonus Health: {Game.PlayerStats.BonusHealth}");
            RandUtil.Console("Rogue Legacy", $"Bonus Strength: {Game.PlayerStats.BonusStrength}");
            RandUtil.Console("Rogue Legacy", $"Bonus Mana: {Game.PlayerStats.BonusMana}");
            RandUtil.Console("Rogue Legacy", $"Bonus Armor: {Game.PlayerStats.BonusDefense}");
            RandUtil.Console("Rogue Legacy", $"Bonus Weight: {Game.PlayerStats.BonusWeight}");
            RandUtil.Console("Rogue Legacy", $"Bonus Magic: {Game.PlayerStats.BonusMagic}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Lich Health: {Game.PlayerStats.LichHealth}");
            RandUtil.Console("Rogue Legacy", $"Lich Mana: {Game.PlayerStats.LichMana}");
            RandUtil.Console("Rogue Legacy", $"Lich Health Mod: {Game.PlayerStats.LichHealthMod}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"New Boss Beaten: {Game.PlayerStats.NewBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Eyeball Boss Beaten: {Game.PlayerStats.EyeballBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fairy Boss Beaten: {Game.PlayerStats.FairyBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fireball Boss Beaten: {Game.PlayerStats.FireballBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Blob Boss Beaten: {Game.PlayerStats.BlobBossBeaten}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Beaten: {Game.PlayerStats.LastbossBeaten}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Times Castle Beaten: {Game.PlayerStats.TimesCastleBeaten}");
            RandUtil.Console("Rogue Legacy", $"Number of Enemies Beaten: {Game.PlayerStats.NumEnemiesBeaten}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Tutorial Complete: {Game.PlayerStats.TutorialComplete}");
            RandUtil.Console("Rogue Legacy", $"Character Found: {Game.PlayerStats.CharacterFound}");
            RandUtil.Console("Rogue Legacy", $"Load Starting Room: {Game.PlayerStats.LoadStartingRoom}");
            RandUtil.Console("Rogue Legacy", "---------------");
            RandUtil.Console("Rogue Legacy", $"Castle Locked: {Game.PlayerStats.LockCastle}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Blacksmith: {Game.PlayerStats.SpokeToBlacksmith}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Enchantress: {Game.PlayerStats.SpokeToEnchantress}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Architect: {Game.PlayerStats.SpokeToArchitect}");
            RandUtil.Console("Rogue Legacy", $"Spoke to Toll Collector: {Game.PlayerStats.SpokeToTollCollector}");
            RandUtil.Console("Rogue Legacy", $"Player Is Dead: {Game.PlayerStats.IsDead}");
            RandUtil.Console("Rogue Legacy", $"Final Door Opened: {Game.PlayerStats.FinalDoorOpened}");
            RandUtil.Console("Rogue Legacy", $"Rerolled Children: {Game.PlayerStats.RerolledChildren}");
            RandUtil.Console("Rogue Legacy", $"Is Female: {Game.PlayerStats.IsFemale}");
            RandUtil.Console("Rogue Legacy", $"Times Dead: {Game.PlayerStats.TimesDead}");
            RandUtil.Console("Rogue Legacy", $"Has Architect Fee: {Game.PlayerStats.HasArchitectFee}");
            RandUtil.Console("Rogue Legacy", $"Player read last diary: {Game.PlayerStats.ReadLastDiary}");
            RandUtil.Console("Rogue Legacy", $"Player read starting room diary this life: {Game.PlayerStats.ReadStartingDiary}");
            RandUtil.Console("Rogue Legacy", $"Player has spoken to last boss: {Game.PlayerStats.SpokenToLastBoss}");
            RandUtil.Console("Rogue Legacy", $"Is Hardcore mode: {Game.PlayerStats.HardcoreMode}");
            RandUtil.Console("Rogue Legacy", $"Total Hours Played {Game.PlayerStats.TotalHoursPlayed}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 1: {Game.PlayerStats.WizardSpellList.X}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 2: {Game.PlayerStats.WizardSpellList.Y}");
            RandUtil.Console("Rogue Legacy", $"Wizard Spell 3: {Game.PlayerStats.WizardSpellList.Z}");

            RandUtil.Console("Rogue Legacy", "Loading Enemy List Data");
        }

        // Load enemy data.
        for (var type = 0; type < 34; type++)
        {
            var enemy = new Vector4(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            Game.PlayerStats.EnemiesKilledList[type] = enemy;

            if (LevelENV.ShowSaveLoadDebugText)
            {
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Basic, Killed: {enemy.X}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Advanced, Killed: {enemy.Y}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Expert, Killed: {enemy.Z}");
                RandUtil.Console("Rogue Legacy", $"Enemy Type: {type}, Difficulty: Mini-Boss, Killed: {enemy.W}");
            }
        }

        var enemiesKilledCount = reader.ReadInt32();
        for (var j = 0; j < enemiesKilledCount; j++)
        {
            var enemy = new Vector2(reader.ReadInt32(), reader.ReadInt32());
            Game.PlayerStats.EnemiesKilledInRun.Add(enemy);
        }

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", "Loading num enemies killed");
            RandUtil.Console("Rogue Legacy", $"Number of enemies killed in run: {enemiesKilledCount}");

            foreach (var enemy in Game.PlayerStats.EnemiesKilledInRun)
            {
                RandUtil.Console("Rogue Legacy", $"Enemy Room Index: {enemy.X}");
                RandUtil.Console("Rogue Legacy", $"Enemy Index in EnemyList: {enemy.Y}");
            }
        }

        // DLC Data
        Game.PlayerStats.ChallengeEyeballUnlocked = reader.ReadBoolean();
        Game.PlayerStats.ChallengeSkullUnlocked = reader.ReadBoolean();
        Game.PlayerStats.ChallengeFireballUnlocked = reader.ReadBoolean();
        Game.PlayerStats.ChallengeBlobUnlocked = reader.ReadBoolean();
        Game.PlayerStats.ChallengeLastBossUnlocked = reader.ReadBoolean();
        Game.PlayerStats.ChallengeEyeballBeaten = reader.ReadBoolean();
        Game.PlayerStats.ChallengeSkullBeaten = reader.ReadBoolean();
        Game.PlayerStats.ChallengeFireballBeaten = reader.ReadBoolean();
        Game.PlayerStats.ChallengeBlobBeaten = reader.ReadBoolean();
        Game.PlayerStats.ChallengeLastBossBeaten = reader.ReadBoolean();

        if (LevelENV.ShowSaveLoadDebugText)
        {
            RandUtil.Console("Rogue Legacy", $"Eyeball Challenge Unlocked: {Game.PlayerStats.ChallengeEyeballUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Skull Challenge Unlocked: {Game.PlayerStats.ChallengeSkullUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Fireball Challenge Unlocked: {Game.PlayerStats.ChallengeFireballUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Blob Challenge Unlocked: {Game.PlayerStats.ChallengeBlobUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Challenge Unlocked: {Game.PlayerStats.ChallengeLastBossUnlocked}");
            RandUtil.Console("Rogue Legacy", $"Eyeball Challenge Beaten: {Game.PlayerStats.ChallengeEyeballBeaten}");
            RandUtil.Console("Rogue Legacy", $"Skull Challenge Beaten: {Game.PlayerStats.ChallengeSkullBeaten}");
            RandUtil.Console("Rogue Legacy", $"Fireball Challenge Beaten: {Game.PlayerStats.ChallengeFireballBeaten}");
            RandUtil.Console("Rogue Legacy", $"Blob Challenge Beaten: {Game.PlayerStats.ChallengeBlobBeaten}");
            RandUtil.Console("Rogue Legacy", $"Last Boss Challenge Beaten: {Game.PlayerStats.ChallengeLastBossBeaten}");
        }

        RandUtil.Console("Rogue Legacy", "Player loading complete.");
        reader.Close();
        stream.Close();
    }

    private void LoadUpgradeData()
    {
        using (
            var stream =
            _storageContainer.OpenFile(
                $"Profile_{Game.ProfileName}/{_fileNameUpgrades}", FileMode.Open,
                FileAccess.Read, FileShare.Read))
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipment States");
                    Console.WriteLine("\nLoading Standard Blueprints");
                }

                var getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
                for (var i = 0; i < 5; i++)
                {
                    for (var j = 0; j < 15; j++)
                    {
                        getBlueprintArray[i][j] = binaryReader.ReadByte();
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write($" {getBlueprintArray[i][j]}");
                        }
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Ability Blueprints");
                }

                var getRuneArray = Game.PlayerStats.GetRuneArray;
                for (var k = 0; k < 5; k++)
                {
                    for (var l = 0; l < 11; l++)
                    {
                        getRuneArray[k][l] = binaryReader.ReadByte();
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write($" {getRuneArray[k][l]}");
                        }
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipped Standard Items");
                }

                var getEquippedArray = Game.PlayerStats.GetEquippedArray;
                for (var m = 0; m < 5; m++)
                {
                    getEquippedArray[m] = binaryReader.ReadSByte();
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {getEquippedArray[m]}");
                    }
                }

                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipped Abilities");
                }

                var getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
                for (var n = 0; n < 5; n++)
                {
                    getEquippedRuneArray[n] = binaryReader.ReadSByte();
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {getEquippedRuneArray[n]}");
                    }
                }

                var skillArray = SkillSystem.GetSkillArray();
                if (LevelENV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Traits");
                }

                SkillSystem.ResetAllTraits();
                Game.PlayerStats.CurrentLevel = 0;
                foreach (var skill in skillArray)
                {
                    skill.MaxLevel = binaryReader.ReadInt32();
                    var level = binaryReader.ReadInt32();

                    for (var j = 0; j < level; j++)
                    {
                        if (SkillSystem.GetManorPiece(skill) == -1)
                        {
                            SkillSystem.LevelUpTrait(skill, false);
                        }
                        else
                        {
                            SkillSystem.LevelUpTrait(skill, false, false);
                        }
                    }
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {skill.CurrentLevel}");
                    }
                }

                var itemReceivedDict = new Dictionary<int, NetworkItem>();
                var count = binaryReader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var index = binaryReader.ReadInt32();
                    var item = new NetworkItem
                    {
                        Item = binaryReader.ReadInt64(),
                        Location = binaryReader.ReadInt64(),
                        Player = binaryReader.ReadInt32()
                    };

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.Write($" {item.Item}:{item.Location}:{item.Player}");
                    }

                    itemReceivedDict.Add(index, item);
                }

                Game.ItemHandler.ReceivedItems = itemReceivedDict;
                // Set fountain pieces
                foreach (var pair in Game.ItemHandler.ReceivedItems)
                {
                    if (pair.Value.Item == ItemCode.FOUNTAIN_PIECE)
                    {
                        Game.PlayerStats.FountainPieces++;
                    }
                }

                binaryReader.Close();
                Game.ScreenManager.Player.UpdateEquipmentColours();
            }

            stream.Close();
            var flag = false;
            var familyTreeArray = Game.PlayerStats.FamilyTreeArray;
            foreach (var current in familyTreeArray)
                if (current.Class > 3)
                {
                    flag = true;
                    break;
                }
        }
    }

    public ProceduralLevelScreen LoadMap()
    {
        GetStorageContainer();
        ProceduralLevelScreen proceduralLevelScreen = null;
        using (
            var stream =
            _storageContainer.OpenFile(
                $"Profile_{Game.ProfileName}/{_fileNameMap}", FileMode.Open,
                FileAccess.Read, FileShare.Read))
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var num = binaryReader.ReadInt32();
                var array = new Vector4[num];
                var array2 = new Vector4[num];
                for (var i = 0; i < num; i++)
                {
                    array[i].W = binaryReader.ReadInt32();
                    array[i].X = binaryReader.ReadByte();
                    array[i].Y = binaryReader.ReadInt32();
                    array[i].Z = binaryReader.ReadInt32();
                    array2[i].W = binaryReader.ReadByte();
                    array2[i].X = binaryReader.ReadByte();
                    array2[i].Y = binaryReader.ReadByte();
                    array2[i].Z = binaryReader.ReadByte();
                }

                proceduralLevelScreen = LevelBuilder2.CreateLevel(array, array2);
                var num2 = binaryReader.ReadInt32();
                var list = new List<byte>();
                for (var j = 0; j < num2; j++) list.Add(binaryReader.ReadByte());
                var list2 = new List<byte>();
                for (var k = 0; k < num2; k++) list2.Add(binaryReader.ReadByte());
                LevelBuilder2.OverrideProceduralEnemies(proceduralLevelScreen, list.ToArray(), list2.ToArray());

                binaryReader.Close();
            }

            stream.Close();
        }

        _storageContainer.Dispose();
        return proceduralLevelScreen;
    }

    private void LoadMapData(ProceduralLevelScreen createdLevel)
    {
        using (
            var stream =
            _storageContainer.OpenFile(
                $"Profile_{Game.ProfileName}/{_fileNameMapData}", FileMode.Open,
                FileAccess.Read, FileShare.Read))
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var num = binaryReader.ReadInt32();
                var list = new List<bool>();
                for (var i = 0; i < num; i++) list.Add(binaryReader.ReadBoolean());
                var num2 = binaryReader.ReadInt32();
                var list2 = new List<bool>();
                for (var j = 0; j < num2; j++) list2.Add(binaryReader.ReadBoolean());
                var list3 = new List<int>();
                for (var k = 0; k < num2; k++) list3.Add(binaryReader.ReadInt32());
                var num3 = binaryReader.ReadInt32();
                var list4 = new List<byte>();
                for (var l = 0; l < num3; l++) list4.Add(binaryReader.ReadByte());
                num3 = binaryReader.ReadInt32();
                var list5 = new List<bool>();
                for (var m = 0; m < num3; m++) list5.Add(binaryReader.ReadBoolean());
                num3 = binaryReader.ReadInt32();
                var list6 = new List<bool>();
                for (var n = 0; n < num3; n++) list6.Add(binaryReader.ReadBoolean());
                var num4 = binaryReader.ReadInt32();
                var list7 = new List<bool>();
                for (var num5 = 0; num5 < num4; num5++) list7.Add(binaryReader.ReadBoolean());
                var num6 = binaryReader.ReadInt32();
                var list8 = new List<bool>();
                for (var num7 = 0; num7 < num6; num7++) list8.Add(binaryReader.ReadBoolean());
                var num8 = 0;
                var num9 = 0;
                var num10 = 0;
                var num11 = 0;
                var num12 = 0;
                var num13 = 0;
                foreach (var current in createdLevel.RoomList)
                {
                    if (num2 > 0)
                    {
                        var bonusRoomObj = current as BonusRoomObj;
                        if (bonusRoomObj != null)
                        {
                            var flag = list2[num8];
                            var iD = list3[num8];
                            num8++;
                            if (flag)
                            {
                                bonusRoomObj.RoomCompleted = true;
                            }

                            bonusRoomObj.ID = iD;
                        }
                    }

                    if (num4 > 0 && !Game.PlayerStats.LockCastle && current.Name != "Boss" &&
                        current.Name != "ChallengeBoss")
                    {
                        foreach (var current2 in current.EnemyList)
                        {
                            var flag2 = list7[num12];
                            num12++;
                            if (flag2)
                            {
                                current2.KillSilently();
                            }
                        }
                    }

                    if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" &&
                        current.Name != "ChallengeBoss")
                    {
                        foreach (var current3 in current.GameObjList)
                        {
                            if (!Game.PlayerStats.LockCastle && num6 > 0)
                            {
                                var breakableObj = current3 as BreakableObj;
                                if (breakableObj != null)
                                {
                                    var flag3 = list8[num13];
                                    num13++;
                                    if (flag3)
                                    {
                                        breakableObj.ForceBreak();
                                    }
                                }
                            }

                            var chestObj = current3 as ChestObj;
                            if (chestObj != null)
                            {
                                chestObj.IsProcedural = false;
                                var chestType = list4[num9];
                                num9++;
                                chestObj.ChestType = (ChestType) chestType;
                                var flag4 = list5[num10];
                                num10++;
                                if (flag4)
                                {
                                    chestObj.ForceOpen();
                                }

                                if (!Game.PlayerStats.LockCastle)
                                {
                                    var fairyChestObj = chestObj as FairyChestObj;
                                    if (fairyChestObj != null)
                                    {
                                        var flag5 = list6[num11];
                                        num11++;
                                        if (flag5)
                                        {
                                            fairyChestObj.SetChestFailed(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (num > 0)
                {
                    var list9 = new List<RoomObj>();
                    var count = list.Count;
                    for (var num14 = 0; num14 < count; num14++)
                        if (list[num14])
                        {
                            list9.Add(createdLevel.RoomList[num14]);
                        }

                    createdLevel.MapRoomsUnveiled = list9;
                }

                binaryReader.Close();
            }

            stream.Close();
        }
    }

    private void LoadLineageData()
    {
        using (
            var stream =
            _storageContainer.OpenFile(
                $"Profile_{Game.ProfileName}/{_fileNameLineage}", FileMode.Open,
                FileAccess.Read, FileShare.Read))
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN LOADING /////");
                var list = new List<PlayerLineageData>();
                var num = binaryReader.ReadInt32();
                for (var i = 0; i < num; i++)
                    list.Add(new()
                    {
                        Name = binaryReader.ReadString(),
                        Spell = binaryReader.ReadByte(),
                        Class = binaryReader.ReadByte(),
                        HeadPiece = binaryReader.ReadByte(),
                        ChestPiece = binaryReader.ReadByte(),
                        ShoulderPiece = binaryReader.ReadByte(),
                        Age = binaryReader.ReadByte(),
                        ChildAge = binaryReader.ReadByte(),
                        Traits = new(binaryReader.ReadByte(), binaryReader.ReadByte()),
                        IsFemale = binaryReader.ReadBoolean()
                    });
                if (list.Count > 0)
                {
                    Game.PlayerStats.CurrentBranches = list;
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Loading Current Branch Lineage Data");
                        var currentBranches = Game.PlayerStats.CurrentBranches;
                        for (var j = 0; j < num; j++)
                        {
                            Console.WriteLine($"Player Name: {currentBranches[j].Name}");
                            Console.WriteLine($"Spell: {currentBranches[j].Name}");
                            Console.WriteLine($"Class: {currentBranches[j].Name}");
                            Console.WriteLine($"Head Piece: {currentBranches[j].HeadPiece}");
                            Console.WriteLine($"Chest Piece: {currentBranches[j].ChestPiece}");
                            Console.WriteLine($"Shoulder Piece: {currentBranches[j].ShoulderPiece}");
                            Console.WriteLine($"Player Age: {currentBranches[j].Age}");
                            Console.WriteLine($"Player Child Age: {currentBranches[j].ChildAge}");
                            Console.WriteLine($"Traits: {currentBranches[j].Traits.X}, {currentBranches[j].Traits.Y}");
                            Console.WriteLine($"Is Female: {currentBranches[j].IsFemale}");
                        }
                    }
                }

                var list2 = new List<FamilyTreeNode>();
                var num2 = binaryReader.ReadInt32();
                for (var k = 0; k < num2; k++)
                {
                    var item = default(FamilyTreeNode);
                    item.Name = binaryReader.ReadString();
                    item.Age = binaryReader.ReadByte();
                    item.Class = binaryReader.ReadByte();
                    item.HeadPiece = binaryReader.ReadByte();
                    item.ChestPiece = binaryReader.ReadByte();
                    item.ShoulderPiece = binaryReader.ReadByte();
                    item.NumEnemiesBeaten = binaryReader.ReadInt32();
                    item.BeatenABoss = binaryReader.ReadBoolean();
                    item.Traits.X = binaryReader.ReadByte();
                    item.Traits.Y = binaryReader.ReadByte();
                    item.IsFemale = binaryReader.ReadBoolean();
                    list2.Add(item);
                }

                if (list2.Count > 0)
                {
                    Game.PlayerStats.FamilyTreeArray = list2;
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        var familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                        Console.WriteLine("Loading Family Tree Data");
                        Console.WriteLine($"Number of Branches: {num2}");
                        for (var l = 0; l < num2; l++)
                        {
                            Console.WriteLine("/// Saving branch");
                            Console.WriteLine($"Name: {familyTreeArray[l].Name}");
                            Console.WriteLine($"Age: {familyTreeArray[l].Age}");
                            Console.WriteLine($"Class: {familyTreeArray[l].Class}");
                            Console.WriteLine($"Head Piece: {familyTreeArray[l].HeadPiece}");
                            Console.WriteLine($"Chest Piece: {familyTreeArray[l].ChestPiece}");
                            Console.WriteLine($"Shoulder Piece: {familyTreeArray[l].ShoulderPiece}");
                            Console.WriteLine($"Number of Enemies Beaten: {familyTreeArray[l].NumEnemiesBeaten}");
                            Console.WriteLine($"Beaten a Boss: {familyTreeArray[l].BeatenABoss}");
                            Console.WriteLine($"Traits: {familyTreeArray[l].Traits.X}, {familyTreeArray[l].Traits.Y}");
                            Console.WriteLine($"Is Female: {familyTreeArray[l].IsFemale}");
                        }
                    }
                }

                Console.WriteLine("///// PLAYER LINEAGE DATA - LOAD COMPLETE /////");
                binaryReader.Close();
            }

            stream.Close();
        }
    }

    public bool FileExists(SaveType saveType)
    {
        var storageNotDisposed = _storageContainer is not { IsDisposed: false };

        GetStorageContainer();
        var result = saveType switch
        {
            SaveType.PlayerData  => _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNamePlayer}"),
            SaveType.UpgradeData => _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameUpgrades}"),
            SaveType.Map         => _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMap}"),
            SaveType.MapData     => _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameMapData}"),
            SaveType.Lineage     => _storageContainer.FileExists($"Profile_{Game.ProfileName}/{_fileNameLineage}"),
            _                    => false,
        };

        if (storageNotDisposed)
        {
            _storageContainer.Dispose();
            _storageContainer = null;
        }

        return result;
    }

    private void GetStorageContainer()
    {
        if (_storageContainer is { IsDisposed: false })
        {
            return;
        }

        var asyncResult = StorageDevice.BeginShowSelector(null, null);
        asyncResult.AsyncWaitHandle.WaitOne();
        var storageDevice = StorageDevice.EndShowSelector(asyncResult);
        asyncResult.AsyncWaitHandle.Close();
        asyncResult = storageDevice.BeginOpenContainer("RogueLegacyRandomizerStorageContainer", null, null);
        asyncResult.AsyncWaitHandle.WaitOne();
        _storageContainer = storageDevice.EndOpenContainer(asyncResult);
        asyncResult.AsyncWaitHandle.Close();
    }

    private void PerformDirectoryCheck()
    {
        GetStorageContainer();

        if (!_storageContainer.DirectoryExists("Profile_DEFAULT"))
        {
            _storageContainer.CreateDirectory("Profile_DEFAULT");
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    private void CopyFile(StorageContainer storageContainer, string fileName, string profileName)
    {
        if (!storageContainer.FileExists(fileName))
        {
            return;
        }

        var stream = storageContainer.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        var stream2 = storageContainer.CreateFile($"{profileName}/{fileName}");
        stream.CopyTo(stream2);
        stream.Close();
        stream2.Close();
    }

    private void ClearFiles(params SaveType[] deleteList)
    {
        GetStorageContainer();
        foreach (var deleteType in deleteList)
        {
            DeleteData(deleteType);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    private void ClearBackupFiles(params SaveType[] deleteList)
    {
        GetStorageContainer();
        foreach (var deleteType in deleteList)
        {
            DeleteBackupData(deleteType);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

}
