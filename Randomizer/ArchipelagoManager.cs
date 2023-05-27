// RogueLegacyRandomizer - ArchipelagoManager.cs
// Last Modified 2023-05-27 2:19 PM by 
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

namespace Randomizer;

public class ArchipelagoManager
{
    private readonly Version            _minimumArchipelagoServerVersion = new(0, 4, 1);
    private readonly ArchipelagoSession _session;
    private          DeathLinkService   _deathLinkService;
    private          DateTime           _lastDeath;

    private ArchipelagoManager(ConnectionInfo connectionInfo, ArchipelagoSession session)
    {
        ConnectionInfo = connectionInfo;

        _session = session;
        _lastDeath = DateTime.MinValue;
    }

    public ConnectionStatus   Status           { get; private set; } = ConnectionStatus.Disconnected;
    public DeathLink          DeathLink        { get; private set; }
    public RandomizerData     RandomizerData   { get; private set; }
    public bool               DeathLinkSafe    { get; set; }
    public Queue<NetworkItem> ReceiveItemQueue { get; } = new();
    public ConnectionInfo     ConnectionInfo   { get; }

    public bool CanRelease => _session.RoomState.ReleasePermissions is Permissions.Goal or Permissions.Enabled;
    public bool CanCollect => _session.RoomState.CollectPermissions is Permissions.Goal or Permissions.Enabled;
    public int  HintCost   => _session.RoomState.HintCost;
    public int  HintPoints => _session.RoomState.HintPoints;

    public static ArchipelagoManager Connect(ConnectionInfo info)
    {
        var session = ArchipelagoSessionFactory.CreateSession(info.Hostname, info.Port);
        var manager = new ArchipelagoManager(info, session);
        try
        {
            manager.Initialize();
        }
        catch
        {
            manager.Disconnect();
            throw;
        }

        return manager;
    }

    public void Disconnect()
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

    public void Release()
    {
        SendPacket(new SayPacket { Text = "!release" });
    }

    public void Collect()
    {
        SendPacket(new SayPacket { Text = "!collect" });
    }

    public void AnnounceVictory()
    {
        SendPacket(new StatusUpdatePacket { Status = ArchipelagoClientState.ClientGoal });
    }

    public void ClearDeathLink()
    {
        DeathLink = null;
    }

    public void SendDeathLinkIfEnabled(string cause)
    {
        // Do not send any DeathLink messages if it's not enabled.
        if (!_session.ConnectionInfo.Tags.Contains("DeathLink"))
            return;

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

    public void CheckLocations(params long[] locations)
    {
        try
        {
            _session.Locations.CompleteLocationChecks(locations);

            foreach (var location in locations) RandomizerData.CheckedLocations[location] = true;
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    public string GetPlayerName(int slot)
    {
        if (slot == 0)
            return "Archipelago Admin Console";

        var name = _session.Players.GetPlayerAlias(slot);
        return string.IsNullOrEmpty(name) ? $"Unknown Player {slot}" : name;
    }

    public string GetItemName(long item)
    {
        var name = _session.Items.GetItemName(item);
        return string.IsNullOrEmpty(name) ? $"Unknown Item {item}" : name;
    }

    private void Initialize()
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
            _minimumArchipelagoServerVersion,
            uuid: Guid.NewGuid().ToString(),
            password: ConnectionInfo.Password
        );

        if (result.Successful)
            return;

        // Disconnect and throw an exception.
        var failure = (LoginFailure) result;
        throw new ArchipelagoSocketClosedException(failure.Errors[0]);
    }

    private void SendPacket(ArchipelagoPacketBase packet)
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

    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        while (helper.Any())
        {
            var item = helper.DequeueItem();
            if (item.Player == RandomizerData.Slot)
                RandomizerData.CheckedLocations[item.Location] = true;

            ReceiveItemQueue.Enqueue(item);
        }
    }

    private void OnDeathLink(DeathLink deathLink)
    {
        // If we receive a DeathLink that is after our last death, let's set it.
        if (DateTime.Compare(deathLink.Timestamp, _lastDeath) > 0)
            DeathLink = deathLink;
    }

    private void OnPacketReceived(ArchipelagoPacketBase packet)
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

    private async Task OnConnected(ConnectedPacket packet)
    {
        RandomizerData = new RandomizerData(packet.SlotData, _session.RoomState.Seed, packet.Slot);

        _deathLinkService = _session.CreateDeathLinkService();
        _deathLinkService.OnDeathLinkReceived += OnDeathLink;

        // Check if DeathLink is enabled and establish the appropriate helper.
        if (RandomizerData.DeathLink)
            _deathLinkService.EnableDeathLink();

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
            RandomizerData.CheckedLocations[location] = true;

        Status = ConnectionStatus.Authenticated;
    }

    private void OnRoomUpdate(RoomUpdatePacket packet)
    {
        foreach (var location in packet.CheckedLocations)
            RandomizerData.CheckedLocations[location] = true;
    }

    private void OnError(Exception exception, string message)
    {
        Console.WriteLine($"Received an unhandled exception in ArchipelagoClient: {message}\n\n{exception}");
    }
}