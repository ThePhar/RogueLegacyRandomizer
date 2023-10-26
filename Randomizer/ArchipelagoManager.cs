//  RogueLegacyRandomizer - ArchipelagoManager.cs
//  Last Modified 2023-10-26 2:48 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using DS2DEngine;
using RogueLegacy.Enums;

namespace Randomizer;

public class ArchipelagoManager
{
    private static readonly Version _supportedArchipelagoVersion = new(0, 4, 3);

    private readonly ArchipelagoConnectionInfo _connectionInfo;
    private          ArchipelagoSession        _session;
    private          DeathLinkService          _deathLinkService;
    private          DateTime                  _lastDeath;

    public DeathLink                                       DeathLinkData      { get; private set; }
    public bool                                            IsDeathLinkSafe    { get; set; }
    public bool                                            Ready              { get; private set; }
    public Queue<Tuple<int, NetworkItem>>                  ItemQueue          { get; private set; } = new();
    public Dictionary<long, NetworkItem>                   LocationDictionary { get; private set; } = new();
    public List<Tuple<JsonMessageType, JsonMessagePart[]>> ChatLog            { get; }              = new();

    public bool   CanCollect   => _session.RoomState.CollectPermissions is Permissions.Goal or Permissions.Enabled;
    public bool   CanRelease   => _session.RoomState.ReleasePermissions is Permissions.Goal or Permissions.Enabled;
    public bool   CanRemaining => _session.RoomState.RemainingPermissions is Permissions.Goal or Permissions.Enabled;
    public string Seed         => _session.RoomState.Seed;
    public int    Slot         => _session.ConnectionInfo.Slot;
    public bool   DeathLink    => _session.ConnectionInfo.Tags.Contains("DeathLink");
    public int    HintPoints   => _session.RoomState.HintPoints;
    public int    HintCost     => _session.RoomState.HintCost;
    public Hint[] Hints        => _session.DataStorage.GetHints();

    public ArchipelagoManager(ArchipelagoConnectionInfo connectionInfo)
    {
        _connectionInfo = connectionInfo;
    }

    /// <summary>
    /// Attempt to connect to the Archipelago server.
    /// </summary>
    /// <returns>
    /// Returns a LoginFailure object if fails to connect, otherwise returns null.
    /// </returns>
    public async Task<LoginFailure> TryConnect()
    {
        _lastDeath = DateTime.MinValue;
        _session = ArchipelagoSessionFactory.CreateSession(_connectionInfo.Url);

        // (Re-)initialize state.
        DeathLinkData = null;
        IsDeathLinkSafe = false;
        Ready = false;
        ItemQueue = new();
        LocationDictionary = new();

        // Watch for the following events.
        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;
        _session.Items.ItemReceived += OnItemReceived;

        // Attempt to connect to the server.
        try
        {
            await _session.ConnectAsync();
        }
        catch (Exception ex)
        {
            Disconnect();
            return new($"Unable to establish an initial connection to the Archipelago server @ {_connectionInfo.Url}");
        }

        var result = await _session.LoginAsync(
            "Rogue Legacy",
            _connectionInfo.SlotName,
            ItemsHandlingFlags.AllItems,
            _supportedArchipelagoVersion,
            uuid: Guid.NewGuid().ToString(),
            password: _connectionInfo.Password
        );

        if (!result.Successful)
        {
            Disconnect();
            return result as LoginFailure;
        }

        // Load randomizer data.
        RandomizerData.LoadSlotData(((LoginSuccessful) result).SlotData);

        // Initialize DeathLink service.
        _deathLinkService = _session.CreateDeathLinkService();
        _deathLinkService.OnDeathLinkReceived += OnDeathLink;
        if (RandomizerData.DeathLinkMode is DeathLinkMode.Enabled or DeathLinkMode.ForcedEnabled)
        {
            _deathLinkService.EnableDeathLink();
        }

        // Build dictionary of locations with item information for fast lookup.
        await BuildLocationDictionary();

        // Return null to signify no error.
        Ready = true;
        return null;
    }

    /// <summary>
    /// Disconnects from the connected Archipelago server and cleans up event listeners.
    /// </summary>
    public void Disconnect()
    {
        Ready = false;

        // Clear DeathLink events.
        if (_deathLinkService != null)
        {
            _deathLinkService.OnDeathLinkReceived -= OnDeathLink;
            _deathLinkService = null;
        }

        // Clear events and session object.
        if (_session != null)
        {
            _session.Socket.ErrorReceived -= OnError;
            _session.Items.ItemReceived -= OnItemReceived;
            _session.Socket.PacketReceived -= OnPacketReceived;
            _session.Socket.DisconnectAsync(); // It'll disconnect on its own time.
            _session = null;
        }
    }

    /// <summary>
    /// Sends the release chat command.
    /// </summary>
    public void Release()
    {
        SendPacket(new SayPacket { Text = "!release" });
    }

    /// <summary>
    /// Sends the collect chat command.
    /// </summary>
    public void Collect()
    {
        SendPacket(new SayPacket { Text = "!collect" });
    }

    /// <summary>
    /// Sends an update status command.
    /// </summary>
    /// <param name="state">The desired state to change to.</param>
    public void UpdateGameStatus(ArchipelagoClientState state)
    {
        SendPacket(new StatusUpdatePacket { Status = state });
    }

    /// <summary>
    ///  Clear any DeathLink data we have stored.
    /// </summary>
    public void ClearDeathLink()
    {
        DeathLinkData = null;
    }

