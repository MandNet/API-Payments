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
    public class CardManagementController : ControllerBase
    {
        private readonly ICardInterface _cardInterface;

        public CardManagementController(ICardInterface cardInteface)
        {
            _cardInterface = cardInteface;
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("Listar/{num}")]
        public async Task<ActionResult<ResponseDTO<List<CardModel>>>> List(int num = 0)
        {
            var cards = await _cardInterface.List(num);
            return Ok(cards);
        }


        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("Listar")]
        public async Task<ActionResult<ResponseDTO<List<CardModel>>>> List()
        {
            var cards = await _cardInterface.List(0);
            return Ok(cards);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> GetById([FromRoute] int Id)
        {
            var card = await _cardInterface.GetById(Id);
            return Ok(card);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("GetByCard/{number}")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> GetByCard([FromRoute] string number)
        {
            var card = await _cardInterface.GetByNumber(number);
            return Ok(card);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPost("Insert")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Insert([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Insert(card);
            return Ok(cardbd);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPut("Update")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Update([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Update(card);
            return Ok(cardbd);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpDelete("Delete")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Delete([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Delete(card);
            return Ok(cardbd);
        }
    }
}
