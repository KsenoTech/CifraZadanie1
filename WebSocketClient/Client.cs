using System.Net.WebSockets;
using System.Text;

namespace WebSocketClient.WebSocketClient
{
    public class Client
    {
        private string serverUri;
        private ClientWebSocket webSocket;

        public Client(string serverUri)
        {
            this.serverUri = serverUri;
            webSocket = new ClientWebSocket();
        }

        public async Task Connect()
        {
            while (webSocket.State != WebSocketState.Open)
            {
                await webSocket.ConnectAsync(new Uri(serverUri), CancellationToken.None);
                Console.WriteLine("Connected to the server.");
                await ReceiveMessages();
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Server closed the connection.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by server", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Message from server: {message}");

            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                Console.WriteLine("Cannot send message. Not connected to server.");
            }
        }
    }
}
