using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using StandoffServer.Services;
using Axlebolt.RpcSupport.Protobuf;
using Google.Protobuf;

namespace StandoffServer.Networking
{
    public class TcpServer
    {
        private readonly TcpListener _listener;
        private readonly MongoService _mongo;
        private readonly RpcHandler _rpcHandler;
        private bool _isRunning;

        public TcpServer(int port, MongoService mongo)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            try
            {
                _listener.Server.ExclusiveAddressUse = false;
                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            catch
            {
                // ignore socket option errors
            }
            _mongo = mongo;
            _rpcHandler = new RpcHandler(mongo);
        }

        public async Task StartAsync(CancellationToken ct)
        {
            try
            {
                _listener.Start();
            }
            catch (SocketException ex)
            {
                _isRunning = false;
                Console.WriteLine($"[Server] Failed to start listener: {ex.SocketErrorCode} ({ex.ErrorCode}) {ex.Message}");
                return;
            }

            _isRunning = true;
            Console.WriteLine($"[Server] Started on :{((IPEndPoint)_listener.LocalEndpoint).Port}");

            while (!ct.IsCancellationRequested && _isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client, ct);
                }
                catch (System.Exception ex)
                {
                    if (_isRunning) Console.WriteLine($"[Server] Accept error: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
        {
            client.NoDelay = true;
            string currentTicket = null;
            using (client)
            using (var stream = client.GetStream())
            {
                Console.WriteLine($"[Server] New connection: {client.Client.RemoteEndPoint}");
                
                try
                {
                    while (client.Connected && !ct.IsCancellationRequested)
                    {
                        byte[] data = NetworkPacket.ReadPacket(stream);
                        if (data == null) 
                        {
                            Console.WriteLine($"[Server] Connection closed by peer: {client.Client.RemoteEndPoint}");
                            break;
                        }

                        if (data.Length == 1)
                        {
                            // Bolt ping — send back empty packet (4 zero bytes = size 0) so client's pong signal resets
                            try
                            {
                                lock (stream)
                                {
                                    stream.Write(new byte[4] { 0, 0, 0, 0 }, 0, 4);
                                    stream.Flush();
                                }
                            }
                            catch { }
                            continue;
                        }

                        RpcRequest request = RpcRequest.Parser.ParseFrom(data);
                        
                        // Process all requests synchronously to avoid race conditions
                        try
                        {
                            bool isHandshake = request.ServiceName == "HandshakeRemoteService" && 
                                              (request.MethodName == "protoHandshake" || request.MethodName == "handshake");
                            
                            // Extract ticket from handshake BEFORE processing so subsequent requests use it
                            if (isHandshake && request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                            {
                                try
                                {
                                    var hs = Axlebolt.Bolt.Protobuf.Handshake.Parser.ParseFrom(request.Params[0].One);
                                    if (!string.IsNullOrWhiteSpace(hs?.Ticket))
                                    {
                                        // Parse "ticket@version" — use only the ticket part for session
                                        var rawTicket = hs.Ticket;
                                        currentTicket = rawTicket.Contains("@") ? rawTicket.Split('@')[0] : rawTicket;
                                        _rpcHandler.RegisterClient(currentTicket, stream);
                                        Console.WriteLine($"[Server] Pre-bound ticket from handshake: {currentTicket}");
                                    }
                                }
                                catch (System.Exception ex) { Console.WriteLine($"[Server] Pre-handshake ticket error: {ex.Message}"); }
                            }

                            string taskTicket = currentTicket;
                            var response = await _rpcHandler.HandleRequestAsync(request, taskTicket);

                            if (isHandshake)
                            {
                                Console.WriteLine($"[Server] Handshake response sent for ticket: {currentTicket}");
                            }

                            bool isAuthCall =
                                (request.ServiceName == "TestAuthRemoteService" && (request.MethodName == "auth" || request.MethodName == "protoAuth")) ||
                                (request.ServiceName != null && request.ServiceName.EndsWith("AuthRemoteService", StringComparison.Ordinal) &&
                                 (request.MethodName == "auth" || request.MethodName == "protoAuth" || request.MethodName == "protoAuthSecured"));

                            if (isAuthCall)
                            {
                                if (response?.Return?.One != null && !response.Return.One.IsEmpty)
                                {
                                    try
                                    {
                                        var returnedTicket = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(response.Return.One).Value;
                                        if (!string.IsNullOrWhiteSpace(returnedTicket))
                                        {
                                            currentTicket = returnedTicket;
                                            _rpcHandler.RegisterClient(returnedTicket, stream);
                                            Console.WriteLine($"[Server] Auth bound ticket: {returnedTicket}");
                                        }
                                    }
                                    catch (System.Exception ex) { Console.WriteLine($"[Server] Auth ticket error: {ex.Message}"); }
                                }
                            }

                            var responseMessage = new ResponseMessage { RpcResponse = response };
                            NetworkPacket.WritePacket(stream, responseMessage);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Server] Request error: {ex.Message}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"[Server] Client loop error: {ex.Message}");
                }
                finally
                {
                    if (currentTicket != null)
                    {
                        _rpcHandler.UnregisterClient(currentTicket);
                    }
                    Console.WriteLine($"[Server] Connection closed for: {client.Client?.RemoteEndPoint ?? (object)"unknown"}");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }
    }
}
