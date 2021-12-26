// 
// RogueLegacyArchipelago - ArchClient.cs
// Last Modified 2021-12-26
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using DS2DEngine;
using RogueCastle.TypeDefinitions;

namespace RogueCastle.Archipelago
{
    public class ArchClient
    {
        private ArchipelagoSession m_session;
        private DeathLinkService m_deathLinkService;
        private string m_seed;
        private int m_slot;
        private NetworkPlayer m_player;
        private SlotData m_data;
        private List<string> m_tags;
        private List<int> m_missingLocations;
        private List<int> m_checkedLocations;

        public int TestNumber = 0;

        public bool IsConnected
        {
            get
            {
                return m_session != null && m_session.Socket.Connected;
            }
        }

        public Dictionary<int, bool> CheckedLocations { get; set; }
        public Dictionary<int, NetworkItem> LocationCache { get; set; }
        public ArchipelagoSession Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// Attempts to create a new session and connect
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="slotName"></param>
        /// <param name="password"></param>
        public void Connect(string hostname, int port, string slotName, string password = null)
        {
            // Disconnect our current session if we are connected.
            if (IsConnected)
                m_session.Socket.Disconnect();

            try
            {
                m_session = ArchipelagoSessionFactory.CreateSession(hostname, port);

                // Establish handlers.
                m_session.Socket.PacketReceived += PacketReceivedHandler;

                // Attempt to connect to AP.
                var result = m_session.TryConnectAndLogin("Rogue Legacy", slotName, LevelEV.AP_VERSION, m_tags, password: password);

                // Testing something.
                m_session.Socket.SendPacket(new GetDataPackagePacket());

                // Error checking.
                if (!result.Successful)
                {
                    var failure = (LoginFailure) result;
                    var screenManager = Game.ScreenManager;
                    var errorUuid = Guid.NewGuid().ToString();

                    // Add why we failed to connect.
                    Console.WriteLine("Failed to connect to server.");
                    DialogueManager.AddText(errorUuid, Enumerable.Repeat("Failed to Connect", failure.Errors.Length).ToArray(), failure.Errors);
                    screenManager.DialogueScreen.SetDialogue(errorUuid);
                    screenManager.DisplayScreen(ScreenType.Dialogue, true);
                }
            }
            catch (Exception ex)
            {
                var screenManager = Game.ScreenManager;
                var errorUuid = Guid.NewGuid().ToString();

                // Print exception message.
                Console.WriteLine(ex);
                DialogueManager.AddText(errorUuid, new []{"An Exception Occurred"}, new []{ex.Message});
                screenManager.DialogueScreen.SetDialogue(errorUuid);
                screenManager.DisplayScreen(ScreenType.Dialogue, true);
            }
        }

        /// <summary>
        /// Disconnects from a currently connected AP server and re-initializes ArchClient fields.
        /// </summary>
        public void DisconnectAndReset()
        {
            if (IsConnected)
                m_session.Socket.Disconnect();

            m_session = null;
            m_deathLinkService = null;
            m_seed = null;
            m_slot = 1;
            m_player = new NetworkPlayer();
            m_data = null;
            m_tags = new List<string>{"AP"};
            m_missingLocations = new List<int>();
            m_checkedLocations = new List<int>();
            LocationCache = new Dictionary<int, NetworkItem>();

            // Load Save Data
            LoadSaveData();
        }

        /// <summary>
        /// Starts a the game.
        /// </summary>
        public void StartGame()
        {
            SoundManager.PlaySound("Game_Start");

            var newGame = !Game.PlayerStats.CharacterFound;
            var heroIsDead = Game.PlayerStats.IsDead;
            var startingRoom = Game.PlayerStats.LoadStartingRoom;

            if (newGame)
            {
                Game.PlayerStats.CharacterFound = true;
                Game.PlayerStats.Gold = 0;
                Game.PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                Game.PlayerStats.EnemiesKilledInRun.Clear();
                Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
                Game.ScreenManager.DisplayScreen(ScreenType.StartingRoom, true);
            }
            else
            {
                if (heroIsDead)
                    Game.ScreenManager.DisplayScreen(ScreenType.Lineage, true);
                else
                    Game.ScreenManager.DisplayScreen(startingRoom ? ScreenType.StartingRoom : ScreenType.Level, true);
            }

            SoundManager.StopMusic(0.2f);
        }

