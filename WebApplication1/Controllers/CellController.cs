using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CellController : ControllerBase
    {
        private readonly ICellService _cellService;

        public CellController(ICellService cellService)
        {
            _cellService = cellService;
        }

        /// <summary>
        /// Добавление ячейки в базу данных
        /// </summary>
        /// <param name="cCell"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add([FromBody] Cell сCell)
        {
            return Ok(await _cellService.IsKeyCorrectAsync(сCell, HttpContext.RequestAborted));
        }
    }
}