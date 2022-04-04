//
//  Rogue Legacy Randomizer - Client.cs
//  Last Modified 2022-04-03
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
using System.Text;
using Archipelago.Definitions;
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
        public const int    MAXIMUM_RECONNECTION_ATTEMPTS = 3;
        public const string MINIMUM_AP_VERSION            = "0.3.1";

        private bool                            _allowReconnect;
        private DeathLinkService                _deathLinkService;
        private Dictionary<string, Permissions> _permissions = new();
        private int                             _reconnectionAttempt;
        private string                          _seed = "0";
        private ArchipelagoSession              _session;
        private List<string>                    _tags = new() { "AP" };

        public Client()
        {
            Initialize();
        }

        public ConnectionStatus              ConnectionStatus        { get; private set; } = ConnectionStatus.Disconnected;
        public DateTime                      LastDeath               { get; private set; } = DateTime.MinValue;
        public ConnectionInfo                CachedConnectionInfo    { get; private set; } = new();
        public DeathLink                     DeathLink               { get; private set; }
        public Dictionary<long, NetworkItem> LocationCache           { get; private set; } = new();
        public SlotData                      Data                    { get; private set; }
        public Queue<NetworkItem>            ItemQueue               { get; private set; } = new();
        public List<long>                    CheckedLocations        { get; private set; } = new();
        public List<string>                  ChatHistory             { get; private set; } = new();
        public bool                          CheckedLocationsUpdated { get; set; }
        public bool                          CanForfeit              => _permissions["forfeit"] is Permissions.Goal or Permissions.Enabled;
        public bool                          CanCollect              => _permissions["collect"] is Permissions.Goal or Permissions.Enabled;

        public void Connect(ConnectionInfo info)
        {
            // Cache our connection info in case we get disconnected later.
            CachedConnectionInfo = info;

            // Disconnect from any session we are currently in if we are attempting to connect.
            if (_session != null)
            {
                Disconnect();
            }

            ConnectionStatus = ConnectionStatus.Connecting;
            try
            {
                _session = ArchipelagoSessionFactory.CreateSession(info.Hostname, info.Port);

                // Establish event handlers.
                _session.Socket.SocketClosed += OnSocketDisconnect;
                _session.Socket.ErrorReceived += OnError;
                _session.Items.ItemReceived += OnReceivedItems;
                _session.Socket.PacketReceived += OnPacketReceived;

                // Attempt to connect to the AP server.
                var result = _session.TryConnectAndLogin(
                    "Rogue Legacy",
                    info.Name,
                    new Version(MINIMUM_AP_VERSION),
                    ItemsHandlingFlags.AllItems,
                    _tags.ToArray(),
                    password: info.Password
                );

                if (result.Successful)
                {
                    _reconnectionAttempt = 0;
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
            ConnectionStatus = ConnectionStatus.Disconnecting;

            // Clear DeathLink handlers.
            if (_deathLinkService != null)
            {
                _deathLinkService.OnDeathLinkReceived -= OnDeathLink;
            }

            // Clear session handlers.
            if (_session != null)
            {
                _session.Socket.SocketClosed -= OnSocketDisconnect;
                _session.Socket.ErrorReceived -= OnError;
                _session.Items.ItemReceived -= OnReceivedItems;
                _session.Socket.PacketReceived -= OnPacketReceived;
                _session.Socket.Disconnect();
            }

            // Reset all values back to their defaults.
            Initialize();
        }

        private void Initialize()
        {
            _session = null;
            _deathLinkService = null;
            _permissions = new Dictionary<string, Permissions>();
            _tags = new List<string> { "AP" };
            _allowReconnect = false;
            _reconnectionAttempt = 0;
            _seed = "0";

            ConnectionStatus = ConnectionStatus.Disconnected;
            CheckedLocations = new List<long>();
            LastDeath = DateTime.MinValue;
            DeathLink = null;
            LocationCache = new Dictionary<long, NetworkItem>();
            Data = null;
            ItemQueue = new Queue<NetworkItem>();
        }

        public void Forfeit()
        {
            // Not sure if there's a better way to do this, but I know this works!
            _session.Socket.SendPacket(new SayPacket { Text = "!forfeit" });
        }

        public void Collect()
        {
            // Not sure if there's a better way to do this, but I know this works!
            _session.Socket.SendPacket(new SayPacket { Text = "!collect" });
        }

        public void AnnounceVictory()
        {
            _session.Socket.SendPacket(new StatusUpdatePacket { Status = ArchipelagoClientState.ClientGoal });
        }

        public void ClearDeathLink()
        {
            if (_deathLinkService != null)
            {
                DeathLink = null;
            }
        }

        public void SendDeathLink(string cause)
        {
            // Log our current time so we can make sure we ignore our own DeathLink.
            LastDeath = DateTime.Now;

            if (!Data.DeathLink || _deathLinkService == null)
            {
                return;
            }

            var causeWithPlayerName = $"{_session.Players.GetPlayerAlias(Data.Slot)}'s {cause}.";
            _deathLinkService.SendDeathLink(
                new DeathLink(_session.Players.GetPlayerAlias(Data.Slot), causeWithPlayerName)
                    { Timestamp = LastDeath });
        }

        public void CheckLocations(params long[] locations)
        {
            _session.Locations.CompleteLocationChecks(locations);
        }

        public string GetPlayerName(int slot)
        {
            var name = _session.Players.GetPlayerAlias(slot);
            return string.IsNullOrEmpty(name) ? "Archipelago" : name;
        }

        public string GetItemName(int item)
        {
            var name = _session.Items.GetItemName(item);
            return string.IsNullOrEmpty(name) ? "Unknown Item" : name;
        }

        public string GetLocationName(int location)
        {
            var name = _session.Locations.GetLocationNameFromId(location);
            return string.IsNullOrEmpty(name) ? "Unknown Location" : name;
        }

        private void OnSocketDisconnect(CloseEventArgs closeEventArgs)
        {
            // Check to see if we are still in a game, and attempt to reconnect if possible.
            switch (ConnectionStatus)
            {
                // We were failing to connect.
                case ConnectionStatus.Connecting:
                    if (!_allowReconnect)
                    {
                        throw new ArchipelagoSocketClosedException("Unable to establish connection to AP server.");
                    }

                    if (_reconnectionAttempt >= MAXIMUM_RECONNECTION_ATTEMPTS)
                    {
                        throw new ArchipelagoSocketClosedException(
                            "Lost connection to AP server and failed to reconnect. Please save and quit to title " +
                            "screen and attempt to reconnect as client is no longer syncing."
                        );
                    }

                    _reconnectionAttempt += 1;
                    ConnectionStatus = ConnectionStatus.Connecting;
                    Connect(CachedConnectionInfo);
                    break;

                // We're in a current game and lost connection, so attempt to reconnect gracefully.
                case ConnectionStatus.Connected:
                    _allowReconnect = true;

                    // Ignore this is a goto. Thanks for playing.
                    goto case ConnectionStatus.Connecting;
            }
        }

        private void OnReceivedItems(ReceivedItemsHelper helper)
        {
            ItemQueue.Enqueue(helper.DequeueItem());
        }

        private void OnDeathLink(DeathLink deathLink)
        {
            var newDeathLink = deathLink.Timestamp.ToString(CultureInfo.InvariantCulture);
            var oldDeathLink = LastDeath.ToString(CultureInfo.InvariantCulture);

            // Ignore deaths that died at the same time as us. Should also prevent the player from dying to themselves.
            if (newDeathLink != oldDeathLink)
            {
                DeathLink = deathLink;
            }
        }

        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            Console.WriteLine($"Received a {packet.GetType().Name} packet");
            Console.WriteLine("==========================================");
            foreach (var property in packet.GetType().GetProperties())
                Console.WriteLine($"{property.Name}: {property.GetValue(packet, null)}");

            Console.WriteLine();

            switch (packet)
            {
                case RoomUpdatePacket roomUpdatePacket:
                    OnRoomUpdate(roomUpdatePacket);
                    break;

                case RoomInfoPacket roomInfoPacket:
                    OnRoomInfo(roomInfoPacket);
                    break;

                case ConnectedPacket connectedPacket:
                    OnConnected(connectedPacket);
                    break;

                case PrintPacket printPacket:
                    OnPrint(printPacket);
                    break;

                case PrintJsonPacket printJsonPacket:
                    OnJsonPrint(printJsonPacket);
                    break;
            }
        }

        private void OnRoomInfo(RoomInfoPacket packet)
        {
            _seed = packet.SeedName;
            _permissions = packet.Permissions;

            // Send this so we have a cache of item/location names.
            _session.Socket.SendPacket(new GetDataPackagePacket());
        }

        private void OnRoomUpdate(RoomUpdatePacket packet)
        {
            foreach (var location in packet.CheckedLocations)
                if (!CheckedLocations.Contains(location))
                {
                    CheckedLocations.Add(location);
                    CheckedLocationsUpdated = true;
                }
        }

        private void OnConnected(ConnectedPacket packet)
        {
            Data = new SlotData(packet.SlotData, _seed, packet.Slot, CachedConnectionInfo.Name);

            // Check if DeathLink is enabled and establish the appropriate helper.
            if (Data.DeathLink)
            {
                _tags.Add("DeathLink");
                _session.UpdateConnectionOptions(_tags.ToArray(), ItemsHandlingFlags.AllItems);

                // Clear old DeathLink handlers.
                if (_deathLinkService != null)
                {
                    _deathLinkService.OnDeathLinkReceived -= OnDeathLink;
                }

                _deathLinkService = _session.CreateDeathLinkServiceAndEnable();
                _deathLinkService.OnDeathLinkReceived += OnDeathLink;
            }

            // Mark our checked locations.
            CheckedLocations = packet.LocationsChecked.ToList();

            // Build our location cache.
            var locations = LocationDefinitions.GetAllLocations(Data).Select(location => (long) location.Code).ToArray();
            _session.Locations.ScoutLocationsAsync(OnReceiveLocationCache, locations.ToArray());

            // Set ourselves to connected.
            ConnectionStatus = ConnectionStatus.Connected;
        }

        private void OnReceiveLocationCache(LocationInfoPacket packet)
        {
            foreach (var item in packet.Locations) LocationCache.Add(item.Location, item);
        }

        private static void OnError(Exception exception, string message)
        {
            Console.WriteLine("Received an unhandled exception in ArchipelagoClient: {0}\n\n{1}", message, exception);
        }

        private void OnPrint(PrintPacket packet)
        {
            Console.WriteLine("AP Server: {0}", packet.Text);
            ChatHistory.Add(packet.Text);
        }

        private void OnJsonPrint(PrintJsonPacket packet)
        {
            var text = new StringBuilder();

            // TODO: Add color support.
            foreach (var element in packet.Data)
            {
                string substring = "";
                switch (element.Type)
                {
                    case JsonMessagePartType.PlayerId:
                        substring = GetPlayerName(int.Parse(element.Text));
                        break;

                    case JsonMessagePartType.ItemId:
                        substring = GetItemName(int.Parse(element.Text));
                        break;

                    case JsonMessagePartType.LocationId:
                        substring = GetLocationName(int.Parse(element.Text));
                        break;

                    default:
                        substring = element.Text;
                        break;
                }

                text.Append(substring);
            }

            ChatHistory.Add(text.ToString());
        }
    }
}
