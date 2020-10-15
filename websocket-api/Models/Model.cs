using System;
using System.Text.Json.Serialization;

namespace websocket_api.Models
{
    [Serializable]
    public class BaseSocketRequest
    {

        [JsonPropertyName("id")]
        public string SocketId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }


    [Serializable]
    public class SendSocketMessageRequest : BaseSocketRequest
    {

    }

    [Serializable]
    public class RemoveSocketRequest : BaseSocketRequest
    {

    }
}
