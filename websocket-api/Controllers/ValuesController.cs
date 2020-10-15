using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using websocket_api.Models;

namespace websocket_api.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly SampleSocketMessageHandler chatMessageHandler;
        public ValuesController(SampleSocketMessageHandler chatMessageHandler)
        {
            this.chatMessageHandler = chatMessageHandler;
        }

        [HttpPost]
        [ActionName("socket/messages")]
        public async Task SendMessage([FromBody] SendSocketMessageRequest request)
        {
            await this.chatMessageHandler.SendMessageAsync(request.SocketId, request.Message);
        }


        [HttpPost]
        [ActionName("socket/close")]
        public async Task RemoveSocket([FromBody] RemoveSocketRequest request)
        {
            await this.chatMessageHandler.RemoveSocket(request.SocketId, request.Message);
        }
    }    
}
