using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface IPaymentInterface
    {
        Task<ResponseDTO<List<RequestModel>>> Process();
        Task<ResponseDTO<List<TransactionModel>>> Execute(List<RequestModel> requests);
        Task<ResponseDTO<List<TransactionModel>>> ProcessPayment(RequestModel request);

    }
}
