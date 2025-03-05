using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface IPaymentInterface
    {
        Task<ResponseDTO<List<RequestModel>>> Process();
        Task<ResponseDTO<TransactionModel>> Authorize(RequestModel request);
        Task<ResponseDTO<List<TransactionModel>>> Execute(List<RequestModel> requests);
    }
}
