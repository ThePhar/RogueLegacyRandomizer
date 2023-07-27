// RogueLegacyRandomizer - ArchipelagoManager.cs
// Last Modified 2023-07-27 1:26 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using RogueLegacy.Enums;

namespace Randomizer;

public static class ArchipelagoManager
{
    private static readonly Version            MinimumArchipelagoServerVersion = new(0, 4, 1);
    private static          ArchipelagoSession _session;
    private static          DeathLinkService   _deathLinkService;
    private static          DateTime           _lastDeath = DateTime.MinValue;

    public static ConnectionStatus   Status           { get; private set; } = ConnectionStatus.Disconnected;
    public static DeathLink          DeathLink        { get; private set; }
    public static RandomizerData     RandomizerData   { get; private set; }
    public static bool               DeathLinkSafe    { get; set; }
    public static Queue<NetworkItem> ReceiveItemQueue { get; } = new();
    public static ConnectionInfo     ConnectionInfo   { get; private set; }

    public static bool CanRelease => _session.RoomState.ReleasePermissions is Permissions.Goal or Permissions.Enabled;
    public static bool CanCollect => _session.RoomState.CollectPermissions is Permissions.Goal or Permissions.Enabled;

    public static void Connect(ConnectionInfo info)
    {
        _session = ArchipelagoSessionFactory.CreateSession(info.Hostname);
        ConnectionInfo = info;
        try
        {
            Initialize();
        }
        catch
        {
            Disconnect();
            throw;
        }
    }

    private static void Disconnect()
    {
        Status = ConnectionStatus.Disconnected;

        // Clear DeathLink handlers.
        if (_deathLinkService != null) _deathLinkService.OnDeathLinkReceived -= OnDeathLink;

        // Clear Session handlers.
        if (_session == null) return;

        _session.Socket.ErrorReceived -= OnError;
        _session.Items.ItemReceived -= OnItemReceived;
        _session.Socket.PacketReceived -= OnPacketReceived;
        _session.Socket.DisconnectAsync();
    }

    public static void Release()
    {
        SendPacket(new SayPacket { Text = "!release" });
    }

    public static void Collect()
    {
        SendPacket(new SayPacket { Text = "!collect" });
    }

    public static void AnnounceVictory()
    {
        SendPacket(new StatusUpdatePacket { Status = ArchipelagoClientState.ClientGoal });
    }

    public static void ClearDeathLink()
    {
        DeathLink = null;
    }

    public static void SendDeathLinkIfEnabled(string cause)
    {
        // Do not send any DeathLink messages if it's not enabled.
        if (!_session.ConnectionInfo.Tags.Contains("DeathLink"))
        {
            return;
        }

        // Log our current time so we can make sure we ignore our own DeathLink.
        _lastDeath = DateTime.Now;
        cause = $"{_session.Players.GetPlayerAlias(RandomizerData.Slot)}'s {cause}.";

        try
        {
            _deathLinkService.SendDeathLink(
                new DeathLink(_session.Players.GetPlayerAliasAndName(RandomizerData.Slot), cause)
            );
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    public static void CheckLocations(params long[] locations)
    {
        try
        {
            _session.Locations.CompleteLocationChecks(locations);

            foreach (var location in locations)
            {
                RandomizerData.CheckedLocations[location] = true;
            }
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    public static string GetPlayerName(int slot)
    {
        if (slot == 0)
        {
            return "Archipelago";
        }

        var name = _session.Players.GetPlayerAlias(slot);
        return string.IsNullOrEmpty(name) ? $"Unknown Player {slot}" : name;
    }

    public static string GetItemName(long item)
    {
        var name = _session.Items.GetItemName(item);
        return string.IsNullOrEmpty(name) ? $"Unknown Item {item}" : name;
    }

    private static void Initialize()
    {
        // Watch for the following events.
        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;
        _session.Items.ItemReceived += OnItemReceived;

        Status = ConnectionStatus.Connecting;

        // Attempt to connect to the server.
        var result = _session.TryConnectAndLogin(
            "Rogue Legacy",
            ConnectionInfo.Name,
            ItemsHandlingFlags.AllItems,
            MinimumArchipelagoServerVersion,
            uuid: Guid.NewGuid().ToString(),
            password: ConnectionInfo.Password
        );

        if (result.Successful)
        {
            return;
        }

        // Disconnect and throw an exception.
        var failure = (LoginFailure) result;
        throw new ArchipelagoSocketClosedException(failure.Errors[0]);
    }

    private static void SendPacket(ArchipelagoPacketBase packet)
    {
        try
        {
            _session.Socket.SendPacket(packet);
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    private static void OnItemReceived(ReceivedItemsHelper helper)
    {
        while (helper.Any())
        {
            var item = helper.DequeueItem();
            if (item.Player == RandomizerData.Slot)
            {
                RandomizerData.CheckedLocations[item.Location] = true;
            }

            ReceiveItemQueue.Enqueue(item);
        }
    }

    private static void OnDeathLink(DeathLink deathLink)
    {
        // If we receive a DeathLink that is after our last death, let's set it.
        if (DateTime.Compare(deathLink.Timestamp, _lastDeath) > 0)
        {
            DeathLink = deathLink;
        }
    }

    private static void OnPacketReceived(ArchipelagoPacketBase packet)
    {
        Console.WriteLine($"Packet {packet.GetType().Name} received!");
        Console.WriteLine("========================================");
        foreach (var property in packet.GetType().GetProperties())
            Console.WriteLine($"{property.Name}: {property.GetValue(packet, null)}");

        switch (packet)
        {
            case ConnectedPacket connectedPacket:
                OnConnected(connectedPacket);
                break;

            case RoomUpdatePacket roomUpdatePacket:
                OnRoomUpdate(roomUpdatePacket);
                break;
        }
    }

    private static async Task OnConnected(ConnectedPacket packet)
    {
        RandomizerData = new RandomizerData(packet.SlotData, _session.RoomState.Seed, packet.Slot);

        _deathLinkService = _session.CreateDeathLinkService();
        _deathLinkService.OnDeathLinkReceived += OnDeathLink;

        // Check if DeathLink is enabled and establish the appropriate helper.
        if (RandomizerData.DeathLinkMode is DeathLinkMode.Enabled or DeathLinkMode.ForcedEnabled)
        {
            _deathLinkService.EnableDeathLink();
        }

        var locations = await _session.Locations.ScoutLocationsAsync(false, packet.LocationsChecked.Concat(packet.MissingChecks).ToArray());
        foreach (var location in locations.Locations)
        {
            var item = new NetworkItem
            {
                Item = location.Item,
                Location = location.Location,
                Player = location.Player,
                Flags = location.Flags
            };

            RandomizerData.ActiveLocations.Add(item.Location, item);
            RandomizerData.CheckedLocations.Add(item.Location, false);
        }

        foreach (var location in packet.LocationsChecked)
        {
            RandomizerData.CheckedLocations[location] = true;
        }

        Status = ConnectionStatus.Authenticated;
    }

    private static void OnRoomUpdate(RoomUpdatePacket packet)
    {
        if (packet.CheckedLocations is null)
        {
            return;
        }

        foreach (var location in packet.CheckedLocations)
        {
            RandomizerData.CheckedLocations[location] = true;
        }
    }

    private static void OnError(Exception exception, string message)
    {
        Console.WriteLine($"Received an unhandled exception in ArchipelagoClient: {message}\n\n{exception}");
    }
}