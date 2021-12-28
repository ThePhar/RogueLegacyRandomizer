// 
// RogueLegacyArchipelago - ArchipelagoClient.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Archipelago.Legacy;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using WebSocketSharp;

namespace Archipelago
{
    public class ArchipelagoClient
    {
        public readonly Version APVersion = Version.Parse("0.2.2");

        public ArchipelagoStatus            Status               { get; private set; }
        public DateTime                     LastDeath            { get; private set; }
        public DeathLink                    DeathLink            { get; private set; }
        public Dictionary<int, NetworkItem> LocationCache        { get; private set; }
        public LegacySlotData               Data                 { get; private set; }
        public Queue<NetworkItem>           ItemQueue            { get; private set; }
        public ConnectionInfo               CachedConnectionInfo { get; private set; }

        public static readonly ReadOnlyDictionary<string, int> LegacyLocations =
            new ReadOnlyDictionary<string, int>(new LegacyLocations());
        public static readonly ReadOnlyDictionary<string, int> LegacyItems =
            new ReadOnlyDictionary<string, int>(new LegacyItems());

        private ArchipelagoSession m_session;
        private DeathLinkService   m_deathLink;
        private List<string>       m_tags;
        private string             m_seed;

        public ArchipelagoClient()
        {
            Initialize();
        }

        /// <summary>
        /// Create a new session and attempt to connect to an AP server.
        /// </summary>
        /// <param name="info">Aggregate Connection Information</param>
        /// <exception cref="ArchipelagoSocketClosedException">
        /// Thrown if function failed to connect to the requested AP server.
        /// </exception>
        public void Connect(ConnectionInfo info)
        {
            // Cache our connection info in case we get disconnected later.
            CachedConnectionInfo = info;

            // Disconnect from any session we are currently in if we are attempting to connect.
            if (m_session != null)
                Disconnect();

            Status = ArchipelagoStatus.Connecting;
            try
            {
                m_session = ArchipelagoSessionFactory.CreateSession(info.Hostname, info.Port);

                // Establish event handlers.
                m_session.Socket.SocketClosed += OnSocketDisconnect;
                m_session.Socket.ErrorReceived += OnError;
                m_session.Items.ItemReceived += OnReceivedItems;
                m_session.Socket.PacketReceived += OnPacketReceived;

                // Attempt to connect to the AP server.
                var result = m_session.TryConnectAndLogin("Rogue Legacy", info.Name, APVersion, m_tags,
                    password: info.Password);

                if (result.Successful)
                {
                    Status = ArchipelagoStatus.Connected;
                    return;
                }

                var failure = (LoginFailure) result;
                throw new ArchipelagoSocketClosedException(failure.Errors[0]);
            }
            catch
            {
                Disconnect();
                throw;
            }
        }

        /// <summary>
        /// Explicitly disconnect from the current session and clean up.
        /// </summary>
        public void Disconnect()
        {
            Status = ArchipelagoStatus.Disconnecting;

            if (m_session != null)
            {
                m_session.Socket.SocketClosed -= OnSocketDisconnect;
                m_session.Socket.ErrorReceived -= OnError;
                m_session.Items.ItemReceived -= OnReceivedItems;
                m_session.Socket.PacketReceived -= OnPacketReceived;
                m_session.Socket.Disconnect();
            }

            if (m_deathLink != null)
                m_deathLink.OnDeathLinkReceived -= OnDeathLink;

            Initialize();
        }

        /// <summary>
        /// Initializes all fields and properties to their default state.
        /// </summary>
        private void Initialize()
        {
            Status = ArchipelagoStatus.Disconnected;
            DeathLink = null;
            LocationCache = new Dictionary<int, NetworkItem>();
            Data = null;
            ItemQueue = new Queue<NetworkItem>();
            LastDeath = DateTime.MinValue;

            m_session = null;
            m_deathLink = null;
            m_tags = new List<string> { "AP" };
            m_seed = "0";
        }

        /// <summary>
        /// Clears the DeathLink object.
        /// </summary>
        public void ClearDeathLink()
        {
            if (m_deathLink != null)
                DeathLink = null;
        }

        /// <summary>
        /// Send a DeathLink.
        /// </summary>
        public void SendDeathLink(string cause)
        {
            // Log our current time so we can make sure we ignore our own DeathLink.
            LastDeath = DateTime.Now;

            if (Data.DeathLink && m_deathLink != null)
            {
                var causeWithPlayerName = string.Format("{0}'s {1}.", m_session.Players.GetPlayerAlias(Data.Slot) , cause);
                m_deathLink.SendDeathLink(new DeathLink(m_session.Players.GetPlayerAlias(Data.Slot), causeWithPlayerName)
                {
                    Timestamp = LastDeath,
                });
            }
        }

        /// <summary>
        /// Sets the current status as playing.
        /// </summary>
        public void StartPlaying()
        {
            Status = ArchipelagoStatus.Playing;
        }

        /// <summary>
        /// Tell the AP server that we just completed a location.
        /// </summary>
        /// <param name="locations"></param>
        public void CheckLocations(params int[] locations)
        {
            m_session.Locations.CompleteLocationChecks(locations);
        }

