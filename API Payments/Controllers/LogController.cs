using API_Payments.DTO;
using API_Payments.Models;
using API_Payments.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Payments.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogInterface _logInterface;

        public LogController(ILogInterface logInteface)
        {
            _logInterface = logInteface;
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<ActionResult<ResponseDTO<List<LogModel>>>> Get()
        {
            var logs = await _logInterface.List(0);
            return Ok(logs);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<LogModel>>> Get([FromRoute] int id)
        {
            var log = await _logInterface.GetById(id);
            return Ok(log);
        }

    }
}
