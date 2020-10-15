using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace client.socket
{
    public abstract class WebSocketHandler
    {
        protected SocketConnectionManager SocketConnectionManager { get; set; }

        public WebSocketHandler(SocketConnectionManager webSocketConnectionManager)
        {
            SocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket, SocketMessageConnectionInfo socketMessageConnectionInfo)
        {
            await Task.Run(() => { SocketConnectionManager.AddSocket(socket, socketMessageConnectionInfo); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await SocketConnectionManager.RemoveSocket(SocketConnectionManager.GetId(socket), "client disconnected");
        }


        public virtual async Task RemoveSocket(string id,string message)
        {
            await SocketConnectionManager.RemoveSocket(id, message);
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(SocketConnectionManager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in SocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        //TODO - decide if exposing the message string is better than exposing the result and buffer
        public abstract Task<SocketMessage> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }

    [Serializable]
    public class SocketMessage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }


    [Serializable]
    public class SocketMessageConnectionInfo
    {
        [JsonPropertyName("user")]
        public string User { get; set; }
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        public string Id
        {
            get
            {
                return $"{this.User}|{this.Ip}";
            }
        }
    }
}