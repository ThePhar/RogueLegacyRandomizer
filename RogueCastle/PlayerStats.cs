/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class PlayerStats : IDisposableObj
	{
		private int m_gold;
		private List<byte[]> m_blueprintArray;
		private List<byte[]> m_runeArray;
		private sbyte[] m_equippedArray;
		private sbyte[] m_equippedRuneArray;
		public List<PlayerLineageData> CurrentBranches;
		public List<FamilyTreeNode> FamilyTreeArray;
		public List<Vector4> EnemiesKilledList;
		public List<Vector2> EnemiesKilledInRun;
		private bool m_isDisposed;
		public int CurrentLevel
		{
			get;
			set;
		}
		public int Gold
		{
			get
			{
				return m_gold;
			}
			set
			{
				m_gold = value;
				if (m_gold < 0)
				{
					m_gold = 0;
				}
			}
		}
		public int CurrentHealth
		{
			get;
			set;
		}
		public int CurrentMana
		{
			get;
			set;
		}
		public byte Age
		{
			get;
			set;
		}
		public byte ChildAge
		{
			get;
			set;
		}
		public byte Spell
		{
			get;
			set;
		}
		public byte Class
		{
			get;
			set;
		}
		public byte SpecialItem
		{
			get;
			set;
		}
		public Vector2 Traits
		{
			get;
			set;
		}
		public string PlayerName
		{
			get;
			set;
		}
		public byte HeadPiece
		{
			get;
			set;
		}
		public byte ShoulderPiece
		{
			get;
			set;
		}
		public byte ChestPiece
		{
			get;
			set;
		}
		public byte DiaryEntry
		{
			get;
			set;
		}
		public int BonusHealth
		{
			get;
			set;
		}
		public int BonusStrength
		{
			get;
			set;
		}
		public int BonusMana
		{
			get;
			set;
		}
		public int BonusDefense
		{
			get;
			set;
		}
		public int BonusWeight
		{
			get;
			set;
		}
		public int BonusMagic
		{
			get;
			set;
		}
		public int LichHealth
		{
			get;
			set;
		}
		public int LichMana
		{
			get;
			set;
		}
		public float LichHealthMod
		{
			get;
			set;
		}
		public bool NewBossBeaten
		{
			get;
			set;
		}
		public bool EyeballBossBeaten
		{
			get;
			set;
		}
		public bool FairyBossBeaten
		{
			get;
			set;
		}
		public bool FireballBossBeaten
		{
			get;
			set;
		}
		public bool BlobBossBeaten
		{
			get;
			set;
		}
		public bool LastbossBeaten
		{
			get;
			set;
		}
		public int TimesCastleBeaten
		{
			get;
			set;
		}
		public int NumEnemiesBeaten
		{
			get;
			set;
		}
		public bool TutorialComplete
		{
			get;
			set;
		}
		public bool CharacterFound
		{
			get;
			set;
		}
		public bool LoadStartingRoom
		{
			get;
			set;
		}
		public bool LockCastle
		{
			get;
			set;
		}
		public bool SpokeToBlacksmith
		{
			get;
			set;
		}
		public bool SpokeToEnchantress
		{
			get;
			set;
		}
		public bool SpokeToArchitect
		{
			get;
			set;
		}
		public bool SpokeToTollCollector
		{
			get;
			set;
		}
		public bool IsDead
		{
			get;
			set;
		}
		public bool FinalDoorOpened
		{
			get;
			set;
		}
		public bool RerolledChildren
		{
			get;
			set;
		}
		public bool IsFemale
		{
			get;
			set;
		}
		public int TimesDead
		{
			get;
			set;
		}
		public bool HasArchitectFee
		{
			get;
			set;
		}
		public bool ReadLastDiary
		{
			get;
			set;
		}
		public bool SpokenToLastBoss
		{
			get;
			set;
		}
		public bool HardcoreMode
		{
			get;
			set;
		}
		public float TotalHoursPlayed
		{
			get;
			set;
		}
		public Vector3 WizardSpellList
		{
			get;
			set;
		}
		public bool ChallengeEyeballUnlocked
		{
			get;
			set;
		}
		public bool ChallengeFireballUnlocked
		{
			get;
			set;
		}
		public bool ChallengeBlobUnlocked
		{
			get;
			set;
		}
		public bool ChallengeSkullUnlocked
		{
			get;
			set;
		}
		public bool ChallengeLastBossUnlocked
		{
			get;
			set;
		}
		public bool ChallengeEyeballBeaten
		{
			get;
			set;
		}
		public bool ChallengeFireballBeaten
		{
			get;
			set;
		}
		public bool ChallengeBlobBeaten
		{
			get;
			set;
		}
		public bool ChallengeSkullBeaten
		{
			get;
			set;
		}
		public bool ChallengeLastBossBeaten
		{
			get;
			set;
		}
		public bool IsDisposed
		{
			get
			{
				return m_isDisposed;
			}
		}
		public List<byte[]> GetBlueprintArray
		{
			get
			{
				return m_blueprintArray;
			}
		}
		public sbyte[] GetEquippedArray
		{
			get
			{
				return m_equippedArray;
			}
		}
		public byte TotalBlueprintsPurchased
		{
			get
			{
				byte b = 0;
				foreach (byte[] current in GetBlueprintArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b2 = array[i];
						if (b2 >= 3)
						{
							b += 1;
						}
					}
				}
				return b;
			}
		}
		public byte TotalRunesPurchased
		{
			get
			{
				byte b = 0;
				foreach (byte[] current in GetRuneArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b2 = array[i];
						if (b2 >= 3)
						{
							b += 1;
						}
					}
				}
				return b;
			}
		}
		public byte TotalBlueprintsFound
		{
			get
			{
				byte b = 0;
				foreach (byte[] current in GetBlueprintArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b2 = array[i];
						if (b2 >= 1)
						{
							b += 1;
						}
					}
				}
				return b;
			}
		}
		public byte TotalRunesFound
		{
			get
			{
				byte b = 0;
				foreach (byte[] current in GetRuneArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b2 = array[i];
						if (b2 >= 1)
						{
							b += 1;
						}
					}
				}
				return b;
			}
		}
		public List<byte[]> GetRuneArray
		{
			get
			{
				return m_runeArray;
			}
		}
		public sbyte[] GetEquippedRuneArray
		{
			get
			{
				return m_equippedRuneArray;
			}
		}
		public PlayerStats()
		{
			if (!LevelEV.RUN_TUTORIAL && !TutorialComplete && LevelEV.RUN_TESTROOM)
			{
				TutorialComplete = true;
			}
			PlayerName = "Sir Lee";
			SpecialItem = 0;
			Class = 0;
			Spell = 1;
			Age = 30;
			ChildAge = 5;
			LichHealthMod = 1f;
			IsFemale = false;
			TimesCastleBeaten = 0;
			EnemiesKilledList = new List<Vector4>();
			for (int i = 0; i < 34; i++)
			{
				EnemiesKilledList.Add(default(Vector4));
			}
			WizardSpellList = new Vector3(1f, 2f, 8f);
			Traits = new Vector2(0f, 0f);
			Gold = 0;
			CurrentLevel = 0;
			HeadPiece = 1;
			ShoulderPiece = 1;
			ChestPiece = 1;
			LoadStartingRoom = true;
			m_blueprintArray = new List<byte[]>();
			m_runeArray = new List<byte[]>();
			m_equippedArray = new sbyte[5];
			m_equippedRuneArray = new sbyte[5];
			FamilyTreeArray = new List<FamilyTreeNode>();
			InitializeFirstChild();
			EnemiesKilledInRun = new List<Vector2>();
			CurrentBranches = null;
			for (int j = 0; j < 5; j++)
			{
				m_blueprintArray.Add(new byte[15]);
				m_runeArray.Add(new byte[11]);
				m_equippedArray[j] = -1;
				m_equippedRuneArray[j] = -1;
			}
			HeadPiece = (byte)CDGMath.RandomInt(1, 5);
			ShoulderPiece = (byte)CDGMath.RandomInt(1, 5);
			ChestPiece = (byte)CDGMath.RandomInt(1, 5);
			CDGMath.RandomInt(0, 14);
			m_blueprintArray[1][0] = 1;
			m_blueprintArray[3][0] = 1;
			m_blueprintArray[0][0] = 1;
			m_runeArray[1][0] = 1;
			m_runeArray[0][1] = 1;
		}
		private void InitializeFirstChild()
		{
			FamilyTreeNode item = new FamilyTreeNode
			{
				Name = "Johannes",
				Age = 30,
				ChildAge = 20,
				Class = 0,
				HeadPiece = 8,
				ChestPiece = 1,
				ShoulderPiece = 1,
				NumEnemiesBeaten = 0,
				BeatenABoss = true,
				IsFemale = false,
				Traits = Vector2.Zero
			};
			FamilyTreeArray.Add(item);
		}
		public void Dispose()
		{
			if (!IsDisposed)
			{
				m_blueprintArray.Clear();
				m_blueprintArray = null;
				m_runeArray.Clear();
				m_runeArray = null;
				m_isDisposed = true;
			}
		}
		public byte GetNumberOfEquippedRunes(int equipmentAbilityType)
		{
			byte b = 0;
			if (LevelEV.UNLOCK_ALL_ABILITIES)
			{
				return 5;
			}
			sbyte[] equippedRuneArray = m_equippedRuneArray;
			for (int i = 0; i < equippedRuneArray.Length; i++)
			{
				sbyte b2 = equippedRuneArray[i];
				if (b2 == equipmentAbilityType)
				{
					b += 1;
				}
			}
			return b;
		}
	}
}