        /// <summary>
        /// Return the string representation for a particular player.
        /// </summary>
        /// <param name="slot">Player Slot ID</param>
        /// <returns></returns>
        public string GetPlayerName(int slot)
        {
            var name = m_session.Players.GetPlayerAliasAndName(slot);
            return string.IsNullOrEmpty(name) ? "Archipelago" : name;
        }

        /// <summary>
        /// Returns the string representation for a particular item.
        /// </summary>
        /// <param name="item">Item ID</param>
        /// <returns></returns>
        public string GetItemName(int item)
        {
            return m_session.Items.GetItemName(item);
        }

        /// <summary>
        /// Handle disconnect events from AP server.
        /// </summary>
        /// <param name="closeEventArgs"></param>
        private void OnSocketDisconnect(CloseEventArgs closeEventArgs)
        {
            switch (Status)
            {
                case ArchipelagoStatus.Disconnected:
                case ArchipelagoStatus.Disconnecting:
                    break;

                // Attempt to re-establish a connection.
                case ArchipelagoStatus.Playing:
                case ArchipelagoStatus.Connecting:
                case ArchipelagoStatus.Initialized:
                case ArchipelagoStatus.FetchingLocations:
                case ArchipelagoStatus.Connected:
                case ArchipelagoStatus.Reconnecting:
                    Status = ArchipelagoStatus.Reconnecting;
                    Connect(CachedConnectionInfo);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Queues all received items onto the Item Queue.
        /// </summary>
        /// <param name="helper">Received Items Helper</param>
        private void OnReceivedItems(ReceivedItemsHelper helper)
        {
            ItemQueue.Enqueue(helper.DequeueItem());
        }

        /// <summary>
        /// Intercepts DeathLink Bounce packets and sets our DeathLink flag if the player should die.
        /// </summary>
        /// <param name="deathLink"></param>
        private void OnDeathLink(DeathLink deathLink)
        {
            Console.WriteLine("REC: {0}", deathLink.Timestamp);
            Console.WriteLine("LST: {0}", LastDeath);
            Console.WriteLine(deathLink.Timestamp.ToString(CultureInfo.InvariantCulture) != LastDeath.ToString(CultureInfo.InvariantCulture));

            // Ignore deaths that died at the same time as us. Should also prevent the player from dying to themselves.
            if (deathLink.Timestamp.ToString(CultureInfo.InvariantCulture) != LastDeath.ToString(CultureInfo.InvariantCulture))
            {
                DeathLink = deathLink;
            }
        }

        /// <summary>
        /// Checks the type of packet being received and sends it to the appropriate packet handler function.
        /// </summary>
        /// <param name="packet">Arbitrary AP Server Packet</param>
        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            Console.WriteLine("Received a {0} packet", packet.GetType().Name);
            Console.WriteLine("==============================");
            foreach (var property in packet.GetType().GetProperties())
            {
                Console.WriteLine("{0}: {1}", property.Name, property.GetValue(packet));
            }
            Console.WriteLine();

            if (packet is RoomInfoPacket)
                OnRoomInfo((RoomInfoPacket) packet);
            else if (packet is ConnectedPacket)
                OnConnected((ConnectedPacket) packet);
            else if (packet is PrintPacket)
                OnPrint((PrintPacket) packet);
        }

        /// <summary>
        /// Set relevant data from RoomInfoPacket.
        /// </summary>
        /// <param name="packet">RoomInfo Packet</param>
        private void OnRoomInfo(RoomInfoPacket packet)
        {
            m_seed = packet.SeedName;

            // Send this so we have a cache of item/location names.
            m_session.Socket.SendPacket(new GetDataPackagePacket());
        }

        /// <summary>
        /// Gets our slot data and prepares any additional information that will be used by AP Client.
        /// </summary>
        /// <param name="packet">Connected Packet</param>
        private void OnConnected(ConnectedPacket packet)
        {
            Data = new LegacySlotData(packet.SlotData, m_seed, packet.Slot, CachedConnectionInfo.Name);

            // Check if DeathLink is enabled and establish the appropriate helper.
            if (Data.DeathLink)
            {
                m_tags.Add("DeathLink");
                m_session.UpdateTags(m_tags);

                // Clear old DeathLink handlers.
                if (m_deathLink != null)
                    m_deathLink.OnDeathLinkReceived -= OnDeathLink;

                m_deathLink = m_session.CreateDeathLinkServiceAndEnable();
                m_deathLink.OnDeathLinkReceived += OnDeathLink;
            }

            // Build our location cache.
            m_session.Locations.ScoutLocationsAsync(OnReceiveLocationCache, new LegacyLocations().Values.ToArray());
        }

        /// <summary>
        /// Insert our compiled locations into our location cache so we can look up item information easily.
        /// </summary>
        private void OnReceiveLocationCache(LocationInfoPacket packet)
        {
            foreach (var item in packet.Locations)
            {
                LocationCache.Add(item.Location, item);
            }
        }

        /// <summary>
        /// Handles all exceptions within our Archipelago Client events.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        private static void OnError(Exception exception, string message)
        {
            Console.WriteLine("Received an unhandled exception in ArchipelagoClient: {0}\n\n{1}", message, exception);
        }

        /// <summary>
        /// Prints server events to Console.
        /// </summary>
        /// <param name="packet">Print Packet</param>
        private static void OnPrint(PrintPacket packet)
        {
            Console.WriteLine("AP Server: {0}", packet.Text);
        }
    }
}
