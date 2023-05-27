using System;
using System.Collections.Generic;
using System.IO;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy
{
    public class SaveGameManager
    {
        private readonly string m_fileNameLineage =
            string.Format("{0}_Lineage.rcdat", LevelENV.GameName.Replace(" ", ""));

        private readonly string m_fileNameMap = string.Format("{0}_Map.rcdat", LevelENV.GameName.Replace(" ", ""));

        private readonly string m_fileNameMapData =
            string.Format("{0}_MapDat.rcdat", LevelENV.GameName.Replace(" ", ""));

        private readonly string m_fileNamePlayer =
            string.Format("{0}_Player.rcdat", LevelENV.GameName.Replace(" ", ""));

        private readonly string m_fileNameUpgrades = string.Format("{0}_BP.rcdat", LevelENV.GameName.Replace(" ", ""));
        private bool m_autosaveLoaded;
        private int m_saveFailCounter;
        private StorageContainer m_storageContainer;

        public SaveGameManager()
        {
            m_saveFailCounter = 0;
            m_autosaveLoaded = false;
        }

        public void Initialize()
        {
            if (m_storageContainer != null)
            {
                m_storageContainer.Dispose();
                m_storageContainer = null;
            }

            PerformDirectoryCheck();
        }

        private void GetStorageContainer()
        {
            if (m_storageContainer != null && !m_storageContainer.IsDisposed)
            {
                return;
            }

            var asyncResult = StorageDevice.BeginShowSelector(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            var storageDevice = StorageDevice.EndShowSelector(asyncResult);
            asyncResult.AsyncWaitHandle.Close();
            asyncResult = storageDevice.BeginOpenContainer("RogueLegacyRandomizerStorageContainer", null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            m_storageContainer = storageDevice.EndOpenContainer(asyncResult);
            asyncResult.AsyncWaitHandle.Close();
        }

        private void PerformDirectoryCheck()
        {
            GetStorageContainer();

            if (!m_storageContainer.DirectoryExists("Profile_DEFAULT"))
            {
                m_storageContainer.CreateDirectory("Profile_DEFAULT");
            }

            m_storageContainer.Dispose();
            m_storageContainer = null;
        }

        public void CreateSaveDirectory()
        {
            GetStorageContainer();
            if (!m_storageContainer.DirectoryExists(string.Format("Profile_{0}", Game.ProfileName)))
            {
                m_storageContainer.CreateDirectory(string.Format("Profile_{0}", Game.ProfileName));
            }

            m_storageContainer.Dispose();
            m_storageContainer = null;
        }

        private void CopyFile(StorageContainer storageContainer, string fileName, string profileName)
        {
            if (storageContainer.FileExists(fileName))
            {
                var stream = storageContainer.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var stream2 = storageContainer.CreateFile(profileName + "/" + fileName);
                stream.CopyTo(stream2);
                stream.Close();
                stream2.Close();
            }
        }

        public void SaveFiles(params SaveType[] saveList)
        {
            if (!LevelENV.DisableSaving)
            {
                GetStorageContainer();
                try
                {
                    for (var i = 0; i < saveList.Length; i++)
                    {
                        var saveType = saveList[i];
                        SaveData(saveType, false);
                    }

                    m_saveFailCounter = 0;
                }
                catch
                {
                    if (m_saveFailCounter > 2)
                    {
                        var screenManager = Game.ScreenManager;
                        screenManager.DialogueScreen.SetDialogue("Save File Error Antivirus");
                        Tween.RunFunction(0.25f, screenManager, "DisplayScreen", 13, true, typeof(List<object>));
                        m_saveFailCounter = 0;
                    }
                    else
                    {
                        m_saveFailCounter++;
                    }
                }
                finally
                {
                    if (m_storageContainer != null && !m_storageContainer.IsDisposed)
                    {
                        m_storageContainer.Dispose();
                    }

                    m_storageContainer = null;
                }
            }
        }

        public void SaveBackupFiles(params SaveType[] saveList)
        {
            if (!LevelENV.DisableSaving)
            {
                GetStorageContainer();
                for (var i = 0; i < saveList.Length; i++)
                {
                    var saveType = saveList[i];
                    SaveData(saveType, true);
                }

                m_storageContainer.Dispose();
                m_storageContainer = null;
            }
        }

        public void SaveAllFileTypes(bool saveBackup)
        {
            if (!saveBackup)
            {
                SaveFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
                return;
            }

            SaveBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData,
                SaveType.Lineage);
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
                            for (var i = 0; i < loadList.Length; i++)
                            {
                                var saveType2 = loadList[i];
                                saveType = saveType2;
                                LoadData(saveType2, level);
                            }
                        }
                    }
                    catch
                    {
                        if (saveType == SaveType.Map || saveType == SaveType.MapData || saveType == SaveType.None)
                        {
                            throw new Exception();
                        }

                        if (!m_autosaveLoaded)
                        {
                            var screenManager = Game.ScreenManager;
                            screenManager.DialogueScreen.SetDialogue("Save File Error");
                            screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
                            screenManager.DisplayScreen(13, false);
                            Game.PlayerStats.HeadPiece = 0;
                        }
                        else
                        {
                            m_autosaveLoaded = false;
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
                    if (m_storageContainer != null && !m_storageContainer.IsDisposed)
                    {
                        m_storageContainer.Dispose();
                    }
                }
            }

            if (!LevelENV.DisableSaving)
            {
                GetStorageContainer();
                for (var j = 0; j < loadList.Length; j++)
                {
                    var loadType = loadList[j];
                    LoadData(loadType, level);
                }

                m_storageContainer.Dispose();
                m_storageContainer = null;
            }
        }

        public void ForceBackup()
        {
            if (m_storageContainer != null && !m_storageContainer.IsDisposed)
            {
                m_storageContainer.Dispose();
            }

            var screenManager = Game.ScreenManager;
            screenManager.DialogueScreen.SetDialogue("Save File Error");
            screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
            screenManager.DisplayScreen(13, false);
        }

        public void LoadAutosave()
        {
            Console.WriteLine("Save file corrupted");
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
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
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            Game.ScreenManager.DisplayScreen(23, true);
        }

        public void ResetAutosave()
        {
            m_autosaveLoaded = false;
        }

        public void LoadAllFileTypes(ProceduralLevelScreen level)
        {
            LoadFiles(level, SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData,
                SaveType.Lineage);
        }

        public void ClearFiles(params SaveType[] deleteList)
        {
            GetStorageContainer();
            foreach (var deleteType in deleteList) DeleteData(deleteType);
            m_storageContainer.Dispose();
            m_storageContainer = null;
        }

        public void ClearBackupFiles(params SaveType[] deleteList)
        {
            GetStorageContainer();
            foreach (var deleteType in deleteList) DeleteBackupData(deleteType);

            m_storageContainer.Dispose();
            m_storageContainer = null;
        }

        public void ClearAllFileTypes(bool deleteBackups)
        {
            if (!deleteBackups)
            {
                ClearFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
                return;
            }

            ClearBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData,
                SaveType.Lineage);
        }

        private void DeleteData(SaveType deleteType)
        {
            switch (deleteType)
            {
                case SaveType.PlayerData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNamePlayer)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNamePlayer));
                    }

                    break;

                case SaveType.UpgradeData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameUpgrades)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameUpgrades));
                    }

                    break;

                case SaveType.Map:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMap)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMap));
                    }

                    break;

                case SaveType.MapData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMapData)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMapData));
                    }

                    break;

                case SaveType.Lineage:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameLineage)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameLineage));
                    }

                    break;
            }

            Console.WriteLine("Save file type " + deleteType + " deleted.");
        }

        private void DeleteBackupData(SaveType deleteType)
        {
            switch (deleteType)
            {
                case SaveType.PlayerData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNamePlayer)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNamePlayer));
                    }

                    break;

                case SaveType.UpgradeData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameUpgrades)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameUpgrades));
                    }

                    break;

                case SaveType.Map:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameMap)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameMap));
                    }

                    break;

                case SaveType.MapData:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameMapData)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameMapData));
                    }

                    break;

                case SaveType.Lineage:
                    if (
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameLineage)))
                    {
                        m_storageContainer.DeleteFile(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                            m_fileNameLineage));
                    }

                    break;
            }

            Console.WriteLine("Backup save file type " + deleteType + " deleted.");
        }

        private void LoadBackups()
        {
            Console.WriteLine("Replacing save file with back up saves");
            GetStorageContainer();
            if (
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                    m_fileNamePlayer)) &&
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                    m_fileNamePlayer)))
            {
                var stream =
                    m_storageContainer.OpenFile(
                        string.Concat("Profile_", Game.ProfileName, "/AutoSave_", m_fileNamePlayer),
                        FileMode.Open);
                var stream2 =
                    m_storageContainer.CreateFile(string.Concat("Profile_", Game.ProfileName, "/",
                        m_fileNamePlayer));
                stream.CopyTo(stream2);
                stream.Close();
                stream2.Close();
            }

            if (
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                    m_fileNameUpgrades)) &&
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                    m_fileNameUpgrades)))
            {
                var stream3 =
                    m_storageContainer.OpenFile(
                        string.Concat("Profile_", Game.ProfileName, "/AutoSave_", m_fileNameUpgrades),
                        FileMode.Open);
                var stream4 =
                    m_storageContainer.CreateFile(string.Concat("Profile_", Game.ProfileName, "/",
                        m_fileNameUpgrades));
                stream3.CopyTo(stream4);
                stream3.Close();
                stream4.Close();
            }

            if (
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                    m_fileNameMap)) &&
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/", m_fileNameMap)))
            {
                var stream5 =
                    m_storageContainer.OpenFile(
                        string.Concat("Profile_", Game.ProfileName, "/AutoSave_", m_fileNameMap),
                        FileMode.Open);
                var stream6 =
                    m_storageContainer.CreateFile(string.Concat("Profile_", Game.ProfileName, "/",
                        m_fileNameMap));
                stream5.CopyTo(stream6);
                stream5.Close();
                stream6.Close();
            }

            if (
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                    m_fileNameMapData)) &&
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                    m_fileNameMapData)))
            {
                var stream7 =
                    m_storageContainer.OpenFile(
                        string.Concat("Profile_", Game.ProfileName, "/AutoSave_", m_fileNameMapData),
                        FileMode.Open);
                var stream8 =
                    m_storageContainer.CreateFile(string.Concat("Profile_", Game.ProfileName, "/",
                        m_fileNameMapData));
                stream7.CopyTo(stream8);
                stream7.Close();
                stream8.Close();
            }

            if (
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/AutoSave_",
                    m_fileNameLineage)) &&
                m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                    m_fileNameLineage)))
            {
                var stream9 =
                    m_storageContainer.OpenFile(
                        string.Concat("Profile_", Game.ProfileName, "/AutoSave_", m_fileNameLineage),
                        FileMode.Open);
                var stream10 =
                    m_storageContainer.CreateFile(string.Concat("Profile_", Game.ProfileName, "/",
                        m_fileNameLineage));
                stream9.CopyTo(stream10);
                stream9.Close();
                stream10.Close();
            }

            m_autosaveLoaded = true;
            m_storageContainer.Dispose();
            m_storageContainer = null;
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

            Console.WriteLine("\nData type " + saveType + " saved!");
        }

        private void SavePlayerData(bool saveBackup)
        {
            var text = m_fileNamePlayer;
            if (saveBackup)
            {
                text = text.Insert(0, "AutoSave_");
            }

            text = text.Insert(0, "Profile_" + Game.ProfileName + "/");
            using (var stream = m_storageContainer.CreateFile(text))
            {
                using (var binaryWriter = new BinaryWriter(stream))
                {
                    binaryWriter.Write(Game.PlayerStats.Gold);
                    Game.PlayerStats.CurrentHealth = Game.ScreenManager.Player.CurrentHealth;
                    binaryWriter.Write(Game.PlayerStats.CurrentHealth);
                    Game.PlayerStats.CurrentMana = (int) Game.ScreenManager.Player.CurrentMana;
                    binaryWriter.Write(Game.PlayerStats.CurrentMana);
                    binaryWriter.Write(Game.PlayerStats.Age);
                    binaryWriter.Write(Game.PlayerStats.ChildAge);
                    binaryWriter.Write(Game.PlayerStats.Spell);
                    binaryWriter.Write(Game.PlayerStats.Class);
                    binaryWriter.Write(Game.PlayerStats.SpecialItem);
                    binaryWriter.Write((byte) Game.PlayerStats.Traits.X);
                    binaryWriter.Write((byte) Game.PlayerStats.Traits.Y);
                    binaryWriter.Write(Game.PlayerStats.PlayerName);
                    binaryWriter.Write(Game.PlayerStats.HeadPiece);
                    binaryWriter.Write(Game.PlayerStats.ShoulderPiece);
                    binaryWriter.Write(Game.PlayerStats.ChestPiece);
                    binaryWriter.Write(Game.PlayerStats.DiaryEntry);
                    binaryWriter.Write(Game.PlayerStats.BonusHealth);
                    binaryWriter.Write(Game.PlayerStats.BonusStrength);
                    binaryWriter.Write(Game.PlayerStats.BonusMana);
                    binaryWriter.Write(Game.PlayerStats.BonusDefense);
                    binaryWriter.Write(Game.PlayerStats.BonusWeight);
                    binaryWriter.Write(Game.PlayerStats.BonusMagic);
                    binaryWriter.Write(Game.PlayerStats.LichHealth);
                    binaryWriter.Write(Game.PlayerStats.LichMana);
                    binaryWriter.Write(Game.PlayerStats.LichHealthMod);
                    binaryWriter.Write(Game.PlayerStats.NewBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.EyeballBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.FairyBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.FireballBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.BlobBossBeaten);
                    binaryWriter.Write(Game.PlayerStats.LastbossBeaten);
                    binaryWriter.Write(Game.PlayerStats.TimesCastleBeaten);
                    binaryWriter.Write(Game.PlayerStats.NumEnemiesBeaten);
                    binaryWriter.Write(Game.PlayerStats.TutorialComplete);
                    binaryWriter.Write(Game.PlayerStats.CharacterFound);
                    binaryWriter.Write(Game.PlayerStats.LoadStartingRoom);
                    binaryWriter.Write(Game.PlayerStats.LockCastle);
                    binaryWriter.Write(Game.PlayerStats.SpokeToBlacksmith);
                    binaryWriter.Write(Game.PlayerStats.SpokeToEnchantress);
                    binaryWriter.Write(Game.PlayerStats.SpokeToArchitect);
                    binaryWriter.Write(Game.PlayerStats.SpokeToTollCollector);
                    binaryWriter.Write(Game.PlayerStats.IsDead);
                    binaryWriter.Write(Game.PlayerStats.FinalDoorOpened);
                    binaryWriter.Write(Game.PlayerStats.RerolledChildren);
                    binaryWriter.Write(Game.PlayerStats.IsFemale);
                    binaryWriter.Write(Game.PlayerStats.TimesDead);
                    binaryWriter.Write(Game.PlayerStats.HasArchitectFee);
                    binaryWriter.Write(Game.PlayerStats.ReadLastDiary);
                    binaryWriter.Write(Game.PlayerStats.SpokenToLastBoss);
                    binaryWriter.Write(Game.PlayerStats.HardcoreMode);
                    binaryWriter.Write(Game.ProfileName);
                    binaryWriter.Write(Game.PlayerStats.FountainPieces);
                    binaryWriter.Write(Program.Game.NextChildItemQueue.Count);
                    foreach (var item in Program.Game.NextChildItemQueue)
                    {
                        binaryWriter.Write(item.Item);
                        binaryWriter.Write(item.Player);
                        binaryWriter.Write(item.Location);
                        binaryWriter.Write((int) item.Flags);
                    }
                    var value = Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength;
                    binaryWriter.Write(value);
                    binaryWriter.Write((byte) Game.PlayerStats.WizardSpellList.X);
                    binaryWriter.Write((byte) Game.PlayerStats.WizardSpellList.Y);
                    binaryWriter.Write((byte) Game.PlayerStats.WizardSpellList.Z);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("\nSaving Player Stats");
                        Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                        Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                        Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                        Console.WriteLine("Age: " + Game.PlayerStats.Age);
                        Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                        Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                        Console.WriteLine("Class: " + Game.PlayerStats.Class);
                        Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                        Console.WriteLine(string.Concat("Traits: ", Game.PlayerStats.Traits.X, ", ",
                            Game.PlayerStats.Traits.Y));
                        Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                        Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                        Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                        Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                        Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                        Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                        Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                        Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                        Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                        Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                        Console.WriteLine("---------------");
                        Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                        Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                        Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                        Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                        Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                        Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                        Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                        Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                        Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                        Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                        Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                        Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                        Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                        Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                        Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                        Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                        Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                        Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                        Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                        Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                        Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                        Console.WriteLine("Profile Name: " + Game.ProfileName);
                        Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                        Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
                        Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
                        Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - BEGIN SAVING /////");
                    var enemiesKilledList = Game.PlayerStats.EnemiesKilledList;
                    foreach (var current in enemiesKilledList)
                    {
                        binaryWriter.Write((byte) current.X);
                        binaryWriter.Write((byte) current.Y);
                        binaryWriter.Write((byte) current.Z);
                        binaryWriter.Write((byte) current.W);
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Saving Enemy List Data");
                        var num = 0;
                        foreach (var current2 in enemiesKilledList)
                        {
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Basic, Killed: ",
                                current2.X));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Advanced, Killed: ",
                                current2.Y));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Expert, Killed: ",
                                current2.Z));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Mini-Boss, Killed: ",
                                current2.W));
                            num++;
                        }
                    }

                    var count = Game.PlayerStats.EnemiesKilledInRun.Count;
                    var enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
                    binaryWriter.Write(count);
                    foreach (var current3 in enemiesKilledInRun)
                    {
                        binaryWriter.Write((int) current3.X);
                        binaryWriter.Write((int) current3.Y);
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Saving num enemies killed");
                        Console.WriteLine("Number of enemies killed in run: " + count);
                        foreach (var current4 in enemiesKilledInRun)
                        {
                            Console.WriteLine("Enemy Room Index: " + current4.X);
                            Console.WriteLine("Enemy Index in EnemyList: " + current4.Y);
                        }
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - SAVE COMPLETE /////");
                    Console.WriteLine("///// DLC DATA - BEGIN SAVING /////");
                    binaryWriter.Write(Game.PlayerStats.ChallengeEyeballUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeSkullUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeFireballUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeBlobUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeLastBossUnlocked);
                    binaryWriter.Write(Game.PlayerStats.ChallengeEyeballBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeSkullBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeFireballBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeBlobBeaten);
                    binaryWriter.Write(Game.PlayerStats.ChallengeLastBossBeaten);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                        Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                        Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
                        Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                        Console.WriteLine("Last Boss Challenge Unlocked: " +
                                          Game.PlayerStats.ChallengeLastBossUnlocked);
                        Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                        Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                        Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                        Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                        Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
                    }

                    Console.WriteLine("///// DLC DATA - SAVE COMPLETE /////");
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

        private void SaveUpgradeData(bool saveBackup)
        {
            var text = m_fileNameUpgrades;
            if (saveBackup)
            {
                text = text.Insert(0, "AutoSave_");
            }

            text = text.Insert(0, "Profile_" + Game.ProfileName + "/");
            using (var stream = m_storageContainer.CreateFile(text))
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
                                Console.Write(" " + b);
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
                                Console.Write(" " + b2);
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
                            Console.Write(" " + b3);
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
                            Console.Write(" " + b4);
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
                        binaryWriter.Write(skillObj.CurrentLevel);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + skillObj.CurrentLevel);
                        }
                    }

                    var statArray = SkillSystem.GetStatArray();
                    foreach (var skillObj in statArray)
                    {
                        binaryWriter.Write(skillObj.CurrentLevel);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + skillObj.CurrentLevel);
                        }
                    }

                    // Store the count as well.
                    binaryWriter.Write(Game.PlayerStats.ReceivedItems.Count);
                    foreach (var item in Game.PlayerStats.ReceivedItems)
                    {
                        binaryWriter.Write(item.Item);
                        binaryWriter.Write(item.Location);
                        binaryWriter.Write(item.Player);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + item.Item + ":" + item.Location + ":" + item.Player);
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
            var text = m_fileNameMap;
            if (saveBackup)
            {
                text = text.Insert(0, "AutoSave_");
            }

            text = text.Insert(0, "Profile_" + Game.ProfileName + "/");
            using (var stream = m_storageContainer.CreateFile(text))
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
                            Console.WriteLine("Map size: " + (levelScreen.RoomList.Count - 12));
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
                                    Console.Write(string.Concat("I:", current.PoolIndex, " T:", (int) current.Zone,
                                        ", "));
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
            var text = m_fileNameMapData;
            if (saveBackup)
            {
                text = text.Insert(0, "AutoSave_");
            }

            text = text.Insert(0, "Profile_" + Game.ProfileName + "/");
            using (var stream = m_storageContainer.CreateFile(text))
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
            var text = m_fileNameLineage;
            if (saveBackup)
            {
                text = text.Insert(0, "AutoSave_");
            }

            text = text.Insert(0, "Profile_" + Game.ProfileName + "/");
            using (var stream = m_storageContainer.CreateFile(text))
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
                            Console.WriteLine("Player Name: " + currentBranches[j].Name);
                            Console.WriteLine("Spell: " + currentBranches[j].Name);
                            Console.WriteLine("Class: " + currentBranches[j].Name);
                            Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
                            Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
                            Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
                            Console.WriteLine("Player Age: " + currentBranches[j].Age);
                            Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
                            Console.WriteLine(string.Concat("Traits: ", currentBranches[j].Traits.X, ", ",
                                currentBranches[j].Traits.Y));
                            Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
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
                        Console.WriteLine("Number of Branches: " + num2);
                        for (var l = 0; l < num2; l++)
                        {
                            Console.WriteLine("/// Saving branch");
                            Console.WriteLine("Name: " + familyTreeArray[l].Name);
                            Console.WriteLine("Age: " + familyTreeArray[l].Age);
                            Console.WriteLine("Class: " + familyTreeArray[l].Class);
                            Console.WriteLine("Head Piece: " + familyTreeArray[l].HeadPiece);
                            Console.WriteLine("Chest Piece: " + familyTreeArray[l].ChestPiece);
                            Console.WriteLine("Shoulder Piece: " + familyTreeArray[l].ShoulderPiece);
                            Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[l].NumEnemiesBeaten);
                            Console.WriteLine("Beaten a Boss: " + familyTreeArray[l].BeatenABoss);
                            Console.WriteLine(string.Concat("Traits: ", familyTreeArray[l].Traits.X, ", ",
                                familyTreeArray[l].Traits.Y));
                            Console.WriteLine("Is Female: " + familyTreeArray[l].IsFemale);
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
            if (FileExists(loadType))
            {
                switch (loadType)
                {
                    case SaveType.PlayerData:
                        LoadPlayerData();
                        break;

                    case SaveType.UpgradeData:
                        LoadUpgradeData();
                        break;

                    case SaveType.Map:
                        Console.WriteLine("Cannot load Map directly from LoadData. Call LoadMap() instead.");
                        break;

                    case SaveType.MapData:
                        if (level != null)
                        {
                            LoadMapData(level);
                        }
                        else
                        {
                            Console.WriteLine("Could not load Map data. Level was null.");
                        }

                        break;

                    case SaveType.Lineage:
                        LoadLineageData();
                        break;
                }

                Console.WriteLine("\nData of type " + loadType + " Loaded.");
                return;
            }

            Console.WriteLine("Could not load data of type " + loadType + " because data did not exist.");
        }

        private void LoadPlayerData()
        {
            using (
                var stream =
                m_storageContainer.OpenFile(
                    string.Concat("Profile_", Game.ProfileName, "/", m_fileNamePlayer), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    Game.PlayerStats.Gold = binaryReader.ReadInt32();
                    Game.PlayerStats.CurrentHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.CurrentMana = binaryReader.ReadInt32();
                    Game.PlayerStats.Age = binaryReader.ReadByte();
                    Game.PlayerStats.ChildAge = binaryReader.ReadByte();
                    Game.PlayerStats.Spell = binaryReader.ReadByte();
                    Game.PlayerStats.Class = binaryReader.ReadByte();
                    Game.PlayerStats.SpecialItem = binaryReader.ReadByte();
                    Game.PlayerStats.Traits = new Vector2(binaryReader.ReadByte(), binaryReader.ReadByte());
                    Game.PlayerStats.PlayerName = binaryReader.ReadString();
                    Game.PlayerStats.HeadPiece = binaryReader.ReadByte();
                    Game.PlayerStats.ShoulderPiece = binaryReader.ReadByte();
                    Game.PlayerStats.ChestPiece = binaryReader.ReadByte();
                    Game.PlayerStats.DiaryEntry = binaryReader.ReadByte();
                    Game.PlayerStats.BonusHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusStrength = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusMana = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusDefense = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusWeight = binaryReader.ReadInt32();
                    Game.PlayerStats.BonusMagic = binaryReader.ReadInt32();
                    Game.PlayerStats.LichHealth = binaryReader.ReadInt32();
                    Game.PlayerStats.LichMana = binaryReader.ReadInt32();
                    Game.PlayerStats.LichHealthMod = binaryReader.ReadSingle();
                    Game.PlayerStats.NewBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.EyeballBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.FairyBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.FireballBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.BlobBossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.LastbossBeaten = binaryReader.ReadBoolean();
                    Game.PlayerStats.TimesCastleBeaten = binaryReader.ReadInt32();
                    Game.PlayerStats.NumEnemiesBeaten = binaryReader.ReadInt32();
                    Game.PlayerStats.TutorialComplete = binaryReader.ReadBoolean();
                    Game.PlayerStats.CharacterFound = binaryReader.ReadBoolean();
                    Game.PlayerStats.LoadStartingRoom = binaryReader.ReadBoolean();
                    Game.PlayerStats.LockCastle = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToBlacksmith = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToEnchantress = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToArchitect = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokeToTollCollector = binaryReader.ReadBoolean();
                    Game.PlayerStats.IsDead = binaryReader.ReadBoolean();
                    Game.PlayerStats.FinalDoorOpened = binaryReader.ReadBoolean();
                    Game.PlayerStats.RerolledChildren = binaryReader.ReadBoolean();
                    Game.PlayerStats.IsFemale = binaryReader.ReadBoolean();
                    Game.PlayerStats.TimesDead = binaryReader.ReadInt32();
                    Game.PlayerStats.HasArchitectFee = binaryReader.ReadBoolean();
                    Game.PlayerStats.ReadLastDiary = binaryReader.ReadBoolean();
                    Game.PlayerStats.SpokenToLastBoss = binaryReader.ReadBoolean();
                    Game.PlayerStats.HardcoreMode = binaryReader.ReadBoolean();
                    Game.ProfileName = binaryReader.ReadString();
                    Game.PlayerStats.FountainPieces = binaryReader.ReadInt32();

                    Program.Game.NextChildItemQueue = new Queue<NetworkItem>();
                    var queuedCount = binaryReader.ReadInt32();
                    for (var i = 0; i < queuedCount; i++)
                    {
                        var item = new NetworkItem
                        {
                            Item = binaryReader.ReadInt64(),
                            Player = binaryReader.ReadInt32(),
                            Location = binaryReader.ReadInt64(),
                            Flags = (ItemFlags) binaryReader.ReadInt32()
                        };

                        Program.Game.NextChildItemQueue.Enqueue(item);
                    }
                    Game.PlayerStats.TotalHoursPlayed = binaryReader.ReadSingle();
                    var b = binaryReader.ReadByte();
                    var b2 = binaryReader.ReadByte();
                    var b3 = binaryReader.ReadByte();
                    Game.PlayerStats.WizardSpellList = new Vector3(b, b2, b3);
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("\nLoading Player Stats");
                        Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                        Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                        Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                        Console.WriteLine("Age: " + Game.PlayerStats.Age);
                        Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                        Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                        Console.WriteLine("Class: " + Game.PlayerStats.Class);
                        Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                        Console.WriteLine(string.Concat("Traits: ", Game.PlayerStats.Traits.X, ", ",
                            Game.PlayerStats.Traits.Y));
                        Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                        Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                        Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                        Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                        Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                        Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                        Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                        Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                        Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                        Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                        Console.WriteLine("---------------");
                        Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                        Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                        Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                        Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                        Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                        Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                        Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                        Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                        Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                        Console.WriteLine("---------------");
                        Console.WriteLine("Castle Locked: " + Game.PlayerStats.LockCastle);
                        Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                        Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                        Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                        Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                        Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                        Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                        Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                        Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                        Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                        Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                        Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                        Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                        Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                        Console.WriteLine("Profile Name: " + Game.ProfileName);
                        Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                        Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
                        Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
                        Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - BEGIN LOADING /////");
                    for (var i = 0; i < 34; i++)
                    {
                        var value = new Vector4(binaryReader.ReadByte(), binaryReader.ReadByte(),
                            binaryReader.ReadByte(), binaryReader.ReadByte());
                        Game.PlayerStats.EnemiesKilledList[i] = value;
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Loading Enemy List Data");
                        var num = 0;
                        foreach (var current in Game.PlayerStats.EnemiesKilledList)
                        {
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Basic, Killed: ",
                                current.X));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Advanced, Killed: ",
                                current.Y));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Expert, Killed: ",
                                current.Z));
                            Console.WriteLine(string.Concat("Enemy Type: ", num, ", Difficulty: Mini-Boss, Killed: ",
                                current.W));
                            num++;
                        }
                    }

                    var num2 = binaryReader.ReadInt32();
                    for (var j = 0; j < num2; j++)
                    {
                        var item = new Vector2(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                        Game.PlayerStats.EnemiesKilledInRun.Add(item);
                    }

                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Loading num enemies killed");
                        Console.WriteLine("Number of enemies killed in run: " + num2);
                        foreach (var current2 in Game.PlayerStats.EnemiesKilledInRun)
                        {
                            Console.WriteLine("Enemy Room Index: " + current2.X);
                            Console.WriteLine("Enemy Index in EnemyList: " + current2.Y);
                        }
                    }

                    Console.WriteLine("///// ENEMY LIST DATA - LOAD COMPLETE /////");
                    if (binaryReader.PeekChar() != -1)
                    {
                        Console.WriteLine("///// DLC DATA FOUND - BEGIN LOADING /////");
                        Game.PlayerStats.ChallengeEyeballUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeSkullUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeFireballUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeBlobUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeLastBossUnlocked = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeEyeballBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeSkullBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeFireballBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeBlobBeaten = binaryReader.ReadBoolean();
                        Game.PlayerStats.ChallengeLastBossBeaten = binaryReader.ReadBoolean();
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.WriteLine(
                                "Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                            Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                            Console.WriteLine("Fireball Challenge Unlocked: " +
                                              Game.PlayerStats.ChallengeFireballUnlocked);
                            Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                            Console.WriteLine("Last Boss Challenge Unlocked: " +
                                              Game.PlayerStats.ChallengeLastBossUnlocked);
                            Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                            Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                            Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                            Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                            Console.WriteLine("Last Boss Challenge Beaten: " +
                                              Game.PlayerStats.ChallengeLastBossBeaten);
                        }

                        Console.WriteLine("///// DLC DATA - LOADING COMPLETE /////");
                    }
                    else
                    {
                        Console.WriteLine("///// NO DLC DATA FOUND - SKIPPED LOADING /////");
                    }

                    binaryReader.Close();
                }

                stream.Close();
            }
        }

        private void LoadUpgradeData()
        {
            using (
                var stream =
                m_storageContainer.OpenFile(
                    string.Concat("Profile_", Game.ProfileName, "/", m_fileNameUpgrades), FileMode.Open,
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
                                Console.Write(" " + getBlueprintArray[i][j]);
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
                                Console.Write(" " + getRuneArray[k][l]);
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
                            Console.Write(" " + getEquippedArray[m]);
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
                            Console.Write(" " + getEquippedRuneArray[n]);
                        }
                    }

                    var skillArray = SkillSystem.GetSkillArray();
                    var skillArray2 = SkillSystem.GetStatArray();
                    if (LevelENV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("\nLoading Traits");
                    }

                    SkillSystem.ResetAllTraits();
                    Game.PlayerStats.CurrentLevel = 0;
                    foreach (var skill in skillArray)
                    {
                        var level = binaryReader.ReadInt32();
                        for (var j = 0; j < level; j++) SkillSystem.LevelUpTrait(skill, false, false);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + skill.CurrentLevel);
                        }
                    }

                    foreach (var skill in skillArray2)
                    {
                        var level = binaryReader.ReadInt32();
                        for (var j = 0; j < level; j++) SkillSystem.LevelUpTrait(skill, false);
                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + skill.CurrentLevel);
                        }
                    }

                    var list = new List<NetworkItem>();
                    var count = binaryReader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var item = new NetworkItem
                        {
                            Item = binaryReader.ReadInt64(),
                            Location = binaryReader.ReadInt64(),
                            Player = binaryReader.ReadInt32()
                        };

                        if (LevelENV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + item.Item + ":" + item.Location + ":" + item.Player);
                        }

                        list.Add(item);
                    }

                    Game.PlayerStats.ReceivedItems = list;

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
                m_storageContainer.OpenFile(
                    string.Concat("Profile_", Game.ProfileName, "/", m_fileNameMap), FileMode.Open,
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

            m_storageContainer.Dispose();
            return proceduralLevelScreen;
        }

        private void LoadMapData(ProceduralLevelScreen createdLevel)
        {
            using (
                var stream =
                m_storageContainer.OpenFile(
                    string.Concat("Profile_", Game.ProfileName, "/", m_fileNameMapData), FileMode.Open,
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
                m_storageContainer.OpenFile(
                    string.Concat("Profile_", Game.ProfileName, "/", m_fileNameLineage), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN LOADING /////");
                    var list = new List<PlayerLineageData>();
                    var num = binaryReader.ReadInt32();
                    for (var i = 0; i < num; i++)
                        list.Add(new PlayerLineageData
                        {
                            Name = binaryReader.ReadString(),
                            Spell = binaryReader.ReadByte(),
                            Class = binaryReader.ReadByte(),
                            HeadPiece = binaryReader.ReadByte(),
                            ChestPiece = binaryReader.ReadByte(),
                            ShoulderPiece = binaryReader.ReadByte(),
                            Age = binaryReader.ReadByte(),
                            ChildAge = binaryReader.ReadByte(),
                            Traits = new Vector2(binaryReader.ReadByte(), binaryReader.ReadByte()),
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
                                Console.WriteLine("Player Name: " + currentBranches[j].Name);
                                Console.WriteLine("Spell: " + currentBranches[j].Name);
                                Console.WriteLine("Class: " + currentBranches[j].Name);
                                Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
                                Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
                                Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
                                Console.WriteLine("Player Age: " + currentBranches[j].Age);
                                Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
                                Console.WriteLine(string.Concat("Traits: ", currentBranches[j].Traits.X, ", ",
                                    currentBranches[j].Traits.Y));
                                Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
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
                            Console.WriteLine("Number of Branches: " + num2);
                            for (var l = 0; l < num2; l++)
                            {
                                Console.WriteLine("/// Saving branch");
                                Console.WriteLine("Name: " + familyTreeArray[l].Name);
                                Console.WriteLine("Age: " + familyTreeArray[l].Age);
                                Console.WriteLine("Class: " + familyTreeArray[l].Class);
                                Console.WriteLine("Head Piece: " + familyTreeArray[l].HeadPiece);
                                Console.WriteLine("Chest Piece: " + familyTreeArray[l].ChestPiece);
                                Console.WriteLine("Shoulder Piece: " + familyTreeArray[l].ShoulderPiece);
                                Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[l].NumEnemiesBeaten);
                                Console.WriteLine("Beaten a Boss: " + familyTreeArray[l].BeatenABoss);
                                Console.WriteLine(string.Concat("Traits: ", familyTreeArray[l].Traits.X, ", ",
                                    familyTreeArray[l].Traits.Y));
                                Console.WriteLine("Is Female: " + familyTreeArray[l].IsFemale);
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
            var flag = !(m_storageContainer != null && !m_storageContainer.IsDisposed);

            GetStorageContainer();
            var result = false;
            switch (saveType)
            {
                case SaveType.PlayerData:
                    result =
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNamePlayer));
                    break;

                case SaveType.UpgradeData:
                    result =
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameUpgrades));
                    break;

                case SaveType.Map:
                    result =
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMap));
                    break;

                case SaveType.MapData:
                    result =
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameMapData));
                    break;

                case SaveType.Lineage:
                    result =
                        m_storageContainer.FileExists(string.Concat("Profile_", Game.ProfileName, "/",
                            m_fileNameLineage));
                    break;
            }

            if (flag)
            {
                m_storageContainer.Dispose();
                m_storageContainer = null;
            }

            return result;
        }

        public StorageContainer GetContainer()
        {
            return m_storageContainer;
        }

        public void GetSaveHeader(byte profile, out byte playerClass, out string playerName, out int playerLevel,
            out bool playerIsDead, out int castlesBeaten)
        {
            playerName = null;
            playerClass = 0;
            playerLevel = 0;
            playerIsDead = false;
            castlesBeaten = 0;
            GetStorageContainer();
            if (m_storageContainer.FileExists(string.Concat("Profile_", profile, "/", m_fileNamePlayer)))
            {
                using (
                    var stream = m_storageContainer.OpenFile(
                        string.Concat("Profile_", profile, "/", m_fileNamePlayer), FileMode.Open, FileAccess.Read,
                        FileShare.Read))
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        playerClass = binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        playerName = binaryReader.ReadString();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadByte();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadSingle();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        castlesBeaten = binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        playerIsDead = binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.ReadBoolean();
                        binaryReader.Close();
                    }

                    stream.Close();
                }
            }

            if (m_storageContainer.FileExists(string.Concat("Profile_", profile, "/", m_fileNameUpgrades)))
            {
                using (
                    var stream2 =
                    m_storageContainer.OpenFile(string.Concat("Profile_", profile, "/", m_fileNameUpgrades),
                        FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var binaryReader2 = new BinaryReader(stream2))
                    {
                        for (var i = 0; i < 5; i++)
                        for (var j = 0; j < 15; j++)
                            binaryReader2.ReadByte();
                        for (var k = 0; k < 5; k++)
                        for (var l = 0; l < 11; l++)
                            binaryReader2.ReadByte();
                        for (var m = 0; m < 5; m++) binaryReader2.ReadSByte();
                        for (var n = 0; n < 5; n++) binaryReader2.ReadSByte();
                        var num = 0;
                        for (var num2 = 0; num2 < 32; num2++)
                        {
                            var num3 = binaryReader2.ReadInt32();
                            for (var num4 = 0; num4 < num3; num4++) num++;
                        }

                        playerLevel = num;
                        binaryReader2.Close();
                    }

                    stream2.Close();
                }
            }

            m_storageContainer.Dispose();
            m_storageContainer = null;
        }
    }
}