        /// <summary>
        /// Main packet receiver handler.
        /// </summary>
        /// <param name="basePacket">The packet sent to the client.</param>
        private void PacketReceivedHandler(ArchipelagoPacketBase basePacket)
        {
            // Connected Packet Handler
            var connected = basePacket as ConnectedPacket;
            if (connected != null)
                ConnectedPacketHandler(connected);

            // RoomInfo Packet Handler
            var roomInfo = basePacket as RoomInfoPacket;
            if (roomInfo != null)
                RoomInfoPacketHandler(roomInfo);
        }

        /// <summary>
        /// Handler for connected packets.
        /// </summary>
        /// <param name="packet">Connected Packet</param>
        private void ConnectedPacketHandler(ConnectedPacket packet)
        {
            m_slot = packet.Slot;
            m_missingLocations = packet.MissingChecks;
            m_checkedLocations = packet.LocationsChecked;
            CreateLocationsDictionary();

            // Create save file if it doesn't exist.
            Game.ChangeSlot(m_seed, m_slot);
            Program.Game.SaveManager.CreateSaveDirectory();

            // Load player/slot specific data.
            m_player = packet.Players[m_slot - 1];
            m_data = new SlotData(packet.SlotData);

            // Rename Sir Lee to the player's name and initial gender if we're on our first character.
            if (Game.PlayerStats.CurrentBranches == null)
            {
                Game.PlayerStats.IsFemale = m_data.StartingGender == StartingGender.Lady;
                Game.PlayerStats.PlayerName = string.Format("{1} {0}", m_player.Name, Game.PlayerStats.IsFemale ? "Lady" : "Sir");
            }

            // Our our player's name to the name pool, so their potential children can be named after them.
            if (Game.PlayerStats.IsFemale && !Game.FemaleNameArray.Contains(m_player.Name))
                Game.FemaleNameArray.Add(m_player.Name);
            else if (!Game.PlayerStats.IsFemale && !Game.NameArray.Contains(m_player.Name))
                Game.NameArray.Add(m_player.Name);

            // Load Settings
            SetSettings();

            // Load DeathLink
            if (m_data.DeathLink)
            {
                m_tags.Add("DeathLink");
                m_session.UpdateTags(m_tags);
                m_deathLinkService = m_session.CreateDeathLinkServiceAndEnable();

                // Attach to death link handler.
                m_deathLinkService.OnDeathLinkReceived += DeathLinkHandler;
            }

            // m_session.Locations.ScoutLocationsAsync(PrepareLocationsAndStart, fairyChests.ToArray());

            // Load the game.
            StartGame();
        }

        private void PrepareLocationsAndStart(LocationInfoPacket locationInfo)
        {
            var locations = locationInfo.Locations;
            foreach (var networkItem in locations)
            {
                Console.WriteLine("{0} - {1} - {2}", networkItem.Item, networkItem.Location, networkItem.Player);
                LocationCache.Add(networkItem.Location, networkItem);
            }
        }

        /// <summary>
        /// Handler for room info packets.
        /// </summary>
        /// <param name="packet">RoomInfo Packet</param>
        private void RoomInfoPacketHandler(RoomInfoPacket packet)
        {
            m_seed = packet.SeedName;
        }

        /// <summary>
        /// Handler for DeathLink messages.
        /// </summary>
        /// <param name="deathLink">DeathLink Message</param>
        private void DeathLinkHandler(DeathLink deathLink)
        {
            // Do not death link on ourselves.
            if (deathLink.Source == m_player.Name)
                return;

            Console.WriteLine("Received a DeathLink!");
            var screen = Game.ScreenManager.GetLevelScreen();
            if (screen != null && screen.Player != null)
            {
                // TODO: Create custom object for handling outside players for DeathLink.
                screen.Player.Kill();
            }
        }

        /// <summary>
        /// Set our settings into our game.
        /// </summary>
        private void SetSettings()
        {
            Game.PlayerStats.TimesCastleBeaten = m_data.Difficulty;
        }

        /// <summary>
        /// Create a locations dictionary to keep track of what locations we opened and which we did not.
        /// </summary>
        private void CreateLocationsDictionary()
        {
            CheckedLocations = new Dictionary<int, bool>();

            foreach (var location in m_missingLocations)
            {
                CheckedLocations.Add(location, false);
            }

            foreach (var location in m_checkedLocations)
            {
                CheckedLocations.Add(location, true);
            }
        }

        /// <summary>
        /// Load Save Data for this slot.
        /// </summary>
        private static void LoadSaveData()
        {
            Game.PlayerStats.TutorialComplete = true;
            Game.NameArray = Game.DefaultNameArray.Clone();
            Game.FemaleNameArray = Game.DefaultFemaleNameArray.Clone();
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            Program.Game.SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
            Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
        }
    }
}
