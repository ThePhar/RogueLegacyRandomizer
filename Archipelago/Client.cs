//
//  RogueLegacyArchipelago - ArchipelagoClient.cs
//  Last Modified 2021-12-29
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using WebSocketSharp;

namespace Archipelago
{
    public class Client
    {
        public readonly Version APVersion = Version.Parse("0.2.2");

        private DeathLinkService m_deathLink;
        private Dictionary<string, Permissions> m_permissions;
        private string m_seed;
        private ArchipelagoSession m_session;
        private List<string> m_tags;
        private List<int> m_checkedLocations;

        public Client()
        {
            Initialize();
        }

        public Status Status { get; private set; }
        public DateTime LastDeath { get; private set; }
        public DeathLink DeathLink { get; private set; }
        public Dictionary<int, NetworkItem> LocationCache { get; private set; }
        public SlotData Data { get; private set; }
        public Queue<NetworkItem> ItemQueue { get; private set; }
        public ConnectionInfo CachedConnectionInfo { get; private set; }

        public IReadOnlyList<int> CheckedLocations
        {
            get
            {
                return m_checkedLocations;
            }
        }

        public bool CanForfeit
        {
            get
            {
                return m_permissions["forfeit"] == Permissions.Goal || m_permissions["forfeit"] == Permissions.Enabled;
            }
        }

