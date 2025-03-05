using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface ITransactionInterface
    {
        Task<ResponseDTO<List<TransactionModel>>> List(int num = 0);
        Task<ResponseDTO<List<TransactionModel>>> ListByCard(string cardNumber);
        Task<ResponseDTO<TransactionModel>> Insert(TransactionModel transaction);
        Task<ResponseDTO<TransactionModel>> GetLast(string cardNumber);
        Task<ResponseDTO<TransactionModel>> GetById(int transactionId);
    }
}


