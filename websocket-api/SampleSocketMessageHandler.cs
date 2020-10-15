using client.socket;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace websocket_api
{
    public class SampleSocketMessageHandler : WebSocketHandler
    {
        public SampleSocketMessageHandler(SocketConnectionManager socketConnectionManager) : base(socketConnectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket, SocketMessageConnectionInfo socketMessageConnectionInfo)
        {
            await base.OnConnected(socket, socketMessageConnectionInfo);
        }

        public override async Task<SocketMessage> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            return await Task.Run(() =>
            {
                var socketMessageText = Encoding.UTF8.GetString(buffer, 0, result.Count);

                SocketMessage socketMessage = null;

                if (socketMessageText != null && socketMessageText != string.Empty)
                {
                    socketMessage = socketMessageText.FromJSON<SocketMessage>();
                }

                return socketMessage;
            });
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
        }
    }
}