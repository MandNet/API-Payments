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


    public class PaymentFeesController : ControllerBase
    {
        private readonly IFeeInterface _feeInterface;

        public PaymentFeesController(IFeeInterface feeInteface)
        {
            _feeInterface = feeInteface;
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<ActionResult<ResponseDTO<List<FeeModel>>>> Get()
        {
            var fees = await _feeInterface.List(0);
            return Ok(fees);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<FeeModel>>> Get([FromRoute] int id)
        {
            var fee = await _feeInterface.GetById(id);
            return Ok(fee);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<ActionResult<ResponseDTO<FeeModel>>> Post()
        {
            var fee = await _feeInterface.Generate();
            return Ok(fee);
        }
    }
}
