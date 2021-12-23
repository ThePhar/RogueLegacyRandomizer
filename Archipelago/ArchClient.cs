// 
// RogueLegacyArchipelago - ArchClient.cs
// Last Modified 2021-12-23
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.Network;
using Archipelago.Packets;
using Newtonsoft.Json;

namespace Archipelago
{
    internal class ArchClient
    {
        private readonly ClientWebSocket m_socket = new ClientWebSocket();
        private readonly int m_bufferSize = 32;

        /// <summary>
        /// Attempts to connect to a given Archipelago server. Once connected, will attempt to initiate the handshake
        /// as well.
        /// </summary>
        /// <param name="address">Hostname or IP address to connect to.</param>
        /// <param name="port">Port number to connect to.</param>
        /// <param name="slotName">The slot name for this player.</param>
        /// <param name="password">The server password, if required. Leave blank if not necessary.</param>
        public async Task Connect(string address, string port, string slotName, string password = "")
        {
            // If we are currently connected, let's disconnect.
            // TODO: Write disconnect logic.

            var uri = "ws://" + address + ":" + port;
            Console.WriteLine("ARCH-CLIENT: Connecting to {0}...", uri);
            await m_socket.ConnectAsync(new Uri(uri), CancellationToken.None);

            if (IsConnected)
            {
                Console.WriteLine("ARCH-CLIENT: Connected to {0}.", uri);
                await Receive();
                await Send(new List<IDataPacket>
                {
                    new ConnectPacket()
                    {
                        Game = "Rogue Legacy",
                        Name = "Phar",
                        Tags = new List<string>{ "AP" },
                        Uuid = "1",
                        Version = new NetworkVersion()
                        {
                            Build = 1,
                            Major = 0,
                            Minor = 2,
                        }
                    }
                });
                await Receive();
            }
            else
            {
                Console.WriteLine("ARCH-CLIENT: Failed to connect to {0}.", uri);
            }

            Console.WriteLine("ARCH-CLIENT: Finished connecting.");
        }

        /// <summary>
        /// Receive the next packet from the AP server and parse it as an IDataPacket List.
        /// </summary>
        /// <returns>List of IDataPackets</returns>
        /// <exception cref="WebSocketException">Attempting to read from socket, when not connected.</exception>
        private async Task<List<IDataPacket>> Receive()
        {
            if (!IsConnected)
                throw new WebSocketException("Attempted to read from non-connected Arch WebSocket.");

            var buffer = new byte[m_bufferSize];
            using (MemoryStream ms = new MemoryStream())
            {
                WebSocketReceiveResult result = null;

                Console.WriteLine("ARCH-CLIENT: Waiting for message...");
                do
                {
                    try
                    {

                        result = await m_socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    if (result.Count > 0)
                    {
                        ms.Write(buffer, 0, result.Count);
                    }
                    else break;

                } while (!result.EndOfMessage);

                buffer = ms.ToArray();
            }

            var message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.WriteLine("ARCH-CLIENT: Received: {0}", message);

            try
            {
                return JsonConvert.DeserializeObject<List<IDataPacket>>(message, new DataPacketConverter());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new List<IDataPacket>();
        }

        private async Task Send(List<IDataPacket> packets)
        {
            var settings = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(packets, settings);

            Console.WriteLine("ARCH-CLIENT: Sending: {0}", json);

            var bytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            await m_socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);

            Console.WriteLine("ARCH-CLIENT: Sent!");
        }

        public bool IsConnected
        {
            get { return m_socket.State == WebSocketState.Open; }
        }

        public bool IsConnecting
        {
            get { return m_socket.State == WebSocketState.Connecting; }
        }

        public bool IsDisconnected
        {
            get { return m_socket.State == WebSocketState.Closed; }
        }
    }
}
