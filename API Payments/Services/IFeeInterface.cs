using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface IFeeInterface
    {
        Task<ResponseDTO<List<FeeModel>>> List(int num = 0);
        Task<ResponseDTO<FeeModel>> Insert(FeeModel fee);
        Task<ResponseDTO<FeeModel>> GetLast();
        Task<ResponseDTO<FeeModel>> Generate();
        Task<ResponseDTO<FeeModel>> GetById(int feeId);
    }
}