        public void Connect(ConnectionInfo info)
        {
            // Cache our connection info in case we get disconnected later.
            CachedConnectionInfo = info;

            // Disconnect from any session we are currently in if we are attempting to connect.
            if (m_session != null)
            {
                Disconnect();
            }

            Status = Status.Connecting;
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
                    Status = Status.Connected;
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

        public void Disconnect()
        {
            Status = Status.Disconnecting;

            if (m_session != null)
            {
                m_session.Socket.SocketClosed -= OnSocketDisconnect;
                m_session.Socket.ErrorReceived -= OnError;
                m_session.Items.ItemReceived -= OnReceivedItems;
                m_session.Socket.PacketReceived -= OnPacketReceived;
                m_session.Socket.Disconnect();
            }

            if (m_deathLink != null)
            {
                m_deathLink.OnDeathLinkReceived -= OnDeathLink;
            }

            Initialize();
        }

        private void Initialize()
        {
            Status = Status.Disconnected;
            DeathLink = null;
            LocationCache = new Dictionary<int, NetworkItem>();
            Data = null;
            ItemQueue = new Queue<NetworkItem>();
            LastDeath = DateTime.MinValue;

            m_session = null;
            m_deathLink = null;
            m_checkedLocations = new List<int>();
            m_tags = new List<string> { "AP" };
            m_seed = "0";
        }

        public void Forfeit()
        {
            m_session.Socket.SendPacket(new SayPacket
            {
                Text = "!forfeit"
            });
        }

        public void AnnounceVictory()
        {
            m_session.Socket.SendPacket(new StatusUpdatePacket
            {
                Status = ArchipelagoClientState.ClientGoal
            });
        }

        public void ClearDeathLink()
        {
            if (m_deathLink != null)
            {
                DeathLink = null;
            }
        }

        public void SendDeathLink(string cause)
        {
            // Log our current time so we can make sure we ignore our own DeathLink.
            LastDeath = DateTime.Now;

            if (Data.DeathLink && m_deathLink != null)
            {
                var causeWithPlayerName =
                    string.Format("{0}'s {1}.", m_session.Players.GetPlayerAlias(Data.Slot), cause);
                m_deathLink.SendDeathLink(
                    new DeathLink(m_session.Players.GetPlayerAlias(Data.Slot), causeWithPlayerName)
                    {
                        Timestamp = LastDeath
                    });
            }
        }

        public void StartPlaying()
        {
            Status = Status.Playing;
        }

        public void CheckLocations(params int[] locations)
        {
            m_session.Locations.CompleteLocationChecks(locations);
        }

        public string GetPlayerName(int slot)
        {
            var name = m_session.Players.GetPlayerAlias(slot);
            return string.IsNullOrEmpty(name) ? "Archipelago" : name;
        }

        public string GetItemName(int item)
        {
            return m_session.Items.GetItemName(item);
        }

        public bool HasCheckedLocation(int location)
        {
            return m_session.Locations.AllLocationsChecked.Contains(location);
        }

        private void OnSocketDisconnect(CloseEventArgs closeEventArgs)
        {
            switch (Status)
            {
                case Status.Disconnected:
                case Status.Disconnecting:
                    break;

                // Attempt to re-establish a connection.
                case Status.Playing:
                case Status.Connecting:
                case Status.Initialized:
                case Status.FetchingLocations:
                case Status.Connected:
                case Status.Reconnecting:
                    Status = Status.Reconnecting;
                    Connect(CachedConnectionInfo);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnReceivedItems(ReceivedItemsHelper helper)
        {
            ItemQueue.Enqueue(helper.DequeueItem());
        }

        private void OnDeathLink(DeathLink deathLink)
        {
            Console.WriteLine("REC: {0}", deathLink.Timestamp);
            Console.WriteLine("LST: {0}", LastDeath);
            Console.WriteLine(deathLink.Timestamp.ToString(CultureInfo.InvariantCulture) !=
                              LastDeath.ToString(CultureInfo.InvariantCulture));

            // Ignore deaths that died at the same time as us. Should also prevent the player from dying to themselves.
            if (deathLink.Timestamp.ToString(CultureInfo.InvariantCulture) !=
                LastDeath.ToString(CultureInfo.InvariantCulture))
            {
                DeathLink = deathLink;
            }
        }

        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            Console.WriteLine("Received a {0} packet", packet.GetType().Name);
            Console.WriteLine("==============================");
            foreach (var property in packet.GetType().GetProperties())
                Console.WriteLine("{0}: {1}", property.Name, property.GetValue(packet));
            Console.WriteLine();

            if (packet is RoomUpdatePacket)
            {
                OnRoomUpdate((RoomUpdatePacket) packet);
            }
            else if (packet is RoomInfoPacket)
            {
                OnRoomInfo((RoomInfoPacket) packet);
            }
            else if (packet is ConnectedPacket)
            {
                OnConnected((ConnectedPacket) packet);
            }
            else if (packet is PrintPacket)
            {
                OnPrint((PrintPacket) packet);
            }
        }

        private void OnRoomInfo(RoomInfoPacket packet)
        {
            m_seed = packet.SeedName;
            m_permissions = packet.Permissions;

            // Send this so we have a cache of item/location names.
            m_session.Socket.SendPacket(new GetDataPackagePacket());
        }

        private void OnRoomUpdate(RoomUpdatePacket packet)
        {
            if (packet.CheckedLocations != null)
            {
                m_checkedLocations = packet.CheckedLocations;
            }
        }

        private void OnConnected(ConnectedPacket packet)
        {
            Data = new SlotData(packet.SlotData, m_seed, packet.Slot, CachedConnectionInfo.Name);

            // Check if DeathLink is enabled and establish the appropriate helper.
            if (Data.DeathLink)
            {
                m_tags.Add("DeathLink");
                m_session.UpdateTags(m_tags);

                // Clear old DeathLink handlers.
                if (m_deathLink != null)
                {
                    m_deathLink.OnDeathLinkReceived -= OnDeathLink;
                }

                m_deathLink = m_session.CreateDeathLinkServiceAndEnable();
                m_deathLink.OnDeathLinkReceived += OnDeathLink;
            }

            // Build our location cache.
            var locations = LocationDefinitions.GetAllLocations(Data).Select(location => location.Code);
            m_session.Locations.ScoutLocationsAsync(OnReceiveLocationCache, locations.ToArray());
        }

        private void OnReceiveLocationCache(LocationInfoPacket packet)
        {
            foreach (var item in packet.Locations) LocationCache.Add(item.Location, item);
        }

        private static void OnError(Exception exception, string message)
        {
            Console.WriteLine("Received an unhandled exception in ArchipelagoClient: {0}\n\n{1}", message, exception);
        }

        private static void OnPrint(PrintPacket packet)
        {
            Console.WriteLine("AP Server: {0}", packet.Text);
        }
    }
}
