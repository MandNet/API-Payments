using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface ICardInterface
    {
        Task<ResponseDTO<List<CardModel>>> List(int num = 0);
        Task<ResponseDTO<CardModel>> Insert(CardModel card);
        Task<ResponseDTO<CardModel>> Update(CardModel card);
        Task<ResponseDTO<CardModel>> Delete(CardModel card);
        Task<ResponseDTO<CardModel>> GetById(int cardId);
        Task<ResponseDTO<CardModel>> GetByNumber(string number);
        Task<ResponseDTO<CardBalanceDTO>> GetBalance(string number);
        Task<ResponseDTO<CardModel>> Debt(int cardId, decimal value);
        Task<ResponseDTO<CardModel>> Debt(string cardNumber, decimal value);
        Task<ResponseDTO<CardModel>> UpdateBalance(string number, decimal value);
        Task<ResponseDTO<CardModel>> UpdateLimit(string number, decimal value);
        Task<ResponseDTO<CardModel>> UpdateStatus(string number, int status);
    }
}


 