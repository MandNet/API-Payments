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

    public class PaymentsController : ControllerBase
    {
        private readonly IRequestInterface _requestInterface;
        private readonly ITransactionInterface _transactionInterface;
        private readonly IPaymentInterface _paymentInterface;

        public PaymentsController(  IRequestInterface requestInteface,
                                    ITransactionInterface transactionInteface,
                                    IPaymentInterface paymentInteface
            )
        {
            _requestInterface = requestInteface;
            _transactionInterface = transactionInteface;
            _paymentInterface = paymentInteface;
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<ActionResult<ResponseDTO<List<TransactionModel>>>> Get()
        {
            var transactions = await _transactionInterface.List(0);
            return Ok(transactions);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<TransactionModel>>> Get([FromRoute] int id)
        {
            var transaction = await _transactionInterface.GetById(id);
            return Ok(transaction);
        }

        [Authorize]
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<ActionResult<ResponseDTO<TransactionModel>>> Post([FromBody] PaymentDTO pay)
        {
            RequestModel request = new RequestModel();
            request.Card = pay.CardNumber;
            request.Value = pay.Value;
            request.Date = DateTime.Now;

            var transactions = await _paymentInterface.ProcessPayment(request);
            return Ok(transactions);
        }
    }
}
