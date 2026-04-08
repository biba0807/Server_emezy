using System;
using System.IO;
using System.Net.Sockets;
using Google.Protobuf;
using Axlebolt.RpcSupport.Protobuf;

namespace StandoffServer.Networking
{
    public class NetworkPacket
    {
        public static void WritePacket(NetworkStream stream, IMessage message)
        {
            byte[] body = message.ToByteArray();
            // Шлем длину тела сообщения (4 байта)
            byte[] header = BitConverter.GetBytes(body.Length);
            
            // Unity Bolt ВСЕГДА ожидает Big Endian (сетевой порядок байт) для заголовка длины.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(header);
            }

            // Use a single buffer to ensure the header and body are sent together in one TCP segment if possible.
            // This prevents the client from reading a partial header and failing.
            byte[] packet = new byte[header.Length + body.Length];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            Buffer.BlockCopy(body, 0, packet, header.Length, body.Length);

            lock (stream)
            {
                stream.Write(packet, 0, packet.Length);
                stream.Flush();
            }
            
            if (message is ResponseMessage rm && rm.RpcResponse != null)
            {
                Console.WriteLine($"[Network] Sent packet {rm.GetType().Name}, body size: {body.Length}, Rpc ID: {rm.RpcResponse.Id}");
            }
            else
            {
                Console.WriteLine($"[Network] Sent packet: {message.GetType().Name}, body size: {body.Length}");
            }
        }

        public static void WritePacket(NetworkStream stream, byte[] body)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            byte[] header = BitConverter.GetBytes(body.Length);

            // Unity Bolt ВСЕГДА ожидает Big Endian (сетевой порядок байт) для заголовка длины.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(header);
            }

            lock (stream)
            {
                stream.Write(header, 0, header.Length);
                stream.Write(body, 0, body.Length);
                stream.Flush();
            }

            // Логируем для отладки
            Console.WriteLine($"[Network] Sent packet: raw, body size: {body.Length}");
        }

        public static byte[] ReadPacket(NetworkStream stream)
        {
            try 
            {
                byte[] header = new byte[4];
                int read = 0;
                while (read < 4)
                {
                    int r = stream.Read(header, read, 4 - read);
                    if (r <= 0) return null;
                    read += r;
                }

                // Unity Bolt присылает длину в Big Endian.
                // Если мы на Little Endian (Windows), переворачиваем, чтобы BitConverter прочитал правильно.
                byte[] lengthBytes = (byte[])header.Clone();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);

                int length = BitConverter.ToInt32(lengthBytes, 0);
                
                // Если заголовок подозрительно похож на 1-байтовый пинг (00-00-00-01)
                // или если это вообще невалидная длина.
                if (length == 1)
                {
                    // Читаем этот 1 байт и возвращаем как спец-пакет
                    byte[] pingBody = new byte[1];
                    stream.Read(pingBody, 0, 1);
                    return pingBody;
                }

                if (length <= 0 || length > 10 * 1024 * 1024) 
                {
                    Console.WriteLine($"[Network] Invalid packet length: {length} (raw header: {BitConverter.ToString(header)})");
                    return null;
                }

                byte[] body = new byte[length];
                read = 0;
                while (read < length)
                {
                    int r = stream.Read(body, read, length - read);
                    if (r <= 0) return null;
                    read += r;
                }

                return body;
            }
            catch (System.Exception ex)
            {
                if (!(ex is IOException || ex is SocketException))
                {
                    Console.WriteLine($"[Network] Read error: {ex.Message}");
                }
                return null;
            }
        }
    }
}
