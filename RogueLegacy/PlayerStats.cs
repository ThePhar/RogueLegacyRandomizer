using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class PlayerStats : IDisposableObj
    {
        public  List<PlayerLineageData> CurrentBranches;
        public  List<Vector2>           EnemiesKilledInRun;
        public  List<Vector4>           EnemiesKilledList;
        public  List<FamilyTreeNode>    FamilyTreeArray;
        private int                     _gold;

        public PlayerStats()
        {
            // Complete the tutorial so we don't have to watch it.
            TutorialComplete = true;

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
            for (var i = 0; i < 34; i++) EnemiesKilledList.Add(default(Vector4));
            WizardSpellList = new Vector3(1f, 2f, 8f);
            Traits = new Vector2(0f, 0f);
            Gold = 0;
            CurrentLevel = 0;
            HeadPiece = 1;
            ShoulderPiece = 1;
            ChestPiece = 1;
            LoadStartingRoom = true;
            GetBlueprintArray = new List<byte[]>();
            GetRuneArray = new List<byte[]>();
            GetEquippedArray = new sbyte[5];
            GetEquippedRuneArray = new sbyte[5];
            FamilyTreeArray = new List<FamilyTreeNode>();
            InitializeFirstChild();
            EnemiesKilledInRun = new List<Vector2>();
            CurrentBranches = null;
            for (var j = 0; j < 5; j++)
            {
                GetBlueprintArray.Add(new byte[15]);
                GetRuneArray.Add(new byte[11]);
                GetEquippedArray[j] = -1;
                GetEquippedRuneArray[j] = -1;
            }

            HeadPiece = (byte) CDGMath.RandomInt(1, 5);
            ShoulderPiece = (byte) CDGMath.RandomInt(1, 5);
            ChestPiece = (byte) CDGMath.RandomInt(1, 5);
            CDGMath.RandomInt(0, 14);

            ReceivedItems = new List<NetworkItem>();
        }

        public int CurrentLevel   { get; set; }
        public int FountainPieces { get; set; }

        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                if (_gold < 0)
                {
                    _gold = 0;
                }
            }
        }

        public int               CurrentHealth             { get; set; }
        public int               CurrentMana               { get; set; }
        public byte              Age                       { get; set; }
        public byte              ChildAge                  { get; set; }
        public byte              Spell                     { get; set; }
        public byte              Class                     { get; set; }
        public byte              SpecialItem               { get; set; }
        public Vector2           Traits                    { get; set; }
        public string            PlayerName                { get; set; }
        public byte              HeadPiece                 { get; set; }
        public byte              ShoulderPiece             { get; set; }
        public byte              ChestPiece                { get; set; }
        public byte              DiaryEntry                { get; set; }
        public int               BonusHealth               { get; set; }
        public int               BonusStrength             { get; set; }
        public int               BonusMana                 { get; set; }
        public int               BonusDefense              { get; set; }
        public int               BonusWeight               { get; set; }
        public int               BonusMagic                { get; set; }
        public int               LichHealth                { get; set; }
        public int               LichMana                  { get; set; }
        public float             LichHealthMod             { get; set; }
        public bool              NewBossBeaten             { get; set; }
        public bool              EyeballBossBeaten         { get; set; }
        public bool              FairyBossBeaten           { get; set; }
        public bool              FireballBossBeaten        { get; set; }
        public bool              BlobBossBeaten            { get; set; }
        public bool              LastbossBeaten            { get; set; }
        public int               TimesCastleBeaten         { get; set; }
        public int               NumEnemiesBeaten          { get; set; }
        public bool              TutorialComplete          { get; set; }
        public bool              CharacterFound            { get; set; }
        public bool              LoadStartingRoom          { get; set; }
        public bool              LockCastle                { get; set; }
        public bool              SpokeToBlacksmith         { get; set; } = true;
        public bool              SpokeToEnchantress        { get; set; } = true;
        public bool              SpokeToArchitect          { get; set; } = true;
        public bool              SpokeToTollCollector      { get; set; } = true;
        public bool              IsDead                    { get; set; }
        public bool              FinalDoorOpened           { get; set; }
        public bool              RerolledChildren          { get; set; }
        public bool              IsFemale                  { get; set; }
        public int               TimesDead                 { get; set; }
        public bool              HasArchitectFee           { get; set; }
        public bool              ReadLastDiary             { get; set; }
        public bool              SpokenToLastBoss          { get; set; }
        public bool              HardcoreMode              { get; set; }
        public float             TotalHoursPlayed          { get; set; }
        public Vector3           WizardSpellList           { get; set; }
        public bool              ChallengeEyeballUnlocked  { get; set; }
        public bool              ChallengeFireballUnlocked { get; set; }
        public bool              ChallengeBlobUnlocked     { get; set; }
        public bool              ChallengeSkullUnlocked    { get; set; }
        public bool              ChallengeLastBossUnlocked { get; set; }
        public bool              ChallengeEyeballBeaten    { get; set; }
        public bool              ChallengeFireballBeaten   { get; set; }
        public bool              ChallengeBlobBeaten       { get; set; }
        public bool              ChallengeSkullBeaten      { get; set; }
        public bool              ChallengeLastBossBeaten   { get; set; }
        public List<byte[]>      GetBlueprintArray         { get; private set; }
        public sbyte[]           GetEquippedArray          { get; private set; }
        public List<NetworkItem> ReceivedItems             { get; set; }

        public bool HasNotReceivedItem(NetworkItem item)
        {
            var nItem = item;

            foreach (var rItem in ReceivedItems)
            {
                if (rItem.Item == nItem.Item && rItem.Location == nItem.Location && rItem.Player == nItem.Player)
                {
                    // We already received this item.
                    return false;
                }
            }

            return true;
        }

        public byte TotalBlueprintsPurchased
        {
            get
            {
                byte b = 0;
                foreach (var current in GetBlueprintArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b2 = array[i];
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
                foreach (var current in GetRuneArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b2 = array[i];
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
                foreach (var current in GetBlueprintArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b2 = array[i];
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
                foreach (var current in GetRuneArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b2 = array[i];
                        if (b2 >= 1)
                        {
                            b += 1;
                        }
                    }
                }

                return b;
            }
        }

        public List<byte[]> GetRuneArray { get; private set; }
        public sbyte[] GetEquippedRuneArray { get; private set; }
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                GetBlueprintArray.Clear();
                GetBlueprintArray = null;
                GetRuneArray.Clear();
                GetRuneArray = null;
                IsDisposed = true;
            }
        }

        private void InitializeFirstChild()
        {
            var item = new FamilyTreeNode
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

        public byte GetNumberOfEquippedRunes(int equipmentAbilityType)
        {
            byte b = 0;
            if (LevelENV.UnlockAllAbilities)
            {
                return 5;
            }

            var equippedRuneArray = GetEquippedRuneArray;
            for (var i = 0; i < equippedRuneArray.Length; i++)
            {
                var b2 = equippedRuneArray[i];
                if (b2 == equipmentAbilityType)
                {
                    b += 1;
                }
            }

            return b;
        }
    }
}
