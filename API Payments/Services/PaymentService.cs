using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Enum;
using API_Payments.Models;
using Azure.Core;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Payments.Services
{
    public class PaymentService : IPaymentInterface
    {
        private readonly AppDbContext _context;
        private readonly ICardInterface _cardService;
        private readonly IRequestInterface _requestService;
        private readonly ITransactionInterface _transactionService;
        private readonly IFeeInterface _feeService;
        private readonly ApiSettings _mySettings;


        public PaymentService(AppDbContext context, 
                                ICardInterface cardService,
                                IRequestInterface requestService,
                                ITransactionInterface transactionService,
                                IFeeInterface feeService,
                                IOptions<ApiSettings> mySettings
            )
        {
            _context = context;
            _cardService = cardService;
            _requestService = requestService;
            _transactionService = transactionService;
            _feeService = feeService;
            _mySettings = mySettings.Value;
        }

        public async Task<ResponseDTO<TransactionModel>> Authorize(RequestModel request)
        {
            ResponseDTO<TransactionModel> resp = new ResponseDTO<TransactionModel>();
            resp.Data = new TransactionModel();
            resp.Status = true;

            try
            {
                var card = await _cardService.GetByNumber(request.Card);
                if (!card.Status)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Status = false;
                    return resp;
                }

                if (card.Data.Status != (int)CardStatusEnum.Authorized)
                {
                    resp.Data = null;
                    resp.Message = "Card not Authorized";
                    resp.Status = false;
                    return resp;
                }

                if (!card.Data.Elegible)
                {
                    resp.Data = null;
                    resp.Message = "Card not Elegible";
                    resp.Status = false;
                    return resp;
                }

                if (!card.Data.Active)
                {
                    resp.Data = null;
                    resp.Message = "Card not Active";
                    resp.Status = false;
                    return resp;
                }

                var fee = await _feeService.GetLast();
                if (!fee.Status)
                {
                    fee = new ResponseDTO<FeeModel>();
                    fee.Data.Value = 0;
                }

                if ((card.Data.Balance + card.Data.Limit + fee.Data.Value) > request.Value)
                {
                    resp.Data = null;
                    resp.Message = "Insuficient Balance";
                    resp.Status = false;
                    return resp;
                }

                var transaction = await _transactionService.GetLast(request.Card);
                if (request.Value == transaction.Data.Value)
                {
                    var interval = DateTime.Now - transaction.Data.Date;
                    if (interval.TotalSeconds < (int)_mySettings.fraud.Interval)
                    {
                        await _cardService.UpdateStatus(request.Card, (int)CardStatusEnum.Suspicious);
                        resp.Data = null;
                        resp.Message = "Fraud";
                        resp.Status = false;
                        return resp;
                    }
                }

                resp.Data.Card_Id = card.Data.Id;
                resp.Data.Date = DateTime.Now;
                resp.Data.Value = request.Value;
                resp.Data.Fee_Id = fee.Data.Id;
                resp.Data.Total = request.Value + fee.Data.Value;
                resp.Data.Request_Id = request.Id;
                resp.Data.AuthorizationCode = Guid.NewGuid().ToString();
                resp.Status = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<List<TransactionModel>>> Execute(List<RequestModel> requests)
        {
            ResponseDTO<List<TransactionModel>> resp = new ResponseDTO<List<TransactionModel>>();
            resp.Data = new List<TransactionModel>();

            try
            {
                foreach (var request in requests)
                {
                    var transaction = await Authorize(request);
                    if (!transaction.Status)
                    {
                        request.Status = (int)RequestStatusEnum.Rejected;
                        request.Motive = resp.Message;
                        await _requestService.Update(request);
                    }
                    else
                    {
                        request.Status = (int)RequestStatusEnum.Aproved;
                        await _requestService.Update(request);
                        await _transactionService.Insert(transaction.Data);
                        await _cardService.Debt(request.Card, transaction.Data.Total);
                        resp.Data.Add(transaction.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }

            return resp;
        }

        public async Task<ResponseDTO<List<RequestModel>>> Process()
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            resp.Data = new List<RequestModel>();

            try
            {
                var requests = await _requestService.MarkToBeProcessed();
                if (!requests.Status)
                {
                    resp.Data = null;
                    resp.Message = requests.Message;
                    resp.Status = false;
                    return resp;
                }

                foreach (var request in requests.Data)
                {
                    resp.Data.Add(request);
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }

            return resp;
        }
    }
}
