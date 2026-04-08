using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StandoffServer.Networking;
using StandoffServer.Services;

namespace StandoffServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Получаем URI из переменной окружения, если есть
            var mongoUri = Environment.GetEnvironmentVariable("STANDOFF_MONGO_URI");

            // Если переменная не задана или пустая - используем локальную MongoDB
            if (string.IsNullOrWhiteSpace(mongoUri))
            {
                mongoUri = "mongodb://localhost:27017";
                Console.WriteLine("[System] STANDOFF_MONGO_URI not set. Using default: mongodb://localhost:27017");
            }

            var dbName = "ProjectLEGA";
            var port = 2222;
            var httpPort = 2223;

            // Проверяем только если URI пустой (уже проверили выше)
            if (string.IsNullOrWhiteSpace(mongoUri))
            {
                Console.WriteLine("[System] No MongoDB URI available. Server cannot start.");
                return;
            }

            Console.WriteLine($"[System] Initializing Standoff TCP Server on port {port}...");
            Console.WriteLine($"[System] Using MongoDB: {mongoUri}");

            try
            {
                var mongo = new MongoService(mongoUri, dbName);
                Console.WriteLine("[System] Connecting to MongoDB...");

                var settings = await mongo.GetSettingsAsync();
                if (string.IsNullOrWhiteSpace(settings.GameVersion))
                {
                    settings.GameVersion = "unknown";
                    await mongo.UpsertSettingsAsync(settings);
                }
                Console.WriteLine($"[System] Current game version in DB: {settings.GameVersion}");

                var server = new TcpServer(port, mongo);
                var cts = new CancellationTokenSource();

                // HTTP сервер для проверки версии
                _ = Task.Run(() => RunHttpServer(mongo, httpPort, cts.Token));

                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                    try { server.Stop(); } catch { }
                    Console.WriteLine("[System] Shutting down...");
                };

                Console.WriteLine("[System] Server is running. Press Ctrl+C to stop.");
                await server.StartAsync(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[FATAL ERROR]");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");

                // Дополнительная диагностика для MongoDB
                if (ex.Message.Contains("MongoDB") || ex.Message.Contains("connection"))
                {
                    Console.WriteLine("\n[MongoDB Troubleshooting]");
                    Console.WriteLine("1. Make sure MongoDB is installed");
                    Console.WriteLine("2. Run MongoDB service: net start MongoDB");
                    Console.WriteLine("3. Or download from: https://www.mongodb.com/try/download/community");
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        static async Task RunHttpServer(MongoService mongo, int port, CancellationToken ct)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port}/");  // + вместо * для совместимости с Windows
            listener.Start();
            Console.WriteLine($"[HTTP] Version check server listening on port {port}");

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var ctx = await listener.GetContextAsync();
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // GET /version?v=0.12.1
                            string clientVersion = ctx.Request.QueryString["v"] ?? "";

                            // Регистрируем версию как отдельный документ в коллекции "version"
                            if (!string.IsNullOrWhiteSpace(clientVersion))
                                await mongo.RegisterVersionAsync(clientVersion);

                            // Проверяем разрешена ли версия
                            bool blocked = !string.IsNullOrWhiteSpace(clientVersion) && !await mongo.IsVersionAllowedAsync(clientVersion);

                            Console.WriteLine($"[HTTP] checkVersion: client={clientVersion} blocked={blocked}");

                            string json = $"{{\"blocked\":{blocked.ToString().ToLower()},\"required\":\"{clientVersion}\"}}";
                            byte[] buf = Encoding.UTF8.GetBytes(json);
                            ctx.Response.ContentType = "application/json";
                            ctx.Response.ContentLength64 = buf.Length;
                            await ctx.Response.OutputStream.WriteAsync(buf, 0, buf.Length);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[HTTP] Error: {ex.Message}");
                        }
                        finally
                        {
                            ctx.Response.Close();
                        }
                    });
                }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    Console.WriteLine($"[HTTP] Listener error: {ex.Message}");
                }
            }

            listener.Stop();
        }
    }
}