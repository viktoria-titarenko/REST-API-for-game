using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectController : ControllerBase
    {
        private readonly IConnectService _connectService;

        public ConnectController(IConnectService connectService)
        {
            _connectService = connectService;
        }

        /// <summary>
        /// Полуучение ключа
        /// </summary>
        /// <param name="hello"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getkey")]
        public async Task<ActionResult> ConnectPost([FromBody] Connect connect)
        {
            return Ok((await _connectService.IsViewCorrectAsync(connect, HttpContext.RequestAborted)));
        }
    }
}