    /// <summary>
    /// Attempt to send a DeathLink if we have it enabled. Otherwise, do nothing.
    /// </summary>
    /// <param name="cause">Reason everyone hates you.</param>
    public void SendDeathLinkIfEnabled(string cause)
    {
        // Do not send any DeathLink messages if it's not enabled.
        if (!DeathLink)
        {
            return;
        }

        // Log our current time so we can make sure we ignore our own DeathLink.
        _lastDeath = DateTime.Now;
        cause = $"{_session.Players.GetPlayerAlias(Slot)}'s {cause}.";

        try
        {
            _deathLinkService.SendDeathLink(new (_session.Players.GetPlayerAlias(Slot), cause));
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    /// <summary>
    /// Mark the following locations on the server as collected.
    /// </summary>
    /// <param name="locations">An array of location ids.</param>
    public void CheckLocations(params long[] locations)
    {
        try
        {
            _session.Locations.CompleteLocationChecks(locations);
        }
        catch (ArchipelagoSocketClosedException)
        {
            // TODO: Send a message to the client that connection has been dropped.
            Disconnect();
        }
    }

    /// <summary>
    /// Returns the alias of a given player.
    /// </summary>
    /// <param name="slot">Slot number of the player.</param>
    /// <returns>The player alias.</returns>
    public string GetPlayerName(int slot)
    {
        if (slot == 0)
        {
            return "Archipelago";
        }

        var name = _session.Players.GetPlayerAlias(slot);
        return string.IsNullOrEmpty(name) ? $"Unknown Player {slot}" : name;
    }

    /// <summary>
    /// Returns the location name of a given location id.
    /// </summary>
    /// <param name="location">The location id.</param>
    /// <returns>The string representation of the location.</returns>
    public string GetLocationName(long location)
    {
        var name = _session.Locations.GetLocationNameFromId(location);
        return string.IsNullOrEmpty(name) ? $"Unknown Location {location}" : name;
    }

    /// <summary>
    /// Returns the item name of a given item id.
    /// </summary>
    /// <param name="item">The item id.</param>
    /// <returns>The string representation of the item.</returns>
    public string GetItemName(long item)
    {
        var name = _session.Items.GetItemName(item);
        return string.IsNullOrEmpty(name) ? $"Unknown Item {item}" : name;
    }

    /// <summary>
    /// If a NetworkItem is a trap, returns a random trap name. Otherwise, returns the normal item name.
    /// </summary>
    /// <param name="item">The network item to get name from.</param>
    /// <returns>A string representation of the item (or a trap).</returns>
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public string GetItemOrTrapName(NetworkItem item)
    {
        if (!item.Flags.HasFlag(ItemFlags.Trap))
        {
            return GetItemName(item.Item);
        }

        // I'm hilarious, obviously.
        var trapItemNames = new[]
        {
            "Blacksmiht",
            "Blaccsmith",
            "Enchantres",
            "Arcitech",
            "Helth Up",
            "Manna Up",
            "Attak Up",
            "Magic Damag Up",
            "Random Children",
            "Grase Runes",
            "Draguns",
            "Trators",
            "Cursed Rune",
            "Vawlt Runes",
            "Sprint Runs",
            "Jump Runes",
            "Ski Runes",
            "Gold Runes",
            "Phar's Love and Adoration", // You know you shouldn't trust something named this, don't kid yourself.
        };

        return trapItemNames[CDGMath.RandomInt(0, trapItemNames.Length - 1)];
    }

    /// <summary>
    /// Enable DeathLink.
    /// </summary>
    public void EnableDeathLink()
    {
        _deathLinkService.EnableDeathLink();
    }

    /// <summary>
    /// Disable DeathLink.
    /// </summary>
    public void DisableDeathLink()
    {
        _deathLinkService.DisableDeathLink();
    }

    /// <summary>
    /// Checks if a given location was checked.
    /// </summary>
    /// <param name="id">The id of the location.</param>
    /// <returns>If location was checked.</returns>
    public bool IsLocationChecked(long id)
    {
        // Verify location exists first, we'll treat locations that don't exist as already checked.
        return !_session.Locations.AllLocations.Contains(id) || _session.Locations.AllLocationsChecked.Contains(id);
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
        var i = helper.Index;
        while (helper.Any())
        {
            ItemQueue.Enqueue(new (i++, helper.DequeueItem()));
        }
    }

    private void OnDeathLink(DeathLink deathLink)
    {
        // If we receive a DeathLink that is after our last death, let's set it.
        if (!IsDeathLinkSafe && DateTime.Compare(deathLink.Timestamp, _lastDeath) > 0)
        {
            DeathLinkData = deathLink;
        }
    }

    private async Task BuildLocationDictionary()
    {
        var locations = await _session.Locations.ScoutLocationsAsync(false, _session.Locations.AllLocations.ToArray());

        foreach (var item in locations.Locations)
        {
            LocationDictionary[item.Location] = item;
        }
    }

    private void OnPacketReceived(ArchipelagoPacketBase packet)
    {
        // Special handling for PrintJSON messages only, otherwise we log complete details about packet below.
        if (packet is PrintJsonPacket messagePacket)
        {
            // Treat unknown message types as "Chat" for forwards compat so we don't crash on new ones.
            ChatLog.Add(new(messagePacket.MessageType ?? JsonMessageType.Chat, messagePacket.Data));
            var message = messagePacket
                .Data
                .Aggregate(new StringBuilder(), (sb, data) => sb.Append(data.Text))
                .ToString();

            RandUtil.Console("Archipelago", $"Message Received: {message}");
            return;
        }

        RandUtil.Console("Archipelago", $"Packet Received: {packet.GetType().Name}");
        RandUtil.PrintProperties(packet);
    }

    private static void OnError(Exception exception, string message)
    {
        RandUtil.Console(
            "Archipelago",
            $"Encountered an unhandled exception in ArchipelagoManager: " +
                    $"{message}\n\nStack Trace:\n{exception.StackTrace}"
        );
    }
}
