// 
// RogueLegacyArchipelago - DataPacketConverter.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Archipelago.Packets
{
    public class DataPacketConverter : Newtonsoft.Json.Converters.CustomCreationConverter<IDataPacket>
    {
        public override IDataPacket Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IDataPacket Create(Type objectType, JObject jObject)
        {
            var type = (string) jObject.Property("cmd");

            switch (type)
            {
                case DataPacketTypes.RoomInfo:
                    return new RoomInfoPacket();

                case DataPacketTypes.ConnectionRefused:
                    return new ConnectionRefusedPacket();

                case DataPacketTypes.Connected:
                    return new ConnectedPacket();

                case DataPacketTypes.ReceivedItems:
                    return new ReceivedItemsPacket();

                case DataPacketTypes.LocationInfo:
                    return new LocationInfoPacket();

                case DataPacketTypes.RoomUpdate:
                    return new RoomUpdatePacket();

                case DataPacketTypes.Print:
                    return new PrintPacket();

                case DataPacketTypes.PrintJson:
                    return new PrintJsonPacket();

                case DataPacketTypes.DataPackage:
                    return new DataPackagePacket();

                case DataPacketTypes.Bounced:
                    return new BouncedPacket();

                case DataPacketTypes.InvalidPacket:
                    return new InvalidPacket();

                default:
                    return new GenericPacket();

                // Just ignore unknown packets. /shrug
                // default:
                //     throw new ApplicationException(String.Format("The data packet of type {0} is not supported!",
                //         type));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var jObject = new JObject();

            Console.WriteLine("FIRED 2!");

            foreach (var prop in type.GetProperties())
            {
                // if (!prop.CanRead) continue;

                var propValue = prop.GetValue(value, null);
                if (propValue != null)
                {
                    Console.WriteLine("FIRED!");
                    jObject.Add(prop.Name.ToLower(), JToken.FromObject(propValue, serializer));
                }
            }

            jObject.WriteTo(writer);
        }
    }
}
