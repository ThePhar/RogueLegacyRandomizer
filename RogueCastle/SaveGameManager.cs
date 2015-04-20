/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using System.IO;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Tweener;

namespace RogueCastle
{
	public class SaveGameManager
	{
		private StorageContainer m_storageContainer;
		private string m_fileNamePlayer = "RogueLegacyPlayer.rcdat";
		private string m_fileNameUpgrades = "RogueLegacyBP.rcdat";
		private string m_fileNameMap = "RogueLegacyMap.rcdat";
		private string m_fileNameMapData = "RogueLegacyMapDat.rcdat";
		private string m_fileNameLineage = "RogueLegacyLineage.rcdat";
		private Game m_game;
		private int m_saveFailCounter;
		private bool m_autosaveLoaded;
		public SaveGameManager(Game game)
		{
			m_saveFailCounter = 0;
			m_autosaveLoaded = false;
			m_game = game;
		}
		public void Initialize()
		{
			if (LevelEV.RUN_DEMO_VERSION)
			{
				m_fileNamePlayer = "RogueLegacyDemoPlayer.rcdat";
				m_fileNameUpgrades = "RogueLegacyDemoBP.rcdat";
				m_fileNameMap = "RogueLegacyDemoMap.rcdat";
				m_fileNameMapData = "RogueLegacyDemoMapDat.rcdat";
				m_fileNameLineage = "RogueLegacyDemoLineage.rcdat";
			}
			if (m_storageContainer != null)
			{
				m_storageContainer.Dispose();
				m_storageContainer = null;
			}
			PerformDirectoryCheck();
		}
		private void GetStorageContainer()
		{
			if (m_storageContainer == null || m_storageContainer.IsDisposed)
			{
				IAsyncResult asyncResult = StorageDevice.BeginShowSelector(null, null);
				asyncResult.AsyncWaitHandle.WaitOne();
				StorageDevice storageDevice = StorageDevice.EndShowSelector(asyncResult);
				asyncResult.AsyncWaitHandle.Close();
				asyncResult = storageDevice.BeginOpenContainer("RogueLegacyEnhancedStorageContainer", null, null);
				asyncResult.AsyncWaitHandle.WaitOne();
				m_storageContainer = storageDevice.EndOpenContainer(asyncResult);
				asyncResult.AsyncWaitHandle.Close();
			}
		}
		private void PerformDirectoryCheck()
		{
			GetStorageContainer();
			if (!m_storageContainer.DirectoryExists("Profile1"))
			{
				m_storageContainer.CreateDirectory("Profile1");
				CopyFile(m_storageContainer, m_fileNamePlayer, "Profile1");
				CopyFile(m_storageContainer, "AutoSave_" + m_fileNamePlayer, "Profile1");
				CopyFile(m_storageContainer, m_fileNameUpgrades, "Profile1");
				CopyFile(m_storageContainer, "AutoSave_" + m_fileNameUpgrades, "Profile1");
				CopyFile(m_storageContainer, m_fileNameMap, "Profile1");
				CopyFile(m_storageContainer, "AutoSave_" + m_fileNameMap, "Profile1");
				CopyFile(m_storageContainer, m_fileNameMapData, "Profile1");
				CopyFile(m_storageContainer, "AutoSave_" + m_fileNameMapData, "Profile1");
				CopyFile(m_storageContainer, m_fileNameLineage, "Profile1");
				CopyFile(m_storageContainer, "AutoSave_" + m_fileNameLineage, "Profile1");
			}
			if (!m_storageContainer.DirectoryExists("Profile2"))
			{
				m_storageContainer.CreateDirectory("Profile2");
			}
			if (!m_storageContainer.DirectoryExists("Profile3"))
			{
				m_storageContainer.CreateDirectory("Profile3");
			}
			m_storageContainer.Dispose();
			m_storageContainer = null;
		}
		private void CopyFile(StorageContainer storageContainer, string fileName, string profileName)
		{
			if (storageContainer.FileExists(fileName))
			{
				Stream stream = storageContainer.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				Stream stream2 = storageContainer.CreateFile(profileName + "/" + fileName);
				stream.CopyTo(stream2);
				stream.Close();
				stream2.Close();
			}
		}
		public void SaveFiles(params SaveType[] saveList)
		{
			if (!LevelEV.DISABLE_SAVING)
			{
				GetStorageContainer();
				try
				{
					for (int i = 0; i < saveList.Length; i++)
					{
						SaveType saveType = saveList[i];
						SaveData(saveType, false);
					}
					m_saveFailCounter = 0;
				}
				catch
				{
					if (m_saveFailCounter > 2)
					{
						RCScreenManager screenManager = Game.ScreenManager;
						screenManager.DialogueScreen.SetDialogue("Save File Error Antivirus");
						Tween.RunFunction(0.25f, screenManager, "DisplayScreen", new object[]
						{
							13,
							true,
							typeof(List<object>)
						});
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
			if (!LevelEV.DISABLE_SAVING)
			{
				GetStorageContainer();
				for (int i = 0; i < saveList.Length; i++)
				{
					SaveType saveType = saveList[i];
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
				SaveFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.UpgradeData,
					SaveType.Map,
					SaveType.MapData,
					SaveType.Lineage
				});
				return;
			}
			SaveBackupFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData,
				SaveType.Map,
				SaveType.MapData,
				SaveType.Lineage
			});
		}
		public void LoadFiles(ProceduralLevelScreen level, params SaveType[] loadList)
		{
			if (LevelEV.ENABLE_BACKUP_SAVING)
			{
				GetStorageContainer();
				SaveType saveType = SaveType.None;
				try
				{
					try
					{
						if (!LevelEV.DISABLE_SAVING)
						{
							for (int i = 0; i < loadList.Length; i++)
							{
								SaveType saveType2 = loadList[i];
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
							RCScreenManager screenManager = Game.ScreenManager;
							screenManager.DialogueScreen.SetDialogue("Save File Error");
							screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave", new object[0]);
							screenManager.DisplayScreen(13, false, null);
							Game.PlayerStats.HeadPiece = 0;
						}
						else
						{
							m_autosaveLoaded = false;
							RCScreenManager screenManager2 = Game.ScreenManager;
							screenManager2.DialogueScreen.SetDialogue("Save File Error 2");
							screenManager2.DialogueScreen.SetConfirmEndHandler(this, "StartNewGame", new object[0]);
							screenManager2.DisplayScreen(13, false, null);
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
			if (!LevelEV.DISABLE_SAVING)
			{
				GetStorageContainer();
				for (int j = 0; j < loadList.Length; j++)
				{
					SaveType loadType = loadList[j];
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
			RCScreenManager screenManager = Game.ScreenManager;
			screenManager.DialogueScreen.SetDialogue("Save File Error");
			screenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave", new object[0]);
			screenManager.DisplayScreen(13, false, null);
		}
		public void LoadAutosave()
		{
			Console.WriteLine("Save file corrupted");
			SkillSystem.ResetAllTraits();
			Game.PlayerStats.Dispose();
			Game.PlayerStats = new PlayerStats();
			Game.ScreenManager.Player.Reset();
			LoadBackups();
			Game.ScreenManager.DisplayScreen(3, true, null);
		}
		public void StartNewGame()
		{
			ClearAllFileTypes(false);
			ClearAllFileTypes(true);
			SkillSystem.ResetAllTraits();
			Game.PlayerStats.Dispose();
			Game.PlayerStats = new PlayerStats();
			Game.ScreenManager.Player.Reset();
			Game.ScreenManager.DisplayScreen(23, true, null);
		}
		public void ResetAutosave()
		{
			m_autosaveLoaded = false;
		}
		public void LoadAllFileTypes(ProceduralLevelScreen level)
		{
			LoadFiles(level, new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData,
				SaveType.Map,
				SaveType.MapData,
				SaveType.Lineage
			});
		}
		public void ClearFiles(params SaveType[] deleteList)
		{
			GetStorageContainer();
			for (int i = 0; i < deleteList.Length; i++)
			{
				SaveType deleteType = deleteList[i];
				DeleteData(deleteType);
			}
			m_storageContainer.Dispose();
			m_storageContainer = null;
		}
		public void ClearBackupFiles(params SaveType[] deleteList)
		{
			GetStorageContainer();
			for (int i = 0; i < deleteList.Length; i++)
			{
				SaveType deleteType = deleteList[i];
				DeleteBackupData(deleteType);
			}
			m_storageContainer.Dispose();
			m_storageContainer = null;
		}
		public void ClearAllFileTypes(bool deleteBackups)
		{
			if (!deleteBackups)
			{
				ClearFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.UpgradeData,
					SaveType.Map,
					SaveType.MapData,
					SaveType.Lineage
				});
				return;
			}
			ClearBackupFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData,
				SaveType.Map,
				SaveType.MapData,
				SaveType.Lineage
			});
		}
		private void DeleteData(SaveType deleteType)
		{
			switch (deleteType)
			{
			case SaveType.PlayerData:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNamePlayer
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/",
						m_fileNamePlayer
					}));
				}
				break;
			case SaveType.UpgradeData:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameUpgrades
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/",
						m_fileNameUpgrades
					}));
				}
				break;
			case SaveType.Map:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMap
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/",
						m_fileNameMap
					}));
				}
				break;
			case SaveType.MapData:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMapData
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/",
						m_fileNameMapData
					}));
				}
				break;
			case SaveType.Lineage:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameLineage
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/",
						m_fileNameLineage
					}));
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
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNamePlayer
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/AutoSave_",
						m_fileNamePlayer
					}));
				}
				break;
			case SaveType.UpgradeData:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameUpgrades
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/AutoSave_",
						m_fileNameUpgrades
					}));
				}
				break;
			case SaveType.Map:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameMap
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/AutoSave_",
						m_fileNameMap
					}));
				}
				break;
			case SaveType.MapData:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameMapData
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/AutoSave_",
						m_fileNameMapData
					}));
				}
				break;
			case SaveType.Lineage:
				if (m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameLineage
				})))
				{
					m_storageContainer.DeleteFile(string.Concat(new object[]
					{
						"Profile",
						Game.GameConfig.ProfileSlot,
						"/AutoSave_",
						m_fileNameLineage
					}));
				}
				break;
			}
			Console.WriteLine("Backup save file type " + deleteType + " deleted.");
		}
		private void LoadBackups()
		{
			Console.WriteLine("Replacing save file with back up saves");
			GetStorageContainer();
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/AutoSave_",
				m_fileNamePlayer
			})) && m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNamePlayer
			})))
			{
				Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNamePlayer
				}), FileMode.Open);
				Stream stream2 = m_storageContainer.CreateFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNamePlayer
				}));
				stream.CopyTo(stream2);
				stream.Close();
				stream2.Close();
			}
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/AutoSave_",
				m_fileNameUpgrades
			})) && m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameUpgrades
			})))
			{
				Stream stream3 = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameUpgrades
				}), FileMode.Open);
				Stream stream4 = m_storageContainer.CreateFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameUpgrades
				}));
				stream3.CopyTo(stream4);
				stream3.Close();
				stream4.Close();
			}
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/AutoSave_",
				m_fileNameMap
			})) && m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameMap
			})))
			{
				Stream stream5 = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameMap
				}), FileMode.Open);
				Stream stream6 = m_storageContainer.CreateFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMap
				}));
				stream5.CopyTo(stream6);
				stream5.Close();
				stream6.Close();
			}
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/AutoSave_",
				m_fileNameMapData
			})) && m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameMapData
			})))
			{
				Stream stream7 = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameMapData
				}), FileMode.Open);
				Stream stream8 = m_storageContainer.CreateFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMapData
				}));
				stream7.CopyTo(stream8);
				stream7.Close();
				stream8.Close();
			}
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/AutoSave_",
				m_fileNameLineage
			})) && m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameLineage
			})))
			{
				Stream stream9 = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/AutoSave_",
					m_fileNameLineage
				}), FileMode.Open);
				Stream stream10 = m_storageContainer.CreateFile(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameLineage
				}));
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
			string text = m_fileNamePlayer;
			if (saveBackup)
			{
				text = text.Insert(0, "AutoSave_");
			}
			text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
			using (Stream stream = m_storageContainer.CreateFile(text))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					binaryWriter.Write(Game.PlayerStats.Gold);
					Game.PlayerStats.CurrentHealth = Game.ScreenManager.Player.CurrentHealth;
					binaryWriter.Write(Game.PlayerStats.CurrentHealth);
					Game.PlayerStats.CurrentMana = (int)Game.ScreenManager.Player.CurrentMana;
					binaryWriter.Write(Game.PlayerStats.CurrentMana);
					binaryWriter.Write(Game.PlayerStats.Age);
					binaryWriter.Write(Game.PlayerStats.ChildAge);
					binaryWriter.Write(Game.PlayerStats.Spell);
					binaryWriter.Write(Game.PlayerStats.Class);
					binaryWriter.Write(Game.PlayerStats.SpecialItem);
					binaryWriter.Write((byte)Game.PlayerStats.Traits.X);
					binaryWriter.Write((byte)Game.PlayerStats.Traits.Y);
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
					float value = Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength;
					binaryWriter.Write(value);
					binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.X);
					binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.Y);
					binaryWriter.Write((byte)Game.PlayerStats.WizardSpellList.Z);
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
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
						Console.WriteLine(string.Concat(new object[]
						{
							"Traits: ",
							Game.PlayerStats.Traits.X,
							", ",
							Game.PlayerStats.Traits.Y
						}));
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
						Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
						Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
						Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
						Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);
					}
					Console.WriteLine("///// ENEMY LIST DATA - BEGIN SAVING /////");
					List<Vector4> enemiesKilledList = Game.PlayerStats.EnemiesKilledList;
					foreach (Vector4 current in enemiesKilledList)
					{
						binaryWriter.Write((byte)current.X);
						binaryWriter.Write((byte)current.Y);
						binaryWriter.Write((byte)current.Z);
						binaryWriter.Write((byte)current.W);
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Saving Enemy List Data");
						int num = 0;
						foreach (Vector4 current2 in enemiesKilledList)
						{
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Basic, Killed: ",
								current2.X
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Advanced, Killed: ",
								current2.Y
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Expert, Killed: ",
								current2.Z
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Miniboss, Killed: ",
								current2.W
							}));
							num++;
						}
					}
					int count = Game.PlayerStats.EnemiesKilledInRun.Count;
					List<Vector2> enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
					binaryWriter.Write(count);
					foreach (Vector2 current3 in enemiesKilledInRun)
					{
						binaryWriter.Write((int)current3.X);
						binaryWriter.Write((int)current3.Y);
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Saving num enemies killed");
						Console.WriteLine("Number of enemies killed in run: " + count);
						foreach (Vector2 current4 in enemiesKilledInRun)
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
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
						Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
						Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
						Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
						Console.WriteLine("Last Boss Challenge Unlocked: " + Game.PlayerStats.ChallengeLastBossUnlocked);
						Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
						Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
						Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
						Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
						Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
					}
					Console.WriteLine("///// DLC DATA - SAVE COMPLETE /////");
					if (saveBackup)
					{
						FileStream fileStream = stream as FileStream;
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
			string text = m_fileNameUpgrades;
			if (saveBackup)
			{
				text = text.Insert(0, "AutoSave_");
			}
			text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
			using (Stream stream = m_storageContainer.CreateFile(text))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nSaving Equipment States");
					}
					List<byte[]> getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Standard Blueprints");
					}
					foreach (byte[] current in getBlueprintArray)
					{
						byte[] array = current;
						for (int i = 0; i < array.Length; i++)
						{
							byte b = array[i];
							binaryWriter.Write(b);
							if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
							{
								Console.Write(" " + b);
							}
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write("\n");
						}
					}
					List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nRune Blueprints");
					}
					foreach (byte[] current2 in getRuneArray)
					{
						byte[] array2 = current2;
						for (int j = 0; j < array2.Length; j++)
						{
							byte b2 = array2[j];
							binaryWriter.Write(b2);
							if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
							{
								Console.Write(" " + b2);
							}
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write("\n");
						}
					}
					sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nEquipped Standard Item");
					}
					sbyte[] array3 = getEquippedArray;
					for (int k = 0; k < array3.Length; k++)
					{
						sbyte b3 = array3[k];
						binaryWriter.Write(b3);
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + b3);
						}
					}
					sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nEquipped Abilities");
					}
					sbyte[] array4 = getEquippedRuneArray;
					for (int l = 0; l < array4.Length; l++)
					{
						sbyte b4 = array4[l];
						binaryWriter.Write(b4);
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + b4);
						}
					}
					SkillObj[] skillArray = SkillSystem.GetSkillArray();
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nskills");
					}
					SkillObj[] array5 = skillArray;
					for (int m = 0; m < array5.Length; m++)
					{
						SkillObj skillObj = array5[m];
						binaryWriter.Write(skillObj.CurrentLevel);
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + skillObj.CurrentLevel);
						}
					}
					if (saveBackup)
					{
						FileStream fileStream = stream as FileStream;
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
			string text = m_fileNameMap;
			if (saveBackup)
			{
				text = text.Insert(0, "AutoSave_");
			}
			text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
			using (Stream stream = m_storageContainer.CreateFile(text))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nSaving Map");
					}
					int num = 0;
					ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
					if (levelScreen != null)
					{
						if (LevelEV.RUN_DEMO_VERSION)
						{
							binaryWriter.Write(levelScreen.RoomList.Count - 4);
						}
						else
						{
							binaryWriter.Write(levelScreen.RoomList.Count - 12);
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.WriteLine("Map size: " + (levelScreen.RoomList.Count - 12));
						}
						List<byte> list = new List<byte>();
						List<byte> list2 = new List<byte>();
						foreach (RoomObj current in levelScreen.RoomList)
						{
							if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" && current.Name != "Compass" && current.Name != "ChallengeBoss")
							{
								binaryWriter.Write(current.PoolIndex);
								binaryWriter.Write((byte)current.LevelType);
								binaryWriter.Write((int)current.X);
								binaryWriter.Write((int)current.Y);
								binaryWriter.Write(current.TextureColor.R);
								binaryWriter.Write(current.TextureColor.G);
								binaryWriter.Write(current.TextureColor.B);
								if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
								{
									Console.Write(string.Concat(new object[]
									{
										"I:",
										current.PoolIndex,
										" T:",
										(int)current.LevelType,
										", "
									}));
								}
								num++;
								if (num > 5)
								{
									num = 0;
									if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
									{
										Console.Write("\n");
									}
								}
								foreach (EnemyObj current2 in current.EnemyList)
								{
									if (current2.IsProcedural)
									{
										list.Add(current2.Type);
										list2.Add((byte)current2.Difficulty);
									}
								}
							}
						}
						int count = list.Count;
						binaryWriter.Write(count);
						foreach (byte current3 in list)
						{
							binaryWriter.Write(current3);
						}
						using (List<byte>.Enumerator enumerator4 = list2.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								byte current4 = enumerator4.Current;
								binaryWriter.Write(current4);
							}
							goto IL_34E;
						}
					}
					Console.WriteLine("WARNING: Attempting to save LEVEL screen but it was null. Make sure it exists in the screen manager before saving it.");
					IL_34E:
					if (saveBackup)
					{
						FileStream fileStream = stream as FileStream;
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
			string text = m_fileNameMapData;
			if (saveBackup)
			{
				text = text.Insert(0, "AutoSave_");
			}
			text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
			using (Stream stream = m_storageContainer.CreateFile(text))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
					if (levelScreen != null)
					{
						List<RoomObj> mapRoomsAdded = levelScreen.MapRoomsAdded;
						List<bool> list = new List<bool>();
						List<bool> list2 = new List<bool>();
						List<int> list3 = new List<int>();
						List<bool> list4 = new List<bool>();
						List<byte> list5 = new List<byte>();
						List<bool> list6 = new List<bool>();
						List<bool> list7 = new List<bool>();
						List<bool> list8 = new List<bool>();
						foreach (RoomObj current in levelScreen.RoomList)
						{
							if (mapRoomsAdded.Contains(current))
							{
								list.Add(true);
							}
							else
							{
								list.Add(false);
							}
							BonusRoomObj bonusRoomObj = current as BonusRoomObj;
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
								foreach (EnemyObj current2 in current.EnemyList)
								{
									if (current2.IsKilled)
									{
										list7.Add(true);
									}
									else
									{
										list7.Add(false);
									}
								}
							}
							if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" && current.Name != "ChallengeBoss")
							{
								foreach (GameObj current3 in current.GameObjList)
								{
									BreakableObj breakableObj = current3 as BreakableObj;
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
									ChestObj chestObj = current3 as ChestObj;
									if (chestObj != null)
									{
										list5.Add(chestObj.ChestType);
										if (chestObj.IsOpen)
										{
											list4.Add(true);
										}
										else
										{
											list4.Add(false);
										}
										FairyChestObj fairyChestObj = chestObj as FairyChestObj;
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
						foreach (bool current4 in list)
						{
							binaryWriter.Write(current4);
						}
						binaryWriter.Write(list2.Count);
						foreach (bool current5 in list2)
						{
							binaryWriter.Write(current5);
						}
						foreach (int current6 in list3)
						{
							binaryWriter.Write(current6);
						}
						binaryWriter.Write(list5.Count);
						foreach (byte current7 in list5)
						{
							binaryWriter.Write(current7);
						}
						binaryWriter.Write(list4.Count);
						foreach (bool current8 in list4)
						{
							binaryWriter.Write(current8);
						}
						binaryWriter.Write(list6.Count);
						foreach (bool current9 in list6)
						{
							binaryWriter.Write(current9);
						}
						binaryWriter.Write(list7.Count);
						foreach (bool current10 in list7)
						{
							binaryWriter.Write(current10);
						}
						binaryWriter.Write(list8.Count);
						using (List<bool>.Enumerator enumerator11 = list8.GetEnumerator())
						{
							while (enumerator11.MoveNext())
							{
								bool current11 = enumerator11.Current;
								binaryWriter.Write(current11);
							}
							goto IL_4C3;
						}
					}
					Console.WriteLine("WARNING: Attempting to save level screen MAP data but level was null. Make sure it exists in the screen manager before saving it.");
					IL_4C3:
					if (saveBackup)
					{
						FileStream fileStream = stream as FileStream;
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
			string text = m_fileNameLineage;
			if (saveBackup)
			{
				text = text.Insert(0, "AutoSave_");
			}
			text = text.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
			using (Stream stream = m_storageContainer.CreateFile(text))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN SAVING /////");
					List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
					int num = 0;
					if (currentBranches != null)
					{
						num = currentBranches.Count;
						binaryWriter.Write(num);
						for (int i = 0; i < num; i++)
						{
							binaryWriter.Write(currentBranches[i].Name);
							binaryWriter.Write(currentBranches[i].Spell);
							binaryWriter.Write(currentBranches[i].Class);
							binaryWriter.Write(currentBranches[i].HeadPiece);
							binaryWriter.Write(currentBranches[i].ChestPiece);
							binaryWriter.Write(currentBranches[i].ShoulderPiece);
							binaryWriter.Write(currentBranches[i].Age);
							binaryWriter.Write(currentBranches[i].ChildAge);
							binaryWriter.Write((byte)currentBranches[i].Traits.X);
							binaryWriter.Write((byte)currentBranches[i].Traits.Y);
							binaryWriter.Write(currentBranches[i].IsFemale);
						}
					}
					else
					{
						binaryWriter.Write(num);
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Saving Current Branch Lineage Data");
						for (int j = 0; j < num; j++)
						{
							Console.WriteLine("Player Name: " + currentBranches[j].Name);
							Console.WriteLine("Spell: " + currentBranches[j].Name);
							Console.WriteLine("Class: " + currentBranches[j].Name);
							Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
							Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
							Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
							Console.WriteLine("Player Age: " + currentBranches[j].Age);
							Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
							Console.WriteLine(string.Concat(new object[]
							{
								"Traits: ",
								currentBranches[j].Traits.X,
								", ",
								currentBranches[j].Traits.Y
							}));
							Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
						}
					}
					List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
					int num2 = 0;
					if (familyTreeArray != null)
					{
						num2 = familyTreeArray.Count;
						binaryWriter.Write(num2);
						for (int k = 0; k < num2; k++)
						{
							binaryWriter.Write(familyTreeArray[k].Name);
							binaryWriter.Write(familyTreeArray[k].Age);
							binaryWriter.Write(familyTreeArray[k].Class);
							binaryWriter.Write(familyTreeArray[k].HeadPiece);
							binaryWriter.Write(familyTreeArray[k].ChestPiece);
							binaryWriter.Write(familyTreeArray[k].ShoulderPiece);
							binaryWriter.Write(familyTreeArray[k].NumEnemiesBeaten);
							binaryWriter.Write(familyTreeArray[k].BeatenABoss);
							binaryWriter.Write((byte)familyTreeArray[k].Traits.X);
							binaryWriter.Write((byte)familyTreeArray[k].Traits.Y);
							binaryWriter.Write(familyTreeArray[k].IsFemale);
						}
					}
					else
					{
						binaryWriter.Write(num2);
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Saving Family Tree Data");
						Console.WriteLine("Number of Branches: " + num2);
						for (int l = 0; l < num2; l++)
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
							Console.WriteLine(string.Concat(new object[]
							{
								"Traits: ",
								familyTreeArray[l].Traits.X,
								", ",
								familyTreeArray[l].Traits.Y
							}));
							Console.WriteLine("Is Female: " + familyTreeArray[l].IsFemale);
						}
					}
					Console.WriteLine("///// PLAYER LINEAGE DATA - SAVE COMPLETE /////");
					if (saveBackup)
					{
						FileStream fileStream = stream as FileStream;
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
			using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNamePlayer
			}), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
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
					if (Game.PlayerStats.HeadPiece == 0 || Game.PlayerStats.ShoulderPiece == 0 || Game.PlayerStats.ChestPiece == 0)
					{
						throw new Exception("Corrupted Save File");
					}
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
					Game.PlayerStats.TotalHoursPlayed = binaryReader.ReadSingle();
					byte b = binaryReader.ReadByte();
					byte b2 = binaryReader.ReadByte();
					byte b3 = binaryReader.ReadByte();
					Game.PlayerStats.WizardSpellList = new Vector3(b, b2, b3);
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
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
						Console.WriteLine(string.Concat(new object[]
						{
							"Traits: ",
							Game.PlayerStats.Traits.X,
							", ",
							Game.PlayerStats.Traits.Y
						}));
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
						Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
						Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList.X);
						Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList.Y);
						Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList.Z);
					}
					Console.WriteLine("///// ENEMY LIST DATA - BEGIN LOADING /////");
					for (int i = 0; i < 34; i++)
					{
						Vector4 value = new Vector4(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
						Game.PlayerStats.EnemiesKilledList[i] = value;
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Loading Enemy List Data");
						int num = 0;
						foreach (Vector4 current in Game.PlayerStats.EnemiesKilledList)
						{
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Basic, Killed: ",
								current.X
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Advanced, Killed: ",
								current.Y
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Expert, Killed: ",
								current.Z
							}));
							Console.WriteLine(string.Concat(new object[]
							{
								"Enemy Type: ",
								num,
								", Difficulty: Miniboss, Killed: ",
								current.W
							}));
							num++;
						}
					}
					int num2 = binaryReader.ReadInt32();
					for (int j = 0; j < num2; j++)
					{
						Vector2 item = new Vector2(binaryReader.ReadInt32(), binaryReader.ReadInt32());
						Game.PlayerStats.EnemiesKilledInRun.Add(item);
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("Loading num enemies killed");
						Console.WriteLine("Number of enemies killed in run: " + num2);
						foreach (Vector2 current2 in Game.PlayerStats.EnemiesKilledInRun)
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
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
							Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
							Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
							Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
							Console.WriteLine("Last Boss Challenge Unlocked: " + Game.PlayerStats.ChallengeLastBossUnlocked);
							Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
							Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
							Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
							Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
							Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
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
			using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameUpgrades
			}), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nLoading Equipment States");
						Console.WriteLine("\nLoading Standard Blueprints");
					}
					List<byte[]> getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
					for (int i = 0; i < 5; i++)
					{
						for (int j = 0; j < 15; j++)
						{
							getBlueprintArray[i][j] = binaryReader.ReadByte();
							if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
							{
								Console.Write(" " + getBlueprintArray[i][j]);
							}
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write("\n");
						}
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nLoading Ability Blueprints");
					}
					List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
					for (int k = 0; k < 5; k++)
					{
						for (int l = 0; l < 11; l++)
						{
							getRuneArray[k][l] = binaryReader.ReadByte();
							if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
							{
								Console.Write(" " + getRuneArray[k][l]);
							}
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write("\n");
						}
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nLoading Equipped Standard Items");
					}
					sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
					for (int m = 0; m < 5; m++)
					{
						getEquippedArray[m] = binaryReader.ReadSByte();
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + getEquippedArray[m]);
						}
					}
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nLoading Equipped Abilities");
					}
					sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
					for (int n = 0; n < 5; n++)
					{
						getEquippedRuneArray[n] = binaryReader.ReadSByte();
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + getEquippedRuneArray[n]);
						}
					}
					SkillObj[] skillArray = SkillSystem.GetSkillArray();
					if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
					{
						Console.WriteLine("\nLoading Traits");
					}
					SkillSystem.ResetAllTraits();
					Game.PlayerStats.CurrentLevel = 0;
					for (int num = 0; num < 32; num++)
					{
						int num2 = binaryReader.ReadInt32();
						for (int num3 = 0; num3 < num2; num3++)
						{
							SkillSystem.LevelUpTrait(skillArray[num], false);
						}
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.Write(" " + skillArray[num].CurrentLevel);
						}
					}
					binaryReader.Close();
					Game.ScreenManager.Player.UpdateEquipmentColours();
				}
				stream.Close();
				if (Game.PlayerStats.GetNumberOfEquippedRunes(0) > 0 && SkillSystem.GetSkill(SkillType.Enchanter).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION)
				{
					throw new Exception("Corrupted Save file");
				}
				bool flag = false;
				List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
				foreach (FamilyTreeNode current in familyTreeArray)
				{
					if (current.Class > 3)
					{
						flag = true;
						break;
					}
				}
				if (flag && SkillSystem.GetSkill(SkillType.Smithy).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION)
				{
					throw new Exception("Corrupted Save file");
				}
			}
		}
		public ProceduralLevelScreen LoadMap()
		{
			GetStorageContainer();
			ProceduralLevelScreen proceduralLevelScreen = null;
			using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameMap
			}), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					int num = binaryReader.ReadInt32();
					Vector4[] array = new Vector4[num];
					Vector3[] array2 = new Vector3[num];
					for (int i = 0; i < num; i++)
					{
						array[i].W = binaryReader.ReadInt32();
						array[i].X = binaryReader.ReadByte();
						array[i].Y = binaryReader.ReadInt32();
						array[i].Z = binaryReader.ReadInt32();
						array2[i].X = binaryReader.ReadByte();
						array2[i].Y = binaryReader.ReadByte();
						array2[i].Z = binaryReader.ReadByte();
					}
					proceduralLevelScreen = LevelBuilder2.CreateLevel(array, array2);
					int num2 = binaryReader.ReadInt32();
					List<byte> list = new List<byte>();
					for (int j = 0; j < num2; j++)
					{
						list.Add(binaryReader.ReadByte());
					}
					List<byte> list2 = new List<byte>();
					for (int k = 0; k < num2; k++)
					{
						list2.Add(binaryReader.ReadByte());
					}
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
			using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameMapData
			}), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					int num = binaryReader.ReadInt32();
					List<bool> list = new List<bool>();
					for (int i = 0; i < num; i++)
					{
						list.Add(binaryReader.ReadBoolean());
					}
					int num2 = binaryReader.ReadInt32();
					List<bool> list2 = new List<bool>();
					for (int j = 0; j < num2; j++)
					{
						list2.Add(binaryReader.ReadBoolean());
					}
					List<int> list3 = new List<int>();
					for (int k = 0; k < num2; k++)
					{
						list3.Add(binaryReader.ReadInt32());
					}
					int num3 = binaryReader.ReadInt32();
					List<byte> list4 = new List<byte>();
					for (int l = 0; l < num3; l++)
					{
						list4.Add(binaryReader.ReadByte());
					}
					num3 = binaryReader.ReadInt32();
					List<bool> list5 = new List<bool>();
					for (int m = 0; m < num3; m++)
					{
						list5.Add(binaryReader.ReadBoolean());
					}
					num3 = binaryReader.ReadInt32();
					List<bool> list6 = new List<bool>();
					for (int n = 0; n < num3; n++)
					{
						list6.Add(binaryReader.ReadBoolean());
					}
					int num4 = binaryReader.ReadInt32();
					List<bool> list7 = new List<bool>();
					for (int num5 = 0; num5 < num4; num5++)
					{
						list7.Add(binaryReader.ReadBoolean());
					}
					int num6 = binaryReader.ReadInt32();
					List<bool> list8 = new List<bool>();
					for (int num7 = 0; num7 < num6; num7++)
					{
						list8.Add(binaryReader.ReadBoolean());
					}
					int num8 = 0;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					int num12 = 0;
					int num13 = 0;
					foreach (RoomObj current in createdLevel.RoomList)
					{
						if (num2 > 0)
						{
							BonusRoomObj bonusRoomObj = current as BonusRoomObj;
							if (bonusRoomObj != null)
							{
								bool flag = list2[num8];
								int iD = list3[num8];
								num8++;
								if (flag)
								{
									bonusRoomObj.RoomCompleted = true;
								}
								bonusRoomObj.ID = iD;
							}
						}
						if (num4 > 0 && !Game.PlayerStats.LockCastle && current.Name != "Boss" && current.Name != "ChallengeBoss")
						{
							foreach (EnemyObj current2 in current.EnemyList)
							{
								bool flag2 = list7[num12];
								num12++;
								if (flag2)
								{
									current2.KillSilently();
								}
							}
						}
						if (current.Name != "Bonus" && current.Name != "Boss" && current.Name != "Compass" && current.Name != "ChallengeBoss")
						{
							foreach (GameObj current3 in current.GameObjList)
							{
								if (!Game.PlayerStats.LockCastle && num6 > 0)
								{
									BreakableObj breakableObj = current3 as BreakableObj;
									if (breakableObj != null)
									{
										bool flag3 = list8[num13];
										num13++;
										if (flag3)
										{
											breakableObj.ForceBreak();
										}
									}
								}
								ChestObj chestObj = current3 as ChestObj;
								if (chestObj != null)
								{
									chestObj.IsProcedural = false;
									byte chestType = list4[num9];
									num9++;
									chestObj.ChestType = chestType;
									bool flag4 = list5[num10];
									num10++;
									if (flag4)
									{
										chestObj.ForceOpen();
									}
									if (!Game.PlayerStats.LockCastle)
									{
										FairyChestObj fairyChestObj = chestObj as FairyChestObj;
										if (fairyChestObj != null)
										{
											bool flag5 = list6[num11];
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
						List<RoomObj> list9 = new List<RoomObj>();
						int count = list.Count;
						for (int num14 = 0; num14 < count; num14++)
						{
							if (list[num14])
							{
								list9.Add(createdLevel.RoomList[num14]);
							}
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
			using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
			{
				"Profile",
				Game.GameConfig.ProfileSlot,
				"/",
				m_fileNameLineage
			}), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN LOADING /////");
					List<PlayerLineageData> list = new List<PlayerLineageData>();
					int num = binaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
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
					}
					if (list.Count > 0)
					{
						Game.PlayerStats.CurrentBranches = list;
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							Console.WriteLine("Loading Current Branch Lineage Data");
							List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
							for (int j = 0; j < num; j++)
							{
								Console.WriteLine("Player Name: " + currentBranches[j].Name);
								Console.WriteLine("Spell: " + currentBranches[j].Name);
								Console.WriteLine("Class: " + currentBranches[j].Name);
								Console.WriteLine("Head Piece: " + currentBranches[j].HeadPiece);
								Console.WriteLine("Chest Piece: " + currentBranches[j].ChestPiece);
								Console.WriteLine("Shoulder Piece: " + currentBranches[j].ShoulderPiece);
								Console.WriteLine("Player Age: " + currentBranches[j].Age);
								Console.WriteLine("Player Child Age: " + currentBranches[j].ChildAge);
								Console.WriteLine(string.Concat(new object[]
								{
									"Traits: ",
									currentBranches[j].Traits.X,
									", ",
									currentBranches[j].Traits.Y
								}));
								Console.WriteLine("Is Female: " + currentBranches[j].IsFemale);
							}
						}
					}
					List<FamilyTreeNode> list2 = new List<FamilyTreeNode>();
					int num2 = binaryReader.ReadInt32();
					for (int k = 0; k < num2; k++)
					{
						FamilyTreeNode item = default(FamilyTreeNode);
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
						if (LevelEV.SHOW_SAVELOAD_DEBUG_TEXT)
						{
							List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
							Console.WriteLine("Loading Family Tree Data");
							Console.WriteLine("Number of Branches: " + num2);
							for (int l = 0; l < num2; l++)
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
								Console.WriteLine(string.Concat(new object[]
								{
									"Traits: ",
									familyTreeArray[l].Traits.X,
									", ",
									familyTreeArray[l].Traits.Y
								}));
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
			bool flag = true;
			if (m_storageContainer != null && !m_storageContainer.IsDisposed)
			{
				flag = false;
			}
			GetStorageContainer();
			bool result = false;
			switch (saveType)
			{
			case SaveType.PlayerData:
				result = m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNamePlayer
				}));
				break;
			case SaveType.UpgradeData:
				result = m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameUpgrades
				}));
				break;
			case SaveType.Map:
				result = m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMap
				}));
				break;
			case SaveType.MapData:
				result = m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameMapData
				}));
				break;
			case SaveType.Lineage:
				result = m_storageContainer.FileExists(string.Concat(new object[]
				{
					"Profile",
					Game.GameConfig.ProfileSlot,
					"/",
					m_fileNameLineage
				}));
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
		public void GetSaveHeader(byte profile, out byte playerClass, out string playerName, out int playerLevel, out bool playerIsDead, out int castlesBeaten)
		{
			playerName = null;
			playerClass = 0;
			playerLevel = 0;
			playerIsDead = false;
			castlesBeaten = 0;
			GetStorageContainer();
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				profile,
				"/",
				m_fileNamePlayer
			})))
			{
				using (Stream stream = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					profile,
					"/",
					m_fileNamePlayer
				}), FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream))
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
			if (m_storageContainer.FileExists(string.Concat(new object[]
			{
				"Profile",
				profile,
				"/",
				m_fileNameUpgrades
			})))
			{
				using (Stream stream2 = m_storageContainer.OpenFile(string.Concat(new object[]
				{
					"Profile",
					profile,
					"/",
					m_fileNameUpgrades
				}), FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (BinaryReader binaryReader2 = new BinaryReader(stream2))
					{
						for (int i = 0; i < 5; i++)
						{
							for (int j = 0; j < 15; j++)
							{
								binaryReader2.ReadByte();
							}
						}
						for (int k = 0; k < 5; k++)
						{
							for (int l = 0; l < 11; l++)
							{
								binaryReader2.ReadByte();
							}
						}
						for (int m = 0; m < 5; m++)
						{
							binaryReader2.ReadSByte();
						}
						for (int n = 0; n < 5; n++)
						{
							binaryReader2.ReadSByte();
						}
						int num = 0;
						for (int num2 = 0; num2 < 32; num2++)
						{
							int num3 = binaryReader2.ReadInt32();
							for (int num4 = 0; num4 < num3; num4++)
							{
								num++;
							}
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
