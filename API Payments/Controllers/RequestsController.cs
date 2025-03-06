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

    public class RequestsController : ControllerBase
    {
        private readonly IRequestInterface _requestInterface;

        public RequestsController(IRequestInterface requestInteface)
        {
            _requestInterface = requestInteface;
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<ActionResult<ResponseDTO<List<RequestModel>>>> Get()
        {
            var requests = await _requestInterface.List(0);
            return Ok(requests);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<RequestModel>>> Get([FromRoute] int id)
        {
            var request = await _requestInterface.GetById(id);
            return Ok(request);
        }

    }
}
