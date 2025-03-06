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
        [HttpGet()]
        public async Task<ActionResult<ResponseDTO<List<CardModel>>>> Get()
        {
            var cards = await _cardInterface.List(0);
            return Ok(cards);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Get([FromRoute] int id)
        {
            var card = await _cardInterface.GetById(id);
            return Ok(card);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Post([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Insert(card);
            return Ok(cardbd);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPut()]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Put([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Update(card);
            return Ok(cardbd);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpDelete()]
        public async Task<ActionResult<ResponseDTO<CardModel>>> Delete([FromBody] CardModel card)
        {
            var cardbd = await _cardInterface.Delete(card);
            return Ok(cardbd);
        }
    }
}
