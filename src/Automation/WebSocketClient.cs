using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace ExampleMod.Automation;

/// <summary>
/// WebSocket client that connects to the MCP server.
/// Runs a background receive loop and dispatches incoming commands.
/// </summary>
public sealed class WebSocketClient
{
    public static WebSocketClient Instance { get; } = new();

    private const string ServerUri = "ws://localhost:19280";
    private const int ReconnectDelayMs = 3000;
    private const int ReceiveBufferSize = 64 * 1024; // 64 KB

    private ClientWebSocket? _ws;
    private CancellationTokenSource? _cts;
    private volatile bool _running;

    public bool IsConnected => _ws?.State == WebSocketState.Open;

    private WebSocketClient() { }

    /// <summary>
    /// Start connection loop on a background thread.
    /// Safe to call multiple times — only the first call takes effect.
    /// </summary>
    public void Initialize()
    {
        if (_running) return;
        _running = true;

        _cts = new CancellationTokenSource();
        var thread = new Thread(() => ConnectionLoop(_cts.Token))
        {
            IsBackground = true,
            Name = "ExampleMod-WS"
        };
        thread.Start();
        Log.Info("[ExampleMod] WebSocket client initialized (background thread started).");
    }

    /// <summary>
    /// Gracefully shut down the client.
    /// </summary>
    public void Shutdown()
    {
        _running = false;
        _cts?.Cancel();
        try
        {
            _ws?.Abort();
        }
        catch { /* ignore */ }

        Log.Info("[ExampleMod] WebSocket client shut down.");
    }

    /// <summary>
    /// Send a UTF-8 JSON message to the MCP server.
    /// Returns false if not connected.
    /// </summary>
    public bool Send(string json)
    {
        if (_ws?.State != WebSocketState.Open) return false;

        try
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None)
               .GetAwaiter().GetResult();
            return true;
        }
        catch (Exception ex)
        {
            Log.Info($"[ExampleMod] WS send error: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Send a UTF-8 JSON message to the MCP server (async version).
    /// </summary>
    public async Task<bool> SendAsync(string json)
    {
        if (_ws?.State != WebSocketState.Open) return false;

        try
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            return true;
        }
        catch (Exception ex)
        {
            Log.Info($"[ExampleMod] WS send error: {ex.Message}");
            return false;
        }
    }

    // -----------------------------------------------------------------------
    // Background connection loop
    // -----------------------------------------------------------------------
    private void ConnectionLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                ConnectAndReceive(ct).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log.Info($"[ExampleMod] WS connection error: {ex.Message}");
            }

            if (ct.IsCancellationRequested) break;

            Log.Info($"[ExampleMod] WS reconnecting in {ReconnectDelayMs}ms...");
            Thread.Sleep(ReconnectDelayMs);
        }
    }

    private async Task ConnectAndReceive(CancellationToken ct)
    {
        _ws = new ClientWebSocket();
        await _ws.ConnectAsync(new Uri(ServerUri), ct);
        Log.Info("[ExampleMod] WebSocket connected to MCP server.");

        var buffer = new byte[ReceiveBufferSize];
        var sb = new StringBuilder();

        while (_ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
        {
            sb.Clear();
            WebSocketReceiveResult result;

            do
            {
                result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Log.Info("[ExampleMod] WS server requested close.");
                    return;
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
            } while (!result.EndOfMessage);

            var message = sb.ToString();
            HandleIncomingMessage(message);
        }
    }

    /// <summary>
    /// Process an incoming command message from the MCP server.
    /// Dispatches execution to the main (Godot) thread.
    /// </summary>
    private void HandleIncomingMessage(string json)
    {
        Log.Info($"[ExampleMod] WS received: {json}");

        // Dispatch to main thread via Godot's CallDeferred
        Callable.From(() =>
        {
            try
            {
                CommandExecutor.HandleCommand(json);
            }
            catch (Exception ex)
            {
                Log.Info($"[ExampleMod] Command execution error: {ex.Message}");
            }
        }).CallDeferred();
    }
}
